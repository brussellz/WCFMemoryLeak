using System;
using System.ServiceModel;
using System.Threading;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            int userCount = 0;
            ShowHeader();
            userCount = AskHowManyUsers();
            while (userCount > 0)
            {
                PoundTheService(userCount);

                Console.WriteLine("--------------------------------------------");
                userCount = 0;
                ShowHeader();
                userCount = AskHowManyUsers();
            }
        }

        static void ShowHeader()
        {

            Console.WriteLine("==============================================");
            Console.WriteLine("Ready to stress test the service.");
            Console.WriteLine("To exit you can enter 0 for # of users.");
            Console.WriteLine();
        }

        static int AskHowManyUsers()
        {
            Console.WriteLine("How many simulated users? (1-64)");
            int userCount;
            if (int.TryParse(Console.ReadLine(), out userCount) == false)
            {
                return 0;
            }

            return userCount;
        }

        static ManualResetEvent[] threadEvents;
        static ServiceCallWorkItem[] threadWorkers;
        static void PoundTheService(int userCount)
        {
            threadEvents = new ManualResetEvent[userCount];
            threadWorkers = new ServiceCallWorkItem[userCount];
            Console.Write("Calling Service with {0} simulated concurrent users...", userCount);
            for (int i = 0; i < userCount; i++)
            {
                threadEvents[i] = new ManualResetEvent(false);
                threadWorkers[i] = new ServiceCallWorkItem(threadEvents[i]);
                ThreadPool.QueueUserWorkItem(threadWorkers[i].ThreadPoolCallback, i);
            }

            Console.Write("[DONE]");
            Console.WriteLine("");
            Console.Write("Waiting for completion...");
            WaitHandle.WaitAll(threadEvents);
            Console.Write("[Done]");
            Console.WriteLine("");
        }
    }

    public sealed class ServiceCallWorkItem
    {
        private ManualResetEvent _doneEvent;

        public ServiceCallWorkItem(ManualResetEvent doneEvent)
        {
            _doneEvent = doneEvent;
        }

        public void ThreadPoolCallback(object context)
        {
            try
            {
                var userIdx = (int)context;
                for (var i = 1; i < 2; i++)
                {
                    ExecuteServiceCall(new DataServiceReference.DataServiceClient(), x =>
                    {
                        var country = (userIdx % 2 == 0) ? "United States" : "Canada";

                        return x.GetCustomersByCountry(country);
                    });
                }
            }
            catch { }
            finally
            {
                _doneEvent.Set();
            }
        }

        private static TReturn ExecuteServiceCall<TService, TReturn>(TService serviceProxy, Func<TService, TReturn> func)
           where TReturn : class
        {
            var isCommunicationObject = serviceProxy as ICommunicationObject != null;

            TReturn result = null;

            try
            {
                result = func(serviceProxy);

                if (isCommunicationObject)
                    ((ICommunicationObject)serviceProxy).Close();
            }
            catch (Exception)
            {
                if (isCommunicationObject)
                {
                    ((ICommunicationObject)serviceProxy).Abort();
                    ((ICommunicationObject)serviceProxy).Close();
                }
            }
            finally
            {
                var dispoableService = serviceProxy as IDisposable;
                if (dispoableService != null)
                {
                    dispoableService.Dispose();
                }
            }

            return result;
        }
    }
}

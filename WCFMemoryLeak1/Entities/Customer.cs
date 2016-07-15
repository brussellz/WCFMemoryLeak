using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WCFMemoryLeak1.Entities
{
    [DataContract]
    public class Customer
    {
        public Customer()
        {
            Addresses = new List<Address>();
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Suffix { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public List<Address> Addresses { get; set; }
    }
}
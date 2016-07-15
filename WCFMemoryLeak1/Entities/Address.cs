using System.Runtime.Serialization;

namespace WCFMemoryLeak1.Entities
{
    [DataContract]
    public class Address
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Line1 { get; set; }

        [DataMember]
        public string Line2 { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string Zip { get; set; }

        [DataMember]
        public string Country { get; set; }
    }
}
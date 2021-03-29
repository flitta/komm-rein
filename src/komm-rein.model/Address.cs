using System.ComponentModel.DataAnnotations;

namespace komm_rein.model
{
    public  class Address : ContextItem
    {
        public string ContactName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string ContactEmail{ get; set; }

        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }

        public string AdditionalInfo { get; set; }

        public string Street_1 { get; set; }

        public string Street_2 { get; set; }

        public string Street_3 { get; set; }

        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
    }
}
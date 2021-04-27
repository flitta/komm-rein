using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.ViewModel
{
    public class CreateFacilityViewModel
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Street { get; set; }

        public string Street_2 { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]

        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email{ get; set; }

    }
}

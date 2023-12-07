using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Jiggys_Interface.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }


        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }


        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Licence Number")]
        public int LicenceNumber { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }


        [Display(Name = "Parish")]
        public int ParishId { get; set; }
        [ForeignKey("ParishId")]
        public virtual Parish? Parish { get; set; }

    }
}

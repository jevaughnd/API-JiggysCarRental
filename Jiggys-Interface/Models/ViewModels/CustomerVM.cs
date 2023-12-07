using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Jiggys_Interface.Models.ViewModels
{
    public class CustomerVM
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
       

        //Select value from DDL
        public List<SelectListItem>? ParishList { get; set; }

        //Selected DDL value
        public int SelectedParishId { get; set; }


    }
}

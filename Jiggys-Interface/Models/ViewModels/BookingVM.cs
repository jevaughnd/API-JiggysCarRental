using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Jiggys_Interface.Models.ViewModels
{
    public class BookingVM
    {

        //Models
        public Customer Customer { get; set; }
        public Vehicle Vehicle { get; set; }
        public Booking Booking { get; set; }

        public BookingAddOn bookingAddOn { get; set; }
        public AddOn AddOn { get; set; } 


        //List
        public IEnumerable<SelectListItem>? CustomerList { get; set; }
        public IEnumerable<SelectListItem>? VehicleList { get; set; }
        public IEnumerable<SelectListItem>? AddOnList { get; set; } 


        //User Selected DDL value
        public int SelectedCustomerId { get; set; }
        public int SelectedVehicleId { get; set;}
        public int SelectedAddOnId { get; set; }




        //use the Create, BookingVM  to insert values

        //for editing form
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string PickUpLocation { get; set; }
        public string DropOffLocation { get; set; }

        public int TotalPrice { get; set; } 
        public bool isPaid { get; set; }


        //BookingAddon
        public int BookingId { get; set; }
        public int AddOnnId { get; set; }


    }
}

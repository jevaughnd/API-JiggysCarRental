using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Jiggys_Interface.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        public int VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle? Vehicle { get; set; }


        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }


        [Display(Name = "Pick Up Location")]
        public string PickUpLocation { get; set; }


        [Display(Name = "Drop Off Location")]
        public string DropOffLocation { get; set; }


        [Display(Name = "Total Price")]
        public int TotalPrice { get; set; } 


        [Display(Name = "Is Paid ?")]
        public bool isPaid { get; set; }

    }
}

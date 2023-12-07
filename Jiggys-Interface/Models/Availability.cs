using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Jiggys_Interface.Models
{
    public class Availability
    {
        public int Id { get; set; }


        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle? Vehicle { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Availability Date ")]
        public DateTime AvailabilityDate { get; set; }


        [Display(Name = "Available ?")]
        public bool IsAvailable { get; set; }
    }
}

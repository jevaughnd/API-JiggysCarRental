using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Jiggys_Interface.Models.ViewModels
{
    public class AvailabilityVM
    {
        public int Id { get; set; }

        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }
        public List<SelectListItem>? VehicleList { get; set; }
        public int SelectedVehicleId { get; set; }


        [Display(Name = "Available Date")]
        public DateTime AvailabilityDate { get; set; }


        [Display(Name = "Is Available ?")]
        public bool IsAvailable { get; set; }
    }
}

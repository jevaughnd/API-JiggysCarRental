using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Jiggys_Interface.Models.ViewModels
{
    public class VehicleVM
    {
        public int Id { get; set; }


        [Display(Name = "Vehicle Category")]
        public int VehicleCategoryId { get; set; }

        //Select List Values
        public List<SelectListItem>? VehicleCategoryList { get; set; }
        public int SelectedVehicleCategoryId { get; set; }


        [Display(Name = "Vehicle Name")]
        public string VehicleName { get; set; }


        [Display(Name = "Vehicle Description")]
        public string VehicleDescription { get; set; }


        [Display(Name = "Licence Plate")]
        public string LicencePlateData { get; set; }



        //3 Images
        [Display(Name = "Front Image")]
        public IFormFile? FrontImgFile { get; set; } 


        [Display(Name = "Side Image")]
        public IFormFile? SideImgFile { get; set; }


        [Display(Name = "Interior Image")]
        public IFormFile? InteriorImgFile { get; set; } 




        //6 Features
        [Display(Name = "Number Of Passengers")]
        public int NumberOf_Passengers { get; set; }

        [Display(Name = "Number Of Large Bags")]
        public int NumberOf_LargeBags { get; set; }



        [Display(Name = "Has Air Conditioner")]
        public bool Has_AirConditioner { get; set; }


        [Display(Name = "Has Radio & CD Player")]
        public bool Has_Radio_CDPlayer { get; set; }


        [Display(Name = "Has Power Steering & Windows")]
        public bool Has_PowerSteering_Windows { get; set; }


        [Display(Name = "Has Automatic Transmission")]
        public bool Has_AutomaticTransmission { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Jiggys_Interface.Models
{
    public class Vehicle
    {
        public int Id { get; set; }


        [Display(Name = "Category")]
        public int VehicleCategoryId { get; set; }
        [ForeignKey("VehicleCategoryId")]
        public virtual VehicleCategory? VehicleCategory { get; set; }


        [Display(Name = "Vehicle Name")]
        public string VehicleName { get; set; }


        [Display(Name = "Vehicle Description")]
        public string VehicleDescription { get; set; }


        [Display(Name = "Plate")]
        public string LicencePlateData { get; set; }



        //3 Images
        [Display(Name = "Front Image")]
        public string? FrontImgFilePath { get; set; } = String.Empty;


        [Display(Name = "Side Image")]
        public string? SideImgFilePath { get; set; } = String.Empty;


        [Display(Name = "Interior Image")]
        public string? InteriorImgFilePath { get; set; } = String.Empty;




        //6 Features

        [Display(Name = "Number of Passengers")]
        public int NumberOf_Passengers { get; set; }


        [Display(Name = "Number of Bags ")]
        public int NumberOf_LargeBags { get; set; }



        [Display(Name = "Has Air Conditioner")]
        public bool Has_AirConditioner { get; set; }


        [Display(Name = "Has Radio & CD Player")]
        public bool Has_Radio_CDPlayer { get; set; }


        [Display(Name = "Has Power Steering")]
        public bool Has_PowerSteering_Windows { get; set; }


        [Display(Name = "Has Automatic Transmission")]
        public bool Has_AutomaticTransmission { get; set; }



    }
}

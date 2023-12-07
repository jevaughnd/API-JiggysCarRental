using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Jiggys_Interface.Models
{
    public class VehicleCreateDTO
    {
        public int VehicleCategoryId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleDescription { get; set; }
        public string LicencePlateData { get; set; }


        //3 Images
        public IFormFile? FrontImgFile { get; set; }
        public IFormFile? SideImgFile { get; set; }
        public IFormFile? InteriorImgFile { get; set; }


        //6 Features
        public int NumberOf_Passengers { get; set; }
        public int NumberOf_LargeBags { get; set; }
        public bool Has_AirConditioner { get; set; }
        public bool Has_Radio_CDPlayer { get; set; }
        public bool Has_PowerSteering_Windows { get; set; }
        public bool Has_AutomaticTransmission { get; set; }
    }
}

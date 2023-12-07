using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_JiggysCarRental.MODELS
{
    public class Vehicle
    {
        [Key]

        public int Id { get; set; }

        public int VehicleCategoryId { get; set; }
        [ForeignKey("VehicleCategoryId")]
        public virtual VehicleCategory? VehicleCategory { get; set; }



        [Column(TypeName = "varchar(50)")]
        public string VehicleName { get; set; }




        [Column(TypeName = "varchar(200)")]
        public string VehicleDescription { get; set; }


        [Column(TypeName = "varchar(10)")]
        public string LicencePlateData { get; set; }



        //3 Images
        [Column(TypeName = "varchar(500)")]
        public string? FrontImgFilePath { get; set; } = String.Empty;


        [Column(TypeName = "varchar(500)")]
        public string? SideImgFilePath { get; set; } = String.Empty;


        [Column(TypeName = "varchar(500)")]
        public string? InteriorImgFilePath { get; set; } = String.Empty;




        //6 Features
        public int NumberOf_Passengers { get; set; }
        public int NumberOf_LargeBags { get; set; }

        public bool Has_AirConditioner { get; set; }
        public bool Has_Radio_CDPlayer { get; set; }
        public bool Has_PowerSteering_Windows { get; set; }
        public bool Has_AutomaticTransmission { get; set; }








    }
}

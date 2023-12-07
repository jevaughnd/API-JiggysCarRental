using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_JiggysCarRental.MODELS
{
    public class Availability
    {
        [Key]

        public int Id { get; set; }


        public int VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle? Vehicle { get; set; }


        public DateTime AvailabilityDate { get; set; }

        public bool IsAvailable { get; set; }



    }
}

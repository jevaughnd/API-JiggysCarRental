using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_JiggysCarRental.MODELS
{
    public class Booking
    {
        [Key]
        public int Id { get; set; } 

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }



        public int VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle? Vehicle { get;set; }



        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string PickUpLocation { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string DropOffLocation { get;set; }


        public int TotalPrice { get; set; } //calculated or inputed
        public bool isPaid { get; set; }









    }
}

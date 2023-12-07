using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_JiggysCarRental.MODELS
{
    public class BookingAddOn
    {
        [Key]

        public int Id { get; set; }


        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public virtual Booking? Booking { get; set; }



        public int AddOnnId { get; set; }
        [ForeignKey("AddOnnId")]
        public virtual AddOn? AddOnn { get; set; } //gets price





        //This Model can be linked to the Booking controller using BookingVM,
        //follow example in renal car app, for Customers and Address, if want to

        //When VM is used, Booking form can be used to Add the AddOn without using an extra controller.


    }
}

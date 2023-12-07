using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_JiggysCarRental.MODELS
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FullName { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string EmailAddress { get; set; }


        [Column(TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; }


        [Column(TypeName = "varchar(10)")]
        public int LicenceNumber { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string Street { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string Town { get; set; }


        public int ParishId { get; set; }
        [ForeignKey("ParishId")]
        public virtual Parish? Parish { get; set; }



    }
}

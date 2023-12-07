using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_JiggysCarRental.MODELS
{
    public class VehicleCategory
    {
        [Key]

        public int Id { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string CategoryName { get; set; }
    }
}

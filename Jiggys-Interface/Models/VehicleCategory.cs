using System.ComponentModel.DataAnnotations.Schema;

namespace Jiggys_Interface.Models
{
    public class VehicleCategory
    {
        public int Id { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string CategoryName { get; set; }
    }
}

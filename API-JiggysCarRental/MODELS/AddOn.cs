using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_JiggysCarRental.MODELS
{
    public class AddOn
    {
        [Key]

        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string AddOnName { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string AddOnDescription { get; set;}


        public int AddOnPrice { get; set;}




    }
}

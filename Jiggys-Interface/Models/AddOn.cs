using System.ComponentModel.DataAnnotations.Schema;

namespace Jiggys_Interface.Models
{
    public class AddOn
    {
        public int Id { get; set; }


        public string AddOnName { get; set; }

        public string AddOnDescription { get; set; }

        public int AddOnPrice { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace ItemManagementLib.Models
{
    public class Item
    {
        public Item()
        {

        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Name { get; set; } = null!;

    }
}
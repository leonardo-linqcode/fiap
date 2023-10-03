using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeekBurger.Products.Contract.Model
{
    public class Item
    {
        [Key]
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

    }
}
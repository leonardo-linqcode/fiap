using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeekBurger.Products.Contract.Model
{
    public class Ingredient
    {
        [Key]
        public Guid IngredientId { get; set; }
        public string Name { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

    }
}
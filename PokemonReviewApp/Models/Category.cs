using System.Reflection.Metadata.Ecma335;

namespace PokemonReviewApp.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}
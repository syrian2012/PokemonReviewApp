using System.Reflection.Metadata.Ecma335;

namespace PokemonReviewApp.Models
{
    public class Review
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Reviewer Reviewer { get; set; }
        public Pokemon Pokemon { get; set; }
    }
}

namespace PokemonReviewApp.Models
{
    public class Reviewer
    {
        public int ID { get; set; }
        public string FisrtName { get; set; }
        public string lastName { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}

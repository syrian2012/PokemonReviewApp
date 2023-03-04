namespace PokemonReviewApp.Models
{
    public class PokemonOwner
    {
        public int PokemonID { get; set; }
        public int OwnerID { get; set; }
        public Pokemon Pokemon { get; set; }
        public Owner Owner { get; set; }
    }
}

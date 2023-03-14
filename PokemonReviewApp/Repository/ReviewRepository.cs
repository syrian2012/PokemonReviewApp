using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public Review GetReview(int reviewID)
        {
            return _context.Reviews.Where(r => r.ID == reviewID).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.OrderBy(r => r.ID).ToList();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokeID)
        {
            return _context.Reviews.Where(r => r.Pokemon.ID == pokeID).ToList();
        }

        public bool ReviewExists(int reviewID)
        {
           return _context.Reviews.Any(r => r.ID == reviewID);
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}

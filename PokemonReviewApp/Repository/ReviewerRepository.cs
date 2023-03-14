using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerID)
        {
            return _context.Reviewers.Where(r => r.ID == reviewerID).Include(e => e.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.OrderBy(r => r.ID).ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerID)
        {
            return _context.Reviews.Where(r => r.Reviewer.ID == reviewerID).ToList();
        }

        public bool ReviewerExists(int reviewerID)
        {
            return _context.Reviewers.Any(r => r.ID == reviewerID);
        }

        public bool Save()
        {
            var saved =_context.SaveChanges();
            return saved > 0 ? true: false;
        }
    }
}

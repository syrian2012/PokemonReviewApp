using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerID);
        ICollection<Review> GetReviewsByReviewer(int reviewerID);
        bool ReviewerExists(int reviewerID);
    }
}

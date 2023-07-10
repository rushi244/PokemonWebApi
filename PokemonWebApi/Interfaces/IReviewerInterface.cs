using PokemonWebApi.Models;

namespace PokemonWebApi.Interfaces
{
    public interface IReviewerInterface
    {
        ICollection<Reviewer>GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewByReviewer(int reviewerId);
        bool IsReviewerExists(int reviewerId);
        bool CreateReviewer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
        bool Save();

    }
}

using Microsoft.EntityFrameworkCore;
using PokemonWebApi.Data;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Repository
{
    public class ReviewerRepository : IReviewerInterface
    {
        private readonly ApplicationDbContext _context;
        public ReviewerRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();
        }

        public ICollection<Review> GetReviewByReviewer(int reviewerId)
        {
         return _context.Reviews.Where(r=>r.Reviewer.Id== reviewerId).ToList();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _context.Reviewers.Where(r => r.Id == reviewerId).Include(e => e.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.ToList();
        }

        public bool IsReviewerExists(int reviewerId)
        {
            return _context.Reviewers.Any(r => r.Id == reviewerId); 
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }
    }
}

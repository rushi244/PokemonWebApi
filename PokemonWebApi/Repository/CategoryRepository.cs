using PokemonWebApi.Data;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Repository
{
    public class CategoryRepository : ICategoryInterface
    {
   
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateCategory(Category category)
        {
            //Change Tracker
            //add,updating,modifying
            _context.Add(category);
           return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
           return _context.Categories.OrderBy(c => c.Id).ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();

        }

        public ICollection<Pokemon> GetPokemonsByCategory(int CategoryId)
        {
            return _context.PokemonCategories.Where(c => c.CategoryId == CategoryId).Select(p => p.Pokemon).ToList();
        }

        public bool IsCategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool Save()
        {
            var saved=_context.SaveChanges();
            return saved > 0 ? true: false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}

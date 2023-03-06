using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private DataContext _context;
        public CategoryRepository(DataContext conext)
        {
            _context = conext;
        }
        public bool CategoriesExists(int id)
        {
            return _context.Categories.Any(c => c.ID == id);
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.OrderBy(c => c.ID).ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.ID == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
        {
            return _context.PokemonCategories.Where(e => e.CategoryID == categoryId).Select(c => c.Pokemon).ToList();
        }
    }
}

using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p => p.ID).ToList();
        }

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.Where(p => p.ID == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemons.Where(p => p.Name == name).FirstOrDefault();
        }

        public bool PokemonExists(int id)
        {
            return _context.Pokemons.Any(p => p.ID == id);
        }

        public bool CreatePokemon(int ownerID, int categoryID, Pokemon pokemon)
        {
            var pokemonOwner = _context.Owners.Where(o => o.ID == ownerID).FirstOrDefault();
            var pokemonOwnerEntity = new PokemonOwner()
            {
                Owner = pokemonOwner,
                Pokemon= pokemon,
            };
            var pokemonCategory = _context.Categories.Where(c => c.ID == categoryID).FirstOrDefault();
            var pokemonCategoryEntity = new PokemonCategory() 
            {
                Pokemon = pokemon,
                Category = pokemonCategory
            };
            _context.Add(pokemonCategoryEntity);
            _context.Add(pokemonOwnerEntity);
            _context.Add(pokemon);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true: false;
        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}

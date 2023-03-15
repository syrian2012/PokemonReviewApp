using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        bool PokemonExists(int id);
        bool CreatePokemon(int ownerID,int categoryID,Pokemon pokemon);
        bool UpdatePokemon(Pokemon pokemon);
        bool Save();
    }
}

using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepositry
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int Ownerid);
        ICollection<Owner> GetOwnersFromCountry(int Countryid);
        bool CountryExist(int id);
        bool CreateCountry(Country country);
        bool Save();
    }
}

﻿using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepositry : ICountryRepositry
    {
        private protected DataContext _context;
        public CountryRepositry(DataContext context)
        {
            _context = context;
        }

        public bool CountryExist(int id)
        {
            return _context.Countries.Any(c => c.ID == id);
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();
        }

        public bool DeleteCountry(int id)
        {
            var country = _context.Countries.Where(c => c.ID == id).FirstOrDefault();
            _context.Remove(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(c => c.ID).ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(c => c.ID == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int Ownerid)
        {
            return _context.Owners.Where(o => o.ID == Ownerid).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int Countryid)
        {
            return _context.Owners.Where(o => o.Country.ID == Countryid).ToList();
        }

        public bool Save()
        {
            var Saved = _context.SaveChanges();
            return Saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }
    }
}

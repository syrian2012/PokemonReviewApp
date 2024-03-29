﻿using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<CreateCountryDto,Country>();
            CreateMap<CreateOwnerDto,Owner>();
            CreateMap<CreatePokemonDto, Pokemon>();
            CreateMap<CreateReviewDto,Review>();
            CreateMap<CreateReviewerDto,Reviewer>();
            CreateMap<CategoryDto, Category>();
        }
    }
}

﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories()).ToList();
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("{categoryID}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryID)
        {
            if (!_categoryRepository.CategoriesExists(categoryID))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryID));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpGet("pokemon/{categoryID}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryID(int categoryID)
        {
            if (!_categoryRepository.CategoriesExists(categoryID))
                return NotFound();

            var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(categoryID));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpPost("{Name}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory(string Name)
        {
            if (Name.IsNullOrEmpty())
                return BadRequest(ModelState);

            var categoryExists = _categoryRepository.GetCategories().Where(c => c.Name.Trim().ToLower() == Name.Trim().ToLower());

            if (categoryExists.Any())
            {
                ModelState.AddModelError("", "Category Already Exists");
                return StatusCode(422,ModelState);
            }
            
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var my_category = new Category();
            my_category.Name = Name.Trim();
            if (!_categoryRepository.CreateCategory(my_category))
            {
                ModelState.AddModelError("", "Some Thing Went Wrong While Saving Your Entry");
                return StatusCode(500, ModelState);
            }
            return Ok("Category Created Successfully");
        }

        [HttpPut("{CategoryID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int CategoryID, [FromBody] CategoryDto updateCategory) 
        {
            if (updateCategory == null)
                return BadRequest(ModelState);

            if (CategoryID != updateCategory.ID)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoriesExists(CategoryID))
                return NotFound();

            var categoryMap = _mapper.Map<Category>(updateCategory);

            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Some Thing Went Wrong While Updating Your Entry");
                return StatusCode(500,ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{CategoryID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int CategoryID)
        {
            if (!_categoryRepository.CategoriesExists(CategoryID))
                return NotFound();

            var category = _categoryRepository.GetCategory(CategoryID);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("", "Somthing Went Wrong While Deleting");
            }
            return Ok("Deleted!!");
        }
    }
}

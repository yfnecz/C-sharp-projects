using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using Recipes_api.Models;
using Microsoft.EntityFrameworkCore;


namespace Recipes_api.Controllers
{
    [ApiController]
    // api will be available at this route
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private RecipeContext _recipeContext;
        
        public RecipesController(RecipeContext context){
            _recipeContext = context;
        }

         /// <summary>
         /// Post a new recipe
         /// </summary>
         [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipes(Recipe recipe)
        {

            _recipeContext.Recipes.Add(recipe);
            await _recipeContext.SaveChangesAsync();
            string url = "/api/recipes/" + recipe.Id;
            return CreatedAtAction(null, null, recipe, "The resource has been created successfully at "+ url);
         
        }     

          [HttpGet("/recipes/searchByIngredients/{ingredient}")]
        public ActionResult<IEnumerable<Recipe>> GetFilterRecipesByIngredients(string ingredient)
        {
            var recipes = _recipeContext.Recipes
            .AsEnumerable()
            .Where(
                recipe =>
                {
                    var ingredientsList = string.IsNullOrEmpty(recipe.IngredientsJson) ? new List<Ingredient>() : JsonSerializer.Deserialize<List<Ingredient>>(recipe.IngredientsJson, new JsonSerializerOptions());
                    return ingredientsList != null && ingredientsList.Any(ingred => ingred.name == ingredient);
                }
                )
            .ToList();
            if (!recipes.Any()) { return NotFound(); }
            return Ok(recipes);
        }

        [HttpGet("/recipes/searchByCuisine/{cuisine}")]
        public ActionResult<IEnumerable<Recipe>> GetFilterRecipesByCuisine(string cuisine)
        {
            var recipes = _recipeContext.Recipes
            .Where(s => s.Cuisine != null && s.Cuisine.Contains(cuisine))
            .ToList();
            if (!recipes.Any()) { return NotFound(); }
            return Ok(recipes);
        }

        [HttpGet("/recipes/searchByTitle/{title}")]
        public ActionResult<IEnumerable<Recipe>> GetFilterRecipesByTitle(string title)
        {
            var recipes = _recipeContext.Recipes
            .Where(s => s.Title != null && s.Title.Contains(title))
            .ToList();
            if (!recipes.Any()) { return NotFound(); }
            return Ok(recipes);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Recipe>> GetRecipes()
        {
            var recipes = _recipeContext.Recipes.ToList();
            return Ok(recipes);
        }

        [HttpPut("{id}")]
    public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
    {
        if (id != recipe.Id)
        {
            return StatusCode(400, "The id parameter should be equal to the id of the new data of the object to be updated");
        }

        // Check if the recipe exists in the database
        var existingRecipe = await _recipeContext.Recipes.FindAsync(id);
        if (existingRecipe == null)
        {
            return StatusCode(404, "The recipe you want to update is not found in the database");
        }

        // Update the existing recipe with new values
        _recipeContext.Entry(existingRecipe).CurrentValues.SetValues(recipe);

        // Save changes to the database
        await _recipeContext.SaveChangesAsync();
        return Ok("Recipe has been updated successfully");
    }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
           var recipe = await _recipeContext.Recipes.FindAsync(id);
           if (recipe == null)
           {
               return NotFound();
           }

           _recipeContext.Recipes.Remove(recipe);
           await _recipeContext.SaveChangesAsync();
           return Ok("Recipe has been deleted successfully");
        }


    }
}
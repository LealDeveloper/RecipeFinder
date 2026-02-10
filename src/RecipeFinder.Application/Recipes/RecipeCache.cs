using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinder.Application.Recipes
{
    public class RecipeCache
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public List<string> Ingredients { get; set; } = new();
    }
}

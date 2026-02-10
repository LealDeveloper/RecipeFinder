using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinder.Application.Recipes.DeleteRecipe
{
    public class DeleteRecipeCommand
    {
        public Guid Id { get; }
        public DeleteRecipeCommand(Guid id)
        {
            Id = id;
        }
    }
}

using FluentValidation;
using RecipeFinder.API.DTOs.Request.Recipe;

namespace RecipeFinder.API.Validators.Recipe
{
    public class SearchRecipesRequestValidator : AbstractValidator<SearchRecipeRequest>
    {
        public SearchRecipesRequestValidator()
        {
            RuleFor(x => x.Ingredients)
                .NotEmpty()
                .WithMessage("You must provide at least one ingredient.")
                .Must(list => list.All(i => !string.IsNullOrWhiteSpace(i)))
                .WithMessage("Ingredients cannot be empty.");
        }
    }
}

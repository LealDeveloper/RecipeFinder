using FluentValidation;
using RecipeFinder.API.DTOs;

namespace RecipeFinder.API.Validators
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

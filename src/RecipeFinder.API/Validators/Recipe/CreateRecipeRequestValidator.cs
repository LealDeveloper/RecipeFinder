using FluentValidation;
using RecipeFinder.API.DTOs.Request.Recipe;

namespace RecipeFinder.API.Validators.Recipe
{
    public class CreateRecipeRequestValidator : AbstractValidator<CreateRecipeRequest>
    {
        public CreateRecipeRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Recipe name cannot be empty.");

            RuleFor(x => x.Ingredients)
                .NotEmpty()
                .WithMessage("Recipe must have at least one ingredient.")
                .Must(list => list.All(i => !string.IsNullOrWhiteSpace(i)))
                .WithMessage("Ingredients cannot be empty.");
        }
    }
}

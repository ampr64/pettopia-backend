using Ardalis.SmartEnum;
using Domain.Enumerations;
using FluentValidation;

namespace Application.Features.Posts.Queries.GetPosts
{
    public class GetPostsQueryValidator : AbstractValidator<GetPostsQuery>
    {
        public GetPostsQueryValidator()
        {
            RuleFor(q => q.PetSpecies)
                .Must((species) => BeValidEnum<PetSpecies>((int)species!))
                .When(q => q.PetSpecies is not null)
                .WithMessage("{PropertyValue} is not a valid {PropertyName}.");

            RuleForEach(q => q.PetGender)
                .Must(BeValidEnum<PetGender>)
                .When(q => q.PetGender is { Count: > 0 })
                .WithMessage("{PropertyValue} is not a valid {PropertyName}.");

            RuleForEach(q => q.PetAge)
                .Must(BeValidEnum<PetAge>)
                .When(q => q.PetAge is { Count: > 0 })
                .WithMessage("{PropertyValue} is not a valid {PropertyName}.");

            RuleForEach(q => q.PostType)
                .Must(BeValidEnum<PostType>)
                .When(q => q.PostType is { Count: > 0 })
                .WithMessage("{PropertyValue} is not a valid {PropertyName}.");

            RuleForEach(q => q.NeuterStatus)
                .Must(BeValidEnum<NeuterStatus>)
                .When(q => q.NeuterStatus is { Count: > 0 })
                .WithMessage("{PropertyValue} is not a valid {PropertyName}.");
        }

        private static bool BeValidEnum<TEnum>(int value) where TEnum : SmartEnum<TEnum>
        {
            return SmartEnum<TEnum>.TryFromValue(value, out _);
        }
    }
}
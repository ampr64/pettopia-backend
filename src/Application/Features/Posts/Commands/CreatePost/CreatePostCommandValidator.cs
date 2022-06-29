using Ardalis.SmartEnum;
using Domain.Enumerations;
using FluentValidation;
using System.Net.Mime;

namespace Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        private static readonly string[] AllowedMediaTypes = new[]
        {
            MediaTypeNames.Image.Jpeg,
            "image/png",
        };

        private static readonly int MinimumSize = 1000; // 1KB
        private static readonly int MaximumSize = 10000000; // 10MB

        public CreatePostCommandValidator()
        {
            RuleFor(c => c.PetName)
                .NotEmpty();

            RuleFor(c => c.Description)
                .NotEmpty()
                .MinimumLength(10);

            RuleFor(c => c.PostType)
                .Must((t) => BeValidEnum<PostType>(t))
                .WithMessage("{PropertyValue} is not a valid {PropertyName}.");

            RuleFor(c => c.PetGender)
                .Must((g) => BeValidEnum<PetGender>(g))
                .WithMessage("{PropertyValue} is not a valid {PropertyName}.");

            RuleFor(c => c.NeuterStatus)
                .Must((n) => BeValidEnum<NeuterStatus>(n))
                .WithMessage("{PropertyValue} is not a valid {PropertyName}.");

            RuleFor(c => c.PetSpecies)
                .Must((s) => BeValidEnum<PetSpecies>(s))
                .WithMessage("{PropertyValue} is not a valid {PropertyName}.");

            RuleFor(c => c.PetAge)
                .Must((a) => BeValidEnum<PetAge>(a))
                .WithMessage("{PropertyValue} is not a valid {PropertyName}.");

            RuleFor(c => c.Images)
                .NotEmpty();

            RuleForEach(c => c.Images)
                .NotEmpty()
                .ChildRules(_ =>
                {
                    _.RuleFor(f => f.ContentType)
                        .NotEmpty()
                        .Must(BeAllowedContentType)
                        .WithMessage("Content type '{PropertyValue}' not recognized.");

                    _.RuleFor(f => f.Length)
                        .InclusiveBetween(MinimumSize, MaximumSize)
                        .WithMessage("Image size cannot be less than 1KB or greater than 10MB.");
                });
        }

        // TODO: Move to extension method
        private static bool BeValidEnum<TEnum>(int value) where TEnum : SmartEnum<TEnum>
        {
            return SmartEnum<TEnum>.TryFromValue(value, out _);
        }

        private static bool BeAllowedContentType(string contentType)
        {
            return AllowedMediaTypes.Contains(contentType);
        }
    }
}
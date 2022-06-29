using FluentValidation;
using System.Net.Mime;

namespace Application.Features.Users.Commands.CompleteProfile
{
    public class CompleteProfileCommandValidator : AbstractValidator<CompleteProfileCommand>
    {
        private static readonly string[] AllowedMediaTypes = new[]
        {
            MediaTypeNames.Image.Jpeg,
            "image/png",
        };

        private static readonly int MinimumSize = 1000; // 1KB
        private static readonly int MaximumSize = 10000000; // 10MB

        public CompleteProfileCommandValidator()
        {
            RuleFor(c => c.AddressProvince)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.AddressCity)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.AddressZipCode)
                .NotEmpty()
                .MaximumLength(6);

            RuleFor(c => c.AddressLine1)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(c => c.AddressLine2)
                .MaximumLength(20);

            RuleFor(c => c.AboutMe)
                .NotEmpty()
                .MinimumLength(10);

            RuleForEach(c => c.AdoptionRequirements)
                .NotEmpty()
                .When(c => c.AdoptionRequirements.Any());

            RuleFor(c => c.PhonePrefix)
                .NotEmpty()
                .Length(2, 4);

            When(c => c.PhonePrefix is { Length: > 0 }, () =>
            {
                RuleFor(c => c.PhonePrefix)
                    .Must(NotStartWithZero)
                    .Must(BeAllDigits);
            });

            RuleFor(c => c.PhoneNumber)
                .Must(BeAllDigits)
                .MaximumLength(10);

            When(c => c.PhoneNumber is { Length: > 0 }, () =>
            {
                RuleFor(c => c.PhoneNumber)
                    .Must(BeAllDigits);
            });

            RuleFor(c => c.InstagramUserName)
                .Must(NotBeAUrl!)
                .When(c => c.InstagramUserName is { Length: > 0 })
                .WithMessage("{PropertyName} must not be a URL.");

            RuleFor(c => c.FacebookUserName)
                .Must(NotBeAUrl!)
                .When(c => c.FacebookUserName is { Length: > 0 })
                .WithMessage("{PropertyName} must not be a URL.");

            When(c => c.Pictures is { Count: > 0 }, () =>
            {
                RuleForEach(c => c.Pictures)
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
            });
        }

        private static bool BeAllDigits(string value)
        {
            return value.All(char.IsDigit);
        }

        private static bool BeAllowedContentType(string contentType)
        {
            return AllowedMediaTypes.Contains(contentType);
        }

        private static bool NotBeAUrl(string value)
        {
            return !value.StartsWith("http", StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool NotStartWithZero(string value)
        {
            return !value.StartsWith("0");
        }
    }
}
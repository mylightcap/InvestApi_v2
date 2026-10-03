using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.Auth.Registration.Commands
{
    public class UserRegistrationValidator : AbstractValidator<UserRegistrationCommand>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.UserRegistrationDto.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters.")
                .MaximumLength(100)
                .Must(NotBeDefaultString)
                .WithMessage("Invalid first name.");

            RuleFor(x => x.UserRegistrationDto.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters.")
                .MaximumLength(100)
                .Must(NotBeDefaultString)
                .WithMessage("Invalid last name.");

            RuleFor(x => x.UserRegistrationDto.MiddleName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.UserRegistrationDto.MiddleName));

            RuleFor(x => x.UserRegistrationDto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(NotBeDefaultString)
                .WithMessage("Invalid email.");

            RuleFor(x => x.UserRegistrationDto.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MinimumLength(10)
                .MaximumLength(15)
                .Must(NotBeDefaultString)
                .WithMessage("Invalid phone number.");

            RuleFor(x => x.UserRegistrationDto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.UserRegistrationDto.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(x => x.UserRegistrationDto.Password)
                .WithMessage("Passwords do not match.");

            RuleFor(x => x.UserRegistrationDto.AcceptTermsAndConditions)
                .Equal(true)
                .WithMessage("You must accept the terms and conditions.");

            RuleFor(x => x.UserRegistrationDto.AcceptPrivacyPolicy)
                .Equal(true)
                .WithMessage("You must accept the privacy policy.");

            RuleFor(x => x.UserRegistrationDto.DeviceId)
                .NotEmpty().WithMessage("Device ID is required.")
                .Must(NotBeDefaultString);

            RuleFor(x => x.UserRegistrationDto.DeviceName)
                .NotEmpty().WithMessage("Device name is required.")
                .Must(NotBeDefaultString);

            RuleFor(x => x.UserRegistrationDto.DeviceType)
                .NotEmpty().WithMessage("Device type is required.")
                .Must(NotBeDefaultString);

            RuleFor(x => x.UserRegistrationDto.IpAddress)
                .NotEmpty().WithMessage("IP Address is required.")
                .Must(NotBeDefaultString);

            RuleFor(x => x.UserRegistrationDto.Country)
                .NotEmpty().WithMessage("Country is required.")
                .Must(NotBeDefaultString);

            RuleFor(x => x.UserRegistrationDto.Gender)
                .Must(x =>
                    string.IsNullOrWhiteSpace(x) ||
                    x.Equals("Male", StringComparison.OrdinalIgnoreCase) ||
                    x.Equals("Female", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Gender must be Male or Female.");

            RuleFor(x => x.UserRegistrationDto.DateOfBirth)
                .LessThan(DateTime.UtcNow)
                .When(x => x.UserRegistrationDto.DateOfBirth.HasValue)
                .WithMessage("Invalid date of birth.");
        }

        private bool NotBeDefaultString(string? value)
        {
            return !string.Equals(
                value?.Trim(),
                "string",
                StringComparison.OrdinalIgnoreCase);
        }
    }
}


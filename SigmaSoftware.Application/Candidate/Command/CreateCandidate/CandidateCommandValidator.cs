using FluentValidation;

namespace SigmaSoftware.Application.Candidate.Command.CreateCandidate
{
    public class CandidateCommandValidator : AbstractValidator<CandidateCommand>
    {
        public CandidateCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address format.")
                .WithMessage("Email is already in use.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber)); 

            RuleFor(x => x.CallTimeInterval)
                .Matches(@"^(\d{1,2}(:\d{2})?\s?(AM|PM))\s?-\s?(\d{1,2}(:\d{2})?\s?(AM|PM))$")
                .WithMessage("Call Time Interval should be in the format '9 AM - 11 AM'")
                .When(x => !string.IsNullOrEmpty(x.CallTimeInterval));

            RuleFor(x => x.LinkedInUrl)
                .Matches(@"^(https?://)?(www\.)?linkedin\.com/.+").WithMessage("Invalid LinkedIn URL format.")
                .When(x => !string.IsNullOrEmpty(x.LinkedInUrl)); 

            RuleFor(x => x.GitHubUrl)
                .Matches(@"^(https?://)?(www\.)?github\.com/.+").WithMessage("Invalid GitHub URL format.")
                .When(x => !string.IsNullOrEmpty(x.GitHubUrl)); 

            RuleFor(x => x.FreeTextComment)
                .NotEmpty().WithMessage("Free text comment is required.");
        }

    }
}

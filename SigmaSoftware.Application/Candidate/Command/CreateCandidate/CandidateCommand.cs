using MediatR;
using Microsoft.Extensions.Logging;
using SigmaSoftware.Application.Candidate.Dto;
using SigmaSoftware.Application.Common.Interfaces;
using SigmaSoftware.Application.Common.Messaging;
using SigmaSoftware.Domain.Common.Model;
using SigmaSoftware.Domain.Errors;
using SigmaSoftware.Domain.Events.CandidateEvents;

namespace SigmaSoftware.Application.Candidate.Command.CreateCandidate
{
    public class CandidateCommand : CandidateRequestDto, ICommand<CandidateResponseDto>
    {
    }

    internal sealed class CandidateCommandHandler(
        ICurrentUserService currentUserService,
        IMediator mediator,
        IUnitOfWork unitOfWork,
        IGenericRepository<Domain.Entities.Candidate> candidateRepository,
        ILogger<CandidateCommand> logger)
        : ICommandHandler<CandidateCommand, CandidateResponseDto>
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<Response<CandidateResponseDto>> Handle(CandidateCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // find an existing candidate by email
                var existingCandidate = await candidateRepository
                    .FirstOrDefaultAsync(c => c.Email == request.Email);

                // Create or update candidate entity
                Domain.Entities.Candidate candidate;

                if (existingCandidate == null)
                {
                    // If candidate does not exist, create a new one
                    candidate = new Domain.Entities.Candidate
                    {
                        Email = request.Email,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        PhoneNumber = request.PhoneNumber,
                        CallTimeInterval = request.CallTimeInterval,
                        LinkedInProfileUrl =  request.LinkedInUrl,
                        GitHubProfileUrl = request.GitHubUrl,
                        Comment = request.FreeTextComment,
                    };

                    await candidateRepository.InsertAsync(candidate);

                    // Publish the domain event for new candidate creation
                    await mediator.Publish(new CandidateCreatedEvent(candidate.Email), cancellationToken);
                }
                else
                {
                    // If candidate exists, update the existing record
                    existingCandidate.FirstName = request.FirstName;
                    existingCandidate.LastName = request.LastName;
                    existingCandidate.PhoneNumber = request.PhoneNumber;
                    existingCandidate.CallTimeInterval = request.CallTimeInterval;
                    existingCandidate.LinkedInProfileUrl = request.LinkedInUrl;
                    existingCandidate.GitHubProfileUrl = request.GitHubUrl;
                    existingCandidate.Comment = request.FreeTextComment;
                    existingCandidate.LastModificationTime = DateTime.UtcNow;

                    candidate = existingCandidate;

                    // Publish the domain event for candidate update
                    await mediator.Publish(new CandidateUpdatedEvent(candidate.Email), cancellationToken);
                }

                // Commit changes to the database
                await unitOfWork.CommitAsync(cancellationToken);

                // Return the response DTO
                return new CandidateResponseDto
                {
                    Email = candidate.Email,
                    FirstName = candidate.FirstName,
                    LastName = candidate.LastName,
                    PhoneNumber = candidate.PhoneNumber,
                    CallTimeInterval = candidate.CallTimeInterval,
                    LinkedInUrl = candidate.LinkedInProfileUrl,
                    GitHubUrl = candidate.GitHubProfileUrl,
                    FreeTextComment = candidate.Comment,
                };
            }
            catch (Exception ex)
            {
                // Log the error and return failure response
                logger.LogError(ex, "Failed to register or update candidate in the database!");
                return Response.Failure<CandidateResponseDto>(UserErrors.UserNotCreated);
            }
        }
    }
}
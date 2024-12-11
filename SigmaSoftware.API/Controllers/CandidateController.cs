using Microsoft.AspNetCore.Mvc;
using SigmaSoftware.API.Controllers.Helper;
using SigmaSoftware.Application.Candidate.Command.CreateCandidate;
using SigmaSoftware.Application.Candidate.Dto;
using SigmaSoftware.Domain.Common.Model;

namespace SigmaSoftware.API.Controllers;

public class CandidateController: ApiControllerBase
{
    [HttpPost]
    public async Task<Response<CandidateResponseDto>> CreateCandidateAsync([FromBody] CandidateCommand command) => await Mediator.Send(command);
}
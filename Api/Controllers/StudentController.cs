using Application.UseCases.Students.Commands.AddSubject;
using Application.UseCases.Students.Commands.CreateStudent;
using Application.UseCases.Students.Queries.GetStudentsBySubjectIdAndIdentification;
using MediatR;

namespace Api.Controllers;

[ApiController]
[Route(ApiRoutes.Students)]
public class StudentController : ControllerBase 
{
    private readonly IMediator _mediator;

    public StudentController(IMediator mediator) => _mediator = mediator;
    
    
    [HttpPost]
    public async Task<IActionResult> CreateStudent(CreateStudentCommand command)
    {
        await _mediator.Send(command);
        return Accepted();
    }
    
    [HttpPatch("{identification}")]
    public async Task<IActionResult> AddSuject([FromRoute] string identification, [FromBody] AddSubjectCommand command)
    {
        command.Identification = identification;
        await _mediator.Send(command);
        return Accepted();
    }
    
    [HttpGet("{identification}/{subjectId:guid}")]
    public async Task<IEnumerable<GetStudentsBySubjectIdAndIndentificationDto>> GetAllSubject([FromRoute] string identification, [FromRoute] Guid subjectId)
    {
        return await _mediator.Send(new GetStudentsBySubjectIdAndIndentificationQuery(identification, subjectId));
    }
}

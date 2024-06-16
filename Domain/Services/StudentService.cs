using Domain.Entities;
using Domain.Exceptions.Common;
using Domain.Ports;

namespace Domain.Services;

[DomainService]
public class StudentService
{
    private readonly IGenericRepository<Student> _studentRepository;
    private readonly SubjectService _subjectService;

    public StudentService(IGenericRepository<Student> studentRepository, SubjectService subjectService)
    {
        _studentRepository = studentRepository;
        _subjectService = subjectService;
    }

    public async Task CreateAsync(Student student)
    {
        await ValidateExistingIdentification(student.Identification);
        await _studentRepository.AddAsync(student);
    }

    public async Task<Student> GetStudentByIdentification(string identification)
    {
        Student? student = (await _studentRepository.GetAsync(
            e =>
                e.Identification == identification,
                includeObjectProperties: f => f.StudentSubjects
            )).FirstOrDefault();
        _ = student ??
            throw new ResourceNotFoundException(string.Format(Messages.ResourceNotFoundException, nameof(identification), identification));
        return student;
    }

    public async Task AddSubject(string identification, IEnumerable<Guid> subjectsId)
    {

        Student student = await GetStudentByIdentification(identification);
        foreach (Guid subjectId in subjectsId)
        {
            Subject subject = await _subjectService.GetSubjectById(subjectId);
            ProfessorSubject professorSubject = subject.ProfessorSubjects.First();
            student.AddSubject(subjectId, professorSubject.ProfessorId);
        }

        await _studentRepository.UpdateAsync(student);
    }

    private async Task ValidateExistingIdentification(string identification)
    {
        bool alredyExistIdentification = await _studentRepository.Exist(
            student => student.Identification == identification
        );
        if (alredyExistIdentification)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, nameof(identification), identification);
            throw new ResourceAlreadyExistException(exceptionMessage);
        }
    }
}
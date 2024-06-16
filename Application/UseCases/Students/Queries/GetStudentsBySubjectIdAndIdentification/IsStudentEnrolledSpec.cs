using Ardalis.Specification;
using Domain.Entities;

namespace Application.UseCases.Students.Queries.GetStudentsBySubjectIdAndIdentification;

public class IsStudentEnrolledSpec : Specification<StudentSubject>
{
    public IsStudentEnrolledSpec(string identification)
    {
        Query.Where(s => s.Student.Identification == identification);
    }
}

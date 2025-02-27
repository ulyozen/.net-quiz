using Quiz.Core.Entities;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Mappers;

public static class SubmissionMapper
{
    public static SubmissionEntity MapToEntity(this Submission submission)
    {
        return new SubmissionEntity
        {
            Id = submission.Id,
            UserId = submission.UserId,
            Answers = submission.Answers.MapToEntities()
        };
    }
    
    public static IEnumerable<Submission> MapToSubmissions(this IEnumerable<SubmissionEntity> entities)
    {
        return entities.Select(MapToSubmission).ToList();
    }

    private static Submission MapToSubmission(this SubmissionEntity entity)
    {
        return Submission.Restore(entity.Id, entity.TemplateId, entity.UserId, entity.Answers.MapToAnswers());
    }
}
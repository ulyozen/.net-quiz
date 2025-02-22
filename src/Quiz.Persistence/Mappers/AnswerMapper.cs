using Quiz.Core.Abstractions;
using Quiz.Core.Entities;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Mappers;

public static class AnswerMapper
{
    public static List<AnswerEntity> MapToEntities(this IEnumerable<IAnswer> answers)
    {
        return answers.Select(MapToEntity).ToList();
    }
    
    public static List<IAnswer> MapToAnswers(this IEnumerable<AnswerEntity> entities)
    {
        return entities.Select(MapToAnswer).ToList();
    }

    private static AnswerEntity MapToEntity(this IAnswer answer)
    {
        return answer switch
        {
            Answer<string?> textAnswer => new AnswerEntity
            {
                AnswerText = textAnswer.Value,
                QuestionId = textAnswer.QuestionId
            },
            Answer<int?> intAnswer => new AnswerEntity
            {
                AnswerValue = intAnswer.Value,
                QuestionId = intAnswer.QuestionId
            },
            Answer<bool?> boolAnswer => new AnswerEntity
            {
                AnswerBoolean = boolAnswer.Value,
                QuestionId = boolAnswer.QuestionId
            },
            _ => throw new InvalidOperationException("Answer must have at least one valid answer value.")
        };
    }
    
    private static IAnswer MapToAnswer(this AnswerEntity entity)
    {
        return entity switch
        {
            { AnswerText: not null } => Answer<string?>.Create(entity.QuestionId, entity.AnswerText),
            { AnswerValue: not null } => Answer<int?>.Create(entity.QuestionId, entity.AnswerValue),
            { AnswerBoolean: not null } => Answer<bool?>.Create(entity.QuestionId, entity.AnswerBoolean),
            _ => throw new InvalidOperationException("AnswerEntity must have at least one valid answer value.")
        };
    }
}
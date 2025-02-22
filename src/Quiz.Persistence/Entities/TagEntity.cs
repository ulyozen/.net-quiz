namespace Quiz.Persistence.Entities;

public class TagEntity : BaseEntity
{
    public string Name { get; set; }
    
    public IList<TemplateTagEntity> Templates { get; set; }
}
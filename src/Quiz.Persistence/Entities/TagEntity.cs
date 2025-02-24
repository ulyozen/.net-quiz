namespace Quiz.Persistence.Entities;

public class TagEntity : BaseEntity
{
    public string Name { get; set; }
    
    public IList<TemplateTag> Templates { get; set; }
}
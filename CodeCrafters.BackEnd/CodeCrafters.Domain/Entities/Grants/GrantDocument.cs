namespace CodeCrafters.Domain.Entities.Grants;

public class GrantDocument
{
    public Guid Id { get; set; }

    public Guid GrantTypeId { get; set; }

    public string DocumentName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsMandatory { get; set; }

    public int DisplayOrder { get; set; }

    public GrantType GrantType { get; set; } = null!;
}


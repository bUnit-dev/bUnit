namespace Bunit.Roles;

public class FindByRoleOptions
{
    /// <summary>
    /// If set to true, includes elements that are normally hidden from accessibility tree.
    /// </summary>
    public bool Hidden { get; set; }
    
    /// <summary>
    /// Specify a name for the element or a RegExp to match against its accessible name.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Controls whether the Name option acts as an exact match or a partial match.
    /// </summary>
    public bool Exact { get; set; } = true;
    
    /// <summary>
    /// Specifies a subset of accessible attributes to filter the element by, in the form of key-value pairs.
    /// </summary>
    public Dictionary<string, string>? Attributes { get; set; }
    
    /// <summary>
    /// Specific level for roles that support levels (e.g., heading levels 1-6).
    /// </summary>
    public int? Level { get; set; }
    
    /// <summary>
    /// Filter by checked state for roles that support it (e.g., checkbox, radio).
    /// </summary>
    public bool? Checked { get; set; }
    
    /// <summary>
    /// Filter by selected state for roles that support it (e.g., option).
    /// </summary>
    public bool? Selected { get; set; }
    
    /// <summary>
    /// Filter by pressed state for roles that support it (e.g., button).
    /// </summary>
    public bool? Pressed { get; set; }
    
    /// <summary>
    /// Filter by expanded state for roles that support it (e.g., button, menu).
    /// </summary>
    public bool? Expanded { get; set; }
    
    /// <summary>
    /// Filter by description (aria-describedby) for roles that have descriptions.
    /// </summary>
    public string? Description { get; set; }
} 
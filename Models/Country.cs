namespace liquidlabs_assignment.Models;

public class Country
{
    public string Uuid { get; set; } = string.Empty;
    public string Name { set; get; } = string.Empty;
    public string Continent { set; get; } = string.Empty;
    public SyncLevel SyncLevel { set; get; }

}
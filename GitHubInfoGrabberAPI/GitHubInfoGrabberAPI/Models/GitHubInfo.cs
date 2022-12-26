namespace GitHubInfoGrabberAPI.Models;
public class GitHubInfo
{
    public List<Project> projects { get; set; } = new List<Project>();
    public string type { get; set; } 
}

public enum GHIType
{
    Popular = 1,
    Pinned = 2,
    PageNotFound = 3,
    NoProjectsFound = 4,
}

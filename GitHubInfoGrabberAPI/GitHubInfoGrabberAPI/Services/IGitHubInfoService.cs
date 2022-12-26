using GitHubInfoGrabberAPI.Models;

namespace GitHubInfoGrabberAPI.Services
{
    public interface IGitHubInfoService
    {
        GitHubInfo GetGitHubInfo(string url);
    }
}
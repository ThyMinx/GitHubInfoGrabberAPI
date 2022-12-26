using GitHubInfoGrabberAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using GitHubInfoGrabberAPI.Services;

namespace GitHubInfoGrabberAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GitHubInfoController : ControllerBase
{
    private readonly IGitHubInfoService _ghiService;

    public GitHubInfoController(IGitHubInfoService ghiService)
    {
        _ghiService = ghiService;
    }

    [HttpGet("GetPinnedProjects/{username}")]
    [EnableCors("_AllowMySite")]
    public IActionResult GetPinnedProject(string username)
    {
        var un = username.ToLower();
        if (un.Contains("www.github"))
        {
            un = un.Split('/').Last();
        }

        string url = $"https://www.github.com/{un}/";

        var info = _ghiService.GetGitHubInfo(url);

        switch (Enum.Parse(typeof(GHIType), info.type))
        {
            case GHIType.Popular:
                return Ok(info);
            case GHIType.Pinned:
                return Ok(info);
            case GHIType.PageNotFound:
                return NotFound($"{url} does not lead to a valid GitHub profile.");
            case GHIType.NoProjectsFound:
                return NotFound($"No Projects could be found at: {url}.");
            default:
                return BadRequest($"Something went wrong trying to get projects from: {url}. There were {info.projects.Count} projects found.");
        }
    }
}

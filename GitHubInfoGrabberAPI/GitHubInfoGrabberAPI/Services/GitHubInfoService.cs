using GitHubInfoGrabberAPI.Models;
using HtmlAgilityPack;

namespace GitHubInfoGrabberAPI.Services;
public class GitHubInfoService : IGitHubInfoService
{
    public GitHubInfo GetGitHubInfo(string url)
    {
        var ghi = new GitHubInfo();

        var web = new HtmlWeb();

        var doc = web.Load(url);

        //Check if the url given exists
        var isPageNotFound = doc.DocumentNode.Descendants().Where(n => n.OriginalName == "img" && n.GetAttributeValue("alt", string.Empty).Contains("404")).Any();
        if (isPageNotFound)
        {
            ghi.type = GHIType.PageNotFound.ToString();
            return ghi;
        }

        var pinnedItemsContainerNode = doc.DocumentNode.Descendants().Single(n => n.OriginalName == "div" && n.HasClass("js-pinned-items-reorder-container"));

        //Check if there are no projects at all
        var isEmptyProjects = pinnedItemsContainerNode.Descendants().Where(n => n.OriginalName == "h2" && n.HasClass("blankslate-heading")).Any();
        if (isEmptyProjects)
        {
            ghi.type = GHIType.NoProjectsFound.ToString();
            return ghi;
        }

        var listHeader = pinnedItemsContainerNode.ChildNodes.Single(n => n.OriginalName == "ol" && n.HasClass("gutter-condensed"));

        if (listHeader.HasClass("js-pinned-items-reorder-list"))
        {
            ghi.type = GHIType.Pinned.ToString();
        }
        else
        {
            ghi.type = GHIType.Popular.ToString();
        }

        var nodesLi = listHeader.ChildNodes.Where(n => n.OriginalName == "li").ToList();

        foreach (var li in nodesLi)
        {
            var liChildren = li.Descendants();

            var projectNameNode = liChildren.Single(n => n.OriginalName == "span" && n.HasClass("repo"));
            var projectName = projectNameNode.InnerText.Trim();

            var projectLink = $"{url}{projectName}";

            var projectDescriptionNode = liChildren.Single(n => n.OriginalName == "p" && n.HasClass("pinned-item-desc"));
            var projectDescription = projectDescriptionNode.InnerText.Trim();

            var project = new Project
            {
                name = projectName,
                link = projectLink,
                description = projectDescription
            };

            ghi.projects.Add(project);
        }

        return ghi;
    }
}

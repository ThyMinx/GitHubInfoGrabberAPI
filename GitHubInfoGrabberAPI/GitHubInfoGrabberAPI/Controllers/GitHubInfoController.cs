using GitHubInfoGrabberAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;

namespace GitHubInfoGrabberAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubInfoController : ControllerBase
    {
        [HttpGet(Name = "GetPinnedProjects")]
        public GitHubInfo GetPinnedProject(string url)
        {
            GitHubInfo info = new GitHubInfo();

            string type = "";

            List<Project> data = new List<Project>();

            try
            {
                type = "pinned";
                data = GetListOfPinnedProjects(url);
            }
            catch (Exception ex)
            {
                type = "popular";
                data = GetListOfPopularProjects(url);
            }
            finally
            {
                info.type = type;
                info.projects = data;
            }

            return info;
        }

        public List<Project> GetListOfPopularProjects(string url)
        {
            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(url);

            var listHeader = doc.DocumentNode.SelectSingleNode("//ol[@class='d-flex flex-wrap list-style-none gutter-condensed mb-4']");

            List<HtmlNode> nodes = new List<HtmlNode>();

            foreach (var node in listHeader.ChildNodes)
            {
                if (node.OriginalName == "li")
                    nodes.Add(node);
            }

            for (int i = 1; i < nodes.Count + 1; i++)
            {
                doc.DocumentNode.SelectSingleNode("/html/body/div[4]/main/div[2]/div/div[2]/div[2]/div/div[1]/div/ol/li[" + i + "]/div/div/div/span[2]").Remove();
            }

            List<Project> projects = new List<Project>();

            foreach (var node in nodes)
            {
                Project project = new Project();

                var boxNode = NodeChecker(node, "//div[@class='Box pinned-item-list-item d-flex p-3 width-full public source']");
                var projectDetailsNode = NodeChecker(boxNode, "//div[@class='pinned-item-list-item-content']");
                var nameNode = NodeChecker(projectDetailsNode, "//div[@class='d-flex width-full flex-items-center position-relative']");
                var descNode = NodeChecker(projectDetailsNode, "//p[@class='pinned-item-desc color-fg-muted text-small d-block mt-2 mb-3']");

                project.name = nameNode.InnerText.TrimStart().TrimEnd();
                project.description = descNode.InnerText.TrimStart().TrimEnd();
                project.link = url.EndsWith("/") ? url + project.name : url + "/" + project.name;

                projects.Add(project);
            }

            return projects;
        }

        public List<Project> GetListOfPinnedProjects(string url)
        {
            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(url);

            var listHeader = doc.DocumentNode.SelectSingleNode("//ol[@class='d-flex flex-wrap list-style-none gutter-condensed mb-2 js-pinned-items-reorder-list']");

            List<HtmlNode> nodes = new List<HtmlNode>();

            foreach (var node in listHeader.ChildNodes)
            {
                if(node.OriginalName == "li")
                    nodes.Add(node);
            }

            for (int i = 1; i < nodes.Count + 1; i++)
            {
                doc.DocumentNode.SelectSingleNode("/html/body/div[4]/main/div[2]/div/div[2]/div[2]/div/div[1]/div/ol/li[" + i + "]/div/div/div/div/span[2]").Remove();
            }

            List<Project> projects = new List<Project>();

            foreach (var node in nodes)
            {
                Project project = new Project();

                var boxNode = NodeChecker(node, "//div[@class='Box d-flex pinned-item-list-item p-3 width-full js-pinned-item-list-item public sortable-button-item source']");
                var projectDetailsNode = NodeChecker(boxNode, "//div[@class='pinned-item-list-item-content']");
                var nameNode = NodeChecker(projectDetailsNode, "//div[@class='d-flex width-full position-relative']");
                var descNode = NodeChecker(projectDetailsNode, "//p[@class='pinned-item-desc color-fg-muted text-small d-block mt-2 mb-3']");

                project.name = nameNode.InnerText.TrimStart().TrimEnd();
                project.description = descNode.InnerText.TrimStart().TrimEnd();
                project.link = url.EndsWith("/") ? url + project.name : url + "/" + project.name;

                projects.Add(project);
            }

            return projects;
        }

        public HtmlNode NodeChecker(HtmlNode parent, string xpath)
        {
            var nodechecker = parent.SelectNodes(xpath);
            var matchedNode = nodechecker.Where(x => x.ParentNode == parent).FirstOrDefault();

            return matchedNode;
        }
    }
}

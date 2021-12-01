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
        [HttpGet(Name = "GetStuff")]
        public GitHubInfo GetProfileInfo(string url)
        {
            List<Project> data = GetInfo(url);

            return new GitHubInfo { projects = data };

            //return new GitHubInfo
            //{
            //    projects = new List<Project>() {
            //    new Project() { name = "one", link = "www.one.com", description = "one" },
            //    new Project() { name = "two", link = "www.two.com", description = "two" },
            //    new Project() { name = "three", link = "www.three.com", description = data }
            //    }
            //};
        }

        public List<Project> GetInfo(string url)
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
                //var nodeRemove = node.SelectNodes("//span[@class='Label Label--secondary v-align-middle mt-1 no-wrap v-align-baseline']");
                //foreach (var item in nodeRemove)
                //{
                //    item.Remove();
                //}

                //var nodechecker = node.SelectNodes("//div[@class='Box d-flex pinned-item-list-item p-3 width-full js-pinned-item-list-item public sortable-button-item source']");

                //var projectDetailsNode = nodechecker.Where(x => x.ParentNode == node).FirstOrDefault();

                var boxNode = NodeChecker(node, "//div[@class='Box d-flex pinned-item-list-item p-3 width-full js-pinned-item-list-item public sortable-button-item source']");

                var projectDetailsNode = NodeChecker(boxNode, "//div[@class='pinned-item-list-item-content']");

                //var nameNode = projectDetailsNode.SelectSingleNode("//div[@class='d-flex width-full position-relative']");
                var nameNode = NodeChecker(projectDetailsNode, "//div[@class='d-flex width-full position-relative']");

                //var descNode = projectDetailsNode.SelectSingleNode("//p[@class='pinned-item-desc color-fg-muted text-small d-block mt-2 mb-3']");
                var descNode = NodeChecker(projectDetailsNode, "//p[@class='pinned-item-desc color-fg-muted text-small d-block mt-2 mb-3']");

                //var linkNode = nameNode.SelectSingleNode("//a");

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

        public async Task<string> GetDataFromWebPage(string fullurl)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(fullurl);

            return response;

            //ParseHtml(response);
        }

        public string ParseHtml(string htmlData)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlData);

            var table = doc.DocumentNode.Descendants("ol[class='js-pinned-items-reorder-list']");

            return table.ToString();

            //foreach (var item in table.ToList()[0].ChildNodes)
            //{

            //}
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GitHubSearch.Models
{
    public class User
    {
        [Required(ErrorMessage = "Required")]
        public string Username { get; set; }

        public string Name { get; set; }
        [DisplayFormat(NullDisplayText = "NULL")]
        public string Location { get; set; }
        public string Avtar { get; set; }

        public List<repoDetails> RepoDetails { get; set; }

        public string getUserDetails(string username)
        {
            string userDetails = "";
            string url = "https://api.github.com/users/" + username.Trim();
            var client = new JsonServiceClient(url);
            try
            {
                userDetails = client.Get<string>("");
                dynamic dynObj = JsonConvert.DeserializeObject(userDetails);
                Name = (string)dynObj["name"];
                Location = (string)dynObj["location"];
                Avtar = (string)dynObj["avatar_url"];
                userDetails = "Success";
            }
            catch(WebServiceException e)
            {
                Username = null;
                userDetails = e.Message;
            }
            return userDetails;
        }
        
        public string getUserRepos(string username)
        {
            string userRepos = "";
            string url = "https://api.github.com/users/" +username.Trim() + "/repos";
            var client = new JsonServiceClient(url);
            try
            {
                userRepos = client.Get<string>("");
                JArray array = JArray.Parse(userRepos);
                JArray sorted = new JArray(array.OrderByDescending(obj => obj["stargazers_count"]));
                int counter = 0;

                RepoDetails = new List<repoDetails>();

                foreach (var item in sorted.Children())
                {
                    var itemProperties = item.Children<JProperty>();
                    var myRepoId = itemProperties.FirstOrDefault(x => x.Name == "id");
                    var myRepoName = itemProperties.FirstOrDefault(x => x.Name == "name");
                    var myRepoURL = itemProperties.FirstOrDefault(x => x.Name == "url");
                    var myHtmlURL = itemProperties.FirstOrDefault(x => x.Name == "html_url");
                    var myDescription = itemProperties.FirstOrDefault(x => x.Name == "description");
                    var mystargazers_count = itemProperties.FirstOrDefault(x => x.Name == "stargazers_count");

                    repoDetails cModel = new repoDetails();

                    cModel.Id = myRepoId.Value.ToObject<int>();
                    cModel.RepoName = myRepoName.ToObject<string>();
                    cModel.URL = myRepoURL.ToObject<string>();
                    cModel.HtmlURL = myHtmlURL.ToObject<string>();
                    cModel.Description = myDescription.ToObject<string>();
                    cModel.stargazers_count = mystargazers_count.ToObject<int>();

                    RepoDetails.Add(cModel);

                    counter++;
                    if (counter == 5) break;
                }
                
            }
            catch(WebServiceException e)
            {

            }
            return userRepos;
        }
    }
    public class repoDetails
    {
        public string RepoName { get; set; }
        public string URL { get; set; }
        public int Id { get; set; }
        public int stargazers_count { get; set; }
        public string HtmlURL { get; set; }
        public string Description { get; set; }
    }
}
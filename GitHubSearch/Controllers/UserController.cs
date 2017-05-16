using System.Web.Mvc;


namespace GitHubSearch.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult User()
        {
           return View();
        }
        [HttpPost]
        public ActionResult User(GitHubSearch.Models.User user)
        {
            if (ModelState.IsValid)
            {
                string details = user.getUserDetails(user.Username);
                if (details == "Success")
                {
                    ViewBag.errMsg = "";
                    string repodetails = user.getUserRepos(user.Username);
                    
                    return View(user);
                }
                else
                {
                    ViewBag.errMsg = "The user is not available in the hub";
                    return View();
                }
                
            }
            return View();
        }
       
    }
}
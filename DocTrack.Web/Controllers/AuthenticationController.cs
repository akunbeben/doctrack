using DocTrack.Data.Repository;
using DocTrack.Web.Service;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace DocTrack.Web.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: Authentication
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            //bool authenticate = ActiveDirectoryService.Authenticate(username, password);

            //if(!authenticate)
            //{
            //    return View();
            //}

            var userdata = _userRepository.GetUserByUsername(username);

            if (userdata == null)
            {
                return View();
            }

            var urlApi = "http://localhost:8080/bonita/loginservice";
            object data = new { username, password };

            WebRequest http = HttpWebRequest.Create(urlApi);
            http.Method = "POST";
            http.ContentType = "application/x-www-form-urlencoded";
            var streamWriter = new StreamWriter(http.GetRequestStream());
            streamWriter.Write(data);
            streamWriter.Close();

            http.GetResponse();

            Session["dht-username"] = userdata.Username;

            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Authentication");
        }
    }
}
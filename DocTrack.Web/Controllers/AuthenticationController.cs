using DocTrack.Data.Repository;
using DocTrack.Web.Service;
using System.IO;
using System.Net;
using System;
using System.Net.Http;
using System.Web.Mvc;
using DocTrack.Web.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocTrack.Web.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly string baseUrl = "http://localhost:8080/bonita/";

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

            string[] userObject = new string[2] { username, password };

            if (userdata == null)
            {
                return RedirectToAction("login", "authentication");
            }

            using (var client = new HttpClient())
            {
                string jsonContent = JsonConvert.SerializeObject(userObject);

                string postUrl = string.Format(baseUrl + "loginservice?username={0}&password={1}&redirect=false", userObject);

                using (HttpContent content = new StringContent(jsonContent))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    HttpResponseMessage response = client.PostAsync(postUrl, content).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("login", "authentication");
                    };

                    var cookie = response.Headers.FirstOrDefault(header => header.Key == "Set-Cookie").Value;
                }
            }

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
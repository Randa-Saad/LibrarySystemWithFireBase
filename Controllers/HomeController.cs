using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using LibrarySystemWithFirebase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibrarySystemWithFirebase.Controllers
{
    public class HomeController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "wqwMPwFvpI1wauyUykuOTCGaepY828XgSL9WPHqD",
            BasePath = "https://rlibrarysystem-default-rtdb.firebaseio.com/"

        };
        IFirebaseClient client;
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Books");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Book>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Book>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }

        [HttpGet]
        public ActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBook(Book book)
        {
            try
            {

                AddBookToFirebase(book);
                ModelState.AddModelError(string.Empty, "Added Successfully");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }

        private void AddBookToFirebase(Book book)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = book;
            PushResponse response = client.Push("Books/", data);
            data.BookId = response.Result.name;
            SetResponse setResponse = client.Set("Books/" + data.BookId, data);


        }
        public ActionResult Books()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Books");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Book>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Book>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }

        public ActionResult Borrowers()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Borrowers");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Borrower>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Borrower>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }

    }
}
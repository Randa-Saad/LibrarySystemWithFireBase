using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using LibrarySystemWithFirebase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibrarySystemWithFirebase.Controllers
{
    public class OperationsController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "wqwMPwFvpI1wauyUykuOTCGaepY828XgSL9WPHqD",
            BasePath = "https://rlibrarysystem-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        // GET: Operations
        public ActionResult Index()
        {
          
            var list=GetBooksList();
            return View(list);
        }
        public List<Book> GetBooksList()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Books");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Book>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Book>(((JProperty)item).Value.ToString()));
            }
            return list;
        }
        public ActionResult RevertPage()
        {
            //go to revert Form
            return View();
        }

        [HttpPost]
        public ActionResult CheckRevert(string BookId, string BookName)
        {
            var list = GetBooksList();
            client = new FireSharp.FirebaseClient(config);
            if (list!=null)
            { 

            for(int i=0;i<=list.Count;i++)
            {
                var ReturnedBook = list[i];
          
                if(ReturnedBook.BookId==BookId && ReturnedBook.BookName.ToLower()== BookName)
                    {
                        if (ReturnedBook.NoOfExistingCopies < ReturnedBook.NoOfCopies)
                        {
                            var IncreaseCopies = (ReturnedBook.NoOfExistingCopies) + 1;
                            ReturnedBook.NoOfExistingCopies = IncreaseCopies;
                            SetResponse response = client.Set("Books/" + ReturnedBook.BookId, ReturnedBook);
                            return View();
                        }
                        else
                        {
                            return RedirectToAction("RevertError");
                        }
                    }
                else
                    {
                        return RedirectToAction("WrongData");
                    }
                }

            }
            return RedirectToAction("WrongData");

        }
        [HttpPost]
        public ActionResult CheckBorrow(string bookid, string borrowername)
        {
            var list = GetBooksList();
            client = new FireSharp.FirebaseClient(config);
            if (list != null)
            {

                for (int i = 0; i < list.Count; i++)
                {
                    var ReturnedBook = list[i];

                    if (ReturnedBook.BookId == bookid)
                    {

                        if (ReturnedBook.NoOfExistingCopies != 0)
                        {
                            var decreaseCopies = (ReturnedBook.NoOfExistingCopies) - 1;
                            ReturnedBook.NoOfExistingCopies = decreaseCopies;
                            //decrease copies of that book by book id
                            SetResponse response = client.Set("Books/" + ReturnedBook.BookId, ReturnedBook);
                            //insert borrower name and bookid in borrwer table
                            var borrower = new Borrower() { BookId = bookid, BorrowerName = borrowername,BookName=ReturnedBook.BookName };
                            var data = borrower;
                            PushResponse response2 = client.Push("Borrowers/", data);
                            data.BorrowerId = response2.Result.name;
                            SetResponse setResponse = client.Set("Borrowers/" + data.BorrowerId, data);
                            return View();
                        }
                        else
                        {

                            return RedirectToAction("BorrowerError");
                        }
                    }
                }
                return RedirectToAction("WrongData");
            }
            return RedirectToAction("WrongData");



        }

        public ActionResult BorrowPage()
        {
            //go to Borrow Form
            return View();
        }

        public ActionResult BorrowerError()
        {
            return View();
        }

        public ActionResult RevertError()
        {
            return View();
        }
        public ActionResult WrongData()
        {
            return View();
        }
    
    }
}
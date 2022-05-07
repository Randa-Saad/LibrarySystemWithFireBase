using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibrarySystemWithFirebase.Models
{
    public class Book
    {
        [Display(Name = "Book Code")]
        public string BookId { get; set; }
        [Display(Name = "Book Name")]
        public string BookName { get; set; }
        [Display(Name = "No Of Copies")]
        public int NoOfCopies { get; set; }
        [Display(Name = "No Of Existing Copies")]
        public int NoOfExistingCopies { get; set; }




    }
}
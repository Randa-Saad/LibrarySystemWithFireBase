using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibrarySystemWithFirebase.Models
{
    public class Borrower
    {
        [Required]
        [Display(Name = "Borrower Code")]
        public string BorrowerId { get; set; }

        [Display(Name = "Borrower Name")]
        [Required(ErrorMessage = "Borrower Name is Required")]
        [StringLength(50)]
        public string BorrowerName { get; set; }

        [Display(Name = "Book Code")]
        public string BookId { get; set; }
        [Display(Name = "Book Name")]
        public string BookName { get; set; }

    }
}
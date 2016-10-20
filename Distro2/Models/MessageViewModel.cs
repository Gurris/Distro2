using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace Distro2.Models
{
    public class CreateMessageViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Reciver")]
        public string selectedReciver { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Message")]
        public string Message { get; set; }

        public string confMessage { get; set; }

        public List<SelectListItem> listItems = new List<SelectListItem>();


        public void setUsers(List<ApplicationUser> input)
        {
            var users = input;

            foreach (ApplicationUser user in users)
            {
                listItems.Add(new SelectListItem
                {
                    Text = user.Email,
                    Value = user.Email
                });
            }

        }
    }

    public class ListMessagesViewModel
    {
        public int messageId { get; set; }

        [Display(Name = "From")]
        public string sender { get; set; }

        [Display(Name = "Title")]
        public string title { get; set; }

        [Display(Name = "sent")]
        public DateTime date { get; set; }

        [Display(Name = "Read")]
        public bool read { get; set; }

        [Display(Name = "Removed messages")]
        public int removedMsg { get; set; }
        [Display(Name = "Read messages")]
        public int nrOfMessages { get; set; }
    }

    public class IndexMessageViewModel
    {
        public int messageId { get; set; }

        [Display(Name = "From")]
        public string sender { get; set; }

    }

    public class DetailsMessageViewModel
    {
        public int messageId { get; set; }

        [Display(Name = "Title")]
        public string title { get; set; }
        [Display(Name = "Message")]
        public string Message { get; set; }
        [Display(Name = "Sender")]
        public string sender { get; set; }
    }
}

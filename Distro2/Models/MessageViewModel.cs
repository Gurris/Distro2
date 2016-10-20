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

        public IEnumerable<SelectListItem> getUsers()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            IEnumerable<ApplicationUser> users = new List<ApplicationUser>();
            List<SelectListItem> listItems = new List<SelectListItem>();
            users = db.Users.ToList();

            foreach (ApplicationUser user in users) // move to another layer
            {
                listItems.Add(new SelectListItem
                {
                    Text = user.Email,
                    Value = user.Email
                });
            }
            
            return listItems;
        }

    }
    

}
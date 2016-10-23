using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Distro2.Models;
using Microsoft.AspNet.Identity;

namespace Distro2.Controllers
{
    [Authorize]
    public class UserLoginTimesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserLoginTimes
        public ActionResult Index()
        {
            var model = new IndexPageViewModel();
            ApplicationUser currnetUser = db.Users.Find(User.Identity.GetUserId());
            model.name = currnetUser.Email;
            // -------- Last time the user was logged in --------
            List<UserLoginTime> logins = db.UserLoginTime.ToList(); // get all logins
            List<UserLoginTime> thisUserLogin = db.UserLoginTime.Where(login => login.user.Id.Contains(currnetUser.Id)).ToList();

            var thisDate = DateTime.Now;
            model.nrOfLoginsThisMonth = 0;
            model.nrOfUnreadMeassages = 0;
            
            foreach(UserLoginTime login in thisUserLogin)
            {
                if(login.loginDate.Year == thisDate.Year && login.loginDate.Month == thisDate.Month)
                {
                    model.nrOfLoginsThisMonth += 1;
                }
            }

            model.lastLogin = thisUserLogin[(thisUserLogin.Count - 1)].loginDate; // heighest login is current login.
            
            List<MessageModel> messages = db.Message.ToList(); // get all messages
            List<MessageModel> thisUserMessages = new List<MessageModel>();
            foreach(MessageModel message in messages)
            {
                if (message.toUser.Id.Equals(currnetUser.Id) && message.read == false)
                {
                    model.nrOfUnreadMeassages += 1;
                }
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

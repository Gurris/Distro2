﻿using System;
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
            List<UserLoginTime> logins = db.UserLoginTime.ToList(); // gets all logins
            List<UserLoginTime> thisUserLogin = new List<UserLoginTime>();
            var thisDate = DateTime.Now;
            model.nrOfLoginsThisMonth = 0;
            foreach (UserLoginTime login in logins) // only get logins related to current user
            {
                if (login.user.Id.Equals(currnetUser.Id))
                {
                    thisUserLogin.Add(login);
                    if(login.loginDate.Year == thisDate.Year && login.loginDate.Month == thisDate.Month)
                    {
                        model.nrOfLoginsThisMonth += 1;
                    }
                }
            }
            if(thisUserLogin.Count == 1)
            {
                model.lastLogin = thisUserLogin[0].loginDate;
            }
            else
            {
                model.lastLogin = thisUserLogin[(thisUserLogin.Count - 2)].loginDate; // heighest login is current login. So get one below it
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
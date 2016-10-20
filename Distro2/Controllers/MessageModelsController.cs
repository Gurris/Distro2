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
using System.Collections;
using System.Data.Entity.Validation;

namespace Distro2.Controllers
{
    [Authorize]
    public class MessageModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MessageModels
        public ActionResult Index()
        {
            //get only singnd in users messages
            List<IndexMessageModel> messageList = new List<IndexMessageModel>();

            var currentUser = db.Users.Find(User.Identity.GetUserId());
            var messages = db.Message.ToList();

            foreach (MessageModel message in messages)
            {
                IndexMessageModel tmp = new IndexMessageModel();
                tmp.messageId = message.messageId;
                tmp.sender = message.fromUser.Email;
                tmp.title = message.title;
                tmp.date = message.date;
                tmp.read = message.read;
                messageList.Add(tmp);
            }
            return View(messageList);
        }

        // GET: MessageModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageModel messageModel = db.Message.Find(id);
            if (messageModel == null)
            {
                return HttpNotFound();
            }
            return View(messageModel);
        }

        // GET: MessageModels/Create
        public ActionResult Create() // make it so you cannot send messages to oneself!
        {
            var model = new CreateMessageViewModel();
            List<ApplicationUser> users = new List<ApplicationUser>();
            users = db.Users.ToList();
            model.setUsers(users);

            return View(model);
        }

        // POST: MessageModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateMessageViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = db.Users.Find(User.Identity.GetUserId());
                MessageModel message = new MessageModel();
                message.title = messageViewModel.Title;
                message.message = messageViewModel.Message;


                var tmp = db.Users.ToList(); // UGLYYYYYYYYYYY!Y!Y!YY!Y!Y!Y!Y!YY!Y!IUAWDHAWIUFgAUQLEYFGasEULYgs<euklfg FIX

                foreach(ApplicationUser user in tmp)
                {
                    if (user.Email.Equals(messageViewModel.selectedReciver))
                    {
                        message.toUser = user;
                        break;
                    }
                }
                
                // IF currentUser = sendToUser return false or somthign!
                
                message.read = false;
                message.fromUser = currentUser;
                message.date = DateTime.Now;

                db.Message.Add(message);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }


                return RedirectToAction("Index");
            }

            return View(messageViewModel);
        }

        
        public IEnumerable<SelectListItem> getUsers()
        {
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

        // GET: MessageModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageModel messageModel = db.Message.Find(id);
            if (messageModel == null)
            {
                return HttpNotFound();
            }
            return View(messageModel);
        }

        // POST: MessageModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "messageId,title,message,date,read")] MessageModel messageModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messageModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(messageModel);
        }

        // GET: MessageModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageModel messageModel = db.Message.Find(id);
            if (messageModel == null)
            {
                return HttpNotFound();
            }
            return View(messageModel);
        }

        // POST: MessageModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MessageModel messageModel = db.Message.Find(id);
            db.Message.Remove(messageModel);
            db.SaveChanges();
            return RedirectToAction("Index");
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

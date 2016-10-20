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
            List<IndexMessageViewModel> messageList = new List<IndexMessageViewModel>();

            var currentUser = db.Users.Find(User.Identity.GetUserId());
            var messages = db.Message.ToList();
            bool dup = false;
            foreach (MessageModel message in messages)
            {
                if (message.toUser == currentUser)
                {
                    dup = false;
                    for(int i=0; i<messageList.Count; i++)
                    {
                        if(message.fromUser.Email.Equals(messageList[i].sender)) // removes duplicate sender
                        {
                            dup = true;
                            continue;
                        }
                    }
                    if (dup)
                        continue;
                    IndexMessageViewModel tmp = new IndexMessageViewModel();
                    tmp.messageId = message.messageId;
                    tmp.sender = message.fromUser.Email;
                    messageList.Add(tmp);
                }
                
            }
            return View(messageList);
        }

        public ActionResult ListMessage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //get only messages from sender and to this user
            List<ListMessagesViewModel> messageList = new List<ListMessagesViewModel>();
            ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());
            MessageModel recivedMessage = db.Message.Find(id);
            ApplicationUser sender = recivedMessage.fromUser;
            int rmvMessages = 0;
            if (sender == null || currentUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<MessageModel> messages = db.Message.ToList();

            foreach (MessageModel message in messages)
            {
                if(message.removed == true)
                {
                    rmvMessages += 1;
                    continue;
                }
                if (message.toUser == currentUser && message.fromUser == sender) // only adds messages with currentUser and sender is correct
                {
                    ListMessagesViewModel tmp = new ListMessagesViewModel();
                    tmp.messageId = message.messageId;
                    tmp.sender = message.fromUser.Email;
                    tmp.title = message.title;
                    tmp.date = message.date;
                    tmp.read = message.read;
                    messageList.Add(tmp);
                }
            }
            if(messageList.Count >= 1)
            {
                messageList[0].removedMsg = rmvMessages;
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
            DetailsMessageViewModel detailMessage = new DetailsMessageViewModel();
            detailMessage.messageId = messageModel.messageId;
            detailMessage.title = messageModel.title;
            detailMessage.sender = messageModel.fromUser.Email;
            detailMessage.Message = messageModel.message;

            messageModel.read = true;

            if (ModelState.IsValid) // mark message as read
            {
                db.Entry(messageModel).State = EntityState.Modified;
                db.SaveChanges();
            }

            return View(detailMessage);
        }

        // GET: MessageModels/Create
        public ActionResult Create()
        {
            var model = new CreateMessageViewModel();
            List<ApplicationUser> allUsers = new List<ApplicationUser>();
            allUsers = db.Users.ToList();
            List<ApplicationUser> modeifiedUsers = new List<ApplicationUser>();
            foreach(ApplicationUser u in allUsers)
            {
                if (!u.Id.Equals(User.Identity.GetUserId())) // only displays user that is not currentUser
                {
                    modeifiedUsers.Add(u);
                }
            }

            model.setUsers(modeifiedUsers);

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
                message.read = false;
                message.fromUser = currentUser;
                message.date = DateTime.Now;
                message.toUser = db.Users.ToList().Where(x => x.Email == messageViewModel.selectedReciver).First();
                
                
                if(message.toUser == message.fromUser)
                {
                    return View(messageViewModel);
                }
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

            if (ModelState.IsValid)
            {
                messageModel.removed = true;
                db.Entry(messageModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            //db.Message.Remove(messageModel);
            //db.SaveChanges();
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

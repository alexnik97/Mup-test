using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MUPTest.Models;

namespace MUPTest.Controllers
{
    public class HomeController : Controller
    {
        private RequestContext db = new RequestContext();
        
        public ActionResult Index(int? statusId, string firstDate, string secondDate)
        {
            AddStatusInViewBag();
            IQueryable<Request> requests = db.Requests.Include(p => p.Status);
            if (statusId != null)
            {
                requests = requests.Where(p => p.StatusId == statusId);
            }
            if (!String.IsNullOrEmpty(firstDate) && !String.IsNullOrEmpty(secondDate))
            {
                DateTime fromDate = DateTime.Today;
                DateTime toDate = DateTime.Today;
                try
                {
                    fromDate = DateTime.Parse(firstDate);
                    toDate = DateTime.Parse(secondDate);
                    requests = requests.Where(p => p.Date >= fromDate);
                    requests = requests.Where(p => p.Date <= toDate);
                }
                catch (FormatException)
                {
                    ViewBag.FormatExciptoin = "формат даты не разобран";
                }
            }

            return View(requests.ToList());
        }

        private void AddStatusInViewBag()
        {
            SelectList statuses = new SelectList(db.Statuses, "Id", "StatusCaption");
            ViewBag.Statuses = statuses;
        }

        // GET: CreateRequest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreateRequest/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Caption")] Request request)
        {
            request.StatusId = 1;
            request.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                request = db.Requests.Add(request);
                var movement = new RequestMovement();
                movement.Comment = "Создана заявка";
                movement.Date = DateTime.Now;
                movement.NewStatusId = 1;
                movement.OldStatusId = 1;
                movement.RequestId = request.Id;
                db.RequestMovements.Add(movement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(request);
        }

        // GET: CreateRequest/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: CreateRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
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

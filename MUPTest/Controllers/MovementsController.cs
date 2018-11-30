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
    public class MovementsController : Controller
    {
        private RequestContext db = new RequestContext();

        // GET: Movements
        public ActionResult Index(int? id)
        {
            return View(db.RequestMovements.Where(p => p.RequestId == id).Include(p => p.Request).Include(p => p.Request.Status).Include(p => p.NewStatus).Include(p => p.OldStatus).ToList());
        }

        // GET: Movements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestMovement requestMovement = db.RequestMovements.Find(id);
            if (requestMovement == null)
            {
                return HttpNotFound();
            }
            return View(requestMovement);
        }

        // GET: Movements/Create
        public ActionResult Create(int id)
        {
            var requestMovement = new RequestMovement();
            requestMovement.RequestId = id;
            requestMovement.OldStatusId = db.Requests.Find(id).StatusId;
            SelectList statuses;
            if (db.Requests.Find(id).StatusId == 2)
                statuses = new SelectList(db.Statuses.Where(p => p.Id > 2), "Id", "StatusCaption");
            else if (db.Requests.Find(id).StatusId == 1)
                statuses = new SelectList(db.Statuses.Where(p => p.Id == 2), "Id", "StatusCaption");
            else
                statuses = new SelectList(db.Statuses.Where(p => p.Id == 4), "Id", "StatusCaption");
            ViewBag.Statuses = statuses;

            return View(requestMovement);
        }

        // POST: Movements/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OldStatusId,NewStatusId,RequestId,Comment")] RequestMovement requestMovement, int id)
        {
            requestMovement.OldStatusId = db.Requests.Find(id).StatusId;
            db.Requests.Find(id).StatusId = requestMovement.NewStatusId;
            requestMovement.RequestId = id;
            if (ModelState.IsValid)
            {
                requestMovement.Date = DateTime.Now;
                db.RequestMovements.Add(requestMovement);
                db.SaveChanges();
                return RedirectToAction("Index/" + id.ToString());
            }

            return View(requestMovement);
        }

    }
}

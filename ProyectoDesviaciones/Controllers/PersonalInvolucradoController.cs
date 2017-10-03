using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoDesviaciones.Models;

namespace ProyectoDesviaciones.Controllers
{
    public class PersonalInvolucradoesController : Controller
    {
        private DesviacionesDBEntities db = new DesviacionesDBEntities();

        // GET: PersonalInvolucradoes
        public ActionResult Index()
        {
            return View(db.PersonalInvolucrado.ToList());
        }

        // GET: PersonalInvolucradoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalInvolucrado personalInvolucrado = db.PersonalInvolucrado.Find(id);
            if (personalInvolucrado == null)
            {
                return HttpNotFound();
            }
            return View(personalInvolucrado);
        }

        // GET: PersonalInvolucradoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonalInvolucradoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPersonal,Nombre,Cargo,Apellido,CorreoElectronico,EstadoPersonal")] PersonalInvolucrado personalInvolucrado)
        {
            if (ModelState.IsValid)
            {
                db.PersonalInvolucrado.Add(personalInvolucrado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(personalInvolucrado);
        }

        // GET: PersonalInvolucradoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalInvolucrado personalInvolucrado = db.PersonalInvolucrado.Find(id);
            if (personalInvolucrado == null)
            {
                return HttpNotFound();
            }
            return View(personalInvolucrado);
        }

        // POST: PersonalInvolucradoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPersonal,Nombre,Cargo,Apellido,CorreoElectronico,EstadoPersonal")] PersonalInvolucrado personalInvolucrado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personalInvolucrado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personalInvolucrado);
        }

        // GET: PersonalInvolucradoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalInvolucrado personalInvolucrado = db.PersonalInvolucrado.Find(id);
            if (personalInvolucrado == null)
            {
                return HttpNotFound();
            }
            return View(personalInvolucrado);
        }

        // POST: PersonalInvolucradoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonalInvolucrado personalInvolucrado = db.PersonalInvolucrado.Find(id);
            db.PersonalInvolucrado.Remove(personalInvolucrado);
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

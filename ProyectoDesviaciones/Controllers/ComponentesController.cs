using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoDesviaciones.BD;

namespace ProyectoDesviaciones.Controllers
{
    [Authorize]
    public class ComponentesController : Controller
    {
        private DesviacionesDBEntities db = new DesviacionesDBEntities();

        // GET: Componentes
        public ActionResult Index()
        {
            return View();
        }

        //Mandarle la informacion al DataTable
        [HttpGet]
        public JsonResult GetComponentes()
        {
            var componentes = db.Componentes.Where(a => a.EstadoComponente == true).Select(x => new {
                Id = x.IdComponente,
                Nombre = x.DescripcionComponente,
                NumeroDeParte = x.NumeroParte
            }).ToList();

            var datos = Json(componentes, JsonRequestBehavior.AllowGet);
            datos.MaxJsonLength = Int32.MaxValue;
            return datos;
        }

        // GET: Componentes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Componentes componentes = db.Componentes.Find(id);
            if (componentes == null)
            {
                return HttpNotFound();
            }
            return View(componentes);
        }

        // GET: Componentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Componentes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdComponente,NumeroParte,DescripcionComponente,EstadoComponente")] Componentes componentes)
        {
            if(componentes.NumeroParte != null && componentes.DescripcionComponente != null)
            {
                if (ModelState.IsValid)
                {
                    db.Componentes.Add(componentes);
                    componentes.EstadoComponente = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Message"] = "Debe llenar los campos requeridos!";
                return View(componentes);
            }


            return View(componentes);
        }

        // GET: Componentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Componentes componentes = db.Componentes.Find(id);
            if (componentes == null)
            {
                return HttpNotFound();
            }
            return View(componentes);
        }

        // POST: Componentes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdComponente,NumeroParte,DescripcionComponente,EstadoComponente")] Componentes componentes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(componentes).State = EntityState.Modified;
                componentes.EstadoComponente = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(componentes);
        }

        // GET: Componentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Componentes componentes = db.Componentes.Find(id);
            if (componentes == null)
            {
                return HttpNotFound();
            }
            return View(componentes);
        }

        // POST: Componentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Componentes componentes = db.Componentes.Find(id);
            db.Entry(componentes).State = EntityState.Modified;
            componentes.EstadoComponente = false;
            //db.Componentes.Remove(componentes);
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

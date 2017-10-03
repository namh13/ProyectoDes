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
    public class TipoDesviacionsController : Controller
    {
        private DesviacionesDBEntities db = new DesviacionesDBEntities();

        // GET: TipoDesviacions
        public ActionResult Index()
        {
            return View(db.TipoDesviacion.Where(x=>x.EstadoTipoDesviacion==true).ToList());
        }

        //Mandarle la informacion al DataTable
        [HttpGet]
        public JsonResult GetTipoDesviacions()
        {
            var tipodesviacion = db.TipoDesviacion.Where(a => a.EstadoTipoDesviacion == true).Select(x => new {
                Id = x.IdTipoDesviacion,
                Nombre = x.DescripcionTipoDesviacion,
            }).ToList();

            var datos = Json(tipodesviacion, JsonRequestBehavior.AllowGet);
            datos.MaxJsonLength = Int32.MaxValue;
            return datos;
        }


        // GET: TipoDesviacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDesviacion tipoDesviacion = db.TipoDesviacion.Find(id);
            if (tipoDesviacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoDesviacion);
        }

        // GET: TipoDesviacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDesviacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTipoDesviacion,DescripcionTipoDesviacion,EstadoTipoDesviacion")] TipoDesviacion tipoDesviacion)
        {
            if(tipoDesviacion.DescripcionTipoDesviacion !=null)
            {
                if (ModelState.IsValid)
                {
                    db.TipoDesviacion.Add(tipoDesviacion);
                    tipoDesviacion.EstadoTipoDesviacion = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            else
            {
                TempData["Message"] = "El campo descripcion debe estar lleno!";
                return View(tipoDesviacion);
            }

            return View(tipoDesviacion);
        }

        // GET: TipoDesviacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDesviacion tipoDesviacion = db.TipoDesviacion.Find(id);
            if (tipoDesviacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoDesviacion);
        }

        // POST: TipoDesviacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTipoDesviacion,DescripcionTipoDesviacion,EstadoTipoDesviacion")] TipoDesviacion tipoDesviacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDesviacion).State = EntityState.Modified;
                tipoDesviacion.EstadoTipoDesviacion = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDesviacion);
        }

        // GET: TipoDesviacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDesviacion tipoDesviacion = db.TipoDesviacion.Find(id);
            if (tipoDesviacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoDesviacion);
        }

        // POST: TipoDesviacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDesviacion tipoDesviacion = db.TipoDesviacion.Find(id);
            //db.TipoDesviacion.Remove(tipoDesviacion);
            db.Entry(tipoDesviacion).State = EntityState.Modified;
            tipoDesviacion.EstadoTipoDesviacion = false;
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

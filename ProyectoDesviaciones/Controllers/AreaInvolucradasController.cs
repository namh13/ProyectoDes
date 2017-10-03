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
    public class AreaInvolucradasController : Controller
    {
        private DesviacionesDBEntities db = new DesviacionesDBEntities();

        // GET: AreaInvolucradas
        public ActionResult Index()
        {
            return View(db.AreaInvolucrada.Where(a=>a.EstadoAreaInvolucrada==true).ToList());
        }

        //Mandarle la informacion al DataTable
        [HttpGet]
        public JsonResult GetAreaInvolucradas()
        {
            var areainvolucrada = db.AreaInvolucrada.Where(a => a.EstadoAreaInvolucrada == true).Select(x => new {
                Id = x.IdAreasInvolucradas,
                Nombre = x.DescripcionAreaInvolucrada,
            }).ToList();

            var datos = Json(areainvolucrada, JsonRequestBehavior.AllowGet);
            datos.MaxJsonLength = Int32.MaxValue;
            return datos;
        }


        // GET: AreaInvolucradas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaInvolucrada areaInvolucrada = db.AreaInvolucrada.Find(id);
            if (areaInvolucrada == null)
            {
                return HttpNotFound();
            }
            return View(areaInvolucrada);
        }

        // GET: AreaInvolucradas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AreaInvolucradas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAreasInvolucradas,DescripcionAreaInvolucrada,EstadoAreaInvolucrada")] AreaInvolucrada areaInvolucrada)
        {
            if(areaInvolucrada.DescripcionAreaInvolucrada != null)
            {
                if (ModelState.IsValid)
                {
                    db.AreaInvolucrada.Add(areaInvolucrada);
                    areaInvolucrada.EstadoAreaInvolucrada = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Message"] = "El campo descripcion es requerido!";
                return View(areaInvolucrada);
            }
            

            return View(areaInvolucrada);
        }

        // GET: AreaInvolucradas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaInvolucrada areaInvolucrada = db.AreaInvolucrada.Find(id);
            if (areaInvolucrada == null)
            {
                return HttpNotFound();
            }
            return View(areaInvolucrada);
        }

        // POST: AreaInvolucradas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAreasInvolucradas,DescripcionAreaInvolucrada,EstadoAreaInvolucrada")] AreaInvolucrada areaInvolucrada)
        {
            if (ModelState.IsValid)
            {
                db.Entry(areaInvolucrada).State = EntityState.Modified;
                areaInvolucrada.EstadoAreaInvolucrada = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(areaInvolucrada);
        }

        // GET: AreaInvolucradas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaInvolucrada areaInvolucrada = db.AreaInvolucrada.Find(id);
            if (areaInvolucrada == null)
            {
                return HttpNotFound();
            }
            return View(areaInvolucrada);
        }

        // POST: AreaInvolucradas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AreaInvolucrada areaInvolucrada = db.AreaInvolucrada.Find(id);
            //db.AreaInvolucrada.Remove(areaInvolucrada);
            db.Entry(areaInvolucrada).State = EntityState.Modified;
            areaInvolucrada.EstadoAreaInvolucrada = false;
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

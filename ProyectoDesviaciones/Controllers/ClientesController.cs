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
    public class ClientesController : Controller
    {
        private DesviacionesDBEntities db = new DesviacionesDBEntities();

        // GET: Clientes
        public ActionResult Index()
        {
            return View();
        }

        //Mandarle la informacion al DataTable
        [HttpGet]
        public JsonResult GetClientes()
        {
            var clientes = db.Clientes.Where(a => a.EstadoCliente == true).Select(x=> new {
                Id = x.IdCliente,
                Nombre = x.DescripcionDelCliente,
                Telefono = x.TelefonoDelCliente
            }).ToList();

           var datos = Json(clientes, JsonRequestBehavior.AllowGet);
           datos.MaxJsonLength = Int32.MaxValue;
           return datos;
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCliente,DescripcionDelCliente,TelefonoDelCliente,EstadoCliente")] Clientes clientes)
        {
            if (clientes.DescripcionDelCliente != null && clientes.TelefonoDelCliente != null) 
            {
                if (ModelState.IsValid)
                {
                    db.Clientes.Add(clientes);
                    clientes.EstadoCliente = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            else
            {
                TempData["Message"] = "Revice los campos que son requeridos!";
                return View(clientes);
            }


            return View(clientes);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCliente,DescripcionDelCliente,TelefonoDelCliente,EstadoCliente")] Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientes).State = EntityState.Modified;
                clientes.EstadoCliente = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clientes);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clientes clientes = db.Clientes.Find(id);
            //db.Clientes.Remove(clientes);
            db.Entry(clientes).State = EntityState.Modified;
            clientes.EstadoCliente = false;
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

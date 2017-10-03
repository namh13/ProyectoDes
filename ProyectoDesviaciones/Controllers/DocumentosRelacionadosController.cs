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
    public class DocumentosRelacionadosController : Controller
    {
        private DesviacionesDBEntities db = new DesviacionesDBEntities();

        // GET: DocumentosRelacionados
        public ActionResult Index()
        {
            var lista = db.DocumentosRelacionados.Where(x => x.EstadoDocumentoRelacionado == true); // Esta variable la declaras vos, porque el sistema te lo crea diferente. Aqui estas mandando a llamar todos los registros de la tabla, siempre y cuando el estado sea true(activo). Con el WHERE se hace la condicion que registros mostrar
            return View(lista.ToList()); //Aqui lo que haces es mostrar todos los campos que obtuviste de la variable anterior, y pones que la variable trae una lista de registros
        }
        //Mandarle la informacion al DataTable
        [HttpGet]
        public JsonResult GetDocumentosRelacionados()
        {
            var documentosrelacionados = db.DocumentosRelacionados.Where(a => a.EstadoDocumentoRelacionado == true).Select(x => new {
                Id = x.IdDocumentosRelacionados,
                Nombre = x.DescripcionDocumentosRelacionados,
            }).ToList();

            var datos = Json(documentosrelacionados, JsonRequestBehavior.AllowGet);
            datos.MaxJsonLength = Int32.MaxValue;
            return datos;
        }



        // GET: DocumentosRelacionados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentosRelacionados documentosRelacionados = db.DocumentosRelacionados.Find(id);
            if (documentosRelacionados == null)
            {
                return HttpNotFound();
            }
            return View(documentosRelacionados);
        }

        // GET: DocumentosRelacionados/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DocumentosRelacionados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDocumentosRelacionados,DescripcionDocumentosRelacionados,EstadoDocumentoRelacionado")] DocumentosRelacionados documentosRelacionados)
        {
            if(documentosRelacionados.DescripcionDocumentosRelacionados != null)
            {
                if (ModelState.IsValid)
                {
                    db.DocumentosRelacionados.Add(documentosRelacionados);
                    documentosRelacionados.EstadoDocumentoRelacionado = true; //Esto lo pones asi para que todo los registros se guarden con el estado activo, debido a que en el index solo mostraras los registros activos
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Message"] = "El campo descripcion es requerido!";
                return View(documentosRelacionados);
            }



            return View(documentosRelacionados);
        }

        // GET: DocumentosRelacionados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentosRelacionados documentosRelacionados = db.DocumentosRelacionados.Find(id);
            if (documentosRelacionados == null)
            {
                return HttpNotFound();
            }
            return View(documentosRelacionados);
        }

        // POST: DocumentosRelacionados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDocumentosRelacionados,DescripcionDocumentosRelacionados,EstadoDocumentoRelacionado")] DocumentosRelacionados documentosRelacionados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentosRelacionados).State = EntityState.Modified;
                documentosRelacionados.EstadoDocumentoRelacionado = true; //Siempre se pone true para que actualize el registro y quede el estado como activo (true)
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(documentosRelacionados);
        }

        // GET: DocumentosRelacionados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentosRelacionados documentosRelacionados = db.DocumentosRelacionados.Find(id);
            if (documentosRelacionados == null)
            {
                return HttpNotFound();
            }
            return View(documentosRelacionados);
        }

        // POST: DocumentosRelacionados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentosRelacionados documentosRelacionados = db.DocumentosRelacionados.Find(id);
            ///db.DocumentosRelacionados.Remove(documentosRelacionados);  ---- Esta linea lo que hace es eliminar los registros de la tabla, pero no se va a borrar sino que solo se va a desactivar por eso se comenta esto
            db.Entry(documentosRelacionados).State = EntityState.Modified; // En esta linea de codigo en si lo que haces es actualizar el registro, lo pasas de true a false, osea de activo a desactivo.
            documentosRelacionados.EstadoDocumentoRelacionado = false;  //Aqui le cambias el estado al registro
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

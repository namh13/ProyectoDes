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
    public class PersonalInvolucradoesController : Controller
    {
        private DesviacionesDBEntities db = new DesviacionesDBEntities();

        // GET: PersonalInvolucradoes
        public ActionResult Index()
        {
            return View(db.PersonalInvolucrado.Where(x=>x.EstadoPersonal==true).ToList());
        }

        //Mandarle la informacion al DataTable
        [HttpGet]
        public JsonResult GetPersonalInvolucradoes()
        {
            var personalinvolucrado = db.PersonalInvolucrado.Where(a => a.EstadoPersonal == true).Select(x => new {
                Id = x.IdPersonal,
                Nombre = x.Nombre,
                Apellido = x.Apellido,
                Cargo = x.Cargo,
                CorreoElectronico = x.CorreoElectronico,
                OrdenEvaluar = x.OrdenAEvaluar,
                Usuario = x.Usuario
            }).ToList();

            var datos = Json(personalinvolucrado, JsonRequestBehavior.AllowGet);
            datos.MaxJsonLength = Int32.MaxValue;
            return datos;
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
        public ActionResult Create([Bind(Include = "IdPersonal,Nombre,Cargo,Apellido,CorreoElectronico,EstadoPersonal,OrdenAEvaluar,Usuario")] PersonalInvolucrado personalInvolucrado)
        {
            if(personalInvolucrado.Nombre != null && personalInvolucrado.Apellido != null && personalInvolucrado.Cargo != null && personalInvolucrado.CorreoElectronico != null && personalInvolucrado.OrdenAEvaluar > 0)
            {
                if (db.PersonalInvolucrado.Where(x=>x.EstadoPersonal).Any(x => x.OrdenAEvaluar == personalInvolucrado.OrdenAEvaluar))
                {
                    TempData["Message"] = "Ya existe un registro con ese numero de Orden a Evaluar!";
                    return View(personalInvolucrado);
                }
                else if (ModelState.IsValid)
                {
                    db.PersonalInvolucrado.Add(personalInvolucrado);
                    personalInvolucrado.EstadoPersonal = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Message"] = "Los Campos no pueden quedar vacio!";
                return View(personalInvolucrado);
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
            Session["Message"] = personalInvolucrado.OrdenAEvaluar;
            return View(personalInvolucrado);
        }

        // POST: PersonalInvolucradoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPersonal,Nombre,Cargo,Apellido,CorreoElectronico,EstadoPersonal,OrdenAEvaluar,Usuario")] PersonalInvolucrado personalInvolucrado)
        {
            var Orden =  Convert.ToInt32(Session["Message"].ToString());
            if (personalInvolucrado.Nombre != null && personalInvolucrado.Apellido != null && personalInvolucrado.Cargo != null && personalInvolucrado.CorreoElectronico != null && personalInvolucrado.OrdenAEvaluar > 0)
            {
                var personal = db.PersonalInvolucrado.Where(x => x.EstadoPersonal).Any(x => x.OrdenAEvaluar == personalInvolucrado.OrdenAEvaluar);
                if (personal==true && Orden != personalInvolucrado.OrdenAEvaluar)
                {
                    TempData["Message"] = "Ya existe un registro con ese numero de Orden a Evaluar!";
                    return View(personalInvolucrado);
                }
                else if (ModelState.IsValid)
                {
                    db.Entry(personalInvolucrado).State = EntityState.Modified;
                    personalInvolucrado.EstadoPersonal = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Message"] = "Los Campos no pueden quedar vacio!";
                return View(personalInvolucrado);
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
            db.Entry(personalInvolucrado).State = EntityState.Modified;
            personalInvolucrado.EstadoPersonal = false;
            //db.PersonalInvolucrado.Remove(personalInvolucrado);
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

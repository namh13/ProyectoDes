using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoDesviaciones.BD;
using Microsoft.AspNet.Identity;
using ProyectoDesviaciones.Models;

namespace ProyectoDesviaciones.Controllers
{
    [Authorize]
    public class DesviacionesController : Controller
    {
        private DesviacionesDBEntities db = new DesviacionesDBEntities();


        // GET: Desviaciones
        public ActionResult Index()
        {
            //var desviaciones = db.Desviaciones.Include(d => d.Clientes).Include(d => d.Componentes).Include(d => d.TipoDesviacion);
            return View();
        }

        public JsonResult GetDesviaciones()
        {
            var desviaciones = db.Desviaciones.Where(a => a.EstadoDesviacion == true).Select(x => new
            {
                id = x.IdDesviacion,
                NDesviacion = x.NumeroDesviacion,
                NParte = x.Componentes.NumeroParte,
                Cliente = x.Clientes.DescripcionDelCliente,
                Fecha = x.FechaDesviacion,
                RequeridoPor = x.RequeridoPor
            }).ToList();

            var datos = Json(desviaciones, JsonRequestBehavior.AllowGet);
            datos.MaxJsonLength = Int32.MaxValue;
            return datos;
        }

        public ActionResult AprobarDesviacion(int? id)
        {
            var desviacion = db.Desviaciones.Where(x => x.EstadoDesviacion == true && x.IdDesviacion == id).Select(x => new
            {
                NoDesviacion = x.NumeroDesviacion,
                NumeroParte = x.Componentes.NumeroParte,
                Cliente = x.Clientes.DescripcionDelCliente,
                FechaDesviacion = x.FechaDesviacion,
                RequeridoPor = x.RequeridoPor,
                ProcesoCorrecto = x.DescripcionProcesoCorrecto,
                Condicion = x.DescripcionCondicionComponente,
                Razon = x.DescripcionRazonDesviacion,
                ResponsableDescripcion = x.DescripcionResponsable,
                FechaDescripcion = x.DescripcionFechaDesviacion,
                Criterio = x.CriterioAceptado,
                DetalleCriterio = x.DetalleCriterioAceptado,
                Vencimiento = x.Vencimiento, 
                FechaVencimiento = x.FechaVencimiento,
                Piezas = x.CantidadDePiezas,
                ListaDocumentos = db.DocumentosRelacionados
            });
            return View();
        }

        //Obtener el PersonalInvolucrado
        //[HttpPost]
        public JsonResult GetPersonalInvolucrado(string Id)
        {
            var datos = db.PersonalInvolucrado.Where(x => x.EstadoPersonal == true).Select(x => new {
                //IdP = x.IdPersonal,
                Nombre = x.Nombre +" "+ x.Apellido,
                Cargo = x.Cargo,
                //FechaAprobacion = x.ResultadoDePersonalInvolucrado.Where(z=>z.IdDesviaciones==x.IdPersonal).Select(y=>y.FechaAprobacion),
                //Aprobado = x.ResultadoDePersonalInvolucrado.Any()

            });
            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        //Obtener los Documentos Relacionados
        [HttpGet]
        public JsonResult GetDocumentosRelacionados()
        {
            var documentos = db.DocumentosRelacionados.Where(x => x.EstadoDocumentoRelacionado).Select(x=> new {
                Id = x.IdDocumentosRelacionados,
                documento = x.DescripcionDocumentosRelacionados
            }).ToList();

            var datos = Json(documentos, JsonRequestBehavior.AllowGet);
            datos.MaxJsonLength = Int32.MaxValue;
            return datos;
        }

        public JsonResult Aprobar()
        {
            int userId = 0;
            int idDesviacion = Convert.ToInt32(TempData["Message"].ToString());
            bool data = false;
            var user = User.Identity.GetUserName();
            var UserId = (from x in db.PersonalInvolucrado where x.Usuario == user select x.IdPersonal).ToList();
            userId = int.Parse((UserId.First()).ToString());

            var existe = db.ResultadoDePersonalInvolucrado.Where(x=>x.EstadoPersonalInvolucrado && x.IdDesviaciones==idDesviacion).Any(x=>x.IdPersonal == userId);

            if(existe != true)
            {
                int orden = 0;
                var Orden = (from x in db.PersonalInvolucrado where x.EstadoPersonal == true && x.IdPersonal == userId select x.OrdenAEvaluar).ToList();
                   // Convert.ToInt32(db.PersonalInvolucrado.Where(x => x.EstadoPersonal && x.IdPersonal == userId).Select(x => new { OrdenD = x.OrdenAEvaluar }));
                orden = int.Parse((Orden.First()).ToString());
                var LstPersonal = db.PersonalInvolucrado.Where(x => x.EstadoPersonal).OrderBy(x=>x.OrdenAEvaluar).ToList();
                bool status = data;

                foreach(var p in LstPersonal)
                {
                    if(p.OrdenAEvaluar < orden)
                    {
                        var existeP = db.ResultadoDePersonalInvolucrado.Where(x => x.EstadoPersonalInvolucrado && x.IdDesviaciones == idDesviacion).Any(x => x.IdPersonal == p.IdPersonal);
                        if (!existeP)
                        {
                            return Json(status, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if(p.OrdenAEvaluar == orden)
                    {
                        var existePD = db.ResultadoDePersonalInvolucrado.Where(x => x.EstadoPersonalInvolucrado && x.IdDesviaciones == idDesviacion).Any(x => x.IdPersonal == p.IdPersonal);
                        if (!existePD)
                        {
                            status = true;
                            var pers = db.ResultadoDePersonalInvolucrado.Add(new ResultadoDePersonalInvolucrado
                            {
                                IdPersonal = userId,
                                IdDesviaciones = idDesviacion,
                                FechaAprobacion = DateTime.Now,
                                EstadoPersonalInvolucrado = true
                            });
                            db.SaveChanges();
                            return Json(status, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(status, JsonRequestBehavior.AllowGet);
                    }
                 }

            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //Obtener las Areas Involucradas
        [HttpGet]
        public JsonResult GetAreasInvolucradas()
        {
            var areas = db.AreaInvolucrada.Where(x => x.EstadoAreaInvolucrada).Select(x => new {
                Id = x.IdAreasInvolucradas,
                Area = x.DescripcionAreaInvolucrada
            }).ToList();

            var datos = Json(areas, JsonRequestBehavior.AllowGet);
            datos.MaxJsonLength = Int32.MaxValue;
            return datos;
        }

        // GET: Desviaciones/Details/5
        public ActionResult Details(int? id)
        {
            TempData["Message"] = Convert.ToString(id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Desviaciones desviaciones = db.Desviaciones.Find(id);

            DesviacionModel desviacion = new DesviacionModel
            {
                IdDesviacion = Convert.ToInt32(id),
                NumeroDeParte = desviaciones.Componentes.DescripcionComponente, 
                IdCliente = desviaciones.IdCliente,
                Cliente = desviaciones.Clientes.DescripcionDelCliente,
                NumeroDesviacion = desviaciones.NumeroDesviacion,
                FechaDesviacion = Convert.ToDateTime(desviaciones.FechaDesviacion),
                RequeridoPor = desviaciones.RequeridoPor,
                DescripcionProcesoCorrecto = desviaciones.DescripcionProcesoCorrecto,
                DescripcionCondicionComponente = desviaciones.DescripcionCondicionComponente,
                DescripcionRazonDesviacion = desviaciones.DescripcionRazonDesviacion,
                DescripcionPlanDeAccion = desviaciones.DescripcionPlanDeAccion,
                DescripcionFechaDesviacion = desviaciones.DescripcionFechaDesviacion,
                DescripcionResponsable = desviaciones.DescripcionResponsable,
                CriterioAceptado = desviaciones.CriterioAceptado,
                DetalleCriterioAceptado = desviaciones.DetalleCriterioAceptado,
                Vencimiento = desviaciones.Vencimiento,
                FechaVencimiento = desviaciones.FechaVencimiento ?? new DateTime(),
                CantidadDePiezas = desviaciones.CantidadDePiezas,
                IdTipoDesviacion = desviaciones.IdTipoDesviacion,
                TipoDesviacion = desviaciones.TipoDesviacion.DescripcionTipoDesviacion,
                LstAreas = desviaciones.ResultadoDeAreaInvolucrada.Where(x => x.EstadoArea).Select(x => x.AreaInvolucrada.DescripcionAreaInvolucrada ).ToList(),
                LstDocumentos = desviaciones.ResultadoDeDocumentosRelacionados.Where(x=>x.EstadoDocumento).Select(x=>x.DocumentosRelacionados.DescripcionDocumentosRelacionados).ToList(),
                //Areas = desviaciones.ResultadoDeAreaInvolucrada.Where(x => x.EstadoArea).Select(x =>new ListaAreasDeshabilitar{IdArea=x.IdArea,DescripcionAreaInvolucrada=x.AreaInvolucrada.DescripcionAreaInvolucrada}).ToList(),
                //ListaAreas = desviaciones.ResultadoDeAreaInvolucrada.Select(x => x.IdAreasInvolucradas).ToList(),
                //ListaDocumentos = desviaciones.ResultadoDeDocumentosRelacionados.Select(x => x.IdDocumentoRealizado).ToList()
                SelectListAreas = desviaciones.ResultadoDeAreaInvolucrada.Where(x => x.EstadoArea == true).Select(x => x.IdAreasInvolucradas).ToArray(),
                SelectListDocumentos = desviaciones.ResultadoDeDocumentosRelacionados.Where(x => x.EstadoDocumento == true).Select(x => x.IdDocumentoRealizado).ToArray()
            };

            if (desviacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListaArea = db.AreaInvolucrada.Where(x => x.EstadoAreaInvolucrada).Select(x => new SelectListItem { Value = x.IdAreasInvolucradas.ToString(), Text = x.DescripcionAreaInvolucrada }).ToList();
            ViewBag.ListaDocumento = db.DocumentosRelacionados.Where(x => x.EstadoDocumentoRelacionado).Select(x => new SelectListItem { Value = x.IdDocumentosRelacionados.ToString(), Text = x.DescripcionDocumentosRelacionados }).ToList();
            return View(desviacion);
        }

        // GET: Desviaciones/Create
        public ActionResult Create()
        {
            ViewBag.IdCliente = new SelectList(db.Clientes.Where(x=>x.EstadoCliente==true), "IdCliente", "DescripcionDelCliente");
            ViewBag.IdComponente = new SelectList(db.Componentes.Where(x=>x.EstadoComponente==true), "IdComponente", "NumeroParte");
            ViewBag.IdTipoDesviacion = new SelectList(db.TipoDesviacion.Where(x => x.EstadoTipoDesviacion == true), "IdTipoDesviacion", "DescripcionTipoDesviacion");
            ViewBag.ListaArea = db.AreaInvolucrada.Where(x=>x.EstadoAreaInvolucrada).Select(x => new SelectListItem { Value = x.IdAreasInvolucradas.ToString(), Text = x.DescripcionAreaInvolucrada }).ToList();
            ViewBag.ListaDocumento= db.DocumentosRelacionados.Where(x => x.EstadoDocumentoRelacionado).Select(x => new SelectListItem { Value = x.IdDocumentosRelacionados.ToString(), Text = x.DescripcionDocumentosRelacionados }).ToList();
            return View();
        }

        // POST: Desviaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DesviacionModel desviaciones)
        {
            if (ModelState.IsValid)
            {
                var nuevadesviacion = db.Desviaciones.Add(new Desviaciones
                {
                    IdComponente = desviaciones.IdComponente,
                    IdCliente = desviaciones.IdCliente,
                    NumeroDesviacion = desviaciones.NumeroDesviacion,
                    FechaDesviacion = desviaciones.FechaDesviacion,
                    RequeridoPor = desviaciones.RequeridoPor,
                    DescripcionProcesoCorrecto = desviaciones.DescripcionProcesoCorrecto,
                    DescripcionCondicionComponente = desviaciones.DescripcionCondicionComponente,
                    DescripcionRazonDesviacion = desviaciones.DescripcionRazonDesviacion,
                    DescripcionPlanDeAccion = desviaciones.DescripcionPlanDeAccion,
                    DescripcionFechaDesviacion = desviaciones.DescripcionFechaDesviacion,
                    DescripcionResponsable = desviaciones.DescripcionResponsable,
                    CriterioAceptado = desviaciones.CriterioAceptado,
                    DetalleCriterioAceptado = desviaciones.DetalleCriterioAceptado,
                    Vencimiento = desviaciones.Vencimiento,
                    FechaVencimiento = desviaciones.FechaVencimiento,
                    CantidadDePiezas = desviaciones.CantidadDePiezas,
                    IdTipoDesviacion = desviaciones.IdTipoDesviacion,
                    CreadoPor = User.Identity.GetUserName(),
                    EstadoDesviacion = true
                });
                foreach(var area in desviaciones.ListaAreas)
                {
                    db.ResultadoDeAreaInvolucrada.Add(new ResultadoDeAreaInvolucrada
                    {
                        IdAreasInvolucradas = area,
                        IdDesviaciones = nuevadesviacion.IdDesviacion,
                        EstadoArea = true
                    });
                }
                foreach (var documento in desviaciones.ListaDocumentos)
                {
                    db.ResultadoDeDocumentosRelacionados.Add(new ResultadoDeDocumentosRelacionados
                    {
                        IdDocumentoRealizado = documento,
                        IdDesviacion = nuevadesviacion.IdDesviacion,
                        EstadoDocumento = true
                    });
                }
                var resultado = db.SaveChanges();
                if(resultado>0)
                {
                    TempData["Message"] = "La desviacion se creo correctamente!";
                }
                else
                {
                    TempData["Message"] = "Error al crear la desviacion!";
                }
                return RedirectToAction("Create");
            }

            ViewBag.IdCliente = new SelectList(db.Clientes, "IdCliente", "DescripcionDelCliente", desviaciones.IdCliente);
            ViewBag.IdComponente = new SelectList(db.Componentes, "IdComponente", "NumeroParte", desviaciones.IdComponente);
            ViewBag.IdTipoDesviacion = new SelectList(db.TipoDesviacion, "IdTipoDesviacion", "DescripcionTipoDesviacion", desviaciones.IdTipoDesviacion);
            return View(desviaciones);
        }

        // GET: Desviaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Desviaciones desviaciones = db.Desviaciones.Find(id);

            DesviacionModel desviacion = new DesviacionModel {
                IdDesviacion = Convert.ToInt32(id),
                IdComponente = desviaciones.IdComponente,
                IdCliente = desviaciones.IdCliente,
                NumeroDesviacion = desviaciones.NumeroDesviacion,
                FechaDesviacion = Convert.ToDateTime(desviaciones.FechaDesviacion),
                RequeridoPor = desviaciones.RequeridoPor,
                DescripcionProcesoCorrecto = desviaciones.DescripcionProcesoCorrecto,
                DescripcionCondicionComponente = desviaciones.DescripcionCondicionComponente,
                DescripcionRazonDesviacion = desviaciones.DescripcionRazonDesviacion,
                DescripcionPlanDeAccion = desviaciones.DescripcionPlanDeAccion,
                DescripcionFechaDesviacion = desviaciones.DescripcionFechaDesviacion,
                DescripcionResponsable = desviaciones.DescripcionResponsable,
                CriterioAceptado = desviaciones.CriterioAceptado,
                DetalleCriterioAceptado = desviaciones.DetalleCriterioAceptado,
                Vencimiento = desviaciones.Vencimiento,
                FechaVencimiento = desviaciones.FechaVencimiento ?? new DateTime(),
                CantidadDePiezas = desviaciones.CantidadDePiezas,
                IdTipoDesviacion = desviaciones.IdTipoDesviacion,
                //ListaAreas = desviaciones.ResultadoDeAreaInvolucrada.Select(x => x.IdAreasInvolucradas).ToList(),
                //ListaDocumentos = desviaciones.ResultadoDeDocumentosRelacionados.Select(x => x.IdDocumentoRealizado).ToList()
                SelectListAreas = desviaciones.ResultadoDeAreaInvolucrada.Where(x=>x.EstadoArea==true).Select(x => x.IdAreasInvolucradas).ToArray(),
                SelectListDocumentos = desviaciones.ResultadoDeDocumentosRelacionados.Where(x=>x.EstadoDocumento==true).Select(x => x.IdDocumentoRealizado).ToArray()
            };
           
            if (desviacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCliente = new SelectList(db.Clientes.Where(x=>x.EstadoCliente==true), "IdCliente", "DescripcionDelCliente", desviacion.IdCliente);
            ViewBag.IdComponente = new SelectList(db.Componentes.Where(c=>c.EstadoComponente==true), "IdComponente", "NumeroParte", desviacion.IdComponente);
            ViewBag.IdTipoDesviacion = new SelectList(db.TipoDesviacion.Where(x=>x.EstadoTipoDesviacion==true), "IdTipoDesviacion", "DescripcionTipoDesviacion", desviacion.IdTipoDesviacion);
            ViewBag.ListaArea = db.AreaInvolucrada.Where(x => x.EstadoAreaInvolucrada).Select(x => new SelectListItem { Value = x.IdAreasInvolucradas.ToString(), Text = x.DescripcionAreaInvolucrada }).ToList();
            ViewBag.ListaDocumento = db.DocumentosRelacionados.Where(x => x.EstadoDocumentoRelacionado).Select(x => new SelectListItem { Value = x.IdDocumentosRelacionados.ToString(), Text = x.DescripcionDocumentosRelacionados }).ToList();
            return View(desviacion);
        }

        // POST: Desviaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DesviacionModel desviaciones)
        {
            if (ModelState.IsValid)
            {
                var nuevadesviacion = db.Desviaciones.Add(new Desviaciones
                {
                    IdDesviacion = desviaciones.IdDesviacion,
                    IdComponente = desviaciones.IdComponente,
                    IdCliente = desviaciones.IdCliente,
                    NumeroDesviacion = desviaciones.NumeroDesviacion,
                    FechaDesviacion = desviaciones.FechaDesviacion,
                    RequeridoPor = desviaciones.RequeridoPor,
                    DescripcionProcesoCorrecto = desviaciones.DescripcionProcesoCorrecto,
                    DescripcionCondicionComponente = desviaciones.DescripcionCondicionComponente,
                    DescripcionRazonDesviacion = desviaciones.DescripcionRazonDesviacion,
                    DescripcionPlanDeAccion = desviaciones.DescripcionPlanDeAccion,
                    DescripcionFechaDesviacion = desviaciones.DescripcionFechaDesviacion,
                    DescripcionResponsable = desviaciones.DescripcionResponsable,
                    CriterioAceptado = desviaciones.CriterioAceptado,
                    DetalleCriterioAceptado = desviaciones.DetalleCriterioAceptado,
                    Vencimiento = desviaciones.Vencimiento,
                    FechaVencimiento = desviaciones.FechaVencimiento,
                    CantidadDePiezas = desviaciones.CantidadDePiezas,
                    IdTipoDesviacion = desviaciones.IdTipoDesviacion,
                    CreadoPor = User.Identity.GetUserName(),
                    EstadoDesviacion = true
                });
                
                var lstArea = db.ResultadoDeAreaInvolucrada.Where(x => x.IdDesviaciones == desviaciones.IdDesviacion);
                foreach (var lst in lstArea)
                {
                    lst.EstadoArea = false;
                }
                //db.Entry(lstArea).State = EntityState.Modified;

                var lstDocumento = db.ResultadoDeDocumentosRelacionados.Where(x => x.IdDesviacion == desviaciones.IdDesviacion);
                foreach (var lst in lstDocumento)
                {
                    lst.EstadoDocumento = false;
                }
                //db.Entry(lstDocumento).State = EntityState.Modified;

                foreach (var area in desviaciones.ListaAreas)
                {
                    db.ResultadoDeAreaInvolucrada.Add(new ResultadoDeAreaInvolucrada
                    {
                        IdAreasInvolucradas = area,
                        IdDesviaciones = nuevadesviacion.IdDesviacion,
                        EstadoArea = true
                    });
                }
                foreach (var documento in desviaciones.ListaDocumentos)
                {
                    db.ResultadoDeDocumentosRelacionados.Add(new ResultadoDeDocumentosRelacionados
                    {
                        IdDocumentoRealizado = documento,
                        IdDesviacion = nuevadesviacion.IdDesviacion,
                        EstadoDocumento = true
                    });
                }

                db.Entry(nuevadesviacion).State = EntityState.Modified;
                var resultado = db.SaveChanges();
                if (resultado > 0)
                {
                    TempData["Message"] = "La desviacion se actualizo correctamente!";
                }
                else
                {
                    TempData["Message"] = "Error al actualizar la desviacion!";
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdCliente = new SelectList(db.Clientes, "IdCliente", "DescripcionDelCliente", desviaciones.IdCliente);
            ViewBag.IdComponente = new SelectList(db.Componentes, "IdComponente", "NumeroParte", desviaciones.IdComponente);
            ViewBag.IdTipoDesviacion = new SelectList(db.TipoDesviacion, "IdTipoDesviacion", "DescripcionTipoDesviacion", desviaciones.IdTipoDesviacion);
            return View(desviaciones);
        }

        // GET: Desviaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Desviaciones desviaciones = db.Desviaciones.Find(id);
            if (desviaciones == null)
            {
                return HttpNotFound();
            }
            return View(desviaciones);
        }

        // POST: Desviaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Desviaciones desviaciones = db.Desviaciones.Find(id);
            db.Desviaciones.Remove(desviaciones);
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

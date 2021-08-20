using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades;
using LogicaNegocio;

namespace Presentacion.Controllers
{
    public class ServicioController : Controller
    {
        ServicioNegocio sn = new();


        //Vista principal de Servicio
        public IActionResult Index()
        {
            var listaServicios = sn.Obtener();
            if (listaServicios == null)
            {
                return NotFound();
            }
            ViewBag.Servicios = listaServicios;
            return View();
        }



        //Guardar un Servicio
        [HttpPost]
        public IActionResult Create(Servicio servicio)
        {
            var vDato = sn.ObtenerByName(servicio.Descripcion);

            if (vDato == null)
            {
                if (ModelState.IsValid)
                {
                    if (sn.Guardar(servicio) == true)
                    {
                        TempData["msj"] = "Este servicio se ha registrado con éxito";
                       
                    }
                    return RedirectToAction("index");
                }
            }
            else
            {
                TempData["error"] = "No se puede guardar por que hay un servicio que tiene el mismo nombre";
            }
            return RedirectToAction("index");
        }


        //GET/ID
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var servicio = sn.ObtenerById(id);

            if (servicio == null)
            {
                return NotFound();
            }
            return View(servicio);
        }



        //Editar un servicio
        [HttpPost]
        public IActionResult Edit(int id, Servicio servicio)
        {

            if (id != servicio.IdServicio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (sn.Editar(servicio) == true)
                {
                    TempData["msj"] = "Información de servicio actualizado";
                }
                else
                {
                    TempData["error"] = "Error al editar";
                }
                return RedirectToAction(nameof(Index));
            }

            return View(servicio);
        }



        //GET/ID
        public IActionResult Delete(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            var vehiculo = sn.ObtenerById(id);

            if (vehiculo == null)
            {
                return NotFound();
            }
            return View(vehiculo);
        }



        //Eliminar un servicio
        [HttpPost]
        public IActionResult Delete(int id, Servicio servicio)
        {

            if (id != servicio.IdServicio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (sn.Elimiar(servicio) == true)
                {
                    TempData["msj"] = "Información de servicio eliminado";
                }
                else
                {
                    TempData["error"] = "Error al eliminar este servicio";
                }
                return RedirectToAction(nameof(Index));
            }

            return View(servicio);
        }
    }
}

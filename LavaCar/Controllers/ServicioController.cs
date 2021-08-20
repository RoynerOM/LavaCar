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


        //Guardar
        [HttpPost]
        //Guardar un vehiculo
        public IActionResult Create(Servicio servicio)
        {

            var vDato = sn.ObtenerById(servicio.IdServicio);

            if (vDato == null)
            {
                if (ModelState.IsValid)
                {
                    if (sn.Guardar(servicio) == true)
                    {
                        TempData["msj"] = "Este servicio se ha registrado con éxito";
                        return RedirectToAction("index");
                    }
                }
            }
            
            return View();
        }



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



        //Editar Vehiculos
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



        //Editar Vehiculos
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
                return RedirectToAction(nameof(Index));
            }

            return View(servicio);
        }
    }
}

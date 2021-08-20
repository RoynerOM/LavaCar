using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades;
using LogicaNegocio;

namespace Presentacion.Controllers
{
    public class ListaController : Controller
    {
        VehiculoServicioNegocio negocio = new();


        //Vista Principal de Lista de vehiculos asignados a un servicio
        public IActionResult Index(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var lista = negocio.ObtenerVehiculos().Where(x => x.IdServicioNavigation.IdServicio == id);
            
            return View(lista);
        }


        //GET/ID
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var data = negocio.ObtenerById(id);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }


        //Eliminar o finalizar un servicio
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var data = negocio.ObtenerById((int)id);

            if (data == null)
            {
                TempData["error"] = "No se pudo finalizar el sevicio";
                return NotFound();

            }

            if (ModelState.IsValid)
            {
                negocio.Eliminar(data);
                TempData["msj"] = "Servicio finalizado con exito";
                return Redirect("/Vehiculo/Index");
            }
            var lista = negocio.ObtenerVehiculos().Where(x => x.IdServicioNavigation.IdServicio == data.IdServicio);
            return View("Index", lista);
        }
    }
}
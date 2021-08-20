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

        public IActionResult Index(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var lista = negocio.ObtenerVehiculos().Where(x => x.IdServicioNavigation.IdServicio == id);

            return View(lista);
        }



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



        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var data = negocio.ObtenerById((int)id);

            if (data == null)
            {
                TempData["error"] = null;
                TempData["msj"] = "No se pudo finalizar el sevicio";
                return NotFound();

            }

            if (ModelState.IsValid)
            {
                negocio.Eliminar(data);
                TempData["error"] = 1;
                TempData["msj"] = "Servicio finalizado con exito";
                return RedirectToAction("Index",data.IdServicio);
            }
            return View();
        }
    }
}
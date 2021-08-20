using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogicaNegocio;
using Entidades;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentacion.Controllers
{
    public class VehiculoController : Controller
    {

        VehiculoNegocio vehiculoNegocio = new();
        VehiculoServicioNegocio vehiculoServicio = new();

        //Vista principal de Vehiculos
        public IActionResult Index()
        {
            var listaServicios = vehiculoNegocio.ObtenerServicios();
            ViewBag.lista = listaServicios;
            ViewBag.Servicio = new SelectList(listaServicios, "IdServicio", "Descripcion");
            return View();
        }


        //Lista de los vehiculos
        public IActionResult ListarVehiculos()
        {
            var listaVehiculos = vehiculoNegocio.Obtener();

            if (listaVehiculos == null)
            {
                return NotFound();
            }


            return View(listaVehiculos);
        }



        [HttpPost]
        //Guardar un vehiculo
        public IActionResult Create(VehiculoModel vehiculo)
        {
            Vehiculo v = new();
            v.Placa = vehiculo.Placa;
            v.Dueno = vehiculo.Dueno;
            v.Marca = vehiculo.Marca;


            var vDato = vehiculoNegocio.ObtenerByPlaca(vehiculo.Placa);

            if (vDato == null)
            {
                if (ModelState.IsValid)
                {
                    if (vehiculoNegocio.Guardar(v) == true && vehiculoServicio.Guardar(v, vehiculo.IdServicio))
                    {
                        TempData["msj"] = "Este vehículo se ha registrado con éxito";
                       
                    }
                    else
                    {
                        TempData["error"] = "Error al eliminar información de este vehículo";
                    }
                    return RedirectToAction("index");
                }
            }
            else
            {
                vehiculoServicio.Guardar(vDato, vehiculo.IdServicio);
                TempData["warning"] = "Este vehículo ya ha sido registrado, por ende se ha asignado un nuevo servicio";
                return RedirectToAction("index");
            }



            return View();
        }



        //GET/ID
        public IActionResult Edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            var vehiculo = vehiculoNegocio.ObtenerById(id);

            if (vehiculo == null)
            {
                return NotFound();
            }
            return View(vehiculo);
        }



        //Editar Vehiculos
        [HttpPost]
        public IActionResult Edit(int id, Vehiculo vehiculo)
        {

            if (id != vehiculo.IdVehiculo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (vehiculoNegocio.Editar(vehiculo) == true)
                {
                    TempData["msj"] = "Informacion de vehiculo actualizado";
                }
                else
                {
                    TempData["error"] = "Error al editar";
                }
                return RedirectToAction(nameof(ListarVehiculos));
            }

            return View(vehiculo);
        }



        //GET/ID
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var vehiculo = vehiculoNegocio.ObtenerById(id);

            if (vehiculo == null)
            {
                return NotFound();
            }
            return View(vehiculo);
        }



        //Editar Vehiculos
        [HttpPost]
        public IActionResult Delete(int id, Vehiculo vehiculo)
        {
            if (id != vehiculo.IdVehiculo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (vehiculoNegocio.Elimiar(vehiculo) == true)
                {
                    TempData["msj"] = "Información de vehículo eliminado";
                }
                else
                {
                    TempData["error"] = "Error al eliminar";
                }
                return RedirectToAction(nameof(ListarVehiculos));
            }

            return View(vehiculo);
        }
    }
}

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
        IEnumerable<VehiculoServicio> itemVehiculos;
        int RegisPorPagina = 10;
        Paginador<VehiculoServicio> paginador;

        //Vista Principal de Lista de vehiculos asignados a un servicio
        public IActionResult Index(int id, int pagina = 1, string buscar = null)
        {
            int _TotalRegistros = 0;

            //Ontengo la lista de un servicio relacionado a un vehiculo
           var lista = negocio.ObtenerVehiculos().Where(x => x.IdServicioNavigation.IdServicio == id); ;
    
              //Cantidad de registros 
            _TotalRegistros = lista.Count();

            //Divide las datos en grupos de de 10
            itemVehiculos = lista.OrderBy(x => x.IdVehiculoServicio)
                                        .Skip((pagina - 1) * RegisPorPagina)
                                        .Take(RegisPorPagina).ToList();

            //Calculo de las paginas
            var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / RegisPorPagina);

            //Filtro por placa o Dueno
            if (!string.IsNullOrEmpty(buscar))
            {
                //Elimino los espacios vacios de la cadena
                foreach (var item in buscar.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    //Recargo los servicios
                    itemVehiculos = negocio.ObtenerVehiculos().OrderBy(x => x.IdVehiculoServicio)
                                        .Where(x => x.IdVehiculoNavigation.Dueno.Contains(item) ||
                                                        x.IdVehiculoNavigation.Placa.Contains(item)).ToList();
                }
                //Re calcula la cantidad de registros
                _TotalRegistros = itemVehiculos.Count();
            }

           
            if (lista == null)
            {
                return NotFound();
            }


            paginador = new Paginador<VehiculoServicio>()
            {
                RegistrosPorPagina = RegisPorPagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = itemVehiculos,
                Input = new VehiculoServicio()
            };
            return View(paginador);
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
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
        IEnumerable<Vehiculo> itemVehiculos;
        int RegisPorPagina = 10;
        Paginador<Vehiculo> paginador;


        public IActionResult Index()
        {
            var listaServicios = vehiculoNegocio.ObtenerServicios();
            ViewBag.lista = listaServicios;
            ViewBag.Servicio = new SelectList(listaServicios, "IdServicio", "Descripcion");
            return View();
        }


        //Lista de los vehiculos
        public IActionResult ListarVehiculos(int pagina = 1, string buscar = null)
        {
            int _TotalRegistros = 0;

            //Se obtiene la lista de los vehiculos 
            var listaVehiculos = vehiculoNegocio.Obtener();


            //Se cuentan los registros
            _TotalRegistros = listaVehiculos.Count();


            //Se hace filtro de limite de datos maximo 10 por pagina
            itemVehiculos = listaVehiculos.OrderBy(x => x.IdVehiculo)
                                            .Skip((pagina - 1) * RegisPorPagina)
                                            .Take(RegisPorPagina).ToList();

            //Se calcula la cantidad de paginas
            var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / RegisPorPagina);


            if (!string.IsNullOrEmpty(buscar))
            {
                //Elimino los espacios vacios de la cadena
                foreach (var item in buscar.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    //Recargo los vehiculos
                    itemVehiculos = vehiculoNegocio.Obtener().OrderBy(x => x.IdVehiculo)
                                        .Where(x => x.Dueno.Contains(item) ||
                                                        x.Placa.Contains(item)).ToList();
                }
                //Re calcula la cantidad de registros
                _TotalRegistros = itemVehiculos.Count();
            }
            if (listaVehiculos == null)
            {
                return NotFound();
            }

            //Se cargar el objeto paginador
            paginador = new Paginador<Vehiculo>()
            {
                RegistrosPorPagina = RegisPorPagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = itemVehiculos
            };

            return View(paginador);
        }



        [HttpPost]
        //Guardar un vehiculo
        public IActionResult Create(VehiculoModel vehiculo)
        {
            //Se carga el modelo
            Vehiculo v = new();
            v.Placa = vehiculo.Placa;
            v.Dueno = vehiculo.Dueno;
            v.Marca = vehiculo.Marca;


            //Se busca el vehiculo por numero de placa
            var vDato = vehiculoNegocio.ObtenerByPlaca(vehiculo.Placa);


            //En caso de que sea null se hace un nuevo registro
            if (vDato == null)
            {
                //Se verifica si el modelo es valido
                if (ModelState.IsValid)
                {
                    //Se guarda un nuevo vehiculo y con su respectivo servicio
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
                //En caso de que de ya este registrado se le asignara un nuevo servicio
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

            //Se busca el vehiculo por su id
            var vehiculo = vehiculoNegocio.ObtenerById(id);

            if (vehiculo == null)
            {
                return NotFound();
            }
            //Se devuelve a la vista
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

            //Se verifica si el modelo es valido
            if (ModelState.IsValid)
            {
                //Se manda a editar el vehiculo
                if (vehiculoNegocio.Editar(vehiculo) == true)
                {
                    TempData["msj"] = "Informacion de vehiculo actualizado";
                }
                else
                {
                    TempData["error"] = "Error al editar";
                }
                //Redirecciona a la vista ListarVehiculos
                return RedirectToAction(nameof(ListarVehiculos));
            }
            //Se devuelve la vista con su modelo
            return View(vehiculo);
        }



        //GET/ID
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            //Se busca el vehiculo por su id
            var vehiculo = vehiculoNegocio.ObtenerById(id);

            if (vehiculo == null)
            {
                return NotFound();
            }
            //Se devuelve a la vista
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

            //Se verifica si el modelo es valido
            if (ModelState.IsValid)
            {
                //Se eliminar el vehiculo
                if (vehiculoNegocio.Elimiar(vehiculo) == true)
                {
                    TempData["msj"] = "Información de vehículo eliminado";
                }
                else
                {
                    TempData["error"] = "Error al eliminar";
                }
                //Redirecciona a la vista ListarVehiculos
                return RedirectToAction(nameof(ListarVehiculos));
            }

            return View(vehiculo);
        }
    }
}

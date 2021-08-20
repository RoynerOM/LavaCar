using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos
{
    public class VehiculoServicioDatos
    {

        //Obtener las lista
        public IEnumerable<VehiculoServicio> ObtenerVehiculos()
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    var listaVehiculos = db.VehiculoServicios.Include(x => x.IdServicioNavigation).Include(x => x.IdVehiculoNavigation).ToList();



                    if (listaVehiculos != null)
                    {
                        return listaVehiculos;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }



        //Eliminar la relacion de servicio con vehiculo
        public bool Eliminar(VehiculoServicio id)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    db.Remove(id);
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return false;
            }
        }



        ///Funcion de guardar
        public bool Guardar(Vehiculo v, int idServicio)
        {
            try
            {

                using (var db = new LavaCarDbContext())
                {
                    var vehiculo = db.Vehiculos.Where(x => x == v).SingleOrDefault();

                    VehiculoServicio vs = new();
                    vs.IdServicio = idServicio;
                    vs.IdVehiculo = vehiculo.IdVehiculo;

                    db.VehiculoServicios.Add(vs);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return false;
            }

        }


        //Buscar por id
        public VehiculoServicio ObtenerById(int id)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    var listaVehiculos = db.VehiculoServicios.
                                        Include(x => x.IdServicioNavigation).
                                        Include(x => x.IdVehiculoNavigation).
                                        Where(x => x.IdVehiculoServicio == id).SingleOrDefault();


                    if (listaVehiculos != null)
                    {
                        return listaVehiculos;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }


        //Buscar las relaciones de vehiculos con servicios
        public IEnumerable<VehiculoServicio> ObtenerRelaciones(int id)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    var relaciones = db.VehiculoServicios
                                    .Include(x => x.IdServicioNavigation)
                                    .Include(x => x.IdVehiculoNavigation)
                                    .Where(x => x.IdVehiculo == id).ToList();

                    if (relaciones != null)
                    {
                        return relaciones;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}

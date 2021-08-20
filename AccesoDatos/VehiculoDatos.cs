using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos
{
    public class VehiculoDatos
    {

        //Listar un vehiculo
        public IEnumerable<Vehiculo> Obtener()
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    return db.Vehiculos.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        //Guardar un vehiculo
        public bool Guardar(Vehiculo vehiculo)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    db.Vehiculos.Add(vehiculo);
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



        //Eliminar un vehiculo
        public bool Eliminar(Vehiculo vehiculo)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    //Buscar todas las relaciones
                    var relaciones = db.VehiculoServicios
                                   .Include(x => x.IdServicioNavigation)
                                   .Include(x => x.IdVehiculoNavigation)
                                   .Where(x => x.IdVehiculoNavigation.IdVehiculo == vehiculo.IdVehiculo).ToList();

                    if (relaciones.Count() > 0)
                    {
                        foreach (var item in relaciones)
                        {
                            db.Entry<VehiculoServicio>(item).State = EntityState.Deleted;
                        }
                    }
                    db.SaveChanges();
                    //db.Vehiculos.Remove(vehiculo);
                    db.Entry<Vehiculo>(vehiculo).State = EntityState.Deleted;
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
        public Vehiculo ObtenerById(int id)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    var listaVehiculos = db.Vehiculos.Where(x => x.IdVehiculo == id).SingleOrDefault();


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



        //Buscar por numero de placa
        public Vehiculo ObtenerByPlaca(string placa)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    var listaVehiculos = db.Vehiculos.Where(x => x.Placa == placa).SingleOrDefault();


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

        //Editar
        public bool Editar(Vehiculo vehiculo)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    db.Vehiculos.Update(vehiculo);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}

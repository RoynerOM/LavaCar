using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos
{
   public class ServicioDatos
    {
        public  IEnumerable<Servicio> Obtener()
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    return db.Servicios.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Guardar(Servicio servicio)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    db.Servicios.Add(servicio);
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
        public bool Eliminar(Servicio servicio)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    //Buscar todas las relaciones
                    var relaciones = db.VehiculoServicios
                                   .Include(x => x.IdServicioNavigation)
                                   .Include(x => x.IdVehiculoNavigation)
                                   .Where(x => x.IdServicio == servicio.IdServicio).ToList();

                    if (relaciones.Count() > 0)
                    {
                        foreach (var item in relaciones)
                        {
                            db.Entry<VehiculoServicio>(item).State = EntityState.Deleted;
                        }  
                    }

                    db.SaveChanges();
                    db.Entry<Servicio>(servicio).State = EntityState.Deleted;
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
        public Servicio ObtenerById(int id)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    var lista = db.Servicios.Where(x => x.IdServicio == id).SingleOrDefault();


                    if (lista != null)
                    {
                        return lista;
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


        //Editar un servicio
        public bool Editar(Servicio servicio)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    db.Servicios.Update(servicio);
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


        //Buscar por nombre
        public Servicio ObtenerByName(string name)
        {
            try
            {
                using (var db = new LavaCarDbContext())
                {
                    var dato = db.Servicios.Where(x => x.Descripcion == name).SingleOrDefault();

                    if (dato != null)
                    {
                        return dato;
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

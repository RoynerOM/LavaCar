using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using AccesoDatos;

namespace LogicaNegocio
{
   public class VehiculoServicioNegocio
    {
        VehiculoServicioDatos vehiculoServicio = new();

        //Obtener lista
        public IEnumerable<VehiculoServicio> ObtenerVehiculos()
        {
            return vehiculoServicio.ObtenerVehiculos();
        }



        //Eliminar
        public bool Eliminar(VehiculoServicio id)
        {
            return vehiculoServicio.Eliminar(id);
        }



        //Guardar
        public bool Guardar(Vehiculo v, int idVehiculo)
        {
            return vehiculoServicio.Guardar(v, idVehiculo);
        }



        public VehiculoServicio ObtenerById(int id)
        {
            return vehiculoServicio.ObtenerById(id);
        }
    }
}

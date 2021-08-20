using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using Entidades;
namespace LogicaNegocio
{
   public class VehiculoNegocio
    {
        VehiculoDatos dv = new();
        ServicioDatos sd = new();

        public IEnumerable<Vehiculo> Obtener()
        {
            return dv.Obtener();
        }


        public IEnumerable<Servicio> ObtenerServicios()
        {
            return sd.Obtener();
        }


        public bool Guardar(Vehiculo vehiculo)
        {
            return dv.Guardar(vehiculo);
        }


        public Vehiculo ObtenerByPlaca(string placa)
        {
            return dv.ObtenerByPlaca(placa);
        }


        public Vehiculo ObtenerById(int id)
        {
            return dv.ObtenerById(id);
        }


        public bool Editar(Vehiculo vehiculo)
        {
            return dv.Editar(vehiculo);
        }


        public bool Elimiar(Vehiculo vehiculo)
        {
            return dv.Eliminar(vehiculo);
        }

        public IEnumerable<Vehiculo> ObtenerVehiculos(string dueno)
        {
            if (dueno == null)
            {
                return dv.Obtener();
            }
            else
            {
                return dv.Obtener().Where(x => x.Dueno == dueno);
            }
        }
    }
}

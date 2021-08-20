using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using AccesoDatos;
namespace LogicaNegocio
{
  
   public class ServicioNegocio
    {
        ServicioDatos sn = new();

        public IEnumerable<Servicio> Obtener()
        {
            return sn.Obtener();
        }


        public bool Guardar(Servicio servicio)
        {
            return sn.Guardar(servicio);
        }


        public Servicio ObtenerById(int id)
        {
            return sn.ObtenerById(id);
        }


        public bool Editar(Servicio servicio)
        {
            return sn.Editar(servicio);
        }


        public bool Elimiar(Servicio servicio)
        {
            return sn.Eliminar(servicio);
        }
    }
}


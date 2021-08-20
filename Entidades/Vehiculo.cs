using System;
using System.Collections.Generic;

#nullable disable

namespace Entidades
{
    public partial class Vehiculo
    {
        public Vehiculo()
        {
            VehiculoServicios = new HashSet<VehiculoServicio>();
        }

        public int IdVehiculo { get; set; }
        public string Placa { get; set; }
        public string Dueno { get; set; }
        public string Marca { get; set; }

        public virtual ICollection<VehiculoServicio> VehiculoServicios { get; set; }
    }
}

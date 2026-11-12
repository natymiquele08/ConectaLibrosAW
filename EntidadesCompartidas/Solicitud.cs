using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesCompartidas
{
    public class Solicitud
    {
        public int ID { get; set; }
        public DateTime FechaActual { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int Cantidad { get; set; }

        public Solicitud() { }

        public Solicitud(int id, DateTime fechaActual, DateTime fechaEntrega, int cantidad)
        {
            ID = id;
            FechaActual = fechaActual;
            FechaEntrega = fechaEntrega;
            Cantidad = cantidad;
            Validar();
        }

        public void Validar()
        {
            if (ID <0)
                throw new Exception("El ID no puede ser negativo");
            if (FechaEntrega < FechaActual.AddHours(24))
                throw new Exception("La fecha de entrega no puede ser menor a la fecha actual");
            if (Cantidad <= 0)
                throw new Exception("La cantidad no puede ser menor o igual a cero");
        }

    }
}

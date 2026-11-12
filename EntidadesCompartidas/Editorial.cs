using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesCompartidas
{
    public class Editorial
    {
        public string NombreEditorial { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public int Antiguedad { get; set; }

        public Editorial() { }

        public Editorial(string nombreEditorial, string direccion, string ciudad, int antiguedad)
        {
            NombreEditorial = nombreEditorial;
            Direccion = direccion;
            Ciudad = ciudad;
            Antiguedad = antiguedad;
            Validar();
        }

        //validar
        public void Validar()
        {
            if (string.IsNullOrEmpty(NombreEditorial))
                throw new Exception("El nombre de la editorial no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(Direccion))
                throw new Exception("La direccion no puede ser nula o vacia");
            if (string.IsNullOrEmpty(Ciudad))
                throw new Exception("La ciudad no puede ser nula o vacia");
            if (Antiguedad < 0)
                throw new Exception("La antiguedad no puede ser negativa");
        }
    }
}

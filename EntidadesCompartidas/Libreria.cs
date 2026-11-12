using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesCompartidas
{
    public class Libreria
    {
        public string RUT { get; set; }
        public string NombreLibreria { get; set; }
        public string DireccionLibreria { get; set; }
        public string CiudadLibreria { get; set; }
        public string TelefonoLibreria { get; set; }

        public Libreria() { }

        public Libreria(string rut, string nombreLibreria, string direccionLibreria, string ciudadLibreria, string telefonoLibreria)
        {
            RUT = rut;
            NombreLibreria = nombreLibreria;
            DireccionLibreria = direccionLibreria;
            CiudadLibreria = ciudadLibreria;
            TelefonoLibreria = telefonoLibreria;
            Validar();
        }

        //validar
        public void Validar()
        {
            if (string.IsNullOrEmpty(RUT) || RUT.Length != 12)
                throw new Exception("El RUT no puede ser nulo o vacio y debe tener 12 digitos");
            if (string.IsNullOrEmpty(NombreLibreria))
                throw new Exception("El nombre de la libreria no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(DireccionLibreria))
                throw new Exception("La direccion de la libreria no puede ser nula o vacia");
            if (string.IsNullOrEmpty(CiudadLibreria))
                throw new Exception("La ciudad de la libreria no puede ser nula o vacia");
            if (string.IsNullOrEmpty(TelefonoLibreria))
                throw new Exception("El telefono de la libreria no puede ser nulo o vacio");
        }

    }
}

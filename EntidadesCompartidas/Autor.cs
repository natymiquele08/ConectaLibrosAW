using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesCompartidas
{
    public class Autor
    {
        public string CodigoInternoAutor { get; set; }
        public string NombreAutor    { get; set; }
        public DateTime FechaNacimiento { get; set; }

        //Constructores
        //por defecto
        public Autor() { }

        //completo
        public Autor(string codigoInterno, string nombreAutor, DateTime fechaNacimiento)
        {
            CodigoInternoAutor = codigoInterno;
            NombreAutor = nombreAutor;
            FechaNacimiento = fechaNacimiento;
            Validar();
        }

        //operacion validar
        public void Validar()
        {
            if (string.IsNullOrEmpty(CodigoInternoAutor))
                throw new Exception("El codigo interno no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(NombreAutor))
                throw new Exception("El nombre del autor no puede ser nulo o vacio");
            if (FechaNacimiento > DateTime.Now.AddYears(-18))
                throw new Exception("Tiene que ser mayor de edad");
        }


    }
}

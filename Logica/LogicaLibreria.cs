using EntidadesCompartidas;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaLibreria
    {
        public static List<Libreria> ListadoLibreria() => PersistenciaLibreria.ListadoLibreria();

        public static Libreria BuscarLibreria(string pRut)
        {
            Libreria libreria = PersistenciaLibreria.BuscarLibreria(pRut);

            return libreria;

        }

        public static void Agregar(Libreria pLibreria)
        {
            if (pLibreria is null)
                throw new Exception("Debe ingresar una libreria");

            else
                PersistenciaLibreria.Agregar(pLibreria);
        }

        public static void Modificar(Libreria pLibreria)
        {
            if (pLibreria is null)
                throw new Exception("Debe indicar una libreria para modificar");
            else
                PersistenciaLibreria.Modificar(pLibreria);
        }


    }

}

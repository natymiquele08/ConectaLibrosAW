using EntidadesCompartidas;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaAutor
    {
        public static List<Autor> ListadoAutores() => PersistenciaAutor.ListadoAutores();

        public static Autor BuscarAutor(string pCodigoInternoAutor)
        {
            Autor autor = PersistenciaAutor.BuscarAutor(pCodigoInternoAutor);

            return autor;
           
        }

        public static void Agregar(Autor pAutor)
        {
            if (pAutor is null)
                throw new Exception("Debe ingresar un autor");

            else
                PersistenciaAutor.Agregar(pAutor);   
        }

        public static void Modificar(Autor pAutor)
        {
            if (pAutor is null)
                throw new Exception("Debe indicar un autor para modificar");
            else
                PersistenciaAutor.Modificar(pAutor);
        }

        public static void Eliminar(Autor pAutor)
        {
            if (pAutor is null)
                throw new Exception("Debe indicar un autor para eliminar");
            else
                PersistenciaAutor.Eliminar(pAutor);
        }

    }
}

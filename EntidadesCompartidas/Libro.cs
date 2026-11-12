using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesCompartidas
{
    public class Libro
    {
        public string ISBN { get; set; }
        public string NombreLibro { get; set; }
        public string Resenia { get; set; }
        public string Genero { get; set; }
        public int StockActual { get; set; }

        public Libro() { }

        public Libro(string isbn, string nombreLibro, string resenia, string genero, int stockActual)
        {
            ISBN = isbn;
            NombreLibro = nombreLibro;
            Resenia = resenia;
            Genero = genero;
            StockActual = stockActual;
            Validar();
        }

        public void Validar()
        {
            if (string.IsNullOrEmpty(ISBN))
                throw new Exception("El ISBN no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(NombreLibro))
                throw new Exception("El nombre del libro no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(Resenia))
                throw new Exception("La reseña del libro no puede ser nula o vacia");
            if (string.IsNullOrEmpty(Genero))
                throw new Exception("El género del libro no puede ser nulo o vacio");
            if (StockActual < 0)
                throw new Exception("El stock actual no puede ser negativo");
        }

    }
}

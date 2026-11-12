using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntidadesCompartidas;

namespace Persistencia
{
    public class PersistenciaAutor
    {
        //Devolver una coleccion 
        public static List<Autor> ListadoAutores()
        {
            List<Autor> colAutores = new List<Autor>();

            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("ListadoAutores", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            try
            {
                oConexion.Open();
                SqlDataReader oReader = oComando.ExecuteReader();

                if (oReader.HasRows)
                {
                    while (oReader.Read())
                    {
                        Autor oAutor = new Autor(
                            oReader["CodigoInternoAutor"].ToString(),
                            oReader["NombreA"].ToString(),
                            Convert.ToDateTime(oReader["FechaNacimiento"])
                            );

                        colAutores.Add(oAutor);
                    }
                }
                oReader.Close();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                oConexion.Close();
            }

            return colAutores;
        }

        //Buscar un autor por su codigo interno
        public static Autor BuscarAutor(string pCodigoInternoAutor)
        {
            Autor oAutor = null;

            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("BuscarAutor", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@CodigoInternoAutor", pCodigoInternoAutor);

            try
            {
                oConexion.Open();
                SqlDataReader oReader = oComando.ExecuteReader();

                if (oReader.HasRows)
                {
                    if (oReader.Read())
                    {
                        oAutor = new Autor(
                            oReader["CodigoInternoAutor"].ToString(),
                            oReader["NombreA"].ToString(),
                            Convert.ToDateTime(oReader["FechaNacimiento"])
                            );
                    }
                }
                oReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oConexion.Close();
            }
            return oAutor;
        }

        //Agregar un autor
        public static void Agregar(Autor pAutor)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("AgregarAutor", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@CodigoInternoAutor", pAutor.CodigoInternoAutor);
            oComando.Parameters.AddWithValue("@NombreA", pAutor.NombreAutor);
            oComando.Parameters.AddWithValue("@FechaNacimiento", pAutor.FechaNacimiento);

            //retorno
            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);

            try
            {
                
                oConexion.Open();
                oComando.ExecuteNonQuery(); 

                int resultado = Convert.ToInt32(oRetorno.Value);


                
                if (resultado == -1)
                    throw new Exception("El codigo de autor ya existe");
                else if (resultado == -2)
                    throw new Exception("Ocurrio un error inesperado");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oConexion.Close();
            }


        }

        //Modificar un autor
        public static void Modificar(Autor pAutor)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("ModificarAutor", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@CodigoInternoAutor", pAutor.CodigoInternoAutor);
            oComando.Parameters.AddWithValue("@NombreA", pAutor.NombreAutor);
            oComando.Parameters.AddWithValue("@FechaNacimiento", pAutor.FechaNacimiento);

            //retorno
            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);

            try
            {
                
                oConexion.Open();
                oComando.ExecuteNonQuery(); 
                int resultado = Convert.ToInt32(oRetorno.Value);



                if (resultado == -1)
                    throw new Exception("No existe el autor a modificar");
                else if (resultado == -2)
                    throw new Exception("El autor debe ser mayor de edad.");
                else if (resultado == -3)
                    throw new Exception("Ocurrió un problema al modificar el autor");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oConexion.Close();
            }
        }

        //Eliminar un autor
        public static void Eliminar(Autor pAutor)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("EliminarAutor", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@CodigoInternoAutor", pAutor.CodigoInternoAutor);

            //retorno
            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);

            try
            {
                
                oConexion.Open();
                oComando.ExecuteNonQuery(); 
                int resultado = Convert.ToInt32(oRetorno.Value);

                if (resultado == -1)
                    throw new Exception("No existe el autor a eliminar");
                else if (resultado == -2)
                    throw new Exception("No se puede eliminar porque tiene libros asociados");
                else if (resultado == -3)
                    throw new Exception("No se pudo eliminar. Ocurrio un error inesperado");
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);

            }
            finally
            {
                oConexion.Close();


            }
        }

    }
}

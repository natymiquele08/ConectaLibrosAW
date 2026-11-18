using EntidadesCompartidas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class PersistenciaLibreria
    {
        //CRUD LIBRERIAS
        //1) LISTADO LIBRERIAS
        public static List<Libreria> ListadoLibreria()
        {
            List<Libreria> colLibreria = new List<Libreria>();

            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("ListadoLibrerias", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            try
            {
                oConexion.Open();
                SqlDataReader oReader = oComando.ExecuteReader();

                if (oReader.HasRows)
                {
                    while (oReader.Read())
                    {
                        Libreria oLibreria = new Libreria(
                            oReader["RUTLibrerias"].ToString(),
                            oReader["NombreLibreria"].ToString(),
                            oReader["DireccionL"].ToString(),
                            oReader["CiudadL"].ToString(),
                            oReader["Telefono"].ToString()
                            );

                        colLibreria.Add(oLibreria);
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

            return colLibreria;
        }

        //Buscar una libreria por su codigo interno
        public static Libreria BuscarLibreria(string pRut)
        {
            Libreria oLibreria = null;

            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("BuscarLibreria", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@RUTLibrerias", pRut);

            try
            {
                oConexion.Open();
                SqlDataReader oReader = oComando.ExecuteReader();

                if (oReader.HasRows)
                {
                    if (oReader.Read())
                    {
                        oLibreria = new Libreria(
                            oReader["RUTLibrerias"].ToString(),
                            oReader["NombreLibreria"].ToString(),
                            oReader["DireccionL"].ToString(),
                            oReader["CiudadL"].ToString(),
                            oReader["Telefono"].ToString()
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
            return oLibreria;
        }

        //Agregar libreria
        public static void Agregar(Libreria pLibreria)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("AgregarLibreria", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@RUTLibrerias", pLibreria.RUT);
            oComando.Parameters.AddWithValue("@NombreLibreria", pLibreria.NombreLibreria);
            oComando.Parameters.AddWithValue("@DireccionL", pLibreria.DireccionLibreria);
            oComando.Parameters.AddWithValue("@CiudadL", pLibreria.CiudadLibreria);
            oComando.Parameters.AddWithValue("@Telefono", pLibreria.TelefonoLibreria);

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
                    throw new Exception("Ya existe esa libreria");
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

        //Modificar
        public static void Modificar(Libreria pLibreria)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("ModificarLibreria", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@RUTLibrerias", pLibreria.RUT);
            oComando.Parameters.AddWithValue("@NombreLibreria", pLibreria.NombreLibreria);
            oComando.Parameters.AddWithValue("@DireccionL", pLibreria.DireccionLibreria);
            oComando.Parameters.AddWithValue("@CiudadL", pLibreria.CiudadLibreria);
            oComando.Parameters.AddWithValue("@Telefono", pLibreria.TelefonoLibreria);

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
                    throw new Exception("No existe la libreria a modificar");
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


    }
}

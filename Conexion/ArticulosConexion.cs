using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Precentacion;
using System.Data.SqlClient;

namespace Conexion
{
    public class ArticulosConexion
    {
        public List<Articulos> listar()
        {
            List<Articulos> lista = new List<Articulos>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select Codigo, Nombre, A.Descripcion, M.Descripcion Marca,C.Descripcion Categoria, IdMarca, IdCategoria, ImagenUrl, Precio,A.Id from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = A.IdMarca and C.Id = A.IdCategoria";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = (int)lector["Id"];
                    aux.CodigoArticulo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];
                    aux.Marca = new Marcas();
                    aux.Marca.Id = (int)lector["IdMarca"];
                    aux.Marca.Descripcion = (string)lector["Marca"];
                    aux.Categoria = new Categorias();
                    aux.Categoria.Id = (int)lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)lector["Categoria"];
                    aux.Imagen = (string)lector["ImagenUrl"];
                    aux.Precio = (decimal)lector["Precio"];

                    lista.Add(aux);
                }
                return lista;
                conexion.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void agregar(Articulos nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into ARTICULOS (Codigo,Nombre,Descripcion,IdMarca,IdCategoria,ImagenUrl,Precio) values (@codigo,@nombre,@descripcion,@marca,@categoria,@imagen,@precio)");
                datos.setearParametros("@codigo", nuevo.CodigoArticulo);
                datos.setearParametros("@nombre", nuevo.Nombre);
                datos.setearParametros("@descripcion", nuevo.Descripcion);
                datos.setearParametros("@marca", nuevo.Marca.Id);
                datos.setearParametros("@categoria", nuevo.Categoria.Id);
                datos.setearParametros("@imagen", nuevo.Imagen);
                datos.setearParametros("@precio", nuevo.Precio);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void modificar(Articulos nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion,IdMarca = @idmarca,IdCategoria = @idcategoria, ImagenUrl = @imagen, Precio = @precio where Id = @id");
                datos.setearParametros("@codigo", nuevo.CodigoArticulo);
                datos.setearParametros("@nombre", nuevo.Nombre);
                datos.setearParametros("@descripcion", nuevo.Descripcion);
                datos.setearParametros("@idmarca", nuevo.Marca.Id);
                datos.setearParametros("@idcategoria", nuevo.Categoria.Id);
                datos.setearParametros("@imagen", nuevo.Imagen);
                datos.setearParametros("@precio", nuevo.Precio);
                datos.setearParametros("@id", nuevo.Id);

                datos.ejecutarAccion();
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

           
        }   
        public void eliminar(int Id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from ARTICULOS where id = @id");
                datos.setearParametros("Id", Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Articulos> filtrar(string campo, string criterio, string valor)
        {
            List<Articulos> lista = new List<Articulos>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = ("select Codigo, Nombre, A.Descripcion, M.Descripcion Marca,C.Descripcion Categoria, IdMarca, IdCategoria, ImagenUrl, Precio,A.Id from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = A.IdMarca and C.Id = A.IdCategoria and ");
                if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "Nombre like '" + valor + "%'";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + valor + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + valor + "%'";
                            break;
                    }
                }
                else if(campo == "Descripcion")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "A.Descripcion like '" + valor + "%'";
                            break;
                        case "Termina con":
                            consulta += "A.Descripcion like '%" + valor + "'";
                            break;
                        default:
                            consulta += "A.Descripcion like '%" + valor + "%'";
                            break;
                    }
                }else if(campo == "Codigo")
                {
                    switch (criterio)
                    {
                        case "Primer Valor":
                            consulta += "Codigo  like '" + valor + "%'";
                            break;
                    }
                }
                        
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodigoArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Marca = new Marcas();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categorias();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Imagen = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
    }
}

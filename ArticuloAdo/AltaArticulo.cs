using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Conexion;
using Precentacion;

namespace ArticuloAdo
{
    public partial class AltaArticulo : Form
    {
        private Articulos aux = null;
        private Articulos articulos = null;
        public AltaArticulo()
        {
            InitializeComponent();
        }
        public AltaArticulo(Articulos articulo)
        {
            InitializeComponent();
            this.articulos = articulo;
            Text = "Modificar Articulo";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticulosConexion conexion = new ArticulosConexion();
            try
            {

                if (articulos == null)

                    articulos = new Articulos();
                articulos.CodigoArticulo = txtCodigo.Text;
                articulos.Nombre = txtNombre.Text;
                articulos.Descripcion = txtDescripcion.Text;
                articulos.Imagen = txtImagen.Text;
                articulos.Precio = decimal.Parse(txtPrecio.Text);
                articulos.Marca = (Marcas)cboMarca.SelectedItem;
                articulos.Categoria = (Categorias)cboCategoria.SelectedItem;

                if (articulos.Id != 0)
                {
                    conexion.modificar(articulos);
                    MessageBox.Show("Modificado Exitosamente");
                }
                else
                {
                    
                    conexion.agregar(articulos); 
                    MessageBox.Show("Agregado Exitosamente");

                    
                }
                Close();
            }
            catch (FormatException)
            {

                MessageBox.Show("Ingrese los datos correspondientes porfavor");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
           
            
        }
        
        public void AltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaConexion conexion = new MarcaConexion();
            CategoriaConexion conexionC = new CategoriaConexion();
            
            try
            {
                
                cboMarca.DataSource = conexion.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = conexionC.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (articulos != null)
                {
                    lblModificar.Text = "Modificar";
                    txtCodigo.Text = articulos.CodigoArticulo;
                    txtNombre.Text = articulos.Nombre;
                    txtDescripcion.Text = articulos.Descripcion;
                    txtImagen.Text = articulos.Imagen;
                    txtPrecio.Text = articulos.Precio.ToString();
                    cargarImagen(articulos.Imagen);
                }
            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }

        }
      
      
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagen.Text);
        }
        private void cargarImagen(string Imagen)
        {
            try
            {
                    pbxAlta.Load(Imagen);   
            }
            catch (Exception ex)
            {

                pbxAlta.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        
    }
}

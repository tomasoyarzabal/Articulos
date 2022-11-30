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
     partial class Formulario : Form
    {
        private List<Articulos> listaArticulos;
        public Formulario()
        {
            InitializeComponent();
        }
        private void Formulario_Load(object sender, EventArgs e)
        {
            Cargar();
            cboCampos.Items.Add("Codigo");
            cboCampos.Items.Add("Nombre");
            cboCampos.Items.Add("Descripcion");
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.Imagen);
            }
        }
        private void Cargar()
        {
            try
            {
                ArticulosConexion conexion = new ArticulosConexion();
                listaArticulos = conexion.listar();
                dgvArticulos.DataSource = listaArticulos;
                cargarImagen(listaArticulos[0].Imagen);
                ocultarColumna();
               
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbx.Load(imagen);
            }
            catch (Exception ex)
            {
                pbx.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }
        private void ocultarColumna()
        {
            dgvArticulos.Columns["Imagen"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AltaArticulo alta = new AltaArticulo();
            alta.ShowDialog();
            Cargar();
            
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Articulos seleccionado;
                seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                AltaArticulo modificar = new AltaArticulo(seleccionado);
                modificar.ShowDialog();
                Cargar();  
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }
        private void eliminar()
        {
            ArticulosConexion conexion = new ArticulosConexion();
            Articulos seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Seguro desea eliminar este registro?", "Eliminar registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                    conexion.eliminar(seleccionado.Id);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            Cargar();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<Articulos> listafiltrada;
            string filtro = textBox1.Text;

            if(filtro.Length >= 2)
            {
                listafiltrada = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()));

            }
            else
            {
                listafiltrada = listaArticulos;
            }
            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listafiltrada;
            ocultarColumna();

        }
        private bool validaFiltro()
        {
            if(cboCampos.SelectedIndex < 0)
            {
                MessageBox.Show("Ingrese un Campo Porfavor");
                return true;
            }
            if(cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Ingrese un Criterio Porfavor");
                return true;
            }
            return false;
        }
        
        private bool soloLetras(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsLetter(caracter)))
                return false;
            }
            return true;
        }
        private void cboCampos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampos.SelectedItem.ToString();
            try
            {
                if(opcion == "Codigo")
                {
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Primer Valor");
                }
            
                if(opcion == "Nombre" || opcion == "Descripcion")
                {
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Empieza con");
                    cboCriterio.Items.Add("Termina con");
                    cboCriterio.Items.Add("Contiene");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticulosConexion conexion = new ArticulosConexion();
            {
                if (validaFiltro())
                {
                    return;
                }
                
            }
            if(cboCampos.SelectedItem.ToString() == "Nombre" || cboCampos.SelectedItem.ToString() == "Descripcion")
            {
                if (string.IsNullOrEmpty(txtValor.Text))
                {
                    MessageBox.Show("Ingrese una letra porfavor");
                    return;
                }
                if (!(soloLetras(txtValor.Text)))
                {
                    MessageBox.Show("Solo letras porfavor");
                    return;
                }
            }
            string campo = cboCampos.SelectedItem.ToString();
            string criterio = cboCriterio.SelectedItem.ToString();
            string valor = txtValor.Text;
            dgvArticulos.DataSource = conexion.filtrar(campo, criterio, valor);
            ocultarColumna();

        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            
            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaArticulos;
          
        }

        private void btnVer_Click(object sender, EventArgs e)
        {
            ArticulosConexion conexion = new ArticulosConexion();
            Articulos seleccionado;
            seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
            Detalles modificar = new Detalles(seleccionado);
            conexion.modificar(seleccionado);

            modificar.ShowDialog();
        }
        
        
     }
}

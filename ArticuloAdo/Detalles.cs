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
    public partial class Detalles : Form
    {
        private Articulos articulos = null;
        public Detalles()
        {
            InitializeComponent();
        }
        public Detalles(Articulos articulos)
        {
            InitializeComponent();
            this.articulos = articulos;

        }

        private void Detalles_Load(object sender, EventArgs e)
        {
           
            label1.Text = articulos.CodigoArticulo;
            label2.Text = articulos.Nombre;
            label3.Text = articulos.Descripcion;
            label4.Text = articulos.Imagen;
            label5.Text = articulos.Precio.ToString();
            label6.Text = articulos.Marca.ToString();
            label7.Text = articulos.Categoria.ToString();

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JuegoMatematico
{
    public partial class FormName : Form
    {
        public FormName()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string nombreJugador = txtname.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombreJugador))
            {
                MessageBox.Show("Por favor, ingrese su nombre para continuar.",
                                "Campo obligatorio",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                txtname.Focus();
                return;
            }

            Form1 juego = new Form1(nombreJugador);
            juego.Show();

            this.Hide();
        }
    }
}



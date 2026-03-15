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
    public partial class Form1 : Form
    {
        Random rnd = new Random();
        int respuestaCorrecta;

        public Form1()
        {
            
            InitializeComponent();
        }

        private void CajaOperaciones_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int velocidad = 10;

            if (e.KeyCode == Keys.W)
            {
                Jugador.Top -= velocidad;
                if (ColisionMuro()) Jugador.Top += velocidad;
            }

            if (e.KeyCode == Keys.S)
            {
                Jugador.Top += velocidad;
                if (ColisionMuro()) Jugador.Top -= velocidad;
            }

            if (e.KeyCode == Keys.A)
            {
                Jugador.Left -= velocidad;
                if (ColisionMuro()) Jugador.Left += velocidad;
            }

            if (e.KeyCode == Keys.D)
            {
                Jugador.Left += velocidad;
                if (ColisionMuro()) Jugador.Left -= velocidad;
            }
        }


        private bool ColisionMuro()
        {//usar buble foreach para que todos los objetos con el Tag "Muro" tengan colision con el jugador 
            foreach (Control x in this.Controls)
            {
                if (x.Tag != null && x.Tag.ToString() == "Muro")
                {
                    if (Jugador.Bounds.IntersectsWith(x.Bounds))
                    {
                        return true;
                    }
                }

                foreach (Control y in x.Controls)
                {
                    if (y.Tag != null && y.Tag.ToString() == "Muro")
                    {
                        if (Jugador.Bounds.IntersectsWith(y.Bounds))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void GenerarOperacion()
        {
            int a = rnd.Next(1, 10);
            int b = rnd.Next(1, 10);

            int tipoOperacion = rnd.Next(1, 5);

            switch (tipoOperacion)
            {
                case 1:
                    LabelOperacion.Text = a + " + " + b;
                    respuestaCorrecta = a + b;
                    break;

                case 2:
                    LabelOperacion.Text = a + " - " + b;
                    respuestaCorrecta = a - b;
                    break;

                case 3:
                    LabelOperacion.Text = a + " × " + b;
                    respuestaCorrecta = a * b;
                    break;

                case 4:
                    a = a * b;
                    LabelOperacion.Text = a + " ÷ " + b;
                    respuestaCorrecta = a / b;
                    break;
            }

           
        }


        private void DiseñoMapa_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void Respuesta3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox48_Click(object sender, EventArgs e)
        {

        }
    }
}

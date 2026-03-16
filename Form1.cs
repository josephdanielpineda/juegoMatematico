using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace JuegoMatematico
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();
        private int respuestaCorrecta =  0;
        private string nombreJugador;
        public Form1(string nombre)
        {
            nombreJugador = nombre;
            InitializeComponent();
        }

        public Form1()
        {
            
            InitializeComponent();
        }

        private void CajaOperaciones_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerarOperacion();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {//configuracion para movimineot del jugador en este caso con W,A,S,D
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
            DetectarRespuestas();
            
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
        {//metodo para que a traves del LabelOperacion se muestre la operacion a realizar 
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
            GenerarRespuestas();
           
        }

        private void DetectarRespuestas()
        {
            //itilizamos if para que las respuestas tengan inteccion con el jugador 
            if (Jugador.Bounds.IntersectsWith(Respuesta1.Bounds))
                VerificarRespuesta(Respuesta1);

            if (Jugador.Bounds.IntersectsWith(Respuesta2.Bounds))
                VerificarRespuesta(Respuesta2);

            if (Jugador.Bounds.IntersectsWith(Respuesta3.Bounds))
                VerificarRespuesta(Respuesta3);

            if (Jugador.Bounds.IntersectsWith(Respuesta4.Bounds))
                VerificarRespuesta(Respuesta4);
        }


        private void VerificarRespuesta(PictureBox respuesta)
        {// para comprobar si las respuestas son verdaderas o falsas 
            int valor = Convert.ToInt32(respuesta.Tag);

            if (valor == respuestaCorrecta)
            {
                respuestaCorrecta++;
                MessageBox.Show("Respuesta Correcta");
            }
            else
            {
                MessageBox.Show("Respuesta Incorrecta");
            }

            GenerarOperacion();
            GenerarRespuestas();
        }


        private void GenerarRespuestas()
        {
            int r1 = respuestaCorrecta;
            int r2 = respuestaCorrecta + rnd.Next(1, 5);
            int r3 = respuestaCorrecta - rnd.Next(1, 5);
            int r4 = respuestaCorrecta + rnd.Next(2, 6);

            List<int> respuestas = new List<int>() { r1, r2, r3, r4 };

            respuestas = respuestas.OrderBy(x => rnd.Next()).ToList();

            Respuesta1.Tag = respuestas[0];
            Respuesta2.Tag = respuestas[1];
            Respuesta3.Tag = respuestas[2];
            Respuesta4.Tag = respuestas[3];

            MostrarNumero(Respuesta1);
            MostrarNumero(Respuesta2);
            MostrarNumero(Respuesta3);
            MostrarNumero(Respuesta4);
        }

        private void MostrarNumero(PictureBox caja)
        {
            caja.Controls.Clear();

            Label numero = new Label();

            numero.Text = caja.Tag.ToString();
            numero.Dock = DockStyle.Fill;
            numero.TextAlign = ContentAlignment.MiddleCenter;
            numero.BackColor = Color.Transparent;
            numero.Font = new Font("Arial", 18, FontStyle.Bold);

            caja.Controls.Add(numero);
        }

        private void GuardarPartida()
        {
            string connectionString = "Server=localhost;Database=JuegoMatematico;Trusted_Connection=True;TrustServerCertificate=True;";

            string query = "INSERT INTO Partida (PlayerName, RespuestaCorrectas) VALUES (@nombreJugador, @respuestasCorrectas)";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombreJugador", nombreJugador);
                    comando.Parameters.AddWithValue("@respuestasCorrectas", respuestaCorrecta);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        private void FinalizarJuego()
        {
            try
            {
                GuardarPartida();

                MessageBox.Show(
                    "Juego terminado.\n" +
                    "Jugador: " + nombreJugador + "\n" +
                    "Respuestas correctas: " + respuestaCorrecta,
                    "Partida guardada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error al guardar la partida:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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

        private void Label_Click(object sender, EventArgs e)
        {

        }

        private void CajaPuntos_Click(object sender, EventArgs e)
        {

        }

        private void CajaTiempo_Click(object sender, EventArgs e)
        {

        }

        private void CajaPuntos_Click_1(object sender, EventArgs e)
        {
           
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            FinalizarJuego()
        }
    }
}

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
        int respuestaCorrecta;
        int puntaje = 0;
        int ejerciciosRealizados = 0;
        int totalEjercicios = 5;
        int ejerciciosRestantes = 5;
        int TotalEjercicios = 5;
        private string nombreJugador;
       
        public Form1(string nombre)
        {
            nombreJugador = nombre;
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
        {
            int num1;
            int num2;
            int operador = rnd.Next(1, 5); // 1=suma, 2=resta, 3=multiplicacion, 4=division

            switch (operador)
            {
                case 1: // SUMA
                    num1 = rnd.Next(1, 20);
                    num2 = rnd.Next(1, 20);
                    respuestaCorrecta = num1 + num2;
                    LabelOperacion.Text = num1 + " + " + num2;
                    break;

                case 2: // RESTA
                    num1 = rnd.Next(1, 20);
                    num2 = rnd.Next(1, 20);
                    respuestaCorrecta = num1 - num2;
                    LabelOperacion.Text = num1 + " - " + num2;
                    break;

                case 3: // MULTIPLICACION
                    num1 = rnd.Next(1, 12);
                    num2 = rnd.Next(1, 12);
                    respuestaCorrecta = num1 * num2;
                    LabelOperacion.Text = num1 + " × " + num2;
                    break;

                case 4: // DIVISION SIN DECIMALES
                    num2 = rnd.Next(1, 12);
                    respuestaCorrecta = rnd.Next(1, 12);
                    num1 = num2 * respuestaCorrecta; // garantiza división exacta
                    LabelOperacion.Text = num1 + " ÷ " + num2;
                    break;
            }
            GenerarRespuestas();
        }
        
           
        

        private void DetectarRespuestas()
        {
            //utilizamos if para que las respuestas tengan inteccion con el jugador 
            if (Jugador.Bounds.IntersectsWith(Respuesta1.Bounds))
                VerificarRespuesta(Respuesta1);

            if (Jugador.Bounds.IntersectsWith(Respuesta2.Bounds))
                VerificarRespuesta(Respuesta2);

            if (Jugador.Bounds.IntersectsWith(Respuesta3.Bounds))
                VerificarRespuesta(Respuesta3);

            if (Jugador.Bounds.IntersectsWith(Respuesta4.Bounds))
                VerificarRespuesta(Respuesta4);

            if (Jugador.Bounds.IntersectsWith(Respuesta5.Bounds))
                VerificarRespuesta(Respuesta5);
        }


        private void VerificarRespuesta(PictureBox respuesta)
        {
            int valor = Convert.ToInt32(respuesta.Tag);

            if (valor == respuestaCorrecta)
            {
                MessageBox.Show("Respuesta Correcta");
                puntaje += 10;
            }
            else
            {
                MessageBox.Show("Respuesta Incorrecta");
                puntaje -= 5;
            }

            LabelPuntaje.Text = "Puntaje: " + puntaje;

            ejerciciosRestantes--;

            TotalEjercicio.Text = ejerciciosRestantes.ToString();

            if (ejerciciosRestantes == 0)
            {
                FinDelJuego();
                return;
            }

            GenerarOperacion();
        }


        private void FinDelJuego()
        {
            try
            {
                GuardarPartida();

                DialogResult resultado = MessageBox.Show(
                    "Juego terminado\n\nJugador: " + nombreJugador +
                    "\nPuntaje final: " + puntaje +
                    "\n\n¿Quieres jugar otra vez?",
                    "Fin del juego",
                    MessageBoxButtons.YesNo
                );

                if (resultado == DialogResult.Yes)
                {
                    ReiniciarJuego();
                }
                else
                {
                    Application.Exit();
                }
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

        private void ReiniciarJuego()
        {
            puntaje = 0;
            ejerciciosRestantes = totalEjercicios;

            LabelPuntaje.Text = "Puntaje: 0";
            TotalEjercicio.Text = ejerciciosRestantes.ToString();

            GenerarOperacion();
        }

        private void GenerarRespuestas()
        {
            int r1 = respuestaCorrecta;
            int r2 = respuestaCorrecta + rnd.Next(1, 5);
            int r3 = respuestaCorrecta - rnd.Next(1, 5);
            int r4 = respuestaCorrecta + rnd.Next(2, 6);
            int r5 = respuestaCorrecta + rnd.Next(2, 6);
            List<int> respuestas = new List<int>() { r1, r2, r3, r4,r5 };

            respuestas = respuestas.OrderBy(x => rnd.Next()).ToList();

            Respuesta1.Tag = respuestas[0];
            Respuesta2.Tag = respuestas[1];
            Respuesta3.Tag = respuestas[2];
            Respuesta4.Tag = respuestas[3];
            Respuesta5.Tag = respuestas[4];

            MostrarNumero(Respuesta1);
            MostrarNumero(Respuesta2);
            MostrarNumero(Respuesta3);
            MostrarNumero(Respuesta4);
            MostrarNumero(Respuesta5);
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

        private void GuardarPartida() // Guardar el nombre del jugador y el puntaje en la base de datos

        {

            string connectionString = "Server=localhost\\SQLEXPRESS;Database=JuegoMatematico;Trusted_Connection=True;TrustServerCertificate=True;";

            string query = "INSERT INTO dbo.Partida (PlayerName, Puntaje) VALUES (@PlayerName, @Puntaje)";

            
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@PlayerName", nombreJugador);
                    comando.Parameters.AddWithValue("@Puntaje", puntaje);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
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

        private void Respuesta5_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardarPrueba_Click(object sender, EventArgs e)
        {
             try
    {
        GuardarPartida();

        MessageBox.Show(
            "Datos guardados correctamente.\n" +
            "Jugador: " + nombreJugador + "\n" +
            "Puntaje actual: " + puntaje,
            "Prueba de guardado",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
    }
    catch (Exception ex)
    {
        MessageBox.Show(
            "Error al guardar:\n" + ex.Message,
            "Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
    }
        }
    }
}

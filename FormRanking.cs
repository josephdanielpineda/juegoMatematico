using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace JuegoMatematico
{
    public partial class FormRanking : Form
    {
        public FormRanking()
        {
            InitializeComponent();
        }

        private void FormRanking_Load(object sender, EventArgs e)
        {
            CargarRanking();
        }

        private void CargarRanking()
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=JuegoMatematico;Trusted_Connection=True;TrustServerCertificate=True;";

            string query = @"
                SELECT TOP 10 
                ROW_NUMBER() OVER (ORDER BY Puntaje DESC) AS Posicion,
                PlayerName AS Jugador,
                Puntaje
                FROM dbo.Partida
                ORDER BY Puntaje DESC";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter adaptador = new SqlDataAdapter(query, conexion))
                {
                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);
                    dgvRanking.DataSource = tabla;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Configuration;

namespace ProyectoBaseDatos
{
    /// <summary>
    /// Lógica de interacción para Actuliza.xaml
    /// </summary>
    public partial class Actuliza : Window
    {
        SqlConnection miConexionSql;
        private int z; 

        public Actuliza(int elId)
        {
            InitializeComponent();

            z = elId;

            string miConexion = ConfigurationManager.ConnectionStrings["ProyectoBaseDatos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;
            miConexionSql = new SqlConnection(miConexion);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string consulta = "UPDATE Cliente SET Nombre = @Nombre WHERE Id=" + z ;

            SqlCommand sqlComando = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            sqlComando.Parameters.AddWithValue("@Nombre", CuadroActualiza.Text);
            sqlComando.ExecuteNonQuery();

            miConexionSql.Close();

            this.Close();

        }
    }
}

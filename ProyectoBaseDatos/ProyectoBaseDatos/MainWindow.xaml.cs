using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;

namespace ProyectoBaseDatos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection miConexionSql;

        public MainWindow()
        {
            // GestionPedidosConnectionString

            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["ProyectoBaseDatos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);

            MuestraCliente();

            MuestraTodosPedidos();
        }

        private void MuestraCliente()
        {
            try
            {
                string consulta = "SELECT * FROM CLIENTE";

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdaptadorSql)
                {
                    DataTable clienteTabla = new DataTable();

                    miAdaptadorSql.Fill(clienteTabla);

                    listaClientes.DisplayMemberPath = "Nombre";
                    listaClientes.SelectedValuePath = "Id";
                    listaClientes.ItemsSource = clienteTabla.DefaultView;
                }
            } catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }

        private void MuestraPedidos()
        {

            try
            {
                string consulta = "SELECT * FROM Pedido P INNER JOIN Cliente C ON C.ID = P.cCliente WHERE C.ID = @ClienteId";

                SqlCommand sqlComando = new SqlCommand(consulta, miConexionSql);

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(sqlComando);

                using (miAdaptadorSql)
                {
                    sqlComando.Parameters.AddWithValue("@ClienteId", listaClientes.SelectedValue);

                    DataTable pedidosTabla = new DataTable();

                    miAdaptadorSql.Fill(pedidosTabla);

                    pedidosClientes.DisplayMemberPath = "fechaPedido";
                    pedidosClientes.SelectedValuePath = "Id";
                    pedidosClientes.ItemsSource = pedidosTabla.DefaultView;
                }
            } catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }

        }

        private void MuestraTodosPedidos()
        {
            try
            {
                string consulta = "SELECT * ,CONCAT(cCLiente, ' ', fechaPedido, ' ', formaPago) AS InfoCompleta FROM Pedido";

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdaptadorSql)
                {
                    DataTable pedidosTabla = new DataTable();

                    miAdaptadorSql.Fill(pedidosTabla);

                    todosPedidos.DisplayMemberPath = "InfoCompleta";
                    todosPedidos.SelectedValuePath = "Id";
                    todosPedidos.ItemsSource = pedidosTabla.DefaultView;
                }
            } catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }

        }

        /* private void listaClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          MuestraPedidos();
        } */

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(todosPedidos.SelectedValue.ToString());

            string consulta = "DELETE FROM Pedido WHERE Id = @PedidoId";

            SqlCommand miSqlComando = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            miSqlComando.Parameters.AddWithValue("@PedidoId", todosPedidos.SelectedValue);

            miSqlComando.ExecuteNonQuery();

            miConexionSql.Close();

            MuestraTodosPedidos();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string consulta = "INSERT INTO Cliente (Nombre) VALUES (@nombre)";

            SqlCommand miSqlComando = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            miSqlComando.Parameters.AddWithValue("@nombre", insertaCliente.Text);

            miSqlComando.ExecuteNonQuery();

            miConexionSql.Close();

            MuestraCliente();

            insertaCliente.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string consulta = "DELETE FROM Cliente WHERE Id = @ClienteId";

            SqlCommand miSqlComando = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            miSqlComando.Parameters.AddWithValue("@ClienteId", listaClientes.SelectedValue);

            miSqlComando.ExecuteNonQuery();

            miConexionSql.Close();

            MuestraCliente();
        }

        private void listaClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MuestraPedidos();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
            Actuliza ventanaActualizar = new Actuliza((int)listaClientes.SelectedValue);
          
            try
            {
                string consulta = "SELECT Nombre FROM CLIENTE WHERE Id = @ClId";

                SqlCommand miSqlCommando = new SqlCommand(consulta, miConexionSql);

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miSqlCommando);

                using (miAdaptadorSql)
                {
                    miSqlCommando.Parameters.AddWithValue("@ClId", listaClientes.SelectedValue);
                    DataTable clientesTabla = new DataTable();

                    miAdaptadorSql.Fill(clientesTabla);

                    ventanaActualizar.CuadroActualiza.Text = clientesTabla.Rows[0]["Nombre"].ToString();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }

            ventanaActualizar.ShowDialog();

            MuestraCliente();
        }
    }
}

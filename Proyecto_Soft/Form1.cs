using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Soft
{
    public partial class Form1 : Form
    {
        delegate void SetTextDelegate(string text);
        public SerialPort ArduinoPort { get; }
        string connectionString = "Server=localhost; Port=3306; Database=Registro_Puerta; Uid=root; Pwd=;";
        public Form1()
        {
            InitializeComponent();

            ArduinoPort = new System.IO.Ports.SerialPort();
            ArduinoPort.PortName = "COM3";
        }

        private void insertarRegistros(string fecha, string hora, string distancia, string tiempo_activo)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string insertar = "INSERT INTO Registro (Fecha, Hora, Distancia, Tiempo_Activo) " +
                    "VALUES(@Fecha, @Hora, @Distancia, @Tiempo_Activo)";
                using (MySqlCommand command = new MySqlCommand(insertar, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", fecha);
                    command.Parameters.AddWithValue("@Fecha", hora);
                    command.Parameters.AddWithValue("@Temperatura", distancia);
                    command.Parameters.AddWithValue("@Servo", tiempo_activo);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }


    }
}

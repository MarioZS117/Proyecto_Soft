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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Soft
{
    public partial class Form1 : Form
    {
        delegate void SetTextDelegate(string text);

        string date = "";
        string time = "";


        public SerialPort ArduinoPort { get; }
        string connectionString = "Server=localhost; Port=3306; Database=registro_puerta; Uid=root; Pwd=;";
        public Form1()
        {
            InitializeComponent();

            ArduinoPort = new System.IO.Ports.SerialPort();
            ArduinoPort.PortName = "COM7";
            ArduinoPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string dato = ArduinoPort.ReadLine();
     


            EscribirTxt(dato);
        }

        private void EscribirTxt(string dato)
        {
            if (InvokeRequired)
                try
                {
                    Invoke(new SetTextDelegate(EscribirTxt), dato);
                }
                catch { }
            else
            {
                lblDistancia.Text = dato;
            }
        }
        private void insertarRegistros(string fecha, string hora, string distancia, string nombre_usuario)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string insertar = "INSERT INTO Registro (Fecha, Hora, Distancia, Nombre_Usuario) " +
                    "VALUES(@fecha, @hora, @distancia, @nombre_usuario)";
                using (MySqlCommand command = new MySqlCommand(insertar, connection))
                {
                    command.Parameters.AddWithValue("@fecha", fecha);
                    command.Parameters.AddWithValue("@hora", hora);
                    command.Parameters.AddWithValue("@distancia", distancia);
                    command.Parameters.AddWithValue("nombre_usuario", nombre_usuario);
            
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            date = DateTime.Now.ToShortDateString();
            time = DateTime.Now.ToShortTimeString();
            lblFecha.Text = date;
            lblHora.Text = time;

            string Distancia = lblDistancia.Text;
            ArduinoPort.Open();


        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            if (!ArduinoPort.IsOpen) {
                ArduinoPort.Open();
            }
            else
            {

                ArduinoPort.WriteLine("a");
                //lblDistancia.Text = dist;
                lblFecha.Text = date;
                lblHora.Text = time;
                string Distancia = lblDistancia.Text;
                string Fecha = DateTime.Now.ToString("yyyy-MM-dd"); ;
                string Hora = lblHora.Text;
                string Nombre_Usuario = txtUsuario.Text;
                insertarRegistros(Fecha,Hora,Distancia,Nombre_Usuario);

            }
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            ArduinoPort.Close();
        }
    }
}

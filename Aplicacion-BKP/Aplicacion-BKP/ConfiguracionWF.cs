using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Configuration;
using System.Reflection;
using Aplicacion_BKP.Properties;

namespace Aplicacion_BKP
{
    public partial class ConfiguracionWF : Form
    {
        public ConfiguracionWF()
        {
            InitializeComponent();
        }
        public static string Servidor;
        public static string Base;
        public static string Puerto;
        public static string Usuario;
        public static string Clave;
        public static string Ruta;
        public Assembly assembly;
        public string path;
        public Configuration config;
        public string cadenaConexionCentral, cadenaConexionNotebook;
        private void Form1_Load(object sender, EventArgs e)
        {

            assembly = Assembly.GetExecutingAssembly();
            path = assembly.Location;

            config = ConfigurationManager.OpenExeConfiguration(path);
            cargaConf();

        }
        private void cargaConf()
        {
            char[] div = { ';' };
            // Carga los valores de la cadena de conexion de la netbook
            string[] cadena = Settings.Default.db.Split(div);
            txtServidor.Text = cadena[0].Replace("server=", "");
            txtPuerto.Text = cadena[1].Replace("Port=", "");
            txtUsuario.Text = cadena[2].Replace("User Id=", "");
            txtClave.Text = cadena[3].Replace("password=", "");
            txtBase.Text = cadena[4].Replace("database=", "");
            string[] cadenaRuta = Settings.Default.ruta.Split(div);
            Ruta = Settings.Default.ruta;
            txtRuta.Text = Ruta;
            Servidor = txtServidor.Text;
            Base = txtBase.Text;
            Puerto = txtPuerto.Text;
            Usuario = txtUsuario.Text;
            Clave = txtClave.Text;
        }
        private void RealizarBackup(string Servidor, string Base, string Puerto, string Usuario, string Clave, string Ruta)
        {
            string coneccion = "server='" + Servidor + "';Port='" + Puerto + "';User Id='" + Usuario + "';password='" + Clave + "';database='" + Base + "';Persist Security Info=True;";
            //string coneccion = "server=localhost;Port=3306;User Id=root;password=admin;database=aniuri;Persist Security Info=True;";
            string miBackup = Ruta;
            using (MySqlConnection conn = new MySqlConnection(coneccion))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(miBackup);
                        conn.Close();
                    }
                }
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            grabaConf();
        }
        private void grabaConf()
        {
            string cadenaConexionCentral = "server=" + txtServidor.Text + ";" + "Port=" + txtPuerto.Text + ";";
            cadenaConexionCentral += "User Id=" + txtUsuario.Text + ";" + "password=" + txtClave.Text + ";" + "database=" + txtBase.Text + ";" + "Persist Security Info = True";
            Properties.Settings.Default["db"] = cadenaConexionCentral;
            Properties.Settings.Default.Save(); // Saves settings in application configuration file

            string CadenaConexionDeRuta = txtRuta.Text;
            Properties.Settings.Default["ruta"] = CadenaConexionDeRuta;
            Properties.Settings.Default.Save(); // Saves settings in application configuration file

            MessageBox.Show("Se módifico el registro exitosamente.");
            Application.Exit();
        }
    }
}

using Aplicacion_BKP.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplicacion_BKP
{
    public partial class TuHogarWF : Form
    {
        public TuHogarWF()
        {
            InitializeComponent();
        }

        private void TuHogarWF_Load(object sender, EventArgs e)
        {
            Timer MyTimer = new Timer();
            MyTimer.Interval = 15000;
            MyTimer.Tick += new EventHandler(timer1_Tick);
            MyTimer.Start();
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
        public static int Contador = 0;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Contador == 0)
            {
                Contador = Contador + 1;
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    assembly = Assembly.GetExecutingAssembly();
                    path = assembly.Location;

                    config = ConfigurationManager.OpenExeConfiguration(path);
                    cargaConf();
                }
                else
                {
                    btnHacerBackup.Visible = true;
                    timer1.Stop();
                    Font fuente = new Font(label1.Font.FontFamily, 12);
                    label1.Font = fuente;
                    label1.Text = "No se encuentra conectado a una red de internet." + "\r\n" + "Conectese a una red y vuelva a intentarlo.";
                }
            }
        }
        private void cargaConf()
        {
            Font fuente = new Font(label1.Font.FontFamily, 12);
            label1.Font = fuente;
            label1.Text = "Se esta realizando el backup de base de datos.";

            char[] div = { '\\' };
            // Hago backup de Tu Hogar        
            Servidor = "btqmi2yioyza0srhlps3-mysql.services.clever-cloud.com";
            Base = "btqmi2yioyza0srhlps3";
            Puerto = "3306";
            Usuario = "uliemrdsy4cquand";
            Clave = "7LWYKNR2Ofm45Q65mR0d";

            // Hago backup de Tu Hogar        
            //Servidor = "localhost";
            //Base = "tuhogar_desarrollo";
            //Puerto = "3306";
            //Usuario = "root";
            //Clave = "admin";

            // string[] cadenaRuta = Settings.Default.ruta.Split(div);
            string cadenaRuta = Settings.Default.ruta;
            //string ruta = cadenaRuta[0] + "\\" + cadenaRuta[1] + "\\";
            //string cadenaRuta = "C:\\Users\\Usuario\\Desktop\\Backup\\";       


            string NombreArchivo = "TuHogar.sql";
            //Ruta = Settings.Default.ruta + NombreArchivo;
            Ruta = cadenaRuta + NombreArchivo;
            bool exito = RealizarBackup(Servidor, Base, Puerto, Usuario, Clave, Ruta);
            if (exito == true)
            {
                label1.Text = "Se realizo el backup exitosamente.";
            }
            else
            {
                label1.Font = fuente;
                label1.Text = "Falló el backup de base de datos.";
                btnHacerBackup.Visible = true;
            }
        }
        private bool RealizarBackup(string Servidor, string Base, string Puerto, string Usuario, string Clave, string Ruta)
        {
            bool Exito = false;
            //string coneccion = "server=localhost;Port=3306;User Id=root;password=admin;database=aniuri;Persist Security Info=True;";
            string coneccion = "server='" + Servidor + "';Port='" + Puerto + "';User Id='" + Usuario + "';password='" + Clave + "';database='" + Base + "';Persist Security Info=True;";
            string miBackup = Ruta;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(coneccion))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.Command.CommandTimeout = 6000;
                            mb.ExportToFile(miBackup);
                            conn.Close();
                            Exito = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = MessageBox.Show(ex.Message, "",
                              MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                throw;
            }
            return Exito;
            //Close();
        }
        private void btnHacerBackup_Click(object sender, EventArgs e)
        {
            assembly = Assembly.GetExecutingAssembly();
            path = assembly.Location;
            config = ConfigurationManager.OpenExeConfiguration(path);
            cargaConf();
        }
        private void label3_Click(object sender, EventArgs e)
        {
            Contador = 1;
            timer1.Stop();
            ConfiguracionWF _configuracion = new ConfiguracionWF();
            _configuracion.Show();
        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Contador = 0;
            Close();
        }
    }
}

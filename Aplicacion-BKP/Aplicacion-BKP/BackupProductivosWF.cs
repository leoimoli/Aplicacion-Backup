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
    public partial class BackupProductivosWF : Form
    {
        public BackupProductivosWF()
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

        private void BackupProductivosWF_Load(object sender, EventArgs e)
        {
            assembly = Assembly.GetExecutingAssembly();
            path = assembly.Location;

            config = ConfigurationManager.OpenExeConfiguration(path);
            cargaConf();
        }
        private void cargaConf()
        {
            char[] div = { ';' };
            // Hago backup de Pulpejitos        
            Servidor = "bksi9gvlsu8lbmkjd1p7-mysql.services.clever-cloud.com";
            Base = "bksi9gvlsu8lbmkjd1p7";
            Puerto = "3306";
            Usuario = "uqv4i9vwt1msz5bm";
            Clave = "Dq7qM0MEqOVWwvfNBhyc";
            //string[] cadenaRuta = Settings.Default.ruta.Split(div);
            string cadenaRuta = "C:\\Users\\Usuario\\Desktop\\Backup\\";
            string NombreArchivo = "Pulpejitos.sql";
            //Ruta = Settings.Default.ruta + NombreArchivo;
            Ruta = cadenaRuta + NombreArchivo;
            bool exito = RealizarBackup(Servidor, Base, Puerto, Usuario, Clave, Ruta);
            if (exito == true)
            {
                ///// Backup Mayorista 509
                exito = BackupMayorista();
                if (exito == true)
                {
                    ///// Backup All In QR
                    exito = BackupAllInQR();
                    if (exito == true)
                    {

                    }
                    else
                    {
                        const string message2 = "Atención: Fallo el backup de All In QR.";
                        const string caption2 = "Atención";
                        var result2 = MessageBox.Show(message2, caption2,
                                                     MessageBoxButtons.OK,
                                                     MessageBoxIcon.Asterisk);
                    }
                }
                else
                {
                    const string message2 = "Atención: Fallo el backup de Mayorista 509.";
                    const string caption2 = "Atención";
                    var result2 = MessageBox.Show(message2, caption2,
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                const string message2 = "Atención: Fallo el backup de Pulpejitos.";
                const string caption2 = "Atención";
                var result2 = MessageBox.Show(message2, caption2,
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Asterisk);
                Hide();
            }

        }

        private bool BackupAllInQR()
        {
            bool Exito = false;
            // Hago backup de All In QR           
            Servidor = "bferilotqyivobmd2h7p-mysql.services.clever-cloud.com";
            Base = "bferilotqyivobmd2h7p";
            Puerto = "3306";
            Usuario = "uweebykw37cfssem";
            Clave = "mTvombqwH6aZqBejoPQh";
            string cadenaRuta = "C:\\Users\\Usuario\\Desktop\\Backup\\";
            string NombreArchivo = "AllInQR.sql";
            Ruta = cadenaRuta + NombreArchivo;
            bool exito = RealizarBackup(Servidor, Base, Puerto, Usuario, Clave, Ruta);
            return Exito;
        }

        private bool BackupMayorista()
        {
            bool Exito = false;
            // Hago backup de Mayorista 509
            Servidor = "bf4dnhohttnh3y9dkh5i-mysql.services.clever-cloud.com";
            Base = "bf4dnhohttnh3y9dkh5i";
            Puerto = "3306";
            Usuario = "uafpwdppu6hq6ztt";
            Clave = "Z10yrE22hdRLA30Z7CPM";
            string cadenaRuta = "C:\\Users\\Usuario\\Desktop\\Backup\\";
            string NombreArchivo = "Mayorista509.sql";
            Ruta = cadenaRuta + NombreArchivo;
            bool exito = RealizarBackup(Servidor, Base, Puerto, Usuario, Clave, Ruta);
            return Exito;
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
    }
}

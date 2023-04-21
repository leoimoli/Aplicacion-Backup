using Aplicacion_BKP.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplicacion_BKP
{
    public partial class InicioWF : Form
    {
        public InicioWF()
        {
            InitializeComponent();
        }

        private void txtDni_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtDni.Text == "Jli20172020")
                {
                    Hide();
                    ConfiguracionWF _form = new ConfiguracionWF();
                    _form.Show();
                }
                else
                {

                }
            }
        }
        private void InicioWF_Load(object sender, EventArgs e)
        {
            cargaConf();
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
        private void cargaConf()
        {
            try
            {
                char[] div = { ';' };
                // Carga los valores de la cadena de conexion de la netbook
                string[] cadena = Settings.Default.db.Split(div);
                Servidor = cadena[0].Replace("server=", "");
                Puerto = cadena[1].Replace("Port=", "");
                Usuario = cadena[2].Replace("User Id=", "");
                Clave = cadena[3].Replace("password=", "");
                Base = cadena[4].Replace("database=", "");
                string[] cadenaRuta = Settings.Default.ruta.Split(div);
                Ruta = Settings.Default.ruta;
                RealizarBackup(Servidor, Base, Puerto, Usuario, Clave, Ruta);
            }
            catch (Exception ex)
            {
            }
        }
        private void RealizarBackup(string Servidor, string Base, string Puerto, string Usuario, string Clave, string Ruta)
        {
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
                            const string message2 = "Se registro el proveedor exitosamente.";
                            const string caption2 = "Éxito";
                            var result2 = MessageBox.Show(message2, caption2,
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Asterisk);
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
            //Close();
        }
    }
}


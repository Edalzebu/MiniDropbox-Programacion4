using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniDropbox.Web.Models.Api;
using RestSharp;

namespace MiniDropBox.WinformsApp
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            var myRestClient = new RestClient();
            var myRestRequest = new RestRequest(@"http://minidropboxclase.apphb.com/api/auth", Method.POST);
            myRestRequest.RequestFormat = DataFormat.Json;
            var myAuthenticationModel = new AuthenticationModel();
            myAuthenticationModel.Username = textBox1.Text;
            myAuthenticationModel.Password = textBox2.Text;
            myRestRequest.AddBody(myAuthenticationModel);
            var response = myRestClient.Execute(myRestRequest);
            string content = response.Content.Replace(@"\", "").Replace(@"""", "");
            if (response.Content != null && content != "ERROR")
            {           
                Program.MyToken = response.Content.Replace(@"\", "").Replace(@"""", "");
                var myFileExplorer = new FormFileExplorer();
                myFileExplorer.Show();
                this.Hide();
            }
            else
            {
                if (response.Content == null)
                {
                    MessageBox.Show("Hubo un Problema Con la Conexion!",
                        "Mensaje de MiniDropbox", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    button1.Focus();
                }
                else if (content == "ERROR")
                {
                    MessageBox.Show("Email y/o Password Invalidos!",
                        "Mensaje de MiniDropbox", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    textBox2.Focus();
                }
            }
    }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Models.Api;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace MiniDropBox.WinformsApp
{
    public partial class FormFileExplorer : Form
    {

        private ImageList myImageList = new ImageList();
        private List<DiskContentModel> _myActualPathFiles;
        private List<string> _actualPath = new List<string>();
        private string _currentPath = "";

        public FormFileExplorer()
        {
            InitializeComponent();
        }

        private void FormFileExplorerLoad(object sender, EventArgs e)
        {
            myImageList.Images.Add("folder", Image.FromFile("Images\\folder.png"));
            myImageList.Images.Add("file", Image.FromFile("Images\\file.png"));
            myImageList.ImageSize = new Size(50, 50);
            ListExplorer.LargeImageList = myImageList;

            _actualPath.Add("");
            FillListView("");
        }

        private void FillListView(string currentPath)
        {
            ListExplorer.BringToFront();
            this.ListExplorer.Items.Clear();
            var myRestClient = new RestClient();
            string param = currentPath == "" ? "" : "&Path=" + currentPath;
            var myRestRequest = new RestRequest(@"http://minidropboxclase.apphb.com/api/folder?token=" + 
                Program.MyToken + param, Method.GET);
            myRestRequest.RequestFormat = DataFormat.Json;
            var response = myRestClient.Execute(myRestRequest);
            var fileList  = JsonConvert.DeserializeObject<FolderModel>(response.Content);
            _myActualPathFiles = fileList.ListaModels;
            if (!response.Content.Contains("error"))
            {
                foreach (var diskModel in fileList.ListaModels)
                {
                    var myLvItem = new ListViewItem(diskModel.Name, diskModel.Type == "" ? 0 : 1);
                    myLvItem.Tag = diskModel.Type == "" ? "Dir" : "File";
                    ListExplorer.Items.Add(myLvItem);
                }
                if (currentPath != _actualPath.ElementAt(_actualPath.Count - 1))
                    _actualPath.Add(currentPath);
                btnUpload.Enabled = true;
                BtnNewFolder.Enabled = true;
                _currentPath = currentPath;
            }
            else
            {
                MessageBox.Show("Hubo un Problema Con la Conexion!",
                        "Mensaje de MiniDropbox", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                Application.Exit();
            }
        }

        private void ShowFileInfo(string name)
        {
            DiskContentModel myDiskContentModel = _myActualPathFiles.Find(x => x.Name == name);
            pnlFileInfo.BringToFront();
            label1.Text = myDiskContentModel.Id.ToString(CultureInfo.InvariantCulture);
            NameFile.Text = myDiskContentModel.Name;
            Type.Text = myDiskContentModel.Type;
            ModifiedDate.Text = myDiskContentModel.ModifiedDate;
            if (name != _actualPath.ElementAt(_actualPath.Count - 1))
                _actualPath.Add(name);
            btnUpload.Enabled = false;
            BtnNewFolder.Enabled = false;            
        }

        private void ListExplorerMouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (this.ListExplorer.SelectedItems.Count > 0)
            {
                if (ListExplorer.SelectedItems[0].Tag.ToString() == "Dir")
                    FillListView(this.ListExplorer.SelectedItems[0].Text);
                else
                    ShowFileInfo(this.ListExplorer.SelectedItems[0].Text);
            }
        }

        private void BtnBackClick(object sender, EventArgs e)
        {
            ListExplorer.BringToFront();
            if (_actualPath.Count != 1)
            {
                _actualPath.RemoveAt(_actualPath.Count - 1);
                FillListView(_actualPath.ElementAt(_actualPath.Count - 1));
            }
        }

        private void BtnNewFolderClick(object sender, EventArgs e)
        {
            pnlNewFolder.Visible = true;
            pnlNewFolder.BringToFront();
            txtNewFolder.Focus();
        }

        private void BtnCreateNewFolderClick(object sender, EventArgs e)
        {
            var myRestClient = new RestClient();
            var myRestRequest = new RestRequest(@"http://minidropboxclase.apphb.com/api/folder?token=" +
                Program.MyToken + "&currentPath=" + _actualPath.ElementAt(_actualPath.Count - 1) + 
                "&folderName=" + txtNewFolder.Text, Method.PUT);
            myRestRequest.RequestFormat = DataFormat.Json;
            var response = myRestClient.Execute(myRestRequest);
            pnlNewFolder.Visible = false;
            FillListView(_actualPath.ElementAt(_actualPath.Count - 1));
        }

        private void BtnDeleteClick(object sender, EventArgs e)
        {
            if (ListExplorer.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Esta seguro de eliminar el archivo " + ListExplorer.SelectedItems[0].Text + "?",
                    "Confirmacion",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string target = "";
                    if (ListExplorer.SelectedItems[0].Tag.ToString() == "Dir")
                        target = "folder";
                    else
                        target = "files";
                    var myRestClient = new RestClient();
                    var myRestRequest =
                        new RestRequest(
                            @"http://minidropboxclase.apphb.com/api/" + target + "?token=" + Program.MyToken + "&id=" +
                            _myActualPathFiles.Find(x => x.Name == ListExplorer.SelectedItems[0].Text).Id, Method.DELETE);
                    myRestRequest.RequestFormat = DataFormat.Json;
                    var response = myRestClient.Execute(myRestRequest);
                    FillListView(_actualPath.ElementAt(_actualPath.Count - 1));
                }
            }
        }

        private void BtnUploadClick(object sender, EventArgs e)
        {
            var myDialog = new OpenFileDialog();
            if (myDialog.ShowDialog() == DialogResult.OK)
            {
                var myRestClient = new RestClient();
                var myRestRequest =
                    new RestRequest(@"http://minidropboxclase.apphb.com/api/files?token=" + Program.MyToken + "&currentPath=" +
                                    _actualPath.ElementAt(_actualPath.Count - 1), Method.POST);
                myRestRequest.RequestFormat = DataFormat.Json;
                myRestRequest.AddHeader("Content-Type", "multipart/form-data");
                myRestRequest.AddFile("file", File.ReadAllBytes(myDialog.FileName), myDialog.SafeFileName,
                    "application/octet-stream");
                var request = myRestClient.Execute(myRestRequest);
                if (request.Content == "No esta nulo")
                    FillListView(_actualPath.ElementAt(_actualPath.Count - 1));
                else
                    MessageBox.Show("Hubo un error al subir el archivo", "Mensaje de MiniDropbox", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
            }
            //new WebClient().UploadFile(@"http://localhost:1840/api/files", "POST",
            //Path.GetFullPath(myDialog.FileName));

        }

        private void BtnDownloadClick(object sender, EventArgs e)
        {
            //var myRestClient = new RestClient();
            //var myRestRequest =
            //    new RestRequest(@"http://localhost:1840/api/files?token=" + Program.MyToken + "&currentPath=" +
            //        _actualPath.ElementAt(_actualPath.Count - 1), Method.POST);
            //myRestRequest.RequestFormat = DataFormat.Json;            
            //var request = myRestClient.Execute(myRestRequest);
            var mySaveDialog = new FolderBrowserDialog();
            if (mySaveDialog.ShowDialog() == DialogResult.OK)
            {
                var client = new WebClient();
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFile(new Uri(@"http://minidropboxclase.apphb.com/api/files?token=" +
                    Program.MyToken + "&id=" + _myActualPathFiles.Find(x => x.Name ==
                    ListExplorer.SelectedItems[0].Text).Id), mySaveDialog.SelectedPath +
                    "\\" + ListExplorer.SelectedItems[0].Text);
            }
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            MessageBox.Show(e.BytesReceived.ToString(CultureInfo.InvariantCulture));
        }

        private void FormFileExplorerFormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void BtnRenameClick(object sender, EventArgs e)
        {
            pnlRename.Visible = true;
            pnlRename.BringToFront();
            textRename.Focus();
        }

        private void Button2Click(object sender, EventArgs e)
        {
            var myRestClient = new RestClient();
            var myRestRequest =
                new RestRequest(
                    @"http://minidropboxclase.apphb.com/api/files?token=" + Program.MyToken + "&objectId=" +
                    _myActualPathFiles.Find(x => x.Name == ListExplorer.SelectedItems[0].Text).Id + "&newName=" + 
                    textRename.Text, Method.POST);
            myRestRequest.RequestFormat = DataFormat.Json;
            var response = myRestClient.Execute(myRestRequest);
            if (response.Content.Replace(@"""", "") != "true")
                MessageBox.Show("No se pudo renombrar el archivo", "Mensaje de MiniDropbox",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            FillListView(_actualPath.ElementAt(_actualPath.Count - 1));
            pnlFileInfo.SendToBack();
            ListExplorer.BringToFront();
            pnlRename.Visible = false;
            pnlNewFolder.Visible = false;
        }

        private void Label2Click(object sender, EventArgs e)
        {
            pnlRename.Visible = false;
            pnlNewFolder.Visible = false;
            label2.Visible = false;
        }
    }
}
    
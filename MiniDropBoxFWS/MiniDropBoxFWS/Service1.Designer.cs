using System.Configuration;
using System.IO;
using  System.Windows.Forms;
using RestSharp;

namespace MiniDropBoxFWS
{
    partial class Service1
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FSWatcher = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.FSWatcher)).BeginInit();
            // 
            // FSWatcher
            // 
            this.FSWatcher.EnableRaisingEvents = true;
            this.FSWatcher.IncludeSubdirectories = true;
            this.FSWatcher.NotifyFilter = ((System.IO.NotifyFilters)(((((((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName) 
            | System.IO.NotifyFilters.Attributes) 
            | System.IO.NotifyFilters.Size) 
            | System.IO.NotifyFilters.LastAccess) 
            | System.IO.NotifyFilters.CreationTime) 
            | System.IO.NotifyFilters.Security)));
            this.FSWatcher.Changed += new System.IO.FileSystemEventHandler(this.FSWatcher_Changed);
            this.FSWatcher.Created += new System.IO.FileSystemEventHandler(this.FSWatcher_Created);
            this.FSWatcher.Deleted += new System.IO.FileSystemEventHandler(this.FSWatcher_Deleted);
            this.FSWatcher.Renamed += new System.IO.RenamedEventHandler(this.FSWatcher_Renamed);
            // 
            // Service1
            // 
            this.ServiceName = "Service1";
            ((System.ComponentModel.ISupportInitialize)(this.FSWatcher)).EndInit();

        }

        #endregion

        private System.IO.FileSystemWatcher FSWatcher;

        private void FSWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            
        }

        private void FSWatcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                FSWatcher.EnableRaisingEvents = false;
                File.Create(ConfigurationManager.AppSettings["WatchPath"] + "\\" + e.Name + " Was Created.txt");
                var myRestClient = new RestClient(@"http://localhost:1840/");
                string type = "";
                if (System.IO.Directory.Exists(e.FullPath))
                    type = "DIR";
                else
                    type = "FILE";
                var FileModels = new ModelFiles();
                FileModels.Path = e.FullPath;
                FileModels.Type = type;
                var myRestRequest = new RestRequest("api/files?path=" + e.FullPath 
                    + "&type=" + type, Method.POST);
                myRestRequest.AddBody(FileModels);
                //myRestRequest.AddUrlSegment("token", Program.MyToken);
                myRestRequest.RequestFormat = DataFormat.Json;
                var response = myRestClient.Execute(myRestRequest);
            }
            finally
            {
                FSWatcher.EnableRaisingEvents = true;
            }
        }

        private void FSWatcher_Deleted(object sender, FileSystemEventArgs e)
        {

        }

        private void FSWatcher_Renamed(object sender, RenamedEventArgs e)
        {

        }
    }
}

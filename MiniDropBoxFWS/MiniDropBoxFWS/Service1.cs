using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MiniDropBoxFWS
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        public void onDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                          + "\\MiniDropbox";
            if(!Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            FSWatcher.Path = path;
        }

        protected override void OnStop()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;

namespace ElectronMVCApplication.Data
{
    public class UserDataLocation
    {
        public string GetAppDataPath()
        {
            string path = Path.Combine(Electron.App.GetPathAsync(ElectronNET.API.Entities.PathName.AppData).Result, "Electron MVC App");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}

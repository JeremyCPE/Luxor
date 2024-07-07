using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luxor.Settings
{
    internal class UserSettingsServices
    {
        private UserSettings _userSettings;
        public string Export()
        {
            throw new NotImplementedException();
        }

        public bool Import(string path)
        {
            try
            {
                var file = File.OpenRead(path);
                UserSettings importSettings = (UserSettings)JsonConvert.DeserializeObject("zz");
                _userSettings.Replace(importSettings);
                return true;

            }
            catch (Exception ex) {
                Debug.WriteLine("Import exception, {ex}", ex);
                return false;
            }

        }

        /// <summary>
        /// Save the config in the correct path
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            try
            {
                File.Create(JsonConvert.SerializeObject(_userSettings));
                return true;
            }
            catch (Exception ex) 
            {
                Debug.WriteLine("Save exception, {ex}",ex);
                return false;
            }
        }
        /// <summary>
        /// Define the path, create the folder, delete the old settings in the old folder if exists
        /// </summary>
        /// <param name="path"></param>
        public void Path(string path) 
        {
            try
            {
                File.Delete(path + """\config.json""");
            }
            catch (Exception ex) {
                Debug.WriteLine("Couldn't delete the config, ex {ex}",ex);
            }
            _userSettings.Path = path;
            Save();
        }
    }
}

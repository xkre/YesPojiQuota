using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Core.Windows.Services
{
    public class LocalStorageService
    {
        private LocalObjectStorageHelper _localStorage;

        public LocalStorageService()
        {
            _localStorage = new LocalObjectStorageHelper();
        }

        public void SaveString(string key, string toSave)
        {
            _localStorage.Save(key, toSave);
        }

        public string ReadString(string key)
        {
            return _localStorage.Read<string>(key);
        }

        public void SaveSimple<T>(string key, T toSave)
        {
            _localStorage.Save(key, toSave);
        }

        public T ReadSimple<T>(string key)
        {
            return _localStorage.Read<T>(key);
        }
    }
}

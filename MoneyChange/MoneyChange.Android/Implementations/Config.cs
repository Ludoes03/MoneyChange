using MoneyChange.Interfaces;
using SQLite.Net.Interop;
using Xamarin.Forms;

[assembly: Dependency(typeof(MoneyChange.Droid.Implementations.Config))]

namespace MoneyChange.Droid.Implementations
{
    public class Config : IConfig
    {

         string directoryDB;
         ISQLitePlatform platform;

        public string DirectoryDB
        {
            get
            {
                if (string.IsNullOrEmpty(directoryDB))
                {
                    directoryDB = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                }

                return directoryDB;
            }
        }

        public ISQLitePlatform Platform
        {
            get
            {
                if (platform == null)
                {
                    platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
                }

                return platform;

            }
        }


    }
}
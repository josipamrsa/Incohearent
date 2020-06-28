using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Incohearent.Data;
using System.Reflection;
using Incohearent.Droid.Data;

[assembly: Xamarin.Forms.Dependency(typeof(SQLiteAndroid))] // potreban dependency
namespace Incohearent.Droid.Data
{ 
    public class SQLiteAndroid : ISQlite
    {
        public SQLiteAndroid() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "IncohearentDB";
            string documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentPath, sqliteFilename);
            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }
    }
}
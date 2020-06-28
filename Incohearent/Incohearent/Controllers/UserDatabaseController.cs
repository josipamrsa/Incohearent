using Incohearent.Data;
using Incohearent.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Incohearent.Controllers
{
    // Metode za upravljanje tablicom korisnika
    public class UserDatabaseController
    {
        static object locker = new object();
        SQLiteConnection db;
        public UserDatabaseController()
        {
            db = DependencyService.Get<ISQlite>().GetConnection();
            db.CreateTable<User>();
        }

        public User GetUser()
        {
            lock (locker)
            {
                if (db.Table<User>().Count() == 0) { return null; }
                else { return db.Table<User>().First(); }
            }
        }

        public int SaveUser(User u)
        {
            lock (locker)
            {
                if (u.UserId != 0)
                {
                    db.Update(u);
                    return u.UserId;
                }
                else {
                    return db.Insert(u);                    
                }

                
            }
        }

        public int DeleteUser(int id)
        {
            lock (locker) { return db.Delete<User>(id); }
        }
    }
}

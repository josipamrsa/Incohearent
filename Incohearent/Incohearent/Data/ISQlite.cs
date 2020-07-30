using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incohearent.Data
{
    // Interface - spajanje s bazom podataka
    public interface ISQlite
    {
        SQLiteAsyncConnection GetConnection();
    }
}

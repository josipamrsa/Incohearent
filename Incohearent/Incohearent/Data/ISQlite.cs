using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incohearent.Data
{
    public interface ISQlite
    {
        SQLiteConnection GetConnection();
    }
}

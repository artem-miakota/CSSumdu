using CSSumdu.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSumdu.ViewModel
{
    class DB
    {
        private SQLiteAsyncConnection conn;

        public async Task init()
        {
            conn = new SQLiteAsyncConnection("cssumdu.db");
            await conn.CreateTableAsync<Event>();
        }

        public async Task addOne()
        {
            Event e = new Event();
            e.name = "eventTest";

            await conn.InsertAsync(e);
        }
    }
}

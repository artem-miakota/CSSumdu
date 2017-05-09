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
        private static SQLiteAsyncConnection conn;

        public async Task init()
        {
            conn = new SQLiteAsyncConnection("cssumdu.db");
            await conn.CreateTableAsync<Event>();
            await conn.CreateTableAsync<Group>();
            await conn.CreateTableAsync<Teacher>();
            await conn.CreateTableAsync<Auditorium>();
        }

        public SQLiteAsyncConnection getConnetion()
        {
            return conn;
        }
    }
}

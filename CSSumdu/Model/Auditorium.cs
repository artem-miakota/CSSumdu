using SQLite;
using System;

namespace CSSumdu.Model
{
    [Table("Auditoriums")]
    class Auditorium
    {
        [PrimaryKey, Indexed]
        public int id { get; set; }

        [Indexed]
        public String name { get; set; }
    }
}

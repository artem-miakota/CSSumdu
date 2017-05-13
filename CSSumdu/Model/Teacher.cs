using SQLite;
using System;

namespace CSSumdu.Model
{
    [Table("Teachers")]
    class Teacher
    {
        [PrimaryKey, Indexed]
        public int id { get; set; }

        [Indexed]
        public String name { get; set; }
    }
}

using SQLite;
using System;

namespace CSSumdu.Model
{
    [Table("Groups")]
    class Group
    {
        [PrimaryKey, Indexed]
        public int id { get; set; }

        [Indexed]
        public String name { get; set; }
    }
}

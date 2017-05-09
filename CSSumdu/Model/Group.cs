using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

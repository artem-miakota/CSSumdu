using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

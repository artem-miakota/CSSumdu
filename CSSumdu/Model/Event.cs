using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSumdu.Model
{
    [Table("Events")]
    class Event
    {
        [PrimaryKey]
        //public int id { get; set; }
        //public String DATE_REG { get; set; }
        //public String TIME_PAIR { get; set; }
        public long START_TIME { get; set; }

        [Indexed]
        public String NAME_FIO { get; set; }

        [Indexed]
        public String NAME_AUD { get; set; }

        [Indexed]
        public String NAME_GROUP { get; set; }

        public String ABBR_DISC { get; set; }

        public String NAME_STUD { get; set; }

        public String REASON { get; set; }

    }
}

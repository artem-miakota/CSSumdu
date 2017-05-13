using SQLite;
using System;

namespace CSSumdu.Model
{
    [Table("Events")]
    class Event
    {
        [PrimaryKey]
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

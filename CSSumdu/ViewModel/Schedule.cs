using CSSumdu.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSSumdu.ViewModel
{
    class Schedule
    {
        public async Task getList(String url, String SBPrefix)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                String ResponseString = await httpClient.GetStringAsync(new Uri(url));

                Dictionary<int, String> list = JsonConvert.DeserializeObject<Dictionary<int, String>>(ResponseString);

                DB db = new DB();
                StringBuilder query = new StringBuilder(SBPrefix);
                foreach (KeyValuePair<int, String> entry in list)
                {
                    query.Append(" (" + entry.Key + ", \"" + entry.Value + "\"),");
                }
                query[query.Length - 1] = ';';

                await db.getConnetion().QueryAsync<Group>(query.ToString());
            }
            catch { }
        }

        // http://schedule.sumdu.edu.ua/index/json?method=getSchedules&id_grp=0&id_fio=0&id_aud=0&date_beg=00.00.0000&date_end=00.00.0000
        public async Task getSchedule(int groupId, int teacherId, int auditoriumId, DateTimeOffset begin, DateTimeOffset end)
        {
            StringBuilder url = new StringBuilder("http://schedule.sumdu.edu.ua/index/json?method=getSchedules");
            url.Append("&id_grp=" + groupId + "&id_fio=" + teacherId + "&id_aud=" + auditoriumId);
            url.Append("&date_beg=" + begin.Day + "." + begin.Month + "." + begin.Year);
            url.Append("&date_end=" + end.Day + "." + end.Month + "." + end.Year);

            try
            {
                HttpClient httpClient = new HttpClient();
                String ResponseString = await httpClient.GetStringAsync(new Uri(url.ToString()));

                List<TEvent> list = JsonConvert.DeserializeObject<List<TEvent>>(ResponseString);

                DB db = new DB();
                StringBuilder query = new StringBuilder("INSERT INTO Events (START_TIME, NAME_FIO, NAME_AUD, NAME_GROUP, ABBR_DISC, NAME_STUD, REASON) VALUES");
                foreach (TEvent entry in list)
                {
                    var date = entry.DATE_REG.Split('.');
                    char[] array = { ':', '-' };
                    var time = entry.TIME_PAIR.Split(array);
                    DateTimeOffset d = new DateTimeOffset(Int32.Parse(date[2]), Int32.Parse(date[1]),
                        Int32.Parse(date[0]), Int32.Parse(time[0]), Int32.Parse(time[1]), 0, new TimeSpan());
                    query.Append(" (" + d.Ticks +
                        ", \"" + entry.NAME_FIO +
                        "\", \"" + entry.NAME_AUD +
                        "\", \"" + entry.NAME_GROUP +
                        "\", \"" + entry.ABBR_DISC +
                        "\", \"" + entry.NAME_STUD +
                        "\", \"" + entry.REASON +
                        "\"),");
                }
                query[query.Length - 1] = ';';

                await db.getConnetion().QueryAsync<Group>(query.ToString());
            }
            catch { }
        }

        private class TEvent
        {
            public String DATE_REG;
            public String NAME_WDAY;
            public String NAME_PAIR;
            public String TIME_PAIR;
            public String NAME_FIO;
            public String NAME_AUD;
            public String NAME_GROUP;
            public String ABBR_DISC;
            public String NAME_STUD;
            public String REASON;
            public String PUB_DATE;
            public String KOD_STUD;
            public String KOD_FIO;
            public String KOD_AUD;
            public String KOD_DISC;
            public String INFO;

            public TEvent()
            {
                DATE_REG = null;
                NAME_WDAY = null;
                NAME_PAIR = null;
                TIME_PAIR = null;
                NAME_FIO = null;
                NAME_AUD = null;
                NAME_GROUP = null;
                ABBR_DISC = null;
                NAME_STUD = null;
                REASON = null;
                PUB_DATE = null;
                KOD_STUD = null;
                KOD_FIO = null;
                KOD_AUD = null;
                KOD_DISC = null;
                INFO = null;            
            }
        }
    }
}

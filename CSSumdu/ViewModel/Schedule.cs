using CSSumdu.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSSumdu.ViewModel
{
    class Schedule
    {
        private static Schedule instance;

        private Schedule() { }

        public static Schedule Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new Schedule();
                }
                return instance;
            }
        }

        public async Task getList(String url, String SBPrefix)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                String ResponseString = await httpClient.GetStringAsync(new Uri(url));

                Dictionary<int, String> list = JsonConvert.DeserializeObject<Dictionary<int, String>>(ResponseString);

                StringBuilder query = new StringBuilder(SBPrefix);
                foreach (KeyValuePair<int, String> entry in list)
                {
                    query.Append(" (" + entry.Key + ", \"" + entry.Value + "\"),");
                }
                query[query.Length - 1] = ';';

                await DB.Instance.getConnetion().QueryAsync<Group>(query.ToString());
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

                List<Event> list = JsonConvert.DeserializeObject<List<Event>>(ResponseString);

                StringBuilder query = new StringBuilder("INSERT INTO Events (START_TIME, DATE_REG, TIME_PAIR, NAME_FIO, NAME_AUD, NAME_GROUP, ABBR_DISC, NAME_STUD, REASON) VALUES");
                foreach (Event entry in list)
                {
                    var date = entry.DATE_REG.Split('.');
                    char[] array = { ':', '-' };
                    var time = entry.TIME_PAIR.Split(array);
                    DateTimeOffset d = new DateTimeOffset(Int32.Parse(date[2]), Int32.Parse(date[1]),
                        Int32.Parse(date[0]), Int32.Parse(time[0]), Int32.Parse(time[1]), 0, new TimeSpan());
                    query.Append(" (" + d.Ticks +
                        ", \"" + entry.DATE_REG +
                        "\", \"" + entry.TIME_PAIR +
                        "\", \"" + entry.NAME_FIO +
                        "\", \"" + entry.NAME_AUD +
                        "\", \"" + entry.NAME_GROUP +
                        "\", \"" + entry.ABBR_DISC +
                        "\", \"" + entry.NAME_STUD +
                        "\", \"" + entry.REASON +
                        "\"),");
                }
                query[query.Length - 1] = ';';

                await DB.Instance.getConnetion().QueryAsync<Event>("DELETE FROM events");
                await DB.Instance.getConnetion().QueryAsync<Event>(query.ToString());
            }
            catch { }
        }
    }
}

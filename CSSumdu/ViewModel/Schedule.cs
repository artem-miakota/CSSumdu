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

                Dictionary<int, String> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<int, String>>(ResponseString);

                DB db = new DB();
                StringBuilder query = new StringBuilder(SBPrefix);
                foreach (KeyValuePair<int, String> entry in htmlAttributes)
                {
                    query.Append(" (" + entry.Key + ", \"" + entry.Value + "\"),");
                }
                query.Length--;
                query.Append(";");

                await db.getConnetion().QueryAsync<Group>(query.ToString());
            }
            catch { }
        }
    }
}

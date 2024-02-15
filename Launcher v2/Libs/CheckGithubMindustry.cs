using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Text;

namespace MGL
{
    internal class CheckGithubMindustry
    {
        #region -- Проверка последней версии игры MIndustry на Github --
        public string CheckLatestRelease()
        {
            // Создаем объект WebClient
            using (WebClient client = new WebClient())
            {
                // ссылка на API GitHub для получения информации о последнем релизе
                string api_url = "https://api.github.com/repos/Anuken/Mindustry/releases/latest";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api_url);

                request.UserAgent = "request";

                // получаем ответ от сервера
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string json;

                // получаем ответ и сохраняем его в переменную json
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    json = reader.ReadToEnd();
                }

                // Разбираем JSON и получаем версию релиза
                JObject jObject = JObject.Parse(json);
                string version = (string)jObject["tag_name"];
                if (version.StartsWith("v"))
                {
                    version = version.Substring(1);
                }
                return version;
            }
        }
        #endregion
    }
}

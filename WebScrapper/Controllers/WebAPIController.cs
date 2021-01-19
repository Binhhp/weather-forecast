using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebScrapper.Models;
namespace WebScrapper.Controllers
{
    public class WebAPIController : Controller
    {
        private readonly WeatherDbContext db = new WeatherDbContext();
        private const string NewSessionn = "NewSession";
        // GET: WebAPI
        public ActionResult WeatherAPI()
        {
            return View();
        }
        //API weather
        [HttpPost]
        public ActionResult APIWeather(string namecity)
        {
            try
            {
                string API = "6c450057c66e4cb0b7f87eb7597fedd2";
                string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", namecity, API);
                using (WebClient webClient = new WebClient())
                {
                    string json = webClient.DownloadString(url);
                    root weatherInfo = (new JavaScriptSerializer()).Deserialize<root>(json);
                    var date = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                    var checkdatetime = db.WeatherAPICurrents.FirstOrDefault(x => x.NgayTao == date && x.Ten == weatherInfo.name && x.Lat == weatherInfo.coord.lat && x.Long == weatherInfo.coord.lon);
                    if (checkdatetime == null)
                    {
                        var webcurennt = new WeatherAPICurrent()
                        {
                            Lat = weatherInfo.coord.lat,
                            Long = weatherInfo.coord.lon,
                            Ten = weatherInfo.name,
                            Datnuoc = weatherInfo.Sys.country,
                            Nhietdo = weatherInfo.main.temp,
                            Anh = weatherInfo.weather[0].icon,
                            Doam = weatherInfo.main.humidity,
                            Tocdogio = weatherInfo.wind.speed,
                            Apsuat = weatherInfo.main.pressure,
                            May = weatherInfo.clouds.all,
                            MoTa = weatherInfo.weather[0].description,
                            NgayTao = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"))
                        };
                        db.WeatherAPICurrents.Add(webcurennt);
                        db.SaveChanges();
                    }
                    return Json(weatherInfo, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult APIWeatherDAY(string namecity)
        {
            try
            {
                string APIDAY = "9f2d48dd6c4b58d7836338be6a94668d";
                string urlDay = string.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&appid={1}", namecity, APIDAY);
                using (WebClient webClient = new WebClient())
                {
                    string jsonDAY = webClient.DownloadString(urlDay);
                    var weatherDAY = (new JavaScriptSerializer()).Deserialize<weatherDay>(jsonDAY);
                    foreach (var item in weatherDAY.list)
                    {
                        DateTime date = Convert.ToDateTime(item.dt_txt);
                        var checkdatetime = db.WebAPI5Day.FirstOrDefault(x => x.NgayTao == date && x.Ten == weatherDAY.city.name);
                        if (checkdatetime == null)
                        {
                            var weather5dayAPI = new WebAPI5Day()
                            {
                                Ten = weatherDAY.city.name,
                                Anh = item.weather[0].icon,
                                Nhietdocao = item.main.temp_max,
                                Nhietdothap = item.main.temp_min,
                                MoTa = item.weather[0].description,
                                May = item.clouds.all,
                                Tocdogio = item.wind.speed,
                                Doam = item.main.humidity,
                                NgayTao = Convert.ToDateTime(item.dt_txt)
                            };
                            db.WebAPI5Day.Add(weather5dayAPI);
                        }
                    }
                    db.SaveChanges();
                    return Json(weatherDAY, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult WeatherMyposition(string lat, string lng)
        {
            string API = "6c450057c66e4cb0b7f87eb7597fedd2";
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}", lat, lng, API);
            using (WebClient client = new WebClient())
            {
                string filejson = client.DownloadString(url);
                root weatherInfo = (new JavaScriptSerializer()).Deserialize<root>(filejson);
                var date = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                var checkdatetime = db.WeatherAPICurrents.FirstOrDefault(x => x.NgayTao == date && x.Ten == weatherInfo.name && x.Lat == weatherInfo.coord.lat && x.Long == weatherInfo.coord.lon);
                if(checkdatetime == null)
                {
                    var webcurennt = new WeatherAPICurrent()
                    {
                        Lat = weatherInfo.coord.lat,
                        Long = weatherInfo.coord.lon,
                        Ten = weatherInfo.name,
                        Datnuoc = weatherInfo.Sys.country,
                        Nhietdo = weatherInfo.main.temp,
                        Anh = weatherInfo.weather[0].icon,
                        Doam = weatherInfo.main.humidity,
                        Tocdogio = weatherInfo.wind.speed,
                        Apsuat = weatherInfo.main.pressure,
                        May = weatherInfo.clouds.all,
                        MoTa = weatherInfo.weather[0].description,
                        NgayTao = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"))
                    };
                    db.WeatherAPICurrents.Add(webcurennt);
                    db.SaveChanges();
                }
                return Json(weatherInfo, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult WeatherMypositionDAY(string lat, string lng)
        {
            try
            {
                string APIDAY = "9f2d48dd6c4b58d7836338be6a94668d";
                string urlDay = string.Format("http://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&appid={2}", lat, lng, APIDAY);
                using (WebClient webClient = new WebClient())
                {
                    string jsonDAY = webClient.DownloadString(urlDay);
                    var weatherDAY = (new JavaScriptSerializer()).Deserialize<weatherDay>(jsonDAY);
                    foreach(var item in weatherDAY.list)
                    {
                        DateTime date = Convert.ToDateTime(item.dt_txt);
                        var checkdatetime = db.WebAPI5Day.FirstOrDefault(x => x.NgayTao == date && x.Ten == weatherDAY.city.name);
                        if(checkdatetime == null)
                        {
                            var weather5dayAPI = new WebAPI5Day()
                            {
                                Ten = weatherDAY.city.name,
                                Anh = item.weather[0].icon,
                                Nhietdocao = item.main.temp_max,
                                Nhietdothap = item.main.temp_min,
                                MoTa = item.weather[0].description,
                                May = item.clouds.all,
                                Tocdogio = item.wind.speed,
                                Doam = item.main.humidity,
                                NgayTao = Convert.ToDateTime(item.dt_txt)
                            };
                            db.WebAPI5Day.Add(weather5dayAPI);
                        }
                    }
                    db.SaveChanges();
                    return Json(weatherDAY, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SearchDatetime5Day(string date,string address)
        {
            var list = db.WebAPI5Day.ToList();
            var listreturn = new List<API5Day>();
            foreach(var item in list)
            {
                var dt = String.Format("{0:M/d/yyyy}", item.NgayTao);
                var datetimesearch = String.Format("{0:M/d/yyyy}", Convert.ToDateTime(date));
                if(datetimesearch.ToString() == dt.ToString() && item.Ten.Contains(address) == true)
                {
                    var weather5dayAPI = new API5Day()
                    {
                        Anh = item.Anh,
                        Nhietdocao = (double)Math.Round((double)(item.Nhietdocao - 274.15)),
                        Nhietdothap = (double)Math.Round((double)(item.Nhietdothap - 274.15)),
                        MoTa = item.MoTa,
                        May = item.May,
                        Tocdogio = item.Tocdogio,
                        Doam = item.Doam,
                        NgayTao = String.Format("{0:d/M/yyyy HH:mm:ss}", item.NgayTao)
                    };
                    listreturn.Add(weather5dayAPI);
                }
            }
            return Json(listreturn, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchCurent(string date,string address)
        {
            var listcurent = db.WeatherAPICurrents.ToList();
            var weatherCurent = new APICurent();
            var datetimesearch = String.Format("{0:M/d/yyyy}", Convert.ToDateTime(date));
            foreach (var item in listcurent)
            {
                var dt = String.Format("{0:M/d/yyyy}", item.NgayTao);
                if (datetimesearch.ToString() == dt.ToString() && item.Ten.Contains(address) == true)
                {
                    
                    weatherCurent.Ten = item.Ten;
                    weatherCurent.Datnuoc = item.Datnuoc;
                    weatherCurent.Lat = item.Lat;
                    weatherCurent.Long = item.Long;
                    weatherCurent.Anh = item.Anh;
                    weatherCurent.Nhietdo = (double)Math.Round((double)(item.Nhietdo - 274.15));
                    weatherCurent.MoTa = item.MoTa;
                    weatherCurent.May = item.May;
                    weatherCurent.Tocdogio = item.Tocdogio;
                    weatherCurent.Apsuat = item.Apsuat;
                    weatherCurent.Doam = item.Doam;
                    weatherCurent.NgayTao = String.Format("{0:d/M/yyyy HH:mm:ss}", item.NgayTao);
                    break;
                }
            }
            return Json(weatherCurent, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getNew()
        {
            if(Session[NewSessionn] != null)
            {
                var listsession = (List<blog>)Session[NewSessionn];
                return Json(listsession, JsonRequestBehavior.AllowGet);
            }
            var url = "https://vietnamnews.vn/tags/29390/weather-forecast.html";
            var doc = new HtmlWeb().Load(url);
            var document = doc.DocumentNode.Descendants("div").Where(x => x.GetAttributeValue("class", "").Contains("vnnews-list-news")).FirstOrDefault();
            var listnew = document.Descendants("li").ToList();
            var listblogreturn = new List<blog>();
            foreach(var item in listnew)
            {
                var bloger = new blog();
                var urla = item.Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
                bloger.href = "https://vietnamnews.vn/" + urla;
                bloger.img = item.Descendants("span").Where(x => x.GetAttributeValue("class", "").Contains("vnnews-img-list-news")).FirstOrDefault().Descendants("img").FirstOrDefault().GetAttributeValue("src", "");
                bloger.title = item.Descendants("span").Where(x => x.GetAttributeValue("class", "").Contains("vnnews-tt-list-news")).FirstOrDefault().InnerText;
                bloger.description = item.Descendants("div").FirstOrDefault().InnerText.Replace(bloger.title,"");
                listblogreturn.Add(bloger);
            }
            Session[NewSessionn] = listblogreturn;
            return Json(listblogreturn, JsonRequestBehavior.AllowGet);
        }
    }
    public class APICurent
    {
        public string Ten { get; set; }

        public string Datnuoc { get; set; }

        public double? Lat { get; set; }

        public double? Long { get; set; }

        public double? Nhietdo { get; set; }

        public string Anh { get; set; }

        public double? Doam { get; set; }

        public double? Tocdogio { get; set; }

        public double? Apsuat { get; set; }

        public double? May { get; set; }

        public string MoTa { get; set; }

        public string NgayTao { get; set; }
    }
    public class API5Day
    {
        public int Id { get; set; }

        public string Anh { get; set; }

        public double? Nhietdocao { get; set; }
        public double? Nhietdothap { get; set; }
        public string MoTa { get; set; }

        public double? May { get; set; }

        public double? Tocdogio { get; set; }

        public double? Doam { get; set; }

        public string NgayTao { get; set; }
    }
    //web scrapper new 
    public class blog
    {
        public string href { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
    //web api
    public class namecity
    {
        public int id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public coord coord { get; set; }
    }
    public class coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }
    public class weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
    public class main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double pressure { get; set; }
        public double humidity { get; set; }
        public double sea_level { get; set; }
        public double grnd_level { get; set; }
        public double temp_kf { get; set; }
    }
    public class wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
    }
    public class clouds
    {
        public int all { get; set; }
    }
    public class sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }
    //5day
    public class list
    {
        public int dt { get; set; }
        public string dt_txt { get; set; }
        public main main { get; set; }
        public List<weather> weather { get; set; }
        public clouds clouds { get; set; }
        public wind wind { get; set; }
        public sys sys { get; set; }
    }
    public class city
    {
        public int id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public int population { get; set; }
        public int timezone { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public coord coord { get; set; }
    }
    //class parent
    public class root
    {
        public string date { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
        public int dt { get; set; }
        public int visibility { get; set; }
        public coord coord { get; set; }
        public sys Sys { get; set; }
        public clouds clouds { get; set; }
        public wind wind { get; set; }
        public main main { get; set; }
        public List<weather> weather { get; set; }
    }
    public class weatherDay
    {
        public int cod { get; set; }
        public string message { get; set; }
        public int cn { get; set; }
        public city city { get; set; }
        public List<list> list { get; set; }
    }
}
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using WebScrapper.Models;

namespace WebScrapper.Controllers
{
    public class ScrapperController : Controller
    {
        private readonly WeatherDbContext db = new WeatherDbContext();
        public ActionResult webScrapper()
        {
            return View();
        }
        // GET: Scrapper
        [HttpPost]
        public async Task<ActionResult> Scrappingforecast(string namecity,string lat, string lon)
        {
           try
            {
                var url = "";
                if (namecity == null || namecity == "")
                {
                    lat = "40.7146";
                    lon = "-74.0071";
                    url = string.Format("https://forecast.weather.gov/MapClick.php?lat={0}&lon={1}#.Xr_xH2gzZPZ", lat, lon);
                }
                else
                {
                    //đọc file json
                    StreamReader r = new StreamReader(Server.MapPath("~/Content/city.list.json"));
                    string json = r.ReadToEnd();
                    var readjson = JsonConvert.DeserializeObject<List<namecity>>(json);
                    var listcity = readjson.FirstOrDefault(x => x.name.Contains(namecity));
                    if (listcity == null)
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                    url = string.Format("https://forecast.weather.gov/MapClick.php?lat={0}&lon={1}#.Xr_xH2gzZPZ", listcity.coord.lat, listcity.coord.lon);
                }
                var doc = await Task.Factory.StartNew(() => (new HtmlWeb()).Load(url));
                var current = doc.DocumentNode.Descendants("div").Where(x => x.GetAttributeValue("id", "").Contains("current-conditions")).FirstOrDefault();
                var heading = new currentweather();
                var scrappercurent = new ScrapperCurent();
                heading.nameconditions = current.Descendants("h2").Where(x => x.GetAttributeValue("class", "").Contains("panel-title")).FirstOrDefault().InnerText;
                scrappercurent.Ten = heading.nameconditions;
                heading.smallTXT = current.Descendants("span").Where(x => x.GetAttributeValue("class", "").Contains("smallTxt")).FirstOrDefault().InnerHtml;
                scrappercurent.smallTxt = heading.smallTXT;
                var summy = current.Descendants("div").Where(x => x.GetAttributeValue("id", "").Contains("current_conditions-summary")).FirstOrDefault();
                string currentcond = summy.InnerHtml;
                currentcond = currentcond.Replace(@"newimages/large", "https://forecast.weather.gov/newimages/large");
                currentcond = currentcond.Replace(@"&deg;", "°");
                heading.currentconditions = currentcond.ToString();
                scrappercurent.Anh = "https://forecast.weather.gov/" + summy.Descendants("img").FirstOrDefault().GetAttributeValue("src", "");
                scrappercurent.Mota = summy.Descendants("p").Where(x => x.GetAttributeValue("class", "").Contains("myforecast-current")).FirstOrDefault().InnerText;
                scrappercurent.DoF = summy.Descendants("p").Where(x => x.GetAttributeValue("class", "").Contains("myforecast-current-lrg")).FirstOrDefault().InnerText.Replace(@"&deg;", "°");
                scrappercurent.DoC = summy.Descendants("p").Where(x => x.GetAttributeValue("class", "").Contains("myforecast-current-sm")).FirstOrDefault().InnerText.Replace(@"&deg;", "°");
                string htmltable = current.Descendants("div").Where(x => x.GetAttributeValue("id", "").Contains("current_conditions_detail")).FirstOrDefault().InnerHtml;
                htmltable = htmltable.Replace(@"Humidity", "Độ ẩm");
                htmltable = htmltable.Replace(@"Wind Speed", "Tốc độ gió");
                htmltable = htmltable.Replace(@"Barometer", "Áp kế");
                htmltable = htmltable.Replace(@"Dewpoint", "Nhiệt độ");
                htmltable = htmltable.Replace(@"Visibility", "Hiển thị");
                htmltable = htmltable.Replace(@"Wind Chill", "Gió lạnh");
                htmltable = htmltable.Replace(@"Last update", "Ngày cập nhật");
                heading.table = htmltable.ToString();
                scrappercurent.Chitiet = heading.table;
                scrappercurent.NgayTao = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                var checkscrappercurents = db.ScrapperCurents.FirstOrDefault(x => x.Ten.Contains(scrappercurent.Ten) && x.NgayTao == scrappercurent.NgayTao);
                if(checkscrappercurents == null)
                {
                    db.ScrapperCurents.Add(scrappercurent);
                    db.SaveChanges();

                }
                var serven = doc.DocumentNode.Descendants("div").Where(x => x.GetAttributeValue("id", "").Contains("seven-day-forecast")).FirstOrDefault();
                var panelheading = serven.Descendants("div").Where(x => x.GetAttributeValue("class", "").Contains("panel-heading")).FirstOrDefault();
                var servenheading = panelheading.InnerHtml;
                servenheading = servenheading.Replace(@"Extended Forecast for", "Dự báo thời tiết mở rộng cho:");
                heading.sevendayheading = servenheading.ToString();
                var sevendayforecast = new Scrapper7day();
                sevendayforecast.Ten = panelheading.Descendants("h2").Where(x => x.GetAttributeValue("class", "").Contains("panel-title")).FirstOrDefault().InnerText;

                string servenbody = serven.Descendants("div").Where(x => x.GetAttributeValue("id", "").Contains("seven-day-forecast-container")).FirstOrDefault().InnerHtml;
                servenbody = servenbody.Replace(@"newimages/medium", "https://forecast.weather.gov/newimages/medium");
                servenbody = servenbody.Replace(@"DualImage.php", "https://forecast.weather.gov/DualImage.php");
                servenbody = servenbody.Replace(@"Overnight", "Qua đêm");
                servenbody = servenbody.Replace(@"Today", "Hôm nay");
                servenbody = servenbody.Replace(@"Tonight", "Tối nay");
                servenbody = servenbody.Replace(@"Sunday Night", "Tối chủ nhật");
                servenbody = servenbody.Replace(@"Sunday", "Chủ nhật");
                servenbody = servenbody.Replace(@"Monday Night", "Tối thứ hai");
                servenbody = servenbody.Replace(@"Monday", "Thứ hai");
                servenbody = servenbody.Replace(@"Tuesday Night", "Tối thứ ba");
                servenbody = servenbody.Replace(@"Tuesday", "Thứ ba");
                servenbody = servenbody.Replace(@"Wednesday Night", "Tối thứ tư");
                servenbody = servenbody.Replace(@"Wednesday", "Thứ tư");
                servenbody = servenbody.Replace(@"Thursday Night", "Tối thứ năm");
                servenbody = servenbody.Replace(@"Thursday", "Thứ năm");
                servenbody = servenbody.Replace(@"Friday Night", "Tối thứ sáu");
                servenbody = servenbody.Replace(@"Friday", "Thứ sáu");
                servenbody = servenbody.Replace(@"Saturday Night", "Tối thứ bảy");
                servenbody = servenbody.Replace(@"Saturday", "Thứ bảy");
                servenbody = servenbody.Replace(@"High", "Cao nhất");
                servenbody = servenbody.Replace(@"Low", "Thấp nhất");
                heading.sevendaybody = servenbody.ToString();
                sevendayforecast.NoiDung = heading.sevendaybody;
                sevendayforecast.NgayTao = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                sevendayforecast.CurentId = db.ScrapperCurents.FirstOrDefault(x => x.Ten == scrappercurent.Ten).Id;
                var checkscrapper7day = db.Scrapper7day.FirstOrDefault(x => x.Ten.Contains(sevendayforecast.Ten) && x.CurentId == sevendayforecast.CurentId && x.NgayTao == sevendayforecast.NgayTao);
                if(checkscrapper7day == null)
                {
                    db.Scrapper7day.Add(sevendayforecast);
                    db.SaveChanges();
                }

                var detailforecast = doc.DocumentNode.Descendants("div").Where(x => x.GetAttributeValue("id", "").Contains("detailed-forecast")).FirstOrDefault();
                string detailbody = detailforecast.Descendants("div").Where(x => x.GetAttributeValue("id", "").Contains("detailed-forecast-body")).FirstOrDefault().InnerHtml;
                detailbody = detailbody.Replace(@"Overnight", "Qua đêm");
                detailbody = detailbody.Replace(@"Today", "Hôm nay");
                detailbody = detailbody.Replace(@"Tonight", "Tối nay");
                detailbody = detailbody.Replace(@"Sunday Night", "Tối chủ nhật");
                detailbody = detailbody.Replace(@"Sunday", "Chủ nhật");
                detailbody = detailbody.Replace(@"Monday Night", "Tối thứ hai");
                detailbody = detailbody.Replace(@"Monday", "Thứ hai");
                detailbody = detailbody.Replace(@"Tuesday Night", "Tối thứ ba");
                detailbody = detailbody.Replace(@"Tuesday", "Thứ ba");
                detailbody = detailbody.Replace(@"Wednesday Night", "Tối thứ tư");
                detailbody = detailbody.Replace(@"Wednesday", "Thứ tư");
                detailbody = detailbody.Replace(@"Thursday Night", "Tối thứ năm");
                detailbody = detailbody.Replace(@"Thursday", "Thứ năm");
                detailbody = detailbody.Replace(@"Friday Night", "Tối thứ sáu");
                detailbody = detailbody.Replace(@"Friday", "Thứ sáu");
                detailbody = detailbody.Replace(@"Saturday Night", "Tối thứ bảy");
                detailbody = detailbody.Replace(@"Saturday", "Thứ bảy");
                heading.detailbody = detailbody.ToString();
                var scrapperdetail = new ScrapperDetail();
                scrapperdetail.Ten = sevendayforecast.Ten;
                scrapperdetail.NoiDung = heading.detailbody;
                scrapperdetail.NgayTao = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                scrapperdetail.CurentId = sevendayforecast.CurentId;
                var checkscrapperdetail = db.ScrapperDetails.FirstOrDefault(x => x.Ten.Contains(scrapperdetail.Ten) && x.CurentId == scrapperdetail.CurentId);
                if(checkscrapperdetail == null)
                {
                    db.ScrapperDetails.Add(scrapperdetail);
                    db.SaveChanges();
                }
                return Json(heading, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Search(string address, string date)
        {
            if(address != null && date != null)
            {
                var listcurent = db.ScrapperCurents.ToList();
                var list7day = db.Scrapper7day.ToList();
                var listdetail = db.ScrapperDetails.ToList();
                var datetimesearch = String.Format("{0:M/d/yyyy}", Convert.ToDateTime(date));
                var model = new scrapper();
                foreach (var curent in listcurent)
                {
                    var dt = String.Format("{0:M/d/yyyy}", curent.NgayTao);
                    if(datetimesearch == dt && curent.Ten.Contains(address) == true)
                    {
                        model.CurentId = curent.Id;
                        model.Tencurent = curent.Ten;
                        model.smallTxt = curent.smallTxt;
                        model.Anh = curent.Anh;
                        model.DoC = curent.DoC;
                        model.DoF = curent.DoF;
                        model.Chitietcurent = curent.Chitiet;
                        model.Motacurent = curent.Mota;
                        break;
                    }
                }
                foreach(var sevenday in list7day)
                {
                    var dt = String.Format("{0:M/d/yyyy}", sevenday.NgayTao);
                    if(datetimesearch == dt && sevenday.CurentId == model.CurentId)
                    {
                        model.Ten7day = sevenday.Ten;
                        model.Noidung7day = sevenday.NoiDung;
                        break;
                    }
                }
                foreach(var detail in listdetail)
                {
                    var dt = String.Format("{0:M/d/yyyy}", detail.NgayTao);
                    if(datetimesearch == dt && detail.CurentId == model.CurentId)
                    {
                        model.Noidungdetail = detail.NoiDung;
                        break;
                    }
                }
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
    public class scrapper
    {
        public int CurentId { get; set; }
        public string Tencurent { get; set; }
        public string smallTxt { get; set; }
        public string Anh { get; set; }
        public string DoF { get; set; }
        public string DoC { get; set; }
        public string Chitietcurent { get; set; }
        public string Motacurent { get; set; }
        public string Ten7day { get; set; }
        public string Noidung7day { get; set; }
        public string Noidungdetail { get; set; }
    }
    //web scrapper
    public class currentweather
    {
        public string nameconditions { get; set; }
        public string smallTXT { get; set; }
        public string table { get; set; }
        public string currentconditions { get; set; }
        public string sevendayheading { get; set; }
        public string sevendaybody { get; set; }
        public string detailbody { get; set; }
    }
    public class entrymodel
    {
        public string title { get; set; }
        public string designate { get; set; }
    }
   
    //public void Export(List<EntryModel> models)
    //{

    //    using (var mem = new MemoryStream())
    //    using (var writter = new StreamWriter(mem))
    //    using (var csWriter = new CsvWriter(writter))
    //    {
    //        csWriter.Configuration.Delimiter = ";";

    //        csWriter.WriteField("Title");
    //        csWriter.WriteField("Description");
    //        csWriter.NextRecord();

    //        foreach(var project in models)
    //        {
    //            csWriter.WriteField(project.Title);
    //            csWriter.WriteField(project.Description);
    //            csWriter.NextRecord();
    //        }

    //        writter.Flush();
    //    }
    //}
}
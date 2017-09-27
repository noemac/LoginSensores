using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sensoresapp.Controllers
{
    public class SensorController : Controller
    {
        // GET: Sensor
        [Authorize]
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            #region listar sensores en grilla directamente
            // URL: http://techfunda.com/howto/305/consuming-external-web-api-in-asp-net-mvc
            string url = "http://192.168.0.173:8080/granja/sensores";

            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new
System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                System.Net.Http.HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var table =
Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);

                    var jsonPuro = data;


                    System.Web.UI.WebControls.GridView gView = new
System.Web.UI.WebControls.GridView();
                    gView.DataSource = table;
                    gView.DataBind();
                    using (System.IO.StringWriter sw = new System.IO.StringWriter())
                    {
                        using (System.Web.UI.HtmlTextWriter htw = new
    System.Web.UI.HtmlTextWriter(sw))
                        {
                            gView.RenderControl(htw);
                            ViewBag.noelia = "hola mundo";
                            ViewBag.GridViewString = sw.ToString();
                        }
                    }
                }
            }

            /**/
            #endregion

            return View();
        }

        // GET: Sensor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sensor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sensor/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sensor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sensor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sensor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sensor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Salon.MVC.Client.ViewModels;

namespace Salon.MVC.Client.Controllers
{
    public class SalonSPAController : Controller
    {
        private readonly ILogger _logger;

        public SalonSPAController(ILogger<SalonSPAController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<SalonSPAViewModel> salonSPAViewModels = null;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:53440/api/");
                var responseTask = client.GetAsync("Salon_SPA");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<SalonSPAViewModel>>();
                    readTask.Wait();

                    salonSPAViewModels = readTask.Result;
                    _logger.LogInformation("List of Salon SPA retrieved");
                    _logger.LogTrace("from log trace");
                }
                else //web api sent error response 
                {
                    //log response status here..

                    salonSPAViewModels = Enumerable.Empty<SalonSPAViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    _logger.LogError("error occurred whiel retrieving Salon SPA");
                    
                }
            }

            return View(salonSPAViewModels);
        }

        public ActionResult AddSalonSPA()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSalonSPA(SalonSPAViewModel salonSPA)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53440/api/Salon_SPA");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<SalonSPAViewModel>("Salon_SPA", salonSPA);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(salonSPA);
        }


        public ActionResult EditSalonSPA(int id)
        {
            SalonSPAViewModel salonSPA = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53440/api/Salon_SPA");
                //HTTP GET
                var responseTask = client.GetAsync("Salon_SPA?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<SalonSPAViewModel>();

                    readTask.Wait();

                    salonSPA = readTask.Result;
                }
            }
            return View(salonSPA);
        }

        [HttpPost]
        public ActionResult EditSalonSPA(SalonSPAViewModel salonSPA)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53440/api/Salon_SPA");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<SalonSPAViewModel>("Salon_SPA", salonSPA);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(salonSPA);
        }


        public ActionResult DeleteSalonSPA(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53440/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("Salon_SPA/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }

}
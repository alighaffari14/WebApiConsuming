using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebAPiConsuming.Models;

namespace WebAPiConsuming.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            IEnumerable<Employee> employees = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44330/api/");
                var responseTask = client.GetAsync("Employee");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Employee>>();
                    readTask.Wait();
                    employees = readTask.Result;
                }
                else
                {
                    //Error response received   
                    employees = Enumerable.Empty<Employee>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(employees);
        }
    }
}
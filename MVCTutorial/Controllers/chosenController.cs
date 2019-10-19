using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTutorial.Models;

namespace MVCTutorial.Controllers
{
    public class chosenController : Controller
    {
        
        public ActionResult AddOrEdit(int id = 0)
        {
            SelectedEmployee emp = new SelectedEmployee();
            using (MVCTutorialEntities2 db = new MVCTutorialEntities2())
            {
                emp.EmployeeCollection = db.Employees.ToList();
            }
                return View(emp);
        }

        [HttpPost]
        public ActionResult AddOrEdit(SelectedEmployee emp)
        {
            using (MVCTutorialEntities2 db = new MVCTutorialEntities2())
            {
                db.SelectedEmployees.Add(emp);
                db.SaveChanges();
            }
            return RedirectToAction("AddOrEdit", new { id = 0 });
                //return View();
        }
    }
}
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DoctorOffice.Models;

namespace DoctorOffice.Controllers
{
    public class DoctorController : Controller
    {
        [HttpGet("/Doctor")]
        public ActionResult Index()
        {
            List<Doctor> allDoctors= Doctor.GetAll();
            return  View(allDoctors);

        }

         [HttpPost("/Doctor")]
        public ActionResult Create()
        {
            
            Doctor newDoctor =new Doctor(Request.Form["new-doctor"]);
            newDoctor.Save();
            List<Doctor> allDoctors= Doctor.GetAll();
             return RedirectToAction("Index");

        }




    }
}

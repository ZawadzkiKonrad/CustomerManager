using DocumentFormat.OpenXml.Wordprocessing;
using MasterDev.Application.Interfaces;
using MasterDev.Application.ViewModels.Klient;
using MasterDev.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterDev.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly IKlientService _klientService;
        public TestController(IKlientService klientService)
        {
            _klientService = klientService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var klient = _klientService.GetAllKlients();
            return View(klient);
        }


        [HttpGet]
        public IActionResult AddKlient()
        {
            return View(new NewKlientVm());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddKlient(NewKlientVm model)
        {
            if (ModelState.IsValid)
            {
                var id = _klientService.AddKlient(model);
                return RedirectToAction("Index");
            }
            return View(model);

        }


        [HttpGet]
        public IActionResult EditKlient(int id)
        {
            var klient = _klientService.GetKlientForEdit(id);
            return View(klient);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditKlient(NewKlientVm model)
        {
            if (ModelState.IsValid)
            {
                _klientService.UpdateKlient(model);
                return RedirectToAction("Index");
            }
            return View(model);

        }

        public IActionResult Delete(int id)
        {
            _klientService.DeleteKlient(id);
            return RedirectToAction("Index");
        }

        public IActionResult CSV()
        {
            var klient = _klientService.GetAllKlients().ToList();
            var builder = new StringBuilder();
            builder.AppendLine("Id,Name,Surname");
            foreach (var kli in klient)
            {
                builder.AppendLine($"{kli.Id},{kli.Name},{kli.Surname}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Employeeinfo.csv");

        }

        public IActionResult Import(IFormFile file)
        {
            _klientService.Import(file);
            return View();
        }
    }
}

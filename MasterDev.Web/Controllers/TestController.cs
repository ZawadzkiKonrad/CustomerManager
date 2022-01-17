using AspNetCoreHero.ToastNotification.Abstractions;
using CsvHelper;
using DocumentFormat.OpenXml.Wordprocessing;
using MasterDev.Application.Interfaces;
using MasterDev.Application.ViewModels.Klient;
using MasterDev.Domain.Model;
using MasterDev.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterDev.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly IKlientService _klientService;
        private readonly INotyfService _notyf;
        private readonly Context _context;
        private readonly ILogger<TestController> _logger;
        public TestController(IKlientService klientService, INotyfService notyf,Context context, ILogger<TestController> logger)
        {
            _logger = logger;
            _klientService = klientService;
            _notyf = notyf;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Jestem w Test/Index");
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
                _notyf.Success("Dane zostały przesłane do bazy!");
                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogWarning("zle dane przy dodawaniu klienta, blad validacji");
            }
            return View(model);

        }


        [HttpGet]
        public IActionResult EditKlient(int id)
        {
            
            var klient = _klientService.GetKlientForEdit(id);
                if (klient!=null)
                {   
                    return View(klient);
                }
                else
                {
                    _notyf.Error("Brak klienta do edycji!");
                    _logger.LogWarning("Proba edycji nie stworzonego rekordu");
                    return RedirectToAction("Index");
                }
                        
           
            
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditKlient(NewKlientVm model)
        {
            if (model!=null)
            {
                try
                {
                  if (ModelState.IsValid)
                              {
                                    _klientService.UpdateKlient(model);
                                    _notyf.Success("Dane zostały zaktualizowane!");
                                    return RedirectToAction("Index");
                                }
                }
                catch (Exception e)
                {

                    _notyf.Error("Dane nie zostały zaktualizowane!");
                    _logger.LogWarning("Proba edycji, zle wpisanie danych, blad validacji",e);
                    return RedirectToAction("Index");
                }

              
                return View(model);
            }
            else
            {
                _notyf.Error("Brak pliku do edycji!");
                _logger.LogWarning("Proba edycji nie stworzonego rekordu");
                return RedirectToAction("Index");
            }   
        }

        public IActionResult Delete(IEnumerable <int> id)

        {
            if (id.Count()!=0)
            {
                

                try
            {
                _klientService.DeleteKlients(id);            
                _context.SaveChanges();
                _notyf.Success("Dane zostały usunięte!");
                
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _notyf.Error("Błąd - nie ma takiego klienta");
                  _logger.LogWarning("Proba usuniecia nie stworzonego rekordu",e);
                    return RedirectToAction("Index");
                throw;
            }
            }
            else
            {
                _notyf.Error("Nie wybrałeś klientów do usunięcia");
                _logger.LogWarning("Proba usuniecia nie stworzonego rekordu");
                return RedirectToAction("Index");
            }
            
            
            
            
        }
        
    

        public IActionResult CSV()
        {
            var klient = _klientService.GetAllKlients().ToList();
            var builder = new StringBuilder();
            builder.AppendLine(@"Id;Name;Surname;BirthYear");
            foreach (var kli in klient)
            {
                builder.AppendLine(String.Format(@"{0};{1};{2};{3}", kli.Id, kli.Name, kli.Surname, kli.BirthYear));
               // builder.AppendLine($"{kli.Id},{kli.Name},{kli.Surname},{kli.BirthYear}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Employeeinfo.csv");

        }

        public IActionResult Import(IFormFile file)
        {
            if (file != null)
            {


                try
                {
                    _klientService.Import(file);
                    _notyf.Success("Dane zostały przesłane do bazy!");
                   
                    return RedirectToAction("Index");
                    //return View();
                }
                catch (Exception e )
                {


                    _notyf.Error("Nieprawidłowy format pliku");
                    _logger.LogWarning("Proba importu - nieprawidlowy format",e);

                    return RedirectToAction("Index");
                }
            }
            else
            {
                _notyf.Error("Brak pliku");
                
                return RedirectToAction("Index");
            }
            


        } public IActionResult ImportExcel(IFormFile file)
        {
            if (file!=null)
            {            
            try
                {
                    _klientService.ImportExcel(file);
                   // _notyf.Success("Dane zostały przesłane do bazy!");
                    
                    return RedirectToAction("Index");
                }
            catch (Exception e)
                {
                    _notyf.Error("Nieprawidłowy format pliku");
                    _logger.LogWarning("Proba importuExcel - nieprawidlowy format", e);

                    return RedirectToAction("Index");
                
                }
            }
            else
            {
                _notyf.Error("Brak pliku");
                
                return RedirectToAction("Index");
            }

        }
    }
}











//var comlumHeadrs = new string[]
//            {
//                "Id",
//                "Name",
//                "Surname",
//                "BirthYear",

//            };

//var employeeRecords = (from employee in _klientService.GetAllKlients()
//                       select new object[]
//                       {
//                                            employee.Id,
//                                            $"{employee.Name}",
//                                            $"\"{employee.Surname}\"", //Escaping ","
//                                            $"\"{employee.BirthYear.ToString("$#,0.00;($#,0.00)")}\"", //Escaping ","

//                       }).ToList();

//// Build the file content
//var employeecsv = new StringBuilder();
//employeeRecords.ForEach(line =>
//{
//    employeecsv.AppendLine(string.Join(",", line));
//});

//byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", comlumHeadrs)}\r\n{employeecsv.ToString()}");
//return File(buffer, "text/csv", $"Employee.csv");



//public IActionResult CSV()
//{
//    var records = _klientService.GetAllKlients();
//    using (var writer = new StreamWriter(@"C:\Users\48797\Desktop\file.csv"))
//    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
//    {
//        csv.WriteRecords(records);
//    }
//    return RedirectToAction("Index");

//}


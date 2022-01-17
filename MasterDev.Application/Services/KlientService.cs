using AutoMapper;
using AutoMapper.QueryableExtensions;

using MasterDev.Application.Interfaces;
using MasterDev.Application.ViewModels.Klient;
using MasterDev.Domain.Interfaces;
using MasterDev.Domain.Model;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using TinyCsvParser;
using MasterDev.Application.Mapping;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace MasterDev.Application.Services
{
    public class KlientService : IKlientService
    {
        private readonly IKlientRepository _klientRepo;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;

        public KlientService(IKlientRepository klientRepo,
         IMapper mapper, INotyfService notyf)
        {
            _klientRepo = klientRepo;
            _mapper = mapper;
            _notyf = notyf;
        }

        public int AddKlient(NewKlientVm model)
        {
            var klient = _mapper.Map<Domain.Model.Klient>(model);
            var id = _klientRepo.AddKlient(klient);
            return id;
        }

         public void DeleteKlient(int id)
            {
                _klientRepo.DeleteKlient(id);
            }
        public void DeleteKlients(IEnumerable<int> id)
                {
                    _klientRepo.DeleteKlients(id);
                }
       
        public IQueryable<KlientForListVm> GetAllKlients()
        {

            var klients = _klientRepo.GetAllKlients()
            .ProjectTo<KlientForListVm>(_mapper.ConfigurationProvider);
            return klients;
        }

        public NewKlientVm GetKlientForEdit(int id)
        {
            
            var klient = _klientRepo.GetKlientById(id);
            var klientVm = _mapper.Map<NewKlientVm>(klient);
            return klientVm;
        }
        public void UpdateKlient(NewKlientVm model)
        {
            
            var klient = _mapper.Map<Domain.Model.Klient>(model);
                        _klientRepo.UpdateKlient(klient);

        }

        public void Import(IFormFile file)
        {
            
                var read = file.OpenReadStream(); //IoFormFile to stream

                string name = file.FileName;
                string existing = name.Substring(Math.Max(0, name.Length - 4));// pobieranie 4 ostatnich znakow(formatu)

                //if (existing==".csv")
                //{
                using (var reader = new StreamReader(read))
                {

                    using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
                    {
                        csv.Context.RegisterClassMap<KlientMap>();

                        var records = csv.GetRecords<Klient>().ToList();
                        foreach (var item in records)
                        {
                            _klientRepo.AddKlient(item);
                        }

                    }

                }
                //}
                        

        }

        public void ImportExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var list = new List<Klient>();
            using (var stream = new MemoryStream())
            {
                file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    if ( worksheet.Cells[1, 2].Value.ToString().Trim() == "Name" && worksheet.Cells[1, 3].Value.ToString().Trim() == "Surname")
                    {

                    
                    for (int row = 2; row <= rowcount; row++)
                    {
                        list.Add(new Klient
                        {


                            Name = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            Surname = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            BirthYear = int.Parse(worksheet.Cells[row, 4].Value.ToString().Trim())
                        });

                    }
                        _notyf.Success("Dane zostały przesłane do bazy!");

                    }
                    else
                    {
                        _notyf.Error("Nieprawidłowy format pliku");
                    }
                }
            }
            foreach (var item in list)
            {
                _klientRepo.AddKlient(item);
            }

        }

        

       
    }
}


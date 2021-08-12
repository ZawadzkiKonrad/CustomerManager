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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterDev.Application.Services
{
    public class KlientService : IKlientService
    {
        private readonly IKlientRepository _klientRepo;
        private readonly IMapper _mapper;

        public KlientService(IKlientRepository klientRepo,
         IMapper mapper)
        {
            _klientRepo = klientRepo;
            _mapper = mapper;
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

        public IQueryable< KlientForListVm> GetAllKlients()
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

        public void Import(IFormFile file)
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
                    for (int row = 2; row <= rowcount; row++)
                    {
                        list.Add(new Klient
                        {

                            
                            Name = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            Surname = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            BirthYear = int.Parse(worksheet.Cells[row, 4].Value.ToString().Trim())
                        });

                    }
                }
            }
            foreach (var item in list)
            {
                _klientRepo.AddKlient(item);
            }
            
        }

        public void UpdateKlient(NewKlientVm model)
        {
            var klient = _mapper.Map<Domain.Model.Klient>(model);
            _klientRepo.UpdateKlient(klient);

        }

       
    }
}

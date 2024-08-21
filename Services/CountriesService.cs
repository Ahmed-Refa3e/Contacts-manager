using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _CountriesRepository;

        public CountriesService(ICountriesRepository CountriesRepository)
        {
            _CountriesRepository = CountriesRepository;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? request)
        {
            
            if (request?.CountryName == null)
            {
                throw new System.ArgumentException("CountryName cannot be null");
            }

            if (await _CountriesRepository.GetCountryByCountryName(request.CountryName) != null) 
            {
                throw new System.ArgumentException("CountryName already exists");
            }

            if (request == null)
            {
                throw new System.ArgumentException("Request cannot be null");
            }

            Country country = request.ToCountry();

            country.CountryID = Guid.NewGuid();

            country.CountryName = request.CountryName;

            await _CountriesRepository.AddCountry(country);

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
             var Countries = await _CountriesRepository.GetAllCountries();

            return Countries.Select(c => c.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? CountryID)
        {
            if (CountryID == null)
            {
                return null;
            }

            Country? country = await _CountriesRepository.GetCountryByCountryID(CountryID.Value);

            if (country == null)
            {
                return null;
            }

            return country.ToCountryResponse();
        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new();
            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Countries"];

                int rowCount = workSheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string? countryName = cellValue;
                        CountryAddRequest country = new() { CountryName = countryName };
                        if (await _CountriesRepository.GetCountryByCountryName(countryName) != null)
                        { continue; }
                        await AddCountry(country);
                        countriesInserted++;
                    }
                }
            }
            return countriesInserted;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI;
using MarketPlace.Models;
using Kendo.Mvc.Extensions;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace MarketPlace.Controllers
{
    public class CategoryController : Controller
    {
        private string baseURL = @"https://marketplaceapicore.azurewebsites.net";
        //private string baseURL = @"https://localhost:44364";
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory([FromBody] CreateCategoryRequestDto categoryData)
        {
            HttpClient client = new HttpClient();
            
            HttpResponseMessage response = await client.PostAsJsonAsync(
            $"{baseURL}/categories/", categoryData);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.


            return Json(null);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateCategory([FromBody] CategoryDto categoryData)
        {
            HttpClient client = new HttpClient();
            //CreateCategoryRequestDto dto = new CreateCategoryRequestDto();
            //dto.buildingId = categoryData.buildingId;
            //dto.categoryId = categoryData.categoryId;
            //dto.categoryIdentifier = categoryData.categoryIdentifier;
            //dto.ipAddress = categoryData.ipAddress;
            

            HttpResponseMessage response = await client.PutAsJsonAsync(
            $"{baseURL}/categories/{categoryData.Id}", categoryData);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.


            return Json(null);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteCategory([FromBody] CategoryDto categoryDto)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync(
            $"{baseURL}/categories/{categoryDto.Id}");
            
            // return URI of the created resource.


            return Json(categoryDto.Id);
        }
        [HttpPost]
        public async Task<ActionResult> GetCategory([FromBody] CategoryDto categoryDto)
        {
            HttpClient client = new HttpClient();

            CategoryDto resCategory = null;
            HttpResponseMessage response = await client.GetAsync($"{baseURL}/categories/{categoryDto.Id}");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                resCategory = await response.Content.ReadAsAsync<CategoryDto>();
            }

            // return URI of the created resource.


            return Json(resCategory);
        }
        
        public async Task<ActionResult> Categories_Read([DataSourceRequest]DataSourceRequest request)
        {

            HttpClient client = new HttpClient();

            List<CategoryDto> categories = null;

            HttpResponseMessage response = await client.GetAsync($"{baseURL}/categories/");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                categories = await response.Content.ReadAsAsync<List<CategoryDto>>();
            }

            var dsResult = categories.ToDataSourceResult(request);
            return Json(dsResult);
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Category_Create([DataSourceRequest] DataSourceRequest request, CreateCategoryRequestDto categoryData)
        {
            HttpClient client = new HttpClient();

            if (categoryData != null && ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(
                $"{baseURL}/categories/", categoryData);
                response.EnsureSuccessStatusCode();

                // return URI of the created resource.

            }

            return Json(new[] { categoryData }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Category_Update([DataSourceRequest] DataSourceRequest request, CategoryDto categoryData)
        {
            CategoryDto result = null;

            HttpClient client = new HttpClient();
            if (categoryData != null && ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(
                $"{baseURL}/categories/{categoryData.Id}", categoryData);
                response.EnsureSuccessStatusCode();

                // Deserialize the updated product from the response body.
                result = await response.Content.ReadAsAsync<CategoryDto>();
            }

            return Json(new[] { result }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Category_Destroy([DataSourceRequest] DataSourceRequest request, CategoryDto categoryData)
        {
            HttpClient client = new HttpClient();

            if (categoryData != null)
            {
                HttpResponseMessage response = await client.DeleteAsync(
               $"{baseURL}/categories/{categoryData.Id}");
            }

            return Json(new[] { categoryData }.ToDataSourceResult(request, ModelState));
        }

        public async Task<ActionResult> GetCategoriesDropdownData()
        {
            HttpClient client = new HttpClient();

            List<CategoryDto> categories = null;

            HttpResponseMessage response = await client.GetAsync($"{baseURL}/categories/");
            
            if (response.IsSuccessStatusCode)
            {
                categories = await response.Content.ReadAsAsync<List<CategoryDto>>();
            }

            
            if (categories != null)
            {
                var list = new List<SelectListItem>();
                list.AddRange(categories.Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }));
                return Json(list.OrderBy(x => x.Text));
            }

            return Json(null);
        }
    }
}

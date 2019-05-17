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

namespace MarketPlace.Controllers
{
    public class PointController : Controller
    {
        private string baseURL = @"https://marketplaceapicore.azurewebsites.net";
        //private string baseURL = @"https://localhost:44364";
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddPoint([FromBody] CreatePointRequestDto pointData)
        {
            HttpClient client = new HttpClient();


            HttpResponseMessage response = await client.PostAsJsonAsync(
            $"{baseURL}/points/", pointData);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.


            return Json(null);
        }
        [HttpPost]
        public async Task<ActionResult> UpdatePoint([FromBody] PointDto pointData)
        {
            HttpClient client = new HttpClient();


            HttpResponseMessage response = await client.PutAsJsonAsync(
            $"{baseURL}/points/{pointData.id}", pointData);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.


            return Json(null);
        }
        [HttpPost]
        public async Task<ActionResult> DeletePoint([FromBody] PointDto clientDto)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync(
            $"{baseURL}/points/{clientDto.id}");
            
            // return URI of the created resource.


            return Json(clientDto.id);
        }
        [HttpPost]
        public async Task<ActionResult> GetPoint([FromBody] PointDto clientDto)
        {
            HttpClient client = new HttpClient();

            PointDto resPoint = null;
            HttpResponseMessage response = await client.GetAsync($"{baseURL}/points/{clientDto.id}");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                resPoint = await response.Content.ReadAsAsync<PointDto>();
            }

            // return URI of the created resource.


            return Json(resPoint);
        }
        public async Task<ActionResult> Points_Read([DataSourceRequest]DataSourceRequest request)
        {

            HttpClient client = new HttpClient();

            List<PointDto> points = null;

            HttpResponseMessage response = await client.GetAsync($"{baseURL}/points/");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                points = await response.Content.ReadAsAsync<List<PointDto>>();
            }

            var dsResult = points.ToDataSourceResult(request);
            return Json(dsResult);
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Point_Create([DataSourceRequest] DataSourceRequest request, CreatePointRequestDto pointData)
        {
            HttpClient client = new HttpClient();

            if (pointData != null && ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(
                $"{baseURL}/points/", pointData);
                response.EnsureSuccessStatusCode();

                // return URI of the created resource.

            }

            return Json(new[] { pointData }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Point_Update([DataSourceRequest] DataSourceRequest request, PointDto pointData)
        {
            PointDto result = null;

            HttpClient client = new HttpClient();
            if (pointData != null && ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(
                $"{baseURL}/points/{pointData.id}", pointData);
                response.EnsureSuccessStatusCode();

                // Deserialize the updated product from the response body.
                result = await response.Content.ReadAsAsync<PointDto>();
            }

            return Json(new[] { result }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Point_Destroy([DataSourceRequest] DataSourceRequest request, PointDto pointData)
        {
            HttpClient client = new HttpClient();

            if (pointData != null)
            {
                HttpResponseMessage response = await client.DeleteAsync(
               $"{baseURL}/points/{pointData.id}");
            }

            return Json(new[] { pointData }.ToDataSourceResult(request, ModelState));
        }
        public async Task<ActionResult> ImportPoint(IFormFile file)
        {
            HttpClient client = new HttpClient();

            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream()))
                data = br.ReadBytes((int)file.OpenReadStream().Length);

            ByteArrayContent bytes = new ByteArrayContent(data);


            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            multiContent.Add(bytes, "file", file.FileName);


            HttpResponseMessage response = await client.PostAsync(
            $"{baseURL}/points/import", multiContent);
            response.EnsureSuccessStatusCode();

            return Json("Success");
        }
    }
}

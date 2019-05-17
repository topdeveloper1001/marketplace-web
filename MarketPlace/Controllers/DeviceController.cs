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
    public class DeviceController : Controller
    {
        private string baseURL = @"https://marketplaceapicore.azurewebsites.net";
        //private string baseURL = @"https://localhost:44364";
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddDevice([FromBody] CreateDeviceRequestDto deviceData)
        {
            HttpClient client = new HttpClient();


            HttpResponseMessage response = await client.PostAsJsonAsync(
            $"{baseURL}/devices/", deviceData);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.


            return Json(null);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateDevice([FromBody] DeviceDto deviceData)
        {
            HttpClient client = new HttpClient();
            //CreateDeviceRequestDto dto = new CreateDeviceRequestDto();
            //dto.buildingId = deviceData.buildingId;
            //dto.clientId = deviceData.clientId;
            //dto.deviceIdentifier = deviceData.deviceIdentifier;
            //dto.ipAddress = deviceData.ipAddress;
            

            HttpResponseMessage response = await client.PutAsJsonAsync(
            $"{baseURL}/devices/{deviceData.id}", deviceData);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.


            return Json(null);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteDevice([FromBody] DeviceDto clientDto)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync(
            $"{baseURL}/devices/{clientDto.id}");
            
            // return URI of the created resource.


            return Json(clientDto.id);
        }
        [HttpPost]
        public async Task<ActionResult> GetDevice([FromBody] DeviceDto clientDto)
        {
            HttpClient client = new HttpClient();

            DeviceDto resDevice = null;
            HttpResponseMessage response = await client.GetAsync($"{baseURL}/devices/{clientDto.id}");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                resDevice = await response.Content.ReadAsAsync<DeviceDto>();
            }

            // return URI of the created resource.


            return Json(resDevice);
        }
        
        public async Task<ActionResult> Devices_Read([DataSourceRequest]DataSourceRequest request)
        {

            HttpClient client = new HttpClient();

            List<DeviceDto> devices = null;

            HttpResponseMessage response = await client.GetAsync($"{baseURL}/devices/");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                devices = await response.Content.ReadAsAsync<List<DeviceDto>>();
            }

            var dsResult = devices.ToDataSourceResult(request);
            return Json(dsResult);
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Device_Create([DataSourceRequest] DataSourceRequest request, CreateDeviceRequestDto deviceData)
        {
            HttpClient client = new HttpClient();

            if (deviceData != null && ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(
                $"{baseURL}/devices/", deviceData);
                response.EnsureSuccessStatusCode();

                // return URI of the created resource.

            }

            return Json(new[] { deviceData }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Device_Update([DataSourceRequest] DataSourceRequest request, DeviceDto deviceData)
        {
            DeviceDto result = null;

            HttpClient client = new HttpClient();
            if (deviceData != null && ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(
                $"{baseURL}/devices/{deviceData.id}", deviceData);
                response.EnsureSuccessStatusCode();

                // Deserialize the updated product from the response body.
                result = await response.Content.ReadAsAsync<DeviceDto>();
            }

            return Json(new[] { result }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Device_Destroy([DataSourceRequest] DataSourceRequest request, DeviceDto deviceData)
        {
            HttpClient client = new HttpClient();

            if (deviceData != null)
            {
                HttpResponseMessage response = await client.DeleteAsync(
               $"{baseURL}/devices/{deviceData.id}");
            }

            return Json(new[] { deviceData }.ToDataSourceResult(request, ModelState));
        }

        public async Task<ActionResult> ImportDevice(IFormFile file)
        {
            HttpClient client = new HttpClient();

            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream()))
                data = br.ReadBytes((int)file.OpenReadStream().Length);

            ByteArrayContent bytes = new ByteArrayContent(data);


            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            multiContent.Add(bytes, "file", file.FileName);


            HttpResponseMessage response = await client.PostAsync(
            $"{baseURL}/devices/import", multiContent);
            response.EnsureSuccessStatusCode();

            return Json("Success");
        }
    }
}

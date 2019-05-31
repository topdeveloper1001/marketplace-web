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
using Microsoft.AspNetCore.Hosting;
namespace MarketPlace.Controllers
{
    public class ConnectorController : Controller
    {
        private string baseURL = @"https://marketplaceapicore.azurewebsites.net";
        private readonly IHostingEnvironment _hostingEnvironment;
        //private string baseURL = @"https://localhost:44364";
        #region Connector
        public ConnectorController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<ActionResult> Index()
        {
            HttpClient client = new HttpClient();

            List<CategoryDto> categories = null;

            HttpResponseMessage response = await client.GetAsync($"{baseURL}/categories/");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                categories = await response.Content.ReadAsAsync<List<CategoryDto>>();
            }
            return View(categories);
        }

        [HttpPost]
        public async Task<ActionResult> AddConnector(CreateConnectorRequestDto connectorData, IFormFile file)
        {
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads\\ConnectorIcons");
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString().Replace("-", "") +
                                Path.GetExtension(file.FileName);
                var filepath = Path.Combine(uploads, fileName);
                
                using (var s = new FileStream(filepath, FileMode.Create))
                {
                    await file.CopyToAsync(s);
                    connectorData.Icon = $"Uploads\\ConnectorIcons\\{fileName}";
                }
            }
            

            HttpClient client = new HttpClient();
            
            HttpResponseMessage response = await client.PostAsJsonAsync(
            $"{baseURL}/connectors/", connectorData);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.


            return Json(null);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateConnector(ConnectorDto connectorData, IFormFile file)
        {
            if (file != null)
            {
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads\\ConnectorIcons");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString().Replace("-", "") +
                                    Path.GetExtension(file.FileName);
                    var filepath = Path.Combine(uploads, fileName);

                    using (var s = new FileStream(filepath, FileMode.Create))
                    {
                        await file.CopyToAsync(s);
                        connectorData.Icon = $"Uploads\\ConnectorIcons\\{fileName}";
                    }
                }
            }
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.PutAsJsonAsync(
            $"{baseURL}/connectors/{connectorData.Id}", connectorData);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.


            return Json(null);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteConnector([FromBody] ConnectorDto connectorDto)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync(
            $"{baseURL}/connectors/{connectorDto.Id}");
            
            // return URI of the created resource.


            return Json(connectorDto.Id);
        }
        [HttpPost]
        public async Task<ActionResult> GetConnector([FromBody] ConnectorDto connectorDto)
        {
            HttpClient client = new HttpClient();

            ConnectorDto resConnector = null;
            HttpResponseMessage response = await client.GetAsync($"{baseURL}/connectors/{connectorDto.Id}");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                resConnector = await response.Content.ReadAsAsync<ConnectorDto>();
            }

            // return URI of the created resource.


            return Json(resConnector);
        }
        
        public async Task<ActionResult> Connectors_Read([DataSourceRequest]DataSourceRequest request, Guid categoryId)
        {

            HttpClient client = new HttpClient();

            List<ConnectorDto> connectors = null;

            HttpResponseMessage response = await client.GetAsync($"{baseURL}/connectorsbycategory/{categoryId}");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                connectors = await response.Content.ReadAsAsync<List<ConnectorDto>>();
            }

            var dsResult = connectors.ToDataSourceResult(request);
            return Json(dsResult);
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Connector_Create([DataSourceRequest] DataSourceRequest request, CreateConnectorRequestDto connectorData)
        {
            HttpClient client = new HttpClient();

            if (connectorData != null && ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(
                $"{baseURL}/connectors/", connectorData);
                response.EnsureSuccessStatusCode();

                // return URI of the created resource.

            }

            return Json(new[] { connectorData }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Connector_Update([DataSourceRequest] DataSourceRequest request, ConnectorDto connectorData)
        {
            ConnectorDto result = null;

            HttpClient client = new HttpClient();
            if (connectorData != null && ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(
                $"{baseURL}/connectors/{connectorData.Id}", connectorData);
                response.EnsureSuccessStatusCode();

                // Deserialize the updated product from the response body.
                result = await response.Content.ReadAsAsync<ConnectorDto>();
            }

            return Json(new[] { result }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Connector_Destroy([DataSourceRequest] DataSourceRequest request, ConnectorDto connectorData)
        {
            HttpClient client = new HttpClient();

            if (connectorData != null)
            {
                HttpResponseMessage response = await client.DeleteAsync(
               $"{baseURL}/connectors/{connectorData.Id}");
            }

            return Json(new[] { connectorData }.ToDataSourceResult(request, ModelState));
        }
        #endregion
        #region ConnectorConfig
        public async Task<ActionResult> ConnectorConfig(Guid cid)
        {
            HttpClient client = new HttpClient();

            ConnectorDto resConnector = null;
            HttpResponseMessage response = await client.GetAsync($"{baseURL}/connectors/{cid.ToString()}");
            if (response.IsSuccessStatusCode)
            {
                resConnector = await response.Content.ReadAsAsync<ConnectorDto>();
            }
            return View(resConnector);
        }
        [HttpPost]
        public async Task<ActionResult> AddConnectorConfig([FromBody] CreateConnectorConfigRequestDto connectorconfigData)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.PostAsJsonAsync(
            $"{baseURL}/connectorconfigs/", connectorconfigData);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.


            return Json(null);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateConnectorConfig([FromBody] ConnectorConfigDto connectorconfigData)
        {
            HttpClient client = new HttpClient();
            //CreateConnectorConfigRequestDto dto = new CreateConnectorConfigRequestDto();
            //dto.buildingId = connectorconfigData.buildingId;
            //dto.connectorconfigId = connectorconfigData.connectorconfigId;
            //dto.connectorconfigIdentifier = connectorconfigData.connectorconfigIdentifier;
            //dto.ipAddress = connectorconfigData.ipAddress;


            HttpResponseMessage response = await client.PutAsJsonAsync(
            $"{baseURL}/connectorconfigs/{connectorconfigData.Id}", connectorconfigData);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.


            return Json(null);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteConnectorConfig([FromBody] ConnectorConfigDto connectorconfigDto)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync(
            $"{baseURL}/connectorconfigs/{connectorconfigDto.Id}");

            // return URI of the created resource.


            return Json(connectorconfigDto.Id);
        }
        [HttpPost]
        public async Task<ActionResult> GetConnectorConfig([FromBody] ConnectorConfigDto connectorconfigDto)
        {
            HttpClient client = new HttpClient();

            ConnectorConfigDto resConnectorConfig = null;
            HttpResponseMessage response = await client.GetAsync($"{baseURL}/connectorconfigs/{connectorconfigDto.Id}");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                resConnectorConfig = await response.Content.ReadAsAsync<ConnectorConfigDto>();
            }

            // return URI of the created resource.


            return Json(resConnectorConfig);
        }

        public async Task<ActionResult> ConnectorConfigs_Read([DataSourceRequest]DataSourceRequest request, Guid connectorId)
        {

            HttpClient client = new HttpClient();

            List<ConnectorConfigDto> connectorconfigs = null;

            HttpResponseMessage response = await client.GetAsync($"{baseURL}/connectorconfigsbyconnector/{connectorId}");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                connectorconfigs = await response.Content.ReadAsAsync<List<ConnectorConfigDto>>();
            }

            var dsResult = connectorconfigs.ToDataSourceResult(request);
            return Json(dsResult);
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> ConnectorConfig_Create([DataSourceRequest] DataSourceRequest request, CreateConnectorConfigRequestDto connectorconfigData)
        {
            HttpClient client = new HttpClient();

            if (connectorconfigData != null && ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(
                $"{baseURL}/connectorconfigs/", connectorconfigData);
                response.EnsureSuccessStatusCode();

                // return URI of the created resource.

            }

            return Json(new[] { connectorconfigData }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> ConnectorConfig_Update([DataSourceRequest] DataSourceRequest request, ConnectorConfigDto connectorconfigData)
        {
            ConnectorConfigDto result = null;

            HttpClient client = new HttpClient();
            if (connectorconfigData != null && ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(
                $"{baseURL}/connectorconfigs/{connectorconfigData.Id}", connectorconfigData);
                response.EnsureSuccessStatusCode();

                // Deserialize the updated product from the response body.
                result = await response.Content.ReadAsAsync<ConnectorConfigDto>();
            }

            return Json(new[] { result }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> ConnectorConfig_Destroy([DataSourceRequest] DataSourceRequest request, ConnectorConfigDto connectorconfigData)
        {
            HttpClient client = new HttpClient();

            if (connectorconfigData != null)
            {
                HttpResponseMessage response = await client.DeleteAsync(
               $"{baseURL}/connectorconfigs/{connectorconfigData.Id}");
            }

            return Json(new[] { connectorconfigData }.ToDataSourceResult(request, ModelState));
        }

        #endregion
    }
}

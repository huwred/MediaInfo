using System.Collections.Generic;
using System.Net.Http;
using MetadataExtractor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.StorageProviders.AzureBlob;

namespace MediaInfo.Controllers
{
    [PluginController("MediaInfo")]
    public class MediaInfoBackofficeApiController : UmbracoAuthorizedJsonController
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MediaInfoBackofficeApiController(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContext;
        }

        [HttpPost]
        public IReadOnlyList<MetadataExtractor.Directory> GetFileInfo(ApiInstruction apiInstruction)
        {
            var filePath = _webHostEnvironment.WebRootPath + apiInstruction.ImageUrl;
            var filInfo = _webHostEnvironment.WebRootFileProvider.GetFileInfo(apiInstruction.ImageUrl);
            if(filInfo.Exists && filInfo is not AzureBlobItemInfo)
            {
                var directories = ImageMetadataReader.ReadMetadata(filePath);
                return directories;
            }
            else
            {
                return GetFileInfoAzure(apiInstruction);
            }
            return null;
        }
        [HttpPost]
        public IReadOnlyList<MetadataExtractor.Directory> GetFileInfoAzure(ApiInstruction apiInstruction)
        {
            var baseUrl = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host;
            using var client = new HttpClient();
            using var response = client.GetAsync(baseUrl + apiInstruction.ImageUrl).Result;
            using var content = response.Content;
            using var stream = content.ReadAsStreamAsync().Result;

            if(stream.Length > 0)
            {
                var directories = ImageMetadataReader.ReadMetadata(stream);
                return directories;
            }
            return null;
        }
        public class ApiInstruction
        {
            public string ImageUrl { get; set; }
        }
    }
}

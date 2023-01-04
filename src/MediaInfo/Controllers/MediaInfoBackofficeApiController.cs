using System.Collections.Generic;
using MetadataExtractor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace MediaInfo.Controllers
{
    [PluginController("MediaInfo")]
    public class MediaInfoBackofficeApiController : UmbracoAuthorizedJsonController
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public MediaInfoBackofficeApiController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public IReadOnlyList<MetadataExtractor.Directory> GetFileInfo(ApiInstruction apiInstruction)
        {
            var filePath = _webHostEnvironment.WebRootPath + apiInstruction.ImageUrl;
            var filInfo = _webHostEnvironment.WebRootFileProvider.GetFileInfo(apiInstruction.ImageUrl);
            if(filInfo.Exists)
            {
                var directories = ImageMetadataReader.ReadMetadata(filePath);
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

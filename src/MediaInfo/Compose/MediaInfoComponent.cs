using System;
using System.Collections.Generic;
using MediaInfo.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace MediaInfo.Compose
{

    public class ServerVariablesParsingNotificationHandler : INotificationHandler<ServerVariablesParsingNotification>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly UmbracoApiControllerTypeCollection _apiControllers;
        private readonly IActionContextAccessor _actionContextAccessor;

        //LinkGenerator _linkGenerator;
        public ServerVariablesParsingNotificationHandler(IHttpContextAccessor httpContextAccessor,UmbracoApiControllerTypeCollection apiControllers,IUrlHelperFactory urlHelperFactory,IActionContextAccessor actionContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _apiControllers = apiControllers;
            _actionContextAccessor = actionContextAccessor;

        }
        public void Handle(ServerVariablesParsingNotification notification)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                throw new InvalidOperationException("HttpContext is null");
            }
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            notification.ServerVariables.Add("MediaInfo", new Dictionary<string, object>
            {
                {
                    "MediaInfoApiUrl",
                    urlHelper.GetUmbracoApiServiceBaseUrl<MediaInfoBackofficeApiController>(_apiControllers,
                        controller => controller.GetFileInfo(new MediaInfoBackofficeApiController.ApiInstruction { ImageUrl = string.Empty }))
                }
            });

        }
    }

}


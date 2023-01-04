using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace MediaInfo.Compose
{

    public class MediaInfoComposer : IComposer
    {

        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<ServerVariablesParsingNotification,ServerVariablesParsingNotificationHandler>();
        }
    }
}

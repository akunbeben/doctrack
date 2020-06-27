using System.Web.Mvc;

namespace DocTrack.Web.Helpers
{
    public static class PageHelpers
    {
        public static string IsActive(this HtmlHelper html,
                                  string control,
                                  string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // must match both
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "kt-menu__item--here" : "";
        }
    }
}
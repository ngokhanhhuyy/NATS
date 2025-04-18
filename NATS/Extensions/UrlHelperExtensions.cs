namespace NATS.Extensions;

public static class UrlHelperExtensions
{
    public static string GetPublicHomeRoutePath(this IUrlHelper urlHelper)
    {
        return urlHelper.RouteUrl(Public.Controllers.HomeController.HomeRouteName);
    }

    public static string GetPublicAboutUsIntroductionRoutePath(this IUrlHelper urlHelper)
    {
        string routeName = Public.Controllers
            .AboutUsIntroductionController
            .AboutUsIntroductionRouteName;
        return urlHelper.RouteUrl(routeName);
    }

    public static string GetPublicSummaryItemListRoutePath(
            this IUrlHelper urlHelper,
            int? id = null)
    {
        string routeName = Public.Controllers.SummaryItemController.SummaryItemListRouteName;
        if (id.HasValue)
        {
            return urlHelper.RouteUrl(routeName, new { id });
        }

        return urlHelper.RouteUrl(routeName);
    }

    public static string GetPublicServiceListRoutePath(this IUrlHelper urlHelper)
    {
        string routeName = Public.Controllers.CatalogItemController.ServiceListRouteName;
        return urlHelper.RouteUrl(routeName);
    }

    public static string GetPublicServiceDetailRoutePath(this IUrlHelper urlHelper, int id)
    {
        string routeName = Public.Controllers.CatalogItemController.ServiceDetailRouteName;
        return urlHelper.RouteUrl(routeName, new { id });
    }

    public static string GetPublicCourseListRoutePath(this IUrlHelper urlHelper)
    {
        string routeName = Public.Controllers.CatalogItemController.CourseListRouteName;
        return urlHelper.RouteUrl(routeName);
    }

    public static string GetPublicCourseDetailRoutePath(this IUrlHelper urlHelper, int id)
    {
        string routeName = Public.Controllers.CatalogItemController.CourseDetailRouteName;
        return urlHelper.RouteUrl(routeName, new { id });
    }

    public static string GetPublicContactRoutePath(this IUrlHelper urlHelper)
    {
        return urlHelper.RouteUrl(Public.Controllers.ContactController.ContactRouteName);
    }

    public static string GetPublicEnquiryRoutePath(this IUrlHelper urlHelper)
    {
        return urlHelper.RouteUrl(Public.Controllers.EnquiryController.EnquiryRouteName);
    }

    public static string GetSignInRoutePath(this IUrlHelper urlHelper)
    {
        string routeName = Protected.Controllers.AuthenticationController.SignInRouteName;
        return urlHelper.RouteUrl(routeName);
    }

    public static string GetProtectedDashboardRoutePath(this IUrlHelper urlHelper)
    {
        string routeName = Protected.Controllers.DashboardController.DashboardRouteName;
        return urlHelper.RouteUrl(routeName);
    }

    public static string GetProtectedSliderItemListRoutePath(this IUrlHelper urlHelper)
    {
        string routeName = Protected.Controllers.SliderItemController.ListRouteName;
        return urlHelper.RouteUrl(routeName);
    }

    public static string GetProtectedSliderItemCreateRoutePath(this IUrlHelper urlHelper)
    {
        string routeName = Protected.Controllers.SliderItemController.CreateRouteName;
        return urlHelper.RouteUrl(routeName);
    }

    public static string GetProtectedSliderItemUpdateRoutePath(
            this IUrlHelper urlHelper,
            int id)
    {
        string routeName = Protected.Controllers.SliderItemController.UpdateRouteName;
        return urlHelper.RouteUrl(routeName, new { id });
    }
}
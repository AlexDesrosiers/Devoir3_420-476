using System.Web;
using System.Web.Mvc;

namespace Devoir3_DesrosiersAlex
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MVCBudget.Forser.Helpers
{
    public class Helper
    {
        public static string RenderViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                    );

                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.GetTypedHeaders().Referer == null || 
                filterContext.HttpContext.Request.GetTypedHeaders().Host.Host.ToString() != 
                filterContext.HttpContext.Request.GetTypedHeaders().Referer.Host.ToString()) 
            {
                filterContext.HttpContext.Response.Redirect("/");
            }
        }
    }

    public static class IdentityHelpers
    {
        public static async Task EnableIdentityInsertAsync<T>(this AppDbContext context) => await SetIdentityInsertAsync<T>(context, enable: true);
        public static async Task DisableIdentityInsertAsync<T>(this AppDbContext context) => await SetIdentityInsertAsync<T>(context, enable: false);
        private static async Task SetIdentityInsertAsync<T>([NotNull] AppDbContext context, bool enable)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var entityType = context.Model.FindEntityType(typeof(T));
            var value = enable ? "ON" : "OFF";
            await context.Database.ExecuteSqlRawAsync(
                $"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} {value}");
        }
        public static async Task SaveChangesWithIdentityInsertAsync<T>([NotNull] this AppDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            await using var transaction = await context.Database.BeginTransactionAsync();
            await context.EnableIdentityInsertAsync<T>();
            await context.SaveChangesAsync();
            await context.DisableIdentityInsertAsync<T>();
            await transaction.CommitAsync();
        }
    }
}
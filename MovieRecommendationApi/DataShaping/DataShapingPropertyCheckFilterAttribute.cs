using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MovieRecommendationApi.DataShaping
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DataShapingPropertyCheckFilterAttribute : ActionFilterAttribute
    {
        private readonly Type _type;
        private readonly IPropertyChecker _propertyChecker;

        public DataShapingPropertyCheckFilterAttribute(Type type, IPropertyChecker propertyChecker)
        {
            _type = type ?? throw new ArgumentNullException(nameof(type));
            _propertyChecker = propertyChecker ?? throw new ArgumentNullException(nameof(propertyChecker));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var fields = context.HttpContext.Request.Query["fields"].FirstOrDefault();
            if (string.IsNullOrEmpty(fields))
            {
                return;
            }

            if (!_propertyChecker.TypeHasProperties(_type, fields))
            {
                context.Result = new BadRequestObjectResult(new
                {
                    Error = "InvalidFields",
                    Message = "Some fields requested for data shaping do not exist."
                });
            }
        }
    }
}

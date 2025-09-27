using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParameters = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType.GetProperties()
                .Any(prop => prop.PropertyType == typeof(IFormFile)));

        if (fileParameters.Any())
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(
                            context.MethodInfo.GetParameters().First().ParameterType, 
                            context.SchemaRepository)
                    }
                }
            };
        }
    }
}
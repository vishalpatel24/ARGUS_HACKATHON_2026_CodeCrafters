using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CodeCrafters.Api.Swagger;

/// <summary>
/// Fixes Swagger generation for multipart/form-data upload so IFormFile is not reflected as a schema.
/// </summary>
public class UploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var actionName = context.ApiDescription.ActionDescriptor.RouteValues["action"];
        if (actionName != "Upload" || context.ApiDescription.ActionDescriptor.RouteValues["controller"] != "Documents")
            return;

        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["documentType"] = new OpenApiSchema
                            {
                                Type = "string",
                                Description = "One of: RegistrationCertificate, AuditedFinancials, Tax80G, Other",
                                Default = new Microsoft.OpenApi.Any.OpenApiString("Other")
                            },
                            ["file"] = new OpenApiSchema
                            {
                                Type = "string",
                                Format = "binary",
                                Description = "Document file to upload"
                            }
                        },
                        Required = new HashSet<string> { "file" }
                    }
                }
            }
        };
    }
}

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCP.WebAPI.Filter
{
    public class BinaryPayloadFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod.ToUpper() != "POST")
                return;
            operation.RequestBody = new OpenApiRequestBody();
            operation.RequestBody.Content.Add("application/json", new OpenApiMediaType()
            {
                Schema = new OpenApiSchema() { Type = "string" }
            });
        }
    }
}

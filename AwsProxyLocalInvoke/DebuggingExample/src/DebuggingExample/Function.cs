using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DebuggingExample
{
    public class Functions
    {
        ITimeProcessor processor = new TimeProcessor();

        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Get Request\n");

            var result = processor.CurrentTimeUTC();

            return CreateResponse(result);
        }


        APIGatewayProxyResponse CreateResponse(DateTime? result) {

            int statusCode = (result != null) ? 
            (int)HttpStatusCode.OK : 
            (int)HttpStatusCode.InternalServerError;

            string body = (result != null) ? 
            JsonSerializer.Serialize(result) : string.Empty;

            var response = new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = body,
                Headers = new Dictionary<string, string>
                { 
                    { "Content-Type", "application/json" }, 
                    { "Access-Control-Allow-Origin", "*" } 
                }
            };
            
            return response;
        }
    }
}









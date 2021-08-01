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
            LogMessage(context, "Processing request started");
            APIGatewayProxyResponse response;
            try
            {
                var result = processor.CurrentTimeUTC();
                response = CreateResponse(result);
                LogMessage(context, "Processing request succeeded.");
            }
            catch (Exception ex)
            {
                LogMessage(context, string.Format("Processing request failed - {0}", ex.Message));
                response = CreateResponse(null);
            }

            return response;
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
   
        void LogMessage(ILambdaContext ctx, string msg)
        {
            ctx.Logger.LogLine(
                string.Format("{0}:{1} - {2}", 
                    ctx.AwsRequestId, 
                    ctx.FunctionName,
                    msg));
        }   
   
    }
}









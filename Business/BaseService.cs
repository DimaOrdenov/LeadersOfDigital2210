using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Business.Definitions.Exceptions;
using Business.Definitions.Types;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace Business
{
    public abstract class BaseService
    {
        private readonly IRestClient _client;
        private readonly ILogger _logger;

        protected BaseService(IRestClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        // protected async Task<bool> ExecuteAsync(IRestRequest request, CancellationToken token)
        // {
        //     request.Timeout = Timeout;
        //
        //     Guid guid = Guid.NewGuid();
        //
        //     if (BeforeRequest != null)
        //     {
        //         BeforeRequest.Invoke(request, guid.ToString());
        //         request.AdvancedResponseWriter = (input, resp) => resp.RawBytes = ReadAsBytes(input);
        //     }
        //
        //     var response = await _client.ExecuteAsync(request, token);
        //
        //     if (AfterRequest != null)
        //     {
        //         AfterRequest.Invoke(request, response, guid.ToString());
        //     }
        //
        //     if (!response.IsSuccessful)
        //     {
        //         CheckResponse(response);
        //     }
        //
        //     return response.IsSuccessful;
        // }

        protected async Task<TOut> ExecuteAsync<TOut>(IRestRequest request, CancellationToken token)
        {
            Guid guid = Guid.NewGuid();

            BeforeRequest(request, guid.ToString());
            request.AdvancedResponseWriter = (input, resp) => resp.RawBytes = ReadAsBytes(input);

            var response = await _client.ExecuteAsync(request, token);

            AfterRequest(request, response, guid.ToString());

            if (!response.IsSuccessful)
            {
                CheckResponse(response);
            }

            return JsonConvert.DeserializeObject<TOut>(response.Content);
        }

        private void AfterRequest(IRestRequest request, IRestResponse response, string message) =>
            _logger?.LogDebug(
                new StringBuilder()
                    .AppendLine(message + " | Response: ")
                    .AppendLine(request.Method + " :: " + _client.BaseUrl + request.Resource)
                    .AppendLine("ContentType: " + response.ContentType)
                    .AppendLine("StatusCode: " + response.StatusCode)
                    .AppendLine("Error: " + response.ErrorMessage)
                    .AppendLine("Content: " + response.Content)
                    .ToString());

        private void BeforeRequest(IRestRequest request, string message) =>
            _logger?.LogDebug(
                new StringBuilder()
                    .AppendLine(message + " | Request: ")
                    .AppendLine(request.Method + " :: " + _client.BaseUrl + request.Resource)
                    .AppendJoin(Environment.NewLine, request.Parameters.Select(param => $"{param.Name} = {JsonConvert.SerializeObject(param.Value)}"))
                    .ToString());

        private byte[] ReadAsBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];

            using (var ms = new MemoryStream())
            {
                int read;

                try
                {
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }

                    return ms.ToArray();
                }
                catch (WebException ex)
                {
                    return Encoding.UTF8.GetBytes(ex.Message);
                }
            }
        }

        private void CheckResponse(IRestResponse response)
        {
            WebException webException = response?.ErrorException as WebException;

            switch (response?.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new BusinessLogicException(BusinessExceptionType.BadRequest, response.Content);

                case HttpStatusCode.NotFound:
                    throw new BusinessLogicException(BusinessExceptionType.NotFound, response.Content);

                case HttpStatusCode.Unauthorized:
                    throw new BusinessLogicException(BusinessExceptionType.Unauthorized, response.Content);
                case (HttpStatusCode)429:
                    throw new BusinessLogicException(BusinessExceptionType.TooManyRequests, response.Content);
                case HttpStatusCode.NoContent:
                    throw new BusinessLogicException(BusinessExceptionType.NoContent, response.Content);
                case 0:
                    if (webException?.Status == WebExceptionStatus.NameResolutionFailure ||
                        webException?.Status == WebExceptionStatus.ConnectFailure)
                    {
                        throw new BusinessLogicException(BusinessExceptionType.NoInternet, response.Content);
                    }

                    if (webException?.Status == WebExceptionStatus.Timeout)
                    {
                        throw new BusinessLogicException(BusinessExceptionType.Timeout, response.Content);
                    }

                    throw new BusinessLogicException(BusinessExceptionType.Unknown, response.Content);

                default:
                    throw new BusinessLogicException(BusinessExceptionType.Unknown, response.Content);
            }
        }
    }
}

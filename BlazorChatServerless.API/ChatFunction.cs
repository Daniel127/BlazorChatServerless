using BlazorChatServerless.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Threading.Tasks;
using BlazorChatServerless.Shared.Constants;

namespace BlazorChatServerless.API
{
    public static class ChatFunction
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request,
            [SignalRConnectionInfo(HubName = "ChatHub")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("messages")]
        public static Task SendMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] ClientMessage clientMessage,
            [SignalR(HubName = "ChatHub")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = HubConstants.ReceiveMessageMethod,
                    Arguments = new object[] { clientMessage }
                });
        }
    }
}

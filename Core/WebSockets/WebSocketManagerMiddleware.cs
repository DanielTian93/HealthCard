using Core.Extentions;
using Core.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Core.WebSockets
{
    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketHandler _webSocketHandler { get; set; }
        private readonly IServiceProvider _serviceProvider;
        private readonly ISessionService _sessionService;
        public WebSocketConnectionManager WebSocketConnectionManager { get; set; }

        public WebSocketManagerMiddleware(RequestDelegate next,
                                          WebSocketHandler webSocketHandler,
                                          ISessionService sessionService,
                                          WebSocketConnectionManager webSocketConnectionManager,
                                          IServiceProvider serviceProvider)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
            _sessionService = sessionService;
            WebSocketConnectionManager = webSocketConnectionManager;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!context.WebSockets.IsWebSocketRequest)
                {
                    return;
                }

                var socket = await context.WebSockets.AcceptWebSocketAsync();
                //StringValues deviceno = "";
                //context.Request.Query.TryGetValue("deviceno", out deviceno);
                await _webSocketHandler.OnConnected(socket, SnowflakeHelper.IdWorker.NextId().ToString());

                await Receive(socket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        await _webSocketHandler.ReceiveAsync(socket, result, buffer);
                        return;
                    }

                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _webSocketHandler.OnDisconnected(socket);
                        return;
                    }

                });

                //TODO - investigate the Kestrel exception thrown when this is the last middleware
                //await _next.Invoke(context);
            }
            catch (Exception)
            {

            }

        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            try
            {

                //var buffer = new byte[1024 * 10];
                //WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                //while (!result.CloseStatus.HasValue)
                //{
                //    result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                //    handleMessage(result, buffer);
                //}
                //var buffer = new byte[1024 * 30];

                //while (socket.State == WebSocketState.Open)
                //{
                //    WebSocketReceiveResult result = null;

                //    result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                //                                   cancellationToken: CancellationToken.None);
                //    handleMessage(result, buffer);
                //}
                const int maxMessageSize = 1024 * 10;
                byte[] buffer = new byte[maxMessageSize];
                while (socket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    handleMessage(result, buffer);

                    //        var buffer = new byte[1024 * 30];
                    //    while (socket.State == WebSocketState.Open)
                    //    {
                    //        WebSocketReceiveResult result = null;
                    //        do
                    //        {
                    //            result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                    //        } while (!result.EndOfMessage);
                    //        handleMessage(result, buffer);
                    //    }
                }
            }
            catch (Exception)
            {

            }

        }
    }
}

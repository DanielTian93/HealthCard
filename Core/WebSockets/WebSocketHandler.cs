using Core.Helpers;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.WebSockets
{
    public abstract class WebSocketHandler
    {
        public WebSocketConnectionManager WebSocketConnectionManager { get; set; }

        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        public virtual async Task OnConnected(WebSocket socket, string id)
        {
            var sid = WebSocketConnectionManager.AddSocket(socket, id);
        }
        public class KeepHeartRedisDomainModel
        {
            public string DeviceNo { get; set; }
            public DateTime DateTime { get; set; }
            public int OpenDoor { get; set; }
            public long OpenDoorUserId { get; set; }
            public string SocketDeviceSocketId { get; set; }
        }
        public virtual async Task OnDisconnected(WebSocket socket)
        {
            var id = WebSocketConnectionManager.GetId(socket);
            if (RedisHelper.Exists($"Socket:{id}"))
            {
                var keepHeartRedisDomainModel = RedisHelper.Get<KeepHeartRedisDomainModel>($"Socket:{id}");
                RedisHelper.Remove($"Device:{keepHeartRedisDomainModel.DeviceNo}{id}");
                RedisHelper.Remove($"Device:Offline:{keepHeartRedisDomainModel.DeviceNo}{id}");
                RedisHelper.Remove($"Socket:{id}");
            }
            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));
        }
        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket == null)
            {
                return;
            }

            if (socket.State != WebSocketState.Open)
            {
                return;
            }

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.Default.GetBytes(message)),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }
        public bool GetWebSocket(string socketId, string token)
        {
            var socket = WebSocketConnectionManager.GetSocketById(socketId);
            if (socket == null)
            {
                RedisHelper.RemoveNotMergeKey(token);
            }
            return socket != null;
        }

        public async Task SendMessageAsync(string socketId, string message)
        {
            try
            {
                await SendMessageAsync(WebSocketConnectionManager.GetSocketById(socketId), message);
            }
            catch (Exception)
            {

            }

        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in WebSocketConnectionManager.GetAll())
            {
                if (pair.Value.State == WebSocketState.Open)
                {
                    await SendMessageAsync(pair.Value, message);
                }
            }
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}

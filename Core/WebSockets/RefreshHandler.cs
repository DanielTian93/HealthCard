using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Core.WebSockets
{
    public class RefreshHandler : WebSocketHandler
    {
        public RefreshHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {

        }
        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string message = Encoding.Default.GetString(buffer, 0, result.Count);
            Console.WriteLine(message);
            //await SendMessageToAllAsync($"返回信息{message}");
        }
    }
}

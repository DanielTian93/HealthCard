using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Core.WebSockets
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }
        public string AddSocket(WebSocket socket, string id)
        {
            string sId = id;
            while (!_sockets.TryAdd(sId, socket))
            {
                sId = CreateConnectionId();
            }
            Console.WriteLine(sId);
            return sId;
        }

        public async Task RemoveSocket(string id)
        {
            try
            {
                WebSocket socket;

                _sockets.TryRemove(id, out socket);


                await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);


            }
            catch (Exception)
            {

            }

        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

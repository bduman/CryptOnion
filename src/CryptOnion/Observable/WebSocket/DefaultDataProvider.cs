using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace CryptOnion.Observable.WebSocket
{
    public class DefaultDataProvider : IDataProvider<ClientWebSocket>
    {
        private readonly Configuration _configuration;

        public DefaultDataProvider(Configuration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<ClientWebSocket> ProvideAsync(CancellationToken cancellationToken)
        {
            var webSocket = new ClientWebSocket();
            await webSocket.ConnectAsync(this._configuration.SocketUri, cancellationToken).ConfigureAwait(false);

            return webSocket;
        }
    }
}

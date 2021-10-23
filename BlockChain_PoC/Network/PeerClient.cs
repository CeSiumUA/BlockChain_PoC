using BlockChain_PoC.Interfaces;
using BlockChain_PoC.Parsers;
using MediatR;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BlockChain_PoC.Network
{
    public class PeerClient : INetworkInterface
    {
        private TcpListener _listener;
        private Task? listeningTask;
        private ObservableCollection<TcpClient> _clients = new ObservableCollection<TcpClient>();
        private IDataParser _parser;
        private IPeerProvider _peerProvider;
        private readonly IMediator _mediator;
        private readonly IUserIO _userIO;

        public PeerClient(IDataParser dataParser, IPeerProvider peerProvider, IMediator mediator, IUserIO userIO)
        {
            _listener = new TcpListener(IPAddress.Any, 4956);
            _parser = dataParser;
            _peerProvider = peerProvider;
            _mediator = mediator;
            _userIO = userIO;
            _clients.CollectionChanged += ClientsCollectionChanged;
        }

        private void ClientsCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _userIO.SendUserTextOutput($"Connected peers: {_clients.Count}");
        }

        public void Dispose()
        {
            _listener.Stop();
            foreach (var client in _clients)
            {
                client.Close();
                client.Dispose();
            }
            listeningTask?.Dispose();
        }

        public async Task Init()
        {
            _listener.Start();
            listeningTask = new Task(async () =>
            {
                while (true)
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    var clientEndpoint = client.Client.RemoteEndPoint as IPEndPoint;
                    _userIO.SendUserTextOutput($"Client {clientEndpoint?.Address}:{clientEndpoint?.Port} connected!");
                    var existingClient = _clients.FirstOrDefault(x =>
                    {
                        var endPoint = (x.Client.RemoteEndPoint as IPEndPoint);
                        if (endPoint?.Address.ToString() == clientEndpoint?.Address.ToString() && endPoint?.Port == clientEndpoint?.Port)
                        {
                            return true;
                        }
                        return false;
                    });
                    if (existingClient != null)
                    {
                        var index = _clients.IndexOf(existingClient);
                        existingClient.Close();
                        existingClient.Dispose();
                        _clients[index] = client;
                    }
                    else
                    {
                        _clients.Add(client);
                    }
                    var clientProcessorTask = new Task(async () =>
                    {
                        var stream = client.GetStream();
                        while (true)
                        {
                            List<byte> data = new List<byte>();
                            var dataArray = new byte[4096];
                            int bytes = 0;
                            do
                            {
                                try
                                {
                                    bytes = stream.Read(dataArray, 0, dataArray.Length);
                                    data.AddRange(dataArray.Take(bytes));
                                }
                                catch (Exception ex)
                                {
                                    _clients.Remove(client);
                                    _userIO.LogException(ex.ToString());
                                    _userIO.SendUserTextOutput($"Client {clientEndpoint?.Address}:{clientEndpoint?.Port} disconnected!");
                                    return;
                                }
                            }
                            while (stream.DataAvailable);
                            if (data.Count > 0)
                            {
                                void WriteResponse(byte[] data)
                                {
                                    stream.Write(data, 0, data.Length);
                                }
                                var transferBytes = data.ToArray();
                                var commnadType = await _parser.GetCommandType(transferBytes);
                                var command = await _parser.Parse(transferBytes, commnadType);
                                if (command != null)
                                {
                                    var result = await _mediator.Send(command);
                                    await ProcessCommandResult(result, WriteResponse);
                                }
                            }
                        }
                    });
                    clientProcessorTask.Start();
                }
            });
            listeningTask.Start();
            await LoadPeers();
        }
        private async Task LoadPeers()
        {
            var peers = await _peerProvider.GetPeersAsync();
            _userIO.SendUserTextOutput($"Connecting to {peers.Count()} peers");
            foreach (var peer in peers)
            {
                TcpClient tcpClient = new TcpClient();
                try
                {
                    _userIO.SendUserTextOutput($"Connecting to {peer.IPAddress}:{peer.Port}!");
                    await tcpClient.ConnectAsync(peer.IPAddress, peer.Port);
                    if (!_clients.Any(x =>
                     {
                         var existingEndPoint = (x.Client.RemoteEndPoint as IPEndPoint);
                         var usedEndPoint = tcpClient.Client.RemoteEndPoint as IPEndPoint;
                         return existingEndPoint?.Address == usedEndPoint?.Address && existingEndPoint?.Port == usedEndPoint?.Port;
                     }))
                    {
                        _clients.Add(tcpClient);
                    }
                    _userIO.SendUserTextOutput($"Connected to {peer.IPAddress}:{peer.Port}!");
                }
                catch (Exception ex)
                {
                    _userIO.LogException(ex.ToString());
                    _userIO.SendUserTextOutput($"Failed to connect to {peer.IPAddress}:{peer.Port}");
                }
            }
        }
        public async Task Broadcast(byte[] data)
        {
            foreach (var peer in _clients)
            {
                var stream = peer.GetStream();
                stream.Write(data, 0, data.Length);
            }
        }
        public async Task Broadcast<T>(T data)
        {
            var serialized = System.Text.Json.JsonSerializer.Serialize(data);
            var serializedBytes = Encoding.UTF8.GetBytes(serialized);
            await Broadcast(serializedBytes);
        }
        private async Task ProcessCommandResult(object? result, Action<byte[]> responseWriteCallback)
        {

        }
    }
}

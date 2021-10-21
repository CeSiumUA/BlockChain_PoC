using BlockChain_PoC.Interfaces;
using BlockChain_PoC.Parsers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Network
{
    public class PeerClient : INetworkInterface
    {
        private TcpListener _listener;
        private Task listeningTask;
        private List<TcpClient> _clients;
        private IDataParser _parser;
        private IPeerProvider _peerProvider;
        private readonly IMediator _mediator;

        public PeerClient(IDataParser dataParser, IPeerProvider peerProvider, IMediator mediator)
        {
            _listener = new TcpListener(IPAddress.Any, 4956);
            _parser = dataParser;
            _peerProvider = peerProvider;
            _mediator = mediator;
        }

        public void Dispose()
        {
            _listener.Stop();
            foreach(var client in _clients)
            {
                client.Close();
                client.Dispose();
            }
        }

        public async Task Init()
        {
            await LoadPeers();
            _listener.Start();
            listeningTask = new Task(async () =>
            {
                while (true)
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    var clientEndpoint = client.Client.RemoteEndPoint as IPEndPoint;
                    var existingClient = _clients.FirstOrDefault(x =>
                    {
                        var endPoint = (x.Client.RemoteEndPoint as IPEndPoint);
                        if(endPoint.Address.ToString() == clientEndpoint.Address.ToString() && endPoint.Port == clientEndpoint.Port)
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
                    else {
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
                                bytes = stream.Read(dataArray, 0, dataArray.Length);
                                data.AddRange(dataArray.Take(bytes));
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
                                var result = await _mediator.Send(command);
                                await ProcessCommandResult(result, WriteResponse);
                            }
                        }
                    });
                    clientProcessorTask.Start();
                }
            });
            listeningTask.Start();
        }
        private async Task LoadPeers()
        {
            var peers = await _peerProvider.GetPeersAsync();
            foreach(var peer in peers)
            {
                TcpClient tcpClient = new TcpClient();
                try
                {
                    await tcpClient.ConnectAsync(peer.IPAddress, peer.Port);
                    _clients.Add(tcpClient);
                }
                catch(Exception ex)
                {

                }
            }
        }
        public async Task Broadcast(byte[] data)
        {
            foreach(var peer in _clients)
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
        private async Task ProcessCommandResult(object result, Action<byte[]> responseWriteCallback)
        {

        }
    }
}

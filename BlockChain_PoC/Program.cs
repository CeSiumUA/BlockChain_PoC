using Autofac;
using BlockChain_PoC;
using BlockChain_PoC.Base;
using BlockChain_PoC.Crypto;
using BlockChain_PoC.Network;
using System.Text;

var builder = new ContainerBuilder();

builder.RegisterType<PeerClient>().SingleInstance();

var blockChain = new BlockChain(true);

builder.RegisterInstance(blockChain).SingleInstance();

var keyPair = KeyGen.LoadKey();

string walletAddress = keyPair.PublicKey.GetHex();

var container = builder.Build();

var client = container.Resolve<PeerClient>();

await client.Init();
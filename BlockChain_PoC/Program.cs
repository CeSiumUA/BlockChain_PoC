using Autofac;
using BlockChain_PoC;
using BlockChain_PoC.Base;
using BlockChain_PoC.Core;
using BlockChain_PoC.Crypto;
using BlockChain_PoC.Interfaces;
using BlockChain_PoC.Network;
using BlockChain_PoC.Parsers;
using MediatR.Extensions.Autofac.DependencyInjection;
using System.Reflection;
using System.Text;

var builder = new ContainerBuilder();

builder.RegisterType<PeerClient>().As<INetworkInterface>().SingleInstance();

builder.RegisterType<ConsoleActionsHandler>().SingleInstance();

builder.RegisterType<BlockChain>().As<IBlockChain>().SingleInstance();

builder.RegisterType<JsonParser>().As<IDataParser>().SingleInstance();

builder.RegisterType<TextFilePeerProvider>().As<IPeerProvider>().SingleInstance();

builder.RegisterMediatR(Assembly.GetExecutingAssembly());

builder.RegisterType<ConsoleUserIO>().As<IUserIO>().SingleInstance();

var keyPair = KeyGen.LoadKey(createIfNotExists: true);

builder.RegisterInstance(keyPair).SingleInstance();

var container = builder.Build();

var client = container.Resolve<INetworkInterface>();

var consoleActionsHanlder = container.Resolve<ConsoleActionsHandler>();

await client.Init();

await consoleActionsHanlder.StartActionsHandler();
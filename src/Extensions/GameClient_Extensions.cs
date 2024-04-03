//todo multiplayer

// using Game.Messages;
// using Network;
// using Network.Client;
// using Network.Messages;
// using Serilog;
// using ClientStatus = Network.Server.ClientStatus;
//
// namespace visible_passengers.Extensions;
//
// public static class GameClient_Extensions
// {
// 	public static void Send(this GameClient deez, IGameMessage message, Channel channel, string playerId)
// 	{
// 		if (deez.ServerClientStatus != ClientStatus.Active)
// 		{
// 			Log.Warning("Will not send message {message}, status is {ServerClientStatus}", message, deez.ServerClientStatus);
// 		}
// 		else
// 		{
// 			Main.Debug("GameClient_Extensions id: "+playerId);
// 			deez.SendNetworkMessage(new GameMessageEnvelope(playerId, message), channel);
// 		}
// 	}
// }
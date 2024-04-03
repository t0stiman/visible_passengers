//todo multiplayer

// using Game.Messages;
// using Network;
// using Network.Client;
// using Serilog;
//
// namespace visible_passengers.Extensions;
//
// public static class ClientManager_Extensions
// {
// 	public static void Send(this ClientManager deez, IGameMessage message, string playerId, bool forceReliable = false)
// 	{
// 		if (deez._client == null)
// 			Log.Warning("Send with null _socket");
// 		else if (deez._inTransaction > 0)
// 		{
// 			deez._transactionMessages.Add(message);
// 		}
// 		else
// 		{
// 			var channel = Multiplayer.ChannelForMessage(message, forceReliable);
// 			deez._client.Send(message, channel, playerId);
// 		}
// 	}
// }
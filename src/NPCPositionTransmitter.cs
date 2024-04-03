//todo multiplayer

// using System;
// using Avatar;
// using Character;
// using Game.Messages;
// using Game.State;
// using Helpers;
// using Network;
// using UnityEngine;
// using visible_passengers.Extensions;
//
// namespace visible_passengers;
//
// public class NPCPositionTransmitter: CharacterPositionTransmitter
// {
// 	[NonSerialized]
// 	public Passenger Passenger;
// 	
// 	private new void Awake()
// 	{
// 		_firstPersonController = null;
// 	}
// 	
// 	public new void SendIfConnected(AvatarPose pose)
// 	{
// 		if (Multiplayer.Client == null || !Multiplayer.IsClientActive)
// 		{
// 			return;
// 		}
//
// 		var now = StateManager.Now;
// 		if (_lastSentTick != 0L && Mathf.Abs(_lastSentTick - now) > 20000.0)
// 		{
// 			Debug.LogWarning($"Resetting _lastSendTick: {now} - {_lastSentTick} = {now - _lastSentTick}");
// 			_lastSentTick = 0L;
// 		}
//
// 		if (_lastSentTick + 100L > now)
// 		{
// 			return;
// 		}
//
// 		var force = _lastSentTick + 2000L <= now;
// 		var (motionSnapshot, car) = Passenger.GetRelativePositionRotation();
// 		string relativeToCarId = null;
// 		if (car == null)
// 		{
// 			motionSnapshot.Position = WorldTransformer.WorldToGame(motionSnapshot.Position);
// 		}
// 		else
// 		{
// 			relativeToCarId = car.id;
// 		}
//
// 		SendIfNeeded(motionSnapshot.Position, motionSnapshot.BodyRotation * Vector3.forward, motionSnapshot.LookRotation * Vector3.forward, motionSnapshot.Velocity, pose, relativeToCarId, force);
// 	}
// 	
// 	private new void SendIfNeeded(
// 		Vector3 position,
// 		Vector3 forward,
// 		Vector3 look,
// 		Vector3 velocity,
// 		AvatarPose pose,
// 		string relativeToCarId,
// 		bool force)
// 	{
// 		// todo do we need all this?
// 		
// 		var now = StateManager.Now;
// 		var pose1 = (CharacterPose) pose;
// 		var position1 = new CharacterPosition(position, relativeToCarId, forward, look);
// 		var position2 = _lastSentUpdateCharacterPosition.Position;
// 		var message = new UpdateCharacterPosition(position1, velocity, pose1, now);
// 		var vector3 = position1.Position - position2.Position;
// 		var magnitude1 = (double) vector3.magnitude;
// 		vector3 = position1.Forward - position2.Forward;
// 		var magnitude2 = vector3.magnitude;
// 		vector3 = position1.Look - position2.Look;
// 		var magnitude3 = vector3.magnitude;
// 		vector3 = message.Velocity - _lastSentUpdateCharacterPosition.Velocity;
// 		var magnitude4 = vector3.magnitude;
// 		var flag1 = position1.RelativeToCarId != position2.RelativeToCarId;
// 		var flag2 = message.Pose != _lastSentUpdateCharacterPosition.Pose;
// 		var flag3 = ((magnitude1 > 0.05000000074505806 || magnitude2 > 0.10000000149011612 || magnitude3 > 0.10000000149011612 ? 1 : (magnitude4 > 0.009999999776482582 ? 1 : 0)) | (flag1 ? 1 : 0) | (flag2 ? 1 : 0)) != 0;
// 		if (!force && !flag3)
// 			return;
// 		
// 		// this line is changed:
// 		Multiplayer.Client.Send(message, Passenger.avatar.Pickable.PlayerId._playerId);
// 		
// 		_lastSentUpdateCharacterPosition = message;
// 		_lastSentTick = now;
// 	}
// }
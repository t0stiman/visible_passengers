using System;
using UnityEngine;
using Avatar;
using Character;
using Model;
using RollingStock;

namespace visible_passengers;

public class Passenger: RemoteAvatar
{
	private Seat seat;
	[NonSerialized]
	public Car Car;
	// private NPCPositionTransmitter transmitter;

	//todo moet ik de parent callen? wsl niet
	private new void Awake()
	{
		// transmitter = gameObject.AddComponent<NPCPositionTransmitter>();
		// transmitter.Passenger = this;
	}

	private new void FixedUpdate()
	{
		//nothing
	}

	private void Update()
	{
		//todo this is needed to keep the Passenger in it's seat. If you know why, please tell me!
		transform.localPosition = seat._seatToFeet * Vector3.down;
	}

	//todo multiplayer
	// private void LateUpdate()
	// {
	// 	if (transmitter is null)
	// 	{
	// 		return;
	// 	}
	// 	transmitter.SendIfConnected(AvatarPose.Sit);
	// }
	
	public void Sit(Seat aSeat)
	{
		avatar.transform.SetParent(aSeat.transform, true);
		
		avatar.transform.position = aSeat.FootPosition;
		avatar.transform.rotation = aSeat.transform.rotation;
		avatar.Animator.SetPose(AvatarPose.Sit);

		seat = aSeat;
	}

	// based on PlayerController.GetRelativePositionRotation
	// public (MotionSnapshot motionSnapshot, Car car) GetRelativePositionRotation()
	// {
	// 	var myMotionSnapshot = GetMotionSnapshot();
	// 	
	// 	var carMotionSnapshot = Car.GetMotionSnapshot();
	// 	var quaternion = Quaternion.Inverse(carMotionSnapshot.Rotation);
	// 	myMotionSnapshot.Position = quaternion * (myMotionSnapshot.Position - carMotionSnapshot.Position);
	// 	myMotionSnapshot.BodyRotation = quaternion * myMotionSnapshot.BodyRotation;
	// 	myMotionSnapshot.LookRotation = quaternion * myMotionSnapshot.LookRotation;
	// 	myMotionSnapshot.Velocity = carMotionSnapshot.Velocity; //todo velocity?
	// 	
	// 	return (myMotionSnapshot, Car);
	// }
	//
	// private MotionSnapshot GetMotionSnapshot()
	// {
	// 	var bodyRotation = seat.transform.rotation;
	// 	return new MotionSnapshot(transform.position, bodyRotation, bodyRotation, Vector3.zero); //todo velocity?
	// }
}
using UnityEngine;
using Avatar;
using RollingStock;

namespace visible_passengers;

public class Passenger: RemoteAvatar
{
	private Seat seat;
	
	private new void Awake()
	{
		//nothing
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
	
	public void Sit(Seat aSeat)
	{
		avatar.transform.SetParent(aSeat.transform, true);
		
		avatar.transform.position = aSeat.FootPosition;
		avatar.transform.rotation = aSeat.transform.rotation;
		avatar.Animator.SetPose(AvatarPose.Sit);

		seat = aSeat;
	}
}
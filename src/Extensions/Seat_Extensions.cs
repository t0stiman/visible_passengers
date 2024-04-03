using Avatar;
using RollingStock;
using UnityEngine;

namespace visible_passengers.Extensions;

public static class Seat_Extensions
{
	// returns true if an NPC is on the seat
	public static bool NPCOnSeat(this Seat deez)
	{
		for (var i = 0; i < deez.transform.childCount; i++)
		{
			var child = deez.transform.GetChild(i);
			if (child.GetComponentInChildren<RemoteAvatar>() is null) //todo use Passenger instead?
			{
				continue;
			}

			if (Vector3.Distance(child.position, deez.FootPosition) < 0.5)
			{
				return true;
			}
		}
		
		return false;
	}
}
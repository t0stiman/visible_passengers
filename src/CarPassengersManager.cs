using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avatar;
using Game;
using Game.State;
using Helpers;
using Model.OpsNew;
using RandomNameGeneratorLibrary;
using RollingStock;
using UnityEngine;
using visible_passengers.Extensions;

namespace visible_passengers;

public class CarPassengersManager: MonoBehaviour
{
	private List<Seat> seats = new();
	private List<Passenger> passengerAvatars = new();
	public Model.Car theCar;
	private static readonly System.Random random = new();

	private void Start()
	{
		seats = transform.GetComponentsInChildren<Seat>().ToList();
		
		var passengerCount = theCar.GetPassengerMarker()?.TotalPassengers ?? 0;
		UpdatePassengers(passengerCount);
	}
	
	private void OnDestroy()
	{
		Main.Debug(nameof(OnDestroy)+" "+passengerAvatars.Count);
		foreach (var avatar in passengerAvatars)
		{
			Destroy(avatar.avatar);
			Destroy(avatar);
		}
	}

	public void UpdatePassengers(int passengersDelta)
	{
		Main.Debug($"{nameof(UpdatePassengers)}: passengersDelta {passengersDelta}");
		
		var passengersToShow = passengerAvatars.Count + passengersDelta;
		if (passengersToShow > seats.Count)
		{
			Main.Debug($"passengersToShow{passengersToShow} > seats.Count{seats.Count}");
			passengersToShow = seats.Count;
		}
		if (passengersToShow < 0)
		{
			passengersToShow = 0;
			Main.Error($"{nameof(UpdatePassengers)}: somehow got below zero passengers");
		}
		
		Main.Debug($"{passengerAvatars.Count} -> {passengersToShow} ");
		
		if (passengerAvatars.Count == passengersToShow)
		{
			return;
		}

		// too many
		if (passengerAvatars.Count > passengersToShow)
		{
			for (var i = passengerAvatars.Count; i <= passengersToShow; i++)
			{
				Destroy(passengerAvatars[i]);
			}

			passengerAvatars.RemoveRange(passengersToShow, passengerAvatars.Count-1);
			return;
		}
		
		// not enough
		var passengersToAdd = passengersToShow - passengerAvatars.Count;
		var unoccupiedSeats = seats
			.Where(seat => !seat.NPCOnSeat())
			.ToList();
		unoccupiedSeats.Shuffle();
		
		// Main.Debug($"{nameof(UpdatePassengers)}: passengersToAdd {passengersToAdd}");
		// Main.Debug($"{nameof(UpdatePassengers)}: unoccupiedSeats {unoccupiedSeats.Count}");

		for (var i = 0; i < passengersToAdd; i++)
		{
			// not enough seats
			if (i >= unoccupiedSeats.Count)
			{
				break;
			}
			
			AddPassenger(unoccupiedSeats[i]);
		}
	}

	private void AddPassenger(Seat aSeat)
	{
		Main.Debug(nameof(AddPassenger));

		int playerIdInt;
		while (true)
		{
			playerIdInt = random.Next(int.MaxValue);
			
			if (PlayerIdInUse(playerIdInt))
			{
				Main.Debug($"id {playerIdInt} is in use, trying another id");
			}
			else
			{
				break;
			}
		}
		
		var playerId = new PlayerId((ulong)playerIdInt);
		var playerGender = random.Next(2) == 1 ? Gender.Male : Gender.Female;
		
		var newAvatar = AvatarManager.Instance.AddNPC(playerId, playerGender);
		newAvatar.Sit(aSeat);
		newAvatar.Car = theCar;
		passengerAvatars.Add(newAvatar);
	}

	// returns true is there is an NPC or a real player that uses this id
	private bool PlayerIdInUse(int playerIdInt)
	{
		return StateManager.Shared.PlayersManager.PlayerForId(new PlayerId((ulong)playerIdInt)) is not null ||
		       passengerAvatars.Count(passenger => passenger.avatar.Pickable.PlayerId._playerId == playerIdInt.ToString()) != 0;
	}
}
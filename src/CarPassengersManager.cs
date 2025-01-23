using System;
using System.Collections.Generic;
using System.Linq;
using Avatar;
using Game;
using Game.State;
using Helpers;
using Model.Ops;
using RollingStock;
using UnityEngine;
using visible_passengers.Extensions;

namespace visible_passengers;

public class CarPassengersManager: MonoBehaviour
{
	private List<Seat> seats = new();
	private List<NPC_Avatar> passengerAvatars = new();
	public Model.Car theCar;
	private static readonly System.Random random = new();
	private DateTime previousUpdateTime;
	private int previousPassengerAmount = 0;

	private void Start()
	{
		previousUpdateTime = DateTime.Now;
		seats = transform.GetComponentsInChildren<Seat>().ToList();
		
		var passengerCount = theCar.GetPassengerMarker()?.TotalPassengers ?? 0;
		UpdatePassengers(passengerCount);
	}
	
	private void OnDestroy()
	{
		Main.Debug(nameof(OnDestroy)+" "+passengerAvatars.Count);
		foreach (var avatar in passengerAvatars)
		{
			Destroy(avatar);
		}
	}

	private void Update()
	{
		if ((DateTime.Now - previousUpdateTime).TotalSeconds < 1f)
		{
			return;
		}
		
		previousUpdateTime = DateTime.Now;
		
		var passengerMarker = theCar.GetPassengerMarker();
		if (passengerMarker.HasValue)
		{
			var passengerCount = passengerMarker.Value.TotalPassengers;
			if (previousPassengerAmount == passengerCount)
			{
				return;
			}
			
			UpdatePassengers(passengerCount);
		}
	}

	public void UpdatePassengers(int passengerCount)
	{
		previousPassengerAmount = passengerCount;
		
		var passengersToShow = passengerCount;
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
		
		if (passengerAvatars.Count == passengersToShow)
		{
			return;
		}
		
		Main.Debug($"{nameof(UpdatePassengers)}: {passengerAvatars.Count} -> {passengersToShow} ");

		// too many
		if (passengerAvatars.Count > passengersToShow)
		{
			for (var i = passengerAvatars.Count-1; i >= passengersToShow; i--)
			{
				Main.Debug($"Destroy passenger {i}");
				Destroy(passengerAvatars[i].gameObject);
			}

			passengerAvatars.RemoveRange(passengersToShow, passengerAvatars.Count-passengersToShow);
			Main.Debug($"passengerAvatars.Count {passengerAvatars.Count}");
			return;
		}
		
		// not enough
		var passengersToAdd = passengersToShow - passengerAvatars.Count;
		var unoccupiedSeats = seats
			.Where(seat => !seat.NPCOnSeat())
			.ToList();
		unoccupiedSeats.Shuffle();

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
		passengerAvatars.Add(newAvatar);
	}

	// returns true is there is an NPC or a real player that uses this id
	private bool PlayerIdInUse(int playerIdInt)
	{
		return StateManager.Shared.PlayersManager.PlayerForId(new PlayerId((ulong)playerIdInt)) is not null ||
		       passengerAvatars.Count(passenger => passenger.avatar.Pickable.PlayerId._playerId == playerIdInt.ToString()) != 0;
	}
}
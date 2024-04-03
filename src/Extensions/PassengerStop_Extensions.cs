using System;
using GalaSoft.MvvmLight.Messaging;
using Game.Events;
using RollingStock;
using UnityEngine;

namespace visible_passengers.Extensions;

public static class PassengerStop_Extensions
{
	public static void FirePassengerStopServed(this PassengerStop deez, int passengersDelta, Model.Car car, bool loading)
	{
		Messenger.Default.Send(new PassengerStopServed(deez.identifier, passengersDelta));
		
		// GetComponent<> doesn't work for some reason... 
		// so enjoy this cursed code
		foreach(var component in car.gameObject.GetComponents(typeof(Component)))
		{
			if (!component.ToString().Contains(nameof(CarPassengersManager))) continue;
				
			var man = (CarPassengersManager)component;
			// you'd think that passengersDelta would always be negative when unloading, but nope
			passengersDelta = Math.Abs(passengersDelta);
			passengersDelta = loading ? passengersDelta : -passengersDelta;
			man.UpdatePassengers(passengersDelta);
				
			return;
		}
	}
}
using visible_passengers.Extensions;
using HarmonyLib;
using Model;
using Model.OpsNew;
using RollingStock;

namespace visible_passengers.Patches;

[HarmonyPatch(typeof(PassengerStop))]
[HarmonyPatch(nameof(PassengerStop.UnloadCar))]
public class PassengerStop_UnloadCar_Patch
{
	private static bool Prefix(ref PassengerStop __instance, Car car, ref bool __result)
	{
		var bonusMultiplier = PassengerStop.CalculateBonusMultiplier(car);
		var passengerMarker = PassengerStop.MarkerForCar(car);
		var flag1 = string.IsNullOrEmpty(passengerMarker.LastStopIdentifier);
		if (flag1 || passengerMarker.LastStopIdentifier != __instance.identifier)
		{
			if (!flag1)
				__instance.FirePassengerStopEdgeMoved(passengerMarker.LastStopIdentifier);
			passengerMarker.LastStopIdentifier = __instance.identifier;
			car.SetPassengerMarker(passengerMarker);
		}
		for (var index = 0; index < passengerMarker.Groups.Count; ++index)
		{
			var group = passengerMarker.Groups[index];
			if (group.Count > 0)
			{
				var flag2 = passengerMarker.Destinations.Contains(group.Destination);
				var flag3 = group.Destination == __instance.identifier;
				if (!(!flag3 & flag2))
				{
					--group.Count;
					if (group.Count > 0)
					{
						passengerMarker.Groups[index] = group;
					}
					else
					{
						passengerMarker.Groups.RemoveAt(index);
						var num = index - 1;
					}
					car.SetPassengerMarker(passengerMarker);
					if (flag3)
					{
						__instance.QueuePayment(1, car.Condition, group.Origin, group.Destination, bonusMultiplier);
						__instance.FirePassengerStopServed(1, car, false);
					}
					else
					{
						__instance.UnloadPassengersToWait(group.Destination, 1);
						__instance.FirePassengerStopServed(-1, car, false);
					}

					__result = true;
					return Stuff.SKIP_ORIGINAL;
				}
			}
		}
		__result = false;
		return Stuff.SKIP_ORIGINAL;
	}
}

[HarmonyPatch(typeof(PassengerStop))]
[HarmonyPatch(nameof(PassengerStop.LoadCar))]
public class PassengerStop_LoadCar_Patch
{
	private static bool Prefix(ref PassengerStop __instance, Car car, ref bool __result)
	{
		var passengerMarker = PassengerStop.MarkerForCar(car);
		var maximum = __instance.PassengerCapacity(car) - passengerMarker.TotalPassengers;
		if (maximum <= 0)
		{
			__result = false;
			return Stuff.SKIP_ORIGINAL;
		}
		
		var num = __instance.AllocateWaitingPassengersForDestinations(maximum, passengerMarker.Destinations, out string destinationOut);
		if (num <= 0)
		{
			__result = false;
			return Stuff.SKIP_ORIGINAL;
		}

		var now = PassengerStop.Now;
		for (var index = 0; index < passengerMarker.Groups.Count; ++index)
		{
			var group = passengerMarker.Groups[index];
			if (group.Destination == destinationOut 
			    && group.Origin == __instance.identifier
			    && now.TotalSeconds - group.Boarded.TotalSeconds <= 600.0)
			{
				group.Count += num;
				passengerMarker.Groups[index] = group;
				car.SetPassengerMarker(passengerMarker);
				__instance.SaveState();
				__instance.FirePassengerStopServed(num, car, true);
				__result = true;
				return Stuff.SKIP_ORIGINAL;
			}
		}
		passengerMarker.Groups.Add(new PassengerMarker.Group(__instance.identifier, destinationOut, num, now));
		car.SetPassengerMarker(passengerMarker);
		__instance.SaveState();
		__instance.FirePassengerStopServed(num, car, true);
		__result = true;

		return Stuff.SKIP_ORIGINAL;
	}
}
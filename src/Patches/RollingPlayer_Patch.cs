using Audio;
using HarmonyLib;
using Model.Definition;

namespace visible_passengers.Patches;

/// <summary>
/// we use RollingPlayer because all rolling stock have this script
/// </summary>
[HarmonyPatch(typeof(RollingPlayer))]
[HarmonyPatch(nameof(RollingPlayer.OnEnable))]
public class RollingPlayer_OnEnable_Patch
{
	private static void Postfix(RollingPlayer __instance)
	{
		var car = __instance._car;

		if (car.Archetype != CarArchetype.Coach)
		{
			return;
		}

		//todo this probably doesn't work
		if (car.gameObject.GetComponent<CarPassengersManager>() is not null)
		{
			Main.Debug($"{car.name} already had {nameof(CarPassengersManager)}");
			return;
		}
	
		var pMan = car.gameObject.AddComponent<CarPassengersManager>();
		if (pMan is null)
		{
			Main.Error($"failed to add {nameof(CarPassengersManager)} to {car.name}");
			return;
		}
		
		Main.Debug($"successfully added {nameof(CarPassengersManager)} to {car.name}");

		pMan.theCar = car;
	}
}

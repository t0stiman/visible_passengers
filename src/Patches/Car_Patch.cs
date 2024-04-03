using HarmonyLib;
using UnityEngine;

namespace visible_passengers.Patches;

/// <summary>
/// this ensures the passenger avatars get destroyed when their car gets destroyed
/// </summary>
[HarmonyPatch(typeof(Model.Car))]
[HarmonyPatch(nameof(Model.Car.DestroyGameObjects))]
public class Car_DestroyGameObjects_Patch
{
	private static void Postfix(Model.Car __instance)
	{
		Main.Debug(nameof(Car_DestroyGameObjects_Patch));
		foreach(var component in __instance.gameObject.GetComponents(typeof(Component)))
		{
			if (!component.ToString().Contains(nameof(CarPassengersManager))) continue;
			GameObject.Destroy(component);
			return;
		}
	}
}


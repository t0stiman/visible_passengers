using System;

namespace visible_passengers;

public static class Stuff
{
	public const bool EXECUTE_ORIGINAL = true;
	public const bool SKIP_ORIGINAL = false;

	public const string PASSENGER_LOAD_ID = "passengers";
	public const string NPC_PREFIX = "[NPC] ";
	
	private static Random random = new();
	
	public static T RandomEnumValue<T> ()
	{
		var v = Enum.GetValues (typeof (T));
		return (T) v.GetValue (random.Next(v.Length));
	}
}
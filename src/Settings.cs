using UnityEngine;
using UnityModManagerNet;

namespace visible_passengers
{
	public class Settings : UnityModManager.ModSettings
	{
		//logging stuff
		public bool LogToConsole = false;
		public bool EnableDebugLogs = false;
		
		public void Draw(UnityModManager.ModEntry modEntry)
		{
			// logging stuff
			GUILayout.Label("Logging stuff: ");
			LogToConsole = GUILayout.Toggle(LogToConsole, "Log messages to the in-game console as well as Player.log");
			EnableDebugLogs = GUILayout.Toggle(EnableDebugLogs, "Enable debug messages");
		}

		public override void Save(UnityModManager.ModEntry modEntry)
		{
			Save(this, modEntry);
		}
	}
}
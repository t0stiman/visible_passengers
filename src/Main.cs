﻿using System;
using System.IO;
using System.Reflection;
using UnityModManagerNet;
using HarmonyLib;

namespace visible_passengers;

[EnableReloading]
static class Main
{
	private static UnityModManager.ModEntry myModEntry;
	private static Harmony myHarmony;
	public static Settings MySettings { get; private set; }

	//================================================================

	private static bool Load(UnityModManager.ModEntry modEntry)
	{
		try
		{
			myModEntry = modEntry;
			MySettings = UnityModManager.ModSettings.Load<Settings>(modEntry);
			
			modEntry.OnGUI = entry => MySettings.Draw(entry);
			modEntry.OnSaveGUI = entry => MySettings.Save(entry);
			modEntry.OnUnload = OnUnload;

			myHarmony = new Harmony(myModEntry.Info.Id);
			myHarmony.PatchAll(Assembly.GetExecutingAssembly());
		}
		catch (Exception ex)
		{
			myModEntry.Logger.LogException($"Failed to load {myModEntry.Info.DisplayName}:", ex);
			myHarmony?.UnpatchAll(myModEntry.Info.Id);
			return false;
		}
		
		modEntry.Logger.Log("loaded");

		return true;
	}

	private static bool OnUnload(UnityModManager.ModEntry modEntry)
	{
		myHarmony?.UnpatchAll(myModEntry.Info.Id);
		return true;
	}
	
	// Logger Commands
	public static void Debug(string message)
	{
		if (!MySettings.EnableDebugLogs)
		{
			return;
		}
		
		if (MySettings.LogToConsole)
		{
			Console.Log($"{myModEntry.Info.Id}[DEBUG]{message}");
		}
		myModEntry.Logger.Log(message);
	}
	
	public static void Info(string message)
	{
		if (MySettings.LogToConsole)
		{
			Console.Log($"{myModEntry.Info.Id}[INFO]{message}");
		}
		myModEntry.Logger.Log(message);
	}

	public static void Warning(string message)
	{
		if (MySettings.LogToConsole)
		{
			Console.Log($"{myModEntry.Info.Id}[WARNING]{message}");
		}
		myModEntry.Logger.Warning(message);
	}

	public static void Error(string message)
	{
		if (MySettings.LogToConsole)
		{
			Console.Log($"{myModEntry.Info.Id}[ERROR]{message}");
		}
		myModEntry.Logger.Error(message);
	}
}
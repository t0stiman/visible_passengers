using System;
using System.Collections.Generic;
using Avatar;
using Helpers;
using KeyValue.Runtime;
using UI.PreferencesWindow;

namespace visible_passengers.Extensions;

// Extending AvatarDescriptor doesn't work for some reason so I'm faking it.
public static class AvatarDescriptor_Extensions
{
	private static readonly Random random = new();

	public static AvatarDescriptor Random()
	{
		return Random(Stuff.RandomEnumValue<Gender>());
	}
	
	public static AvatarDescriptor Random(Gender gender)
	{
		return new AvatarDescriptor(
			gender, 
			random.Next(0,2), //0 = white, 1 = black 
			new Dictionary<string, Value>()
		{
			{
				"hat",
				Value.String(CharacterSettingsBuilder.Hats.Random().Identifier)
			},
			{
				"glasses",
				Value.String(CharacterSettingsBuilder.Glasses.Random().Identifier)
			},
			{
				"bandana",
				Value.String(CharacterSettingsBuilder.Bandana.Random().Identifier)
			},
			{
				"gloves",
				Value.String(CharacterSettingsBuilder.Gloves.Random().Identifier)
			},
		});
	}
}
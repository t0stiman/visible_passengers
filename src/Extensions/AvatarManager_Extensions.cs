using Avatar;
using Character;
using Game;
using RandomNameGeneratorLibrary;

namespace visible_passengers.Extensions;

public static class AvatarManager_Extensions
{
	private static readonly PersonNameGenerator personGenerator = new();
	
	public static NPC_Avatar AddNPC(this AvatarManager deez, PlayerId playerId, Gender gender)
	{
		var playerName = (gender == Gender.Male ? personGenerator.GenerateRandomMaleFirstAndLastName() : personGenerator.GenerateRandomFemaleFirstAndLastName())
			+ Stuff.NPC_POSTFIX;
		
		var avatarPrefab = deez.AddAvatar(AvatarDescriptor_Extensions.Random(gender), false, playerId, playerName);
		var remoteAvatar = avatarPrefab.gameObject.AddComponent<NPC_Avatar>();
		remoteAvatar.avatar = avatarPrefab;
		remoteAvatar.name = $"{playerId} ${playerName}"; //123 $John Smith
		return remoteAvatar;
	}
}



using Avatar;
using Character;
using Game;
using RandomNameGeneratorLibrary;

namespace visible_passengers.Extensions;

public static class AvatarManager_Extensions
{
	private static readonly PersonNameGenerator personGenerator = new();
	
	public static Passenger AddNPC(this AvatarManager deez, PlayerId playerId, Gender gender)
	{
		var playerName = Stuff.NPC_PREFIX+
		                 (gender == Gender.Male ? personGenerator.GenerateRandomMaleFirstAndLastName() : personGenerator.GenerateRandomFemaleFirstAndLastName());
		
		var avatarPrefab = deez.AddAvatar(AvatarDescriptor_Extensions.Random(gender), false, playerId, playerName);
		var remoteAvatar = avatarPrefab.gameObject.AddComponent<Passenger>();
		remoteAvatar.avatar = avatarPrefab;
		remoteAvatar.name = $"{playerId} ${playerName}"; //123 $John Smith
		return remoteAvatar;
	}
}



# FGNetworking Assignment
## 1. Overhead Names
[PlayerNameUI.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/UI/PlayerNameUI.cs)    [PlayerName.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Player/PlayerName.cs)

To display the player name above the players character I utilize the [SavedClientInformationManager.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/NetowkingScripts/Common/SavedClientInformationManager.cs) to get the UserData of that player. By inputing the clientID into the SavedCleintInformationManager I got the saved username from the player. Which is then stored in a NetworkVariable<FixedString64Bytes> with server only write perms which synchronizes it over all clients on the server. The PlayerNameUI class then gets a reference to the PlayerName class to display it on the client side only.

## 2. Health Packs
[HealthPack.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Powerups/HealthPack.cs)    [Health.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Player/Health.cs)

The health packs works like the mine by doing collision check on the server side and then calls the Health classes function Heal which takes a parameter for the amount health to be healed.

## 3. Sprite Renderer
[Moving.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Player/Moving.cs)    [MovingSpriteChange](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Player/MovingSpriteChange.cs)

The Moving class checks wether the rigidbodys velocitys magnitude is above 1 and changes a NetworkVariable<bool> to true so that all clients knows that player is moving. Then the MovingSpriteChange class has subscribed to the onValueChanged event on the bool networkVariable to activate a sprite if it is true to simulate a thruster/fire behind the ship. 

## 4. Limited Ammo
[FiringAction.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Player/FiringAction.cs)

In the FiringAction class I added a NetworkVariable<int> which has server only write permission that has the ammo amount left. When the player tries to shoot the function to fire on the client side checks if the ammo variable is greather then zero and if so it proceeds to shoot. When the serverRPC to shoot a bullet is called it decreases the ammo amount by one.

## 5. Shoot Timer
[FiringAction.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Player/FiringAction.cs)

In the FiringAction class I added a NetworkVariable<bool> which is controlled by the server by changing it into true if the player can shoot. In the Fire function it checks wether the variable is true and if so proceeds to shoot. When the server shoots its bullet it resets a timer and makes the can shoot variable to false. Then the server has a timer that makes the canshoot variable true after the cooldown time is over.

## 6. Ammo Pack
[AmmoPack.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Powerups/AmmoPack.cs)    [FiringAction.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Player/FiringAction.cs)

The Ammo pack works exactly like the health pack but instead of getting the health component and increasing health it instead gets the firing action and calls a function which replenishes the ammo to the start amount which is stored in a variable in the FiringAction. 

## 7. Cheat Detection
[PlayerAntiCheat.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/NetowkingScripts/PlayerAntiCheat.cs)    [FiringAction.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Player/FiringAction.cs)    [KickPlayer.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/NetowkingScripts/KickPlayer.cs)

The anti cheat system uses the PlayerAntiCheat class which checks the position from last frame to the current on the server side only and then there is a bool variable which checks if the distance between the positions are greater than the CheatSpeedThreshold float variable. Then in the ShootBulletServerRpc function in FiringAction it checks wether this variable is true and if so the player is kicked and a text appears on all players screen telling them that player has been kicked for cheating by using the KickPlayer class.

## 8. Shield Power-Up
[Health.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Player/Health.cs)    [ShieldPack.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Powerups/ShieldPack.cs)

In the Health class I added a NetworkVariable<Int> which holds how many shields the player has left. In the TakeDamage function it firstly checks wether the player has any shields left and if so it removes one of them instead of dealing the damage towards the health. I also decided to add a shield pack that will replenish the shield and it is triggered exactly like the health pack with collision.

## 9. Player Death || 10. Unlimited Respawn
[Health.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Player/Health.cs)

To make the player be able to die and respawn I expanded upon the TakingDamage function. After the player has taken damage I check wether the currentHealth is less or equal to zero and if so I reset the players health to its original amount and the same with the ammo. Then the server calls a clientRpc making the client place itself in a random position because the NetworkTransformClientAuth is client authority.

## 11. Limited Respawn
[GameManager.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/GameManager/GameManager.cs)      [Health.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/Player/Health.cs)    [GameOptionsUI.cs](https://github.com/ChristianBackstrom/FGNetworking/blob/master/Assets/Scripts/UI/MainMenuUI/GameOptionsUI.cs)

For the respawn amount I wanted the host to decide wether the players had unlimited respawns or if they were limited. For this I added a toggle in the main menu which if activated made the players have unlimited lives. That variable is stored in the playerPrefs so that the server can reach it when the game starts. In the gameManager I made a struct called GameOptions which right now holds only a bool for the unlimited lives option but it is made for future game options. That struct is stored in a NetworkVariable inside of the GameManager so that the Health class can then check if the game has unlimited lives and if so do not depleat the lives of the player when they die. The health amount is no longer displayed if the unlimited lives is true.

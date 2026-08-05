# ImmersiveTravel

A mod for Daggerfall Unity that implements diegetic fast-travel systems, inspired by the Silt Striders and boats in TES3:Morrowind. 

**1.5 UPDATE INCLUDES SIGNIFICANT CHANGES THAT CAN BE INCOMPATIBLE WITH SAVES MADE WITH PREVIOUS VERSIONS. IF YOU ARE CURRENTLY PLAYING WITH IMMERSIVETRAVEL 1.4, I RECOMMEND FINISHING YOUR PLAYTHROUGH BEFORE UPDATING THE MOD**

## Features
### Carriage Travel
You can find carriages outside the gates of every major city. Speaking to the driver initiates fast travel. In the settings, you can choose the daily cost of traveling with a carriage, which destination types (temples, dungeons, villages, etc.) to allow. You can also choose to restrict carriage travel to locations in the current region.

### Disable fast travel from anywhere
Optionally, you can disable the regular fast travel from anywhere, meaning you'll have to manually walk to any location that you can't reach by carriage. **This setting makes the game a lot harder and it could make completing the main quest impossible. It's not recommended to play this way if you're on your first playthrough.**

### Ship Travel
The mod [World of Daggerfall](https://www.nexusmods.com/daggerfallunity/mods/249) adds boats to all coastal towns. ImmersiveTravel gives them a fast travel function similar to the carriages. If World of Daggerfall is not installed, ship travel won't be available but the rest of the mod will work fine. In the settings, you can adjust the daily cost of ship travel, as well as restrict travel from small boats to the same region only. 

### Basic Roads integration
If [Basic Roads](https://www.nexusmods.com/daggerfallunity/mods/134) is installed, ImmersiveTravel will display its roads and paths on every map. This includes the carriage and ship travel map, as well as the normal travel map.

### Compatibility
* [Beatiful Cities of Daggerfall](https://www.nexusmods.com/daggerfallunity/mods/720): you must download the ImmersiveTravel compatibility patch from its nexus page.
* [TravelOptions](https://www.nexusmods.com/daggerfallunity/mods/122): should be loaded **before** ImmersiveTravel. In ImmersiveTravel's settings, make sure *disable normal fast travel* is off if you want to use all the features from TO. If *disable normal fast travel* is on, then you won't be able to use TO from the travel map, but you will still be able to initiate player-controlled sped-up travel by following roads.
* [Hidden Map Locations](https://www.nexusmods.com/daggerfallunity/mods/265): should be compatible but it hasn't been tested thoroughly, please contact me if you find any compatibility issues.
* Any other mod should be compatible as long as it doesn't touch fast travel and map mechanics, or changes city gates RMB blocks.

### Recommended Mods
* [World of Daggerfall](https://www.nexusmods.com/daggerfallunity/mods/249) for ship travel, see above
* [Quest Offer Locations](https://www.nexusmods.com/daggerfallunity/mods/201) is great if using the "disable normal fast travel" setting
* [Climates and Calories](https://www.nexusmods.com/daggerfallunity/mods/49) can make long distance travel more fun by adding survival and hunger mechanics.
* Any mod that makes manual travel more fun should be great, like [Free Rein](https://www.nexusmods.com/daggerfallunity/mods/854), [Come Sail Away](https://www.nexusmods.com/daggerfallunity/mods/1131), [Warm Ashes](https://www.nexusmods.com/daggerfallunity/mods/520) and many more.

### Full Changelog for version 1.5
* Added sliders to customise the cost of ship travel: a slider for the daily cost of renting a ship, only applicable if the player doesn't have one of their own; and a daily pay for a captain & crew to man the ship (still applies even if the player owns a ship)

* Added an option to lock carriages to regions: if this option is active, you will only be able to travel between the capitals of different regions, while from any other city you will be restricted to travelling within the same region.

* You can no longer use travel services when enemies are nearby

* The Travellers and Seafarers Guilds have been removed. This ensure full compatibility with mods that add other guilds, and it also makes gameplay smoother.

* Added some arbitrary "large ports" to every island in the game to make sure you can always travel out of them by sea. Port of Daggerfall is also now considered a "large port". These ports are:
Singbrugh in Balfiera
Warwych in Balfiera
Ruins of Mastersley Grange in Balfiera
Gallomarket in Balfiera
Old Lysausa's farm in Balfiera
Old Yeomhart's Graveyard in Balfiera
Kirkbeth Hamlet in Betony
Ipsmoth in Bhoriane
Old Vannabyth's farm in Bhoriane
Port of Daggerfall
Ruins of Cosh Hall in Cybiades
Zagoparia in Mournoth
Crimson Cat Inn in Satakalaam
Inner Altar of Dibella in Tigonus
Citadel of Heartham in Tulune
Let me know if I forgot any islands.

* BasicRoads is no longer considered a mandatory dependency

* The "disable normal travel" setting has been reworked. Now the vanilla TravelMap will still be replaced even if the option is toggled off, so that it can show roads and variable sized dots. If TravelOptions is active and the setting is off, the fast travel system will be entirely handled by TravelOptions, which should retain all of its functionalities. If the setting is toggled on, it will block TO's features like before, but road following will now work properly. Also, the setting will be off by default, so only people who really know what theyre getting into will disable fast travel.

* Some settings have been changed for better clarity and might need to be manually restored to their previous value after the update.

* All of the code has been revised, which lead to a few very minor bug fixes and performance improvements.

# ImmersiveTravel

A mod for Daggerfall Unity that implements diegetic fast-travel systems, inspired by the Silt Striders and boats in TES3:Morrowind. 

### Disclaimer and special thanks
*   This is my first mod (of any game), my first time using Unity, and my first time coding in C#, so feedback and bug reports are especially appreciated.
*  Special thanks to the DFU community in the Lysandus' Tomb discord server, especially [carademono](https://next.nexusmods.com/profile/carademono?gameId=2927) who provided invaluable support and helped with parts of the code; and [arjb](https://next.nexusmods.com/profile/ajrb?gameId=2927) whose many DFU modding tutorials helped me immensely; and finally all nexus users who commented on this mod providing much needed feedback and ideas.

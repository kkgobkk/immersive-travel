/* ImmersiveTravel.cs
 * Main class of the mod, contains initializations methods.
 * Handles integration with other mods and register custom factions,
 * guilds and UI windows.
 */

using UnityEngine;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Guilds;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;

namespace ImmersiveTravel
{
    public class ImmersiveTravel : MonoBehaviour
    {
        static Mod mod;
        static public ModSettings Settings;
        static public bool BasicRoadsEnabled { get; private set; }
        static public bool HiddenMapLocationsEnabled { get; private set; }
        static public bool TravelOptionsEnabled { get; private set; }
        
        /*direction constants from BasicRoadsTexturing (needed to
        draw BasicRoads roads and tracks on the travel map windows)*/
        public const byte N = 128;
        public const byte NE = 64;
        public const byte E = 32;
        public const byte SE = 16;
        public const byte S = 8;
        public const byte SW = 4;
        public const byte W = 2;
        public const byte NW = 1;

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            Debug.Log("[ImmersiveTravel] initializing mod");
            mod = initParams.Mod;
            Settings = mod.GetSettings();

            //check for mods to integrate
            Debug.Log("[ImmersiveTravel] checking for other mods to integrate");
            Mod basicRoads = ModManager.Instance.GetMod("BasicRoads");
            BasicRoadsEnabled = basicRoads != null && basicRoads.Enabled;
            if (BasicRoadsEnabled)
            {
                Debug.Log("[ImmersiveTravel] BasicRoads mod detected");
            }

            Mod travelOptions = ModManager.Instance.GetMod("TravelOptions");
            TravelOptionsEnabled = travelOptions != null && travelOptions.Enabled;
            if (TravelOptionsEnabled)
            {
                Debug.Log("[ImmersiveTravel] TravelOptions mod detected");
            }

            Mod hiddenMapLocations = ModManager.Instance.GetMod("Hidden Map Locations");
            HiddenMapLocationsEnabled = hiddenMapLocations != null && hiddenMapLocations.Enabled;
            if (HiddenMapLocationsEnabled)
            {
                Debug.Log("[ImmersiveTravel] Hidden Map Locations mod detected");
            }

            //register custom windows
            if (!TravelOptionsEnabled)
            {
                UIWindowFactory.RegisterCustomUIWindow(UIWindowType.TravelMap, typeof(CarriageMap));
            }

            if (Settings.GetValue<bool>("General", "DisableNormalTravel"))
            {
                UIWindowFactory.RegisterCustomUIWindow(UIWindowType.TravelPopUp, typeof(ImmersiveTravelPopUp));
            }

            //register custom factions
            Debug.Log("[ImmersiveTravel] registering factions");
            bool success = FactionFile.RegisterCustomFaction(8642, new FactionFile.FactionData()
            {
                id = 8642,
                parent = 0,
                type = 15,
                name = "Travellers Guild",
                summon = -1,
                region = -1,
                power = 100,
                face = -1,
                race = -1,
                sgroup = 1,
                ggroup = 16,
            });

            success = success && FactionFile.RegisterCustomFaction(8643, new FactionFile.FactionData()
            {
                id = 8643,
                parent = 8642,
                type = 15,
                name = "Travellers Guild Seafarers",
                summon = -1,
                region = -1,
                power = 100,
                face = -1,
                race = -1,
                sgroup = 1,
                ggroup = 16,
            });
           
            if (!success)
            {
                Debug.Log("[ImmersiveTravel] could not register custom factions!");
            }
            else
            {
                //register custom guilds
                Debug.Log("[ImmersiveTravel] registering guilds");
                if (!GuildManager.RegisterCustomGuild(FactionFile.GuildGroups.GGroup16, typeof(TravellersGuild)))
                {
                    Debug.Log("[ImmersiveTravel] GGgroup16 already in use!");
                }
                else{
                    //register custom services
                    Debug.Log("[ImmersiveTravel] registering services");
                    Services.RegisterGuildService(8642, TravellersGuild.CarriageTravelService, "Carriage Travel");
                    Services.RegisterGuildService(8643, TravellersGuild.ShipTravelService, "Ship Travel");
                }
            }

            //done
            mod.IsReady = true;
            Debug.Log("[ImmersiveTravel] finished mod init");
        }
    }
}
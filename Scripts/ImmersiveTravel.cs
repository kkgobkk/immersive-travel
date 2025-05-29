//this is the main class of the mod. It registers all new factions and guild services (CarriageDriversGuild.cs) 

using System;
using System.Collections.Generic;
using UnityEngine;
using DaggerfallConnect;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Formulas;
using DaggerfallWorkshop.Game.Guilds;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.MagicAndEffects.MagicEffects;
using DaggerfallWorkshop.Game.Questing;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.Serialization;

namespace ImmersiveTravel{
    
    public class ImmersiveTravel : MonoBehaviour
    {
        static public Mod mod;
        static public bool BasicRoadsEnabled { get; private set; }
        static public bool HiddenMapLocationsEnabled { get; private set; }
        
        //direction constants copied from BasicRoadsTexturing
        public const byte N = 128; //0b_1000_0000;
        public const byte NE = 64; //0b_0100_0000;
        public const byte E = 32;  //0b_0010_0000;
        public const byte SE = 16; //0b_0001_0000;
        public const byte S = 8;   //0b_0000_1000;
        public const byte SW = 4;  //0b_0000_0100;
        public const byte W = 2;   //0b_0000_0010;
        public const byte NW = 1;  //0b_0000_0001;

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams){
            Debug.Log("[ImmersiveTravel] initializing mod");
            mod = initParams.Mod;

            //register travel popup
            UIWindowFactory.RegisterCustomUIWindow(UIWindowType.TravelPopUp, typeof(ImmersiveTravelPopUp));

            //register new factions
            Debug.Log("[ImmersiveTravel] registering factions");
            FactionFile.RegisterCustomFaction(8642, new FactionFile.FactionData()
            {
                id = 8642,
                parent = 0,
                type = 15,
                name = "Iliac Bay Transport Company",
                summon = -1,
                region = -1,
                power = 100,
                face = -1,
                race = -1,
                sgroup = 1,
                ggroup = 16,
            }); 
            FactionFile.RegisterCustomFaction(8643, new FactionFile.FactionData()
            {
                id = 8643,
                parent = 0,
                type = 15,
                name = "Seafarers League",
                summon = -1,
                region = -1,
                power = 100,
                face = -1,
                race = -1,
                sgroup = 1,
                ggroup = 18,
            });
           
           //registr new guilds
           Debug.Log("[ImmersiveTravel] registering guilds");
            GuildManager.RegisterCustomGuild(FactionFile.GuildGroups.GGroup16, typeof(CarriageDriversGuild));
            GuildManager.RegisterCustomGuild(FactionFile.GuildGroups.GGroup18, typeof(SeafarersLeague));

            //registr new services
            Debug.Log("[ImmersiveTravel] registering services");
            Services.RegisterGuildService(8642, CarriageDriversGuild.CarriageTravelService, "Carriage Travel");
            Services.RegisterGuildService(8643, SeafarersLeague.ShipTravelService, "Ship Travel");

            Debug.Log("[ImmersiveTravel] checking for other mods to integrate");
            //basic roads integration
            Mod BasicRoads = ModManager.Instance.GetMod("BasicRoads");
            BasicRoadsEnabled = (BasicRoads != null) && BasicRoads.Enabled;

            //HiddenMapLocations integration
            Mod hiddenMapLocations = ModManager.Instance.GetMod("Hidden Map Locations");
            HiddenMapLocationsEnabled = hiddenMapLocations != null && hiddenMapLocations.Enabled;

            ModManager.Instance.GetMod(initParams.ModTitle).IsReady = true;
            Debug.Log("[ImmersiveTravel] finished mod init");
        }
    }
}
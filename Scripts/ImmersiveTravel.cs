//this is the main class of the mod. It registers all new factions and guild services (CarriageDriversGuild.cs)  and the overridden map (ImmersiveTravelMap.cs) to the game.

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
        static private StaticNPC npc;

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams){
            mod = initParams.Mod;
            if(!(AddCarriageDriversFaction() && AddTravelService() && AddFakeGuild()))
                throw new Exception("faction and service ID is already used.");
            if(mod.GetSettings().GetValue<bool>("General", "DisableFastTravel"))
                UIWindowFactory.RegisterCustomUIWindow(UIWindowType.TravelMap, typeof(ImmersiveTravelMap));
            UIWindowFactory.RegisterCustomUIWindow(UIWindowType.TravelPopUp, typeof(ImmersiveTravelPopUp));
            ModManager.Instance.GetMod(initParams.ModTitle).IsReady = true;
        }

        //when loading the mod, replaces the vanilla travel map with a disabled one (you can open it, but not travel)
        public void Awake(){
            if(mod.GetSettings().GetValue<bool>("General", "DisableFastTravel"))
                UIWindowFactory.RegisterCustomUIWindow(UIWindowType.TravelMap, typeof(ImmersiveTravelMap));
        }

        //register a new faction to FACTIONS.TXT, used to check if an npc is a carriage driver
        private static bool AddCarriageDriversFaction(){
            return FactionFile.RegisterCustomFaction(8642, new FactionFile.FactionData()
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
                ggroup = 8,
            });
        }

        private static bool AddTravelService(){
            return Services.RegisterGuildService(8642, CarriageDriversGuild.CarriageTravelService, "Carriage Travel");
        }

        private static bool AddFakeGuild(){
            return GuildManager.RegisterCustomGuild(FactionFile.GuildGroups.GGroup8, typeof(CarriageDriversGuild));
        }
    }
}
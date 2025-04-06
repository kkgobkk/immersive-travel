//This class implements the "carriage drivers" guild, now renamed to "Iliac Bay Transport Company" in-game.

using System.Collections.Generic;
using UnityEngine;
using DaggerfallConnect;
using DaggerfallConnect.Arena2;
using DaggerfallConnect.FallExe;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using ImmersiveTravel;

namespace DaggerfallWorkshop.Game.Guilds{
    public class CarriageDriversGuild : Guild
    {
        private const int factionId = 8642;
        protected static string[] rankTitles = {"associate"};
        protected static List<DFCareer.Skills> guildSkills = new List<DFCareer.Skills>() {};
        protected static List<DFCareer.Skills> trainingSkills = new List<DFCareer.Skills>() {};
        public override string[] RankTitles { get { return rankTitles; } }
        public override List<DFCareer.Skills> GuildSkills { get { return guildSkills; } }
        public override List<DFCareer.Skills> TrainingSkills { get { return trainingSkills; } }
        protected static TextFile.Token newLine = TextFile.CreateFormatToken(TextFile.Formatting.JustifyCenter);

        //it's not like this text will ever be displayed, but since it's required, might as well write it
        public override TextFile.Token[] TokensIneligible(PlayerEntity playerEntity){
            TextFile.Token[] tmp = {
                TextFile.CreateTextToken("Hmm... I'm sorry, I don't think i want"), newLine,
                TextFile.CreateTextToken("to travel with the likes of you. Get lost!"), newLine,
            };
            return tmp;
        }
        
        public override TextFile.Token[] TokensWelcome(){
            TextFile.Token[] tmp = {
                TextFile.CreateTextToken("Great! Whenever you're ready, just come to me"), newLine,
                TextFile.CreateTextToken("and tell me where you need to go. I'll"), newLine,
                TextFile.CreateTextToken("take care of the rest. Just remember that my"), newLine,
                TextFile.CreateTextToken("fees only include the journey itself, if you"), newLine,
                TextFile.CreateTextToken("want to rest at an inn or take a ferry across"), newLine,
                TextFile.CreateTextToken("the Bay, you'll need to pay for it yourself."), newLine,
            };
            return tmp;
        }

        public override TextFile.Token[] TokensEligible(PlayerEntity playerEntity){
            TextFile.Token[] tmp = {
                TextFile.CreateTextToken("Hello adventurer. Want to travel?"), newLine,
                TextFile.CreateTextToken("I can take you anywhere in the Iliac Bay"), newLine,
                TextFile.CreateTextToken("for a modest price. All you need to do is"), newLine,
                TextFile.CreateTextToken("sign this contract and officially become"), newLine,
                TextFile.CreateTextToken("an associate of the Transport Company."), newLine,
            };
            return tmp;
        }

        //it's not like this text will ever be displayed, but since it's required, might as well write it
        public override TextFile.Token[] TokensPromotion(int newRank){
            TextFile.Token[] tmp = {
                TextFile.CreateTextToken("Promotion? Lmao, you know this isn't a"), newLine,
                TextFile.CreateTextToken("real guild, don't you?"), newLine,
            };
            return tmp;
        }

        public override TextFile.Token[] TokensExpulsion(){
            TextFile.Token[] tmp = {
                TextFile.CreateTextToken("It seems your contract with our Company has"), newLine,
                TextFile.CreateTextToken("expired! You'll have to sign a new one if you"), newLine,
                TextFile.CreateTextToken("want to keep using our services! You know, bureaucracy."), newLine,
            };
            return tmp;
        }


        //you can always travel. (tho it would be fun to force evil/low PER characters to walk everywhere just because people hate them)
        public override bool IsEligibleToJoin(PlayerEntity playerEntity)
        {
            return true;
        }

        public override bool CanAccessService(GuildServices service){
            return true;
        }

        override public void Join()
        {
            base.Join();
        }

        public override int GetFactionId(){
            return factionId;
        }

        //create a custom "guild" service for the carriage drivers
        public static void CarriageTravelService(IUserInterfaceWindow window){
            CarriageMap cTravelMap = new CarriageMap(DaggerfallUI.UIManager);
            DaggerfallUI.UIManager.PushWindow(cTravelMap);
        }
    }
}

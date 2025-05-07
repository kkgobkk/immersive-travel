//This class implements the "seafarers league" guild

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
    public class SeafarersLeague : Guild
    {
        private const int factionId = 8643;
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
                TextFile.CreateTextToken("Excellent! talk to me whenever you're ready"), newLine,
                TextFile.CreateTextToken("to set sail! Also, remember that you'll find "), newLine,
                TextFile.CreateTextToken("many other captains of the Seafarers League "), newLine,
                TextFile.CreateTextToken("in every costal city of the Iliac Bay."), newLine,
            };
            return tmp;
        }

        public override TextFile.Token[] TokensEligible(PlayerEntity playerEntity){
            TextFile.Token[] tmp = {
                TextFile.CreateTextToken("If you need a ferry across the Bay, look no further!"), newLine,
                TextFile.CreateTextToken("The Seafarers League will put their many"), newLine,
                TextFile.CreateTextToken("ships and sailors at your disposal for a"), newLine,
                TextFile.CreateTextToken("small fee. Do you wish to use our services?"), newLine,
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
                TextFile.CreateTextToken("It seems your travelling permit with the League has"), newLine,
                TextFile.CreateTextToken("expired! I can sign a new one for you if"), newLine,
                TextFile.CreateTextToken("you need, all you need to do is ask!"), newLine,
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
        public static void ShipTravelService(IUserInterfaceWindow window){
            SeafarersMap sTravelMap = new SeafarersMap(DaggerfallUI.UIManager);
            DaggerfallUI.UIManager.PushWindow(sTravelMap);
        }
    }
}

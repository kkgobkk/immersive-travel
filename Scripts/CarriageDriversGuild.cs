//This class implements the "carriage drivers" guild, now renamed to "Transport Guild" in-game.

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
                TextFile.CreateTextToken("and tell me where you need to go. I'll take care"), newLine,
                TextFile.CreateTextToken("of the rest. Just know that extras like sleeping"), newLine,
                TextFile.CreateTextToken("at an inn or taking a ferry aren't covered in the"), newLine,
                TextFile.CreateTextToken("fee. Oh, and if you need to use the guild's services"), newLine,
                TextFile.CreateTextToken("again, you'll find carriage drivers just outside"), newLine,
                TextFile.CreateTextToken("the gates of most large towns."), newLine,
            };
            return tmp;
        }

        public override TextFile.Token[] TokensEligible(PlayerEntity playerEntity){
            TextFile.Token[] tmp = {
                TextFile.CreateTextToken("Hello adventurer. Want to travel?"), newLine,
                TextFile.CreateTextToken("The Travellers Guild can take you anywhere"), newLine,
                TextFile.CreateTextToken("in the Iliac Bay for a modest price. "), newLine,
                TextFile.CreateTextToken("All you need to do is sign this contract"), newLine,
                TextFile.CreateTextToken("to officially become an associate."), newLine,
            };
            return tmp;
        }

        //it's not like this text will ever be displayed, but since it's required, might as well write it just in case
        public override TextFile.Token[] TokensPromotion(int newRank){
            TextFile.Token[] tmp = {
                TextFile.CreateTextToken("Promotion? Lmao, you know this isn't a"), newLine,
                TextFile.CreateTextToken("real guild, don't you?"), newLine,
            };
            return tmp;
        }

        public override TextFile.Token[] TokensExpulsion(){
            TextFile.Token[] tmp = {
                TextFile.CreateTextToken("It seems your travelling permit for the Guild has"), newLine,
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

        override public void Join(){
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

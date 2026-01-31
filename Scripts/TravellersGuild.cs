/* TravellersGuild.cs
 * This class implements the "Travellers Guild". It provides methods
 * to check wether the player is able to join the guild and access its
 * services, methods that return the text to be displayed in game for various
 * actions, and methods that implement the actual services.
*/

using System.Collections.Generic;
using DaggerfallConnect;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.UserInterface;
using ImmersiveTravel;

namespace DaggerfallWorkshop.Game.Guilds
{
    public class TravellersGuild : Guild
    {
        private const int factionId = 8642;
        private const int carriageTravelServiceID = 8642;
        private const int shipTravelServiceID = 8643;

        protected static string[] rankTitles = {"associate"};
        protected static List<DFCareer.Skills> guildSkills = new List<DFCareer.Skills>() {};
        protected static List<DFCareer.Skills> trainingSkills = new List<DFCareer.Skills>() {};
        protected static TextFile.Token newLine = TextFile.CreateFormatToken(TextFile.Formatting.JustifyCenter);
        
        public override string[] RankTitles { get { return rankTitles; } }
        public override List<DFCareer.Skills> GuildSkills { get { return guildSkills; } }
        public override List<DFCareer.Skills> TrainingSkills { get { return trainingSkills; } }
        
        public override int GetFactionId()
        {
            return factionId;
        }
        
        public override bool IsEligibleToJoin(PlayerEntity playerEntity)
        {
            return true; //you can always travel
        }
       
        public override bool CanAccessService(GuildServices service)
        {
            return true;  //you can always travel
        }

        //custom guild service for the carriage drivers
        public static void CarriageTravelService(IUserInterfaceWindow window)
        {
            CarriageMap carriageTravelMap = new CarriageMap(DaggerfallUI.UIManager);
            DaggerfallUI.UIManager.PushWindow(carriageTravelMap);
        }

        //custom guild service for ship captain
        public static void ShipTravelService(IUserInterfaceWindow window)
        {
            SeafarersMap shipTravelMap = new SeafarersMap(DaggerfallUI.UIManager);
            DaggerfallUI.UIManager.PushWindow(shipTravelMap);
        }

        public override TextFile.Token[] TokensEligible(PlayerEntity playerEntity)
        {
            TextFile.Token[] tmp = 
            {
                TextFile.CreateTextToken("Greetings, adventurer!"), newLine, newLine,
                TextFile.CreateTextToken("Are you looking for transportation?"), newLine,
                TextFile.CreateTextToken("Wether it's for pleasure or business, the"), newLine,
                TextFile.CreateTextToken("Travellers Guild will take you anywhere in"), newLine,
                TextFile.CreateTextToken("the Iliac Bay for a reasonable price."), newLine,
                TextFile.CreateTextToken("We have plenty of carriages outside the"), newLine,
                TextFile.CreateTextToken("gates of every major town, and you'll find"), newLine,
                TextFile.CreateTextToken("our ships at every dock in the bay! If you"), newLine,
                TextFile.CreateTextToken("wish to travel with the Guild, all you need"), newLine,
                TextFile.CreateTextToken("to do is sign this brief contract right here!"), newLine,

            };
            return tmp;
        }

        public override TextFile.Token[] TokensWelcome()
        {
            TextFile.Token[] tmp = 
            {
                TextFile.CreateTextToken("Great! Whenever you're ready, just come to me"), newLine,
                TextFile.CreateTextToken("and tell me where you need to go. I'll take care"), newLine,
                TextFile.CreateTextToken("of the rest. Just know that extras like sleeping"), newLine,
                TextFile.CreateTextToken("at an inn or taking a ferry aren't covered in the"), newLine,
                TextFile.CreateTextToken("fee. Oh, and remember: if you need to use the"), newLine,
                TextFile.CreateTextToken("Guild's services again, you'll find carriage drivers"), newLine,
                TextFile.CreateTextToken("just outside the gates of most large towns."), newLine,
            };
            return tmp;
        }

        public override TextFile.Token[] TokensExpulsion()
        {
            TextFile.Token[] tmp = 
            {
                TextFile.CreateTextToken("It seems your travelling permit has expired."), newLine,
                TextFile.CreateTextToken("You know, bureaucracy! I can sign a new one"), newLine,
                TextFile.CreateTextToken("for you if you wish, all you need to do is ask!"), newLine,
            };
            return tmp;
        }

        //it shouldn't be possible to actually see this text in game
        public override TextFile.Token[] TokensIneligible(PlayerEntity playerEntity)
        {
            TextFile.Token[] tmp = 
            {
                TextFile.CreateTextToken("Hmm... I'm sorry, I don't think i want"), newLine,
                TextFile.CreateTextToken("to travel with the likes of you. Get lost!"), newLine,
            };
            return tmp;
        }
        
        //it shouldn't be possible to actually see this text in game
        public override TextFile.Token[] TokensPromotion(int newRank)
        {
            TextFile.Token[] tmp = 
            {
                TextFile.CreateTextToken("Promotion? You know you're just an associate,"), newLine,
                TextFile.CreateTextToken("not a worker in the guild, don't you?"), newLine,
            };
            return tmp;
        }
    }
}

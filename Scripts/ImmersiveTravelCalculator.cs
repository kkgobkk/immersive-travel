/*This class is used to replace the travel cost formula so that it accounts for the driver's guild. This cost shouldn't 
be a problem for players as it's just 1/5th of the cost for inns.*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DaggerfallConnect.Utility;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;

namespace ImmersiveTravel{
    public class ImmersiveTravelCalculator : TravelTimeCalculator
    {
        public override void CalculateTripCost(int travelTimeInMinutes, bool sleepModeInn, bool hasShip, bool travelShip)
            {
                int travelTimeInHours = (travelTimeInMinutes + 59) / 60;
                int CarriageFee = ImmersiveTravel.mod.GetSettings().GetValue<int>("General", "DailyCarriageFee");
                piecesCost = 0;
                if (sleepModeInn && !GameManager.Instance.GuildManager.GetGuild(FactionFile.GuildGroups.KnightlyOrder).FreeTavernRooms())
                {
                    piecesCost = 5 * ((travelTimeInHours - OceanPixels) / 24);
                    if (piecesCost < 0)     // This check is absent from classic. Without it travel cost can become negative.
                        piecesCost = 0;
                    piecesCost += 5;        // Always at least one stay at an inn
                }
                totalCost = piecesCost + CarriageFee * ((travelTimeInHours - OceanPixels) / 24) + CarriageFee; 
                if ((OceanPixels > 0) && !hasShip && travelShip)
                    totalCost += 25 * (OceanPixels / 24 + 1);
                if (totalCost < 0)     // This check is absent from classic. Without it travel cost can become negative.
                        totalCost = 0;
            }
    }
}
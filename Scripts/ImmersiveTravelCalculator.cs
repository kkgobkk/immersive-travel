/*
ImmersiveTravelCalculator.cs

This class overrides the vanilla TravelTimeCalculator class, used
to compute travel times and travel costs. This modified version accounts
for additional costs added by the mod, such as carriage guild fees and
ship captain fees.
*/

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
                int carriageFee = ImmersiveTravel.mod.GetSettings().GetValue<int>("General", "DailyCarriageFee");
                int shipFee = ImmersiveTravel.mod.GetSettings().GetValue<int>("ShipTravel", "DailyShipCost");
                int captainFee = ImmersiveTravel.mod.GetSettings().GetValue<int>("ShipTravel", "DailyCaptainFee");
                piecesCost = 0; //the part of the total cost that must be paid in gold pieces (not letters of credit)

                //compute total cost of sleeping at inns
                if (sleepModeInn && !GameManager.Instance.GuildManager.GetGuild(FactionFile.GuildGroups.KnightlyOrder).FreeTavernRooms())
                {
                    piecesCost = 5 * ((travelTimeInHours - OceanPixels) / 24);
                    if (piecesCost < 0)
                        piecesCost = 0;
                    piecesCost += 5;    //Always at least one stay at an inn
                }

                //add carriage fees
                totalCost = piecesCost + carriageFee * ((travelTimeInHours - OceanPixels) / 24) + carriageFee; 

                if (travelShip) 
                {
                    //add ship fees (only if the player has to rent a ship. This cost will be 0 if the player already owns a ship)
                    if (!hasShip)
                        totalCost += shipFee * (OceanPixels / 24 + 1);

                    //always add ship captain fees
                    totalCost += captainFee * (OceanPixels / 24 + 1);
                }

                //just in case
                if (totalCost < 0)
                    totalCost = 0;
            }
    }
}
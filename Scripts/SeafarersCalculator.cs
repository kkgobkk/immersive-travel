/*
SeafarersCalculator.cs

This class overrides the vanilla TravelTimeCalculator class, used
to compute travel times and travel costs. This modified version accounts
for additional costs added by the mod, such as carriage guild fees and
ship captain fees. This specific version is only used when travelling
with a ship captain.
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
    public class SeafarersCalculator : TravelTimeCalculator
    {

        public override int CalculateTravelTime(DFPosition endPos,
            bool speedCautious = false,
            bool sleepModeInn = false,
            bool travelShip = false,
            bool hasHorse = false,
            bool hasCart = false)
        {
            DFPosition position = GetPlayerTravelPosition();
            int playerXMapPixel = position.X;
            int playerYMapPixel = position.Y;
            int distanceXMapPixels = endPos.X - playerXMapPixel;
            int distanceYMapPixels = endPos.Y - playerYMapPixel;
            int distanceXMapPixelsAbs = Mathf.Abs(distanceXMapPixels);
            int distanceYMapPixelsAbs = Mathf.Abs(distanceYMapPixels);
            int furthestOfXandYDistance = 0;

            if (distanceXMapPixelsAbs <= distanceYMapPixelsAbs)
                furthestOfXandYDistance = distanceYMapPixelsAbs;
            else
                furthestOfXandYDistance = distanceXMapPixelsAbs;

            int xPixelMovementDirection = (distanceXMapPixels >= 0) ? 1 : -1;
            int yPixelMovementDirection = (distanceYMapPixels >= 0) ? 1 : -1;

            int numberOfMovements = 0;
            int shorterOfXandYDistanceIncrementer = 0;

            int minutesTakenTotal = 0;

            while (numberOfMovements < furthestOfXandYDistance)
            {
                if (furthestOfXandYDistance == distanceXMapPixelsAbs)
                {
                    playerXMapPixel += xPixelMovementDirection;
                    shorterOfXandYDistanceIncrementer += distanceYMapPixelsAbs;

                    if (shorterOfXandYDistanceIncrementer > distanceXMapPixelsAbs)
                    {
                        shorterOfXandYDistanceIncrementer -= distanceXMapPixelsAbs;
                        playerYMapPixel += yPixelMovementDirection;
                    }
                }
                else
                {
                    playerYMapPixel += yPixelMovementDirection;
                    shorterOfXandYDistanceIncrementer += distanceXMapPixelsAbs;

                    if (shorterOfXandYDistanceIncrementer > distanceYMapPixelsAbs)
                    {
                        shorterOfXandYDistanceIncrementer -= distanceYMapPixelsAbs;
                        playerXMapPixel += xPixelMovementDirection;
                    }
                }

                minutesTakenTotal += 51;
                ++numberOfMovements;
            }

            if (!speedCautious)
                minutesTakenTotal = minutesTakenTotal >> 1;

            return minutesTakenTotal;
        }

        public override void CalculateTripCost(int travelTimeInMinutes, bool sleepModeInn, bool hasShip, bool travelShip)
            {
                int travelTimeInHours = (travelTimeInMinutes + 59) / 60;
                int shipFee = ImmersiveTravel.mod.GetSettings().GetValue<int>("ShipTravel", "DailyShipCost");
                int captainFee = ImmersiveTravel.mod.GetSettings().GetValue<int>("ShipTravel", "DailyCaptainFee");
                totalCost = 0;

                //add ship fees (only if the player has to rent a ship. This cost will be 0 if the player already owns a ship)
                if (!hasShip)
                    totalCost += shipFee * (travelTimeInHours / 24 + 1);

                //always add ship captain fees
                totalCost += captainFee * (travelTimeInHours / 24 + 1);

                //just in case
                if (totalCost < 0)
                    totalCost = 0;
            }
    }
}
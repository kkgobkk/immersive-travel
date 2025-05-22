//overrides the travel popup window, changing the cost formula. See also: ImmersiveTravelCalculator.cs

using System;
using UnityEngine;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallConnect.Utility;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Utility;
using DaggerfallConnect;

namespace ImmersiveTravel
{
    public class ImmersiveTravelPopUp : DaggerfallTravelPopUp
    {
        protected static bool DisableShipTravel = ImmersiveTravel.BasicRoadsEnabled && ImmersiveTravel.mod.GetSettings().GetValue<bool>("ShipTravel", "DisableShipTravelOutsideDocks");

        public ImmersiveTravelPopUp(IUserInterfaceManager uiManager, IUserInterfaceWindow previousWindow = null, DaggerfallTravelMapWindow travelWindow = null)
            : base(uiManager, previousWindow, travelWindow)
        {
            travelTimeCalculator = new ImmersiveTravelCalculator();
        }

        // Called by the travel map window when the destination is selected.
        // Ensure that the coordinates passed are in the same region coordinate space that the map file reader expects.
        public void SetEndPosPlease(DFPosition pos)
        {
            this.EndPos = pos;
            Debug.Log("ImmersiveTravelPopUp: Destination EndPos set to: " + pos);
        }

        public override void OnPush()
        {
            base.OnPush();  // Call the base implementation

            if(DisableShipTravel)
                TravelShip = false;

            CarriageMap carriageMap = TravelWindow as CarriageMap;
            if (carriageMap == null)
            {
                // Try to retrieve the location summary for the destination.
                ContentReader.MapSummary summary;
                if (DaggerfallWorkshop.DaggerfallUnity.Instance.ContentReader.HasLocation(EndPos.X, EndPos.Y, out summary))
                {
                    DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
                    messageBox.OnButtonClick += (_sender, button) =>
                    {
                        CloseWindow();  // Close the popup when OK is clicked
                        CloseWindow();  // Close the popup when OK is clicked
                    };
                    if (CarriageMap.IsDestinationValid(summary.LocationType))
                    {
                        messageBox.SetText("You must talk to a carriage driver to travel there.");
                        Button okButton = messageBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.OK, true);
                        // Push the message box so it displays immediately.
                        uiManager.PushWindow(messageBox);
                    }
                    else
                    {
                        messageBox.SetText("You cannot travel to this type of location.");
                        Button okButton = messageBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.OK, true);
                        // Push the message box so it displays immediately.
                        uiManager.PushWindow(messageBox);
                    }
                }
                else
                {
                    Debug.Log("No location summary found for EndPos " + EndPos);
                }
            }
        }

        //disables ship travel and pushes an error to the screen
        public void ForceNonShipTravel(){
            TravelShip = false;
            DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
            messageBox.SetText("Cannot enable ship travel without talking to a ship captain.");
            Button okButton = messageBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.OK, true);
            messageBox.OnButtonClick += (_sender, button) =>
            {
                CloseWindow();  // Close the popup when OK is clicked
            };
            // Push the message box so it displays immediately.
            uiManager.PushWindow(messageBox);
        }

        //the following few methods are overridden to handle the "disable ship travel outside of docks" setting

        public override void TransportModeButtonOnClickHandler(BaseScreenComponent sender, Vector2 position)
        {
            if ((!TravelShip) && DisableShipTravel){
                ForceNonShipTravel();
                return;
            }
            base.TransportModeButtonOnClickHandler(sender, position);
        }

        public override void ToggleTransportModeButtonOnScrollHandler(BaseScreenComponent sender)
        {
            if ((!TravelShip) && DisableShipTravel){
                ForceNonShipTravel();
                return;
            }
            base.ToggleTransportModeButtonOnScrollHandler(sender);
        }

        public override void SleepModeButtonOnClickHandler(BaseScreenComponent sender, Vector2 position)
        {
            if (sender == campOutToggleButton && DisableShipTravel)
                    TravelShip = false;
            base.SleepModeButtonOnClickHandler(sender, position);
        }

        public override void ToggleSleepModeButtonOnScrollHandler(BaseScreenComponent sender)
        {
            if (sender == campOutToggleButton && DisableShipTravel)
                TravelShip = false;
            base.ToggleSleepModeButtonOnScrollHandler(sender);
        }
    }
}

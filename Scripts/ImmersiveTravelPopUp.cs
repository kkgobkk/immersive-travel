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

            CarriageMap carriageMap = TravelWindow as CarriageMap;
            if (carriageMap == null)
            {
                Debug.Log("ImmersiveTravelPopUp OnPush, EndPos: " + EndPos.X + ", " + EndPos.Y);

                // Try to retrieve the location summary for the destination.
                ContentReader.MapSummary summary;
                if (DaggerfallWorkshop.DaggerfallUnity.Instance.ContentReader.HasLocation(EndPos.X, EndPos.Y, out summary))
                {
                    Debug.Log("Retrieved location summary for EndPos " + EndPos + " with type: " + summary.LocationType.ToString());

                    // Only show the message if the destination is a TownCity.
                    if (summary.LocationType == DFRegion.LocationTypes.TownCity ||
                        summary.LocationType == DFRegion.LocationTypes.TownHamlet ||
                        summary.LocationType == DFRegion.LocationTypes.TownVillage)
                    {
                        // Create and configure the message box.
                        DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
                        messageBox.SetText("You must talk to a carriage driver to travel there.");
                        Button okButton = messageBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.OK, true);
                        messageBox.OnButtonClick += (_sender, button) =>
                        {
                            CloseWindow();  // Close the popup when OK is clicked
                            CloseWindow();  // Close the popup when OK is clicked
                        };

                        // Push the message box so it displays immediately.
                        uiManager.PushWindow(messageBox);
                    }
                    else
                    {
                        // Create and configure the message box.
                        DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
                        messageBox.SetText("Carriage drivers will only take you to towns.");
                        Button okButton = messageBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.OK, true);
                        messageBox.OnButtonClick += (_sender, button) =>
                        {
                            CloseWindow();  // Close the popup when OK is clicked
                            CloseWindow();  // Close the popup when OK is clicked
                        };

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

    }
}


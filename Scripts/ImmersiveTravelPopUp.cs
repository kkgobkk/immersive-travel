/* ImmersiveTravelPopup.cs
 * Overrides the travel popup window to change the cost formula and
 * ensure fast travel only happens through carriage drivers, and between
 * valid locations according to the mod settings. If "disable vanilla
 * fast travel" is false, this class is only used when travelling with
 * a carriage driver and doesn't interfere with traveling through the map.
*/

using UnityEngine;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Utility;
using DaggerfallConnect.Utility;
using DaggerfallConnect.Arena2;

namespace ImmersiveTravel
{
    public class ImmersiveTravelPopUp : DaggerfallTravelPopUp
    {
        protected static bool disableShipTravel = ImmersiveTravel.Settings.GetValue<bool>("ShipTravel", "DisableShipTravelOutsideDocks");
        protected static bool travelOnlyFromNPCs = ImmersiveTravel.Settings.GetValue<bool>("General", "DisableNormalTravel");

        public ImmersiveTravelPopUp(IUserInterfaceManager uiManager, IUserInterfaceWindow previousWindow = null, DaggerfallTravelMapWindow travelWindow = null)
            : base(uiManager, previousWindow, travelWindow)
        {
            travelTimeCalculator = new ImmersiveTravelCalculator();
        }

        /* Called by the travel map window when the destination is selected.
        Ensure that the coordinates passed are in the same region coordinate space that the map file reader expects. */
        public void SetEndPosition(DFPosition pos)
        {
            this.EndPos = pos;
            Debug.Log("ImmersiveTravelPopUp: Destination EndPos set to: " + pos);
        }

        public override void OnPush()
        {
            base.OnPush();  // Call the base implementation
            
            if(disableShipTravel)
                TravelShip = false;

            CarriageMap carriageMap = TravelWindow as CarriageMap;
            if ((carriageMap == null || !carriageMap.CreatedByNPC) && travelOnlyFromNPCs)
            {
                // Try to retrieve the location summary for the destination.
                ContentReader.MapSummary summary;
                if (DaggerfallWorkshop.DaggerfallUnity.Instance.ContentReader.HasLocation(EndPos.X, EndPos.Y, out summary))
                {
                    DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
                    messageBox.OnButtonClick += (_sender, button) => {
                    
                        DaggerfallUI.Instance.PlayOneShot(DaggerfallWorkshop.SoundClips.ButtonClick);
                        PopWindow();
                        PopWindow();
                        doFastTravel = false;
                    };
                    if (CarriageMap.IsDestinationValid(summary.LocationType))
                    {
                        messageBox.SetText("You must hire a carriage driver to travel there.");
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
        public void ForceNonShipTravel()
        {
            TravelShip = false;
            DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
            messageBox.SetText("You must talk to a sailor to initiate ship travel.");
            Button okButton = messageBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.OK, true);
            messageBox.OnButtonClick += (_sender, button) =>
            {
                CloseWindow();  // Close the popup when OK is clicked
            };
            // Push the message box so it displays immediately.
            uiManager.PushWindow(messageBox);
        }

        //the following method is overridden to handle the "disable ship travel outside of docks" setting
        public override void TransportModeButtonOnClickHandler(BaseScreenComponent sender, Vector2 position)
        {
            if ((!TravelShip) && disableShipTravel)
            {
                ForceNonShipTravel();
                return;
            }
            base.TransportModeButtonOnClickHandler(sender, position);
        }

        //the following method is overridden to handle the "disable ship travel outside of docks" setting
        public override void ToggleTransportModeButtonOnScrollHandler(BaseScreenComponent sender)
        {
            if ((!TravelShip) && disableShipTravel)
            {
                ForceNonShipTravel();
                return;
            }
            base.ToggleTransportModeButtonOnScrollHandler(sender);
        }

        //the following method is overridden to handle the "disable ship travel outside of docks" setting
        public override void SleepModeButtonOnClickHandler(BaseScreenComponent sender, Vector2 position)
        {
            if (sender == campOutToggleButton && disableShipTravel)
                    TravelShip = false;
            base.SleepModeButtonOnClickHandler(sender, position);
        }

        //the following method is overridden to handle the "disable ship travel outside of docks" setting
        public override void ToggleSleepModeButtonOnScrollHandler(BaseScreenComponent sender)
        {
            if (sender == campOutToggleButton && disableShipTravel)
                TravelShip = false;
            base.ToggleSleepModeButtonOnScrollHandler(sender);
        }
    }
}

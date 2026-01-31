/* SeafarersPopUp.cs
 * Travel popup window used when travelling with a ship captain.
 * Overrides cost formulas to account for ship travel fees according
 * to the mod settings. Also prevents the player from unchecking the
 * trasport mode: ship button.
 */

using UnityEngine;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallConnect.Utility;

namespace ImmersiveTravel
{
    public class SeafarersPopUp : DaggerfallTravelPopUp
    {
        public SeafarersPopUp(IUserInterfaceManager uiManager, IUserInterfaceWindow previousWindow = null, DaggerfallTravelMapWindow travelWindow = null) : base(uiManager, previousWindow, travelWindow)
        {
            travelTimeCalculator = new SeafarersCalculator();
        }

        public void SetEndPosition(DFPosition pos)
        {
            this.EndPos = pos;
            Debug.Log("ImmersiveTravelPopUp: Destination EndPos set to: " + pos);
        }

        //enables ship travel and pushes an error message to the screen
        public void ForceShipTravel(){
            TravelShip = true;
            DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
            messageBox.SetText("Cannot disable ship travel when travelling with a ship captain.");
            Button okButton = messageBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.OK, true);
            
            messageBox.OnButtonClick += (_sender, button) =>
            {
                CloseWindow();  //Close the popup when OK is clicked
            };
            
            //Push the message box so it displays immediately.
            uiManager.PushWindow(messageBox);
        }

        //disables inns aand pushes an error message to the screen
        public void ForceCampOut()
        {
            SleepModeInn = false;
            DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
            messageBox.SetText("There are no inns in the middle of the sea.");
            Button okButton = messageBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.OK, true);
            
            messageBox.OnButtonClick += (_sender, button) =>
            {
                CloseWindow();  //Close the popup when OK is clicked
            };
            
            //Push the message box so it displays immediately.
            uiManager.PushWindow(messageBox);
        }

        //the following method is overridden to ensure that ship travel is always selected
        public override void OnPush()
        {
            base.OnPush();
            TravelShip = true;
            SleepModeInn = false;
            if (IsSetup)
            {
                Refresh();
            }
        }

        //the following method is overridden to ensure that ship travel is always selected
        public override void TransportModeButtonOnClickHandler(BaseScreenComponent sender, Vector2 position)
        {
            if (TravelShip)
            {
                ForceShipTravel();
                return;
            }
            base.TransportModeButtonOnClickHandler(sender, position);
        }

        //the following method is overridden to ensure that ship travel is always selected
        public override void ToggleTransportModeButtonOnScrollHandler(BaseScreenComponent sender)
        {
            if (TravelShip)
            {
                ForceShipTravel();
                return;
            }
            base.ToggleTransportModeButtonOnScrollHandler(sender);
        }

        //the following method is overridden to ensure that camp out is always selected
        public override void SleepModeButtonOnClickHandler(BaseScreenComponent sender, Vector2 position)
        {
            if (!SleepModeInn)
            {
                ForceCampOut();
                return;
            }
            base.SleepModeButtonOnClickHandler(sender, position);
        }

        //the following method is overridden to ensure that camp out is always selected
        public override void ToggleSleepModeButtonOnScrollHandler(BaseScreenComponent sender)
        {
            if (!SleepModeInn)
            {
                ForceCampOut();
                return;
            }
            base.ToggleSleepModeButtonOnScrollHandler(sender);
        }
    }
}
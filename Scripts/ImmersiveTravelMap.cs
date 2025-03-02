//this class overrides the vanilla travel map with a new version that prevents the player from fast traveling from anywhere

using UnityEngine;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallConnect;
using DaggerfallConnect.Utility;
using DaggerfallConnect.Arena2;
using System;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Game.UserInterface;
using Wenzil.Console;
using Wenzil.Console.Commands;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Utility.AssetInjection;
using DaggerfallWorkshop.Game.Questing;
using TravelOptions;

namespace ImmersiveTravel{
    public class ImmersiveTravelMap : DaggerfallTravelMapWindow
    {
        public ImmersiveTravelMap(IUserInterfaceManager uiManager) : base(uiManager){}

        protected override void CreateConfirmationPopUp(){}   //prevents the player from travelling to a location after searching it

        protected override void CreatePopUpWindow()
        {
            DFPosition pos = MapsFile.GetPixelFromPixelID(locationSummary.ID);
            if (teleportationTravel)    //the popup from the mage's guild teleportation service hasn't changed.
            {
                DaggerfallTeleportPopUp telePopup = (DaggerfallTeleportPopUp)UIWindowFactory.GetInstanceWithArgs(UIWindowType.TeleportPopUp, new object[] { uiManager, uiManager.TopWindow, this });
                telePopup.DestinationPos = pos;
                telePopup.DestinationName = GetLocationNameInCurrentRegion(locationSummary.MapIndex);
                uiManager.PushWindow(telePopup);
            }
            else    //the one from manual travel has been changed to an error message.
            {
                DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
                messageBox.SetText("You must talk to a carriage driver to travel there.");
                uiManager.PushWindow(messageBox);
            }
        }
    }
}
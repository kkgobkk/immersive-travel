//Implements a modified version of the travel map that is opened when talking to carriages drivers

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
using ImmersiveTravel;

namespace DaggerfallWorkshop.Game.UserInterfaceWindows{
    public class CarriageMap : DaggerfallTravelMapWindow
    {
        public CarriageMap(IUserInterfaceManager uiManager) : base(uiManager){}
        
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
            else    //the one from manual travel has been changed to only enable travelling to cities, towns and hamlets
            {
                DFRegion.LocationTypes locType = locationSummary.LocationType;
                if(locType == DFRegion.LocationTypes.TownCity || locType == DFRegion.LocationTypes.TownVillage || locType == DFRegion.LocationTypes.TownHamlet){
                    ImmersiveTravelPopUp popUp = (ImmersiveTravelPopUp)UIWindowFactory.GetInstanceWithArgs(UIWindowType.TravelPopUp, new object[] { uiManager, uiManager.TopWindow, this });
                    popUp.setEndPosPlease(pos);
                    uiManager.PushWindow(popUp);
                }
                else{
                    DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
                    messageBox.SetText("Carriage drivers will only take you to towns.");
                    uiManager.PushWindow(messageBox);
                }
            }
        }
    }
}
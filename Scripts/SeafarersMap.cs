//Implements a modified version of the travel map that is opened when talking to seafarer league sailors

using System;
using System.Collections.Generic;
using UnityEngine;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallConnect;
using DaggerfallConnect.Utility;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Game.UserInterface;
using Wenzil.Console;
using Wenzil.Console.Commands;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Utility.AssetInjection;
using DaggerfallWorkshop.Game.Questing;
using DaggerfallWorkshop;

namespace ImmersiveTravel{
    public class SeafarersMap : CarriageMap
    {
        public SeafarersMap(IUserInterfaceManager uiManager) : base(uiManager){}

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
                    if(HasDock(locType)){
                        SeafarersPopUp popUp = (SeafarersPopUp)UIWindowFactory.GetInstanceWithArgs(UIWindowType.TravelPopUp, new object[] { uiManager, uiManager.TopWindow, this });
                        popUp.SetEndPosPlease(pos);
                        uiManager.PushWindow(popUp);
                    }
                    else{
                        DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
                        messageBox.SetText("This location doesn't have a port.");
                        Button okButton = messageBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.OK, true);
                        messageBox.OnButtonClick += (_sender, button) =>{CloseWindow();};
                        uiManager.PushWindow(messageBox);
                    }
                }
        }
        
        private bool HasDock(DFRegion.LocationTypes type){
            return type == DFRegion.LocationTypes.TownVillage;
        }
    }
}
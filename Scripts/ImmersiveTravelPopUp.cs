//overrides the travel popup window, changing the cost formula. See also: ImmersiveTravelCalculator.cs

using System;
using UnityEngine;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallConnect.Utility;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Utility;
using DaggerfallConnect;


namespace ImmersiveTravel{
    public class ImmersiveTravelPopUp : DaggerfallTravelPopUp
    {
        public ImmersiveTravelPopUp(IUserInterfaceManager uiManager, IUserInterfaceWindow previousWindow = null, DaggerfallTravelMapWindow travelWindow = null)
            : base(uiManager, previousWindow, travelWindow)
        {
            travelTimeCalculator = new ImmersiveTravelCalculator();
        }
        public void setEndPosPlease(DFPosition pos){
            this.EndPos = pos;
        }
    }
}

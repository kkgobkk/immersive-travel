/* SeafarersMap.cs
 * Modified version of the vanilla TravelMapWindow. Used
 * only when travelling with ship captains.
 */

using UnityEngine;
using System;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallConnect;
using DaggerfallConnect.Utility;
using DaggerfallConnect.Arena2;

namespace ImmersiveTravel{
    public class SeafarersMap : CarriageMap
    {
        protected static bool showLargerDocks = ImmersiveTravel.Settings.GetValue<bool>("ShipTravel", "ShowLargerDocks");
        protected static bool showOnlyDocks = ImmersiveTravel.Settings.GetValue<bool>("ShipTravel", "ShowOnlyDocks");
        protected static bool reducedRangeInVillages = ImmersiveTravel.Settings.GetValue<bool>("ShipTravel", "LimitedRangeInSmallDocks");

        public SeafarersMap(IUserInterfaceManager uiManager) : base(uiManager){
            Debug.LogFormat("current player mapID: {0}", GameManager.Instance.PlayerGPS.CurrentMapID);
        }

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
                else
                {
                    if (NearDock(locationSummary.MapID))
                    {
                        //if the reducedRangeInVillages option is enabled and the current location isn't a city, only allow travel to locations in the same region
                        PlayerGPS playerGPS = GameManager.Instance.PlayerGPS;
                        int borderingRegionIndex = BorderingRegionIndex(playerGPS.CurrentMapPixel.X, playerGPS.CurrentMapPixel.Y);

                        if((!reducedRangeInVillages) || IsPlayerInTown(playerGPS) || borderingRegionIndex == locationSummary.RegionIndex || borderingRegionIndex == -1){
                            SeafarersPopUp popUp = new SeafarersPopUp(uiManager, uiManager.TopWindow, this);
                            popUp.SetEndPosition(pos);
                            uiManager.PushWindow(popUp);
                        }
                        else{
                            DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
                            messageBox.SetText("This small boat will only take you to locations in the same region. Travel to a major port to find a larger ship.");
                            Button okButton = messageBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.OK, true);
                            messageBox.OnButtonClick += (_sender, button) =>{CloseWindow();};
                            uiManager.PushWindow(messageBox);
                        }
                    }
                    else
                    {
                        DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
                        messageBox.SetText("That location doesn't have a suitable dock.");
                        Button okButton = messageBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.OK, true);
                        messageBox.OnButtonClick += (_sender, button) =>{CloseWindow();};
                        uiManager.PushWindow(messageBox);
                    }
                }
        }

        protected override bool checkLocationDiscovered(ContentReader.MapSummary summary)
        {
            bool tmp;
            if (ImmersiveTravel.HiddenMapLocationsEnabled)
                tmp = discoveredMapSummaries.Contains(summary) || revealedLocationTypes.Contains(summary.LocationType);
            else   
                tmp = base.checkLocationDiscovered(summary);
            if(showOnlyDocks)
                tmp = tmp && NearDock(summary.ID);
            return tmp;
        }

        protected override bool IsLocationLarge(ContentReader.MapSummary summary)
        {
            if(showLargerDocks)
                return NearDock(summary.MapID);
            else
                return base.IsLocationLarge(summary);
        }
        
        private static bool HasDock(int mapID)
        {
            mapID = mapID & 0x000FFFFF;
            return Array.Exists(dockMapIDs, element => element == mapID);
        }

        private static bool NearDock(int mapID)
        {
            mapID = mapID & 0x000FFFFF;
            return HasDock(mapID) || HasDock(mapID - 1000) || HasDock(mapID + 1) || HasDock(mapID + 1000) || HasDock(mapID -1);
        }

        private static bool IsPlayerInTown(PlayerGPS gps)
        {
            return gps.CurrentLocationType == DFRegion.LocationTypes.TownCity || gps.CurrentLocationType == DFRegion.LocationTypes.TownHamlet 
            || Array.Exists(hardcodedLargeDocksIDs, element => (element & 0x000FFFFF) == (gps.CurrentMapID & 0x000FFFFF))    //these hardcoded large ports ensure you can always get out of island regions and that Port of Daggerfall is considered large
            || gps.CurrentRegionIndex == 31;    //this means the dock is in a sea map cell, which only happens in large towns
        }

        //please don't look past this point...
        protected static int[] hardcodedLargeDocksIDs =
        {
            1306739903, //Port of Daggerfall
            1303690, //Kirkbeth Hamlet in Betony
            20115597, //Singbrugh in Balfiera
            5405511, //Warwych in Balfiera
            16955879, //Ruins of Mastersley Grange in Balfiera
            11700980, //Gallomarket in Balfiera
            150625, //Old Lysausa's farm in Balfiera
            24312929, //Old Yeomhart's Graveyard in Balfiera
            263832, //Crimson Cat Inn in Satakalaam
            343439, //Ruins of Cosh Hall in Cybiades
            192035017, //Ipsmoth in Bhoriane
            198333451, //Old Vannabyth's farm in Bhoriane
            124928606, //Citadel of Heartham in Tulune
            276583, //Inner Altar of Dibella in Tigonus
            6570357, //Zagoparia in Mournoth
        };

        protected static int[] dockMapIDs = 
        {
            5222,
            5224,
            5229,
            8219,
            11215,
            14210,
            22763,
            23240,
            29347,
            34162,
            35152,
            35449,
            38152,
            40361,
            41483,
            42127,
            45124,
            49219,
            56637,
            69406,
            73079,
            75104,
            77077,
            83137,
            83668,
            86218,
            86334,
            86496,
            89333,
            91170,
            93168,
            96150,
            96212,
            100086,
            106061,
            107137,
            109167,
            111631,
            111645,
            112652,
            112707,
            113637,
            113646,
            113649,
            115622,
            118573,
            120375,
            121067,
            121377,
            121473,
            123073,
            125675,
            129449,
            133701,
            133718,
            134553,
            134711,
            135713,
            135717,
            135735,
            137544,
            137558,
            137561,
            137593,
            138583,
            138745,
            139547,
            139588,
            140758,
            140760,
            143535,
            143542,
            144059,
            145609,
            146607,
            147614,
            148062,
            148460,
            148463,
            148589,
            148782,
            149513,
            150459,
            150625,
            151591,
            151788,
            152587,
            157092,
            157795,
            158499,
            158629,
            160305,
            162098,
            162631,
            162646,
            164072,
            164648,
            166644,
            168357,
            168652,
            169640,
            170654,
            172455,
            175401,
            178663,
            181800,
            182685,
            183401,
            184263,
            187406,
            187807,
            188727,
            189097,
            192248,
            192361,
            192653,
            193358,
            193366,
            193793,
            194242,
            194279,
            194855,
            195353,
            195361,
            195681,
            195860,
            196245,
            197366,
            199102,
            199111,
            200356,
            201118,
            201125,
            201654,
            202333,
            202646,
            203671,
            205676,
            210132,
            210785,
            212138,
            214207,
            214209,
            214210,
            214211,
            214285, //Ripmore in Daggerfall (actual location is 213285)
            214294,
            214297,
            215821,
            216791,
            217198,
            217966,
            221160,
            223207,
            223828,
            224178,
            225169,
            225685,
            225840,
            226205,
            228209,
            229847,
            234712,
            234726,
            235146,
            236143,
            236854,
            239139,
            239144,
            239146,
            240841,
            241140,
            242856,
            243846,
            245859,
            247836,
            249839,
            249861,
            249866,
            250875,
            255114,
            255876,
            255884,
            256887,
            256900,
            257889,
            258892,
            258907,
            261923,
            261925,
            262907,
            262931,
            263119,
            263832,
            264825,
            264900,
            264902,
            264940,
            264942,
            265956,
            266964,
            269847,
            269849,
            273975,
            276583,
            276835,
            277751,
            277798,
            278764,
            278817,
            278843,
            278901,
            278962,
            279596,
            279644,
            279697,
            279749,
            279754,
            279766,
            279815,
            280614,
            280747,
            281656,
            281658,
            281663,
            281699,
            281702,
            281704,
            281741,
            281770,
            281872,
            281969,
            282712,
            282724,
            282728,
            282731,
            282734,
            282737,
            283687,
            283707,
            283779,
            283782,
            284685,
            285597,
            285682,
            285898,
            286674,
            287827,
            287829,
            289737,
            289866,
            290582,
            292695,
            293697,
            294569,
            294842,
            295564,
            296558,
            297552,
            302839,
            305534,
            306854,
            308524,
            308527,
            308530,
            309521,
            310763,
            311766,
            312518,
            313516,
            316550,
            318514,
            324981,
            325404,
            325406,
            327404,
            328409,
            334515,
            337704,
            337914,
            338912,
            339496,
            341392,
            341496,
            341916,
            342397, //Sentinel (actual location is 343397)
            342399,
            343439,
            344387,
            345378,
            345383,
            346398,
            346475,
            346918,
            347375,
            348372,
            348396,
            350370,
            351392,
            351470,
            351919,
            352369,
            353387,
            354364,
            354916,
            357347,
            357915,
            357918,
            358355,
            359347,
            361382,
            361913,
            363915,
            364381,
            364449,
            364868,
            369385,
            369388,
            370441,
            370446,
            370908,
            372411,
            372439,
            373407,
            373415,
            373422,
            373425,
            373427,
            373429,
            374419,
            376329,
            379876,
            379888,
            380885,
            381881,
            382879,
            393300,
            397296,
            402451,
            403279,
            405246,
            405263,
            406266,
            406276,
            407241,
            408235,
            408249,
            417227,
            418291,
            422217,
            432218,
            433205,
            435202,
            437653,
            443401,
            446471,
            451180,
            451186,
            453173,
            455174,
            455199,
            457179,
            458198,
            459220,
            460176,
            461173,
            463171,
            468168,
            468188,
            469891,
            472431,
            473169,
            474207,
            476162,
            476164,
            477177,
            478159,
            480415,
            483153,
            485856,
            493144,
            495141,
        };
    }
}
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
using DaggerfallWorkshop;

namespace ImmersiveTravel{
    public class CarriageMap : DaggerfallTravelMapWindow
    {
        const int path_roads = 0;
        const int path_tracks = 1;
        const int path_rivers = 2;
        const int path_streams = 3;
        internal static byte[][] pathsData = new byte[4][];
        protected bool[] showPaths = { true, false, false, false };

        public static Color32 roadColor = new Color32(60, 60, 60, 255);
        public static Color32 trackColor = new Color32(160, 118, 74, 255);
        static Color32 riverColor = new Color32(48, 79, 250, 255);
        static Color32 streamColor = new Color32(48, 120, 230, 255);

        protected int markedLocationId = -1;
        Color32 MarkLocationColor = new Color32(255, 235, 5, 255);

        private static bool DrawRoads = ImmersiveTravel.BasicRoadsEnabled && ImmersiveTravel.mod.GetSettings().GetValue<bool>("General", "DrawRoads");

        public CarriageMap(IUserInterfaceManager uiManager) : base(uiManager){
            if (DrawRoads)
            {
                // Try to get path data from BasicRoads mod
                ModManager.Instance.SendModMessage("BasicRoads", "getPathData", path_roads,
                    (string message, object data) => { pathsData[path_roads] = (byte[])data; });
            }
        }

        protected override void Setup()
        {
            base.Setup();
            
            if (DrawRoads){
                locationDotsPixelBuffer = new Color32[(int)regionTextureOverlayPanelRect.width * (int)regionTextureOverlayPanelRect.height * 25];
                locationDotsTexture = new Texture2D((int)regionTextureOverlayPanelRect.width * 5, (int)regionTextureOverlayPanelRect.height * 5, TextureFormat.ARGB32, false);
            }
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
            else    //the one from manual travel has been changed to only enable travelling to cities, towns and hamlets
            {
                DFRegion.LocationTypes locType = locationSummary.LocationType;
                if(locType == DFRegion.LocationTypes.TownCity || locType == DFRegion.LocationTypes.TownVillage || locType == DFRegion.LocationTypes.TownHamlet){
                    ImmersiveTravelPopUp popUp = (ImmersiveTravelPopUp)UIWindowFactory.GetInstanceWithArgs(UIWindowType.TravelPopUp, new object[] { uiManager, uiManager.TopWindow, this });
                    popUp.SetEndPosPlease(pos);
                    uiManager.PushWindow(popUp);
                }
                else{
                    DaggerfallMessageBox messageBox = new DaggerfallMessageBox(uiManager, this);
                    messageBox.SetText("Carriage drivers will only take you to towns.");
                    Button okButton = messageBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.OK, true);
                    messageBox.OnButtonClick += (_sender, button) =>{CloseWindow();};
                    uiManager.PushWindow(messageBox);
                }
            }
        }

        protected override void UpdateMapLocationDotsTexture()
        {
            if (DrawRoads && selectedRegion != 61)
            {
                UpdateMapLocationDotsTextureWithPaths();
            }
            else
            {
                base.UpdateMapLocationDotsTexture();
            }
        }

        protected override void ZoomMapTextures()
        {
            base.ZoomMapTextures();

            if (DrawRoads && RegionSelected && zoom)
            {
                // Adjust cropped location dots overlay to x5 version
                int width = (int)regionTextureOverlayPanelRect.width;
                int height = (int)regionTextureOverlayPanelRect.height;
                int zoomWidth = width / (zoomfactor * 2);
                int zoomHeight = height / (zoomfactor * 2);
                int startX = (int)zoomPosition.x - zoomWidth;
                int startY = (int)(height + (-zoomPosition.y - zoomHeight)) + regionPanelOffset;
                // Clamp to edges
                if (startX < 0)
                    startX = 0;
                else if (startX + width / zoomfactor >= width)
                    startX = width - width / zoomfactor;
                if (startY < 0)
                    startY = 0;
                else if (startY + height / zoomfactor >= height)
                    startY = height - height / zoomfactor;

                Rect locationDotsNewRect = new Rect(startX * 5, startY * 5, width * 5 / zoomfactor, height * 5 / zoomfactor);
                regionLocationDotsOverlayPanel.BackgroundCroppedRect = locationDotsNewRect;

                UpdateBorder();
            }
        }

        protected DFPosition GetClickMPCoords()
        {
            float scale = GetRegionMapScale(selectedRegion);
            Vector2 coordinates = GetCoordinates();
            int x = (int)(coordinates.x / scale);
            int y = (int)(coordinates.y / scale);

            if (selectedRegion == betonyIndex)      // Manually correct Betony offset
            {
                x += 60;
                y += 212;
            }
            if (selectedRegion == 61)               // Fix for Cybiades zoom-in map. Map is more zoomed in than for other regions but the pixel coordinates are not scaled to match.
            {
                int xDiff = x - 440;
                int yDiff = y - 340;
                xDiff /= 4;
                yDiff /= 4;
                x = 440 + xDiff;
                y = 340 + yDiff;
            }
            return new DFPosition(x, y);
        }

        public void DrawMapSection(int originX, int originY, int width, int height, ref Color32[] pixelBuffer, bool circular = false)
        {
            Array.Clear(pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < height; y++)
            {
                int mpY = originY + y;
                if (mpY < 0 || mpY >= MapsFile.MaxMapPixelY)
                    continue;

                for (int x = 0; x < width; x++)
                {
                    int mpX = originX + x;
                    if (mpX < 0 || mpX >= MapsFile.MaxMapPixelX)
                        continue;

                    if (circular && height == width && Mathf.Sqrt(Mathf.Pow(Mathf.Abs(x - (width / 2) + 0.5f), 2) + Mathf.Pow(Mathf.Abs(y - (height / 2) + 0.5f), 2)) >= (height + 1.5) / 2)
                        continue;

                    int offset = ((height - y - 1) * width) + x;
                    if (offset >= (width * height))
                        continue;
                    int width5 = width * 5;
                    int offset5 = ((height - y - 1) * 5 * width5) + (x * 5);

                    int pIdx = mpX + (mpY * MapsFile.MaxMapPixelX);
                    //Debug.LogFormat("Checking paths at x:{0} y:{1}  index:{2}", mpX, mpY, pIdx);
                    if (showPaths[path_tracks])
                        DrawPath(offset5, width5, pathsData[path_tracks][pIdx], trackColor, ref pixelBuffer);
                    if (showPaths[path_roads])
                        DrawPath(offset5, width5, pathsData[path_roads][pIdx], roadColor, ref pixelBuffer);

                    ContentReader.MapSummary summary;
                    if (DaggerfallUnity.Instance.ContentReader.HasLocation(mpX, mpY, out summary))
                    {
                        if (checkLocationDiscovered(summary))
                        {
                            int index = GetPixelColorIndex(summary.LocationType);
                            if (index != -1)
                            {
                                DrawLocation(offset5, width5, locationPixelColors[index], IsLocationLarge(summary.LocationType), ref pixelBuffer, summary.ID == markedLocationId);
                            }
                        }
                    }
                }
            }
        }

        protected virtual void UpdateMapLocationDotsTextureWithPaths()
        {
            // Get map and dimensions
            string mapName = selectedRegionMapNames[mapIndex];
            Vector2 origin = offsetLookup[mapName];
            int originX = (int)origin.x;
            int originY = (int)origin.y;
            int width = (int)regionTextureOverlayPanelRect.width;
            int height = (int)regionTextureOverlayPanelRect.height;

            // Plot locations to color array
            scale = GetRegionMapScale(selectedRegion);
            Array.Clear(locationDotsPixelBuffer, 0, locationDotsPixelBuffer.Length);
            Array.Clear(locationDotsOutlinePixelBuffer, 0, locationDotsOutlinePixelBuffer.Length);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int offset = (int)((((height - y - 1) * width) + x) * scale);
                    if (offset >= (width * height))
                        continue;
                    int sampleRegion = DaggerfallUnity.ContentReader.MapFileReader.GetPoliticIndex(originX + x, originY + y) - 128;

                    int width5 = width * 5;
                    int offset5 = (int)((((height - y - 1) * 5 * width5) + (x * 5)) * scale);

                    int pIdx = originX + x + ((originY + y) * MapsFile.MaxMapPixelX);
                    /*
                    if (showPaths[path_streams])
                        DrawPath(offset5, width5, pathsData[path_streams][pIdx], streamColor, ref locationDotsPixelBuffer);
                    if (showPaths[path_tracks])
                        DrawPath(offset5, width5, pathsData[path_tracks][pIdx], trackColor, ref locationDotsPixelBuffer);
                    if (showPaths[path_rivers])
                        DrawPath(offset5, width5, pathsData[path_rivers][pIdx], riverColor, ref locationDotsPixelBuffer);
                    if (showPaths[path_roads])*/
                        DrawPath(offset5, width5, pathsData[path_roads][pIdx], roadColor, ref locationDotsPixelBuffer);

                    ContentReader.MapSummary summary;
                    if (DaggerfallUnity.ContentReader.HasLocation(originX + x, originY + y, out summary))
                    {
                        if (checkLocationDiscovered(summary))
                        {
                            int index = GetPixelColorIndex(summary.LocationType);
                            if (index != -1)
                            {
                                if (DaggerfallUnity.Settings.TravelMapLocationsOutline)
                                    locationDotsOutlinePixelBuffer[offset] = dotOutlineColor;
                                DrawLocation(offset5, width5, locationPixelColors[index], IsLocationLarge(summary.LocationType), ref locationDotsPixelBuffer, summary.ID == markedLocationId);
                            }
                        }
                    }
                }
            }

            // Apply updated color array to texture
            if (DaggerfallUnity.Settings.TravelMapLocationsOutline)
            {
                locationDotsOutlineTexture.SetPixels32(locationDotsOutlinePixelBuffer);
                locationDotsOutlineTexture.Apply();
            }
            locationDotsTexture.SetPixels32(locationDotsPixelBuffer);
            locationDotsTexture.Apply();

            // Present texture
            if (DaggerfallUnity.Settings.TravelMapLocationsOutline)
                for (int i = 0; i < outlineDisplacements.Length; i++)
                    regionLocationDotsOutlinesOverlayPanel[i].BackgroundTexture = locationDotsOutlineTexture;
            regionLocationDotsOverlayPanel.BackgroundTexture = locationDotsTexture;
        }

        public static void DrawPath(int offset, int width, byte pathDataPt, Color32 pathColor, ref Color32[] pixelBuffer)
        {
            if (pathDataPt == 0)
                return;

            pixelBuffer[offset + (width * 2) + 2] = pathColor;
            if ((pathDataPt & ImmersiveTravel.S) != 0)
            {
                pixelBuffer[offset + 2] = pathColor;
                pixelBuffer[offset + width + 2] = pathColor;
            }
            if ((pathDataPt & ImmersiveTravel.SE) != 0)
            {
                pixelBuffer[offset + 4] = pathColor;
                pixelBuffer[offset + width + 3] = pathColor;
            }
            if ((pathDataPt & ImmersiveTravel.E) != 0)
            {
                pixelBuffer[offset + (width * 2) + 3] = pathColor;
                pixelBuffer[offset + (width * 2) + 4] = pathColor;
            }
            if ((pathDataPt & ImmersiveTravel.NE) != 0)
            {
                pixelBuffer[offset + (width * 3) + 3] = pathColor;
                pixelBuffer[offset + (width * 4) + 4] = pathColor;
            }
            if ((pathDataPt & ImmersiveTravel.N) != 0)
            {
                pixelBuffer[offset + (width * 3) + 2] = pathColor;
                pixelBuffer[offset + (width * 4) + 2] = pathColor;
            }
            if ((pathDataPt & ImmersiveTravel.NW) != 0)
            {
                pixelBuffer[offset + (width * 3) + 1] = pathColor;
                pixelBuffer[offset + (width * 4)] = pathColor;
            }
            if ((pathDataPt & ImmersiveTravel.W) != 0)
            {
                pixelBuffer[offset + (width * 2)] = pathColor;
                pixelBuffer[offset + (width * 2) + 1] = pathColor;
            }
            if ((pathDataPt & ImmersiveTravel.SW) != 0)
            {
                pixelBuffer[offset] = pathColor;
                pixelBuffer[offset + width + 1] = pathColor;
            }
        }

        void DrawLocation(int offset, int width, Color32 color, bool large, ref Color32[] pixelBuffer, bool highlight = false)
        {
            int st = large ? 0 : 1;
            int en = large ? 5 : 4;
            for (int y = st; y < en; y++)
            {
                for (int x = st; x < en; x++)
                {
                    pixelBuffer[offset + (y * width) + x] = color;
                }
            }
            if (highlight)
            {
                for (int y = -2; y < 8; y = y + 8)
                {
                    for (int x = -2; x < 7; x++)
                    {
                        pixelBuffer[offset + (y * width) + x] = MarkLocationColor;
                    }
                }
                for (int x = -2; x < 8; x = x + 8)
                {
                    for (int y = -2; y < 7; y++)
                    {
                        pixelBuffer[offset + (y * width) + x] = MarkLocationColor;
                    }
                }
            }
        }

        bool IsLocationLarge(DFRegion.LocationTypes locationType)
        {
            return locationType == DFRegion.LocationTypes.TownCity || locationType == DFRegion.LocationTypes.TownHamlet || !(ImmersiveTravel.mod.GetSettings().GetValue<bool>("General", "ClearerMapDots"));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stargazer.Assets
{
    public static class QuickAssets
    {
        public static GameObject GetDoorSabotageButton()
        {
            return MapAssets.GetAsset(0).MapPrefab.infectedOverlay.rooms[0].door.gameObject;
        } 

        public static GameObject GetLadderBase()
        {
            return MapAssets.GetAsset(4).transform.FindChild("MeetingRoom").FindChild("ladder_meeting").gameObject;
        }
        public static Ladder GetLadder(bool isTop)
        {
            var ladderBase = GetLadderBase().transform;
            var ladder = ladderBase.FindChild(isTop ? "LadderTop" : "LadderBottom");
            return ladder.gameObject.GetComponent<Ladder>();
        }

        public static Vent GetVent(byte type)
        {
            if (type == 0) return MapAssets.GetAsset(0).transform.GetComponentInChildren<Vent>();
            else if (type == 1) return MapAssets.GetAsset(2).transform.GetComponentInChildren<Vent>();

            return MapAssets.GetAsset(0).transform.GetComponentInChildren<Vent>();
        }

        public static Minigame? GetMinigamePrefab(TaskTypes taskTypes, int arg = 0)
        {
            var prefab = GetMinigameOriginalPrefab(taskTypes,arg);
            if (prefab == null) return null;

            return prefab;
        }

        private static Minigame? GetMinigameOriginalPrefab(TaskTypes taskTypes,int arg = 0)
        {
            switch (taskTypes)
            {
                case TaskTypes.SwipeCard:
                    return MapAssets.GetAsset(0).CommonTasks[0].MinigamePrefab;
                case TaskTypes.FixWiring:
                    return MapAssets.GetAsset(0).CommonTasks[1].MinigamePrefab;
                case TaskTypes.EnterIdCode:
                    return MapAssets.GetAsset(1).CommonTasks[1].MinigamePrefab;
                case TaskTypes.InsertKeys:
                    return MapAssets.GetAsset(2).CommonTasks[1].MinigamePrefab;
                case TaskTypes.ScanBoardingPass:
                    return MapAssets.GetAsset(2).CommonTasks[2].MinigamePrefab;
                case TaskTypes.UploadData:
                    if (arg == 0)
                        return MapAssets.GetAsset(0).NormalTasks[0].MinigamePrefab;
                    else
                        return MapAssets.GetAsset(4).NormalTasks[2].MinigamePrefab;
                case TaskTypes.CalibrateDistributor:
                    return MapAssets.GetAsset(0).NormalTasks[1].MinigamePrefab;
                case TaskTypes.ChartCourse:
                    return MapAssets.GetAsset(0).NormalTasks[2].MinigamePrefab;
                case TaskTypes.CleanO2Filter:
                    return MapAssets.GetAsset(0).NormalTasks[3].MinigamePrefab;
                case TaskTypes.UnlockManifolds:
                    return MapAssets.GetAsset(0).NormalTasks[4].MinigamePrefab;
                case TaskTypes.StabilizeSteering:
                    return MapAssets.GetAsset(0).NormalTasks[6].MinigamePrefab;
                case TaskTypes.PrimeShields:
                    return MapAssets.GetAsset(0).NormalTasks[8].MinigamePrefab;
                case TaskTypes.DivertPower:
                    return MapAssets.GetAsset(0).NormalTasks[11].MinigamePrefab;
                case TaskTypes.VentCleaning:
                    return MapAssets.GetAsset(0).NormalTasks[19].MinigamePrefab;
                case TaskTypes.AssembleArtifact:
                    return MapAssets.GetAsset(1).NormalTasks[3].MinigamePrefab;
                case TaskTypes.SortSamples:
                    return MapAssets.GetAsset(1).NormalTasks[4].MinigamePrefab;
                case TaskTypes.MeasureWeather:
                    return MapAssets.GetAsset(1).NormalTasks[7].MinigamePrefab;
                case TaskTypes.BuyBeverage:
                    return MapAssets.GetAsset(1).NormalTasks[9].MinigamePrefab;
                case TaskTypes.ProcessData:
                    return MapAssets.GetAsset(1).NormalTasks[10].MinigamePrefab;
                case TaskTypes.RunDiagnostics:
                    return MapAssets.GetAsset(1).NormalTasks[11].MinigamePrefab;
                case TaskTypes.MonitorOxygen:
                    return MapAssets.GetAsset(2).NormalTasks[0].MinigamePrefab;
                case TaskTypes.StoreArtifacts:
                    return MapAssets.GetAsset(2).NormalTasks[2].MinigamePrefab;
                case TaskTypes.FillCanisters:
                    return MapAssets.GetAsset(2).NormalTasks[3].MinigamePrefab;
                case TaskTypes.ActivateWeatherNodes:
                    if(arg==1)
                        return MapAssets.GetAsset(2).NormalTasks[8].Cast<WeatherNodeTask>().Stage2Prefab;
                    else
                        return MapAssets.GetAsset(2).NormalTasks[8].MinigamePrefab;
                case TaskTypes.AlignTelescope:
                    return MapAssets.GetAsset(2).NormalTasks[10].MinigamePrefab;
                case TaskTypes.RepairDrill:
                    return MapAssets.GetAsset(2).NormalTasks[11].MinigamePrefab;
                case TaskTypes.RecordTemperature:
                    return MapAssets.GetAsset(2).NormalTasks[12].MinigamePrefab;
                case TaskTypes.PolishRuby:
                    return MapAssets.GetAsset(4).NormalTasks[0].MinigamePrefab;
                case TaskTypes.PickUpTowels:
                    return MapAssets.GetAsset(4).NormalTasks[14].MinigamePrefab;
                case TaskTypes.CleanToilet:
                    return MapAssets.GetAsset(4).NormalTasks[15].MinigamePrefab;
                case TaskTypes.DressMannequin:
                    return MapAssets.GetAsset(4).NormalTasks[16].MinigamePrefab;
                case TaskTypes.SortRecords:
                    return MapAssets.GetAsset(4).NormalTasks[17].MinigamePrefab;
                case TaskTypes.PutAwayPistols:
                    return MapAssets.GetAsset(4).NormalTasks[18].MinigamePrefab;
                case TaskTypes.PutAwayRifles:
                    return MapAssets.GetAsset(4).NormalTasks[19].MinigamePrefab;
                case TaskTypes.Decontaminate:
                    return MapAssets.GetAsset(4).NormalTasks[20].MinigamePrefab;
                case TaskTypes.MakeBurger:
                    return MapAssets.GetAsset(4).NormalTasks[21].MinigamePrefab;
                case TaskTypes.FixShower:
                    return MapAssets.GetAsset(4).NormalTasks[22].MinigamePrefab;
                case TaskTypes.ClearAsteroids:
                    return MapAssets.GetAsset(0).LongTasks[0].MinigamePrefab;
                case TaskTypes.AlignEngineOutput:
                    return MapAssets.GetAsset(0).LongTasks[1].MinigamePrefab;
                case TaskTypes.SubmitScan:
                    return MapAssets.GetAsset(0).LongTasks[2].MinigamePrefab;
                case TaskTypes.InspectSample:
                    return MapAssets.GetAsset(0).LongTasks[3].MinigamePrefab;
                case TaskTypes.FuelEngines:
                    return MapAssets.GetAsset(0).LongTasks[4].MinigamePrefab;
                case TaskTypes.StartReactor:
                    return MapAssets.GetAsset(0).LongTasks[5].MinigamePrefab;
                case TaskTypes.EmptyChute:
                    return MapAssets.GetAsset(0).LongTasks[6].MinigamePrefab;
                case TaskTypes.EmptyGarbage:
                    if(arg==0)
                        return MapAssets.GetAsset(0).LongTasks[7].MinigamePrefab;
                    else
                        return MapAssets.GetAsset(4).LongTasks[10].MinigamePrefab;
                case TaskTypes.WaterPlants:
                    return MapAssets.GetAsset(1).LongTasks[8].MinigamePrefab;
                case TaskTypes.OpenWaterways:
                    return MapAssets.GetAsset(2).LongTasks[7].MinigamePrefab;
                case TaskTypes.ReplaceWaterJug:
                    return MapAssets.GetAsset(2).LongTasks[9].MinigamePrefab;
                case TaskTypes.RebootWifi:
                    return MapAssets.GetAsset(2).LongTasks[14].MinigamePrefab;
                case TaskTypes.ResetBreakers:
                    return MapAssets.GetAsset(4).LongTasks[1].MinigamePrefab;
                case TaskTypes.UnlockSafe:
                    return MapAssets.GetAsset(4).LongTasks[7].MinigamePrefab;
                case TaskTypes.StartFans:
                    return MapAssets.GetAsset(4).LongTasks[8].MinigamePrefab;
                case TaskTypes.DevelopPhotos:
                    return MapAssets.GetAsset(4).LongTasks[12].MinigamePrefab;
                case TaskTypes.RewindTapes:
                    return MapAssets.GetAsset(4).LongTasks[14].MinigamePrefab;
            }
            return null;
        }
    }
}

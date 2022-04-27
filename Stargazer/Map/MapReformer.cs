using UnityEngine;

namespace Stargazer.Map
{
    public static class MapReformer
    {
        static public void Dismantle(ShipStatus shipStatus)
        {
            // Delete Children
            Transform shipTransform = shipStatus.gameObject.gameObject.transform;
            for (int i = shipTransform.childCount - 1; i >= 0; i--)
            {
                Transform child = shipTransform.GetChild(i);
                GameObject.Destroy(child.gameObject);
            }

            // Ship Status
            shipStatus.AllCameras = new UnhollowerBaseLib.Il2CppReferenceArray<SurvCamera>(0);
            shipStatus.AllDoors = new UnhollowerBaseLib.Il2CppReferenceArray<PlainDoor>(0);
            shipStatus.AllConsoles = new UnhollowerBaseLib.Il2CppReferenceArray<Console>(0);
            shipStatus.AllRooms = new UnhollowerBaseLib.Il2CppReferenceArray<PlainShipRoom>(0);
            shipStatus.AllStepWatchers = new UnhollowerBaseLib.Il2CppReferenceArray<IStepWatcher>(0);
            shipStatus.AllVents = new UnhollowerBaseLib.Il2CppReferenceArray<Vent>(0);
            shipStatus.DummyLocations = new UnhollowerBaseLib.Il2CppReferenceArray<Transform>(0);
            shipStatus.SpecialTasks = new UnhollowerBaseLib.Il2CppReferenceArray<PlayerTask>(0);
            shipStatus.CommonTasks = new UnhollowerBaseLib.Il2CppReferenceArray<NormalPlayerTask>(0);
            shipStatus.LongTasks = new UnhollowerBaseLib.Il2CppReferenceArray<NormalPlayerTask>(0);
            shipStatus.NormalTasks = new UnhollowerBaseLib.Il2CppReferenceArray<NormalPlayerTask>(0);
            shipStatus.FastRooms = new Il2CppSystem.Collections.Generic.Dictionary<SystemTypes, PlainShipRoom>();
            shipStatus.SystemNames = new UnhollowerBaseLib.Il2CppStructArray<StringNames>(0);
            shipStatus.Systems = new Il2CppSystem.Collections.Generic.Dictionary<SystemTypes, ISystemType>();
        }

        static public void CreateStandardMap(ShipStatus shipStatus)
        {
            // Spawn
            shipStatus.InitialSpawnCenter = new Vector2(0, 0);
            shipStatus.MeetingSpawnCenter = new Vector2(0, 0);
            shipStatus.MeetingSpawnCenter2 = new Vector2(0, 0);

            Camera main = Camera.main;
            main.backgroundColor = shipStatus.CameraColor;
            FollowerCamera component = main.GetComponent<FollowerCamera>();
            DestroyableSingleton<HudManager>.Instance.ShadowQuad.material.SetInt("_Mask", 7);
            if (component)
            {
                component.shakeAmount = 0f;
                component.shakePeriod = 0f;
            }
        }

        static public void CreateDefaultSystemTypes(ShipStatus shipStatus)
        {
            // Sabotages
            shipStatus.Systems.Add(SystemTypes.Electrical, new SwitchSystem().Cast<ISystemType>());
            shipStatus.Systems.Add(SystemTypes.Comms, new HudOverrideSystemType().Cast<ISystemType>());
            shipStatus.Systems.Add(SystemTypes.Laboratory, new ReactorSystemType(60f, SystemTypes.Laboratory).Cast<ISystemType>());
            shipStatus.Systems.Add(SystemTypes.Doors, new AutoDoorsSystemType().Cast<ISystemType>());
            shipStatus.Systems.Add(SystemTypes.Sabotage, new SabotageSystemType(new IActivatable[] {
                shipStatus.Systems[SystemTypes.Electrical].Cast<IActivatable>(),
                shipStatus.Systems[SystemTypes.Comms].Cast<IActivatable>(),
                shipStatus.Systems[SystemTypes.Laboratory].Cast<IActivatable>(),
                shipStatus.Systems[SystemTypes.Doors].Cast<IActivatable>()
            }).Cast<ISystemType>());

            //
            shipStatus.Systems.Add(SystemTypes.Ventilation, new VentilationSystem().Cast<ISystemType>());

            // Other
            shipStatus.Systems.Add(SystemTypes.Security, new SecurityCameraSystemType().Cast<ISystemType>());
            shipStatus.Systems.Add(SystemTypes.MedBay, new MedScanSystem().Cast<ISystemType>());
        }
    }
}

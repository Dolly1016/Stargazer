using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using Stargazer.Map.Configurations;

namespace Stargazer.Map
{
    public class Blueprint : Builder.CustomHolder
    {
        public byte BaseMapId { get; set; }
        public bool RequirePlainMap { get; set; }

        public byte VentType { get; set; }

        public MinimapConfiguration MinimapConfiguration { get; set; }

        public Dictionary<string, Builder.CustomConsole> Consoles;
        public Dictionary<string, Builder.CustomVent> Vents;

        private HashSet<Database.TaskData> TaskDatabase { get; set; }

        public void RegisterTask(Database.TaskData taskData)
        {
            TaskDatabase.Add(taskData);
        }

        public Material MaskingShader { get; private set; }
        public Material HighlightMaterial { get; private set; }

        private Material GetMaterial(string name)
        {
            foreach (var mat in UnityEngine.Object.FindObjectsOfTypeIncludingAssets(Material.Il2CppType))
            {
                if (mat.name != name) continue;
                return mat.Cast<Material>();
            }
            return Material.GetDefaultMaterial();
        }

        public Blueprint(string name):base(name,Vector2.zero)
        {
            BaseMapId = 0;
            RequirePlainMap = true;
            VentType = 0;
            MinimapConfiguration = new MinimapConfiguration();
            Consoles = new Dictionary<string, Builder.CustomConsole>();
            Vents = new Dictionary<string, Builder.CustomVent>();
            TaskDatabase = new HashSet<Database.TaskData>();
        }

        private void CleanMiniMap(MapBehaviour mapBehaviour)
        {
            //カウンタを削除
            foreach(var area in mapBehaviour.countOverlay.CountAreas)
                UnityEngine.Object.Destroy(area.gameObject);
            mapBehaviour.countOverlay.CountAreas = new UnhollowerBaseLib.Il2CppReferenceArray<CounterArea>(0);

            foreach (var room in mapBehaviour.infectedOverlay.rooms)
                UnityEngine.Object.Destroy(room.gameObject);
            mapBehaviour.infectedOverlay.rooms = new UnhollowerBaseLib.Il2CppReferenceArray<MapRoom>(0);
            mapBehaviour.infectedOverlay.allButtons = new UnhollowerBaseLib.Il2CppReferenceArray<ButtonBehavior>(0);
            

            foreach (var text in mapBehaviour.transform.FindChild("RoomNames").GetComponentsInChildren<TMPro.TextMeshPro>())
                UnityEngine.Object.Destroy(text.gameObject);
        }

        public override void PreBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {
            //初期化
            Consoles.Clear();
            Vents.Clear();

            //shipStatus.MapPrefab = UnityEditor.PrefabUtility.SaveAsPrefabAsset(shipStatus.MapPrefab.gameObject, "Stargazer/Minimap.prefab").GetComponent<MapBehaviour>();
            shipStatus.MapPrefab = GameObject.Instantiate(shipStatus.MapPrefab, HudManager.Instance.transform);
            shipStatus.MapPrefab.Close();

            if (blueprint.RequirePlainMap) CleanMiniMap(shipStatus.MapPrefab);

            //コンポーネントの設定
            Behaviours.CustomShipStatus.Instance = shipStatus.gameObject.AddComponent<Behaviours.CustomShipStatus>();

            MaskingShader = new Material(GetMaterial("MaskingShader"));
            HighlightMaterial = GetMaterial("HighlightMat");

            //マップ周りの設定
            if (!(MinimapConfiguration.MapScale > 0f))
                MinimapConfiguration.MapScale = shipStatus.MapScale;
            if (MinimapConfiguration.CenterPosition == null)
                MinimapConfiguration.CenterPosition = Module.MinimapSpriteGenerator.CalcCenter(shipStatus);
            shipStatus.MapScale = MinimapConfiguration.MapScale;
            shipStatus.MapPrefab.transform.FindChild("HereIndicatorParent").localPosition = (Vector3)(-MinimapConfiguration.CenterPosition / MinimapConfiguration.MapScale);

            GameObject = shipStatus.gameObject;
            GameObject.SetName(GameObject, Name + "Status");
            PreBuildChildren(blueprint,shipStatus);
        }

        public override void PostBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {
            PostBuildChildren(blueprint,shipStatus);

            GameObject manager = new GameObject("TaskManager");
            manager.transform.SetParent(shipStatus.transform);

            foreach (var task in TaskDatabase)
            {
                if (!Builder.Task.TaskBuilder.TaskBuilders.ContainsKey(task.TaskType)) continue;

                GameObject obj = new GameObject();
                obj.transform.SetParent(manager.transform);

                var result = Builder.Task.TaskBuilder.TaskBuilders[task.TaskType].BuildTask(shipStatus,obj, this, task);

                switch (task.TaskCategory)
                {
                    case Database.TaskCategory.CommonTask:
                        shipStatus.CommonTasks = Helpers.AddToReferenceArray(shipStatus.CommonTasks, result);
                        break;
                    case Database.TaskCategory.LongTask:
                        shipStatus.LongTasks = Helpers.AddToReferenceArray(shipStatus.LongTasks, result);
                        break;
                    case Database.TaskCategory.ShortTask:
                        shipStatus.NormalTasks = Helpers.AddToReferenceArray(shipStatus.NormalTasks, result);
                        break;
                }
            }

            UnityEngine.Debug.Log("1");

            //タスクが存在しない場合にゲームが始まらない問題を回避
            if (shipStatus.CommonTasks.Count == 0) shipStatus.CommonTasks = Helpers.AddToReferenceArray(shipStatus.CommonTasks, Builder.Task.TaskBuilder.GenerateDefaultTask());
            if (shipStatus.LongTasks.Count == 0) shipStatus.LongTasks = Helpers.AddToReferenceArray(shipStatus.LongTasks, Builder.Task.TaskBuilder.GenerateDefaultTask());
            if (shipStatus.NormalTasks.Count == 0) shipStatus.NormalTasks = Helpers.AddToReferenceArray(shipStatus.NormalTasks, Builder.Task.TaskBuilder.GenerateDefaultTask());

            UnityEngine.Debug.Log("2");

            //マップの最終設定
            shipStatus.MapPrefab.infectedOverlay.SabSystem = ShipStatus.Instance.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>();
            ISystemType systemType;
            if (ShipStatus.Instance.Systems.TryGetValue(SystemTypes.Doors, out systemType))
            {
                shipStatus.MapPrefab.infectedOverlay.doors = systemType.Cast<IActivatable>();
            }

            UnityEngine.Debug.Log("3");
        }

        public string GetAddressPrefix()
        {
            return "Stargazer/" + Name + "/";
        }
    }
}

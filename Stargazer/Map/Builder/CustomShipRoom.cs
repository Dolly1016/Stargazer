using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stargazer.Map.Builder
{
    public class CustomShipRoom : CustomHolder
    {
        public class RoomOverrayBuilder
        {
            public Vector2 NameOffset { get; set; }
            public Vector2 CounterOffset { get; set; }
            public Vector2 InfectedOffset { get; set; }

            public RoomOverrayBuilder()
            {
                NameOffset = Vector3.zero;
                CounterOffset = Vector3.zero;
                InfectedOffset = Vector3.zero;
            }

            public RoomOverrayBuilder(Vector2 nameOffset, Vector2 counterOffset)
            {
                NameOffset = nameOffset;
                CounterOffset = counterOffset;
                InfectedOffset = Vector3.zero;
            }
        }

        public SpriteRenderer? RoomRenderer { get; private set; }
        public PlainShipRoom? ShipRoom { get; private set; }

        private Vector2[] Edges;
        private SystemTypes RoomId;
        private StringNames RoomStrings;

        public RoomOverrayBuilder? RoomOverray { get; set; }

        public AddressableSprite Sprite { get; set; }

        public CustomShipRoom(string name, SystemTypes room, StringNames stringNames, Vector2 pos) : base(name, pos)
        {
            RoomId = room;
            RoomStrings = stringNames;
            Edges = new Vector2[] { };
            RoomRenderer = null;
            Sprite = new AddressableSprite();

            //ミニマップ関連
            RoomOverray = null;
        }

        public void SetEdge(params Vector2[] edges)
        {
            Edges = edges;
        }

        private Vector2 GetBasePos(Blueprint blueprint)
        {
            return Module.MinimapSpriteGenerator.ConvertToMapPos(ShipRoom.transform.position, blueprint);
        }

        public override void PreBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {
            base.PreBuild(blueprint, shipStatus, parent);

            ShipRoom = GameObject.AddComponent<PlainShipRoom>();
            ShipRoom.RoomId = RoomId;
            var collider = GameObject.AddComponent<PolygonCollider2D>();
            collider.SetPath(0, Edges);
            collider.isTrigger = true;
            ShipRoom.roomArea = collider;
            shipStatus.AllRooms = Helpers.AddToReferenceArray<PlainShipRoom>(shipStatus.AllRooms, ShipRoom);
            shipStatus.FastRooms.set_Item(RoomId,ShipRoom);

            if (RoomOverray != null)
            {
                Vector2 basePos = GetBasePos(blueprint);

                //アドミンのカウンタを追加
                CounterArea counterArea = UnityEngine.Object.Instantiate(Assets.MapAssets.GetAsset(0).MapPrefab.countOverlay.CountAreas[0]);
                GameObject.DontDestroyOnLoad(counterArea.gameObject);
                counterArea.RoomType = RoomId;
                counterArea.name = Name;
                counterArea.transform.SetParent(shipStatus.MapPrefab.countOverlay.transform);
                counterArea.transform.localPosition = (basePos / 0.75f) + RoomOverray.CounterOffset;
                counterArea.pool = shipStatus.MapPrefab.countOverlay.gameObject.GetComponent<ObjectPoolBehavior>();

                shipStatus.MapPrefab.countOverlay.CountAreas = Helpers.AddToReferenceArray(
                    shipStatus.MapPrefab.countOverlay.CountAreas, counterArea
                    );

                //部屋名表示を追加
                var textPrefab = Assets.MapAssets.GetAsset(1).MapPrefab.transform.FindChild("RoomNames").GetChild(0);
                Transform transform = UnityEngine.Object.Instantiate(textPrefab);
                GameObject.DontDestroyOnLoad(transform.gameObject);
                transform.name = Name;
                transform.SetParent(shipStatus.MapPrefab.transform.FindChild("RoomNames").transform);
                transform.localPosition = basePos + new Vector2(0, 0.4f) + RoomOverray.NameOffset;
                var text = transform.gameObject.GetComponent<TextTranslatorTMP>();
                text.TargetText = RoomStrings;
            }

            if (Sprite.GetSprite(blueprint) != null)
            {
                var obj = new GameObject("RoomRenderer");
                obj.transform.SetParent(GameObject.transform);
                obj.transform.localPosition = new Vector3(0, 0, 5f);
                obj.layer = LayerMask.NameToLayer("Ship");
                RoomRenderer = obj.AddComponent<SpriteRenderer>();
                RoomRenderer.sprite = Sprite.GetSprite(blueprint);
                RoomRenderer.material = blueprint.MaskingShader;
            }
        }

        private bool HasDoorSabotage(ShipStatus shipStatus)
        {
            foreach (var door in shipStatus.AllDoors)
                if (door.Room == RoomId) return true;

            return false;
        }

        private bool HasSpecialSabotage(Blueprint blueprint)
        {
            return false;
        }

        private bool HasSabotage(Blueprint blueprint, ShipStatus shipStatus)
        {
            return HasDoorSabotage(shipStatus) || HasSpecialSabotage(blueprint);
        }

        public override void PostBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {
            if (!HasSabotage(blueprint, shipStatus)) return;

            Vector2 basePos = GetBasePos(blueprint);

            MapRoom mapRoom = UnityEngine.Object.Instantiate(Assets.MapAssets.GetAsset(0).MapPrefab.infectedOverlay.rooms[0]);
            GameObject.DontDestroyOnLoad(mapRoom.gameObject);
            mapRoom.Parent = mapRoom._Parent_k__BackingField = shipStatus.MapPrefab.infectedOverlay;
            mapRoom.room = RoomId;
            mapRoom.name = Name;
            mapRoom.transform.SetParent(shipStatus.MapPrefab.infectedOverlay.transform);
            mapRoom.transform.localPosition = (Vector3)(basePos + RoomOverray.InfectedOffset) + new Vector3(0, 0, -1);
            if (mapRoom.door) GameObject.Destroy(mapRoom.door.gameObject);
            if (mapRoom.special) GameObject.Destroy(mapRoom.special.gameObject);
            mapRoom.door = null;
            mapRoom.special = null;
            shipStatus.MapPrefab.infectedOverlay.rooms = Helpers.AddToReferenceArray(shipStatus.MapPrefab.infectedOverlay.rooms, mapRoom);

            bool hasDoorSabotage = HasDoorSabotage(shipStatus);
            bool hasSpecialSabotage = HasSpecialSabotage(blueprint);

            int sabs = 0;
            sabs += hasDoorSabotage ? 1 : 0;
            sabs += hasSpecialSabotage ? 1 : 0;

            if (hasDoorSabotage)
            {
                var doorSab = UnityEngine.Object.Instantiate(Assets.QuickAssets.GetDoorSabotageButton());
                doorSab.transform.SetParent(mapRoom.transform);
                doorSab.GetComponent<ButtonBehavior>().OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                doorSab.GetComponent<ButtonBehavior>().OnClick.AddListener((UnityEngine.Events.UnityAction)(
                    () => mapRoom.SabotageDoors() ));

                doorSab.transform.localPosition = new Vector3(sabs == 2 ? -0.3f : 0, 0, -1);
                shipStatus.MapPrefab.infectedOverlay.allButtons = Helpers.AddToReferenceArray(shipStatus.MapPrefab.infectedOverlay.allButtons, doorSab.GetComponent<ButtonBehavior>());
                mapRoom.door = doorSab.GetComponent<SpriteRenderer>();
                
            }
            if (hasSpecialSabotage)
            {

            }
        }
    }
}

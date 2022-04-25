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

            public RoomOverrayBuilder()
            {
                NameOffset = Vector3.zero;
                CounterOffset = Vector3.zero;
            }

            public RoomOverrayBuilder(Vector2 nameOffset,Vector2 counterOffset)
            {
                NameOffset = nameOffset;
                CounterOffset = counterOffset;
            }
        }

        public SpriteRenderer? RoomRenderer { get; private set; }
        public PlainShipRoom? ShipRoom { get; private set; }

        private Vector2[] Edges;
        private SystemTypes RoomId;
        private StringNames RoomStrings;

        public RoomOverrayBuilder? RoomOverray { get; set; }

        public AddressableSprite Sprite { get; set; }

        public CustomShipRoom(string name,SystemTypes room,StringNames stringNames, Vector2 pos) : base(name,pos)
        {
            RoomId = room;
            RoomStrings=stringNames;
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

        public override void PreBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {
            base.PreBuild(blueprint,shipStatus,parent);

            ShipRoom = GameObject.AddComponent<PlainShipRoom>();
            ShipRoom.RoomId = RoomId;
            var collider = GameObject.AddComponent<PolygonCollider2D>();
            collider.SetPath(0,Edges);
            collider.isTrigger = true;
            ShipRoom.roomArea = collider;
            shipStatus.AllRooms = Helpers.AddToReferenceArray<PlainShipRoom>(shipStatus.AllRooms, ShipRoom);
            shipStatus.FastRooms[RoomId] = ShipRoom;

            if (RoomOverray!=null)
            {
                Vector2 basePos= Module.MinimapSpriteGenerator.ConvertToMapPos(ShipRoom.transform.localPosition, blueprint);

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
    }
}

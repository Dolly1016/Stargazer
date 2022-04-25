using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stargazer.Map.Builder
{
    /// <summary>
    /// ドアの生成部分です。
    /// MODによる新たなドアを生成させるには、CustomDoorTypeを継承したクラスの
    /// インスタンスをあらかじめ生成させてください。
    /// </summary>
    public class CustomDoorType
    {
        public static Dictionary<string, CustomDoorType> AllTypes = new Dictionary<string, CustomDoorType>();

        public static void LoadVanillaDoorType()
        {
            new VanillaDoorType("SkeldDoor",0);
            new VanillaDoorType("PolusDoor", 1);
            new VanillaDoorType("AirshipDoor", 2);
        }

        public CustomDoorType(string name)
        {
            AllTypes[name] = this;
        }

        public virtual PlainDoor? PreBuild(CustomDoor customDoor, Blueprint blueprint, Transform parent) => null;
    }

    public class VanillaDoorType : CustomDoorType
    {
        private int VanillaType { get; }

        public VanillaDoorType(string name,int type):base(name)
        {
            VanillaType = type;
        }

        public override PlainDoor? PreBuild(CustomDoor customDoor, Blueprint blueprint, Transform parent)
        {
            GameObject? obj = null;
            if (VanillaType == 0)
            {
                //Skeld Door
                obj = Assets.MapAssets.GetAsset(0).transform.FindChild("Cafeteria").FindChild(customDoor.IsVert ? "RightDoor" : "LowerDoor").gameObject;
            }
            else if (VanillaType == 1)
            {
                //Polus Door
                obj = Assets.MapAssets.GetAsset(2).transform.FindChild("Electrical").FindChild(customDoor.IsVert ? "RightDoor" : "BottomDoor").gameObject;
            }
            else if (VanillaType == 2)
            {
                //Airship Door
                obj = Assets.MapAssets.GetAsset(4).transform.FindChild("Records").FindChild(customDoor.IsVert ? "Door_VertOpen (11)" : "Door_HortOpen (2)").gameObject;
            }
            else if (VanillaType == 3)
            {
                //Polus Decontamination Door
            }
            if (obj == null) return null;

            var door = UnityEngine.GameObject.Instantiate(obj,parent);
            var plainDoor=door.GetComponent<PlainDoor>();
            plainDoor.Room = customDoor.RoomId;
            return plainDoor;
        }
    }

    public class CustomDoor : CustomBuilder
    {
        public SystemTypes RoomId { get; set; }
        public bool IsVert { get; set; }
        public string DoorType { get; set; }

        public CustomDoor(string name, Vector2 pos, SystemTypes roomId) : base(name, pos)
        {
            RoomId = roomId;
            DoorType = "SkeldDoor";
        }

        public override void PreBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {
            if (!CustomDoorType.AllTypes.ContainsKey(DoorType)) return;

            PlainDoor? door = CustomDoorType.AllTypes[DoorType].PreBuild(this,blueprint,parent);
            door.transform.localScale = Scale;
            if (door == null) return;
            door.Id = shipStatus.AllDoors.Count + 1;
            shipStatus.AllDoors = Helpers.AddToReferenceArray(shipStatus.AllDoors, door);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stargazer.Map.Builder
{
    public class CustomVent :CustomBuilder
    {
        public string UniqueVentId { get; set; }
        public Vent Vent { get; private set; }
        public string? Left, Center, Right;

        public CustomVent(string name,string uniqueId,Vector2 pos):base(name,pos)
        {
            UniqueVentId = uniqueId;
            Left = null;
            Center = null;
            Right = null;
        }

        public override void PreBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {
            Vent vent = Assets.QuickAssets.GetVent(blueprint.VentType);
            Vent = GameObject.Instantiate<Vent>(vent, parent);
            Vent.Left = null;
            Vent.Center = null;
            Vent.Right = null;
            Vent.name = Name;
            Vent.Id = shipStatus.AllVents.Count + 1;

            GameObject = Vent.gameObject;
            GameObject.name = Name;
            GameObject.transform.localPosition = new Vector3(Position.x, Position.y, 4f);
            GameObject.transform.localScale = Scale;
            GameObject.layer = LayerMask.NameToLayer("ShortObjects");

            shipStatus.AllVents = Helpers.AddToReferenceArray(shipStatus.AllVents, Vent);

            //ベント一覧に追加
            blueprint.Vents[UniqueVentId] = this;
        }

        public override void PostBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {
            if (Left != null) Vent.Left = blueprint.Vents.ContainsKey(Left) ? blueprint.Vents[Left].Vent : null;
            if (Center != null) Vent.Center = blueprint.Vents.ContainsKey(Center) ? blueprint.Vents[Center].Vent : null;
            if (Right != null) Vent.Right = blueprint.Vents.ContainsKey(Right) ? blueprint.Vents[Right].Vent : null;
        }
    }
}

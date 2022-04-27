using System;
using System.Collections.Generic;
using System.Text;
using Stargazer.Map;
using UnityEngine;

namespace Stargazer.Module
{
    public class MinimapSpriteGenerator
    {
        private Blueprint Blueprint { get; set; }
        private ShipStatus ShipStatus { get; set; }

        public MinimapSpriteGenerator(ShipStatus shipStatus,Blueprint blueprint)
        {
            ShipStatus = shipStatus;
            Blueprint = blueprint;
            
        }

        static public Vector2 CalcCenter(ShipStatus shipStatus)
        {
            return (Vector2)(-shipStatus.MapPrefab.HerePoint.transform.parent.localPosition * shipStatus.MapScale);
        }

        static public Vector2 ConvertToMapPos(Vector2 pos,Blueprint blueprint)
        {
            pos = pos - (Vector2)ShipStatus.Instance.transform.position;
            var center = blueprint.MinimapConfiguration.CenterPosition ?? Vector2.zero;
            var scale = blueprint.MinimapConfiguration.MapScale;
            return (pos - center)/scale;
        }
    }
}

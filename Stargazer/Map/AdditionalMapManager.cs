using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

namespace Stargazer.Map
{
    static class AdditionalMapManager
    {
        static List<Blueprint> AdditionalMaps = new List<Blueprint>();

        static public void Load()
        {
            
            var b = new Blueprint("Map");
            b.BaseMapId = 1;
            b.RequirePlainMap = true;

            Builder.CustomObject obj = new Builder.CustomObject("Map",Vector2.zero);
            obj.SetScale(0.7f);
            obj.Sprite.SetAddress("Map.png");
            b.AddChild(obj);

            b.MinimapConfiguration.CenterPosition = new Vector2(0, 3);
            b.MinimapConfiguration.MapScale = 7f;

            AdditionalMaps.Add(b);
            

            var list = Constants.MapNames.ToList();
            foreach (var bp in AdditionalMaps)
            {
                list.Add(bp.Name);
            }
            Constants.MapNames = new UnhollowerBaseLib.Il2CppStringArray(list.ToArray());
        }

        static public void AddPrefabs(AmongUsClient client)
        {
            foreach (var additionalMap in AdditionalMaps)
                client.ShipPrefabs.Add(client.ShipPrefabs[additionalMap.BaseMapId]);
        }

        static public Blueprint? GetBlueprint(byte mapId)
        {
            if (mapId < 5) return null;
            int index = mapId - 5;
            if (index >= AdditionalMaps.Count) return null;
            return AdditionalMaps[index];
        }
    }
}

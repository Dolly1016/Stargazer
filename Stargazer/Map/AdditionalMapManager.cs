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

            b.MinimapConfiguration.CenterPosition = new Vector2(0, 3);
            b.MinimapConfiguration.MapScale = 7f;

            Builder.CustomShipRoom room;
            Builder.CustomConsole console;
            Database.TaskData task;

            Module.CustomSystemTypes.RegisterSystemTypes("Shelter").Text= "Shelter";
            Module.CustomStrings.RegisterStrings("Shelter").Text = "Shelter";
            room = new Builder.CustomShipRoom("Room",Module.CustomSystemTypes.GetSystemTypes("Shelter"),Module.CustomStrings.GetStringNames("Shelter"),Vector2.zero);
            room.SetEdge(new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1));
            room.Sprite.SetAddress("Room.png");
            room.RoomOverray = new Builder.CustomShipRoom.RoomOverrayBuilder();
            b.AddChild(room);

            console = new Builder.CustomConsole("Download1", "Download1", new Vector2(11.5f, -1.07f), Module.CustomSystemTypes.GetSystemTypes("Shelter"));
            console.Sprite.SetAddress("/panel_data");
            console.SetScale(0.7f);
            console.TaskConsoleId = 1;
            b.AddChild(console);

            console = new Builder.CustomConsole("Download2", "Download2", new Vector2(-11.66f, 15.4f), SystemTypes.Armory);
            console.Sprite.SetAddress("/panel_data");
            console.SetScale(0.7f);
            console.TaskConsoleId = 2;
            b.AddChild(console);

            console = new Builder.CustomConsole("Upload", "Upload", new Vector2(-7.26f, 7.2f), SystemTypes.Security);
            console.Sprite.SetAddress("/panel_data");
            console.SetScale(0.7f);
            console.TaskConsoleId = 0;
            b.AddChild(console);

            Builder.CustomObject obj = new Builder.CustomObject("Map", Vector2.zero);
            obj.SetScale(0.7f);
            obj.Sprite.SetAddress("Map.png");
            obj.UseCustomZ(true, 5f);
            b.AddChild(obj);

            task = new Database.TaskData();
            task.ConsoleList.Add(new List<string>(new string[]{ "Download1", "Download2" }));
            task.ConsoleList.Add(new List<string>(new string[] { "Upload" }));
            task.MaxSteps = 2;
            task.TaskCategory = Database.TaskCategory.ShortTask;
            task.TaskType = TaskTypes.UploadData;
            task.TaskTypeArgument = 0;
            b.RegisterTask(task);
            

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
                client.ShipPrefabs.Add(client.ShipPrefabs.get_Item(additionalMap.BaseMapId));
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

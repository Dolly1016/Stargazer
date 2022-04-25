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
            var b = new Blueprint("MIRA HQ+");
            b.BaseMapId = 1;
            b.RequirePlainMap = true;

            var edges = new Vector2[] {
                new Vector2(-3, -2), new Vector2(-3, 2), new Vector2(3, 2), new Vector2(3, -2)
            };
            var edgesS = new Vector2[] {
                new Vector2(-3, -2f), new Vector2(-3, 2.7f), new Vector2(3, 2.7f), new Vector2(3, -2)
            };
            Builder.CustomShipRoom room;
            Builder.CustomVent vent;
            Builder.CustomConsole console;

            room = new Builder.CustomShipRoom("MainHall", SystemTypes.MainHall,StringNames.MainHall, new Vector2(-5, 23));
            room.Sprite.SetAddress("TestRoom.png");
            room.SetEdge(edges);
            room.RoomOverray = new Builder.CustomShipRoom.RoomOverrayBuilder();
            room.AddChild(new Builder.CustomWall("Wall", edges));
            room.AddChild(new Builder.CustomShadow("Shadows", edgesS));
            vent = new Builder.CustomVent("Vent1","CVent1",new Vector2(-1,1));
            vent.Left = "CVent2";
            room.AddChild(vent);
            console = new Builder.CustomConsole("Download", "Download", new Vector2(-2, 2.4f), SystemTypes.MainHall);
            console.Sprite.SetAddress("TestConsole.png");
            console.TaskConsoleId = 0;
            room.AddChild(console);
            console = new Builder.CustomConsole("Admin", "Admin", new Vector2(-1, 2.4f), SystemTypes.MainHall);
            console.Sprite.SetAddress("TestConsole.png");
            console.TaskConsoleId = 0;
            room.AddChild(console);
            console = new Builder.CustomConsole("DivertBase", "DivertBase", new Vector2(1, 2.4f), SystemTypes.MainHall);
            console.Sprite.SetAddress("TestConsole.png");
            console.TaskConsoleId = 0;
            room.AddChild(console);
            b.AddChild(room);

            room = new Builder.CustomShipRoom("Navigation", SystemTypes.Nav, StringNames.Lounge, new Vector2(1.05f, 23));
            room.Sprite.SetAddress("TestRoom.png");
            room.SetEdge(edges);
            room.RoomOverray = new Builder.CustomShipRoom.RoomOverrayBuilder();
            room.AddChild(new Builder.CustomWall("Wall", edges));
            room.AddChild(new Builder.CustomShadow("Shadows", edgesS));
            vent = new Builder.CustomVent("Vent2", "CVent2", new Vector2(1, -1));
            vent.Left = "CVent1";
            room.AddChild(vent);
            console = new Builder.CustomConsole("Upload", "Upload", new Vector2(-2, -1f), SystemTypes.Nav);
            console.Sprite.SetAddress("/snowman");
            console.IsBack = false;
            console.TaskConsoleId = 0;
            room.AddChild(console);
            console = new Builder.CustomConsole("DivertFor", "DivertFor", new Vector2(1, 2.4f), SystemTypes.Nav);
            console.Sprite.SetAddress("TestConsole.png");
            console.TaskConsoleId = 0;
            room.AddChild(console);
            Builder.CustomLadder ladder = new Builder.CustomLadder("NavLadder", new Vector2(0, 2.4f), new Vector2(0, 1.25f), new Vector2(0, -1.63f), Builder.CustomLadder.UsableMask.CanOnlyClimbUp);
            ladder.Sprite.SetAddress("/ladder_electrical");
            ladder.SetScale(0.7f);
            room.AddChild(ladder);
            b.AddChild(room);

            Database.TaskData task;
            List<string> tList;
            
            task = new Database.TaskData();
            tList = new List<string>();
            tList.Add("Admin");
            task.ConsoleList.Add(tList);
            task.MaxSteps = 1;
            task.TaskCategory = Database.TaskCategory.CommonTask;
            task.TaskType = TaskTypes.SwipeCard;
            task.StartAt = SystemTypes.MainHall;
            b.RegisterTask(task);

            task = new Database.TaskData();
            tList = new List<string>();
            tList.Add("Download");
            task.ConsoleList.Add(tList);
            tList = new List<string>();
            tList.Add("Upload");
            task.ConsoleList.Add(tList);
            task.MaxSteps = 2;
            task.TaskCategory = Database.TaskCategory.ShortTask;
            task.TaskType = TaskTypes.UploadData;
            b.RegisterTask(task);

            task = new Database.TaskData();
            tList = new List<string>();
            tList.Add("DivertBase");
            task.ConsoleList.Add(tList);
            tList = new List<string>();
            tList.Add("DivertFor");
            task.ConsoleList.Add(tList);
            task.MaxSteps = 2;
            task.TaskCategory = Database.TaskCategory.LongTask;
            task.TaskType = TaskTypes.DivertPower;
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

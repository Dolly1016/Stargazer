using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stargazer.Map.Builder.Task
{
    public class TaskBuilder
    {
        public static Dictionary<TaskTypes, TaskBuilder> TaskBuilders = new Dictionary<TaskTypes, TaskBuilder>();

        public static void LoadVanillaTaskBuilders()
        {
            TaskBuilders[TaskTypes.UploadData] = new UploadTaskBuilder();
            
            TaskBuilder defaultBuilder = new TaskBuilder();
            TaskBuilders[TaskTypes.PrimeShields] = defaultBuilder;
            TaskBuilders[TaskTypes.FuelEngines] = defaultBuilder;
            TaskBuilders[TaskTypes.ChartCourse] = defaultBuilder;
            TaskBuilders[TaskTypes.StartReactor] = defaultBuilder;
            TaskBuilders[TaskTypes.SwipeCard] = defaultBuilder;
            TaskBuilders[TaskTypes.ClearAsteroids] = defaultBuilder;
            TaskBuilders[TaskTypes.InspectSample] = defaultBuilder;
            TaskBuilders[TaskTypes.EmptyChute] = defaultBuilder;
            TaskBuilders[TaskTypes.EmptyGarbage] = defaultBuilder;
            TaskBuilders[TaskTypes.AlignEngineOutput] = defaultBuilder;
            TaskBuilders[TaskTypes.FixWiring] = defaultBuilder;
            TaskBuilders[TaskTypes.CalibrateDistributor] = defaultBuilder;
            TaskBuilders[TaskTypes.UnlockManifolds] = defaultBuilder;
            TaskBuilders[TaskTypes.ResetReactor] = defaultBuilder;
            TaskBuilders[TaskTypes.FixLights] = defaultBuilder;
            TaskBuilders[TaskTypes.CleanO2Filter] = defaultBuilder;
            TaskBuilders[TaskTypes.FixComms] = defaultBuilder;
            TaskBuilders[TaskTypes.RestoreOxy] = defaultBuilder;
            TaskBuilders[TaskTypes.StabilizeSteering] = defaultBuilder;
            TaskBuilders[TaskTypes.AssembleArtifact] = defaultBuilder;
            TaskBuilders[TaskTypes.SortSamples] = defaultBuilder;
            TaskBuilders[TaskTypes.MeasureWeather] = defaultBuilder;
            TaskBuilders[TaskTypes.EnterIdCode] = defaultBuilder;
            TaskBuilders[TaskTypes.BuyBeverage] = defaultBuilder;
            TaskBuilders[TaskTypes.ProcessData] = defaultBuilder;
            TaskBuilders[TaskTypes.RunDiagnostics] = defaultBuilder;
            TaskBuilders[TaskTypes.WaterPlants] = defaultBuilder;
            TaskBuilders[TaskTypes.MonitorOxygen] = defaultBuilder;
            TaskBuilders[TaskTypes.StoreArtifacts] = defaultBuilder;
            TaskBuilders[TaskTypes.FillCanisters] = defaultBuilder;
            TaskBuilders[TaskTypes.InsertKeys] = defaultBuilder;
            TaskBuilders[TaskTypes.ResetSeismic] = defaultBuilder;
            TaskBuilders[TaskTypes.ScanBoardingPass] = defaultBuilder;
            TaskBuilders[TaskTypes.ReplaceWaterJug] = defaultBuilder;
            TaskBuilders[TaskTypes.RepairDrill] = defaultBuilder;
            TaskBuilders[TaskTypes.AlignTelescope] = defaultBuilder;
            TaskBuilders[TaskTypes.RecordTemperature] = defaultBuilder;
            TaskBuilders[TaskTypes.RebootWifi] = defaultBuilder;
            TaskBuilders[TaskTypes.PolishRuby] = defaultBuilder;
            TaskBuilders[TaskTypes.ResetBreakers] = defaultBuilder;
            TaskBuilders[TaskTypes.Decontaminate] = defaultBuilder;
            TaskBuilders[TaskTypes.MakeBurger] = defaultBuilder;
            TaskBuilders[TaskTypes.UnlockSafe] = defaultBuilder;
            TaskBuilders[TaskTypes.SortRecords] = defaultBuilder;
            TaskBuilders[TaskTypes.FixShower] = defaultBuilder;
            TaskBuilders[TaskTypes.CleanToilet] = defaultBuilder;
            TaskBuilders[TaskTypes.DressMannequin] = defaultBuilder;
            TaskBuilders[TaskTypes.RewindTapes] = defaultBuilder;
            TaskBuilders[TaskTypes.StartFans] = defaultBuilder;
            TaskBuilders[TaskTypes.DevelopPhotos] = defaultBuilder;
            TaskBuilders[TaskTypes.GetBiggolSword] = defaultBuilder;
            TaskBuilders[TaskTypes.StopCharles] = defaultBuilder;
            TaskBuilders[TaskTypes.VentCleaning] = defaultBuilder;

            //特殊なComponentを要求するタスク
            TaskBuilders[TaskTypes.PutAwayPistols] = new ExtendedTaskBuilder<StoreArmsTaskConsole, NormalPlayerTask>((console, data) => {
                var orig = Assets.MapAssets.GetAsset(4).NormalTasks[18].Cast<StoreArmsTaskConsole>();
                console.timesUsed = orig.timesUsed;
                console.Images = orig.Images;
                console.useSound = orig.useSound;
                console.usesPerStep = orig.usesPerStep;
            });
            TaskBuilders[TaskTypes.PutAwayRifles] = new ExtendedTaskBuilder<StoreArmsTaskConsole, NormalPlayerTask>((console, data) => {
                var orig = Assets.MapAssets.GetAsset(4).NormalTasks[19].Cast<StoreArmsTaskConsole>();
                console.timesUsed = orig.timesUsed;
                console.Images = orig.Images;
                console.useSound = orig.useSound;
                console.usesPerStep = orig.usesPerStep;
            });
            TaskBuilders[TaskTypes.DivertPower] = new DivertPowerTaskBuilder();
            TaskBuilders[TaskTypes.PickUpTowels] = new ExtendedTaskBuilder<TowelTaskConsole, TowelTask>((console,data)=> { console.useSound=Assets.MapAssets.GetAsset(4).NormalTasks[14].Cast<TowelTaskConsole>().useSound; });
            TaskBuilders[TaskTypes.OpenWaterways] = new ExtendedTaskBuilder<Console, WaterWayTask>();

            //特殊な設定を必要とするComponentを要求するタスク
            TaskBuilders[TaskTypes.ActivateWeatherNodes] = new ExtendedTaskBuilder<Console, WeatherNodeTask>(null, (task, data) => { task.Stage2Prefab = Assets.QuickAssets.GetMinigamePrefab(data.TaskType, 1); });
        }

        protected virtual Console SetUpConsole(CustomConsole cc, Database.TaskData taskData,int step)
        {
            Console console;
            console = cc.GameObject.AddComponent<Console>();
            InitializeConsole(console,cc,taskData,step);
            return console;
        }

        protected void InitializeConsole(Console console, CustomConsole cc,Database.TaskData taskData,int step)
        {
            console.checkWalls = true;
            console.usableDistance = 0.9f;
            console.TaskTypes = new TaskTypes[] { taskData.TaskType };
            TaskSet taskSet = new TaskSet();
            taskSet.taskType = taskData.TaskType;
            taskSet.taskStep = new IntRange(step,step);
            console.ValidTasks = new UnhollowerBaseLib.Il2CppReferenceArray<TaskSet>(new TaskSet[] { taskSet });
            console.Room = cc.RoomId;
            console.ConsoleId = cc.TaskConsoleId;
            console.TaskTypes = new TaskTypes[] { taskData.TaskType };
        }

        protected virtual void SetUpButton(CustomConsole cc, Blueprint blueprint, Database.TaskData taskData)
        {
            PassiveButton button = cc.GameObject.GetComponent<PassiveButton>();

            if (!button)
            {
                button = cc.GameObject.AddComponent<PassiveButton>();
                button.OnMouseOut = new UnityEngine.Events.UnityEvent();
                button.OnMouseOver = new UnityEngine.Events.UnityEvent();
                button._CachedZ_k__BackingField = 0.1f;
                button.CachedZ = 0.1f;
            }
        }

        protected virtual void SetUpCollider(CustomConsole cc, Blueprint blueprint, Database.TaskData taskData)
        {
            CircleCollider2D collider = cc.GameObject.GetComponent<CircleCollider2D>();

            if (!collider)
            {
                collider = cc.GameObject.AddComponent<CircleCollider2D>();
                collider.radius = 0.4f;
                collider.isTrigger = true;
            }
        }

        protected virtual NormalPlayerTask SetUpPlayerTask(GameObject taskHolder, Database.TaskData taskData)
        {
            NormalPlayerTask task;
            task = taskHolder.AddComponent<NormalPlayerTask>();

            InitializePlayerTask(task,taskData);

            return task;
        }

        protected static void InitializePlayerTask(NormalPlayerTask task,Database.TaskData taskData)
        {
            task.MaxStep = taskData.MaxSteps;
            task.ShowTaskStep = taskData.ShowTaskSteps && task.MaxStep > 1;
            task.ShowTaskTimer = taskData.ShowTaskTimer;
            task.TaskTimer = 0;
            task.TimerStarted = NormalPlayerTask.TimerState.NotStarted;
            task.TaskType = taskData.TaskType;
            task.HasLocation = true;
            task.StartAt = taskData.StartAt;
            task.Length = Database.TaskData.ConvertLength(taskData.TaskCategory);
            task.MinigamePrefab = Assets.QuickAssets.GetMinigamePrefab(taskData.TaskType,taskData.TaskTypeArgument);
            task.Arrow = GameObject.Instantiate(Assets.MapAssets.GetAsset(0).NormalTasks[0].Arrow, task.gameObject.transform);
        }

        public static NormalPlayerTask GenerateDefaultTask()
        {
            GameObject holder = new GameObject("DefaultTask");
            var task = holder.AddComponent<NormalPlayerTask>();
            var data = new Database.TaskData();
            InitializePlayerTask(task,data);
            return task;
        }

        protected SystemTypes GetStartAtRoomByConsoles(List<string> consoles,Blueprint blueprint)
        {
            string id = consoles[StargazerPlugin.Random.Next(consoles.Count)];
            if (!blueprint.Consoles.ContainsKey(id)) return SystemTypes.Hallway;
            var cc = blueprint.Consoles[id];
            return cc.RoomId;
        }

        protected void ScheduleSetStartAtRoomByConsoles(NormalPlayerTask task, List<string> consoles, ShipStatus shipStatus, Blueprint blueprint, Database.TaskData taskData)
        {
            var opt = task.gameObject.AddComponent<Behaviours.NormalPlayerTaskOption>();
            
            opt.InitializerId.Set(Behaviours.CustomShipStatus.Instance.TaskActions.RegisterTaskInitializer(
                (task) =>
                {
                    task.StartAt = GetStartAtRoomByConsoles(consoles, blueprint);
                }
                ));
        }

        protected void SetUpConsolesByList(List<string> consoles, ShipStatus shipStatus, Blueprint blueprint, Database.TaskData taskData,int step,Action<Console>? afterAction=null)
        {
            foreach (string id in consoles)
            {
                if (!blueprint.Consoles.ContainsKey(id)) continue;
                var cc = blueprint.Consoles[id];
                var console = SetUpConsole(cc, taskData,step);
                shipStatus.AllConsoles = Helpers.AddToReferenceArray(shipStatus.AllConsoles, console);
                console.Image = cc.GameObject.GetComponent<SpriteRenderer>();
                console.Image.material = new Material(blueprint.HighlightMaterial);
                SetUpButton(cc, blueprint, taskData);
                SetUpCollider(cc, blueprint, taskData);

                if (afterAction != null) afterAction.Invoke(console);
            }
        }

        //タスクデータを基にタスクを生成します。
        public virtual NormalPlayerTask BuildTask(ShipStatus shipStatus,GameObject taskHolder,Blueprint blueprint, Database.TaskData taskData)
        {
            int step = 0;
            foreach (var list in taskData.ConsoleList)
            {
                SetUpConsolesByList(list, shipStatus, blueprint, taskData, step);
                step++;
            }

            return SetUpPlayerTask(taskHolder,taskData);
        }
    }

    public class ExtendedTaskBuilder<EConsole,ETask> : TaskBuilder where EConsole : Console where ETask : NormalPlayerTask
    {
        public delegate void ExtendedTaskBuilderF(ETask task,Database.TaskData taskData);
        public delegate void ExtendedConsoleBuilderF(EConsole task, Database.TaskData taskData);

        private ExtendedTaskBuilderF? TBuilder;
        private ExtendedConsoleBuilderF? CBuilder;

        public ExtendedTaskBuilder(ExtendedConsoleBuilderF? cBuilder = null,ExtendedTaskBuilderF ? tBuilder = null)
        {
            CBuilder = cBuilder;
            TBuilder = tBuilder;
        }

        protected override NormalPlayerTask SetUpPlayerTask(GameObject taskHolder, Database.TaskData taskData)
        {
            ETask task;
            task = taskHolder.AddComponent<ETask>();

            InitializePlayerTask(task, taskData);
            if(TBuilder!=null)TBuilder.Invoke(task,taskData);

            return task;
        }

        protected override Console SetUpConsole(CustomConsole cc, Database.TaskData taskData,int step)
        {
            EConsole console;
            console = cc.GameObject.AddComponent<EConsole>();

            InitializeConsole(console, cc, taskData,step);
            if (CBuilder != null) CBuilder.Invoke(console, taskData);
            
            return console;
        }
    }

    public class UploadTaskBuilder : TaskBuilder
    {
        protected override NormalPlayerTask SetUpPlayerTask(GameObject taskHolder, Database.TaskData taskData)
        {
            NormalPlayerTask task;
            if (taskData.TaskTypeArgument == 0)
            {
                task = taskHolder.AddComponent<UploadDataTask>();
            }
            else
            {
                task = taskHolder.AddComponent<AirshipUploadTask>();
            }

            InitializePlayerTask(task, taskData);

            return task;
        }

        public override NormalPlayerTask BuildTask(ShipStatus shipStatus, GameObject taskHolder, Blueprint blueprint, Database.TaskData taskData)
        {
            var task = base.BuildTask(shipStatus, taskHolder, blueprint, taskData);
            if (taskData.TaskTypeArgument == 0)
                task.GetComponent<UploadDataTask>().EndAt = blueprint.Consoles[taskData.ConsoleList[1][0]].RoomId;

            foreach (string console in taskData.ConsoleList[0])
            {
                if (!blueprint.Consoles.ContainsKey(console)) continue;
                var cc = blueprint.Consoles[console];
                cc.GameObject.GetComponent<Console>().TaskTypes = new TaskTypes[0];
            }
            foreach (string console in taskData.ConsoleList[1])
            {
                if (!blueprint.Consoles.ContainsKey(console)) continue;
                var cc = blueprint.Consoles[console];
                cc.GameObject.GetComponent<Console>().ValidTasks = new UnhollowerBaseLib.Il2CppReferenceArray<TaskSet>(0);
            }

            ScheduleSetStartAtRoomByConsoles(task,taskData.ConsoleList[0],shipStatus,blueprint,taskData);
            return task;
        }
    }

    public class DivertPowerTaskBuilder : ExtendedTaskBuilder<Console, DivertPowerTask>
    {
        public override NormalPlayerTask BuildTask(ShipStatus shipStatus, GameObject taskHolder, Blueprint blueprint, Database.TaskData taskData)
        {
            var result = base.BuildTask(shipStatus,taskHolder,blueprint,taskData);
            
            var opt = result.gameObject.AddComponent<Behaviours.NormalPlayerTaskOption>();
            
            opt.InitializerId.Set(Behaviours.CustomShipStatus.Instance.TaskActions.RegisterTaskInitializer(
                (task) =>
                {
                    task.gameObject.GetComponent<DivertPowerTask>().TargetSystem = GetStartAtRoomByConsoles(taskData.ConsoleList[1], blueprint);
                }
                ));

            return result;
        }
    }

    public delegate NormalPlayerTask TaskBuilderF(ShipStatus shipStatus, GameObject taskHolder, Blueprint blueprint, Database.TaskData taskData);

    public class SimpleTaskBuilder : TaskBuilder
    {
        private TaskBuilderF Builder;

        public SimpleTaskBuilder(TaskBuilderF taskBuilder)
        {
            Builder = taskBuilder;
        }

        public override NormalPlayerTask BuildTask(ShipStatus shipStatus,GameObject taskHolder, Blueprint blueprint, Database.TaskData taskData)
        {
            return Builder.Invoke(shipStatus,taskHolder, blueprint, taskData);
        }
    }
}

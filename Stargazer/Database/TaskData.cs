using System;
using System.Collections.Generic;
using System.Text;

namespace Stargazer.Database
{
    public enum TaskCategory
    {
        CommonTask,
        LongTask,
        ShortTask
    }

    public class TaskData
    {
        static public NormalPlayerTask.TaskLength ConvertLength(TaskCategory category)
        {
            switch (category)
            {
                case TaskCategory.CommonTask:
                    return NormalPlayerTask.TaskLength.Common;
                case TaskCategory.LongTask:
                    return NormalPlayerTask.TaskLength.Long;
                case TaskCategory.ShortTask:
                    return NormalPlayerTask.TaskLength.Short;
            }
            return NormalPlayerTask.TaskLength.None;
        }

        public List<List<string>> ConsoleList { get; set; }

        public TaskTypes TaskType { get; set; }
        public int TaskTypeArgument { get; set; }

        public int MaxSteps { get; set; }
        public TaskCategory TaskCategory { get; set; }

        public bool ShowTaskTimer { get; set; }
        public bool ShowTaskSteps { get; set; }

        public SystemTypes StartAt { get; set; }

        public TaskData()
        {
            ConsoleList = new List<List<string>>();
            TaskType = TaskTypes.SwipeCard;
            MaxSteps = 1;
            TaskCategory = TaskCategory.CommonTask;
            TaskTypeArgument = 0;

            ShowTaskTimer = false;
            ShowTaskSteps = true;

            StartAt = SystemTypes.Hallway;
        }
    }
}

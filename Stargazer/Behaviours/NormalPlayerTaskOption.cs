using System;
using System.Collections.Generic;
using System.Text;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using UnhollowerBaseLib.Attributes;

namespace Stargazer.Behaviours
{
 
    public class TaskActionStorage
    {
        private List<Action<NormalPlayerTask>> TaskInitializerList = new List<Action<NormalPlayerTask>>();
        private List<Action<Minigame>> PrebeginReformerList = new List<Action<Minigame>>();

        public int RegisterTaskInitializer(Action<NormalPlayerTask> action)
        {
            int result = TaskInitializerList.Count;
            TaskInitializerList.Add(action);
            return result+1;
        }

        public int RegisterPrebeginReformer(Action<Minigame> action)
        {
            int result = PrebeginReformerList.Count;
            PrebeginReformerList.Add(action);
            return result+1;
        }

        public void RunTaskInitializer(int id, NormalPlayerTask task)
        {
            id--;
            if (TaskInitializerList.Count <= id || id < 0) return;
            TaskInitializerList[id].Invoke(task);
        }

        public void RunPrebeginReformer(int id, Minigame minigame)
        {
            id--;
            if (PrebeginReformerList.Count <= id || id < 0) return;
            PrebeginReformerList[id].Invoke(minigame);
        }
    }

    public class NormalPlayerTaskOption : MonoBehaviour
    {
        static NormalPlayerTaskOption()
        {
            ClassInjector.RegisterTypeInIl2Cpp<NormalPlayerTaskOption>();
        }


        public NormalPlayerTaskOption(IntPtr ptr) : base(ptr)
        {
            enabled = true;
        }



        public Il2CppValueField<int> InitializerId;
        public Il2CppValueField<int> PrebeginReformerId;

        public void Initialize()
        {
            CustomShipStatus.Instance.TaskActions.RunTaskInitializer(InitializerId, gameObject.GetComponent<NormalPlayerTask>());
        }

        public void Prebegin(Minigame minigame)
        {
            CustomShipStatus.Instance.TaskActions.RunPrebeginReformer(PrebeginReformerId, minigame);
        }
    }
}

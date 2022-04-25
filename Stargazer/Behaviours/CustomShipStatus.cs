using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnhollowerRuntimeLib;

namespace Stargazer.Behaviours
{
    public class CustomShipStatus : MonoBehaviour
    {
        public Behaviours.TaskActionStorage TaskActions;

        static public CustomShipStatus? Instance;

        public List<Ladder> Ladders;
        public List<MovingPlatformBehaviour> MovingPlatformBehaviours;

        static CustomShipStatus()
        {
            ClassInjector.RegisterTypeInIl2Cpp<CustomShipStatus>();
        }

        public CustomShipStatus(IntPtr ptr) : base(ptr)
        {
            enabled = true;

            Ladders = new List<Ladder>();
            MovingPlatformBehaviours = new List<MovingPlatformBehaviour>();

            TaskActions = new TaskActionStorage();
        }
    }
}

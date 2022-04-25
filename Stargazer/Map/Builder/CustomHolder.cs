using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stargazer.Map.Builder
{
    public class CustomHolder:CustomBuilder
    {
        private HashSet<CustomBuilder> builders;

        public CustomHolder(string name,Vector2 pos) : base(name, pos)
        {
            builders = new HashSet<CustomBuilder>();
        }

        protected void PreBuildChildren(Blueprint blueprint, ShipStatus shipStatus)
        {
            foreach(var builder in builders) { builder.PreBuild(blueprint,shipStatus, GameObject.transform); }
        }

        protected void PostBuildChildren(Blueprint blueprint, ShipStatus shipStatus)
        {
            foreach (var builder in builders) { builder.PostBuild(blueprint,shipStatus, GameObject.transform); }
        }

        public override bool AddChild(CustomBuilder builder)
        {
            builders.Add(builder);
            return true;
        }

        public override void PreBuild(Blueprint blueprint, ShipStatus shipStatus,Transform parent)
        {
            GameObject = new GameObject(Name);
            GameObject.transform.SetParent(parent);
            GameObject.transform.localPosition = Position;
            GameObject.transform.localScale = Scale;
            GameObject.SetActive(true);

            PreBuildChildren(blueprint, shipStatus);
        }

        public override void PostBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {
            PostBuildChildren(blueprint,shipStatus);
        }
    }
}

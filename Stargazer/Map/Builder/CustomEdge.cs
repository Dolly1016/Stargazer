using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stargazer.Map.Builder
{
    public class CustomEdge : CustomBuilder
    {
        private Vector2[] Edges;

        public CustomEdge(string name, params Vector2[] edges) : base(name, Vector2.zero)
        {
            Edges = edges;
        }

        public bool AddChild(CustomBuilder builder) => false;

        public override void PreBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {
            GameObject = new GameObject(Name);
            GameObject.transform.SetParent(parent);
            GameObject.transform.localPosition = Position;
            GameObject.transform.localScale = Scale;
            GameObject.SetActive(true);

            var Collider = GameObject.AddComponent<EdgeCollider2D>();
            Collider.points = Edges;
            Collider.enabled = true;
        }

        public override void PostBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {

        }
    }
}

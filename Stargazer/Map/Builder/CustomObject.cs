using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stargazer.Map.Builder
{
    public class CustomObject : CustomBuilder
    {
        public bool IsFront { get; set; }

        public bool IsBack { get; set; }

        public SpriteRenderer Renderer { get; private set; }
        public AddressableSprite Sprite { get; set; }

        public CustomObject(string name,Vector2 pos,bool isFront = false,bool isBack=true):base(name,pos)
        {
            IsFront = isFront;
            IsBack = isBack;
            Sprite = new AddressableSprite();
        }

        public override void PreBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {
            GameObject = new GameObject(Name);
            GameObject.transform.SetParent(parent);
            GameObject.transform.localPosition = new Vector3(Position.x, Position.y, IsFront ? -2f : 4f);
            if (!(IsBack || IsFront))
            {
                var pos = GameObject.transform.position;
                GameObject.transform.position = new Vector3(pos.x, pos.y, pos.y / 1000f);
            }
            GameObject.transform.localScale = Scale;
            GameObject.SetActive(true);
            GameObject.layer = LayerMask.NameToLayer("Ship");

            if (Sprite.GetSprite(blueprint) != null)
            {
                Renderer = GameObject.AddComponent<SpriteRenderer>();
                Renderer.sprite = Sprite.GetSprite(blueprint);
                Renderer.material = blueprint.MaskingShader;
            }
        }

        public override void PostBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {

        }
    }
}

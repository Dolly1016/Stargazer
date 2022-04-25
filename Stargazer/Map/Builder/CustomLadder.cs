using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stargazer.Map.Builder
{
    public class CustomLadder :CustomBuilder
    {
        [Flags]
        public enum UsableMask
        {
            None = 0x00,
            CanOnlyClimbUp=0x01,
            CanOnlyClimbDown = 0x10,
            Normal=0x11
        }

        public SpriteRenderer Renderer { get; private set; }
        public AddressableSprite Sprite { get; set; }

        public UsableMask Mask { get; set; }

        public Vector3 TopPos { get; set; }
        public Vector3 BottomPos { get; set; }

        public Ladder Top, Bottom;

        public CustomLadder(string name, Vector2 pos,Vector2 topDis,Vector2 bottomDis, UsableMask usableMask) : base(name, pos)
        {
            TopPos = new Vector3(topDis.x,topDis.y,0.1f);
            BottomPos = new Vector3(bottomDis.x, bottomDis.y, 0.1f);
            Mask = usableMask;
            Sprite = new AddressableSprite();
        }

        public CustomLadder(string name, Vector2 pos, float length, UsableMask usableMask) : base(name, pos)
        {
            TopPos = new Vector3(0, length / 2f,0.1f);
            BottomPos = new Vector3(0, -length / 2f,0.1f);
            Mask = usableMask;
            Sprite = new AddressableSprite();
        }


        public override void PreBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent)
        {
            GameObject = GameObject.Instantiate(Assets.QuickAssets.GetLadderBase());
            GameObject.SetName(GameObject, Name);
            GameObject.transform.SetParent(parent);
            GameObject.transform.localPosition = new Vector3(Position.x, Position.y, 1f);
            GameObject.transform.localScale = Scale;
            GameObject.SetActive(true);
            GameObject.layer = LayerMask.NameToLayer("ShortObjects");

            if (Sprite.GetSprite(blueprint) != null)
            {
                Renderer = GameObject.GetComponent<SpriteRenderer>();
                Renderer.sprite = Sprite.GetSprite(blueprint);
            }

            Top = GameObject.transform.FindChild("LadderTop").gameObject.GetComponent<Ladder>();
            Top.transform.SetParent(GameObject.transform);
            Top.transform.localPosition = TopPos;
            Top.Image = Renderer;

            Bottom = GameObject.transform.FindChild("LadderBottom").gameObject.AddComponent<Ladder>();
            Bottom.transform.SetParent(GameObject.transform);
            Bottom.transform.localPosition = BottomPos;
            Bottom.Image = Renderer;

            var ladders = Behaviours.CustomShipStatus.Instance.Ladders;

            Top.Id = (byte)ladders.Count;
            Behaviours.CustomShipStatus.Instance.Ladders.Add(Top);

            Bottom.Id = (byte)ladders.Count;
            Behaviours.CustomShipStatus.Instance.Ladders.Add(Bottom);

            Bottom.gameObject.SetActive((int)(Mask & UsableMask.CanOnlyClimbUp) != 0 );
            Top.gameObject.SetActive((int)(Mask & UsableMask.CanOnlyClimbDown) != 0);
        }
    }
}

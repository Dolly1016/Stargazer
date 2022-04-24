﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stargazer.Map.Builder
{
    public class AddressableSprite
    {
        public string? Address { get; private set; }
        private Sprite? Sprite;
        private bool isDirty;
        public AddressableSprite()
        {
            Address = null;
            Sprite = null;
            isDirty = false;
        }

        public void SetAddress(string address)
        {
            Address = address;
            isDirty = true;
        }

        public Sprite? GetSprite(Blueprint blueprint)
        {
            if (!isDirty) return Sprite;
            
            isDirty = false;

            if (Address == null) { Sprite = null; return null; }
            if (Address.StartsWith("/"))
            {
                Sprite = Assets.MapAssets.GetSprite(Address.Substring(1));
            }
            else
            {
                Sprite = Helpers.loadSpriteFromDisk(blueprint.GetAddressPrefix() + Address, 100f);
            }
            return Sprite;
        }
    }

    public class CustomBuilder
    {
        public string Name { get; set; }
        public GameObject? GameObject { get; set; }
        public Vector3 Position { get; set; }

        protected CustomBuilder(string name,Vector3 pos)
        {
            Name = name;
            Position = pos;
            GameObject = null;
        }

        public virtual bool AddChild(CustomBuilder builder) => false;
        public virtual void PreBuild(Blueprint blueprint,ShipStatus shipStatus, Transform parent) { }
        public virtual void PostBuild(Blueprint blueprint, ShipStatus shipStatus, Transform parent) { }
    }
}

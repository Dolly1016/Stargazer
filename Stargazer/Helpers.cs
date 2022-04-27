using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;
using UnhollowerBaseLib;

namespace Stargazer
{
    public static class Helpers
    {
        static public bool PlayingModMap()
        {
            return PlayerControl.GameOptions.MapId >= 5;
        }

        static public void AddToReferenceArray<T>(ref UnhollowerBaseLib.Il2CppReferenceArray<T> array,T obj) where T : UnhollowerBaseLib.Il2CppObjectBase
        {
            array = AddToReferenceArray<T>(array, obj);
        }

        static public UnhollowerBaseLib.Il2CppReferenceArray<T> AddToReferenceArray<T>(UnhollowerBaseLib.Il2CppReferenceArray<T> array, T obj) where T : UnhollowerBaseLib.Il2CppObjectBase
        {
            var list = array.ToList();
            list.Add(obj);
            return new UnhollowerBaseLib.Il2CppReferenceArray<T>(list.ToArray());
        }

        public static Sprite loadSpriteFromDisk(string path, float pixelsPerUnit, Rect textureRect)
        {
            return Sprite.Create(loadTextureFromDisk(path), textureRect, new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }

        public static Sprite loadSpriteFromDisk(string path, float pixelsPerUnit, Rect textureRect, Vector2 pivot)
        {
            return Sprite.Create(loadTextureFromDisk(path), textureRect, pivot, pixelsPerUnit);
        }

        public static Sprite loadSpriteFromDisk(string path, float pixelsPerUnit)
        {
            var tex = loadTextureFromDisk(path);
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }

        public static Texture2D loadTextureFromDisk(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                    byte[] byteTexture = File.ReadAllBytes(path);
                    LoadImage(texture, byteTexture, false);
                    return texture;
                }
            }
            catch
            {
                System.Console.WriteLine("Error loading texture from disk: " + path);
            }
            return null;
        }

        internal delegate bool d_LoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
        internal static d_LoadImage iCall_LoadImage;
        private static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
        {
            if (iCall_LoadImage == null)
                iCall_LoadImage = IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");
            var il2cppArray = (Il2CppStructArray<byte>)data;
            return iCall_LoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
        }

        public static bool InModMap()
        {
            return Behaviours.CustomShipStatus.Instance != null;
        }

        public static void AlignZ(this Transform transform)
        {
            var pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, pos.z / 1000f);
        }
    }
}

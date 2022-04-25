using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Stargazer.Module
{
    public class CustomImageNames
    {
        static private Dictionary<string, ImageNames> vanillaDic = new Dictionary<string, ImageNames>();
        static private Dictionary<string, string> spriteAddressDic = new Dictionary<string, string>();
        static private Dictionary<string, CustomImageNames> stringDic = new Dictionary<string, CustomImageNames>();
        static private Dictionary<int, CustomImageNames> intDic = new Dictionary<int, CustomImageNames>();
        static private int availableId = 128;

        private int Id;
        private string Name,Address;
        private Sprite sprite;

        public static CustomImageNames RegisterImageNames(string name,string address)
        {
            if (stringDic.ContainsKey(name)) return stringDic[name];
            return new CustomImageNames(name, address, availableId++);
        }

        private CustomImageNames(string name,string address, int id)
        {
            Name = name;
            Address = address;
            Id = id;

            spriteAddressDic[name] = address;
            stringDic[name] = this;
            intDic[id] = this;
        }

        public Sprite GetSprite()
        {
            if (sprite) return sprite;
            sprite = Helpers.loadSpriteFromDisk(Address,100f);
            return sprite;
        }

        public static bool IsCustomImageNames(ImageNames systemTypes)
        {
            return intDic.ContainsKey((int)systemTypes);
        }

        public static CustomImageNames? GetCustomImageNames(ImageNames imageNames)
        {
            if (IsCustomImageNames(imageNames)) return intDic[(int)imageNames];
            return null;
        }
    }

    public class CustomSystemTypes
    {
        static private Dictionary<string, SystemTypes> vanillaDic = new Dictionary<string, SystemTypes>();
        static private Dictionary<string, CustomSystemTypes> stringDic = new Dictionary<string, CustomSystemTypes>();
        static private Dictionary<int, CustomSystemTypes> intDic = new Dictionary<int, CustomSystemTypes>();
        static private int availableId = 128;

        private int Id;
        private string Name;

        public static void LoadVanillaSystemTypes()
        {
            foreach(var systemTypes in SystemTypeHelpers.AllTypes)
                vanillaDic[systemTypes.ToString()] = systemTypes;
        }

        public static CustomSystemTypes RegisterSystemTypes(string name)
        {
            if (stringDic.ContainsKey(name)) return stringDic[name];
            return new CustomSystemTypes(name,availableId++);
        }

        private CustomSystemTypes(string name,int id)
        {
            Name = name;
            Id = id;

            stringDic[name] = this;
            intDic[id] = this;
        }

        public string GetTranslatedString()
        {
            return "Default";
        }

        public static bool IsCustomSystemTypes(SystemTypes systemTypes)
        {
            return intDic.ContainsKey((int)systemTypes);
        }

        public static CustomSystemTypes? GetCustomSystemTypes(SystemTypes systemTypes)
        {
            if (IsCustomSystemTypes(systemTypes)) return intDic[(int)systemTypes];
            return null;
        }
    }

    public class CustomTaskTypes
    {
        static private Dictionary<string, TaskTypes> vanillaDic = new Dictionary<string, TaskTypes>();
        static private Dictionary<string, CustomTaskTypes> stringDic = new Dictionary<string, CustomTaskTypes>();
        static private Dictionary<int, CustomTaskTypes> intDic = new Dictionary<int, CustomTaskTypes>();
        static private int availableId = 128;

        private int Id;
        private string Name;

        public static void LoadVanillaTaskTypes()
        {
            foreach (var taskTypes in TaskTypesHelpers.AllTypes)
                vanillaDic[taskTypes.ToString()] = taskTypes;
        }

        public static CustomTaskTypes RegisterSystemTypes(string name)
        {
            if (stringDic.ContainsKey(name)) return stringDic[name];
            return new CustomTaskTypes(name, availableId++);
        }

        private CustomTaskTypes(string name, int id)
        {
            Name = name;
            Id = id;

            stringDic[name] = this;
            intDic[id] = this;
        }

        //TODO: ここに追加のSystemTypesを表示する文字列に変換するメソッドを作る
        public string GetTranslatedString()
        {
            return "Default Room";
        }

        public static bool IsCustomTaskTypes(TaskTypes taskTypes)
        {
            return intDic.ContainsKey((int)taskTypes);
        }

        public static CustomTaskTypes? GetCustomTaskTypes(TaskTypes taskTypes)
        {
            if (IsCustomTaskTypes(taskTypes)) return intDic[(int)taskTypes];
            return null;
        }
    }

    public class CustomStrings
    {
        static private Dictionary<string, CustomStrings> stringDic = new Dictionary<string, CustomStrings>();
        static private Dictionary<int, CustomStrings> intDic = new Dictionary<int, CustomStrings>();
        static private int availableId = 128;

        private int Id;
        private string Name;

        public static CustomStrings RegisterStrings(string name)
        {
            if (stringDic.ContainsKey(name)) return stringDic[name];
            return new CustomStrings(name, availableId++);
        }

        private CustomStrings(string name, int id)
        {
            Name = name;
            Id = id;

            stringDic[name] = this;
            intDic[id] = this;
        }

        //TODO: ここに追加のStringNamesを表示する文字列に変換するメソッドを作る
        public string GetTranslatedString()
        {
            return "Default";
        }

        public static bool IsCustomStrings(StringNames stringNames)
        {
            return intDic.ContainsKey((int)stringNames);
        }

        public static CustomStrings? GetCustomStrings(StringNames stringNames)
        {
            if (IsCustomStrings(stringNames)) return intDic[(int)stringNames];
            return null;
        }
    }
}

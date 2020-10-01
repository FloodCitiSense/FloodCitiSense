//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EnumHelper.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   EnumHelper.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions;
using IIASA.FloodCitiSense.Mobile.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Mobile.Core.Extensions
{
    public static class EnumHelper<T>
    {
        public static IDictionary<string, T> GetValues(bool ignoreCase)
        {
            var enumValues = new Dictionary<string, T>();

            foreach (FieldInfo fi in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                string key = fi.Name;

                var display = fi.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
                if (display != null)
                    key = (display.Length > 0) ? display[0].Name : fi.Name;

                if (ignoreCase)
                    key = key.ToLower();

                if (!enumValues.ContainsKey(key))
                    enumValues[key] = (T)fi.GetRawConstantValue();
            }

            return enumValues;
        }

        public static List<string> GetValues()
        {
            var enumValues = new List<string>();

            foreach (var value in Enum.GetValues(typeof(T)))
            {
                enumValues.Add(value.ToString());
            }

            return enumValues;
        }

        public static List<DisplayItem> GetDisplayItems()
        {
            var enumValues = new List<DisplayItem>();

            foreach (var value in Enum.GetValues(typeof(T)))
            {
                enumValues.Add(new DisplayItem
                {
                    Text = value.ToString().Translate(),
                    Value = value.ToString()
                });
            }
            return enumValues;
        }

        public static string GetEnumDescription(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            string result = value.ToString();

            //is there a resource entry?
            var match = result.Translate();
            if (!string.IsNullOrWhiteSpace(match))
                result = match;

            return result;
        }

        public static T ParseInt(int value)
        {
            T result;

            try
            {
                result = (T)Enum.ToObject(typeof(T), value);
            }
            catch (System.Exception)
            {
                result = default(T);
            }

            return result;
        }

        public static ImageSource GetImage(int value)
        {
            T result;

            try
            {
                result = (T)Enum.ToObject(typeof(T), value);

                var source = $"IIASA.FloodCitiSense.Mobile.Core.UI.Assets.Images.{result}.png";
                return ImageSource.FromResource(source);
            }
            catch (System.Exception)
            {
                result = default(T);
            }

            return ImageSource.FromResource($"IIASA.FloodCitiSense.Mobile.Core.UI.Assets.Images.error.png");
        }

        public static ImageSource GetImage(T value)
        {
            try
            {
                var source = $"IIASA.FloodCitiSense.Mobile.Core.UI.Assets.Images.{value}.png";
                return ImageSource.FromResource(source);
            }
            catch (System.Exception)
            {

            }

            return ImageSource.FromResource($"IIASA.FloodCitiSense.Mobile.Core.UI.Assets.Images.error.png");
        }

        public static T Parse(string value)
        {
            T result;

            try
            {
                result = (T)Enum.Parse(typeof(T), value, true);
            }
            catch (System.Exception)
            {
                result = ParseDisplayValues(value, true);
            }


            return result;
        }

        private static T ParseDisplayValues(string value, bool ignoreCase)
        {
            IDictionary<string, T> values = GetValues(ignoreCase);

            string key = null;
            if (ignoreCase)
                key = value.ToLower();
            else
                key = value;

            if (values.ContainsKey(key))
                return values[key];

            throw new ArgumentException(value);
        }
    }
}
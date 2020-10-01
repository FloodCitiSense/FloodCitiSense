// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStorageManager.cs" company="IIASA">
//   EOS
// </copyright>
// <summary>
//   Defines the DataStorageManager.cs type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Abp.Dependency;
using Abp.Runtime.Security;
using IIASA.FloodCitiSense.Mobile.Core.Core.Type;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage
{
    /// <summary>
    ///     Uses Xamarin.Forms Application Properties to save data.
    ///     If you need to store secure values such as password, use ISecureStorage.
    /// </summary>
    public class DataStorageManager : ISingletonDependency, IDataStorageManager
    {
        /// <summary>
        ///     The HasKey
        /// </summary>
        /// <param name="key">The key<see cref="string" /></param>
        /// <returns>The <see cref="bool" /></returns>
        public bool HasKey(string key)
        {
            return Application.Current != null && Application.Current.Properties.ContainsKey(key);
        }

        /// <summary>
        ///     The Retrieve
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key<see cref="string" /></param>
        /// <param name="defaultValue">The defaultValue<see cref="T" /></param>
        /// <param name="shouldDecrpyt">The shouldDecrpyt<see cref="bool" /></param>
        /// <returns>The <see cref="T" /></returns>
        public T Retrieve<T>(string key, T defaultValue = default(T), bool shouldDecrpyt = false)
        {
            var value = TypeHelperExtended.IsPrimitive(typeof(T), false)
                ? GetPrimitive(key, defaultValue)
                : RetrieveObject(key, defaultValue);

            if (!shouldDecrpyt) return value;

            var decrypted = SimpleStringCipher.Instance.Decrypt(Convert.ToString(value));
            return (T)Convert.ChangeType(decrypted, typeof(T));
        }

        public T Retrieve<T>(string key, JsonConverter converter, T defaultValue = default(T),
            bool shouldDecrpyt = false)
        {
            var value = TypeHelperExtended.IsPrimitive(typeof(T), false)
                ? GetPrimitive(key, defaultValue)
                : RetrieveObject(key, converter, defaultValue);

            if (!shouldDecrpyt) return value;

            var decrypted = SimpleStringCipher.Instance.Decrypt(Convert.ToString(value));
            return (T)Convert.ChangeType(decrypted, typeof(T));
        }

        /// <summary>
        ///     The StoreAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key<see cref="string" /></param>
        /// <param name="value">The value<see cref="T" /></param>
        /// <param name="shouldEncrypt">The shouldEncrypt<see cref="bool" /></param>
        /// <returns>The <see cref="Task" /></returns>
        public async Task StoreAsync<T>(string key, T value, bool shouldEncrypt = false)
        {
            if (TypeHelperExtended.IsPrimitive(typeof(T), false))
            {
                if (shouldEncrypt)
                    StorePrimitive(key, SimpleStringCipher.Instance.Encrypt(Convert.ToString(value)));
                else
                    StorePrimitive(key, value);
            }
            else
            {
                StoreObject(key, value);
            }

            if (Application.Current != null) await Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        ///     The RemoveIfExists
        /// </summary>
        /// <param name="key">The key<see cref="string" /></param>
        public void RemoveIfExists(string key)
        {
            if (HasKey(key))
            {
                Application.Current?.Properties.Remove(key);
                Application.Current?.SavePropertiesAsync();
            }
        }

        /// <summary>
        ///     The StorePrimitive
        /// </summary>
        /// <param name="key">The key<see cref="string" /></param>
        /// <param name="value">The value<see cref="object" /></param>
        private static void StorePrimitive(string key, object value)
        {
            Application.Current.Properties[key] = value;
        }

        /// <summary>
        ///     The StoreObject
        /// </summary>
        /// <param name="key">The key<see cref="string" /></param>
        /// <param name="value">The value<see cref="object" /></param>
        private static void StoreObject(string key, object value)
        {
            if (Application.Current != null) Application.Current.Properties[key] = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///     The GetPrimitive
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key<see cref="string" /></param>
        /// <param name="defaultValue">The defaultValue<see cref="T" /></param>
        /// <returns>The <see cref="T" /></returns>
        private T GetPrimitive<T>(string key, T defaultValue = default(T))
        {
            if (!HasKey(key)) return defaultValue;

            return (T)Convert.ChangeType(Application.Current.Properties[key], typeof(T));
        }

        /// <summary>
        ///     The RetrieveObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key<see cref="string" /></param>
        /// <param name="defaultValue">The defaultValue<see cref="T" /></param>
        /// <returns>The <see cref="T" /></returns>
        private T RetrieveObject<T>(string key, T defaultValue = default(T))
        {
            return !HasKey(key)
                ? defaultValue
                : JsonConvert.DeserializeObject<T>(Convert.ToString(Application.Current.Properties[key]));
        }

        private T RetrieveObject<T>(string key, JsonConverter converter, T defaultValue = default(T))
        {
            return !HasKey(key)
                ? defaultValue
                : JsonConvert.DeserializeObject<T>(Convert.ToString(Application.Current.Properties[key]), converter);
        }
    }
}
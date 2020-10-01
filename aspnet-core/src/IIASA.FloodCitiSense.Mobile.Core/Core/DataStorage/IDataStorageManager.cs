// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataStorageManager.cs" company="IIASA">
//   EOS
// </copyright>
// <summary>
//   Defines the IDataStorageManager.cs type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace IIASA.FloodCitiSense.Mobile.Core.Core.DataStorage
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IDataStorageManager" />
    /// </summary>
    public interface IDataStorageManager
    {
        /// <summary>
        /// The HasKey
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        bool HasKey(string key);

        /// <summary>
        /// The Retrieve
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="defaultValue">The defaultValue<see cref="T"/></param>
        /// <param name="shouldDecrpyt">The shouldDecrpyt<see cref="bool"/></param>
        /// <returns>The <see cref="T"/></returns>
        T Retrieve<T>(string key, T defaultValue = default(T), bool shouldDecrpyt = false);

        T Retrieve<T>(string key, JsonConverter converter, T defaultValue = default(T), bool shouldDecrpyt = false);
        /// <summary>
        /// The StoreAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="value">The value<see cref="T"/></param>
        /// <param name="shouldEncrypt">The shouldEncrypt<see cref="bool"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task StoreAsync<T>(string key, T value, bool shouldEncrypt = false);

        /// <summary>
        /// The RemoveIfExists
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        void RemoveIfExists(string key);
    }
}

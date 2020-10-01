//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TranslateStringExtension.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   TranslateStringExtension.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using IIASA.FloodCitiSense.Mobile.Core.Localization;

namespace IIASA.FloodCitiSense.Mobile.Core.Extensions.MarkupExtensions
{
    /// <summary>
    /// The translate string extension.
    /// </summary>
    public static class TranslateStringExtension
    {
        /// <summary>
        /// The translate.
        /// </summary>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Translate(this string str)
        {
            return str == null ? string.Empty : L.Localize(str);
        }
    }
}
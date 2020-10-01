//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ITokenService.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   ITokenService.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Token
{
    public interface ITokenService
    {
        Task<string> GetAccessToken();
    }
}
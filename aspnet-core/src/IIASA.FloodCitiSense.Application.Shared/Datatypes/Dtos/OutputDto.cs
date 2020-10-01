//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="OutputDto.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   OutputDto.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using Abp.Application.Services.Dto;

namespace IIASA.FloodCitiSense.Datatypes.Dtos
{
    public class OutputDto : EntityDto
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
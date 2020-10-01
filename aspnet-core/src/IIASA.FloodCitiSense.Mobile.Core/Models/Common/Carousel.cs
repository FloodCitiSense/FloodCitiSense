//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Carousel.cs" company="IIASA">
//    EOS
//  </copyright>
//  <summary>
//   Carousel.cs
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Mobile.Core.Models.Common
{
    public class Carousel
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string ButtonText { get; set; }
        public string ImageSource { get; set; }



        //Note:
        // This should normally be in a service or sit in the view-model. 

        public static IList<Carousel> All { set; get; }
        static Carousel()
        {

        }
    }
}
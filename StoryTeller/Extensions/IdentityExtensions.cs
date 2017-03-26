//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Security.Principal;
//using System.Web;

//namespace StoryTeller.Extensions
//{
//    public static class IdentityExtensions
//    {
//        public static string GetIsWritting(this IIdentity identity)
//        {
//            var claim = ((ClaimsIdentity)identity).FindFirst("isWritting");
//            return (claim != null) ? claim.Value : string.Empty;
//        }
//    }
//}
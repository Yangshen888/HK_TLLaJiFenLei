using System;
using System.Security.Claims;
using HaikanRefuseClassification.Api.Entities;
using Microsoft.AspNetCore.Http;

namespace HaikanRefuseClassification.Api.Extensions.AuthContext
{
    public static class AuthContextService
    {
        private static IHttpContextAccessor _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;
        }
        /// <summary>
        /// 
        /// </summary>
        public static HttpContext Current => _context.HttpContext;
        /// <summary>
        /// 
        /// </summary>
        public static AuthContextUser CurrentUser
        {
            get
            {
                var user = new AuthContextUser
                {
                    LoginName = Current.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    DisplayName = Current.User.FindFirstValue("displayName"),
                    EmailAddress = Current.User.FindFirstValue("emailAddress"),
                    UserType = Current.User.FindFirstValue("userType").ToString(),
                    Avator = Current.User.FindFirstValue("avator"),
                    Guid = new Guid(Current.User.FindFirstValue("guid")),
                    Roleid = Current.User.FindFirstValue("roleid"),
                    RoleName = Current.User.FindFirstValue("roleName"),
                    ZYZ = Current.User.FindFirstValue("ZYZ"),
                    YH = Current.User.FindFirstValue("YH"),
                    DDY = Current.User.FindFirstValue("DDY"),
                    SJ = Current.User.FindFirstValue("SJ"),
                    Vuuid = Current.User.FindFirstValue("Vuuid"),
                    Streets = Current.User.FindFirstValue("Streets"),
                    Biotope = Current.User.FindFirstValue("Biotope"),
                    Community = Current.User.FindFirstValue("Community"),
                };
                return user;
            }
        }

        /// <summary>
        /// 是否已授权
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                return Current.User.Identity.IsAuthenticated;
            }
        }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public static bool IsSupperAdministator
        {
            get
            {
                return (Current.User.FindFirstValue("userType").ToString() == "1");
            }
        }
    }
}


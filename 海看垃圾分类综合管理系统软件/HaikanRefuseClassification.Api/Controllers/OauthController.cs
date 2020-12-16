﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Haikan3.Utils;
//using Haikan3.Auth;
using HaikanRefuseClassification.Api.Auth;
using HaikanRefuseClassification.Api.Configurations;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Entities.Enums;
using HaikanRefuseClassification.Api.Entities.QueryModels.SystemPermission;
using HaikanRefuseClassification.Api.Entities.User;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.ViewModels.Rbac.SystemUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HaikanRefuseClassification.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OauthController : ControllerBase
    {
        private readonly AppAuthenticationSettings _appSettings;
        private readonly RefuseClassificationContext _dbContext;
        private readonly MdDesEncrypt MdDesEncrypt;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSettings"></param>
        public OauthController(IOptions<AppAuthenticationSettings> appSettings, RefuseClassificationContext dbContext, IOptions<MdDesEncrypt> mdDesEncrypt)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
            MdDesEncrypt = mdDesEncrypt.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Auth(UserData userdata)
        {
            var response = ResponseModelFactory.CreateInstance;
            SystemUser user;
            using (_dbContext)
            {
                user = _dbContext.SystemUser.FirstOrDefault(x => x.LoginName == userdata.username.Trim());
                if (user == null || user.IsDeleted == 1)
                {
                    response.SetFailed("用户不存在");
                    return Ok(response);
                }
                string s = Haikan3.Utils.DesEncrypt.Encrypt(userdata.password.Trim(), MdDesEncrypt.SecretKey);
                if (user.PassWord != Haikan3.Utils.DesEncrypt.Encrypt(userdata.password.Trim(), MdDesEncrypt.SecretKey))
                {
                    response.SetFailed("密码不正确");
                    return Ok(response);
                }
                //if (user.IsLocked == CommonEnum.IsLocked.Locked)
                //{
                //    response.SetFailed("账号已被锁定");
                //    return Ok(response);
                //}
                //if (user.Status == UserStatus.Forbidden)
                //{
                //    response.SetFailed("账号已被禁用");
                //    return Ok(response);
                //}

                //获取权限名
                string[] roleid = user.SystemRoleUuid.TrimEnd(',').Split(",");
                string rolename = "";
                for (int i = 0; i < roleid.Length; i++)
                {
                    if (!string.IsNullOrEmpty(roleid[i]))
                        rolename += _dbContext.SystemRole.FirstOrDefault(x => x.SystemRoleUuid == Guid.Parse(roleid[i])).RoleName + ",";
                }
                string zyz = "";
                string yh = "";
                string ddy = "";
                string sj = "";
                string vuuid = "";
                //志愿者roleid
                var temp1 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("志愿者")).Select(x => new { x.SystemRoleUuid }).ToList();
                if (temp1.Count > 0)
                    zyz = temp1[0].SystemRoleUuid.ToString();

                //普通用户roleid
                var temp2 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("用户")).Select(x => new { x.SystemRoleUuid }).ToList();
                if (temp2.Count > 0)
                    yh = temp2[0].SystemRoleUuid.ToString();

                //督导员roleid
                var temp3 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("督导员")).Select(x => new { x.SystemRoleUuid }).ToList();
                if (temp3.Count > 0)
                    ddy = temp3[0].SystemRoleUuid.ToString();

                //社区管理员Vuuid
                //var temp6 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("社区")).Select(x => new { x.SystemRoleUuid }).ToList();
                if (rolename.Contains("社区管理员"))
                    vuuid = user.VillageId.ToString();

                //商户
                var temp4 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("商")).Select(x => new { x.SystemRoleUuid }).ToList();
                if (temp4.Count > 0)
                    sj = temp4[0].SystemRoleUuid.ToString();
                string superAdmin = "";

                //超管roleid
                var temp5 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("超级")).Select(x => new { x.SystemRoleUuid }).ToList();
                if (temp5.Count > 0)
                    superAdmin = temp5[0].SystemRoleUuid.ToString();
                int usertype = 0;
                if (!user.SystemRoleUuid.Contains(superAdmin))
                {
                    usertype = 2;
                }
                var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userdata.username),
                    new Claim("guid",user.SystemUserUuid.ToString()),
                    new Claim("avatar",""),
                    new Claim("displayName",user.RealName),
                    new Claim("loginName",user.LoginName),
                    new Claim("streets",user.Streets==null?"":user.Streets),
                    new Claim("community",user.Community==null?"":user.Community),
                    new Claim("biotope",user.Biotope==null?"":user.Biotope),
                    new Claim("Vuuid",(vuuid)),
                    new Claim("emailAddress",""),
                    //new Claim("guid",user.SystemUserUuid.ToString()),
                    new Claim("userType",usertype.ToString()),
                    new Claim("roleid",(user.SystemRoleUuid.TrimEnd(','))),
                    new Claim("roleName",(rolename.TrimEnd(','))),
                    new Claim("ZYZ",(zyz)),
                    new Claim("YH",(yh)),
                    new Claim("DDY",(ddy)),
                    new Claim("SJ",(sj))
                });
                var token = JwtBearerAuthenticationExtension.GetJwtAccessToken(_appSettings, claimsIdentity);

                response.SetData(token);
                return Ok(response);
            }
        }

        /// <summary>
        /// 微信端--手机号与密码登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult WXAuth2(UserData userdata)
        {
            var response = ResponseModelFactory.CreateInstance;
            SystemUser user;
            using (_dbContext)
            {
                user = _dbContext.SystemUser.FirstOrDefault(x => x.Phone == userdata.username.Trim());
                if (user == null || user.IsDeleted == 1)
                {
                    response.SetFailed("用户不存在");
                    return Ok(response);
                }
                if (user.PassWord != Haikan3.Utils.DesEncrypt.Encrypt(userdata.password.Trim(), MdDesEncrypt.SecretKey))
                {
                    response.SetFailed("密码不正确");
                    return Ok(response);
                }
                //获取权限名
                string[] roleid = user.SystemRoleUuid.TrimEnd(',').Split(",");
                string rolename = "";
                for (int i = 0; i < roleid.Length; i++)
                {
                    if (!string.IsNullOrEmpty(roleid[i]))
                        rolename += _dbContext.SystemRole.FirstOrDefault(x => x.SystemRoleUuid == Guid.Parse(roleid[i])).RoleName + ",";
                }
                string zyz = "";
                string yh = "";
                string ddy = "";
                string sj = "";

                var temp1 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("志愿者")).Select(x => new { x.SystemRoleUuid }).ToList();
                if (temp1.Count > 0)
                    zyz = temp1[0].SystemRoleUuid.ToString();
                var temp2 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("用户")).Select(x => new { x.SystemRoleUuid }).ToList();
                if (temp2.Count > 0)
                    yh = temp2[0].SystemRoleUuid.ToString();
                var temp3 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("督导员")).Select(x => new { x.SystemRoleUuid }).ToList();
                if (temp3.Count > 0)
                    ddy = temp3[0].SystemRoleUuid.ToString();
                var temp4 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("商")).Select(x => new { x.SystemRoleUuid }).ToList();
                if (temp4.Count > 0)
                    sj = temp4[0].SystemRoleUuid.ToString();

                var claimsIdentity = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, userdata.username),
                    new Claim("guid",user.SystemUserUuid.ToString()),
                    new Claim("avatar",""),
                    new Claim("displayName",user.RealName),
                    new Claim("loginName",user.LoginName),
                    new Claim("emailAddress",""),
                    //new Claim("guid",user.SystemUserUuid.ToString()),
                    new Claim("userType",((int)user.UserType).ToString()),
                    new Claim("roleid",(user.SystemRoleUuid.TrimEnd(','))),
                    new Claim("roleName",(rolename.TrimEnd(','))),
                    new Claim("ZYZ",(zyz)),
                    new Claim("YH",(yh)),
                    new Claim("DDY",(ddy)),
                    new Claim("SJ",(sj))
                    });
                var token = JwtBearerAuthenticationExtension.GetJwtAccessToken(_appSettings, claimsIdentity);

                response.SetData(token);
                return Ok(response);
            }
        }

        /// <summary>
        /// 微信授权登录
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult WXAuth(WXUserInfo info)
        {
            var response = ResponseModelFactory.CreateInstance;
            SystemUser user = new SystemUser();
            //string result= EWM.AES_decrypt(info.EncryptedData,info.Session_key,info.Iv);
            //return Ok(response);
            using (_dbContext)
            {
                var ddyuuid= _dbContext.SystemRole.FirstOrDefault(x => x.RoleName == "督导员").SystemRoleUuid.ToString();
                var entity = _dbContext.SystemUser.FirstOrDefault(x=>x.Wechat == info.Openid);
                var ent = _dbContext.SystemUser.FirstOrDefault(x => x.Phone == info.Phone);
                var ddyent= _dbContext.SystemUser.FirstOrDefault(x => x.Phone == info.Phone&&x.SystemRoleUuid==ddyuuid&&x.Wechat==null);
                //督导员授权登录
                if (ddyent != null)
                {
                    ddyent.Wechat = info.Openid;
                    ddyent.UserType = 5;
                    ddyent.LoginName = info.NickName;
                    if (info.Sex == 0)
                    {
                        ddyent.Sex = "未知";
                    }
                    if (info.Sex == 1)
                    {
                        ddyent.Sex = "男";
                    }
                    if (info.Sex == 2)
                    {
                        ddyent.Sex = "女";
                    }
                    ddyent.Phone = info.Phone;
                    var jtroleuuid= _dbContext.SystemRole.FirstOrDefault(x => x.RoleName == "家庭用户").SystemRoleUuid.ToString();
                    ddyent.SystemRoleUuid =ddyuuid+","+jtroleuuid;
                    _dbContext.SaveChanges();
                    response.SetSuccess("授权成功");
                    return Ok(response);

                }
                if (entity==null && ent==null)
                {
                    user.SystemUserUuid = Guid.NewGuid();
                    user.LoginName = info.NickName;
                    user.RealName = "";
                    user.Wechat = info.Openid;
                    //授权登录的家庭用户
                    user.UserType = 5;
                    user.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                    if (info.Sex == 0)
                    {
                        user.Sex = "未知";
                    }
                    if (info.Sex == 1)
                    {
                        user.Sex = "男";
                    }
                    if (info.Sex == 2)
                    {
                        user.Sex = "女";
                    }
                        user.Phone = info.Phone;
                        user.SystemRoleUuid = _dbContext.SystemRole.FirstOrDefault(x => x.RoleName == "家庭用户").SystemRoleUuid.ToString();
                    
                    user.IsDeleted = 0;
                    _dbContext.SystemUser.Add(user);
                }
                else if (ent!=null && entity==null)
                {

                    user.LoginName = info.NickName;
                    user.Wechat = info.Openid;
                    //授权登录的家庭用户
                    user.UserType = 5;
                    user.Phone = info.Phone;
                    if (info.Sex == 0)
                    {
                        user.Sex = "未知";
                    }
                    if (info.Sex == 1)
                    {
                        user.Sex = "男";
                    }
                    if (info.Sex == 2)
                    {
                        user.Sex = "女";
                    }
                    user.SystemRoleUuid = AuthContextService.CurrentUser.YH + "," + user.SystemRoleUuid;
                    _dbContext.SystemUser.Add(user);
                }
                else
                {
                    entity.LoginName = info.NickName;
                    //if (info.Sex == 0)
                    //{
                    //    entity.Sex = "未知";
                    //}
                    //if (info.Sex == 1)
                    //{
                    //    entity.Sex = "男";
                    //}
                    //if (info.Sex == 2)
                    //{
                    //    entity.Sex = "女";
                    //}
                    entity.IsDeleted = 0;
                }

                _dbContext.SaveChanges();

                response.SetSuccess("授权成功");
                return Ok(response);
            }
        }

        [HttpPost]
        public IActionResult WXPhone(WXUserInfo info)
        {
            var response = ResponseModelFactory.CreateInstance;
            string result = EWM.AES_decrypt(info.EncryptedData, info.Session_key, info.Iv);
            UserPhoneInfoModel model = JsonConvert.DeserializeObject<UserPhoneInfoModel>(result);

            if (model == null)
            {
                response.SetFailed();
            }
            else
            {
                response.SetData(model.purePhoneNumber);
            }
            return Ok(response);
        }


        /// <summary>
        /// 微信端--手机号与密码登录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult WXOpenAuth(string openid)
        {
            var response = ResponseModelFactory.CreateInstance;
            SystemUser user;
            using (_dbContext)
            {
                user = _dbContext.SystemUser.FirstOrDefault(x => x.Wechat == openid);
                if (user == null)
                {
                    response.SetFailed("需要微信授权登录！");
                    return Ok(response);
                }
                else
                {
                    //获取权限名
                    string[] roleid = user.SystemRoleUuid.TrimEnd(',').Split(",");
                    string rolename = "";
                    for (int i = 0; i < roleid.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(roleid[i]))
                            rolename += _dbContext.SystemRole.FirstOrDefault(x => x.SystemRoleUuid == Guid.Parse(roleid[i])).RoleName + ",";
                    }
                    string zyz = "";
                    string yh = "";
                    string ddy = "";
                    string sj = "";

                    var temp1 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("志愿者")).Select(x => new { x.SystemRoleUuid }).ToList();
                    if (temp1.Count > 0)
                        zyz = temp1[0].SystemRoleUuid.ToString();
                    var temp2 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("用户")).Select(x => new { x.SystemRoleUuid }).ToList();
                    if (temp2.Count > 0)
                        yh = temp2[0].SystemRoleUuid.ToString();
                    var temp3 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("督导员")).Select(x => new { x.SystemRoleUuid }).ToList();
                    if (temp3.Count > 0)
                        ddy = temp3[0].SystemRoleUuid.ToString();
                    var temp4 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("商")).Select(x => new { x.SystemRoleUuid }).ToList();
                    if (temp4.Count > 0)
                        sj = temp4[0].SystemRoleUuid.ToString();

                    var claimsIdentity = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Name, user.LoginName),
                    new Claim("guid",user.SystemUserUuid.ToString()),
                    new Claim("avatar",""),
                    new Claim("displayName",user.RealName),
                    new Claim("loginName",user.LoginName),
                    new Claim("emailAddress",""),
                    //new Claim("guid",user.SystemUserUuid.ToString()),
                    new Claim("userType",((int)user.UserType).ToString()),
                    new Claim("roleid",(user.SystemRoleUuid.TrimEnd(','))),
                    new Claim("roleName",(rolename.TrimEnd(','))),
                    new Claim("ZYZ",(zyz)),
                    new Claim("YH",(yh)),
                    new Claim("DDY",(ddy)),
                    new Claim("SJ",(sj))
                        });
                    var token = JwtBearerAuthenticationExtension.GetJwtAccessToken(_appSettings, claimsIdentity);
                    //查询当前登录用户拥有的权限集合(非超级管理员)
                    var sqlPermission = @"SELECT P.SystemPermissionUUID AS PermissionCode,P.ActionCode AS PermissionActionCode,P.Name AS PermissionName,P.Type AS PermissionType,M.Name AS MenuName,M.SystemMenuUUID AS MenuGuid,M.Alias AS MenuAlias,M.IsDefaultRouter FROM SystemRolePermissionMapping AS RPM 
LEFT JOIN SystemPermission AS P ON P.SystemPermissionUUID = RPM.SystemPermissionUUID
INNER JOIN SystemMenu AS M ON M.SystemMenuUUID = P.SystemMenuUUID
WHERE P.IsDeleted=0 AND P.Status=1 AND EXISTS (SELECT 1 FROM SystemUserRoleMapping AS URM WHERE URM.SystemUserUUID={0} AND URM.SystemRoleUUID=RPM.SystemRoleUUID)";
                    List<SystemPermissionWithMenu> permissions = new List<SystemPermissionWithMenu>();
                    if (user.UserType == 0)
                    {
                        //如果是超级管理员
                        permissions = _dbContext.ViewSystemPermissionWithMenu2.Where(x => x.Pd == 0 && x.Ps == 1).Select(x => new SystemPermissionWithMenu
                        {
                            PermissionCode = x.PermissionCode,
                            PermissionActionCode = x.PermissionActionCode,
                            PermissionName = x.PermissionName,
                            PermissionType = (Entities.Enums.CommonEnum.PermissionType)x.PermissionType,
                            MenuName = x.MenuName,
                            MenuGuid = x.MenuGuid,
                            MenuAlias = x.MenuAlias,
                            IsDefaultRouter = (Entities.Enums.CommonEnum.YesOrNo)x.IsDefaultRouter,
                        }).ToList();
                    }
                      permissions = _dbContext.ViewSystemPermissionWithMenu.Where(x => x.Pd == 0 && x.Ps == 1 && _dbContext.SystemUserRoleMapping.Any(y => y.SystemUserUuid == user.SystemUserUuid && y.SystemRoleUuid == x.SystemRoleUuid)).Select(x => new SystemPermissionWithMenu
                    {
                        PermissionCode = x.PermissionCode.Value,
                        PermissionActionCode = x.PermissionActionCode,
                        PermissionName = x.PermissionName,
                        PermissionType = (Entities.Enums.CommonEnum.PermissionType)x.PermissionType,
                        MenuName = x.MenuName,
                        MenuGuid = x.MenuGuid,
                        MenuAlias = x.MenuAlias,
                        IsDefaultRouter = (Entities.Enums.CommonEnum.YesOrNo)x.IsDefaultRouter,
                    }).ToList();

                    var pagePermissions = permissions.GroupBy(x => x.MenuAlias).ToDictionary(g => g.Key, g => g.Select(x => x.PermissionActionCode).Distinct());
                    response.SetData(new
                    {
                        access = new string[] { },
                        user_guid = user.SystemUserUuid,
                        user_name = user.RealName,
                        nickName = user.LoginName,
                        user_type = user.UserType,
                        permissions = pagePermissions,
                        roleName = GetroleName(user.SystemRoleUuid),
                        address = user.Address,
                        tokens = token,
                        phone=user.Phone,
                        shop_guid = user.ShopUuid,
                        HomeAddressUUID = user.HomeAddressUuid,
                        openid,
                        idCard=user.UserIdCard,
                    });
                }
                return Ok(response);
            }
        }
        private string GetroleName(string ids)
        {
            string s = "";
            string[] temp = ids.TrimEnd(',').Split(',');
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    if (!string.IsNullOrEmpty(temp[i]))
                    {
                        var qu = cc.SystemRole.Where(x => x.SystemRoleUuid == Guid.Parse(temp[i])).Select(x => new { x.RoleName }).ToList();
                        if (!string.IsNullOrEmpty(qu[0].RoleName))
                            s += qu[0].RoleName + ",";
                    }
                }
                return s.TrimEnd(',');
            }

        }

        /// <summary>
        /// h5--手机号与密码登录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult h5OpenAuth(string phone)
        {
            var response = ResponseModelFactory.CreateInstance;
            SystemUser user;
            using (_dbContext)
            {
                user = _dbContext.SystemUser.FirstOrDefault(x => x.Phone == phone);
                if (user == null)
                {
                    response.SetFailed("需要微信小程序授权登录！");
                    return Ok(response);
                }
                else
                {
                    //获取权限名
                    string[] roleid = user.SystemRoleUuid.TrimEnd(',').Split(",");
                    string rolename = "";
                    for (int i = 0; i < roleid.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(roleid[i]))
                            rolename += _dbContext.SystemRole.FirstOrDefault(x => x.SystemRoleUuid == Guid.Parse(roleid[i])).RoleName + ",";
                    }
                    string zyz = "";
                    string yh = "";
                    string ddy = "";
                    string sj = "";

                    var temp1 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("志愿者")).Select(x => new { x.SystemRoleUuid }).ToList();
                    if (temp1.Count > 0)
                        zyz = temp1[0].SystemRoleUuid.ToString();
                    var temp2 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("用户")).Select(x => new { x.SystemRoleUuid }).ToList();
                    if (temp2.Count > 0)
                        yh = temp2[0].SystemRoleUuid.ToString();
                    var temp3 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("督导员")).Select(x => new { x.SystemRoleUuid }).ToList();
                    if (temp3.Count > 0)
                        ddy = temp3[0].SystemRoleUuid.ToString();
                    var temp4 = _dbContext.SystemRole.Where(x => x.RoleName.Contains("商")).Select(x => new { x.SystemRoleUuid }).ToList();
                    if (temp4.Count > 0)
                        sj = temp4[0].SystemRoleUuid.ToString();

                    var claimsIdentity = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Name, user.LoginName),
                    new Claim("guid",user.SystemUserUuid.ToString()),
                    new Claim("avatar",""),
                    new Claim("displayName",user.RealName),
                    new Claim("loginName",user.LoginName),
                    new Claim("emailAddress",""),
                    //new Claim("guid",user.SystemUserUuid.ToString()),
                    new Claim("userType",((int)user.UserType).ToString()),
                    new Claim("roleid",(user.SystemRoleUuid.TrimEnd(','))),
                    new Claim("roleName",(rolename.TrimEnd(','))),
                    new Claim("ZYZ",(zyz)),
                    new Claim("YH",(yh)),
                    new Claim("DDY",(ddy)),
                    new Claim("SJ",(sj))
                        });
                    var token = JwtBearerAuthenticationExtension.GetJwtAccessToken(_appSettings, claimsIdentity);
                    //查询当前登录用户拥有的权限集合(非超级管理员)
                    var sqlPermission = @"SELECT P.SystemPermissionUUID AS PermissionCode,P.ActionCode AS PermissionActionCode,P.Name AS PermissionName,P.Type AS PermissionType,M.Name AS MenuName,M.SystemMenuUUID AS MenuGuid,M.Alias AS MenuAlias,M.IsDefaultRouter FROM SystemRolePermissionMapping AS RPM 
LEFT JOIN SystemPermission AS P ON P.SystemPermissionUUID = RPM.SystemPermissionUUID
INNER JOIN SystemMenu AS M ON M.SystemMenuUUID = P.SystemMenuUUID
WHERE P.IsDeleted=0 AND P.Status=1 AND EXISTS (SELECT 1 FROM SystemUserRoleMapping AS URM WHERE URM.SystemUserUUID={0} AND URM.SystemRoleUUID=RPM.SystemRoleUUID)";
                    if (user.UserType == 0)
                    {
                        //如果是超级管理员
                        sqlPermission = @"SELECT P.SystemPermissionUUID AS PermissionCode,P.ActionCode AS PermissionActionCode,P.Name AS PermissionName,P.Type AS PermissionType,M.Name AS MenuName,M.SystemMenuUUID AS MenuGuid,M.Alias AS MenuAlias,M.IsDefaultRouter FROM SystemPermission AS P 
INNER JOIN SystemMenu AS M ON M.SystemMenuUUID = P.SystemMenuUUID
WHERE P.IsDeleted=0 AND P.Status=1";
                    }
                    List<SystemPermissionWithMenu> permissions = new List<SystemPermissionWithMenu>();
                    //var permissions = _dbContext.SystemPermissionWithMenu.FromSql(sqlPermission, user.SystemUserUuid.ToString()).ToList();

                    var pagePermissions = permissions.GroupBy(x => x.MenuAlias).ToDictionary(g => g.Key, g => g.Select(x => x.PermissionActionCode).Distinct());
                    response.SetData(new
                    {
                        access = new string[] { },
                        user_guid = user.SystemUserUuid,
                        user_name = user.RealName,
                        user_type = user.UserType,
                        permissions = pagePermissions,
                        roleName = GetroleName(user.SystemRoleUuid),
                        address = user.Address,
                        tokens = token,
                        phone = user.Phone,
                        shop_guid = user.ShopUuid,
                        HomeAddressUUID = user.HomeAddressUuid,
                        openid=user.Wechat,
                        idCard = user.UserIdCard,
                    });
                }
                return Ok(response);
            }
        }
    }
}
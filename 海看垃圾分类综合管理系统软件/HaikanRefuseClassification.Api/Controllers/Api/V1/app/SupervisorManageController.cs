using AutoMapper;
using Haikan3.Utils;
using HaikanRefuseClassification.Api.Configurations;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using HaikanRefuseClassification.Api.ViewModels.App;
using HaikanRefuseClassification.Api.ViewModels.Person;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.app
{
    [Route("api/v1/app/[controller]/[action]")]
    [ApiController]
    //[CustomAuthorize]
    public class SupervisorManageController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MdDesEncrypt MdDesEncrypt;
        private IHostingEnvironment _hostingEnvironment;
        private readonly string private_key = "-----BEGIN RSA PRIVATE KEY-----MIIEpQIBAAKCAQEAvG6zZy9Nqc0mhuo3oxYsjo3+j6ZPdzgG0ToLPcgRyBTyN5zXILTavrwRnbvhrZn/MVO968IMHKGGZdZox0NHNvMQWMpPG9XhSWZOuo2xER4welFDkMBGgrAe9B1lXpTPVtnOJxfUeTcZvRFL4BYOjfdb7R1m0aqBfJ6Z7yQGJvonzmhyGyhh1LDKNBxoPv5KCWxClqaGYRwmVAcxSEfMiMjLLqcomha74m8JfND1Zf9hqkKULCpA2AqPSKmcoZ+P2I1s0VFiaoULlxu73tYPQKqNrwaUKUs/RY+X5PMgBcbur3TgbeEu0WCxZyV5HPwT8j86MSFsuja08CO3uz3dLwIDAQABAoIBAHVZQ6TpEqbCulGfH5MTRiBpUbVIT4jhfW1jhoitRlWipc34gQk/WFMccKQY08z3cLPZgReHu19BQJ+/TXV68qjH6tBA/c9J/Ylmi3UmtLUCZhJm9Xr6I29UG5LPk8e5SP6/meFCt/HdXMwgL26YjevftoIOo2/Djex8IUWK28H9SUEPak5JUg+l741r55XGZEhnoAY/vhcghweU0DJajK8Wi3UxRVUXBuf5ls00dGYMZCZAL5+TxjQwwqizqDiZ0RM1eUFwz9aJBGjw6sCZeLc37KoNv5Q5pfa80JTy0u7uzI2AxwVfQbC2lxA9dRkSx4cxLyATsmPatSVB5bc32TECgYEA8w50C6x6/OybhC7kKls5tF3H5DLAD307ueGUQX2BJZ5VJaAhAOtsnEHMsz/R7NKY70rjGKE47n++CEEbneBgqBdqawzOToWpeqafjoiKzzIbnuNBwiAQmp68/UZ9ZV2Ls8fZpq/1OqNQIbV6O9Ecp4najMY0P1s+v/PIbDJ6f5MCgYEAxneRRpiA9WcvqPolZ+Q1BccQvWRkFcarLtBvWay6Cb7QQss5LWaBTZXEbbRWuTowlBnsX4G9FjsZ8RxqH77v8Dc3ehp0l9Sx2Vw8YGjfCzbV3IGDmlAGFGKP3tBEIMkDK8YdvMurQQqmMjFrx6YR7LpUtLxi/oglYAewxMWblXUCgYEAgl8hTdWxjpMXg9pnFnUiSaX3/2ZdcLF65OSj0lEQge4gu/LdYRHmixYcR5WW85Gu6MPhdiecUwmAFAtgVdmx3tfYhB01WBcH5jsT4K9KzYKSIDLD5e2vGlDFDJHP1xxLQB6Vl3xQbKiG5d3i98zdstwVt2blRYqa6PlJawfUfzMCgYEAiQgRdI7jq409aQyeOydkPML/meTw/eAYXdBosaADK6tmHFg4+FHoQWuBHsX/gxDcbcWgYSkxJ2JTPRkDZTvuawuU8GfHzPV8frmirmZ6akHIU+HQvgE20WhkMdHW2FQyLk0yRyLQ8a2qpslcw5K0maDlz4yrRVc3hyCIOrS+AekCgYEAqVbuQeWqW2q9EGbsPwa9/woI7aHEIdAx+Wfn50PM7a1KK5+oHpv2pVFcQND5gqsN5kcJ7WRO3i8KxR6y/xT1CDNsjA97CJ7/LGo4aLzvOs7zlBBaQpzET6f7r6+oed1iVE1PQi7KQSGJBcCsbPY9YFJH4KZCGIXYiWgUP6pe27Q=-----END RSA PRIVATE KEY-----";
        private readonly string public_key = "-----BEGIN PUBLIC KEY-----MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAjGpnmKByseFngibUJiNIc4" +
                "/6wfIQHMOHAsUNe1EACJUrFDFat78IqCJZIsYZ91G8He+88rBQL9+zK7DoubJsHPb6JTHE8krdFN1u/oaEDQz" +
                "/k5YNff89byrAdjsa3g5GcRU4d1/1D1TpkSVk0BTojtAgCgyNfRPqxB95gF" +
                "+Bsu2LQB1qMojSVoUx7rbSMofexahjqCWBqIryDYskegjNZhcfh0fiIZd/hK7bZDILN" +
                "+gHhi60H5vvAEe9M84GYo4yszW51qt8Blf2u5SZ0WkiAsh4+AMFdoRpBDP7MGsmWIYLkylSYy2QoSM4nyaEFZXp3qk" +
                "/zI9k2HSB/XuW8FwFXQIDAQAB-----END PUBLIC KEY-----";

        public SupervisorManageController(RefuseClassificationContext dbContext, IOptions<MdDesEncrypt> mdDesEncrypt, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            MdDesEncrypt = mdDesEncrypt.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 通过老年卡号，手机号获取用户信息
        /// </summary>
        /// <param name="idcard"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult getOneUser(Guid guid)
        {

            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.HomeAddress.FirstOrDefault(x => x.HomeAddressUuid == guid);
                AddScore score = new AddScore();
                if (entity != null)
                {
                    score.Address = entity.Address;
                    score.HomeAddressUuid = entity.HomeAddressUuid;
                    if (entity.Score < 0)
                        entity.Score = 0;
                    score.Score = entity.Score.ToString();
                    response.SetData(score);
                    return Ok(response);
                }
                else
                {
                    response.SetFailed("查找信息失败");
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// 垃圾投放赋分
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult VolunteeraddScore(GrabageDisposal model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                //不能给自己赋分
                if (model.SystemUserUuid == AuthContextService.CurrentUser.Guid)
                {
                    response.SetFailed("不能给自己赋分");
                    return Ok(response);
                }

                //获取赋分次数
                var totalcount = _dbContext.Overallsituation.ToList()[0].SetNumber;
                //一个家庭组每天赋分不能超过指定次数

                var family_address = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == model.SystemUserUuid).Address;
                if (String.IsNullOrEmpty(family_address))
                {
                    response.SetFailed("家庭地址为空，赋分失败");
                    return Ok(response);
                }
                var family = _dbContext.SystemUser.Where(x => x.Address == family_address && x.SystemUserUuid != model.SystemUserUuid).ToList();
                var count1 = _dbContext.GrabageDisposal.Where(x => x.SystemUserUuid == model.SystemUserUuid && x.ScoreAddtime.CompareTo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) >= 0).ToList().Count();
                if (count1 >= totalcount)
                {
                    response.SetFailed("每个家庭组每天最多赋分" + totalcount + "次");
                    return Ok(response);
                }
                if (family.Count() > 0)
                {
                    for (int i = 0; i < family.Count(); i++)
                    {
                        count1 += _dbContext.GrabageDisposal.Where(x => x.SystemUserUuid == family[i].SystemUserUuid && x.ScoreAddtime.Contains(DateTime.Now.ToString("yyyy-MM-dd"))).ToList().Count();
                        if (count1 >= totalcount)
                        {
                            response.SetFailed("每个家庭组每天最多赋分" + totalcount + "次");
                            return Ok(response);
                        }
                    }
                }

                var entity = new GrabageDisposal();
                entity.GarbageDisposalUuid = Guid.NewGuid();
                entity.ScoreSettingUuid = model.ScoreSettingUuid;
                entity.IsDelete = "0";
                entity.SystemUserUuid = model.SystemUserUuid;
                entity.AddTime = DateTime.Now.ToString();
                entity.ScoreAddtime = DateTime.Now.ToString();
                entity.SupervisorUuid = AuthContextService.CurrentUser.Guid;
                entity.GrabType = model.GrabType;
                entity.GrabageRoomId = model.GrabageRoomId;
                _dbContext.GrabageDisposal.Add(entity);


                _dbContext.SaveChanges();
                response.SetSuccess("赋分成功");
                return Ok(response);
            }
        }


        /// <summary>
        /// 督导员赋分记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult AddScoreRecode(GetscoreParms model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                List<AddScore> scoreRecode = new List<AddScore>();
                DateTime time1 = DateTime.Parse(model.startdate);
                DateTime time2 = DateTime.Parse(model.enddate + " 23:59:59"); 
                var date1 = time1.ToString("yyyy-MM-dd HH:mm:ss");
                var date2 = time2.ToString("yyyy-MM-dd HH:mm:ss");
                if (model.trashtype == "志愿者服务积分")
                {
                    //垃圾投放
                    var trash_entity = _dbContext.VolunteerService.Where(x => x.SupervisorUuid.ToString() == model.uuid &&
                    x.ScoreAddtime.CompareTo(date1) >= 0 && x.ScoreAddtime.CompareTo(date2) <= 0).ToList();


                    if (trash_entity != null)
                    {
                        for (int i = 0; i < trash_entity.Count; i++)
                        {
                            var pmtime = DateTime.Parse(trash_entity[i].ScoreAddtime);
                            if (model.pa == "am")
                            {
                                if (pmtime.Hour <= 12)
                                {
                                    AddScore score = new AddScore();
                                    score.RealName = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == trash_entity[i].SystemUserUuid).RealName;
                                    score.Score = SupervisorManageController.GetSingleScore(trash_entity[i].VolunteerServiceUuid) + "";
                                    score.AddTime = trash_entity[i].ScoreAddtime;//赋分时间
                                    score.GrabageRoomNum = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == trash_entity[i].GrabageRoomId).Ljname;
                                    score.AddPeople = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid.ToString() == model.uuid).RealName;

                                    scoreRecode.Add(score);
                                }

                            }
                            else
                            {
                                if (pmtime.Hour > 12)
                                {
                                    AddScore score = new AddScore();
                                    score.RealName = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == trash_entity[i].SystemUserUuid).RealName;
                                    score.Score = SupervisorManageController.GetSingleScore(trash_entity[i].VolunteerServiceUuid) + "";
                                    score.AddTime = trash_entity[i].ScoreAddtime;//赋分时间
                                    score.GrabageRoomNum = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == trash_entity[i].GrabageRoomId).Ljname;
                                    score.AddPeople = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid.ToString() == model.uuid).RealName;

                                    scoreRecode.Add(score);
                                }
                            }

                        }
                    }
                    else
                    {
                        response.SetFailed("无数据");
                        return Ok(response);
                    }
                    response.SetSuccess();
                    response.SetData(scoreRecode);
                    return Ok(response);
                }
                else
                {
                    //垃圾投放
                    var trash_entity = _dbContext.GrabageDisposal.Where(x => x.SupervisorUuid.ToString() == model.uuid &&
                    x.ScoreAddtime.CompareTo(date1) >= 0 && x.ScoreAddtime.CompareTo(date2) <= 0).ToList();

                    if (trash_entity != null)
                    {
                        for (int i = 0; i < trash_entity.Count; i++)
                        {
                            var pmtime = DateTime.Parse(trash_entity[i].ScoreAddtime);
                            if (model.pa == "am")
                            {
                                if (pmtime.Hour <= 12)
                                {
                                    AddScore score = new AddScore();
                                    score.RealName = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == trash_entity[i].SystemUserUuid).RealName;
                                    score.Score = _dbContext.ScoreSetting.FirstOrDefault(x => x.ScoreUuid == trash_entity[i].ScoreSettingUuid).Integral + "";
                                    score.AddTime = trash_entity[i].ScoreAddtime;//赋分时间
                                    score.GrabageRoomNum = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == trash_entity[i].GrabageRoomId).Ljname;
                                    score.AddPeople = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid.ToString() == model.uuid).RealName;

                                    scoreRecode.Add(score);
                                }

                            }
                            else
                            {
                                if (pmtime.Hour > 12)
                                {
                                    AddScore score = new AddScore();
                                    score.RealName = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == trash_entity[i].SystemUserUuid).RealName;
                                    score.Score = _dbContext.ScoreSetting.FirstOrDefault(x => x.ScoreUuid == trash_entity[i].ScoreSettingUuid).Integral + "";
                                    score.AddTime = trash_entity[i].ScoreAddtime;//赋分时间
                                    score.GrabageRoomNum = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == trash_entity[i].GrabageRoomId).Ljname;
                                    score.AddPeople = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid.ToString() == model.uuid).RealName;

                                    scoreRecode.Add(score);
                                }
                            }

                        }
                    }
                    else
                    {
                        response.SetFailed("无数据");
                        return Ok(response);
                    }
                    response.SetSuccess();
                    response.SetData(scoreRecode);
                    return Ok(response);
                }

            }
        }
        /// <summary>
        /// 获取单个分数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static double GetSingleScore(Guid id)
        {
            using (RefuseClassificationContext cc = new RefuseClassificationContext())
            {
                double hs = cc.Overallsituation.FirstOrDefault().HourScore;
                double s = 0;
                string t = "";
                TimeSpan ts = new TimeSpan();
                var query = cc.VolunteerService.FirstOrDefault(x => x.VolunteerServiceUuid == id);
                if (!string.IsNullOrEmpty(query.Intime) && !string.IsNullOrEmpty(query.Outtime))
                {
                    ts = (DateTime.Parse(query.Outtime) - DateTime.Parse(query.Intime));
                }
                t = Math.Floor(ts.TotalHours).ToString();
                s = hs * double.Parse(t);
                return s;
            }
        }

        /// <summary>
        /// 志愿者管理(列表)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AllList()
        {
            using (_dbContext)
            {
                var roletype = _dbContext.SystemRole.FirstOrDefault(x => x.RoleName == "志愿者");
                var query = _dbContext.SystemUser.Where(x => x.SystemRoleUuid.Contains(roletype.SystemRoleUuid.ToString())).AsQueryable();

                ////时间筛选
                //if (!string.IsNullOrEmpty(payload.kw2[0]) && !string.IsNullOrEmpty(payload.kw2[1]))
                //{
                //    var stime =
                //    query = query.Where(x => Convert.ToDateTime(x.AddTime) >= Convert.ToDateTime(payload.kw2[0]) && Convert.ToDateTime(x.AddTime) <= Convert.ToDateTime(payload.kw2[1]));
                //}
                query = query.Where(x => x.IsDeleted == 0);
                //分页
                var list = query.ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        // <summary>
        /// 获取督导员管理的垃圾厢房的位置
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult GetTrashLocation(Guid guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == guid);


                response.SetSuccess();
                response.SetData(entity);
                return Ok(response);
            }
        }


        /// <summary>
        /// 积分评价列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult scoreList()
        {
            using (_dbContext)
            {
                var entity = _dbContext.ScoreSetting.OrderBy(x => x.Integral).ToArray();


                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 垃圾厢房列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult TrashRoomList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var entity = _dbContext.GrabageRoom.Where(x => x.IsDelete == "0").ToArray();
                string idlist = "0";
                for (var i = 0; i < entity.Length; i++)
                {
                    //判断数据库的经纬度是否为空                    
                    if (!string.IsNullOrEmpty(entity[i].Lon) && !string.IsNullOrEmpty(entity[i].Lat))
                    {
                        idlist += "," + entity[i].Id;
                    }
                }
                var shuzu = idlist.Split(',').Select(s => int.Parse(s)).ToList();
                var entitys = _dbContext.GrabageRoom.Where(x => shuzu.Contains(x.Id)).ToArray();
                response.SetData(entity);
                return Ok(response);
            }
        }
        /// <summary>
        /// 志愿者服务评价
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="pj"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult volunteerPJ(Guid guid, string pj)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.Attendance.FirstOrDefault(x => x.SystemUserUuid == guid);
                if (entity == null)
                {
                    response.SetFailed("未获取到今天值班的志愿者，评价失败");
                    return Ok(response);
                }
                if (!String.IsNullOrEmpty(entity.Volunteerevaluation))
                {
                    response.SetFailed("请勿重复评价");
                    return Ok(response);
                }
                entity.Volunteerevaluation = pj;

                _dbContext.SaveChanges();
                response.SetSuccess("评价成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 用户注册，获取验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet("{phone}")]
        public IActionResult GetRegCode(string phone)
        {
            SystemPublic.CreateSystemLog("短信验证发送开始", "adds");
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                if (!String.IsNullOrEmpty(phone))
                {
                    string str = CreateValidateNumber(4);

                    var result = HaikanRefuseClassification.Api.Entities.YunSendMsg2.SendMsg(phone, str);
                    SystemPublic.CreateSystemLog("短信验证发送", "add");
                    response.SetData(new { str });
                    response.SetSuccess();
                    return Ok(response);
                }

                response.SetFailed("请输入正确的手机号");
                return Ok(response);
            }
        }



        /// <summary>
        /// 重置密码时的验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet]
        public IActionResult GetResCode(string phone)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                if (!String.IsNullOrEmpty(phone))
                {
                    string str = CreateValidateNumber(4);

                    var result = YunSendMsg.SendMsg(phone, str);

                    response.SetData(new { str });
                    response.SetSuccess();
                    return Ok(response);
                }

                response.SetFailed("请输入正确的手机号");
                return Ok(response);
            }
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost]
        public IActionResult SetNewPass(ChangPassInfoModel model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                if (!string.IsNullOrEmpty(model.Phone) && !string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.Code))
                {
                    var entity = _dbContext.SystemUser.Where(x => x.Phone == model.Phone && x.IsDeleted == 0).FirstOrDefault();
                    if (entity == null)
                    {
                        response.SetFailed("不存在该手机号的用户");
                        return Ok(response);
                    }
                    //entity.Id = null;
                    entity.PassWord = Haikan3.Utils.DesEncrypt.Encrypt(model.Password.Trim(), MdDesEncrypt.SecretKey);
                    //_dbContext.SystemUser.Update(entity);
                    var num = _dbContext.SaveChanges();
                    if (num >= 0)
                    {
                        response.SetSuccess("密码修改成功");
                        return Ok(response);
                    }
                    else
                    {
                        response.SetSuccess("密码修改失败");
                        return Ok(response);
                    }
                }
                response.SetFailed("请输入信息有误");
                return Ok(response);
            }
        }

        //产生验证码的字符集
        private static string[] ValidateCharArray = new string[] { "2", "3", "4", "5", "6", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "M", "N", "P", "R", "S", "U", "W", "X", "Y", "a", "b", "c", "d", "e", "f", "g", "h", "j", "k", "m", "n", "p", "r", "s", "u", "w", "x", "y" };

        ///<summary>
        ///生成随机验证码（数字字母）
        ///</summary>
        ///<param name="length">验证码的长度</param>
        ///<returns></returns>
        public static string CreateValidateNumber(int length)
        {
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(ValidateCharArray.Length);
                if (temp != -1 && temp == t)
                {
                    return CreateValidateNumber(length);
                }
                temp = t;
                randomCode += ValidateCharArray[t];
            }
            return randomCode;
        }
        /// <summary>
        /// 预约垃圾厢房
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult TrashRoomYYList(Guid guid, string ap)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var date = DateTime.Now.ToString("yyyy-MM-dd");
                var entity = (from a in _dbContext.VolunteerYy
                              join b in _dbContext.GrabageRoom
                              on a.GrabRoomUuid equals b.GarbageRoomUuid
                              where a.VolunteerUuid == guid &&
                              b.IsDelete != "1" &&
                              a.Ap == ap && a.StartTime.Contains(date)
                              select new
                              {
                                  a,
                                  b.Ljname,
                                  b.Lat,
                                  b.Lon
                              }).ToList();
                response.SetData(entity);
                return Ok(response);
            }
        }
        /// <summary>
        /// 判断距离
        /// </summary>
        /// <param name="lnglat"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Roomjuli(string lnglat, Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                double lng2 = 0;
                double lat2 = 0;
                if (lnglat != "")
                {
                    string[] lnglatlist = lnglat.Split(',');
                    if (lnglatlist.Length > 0)
                    {
                        lng2 = Convert.ToDouble(lnglatlist[0]);
                        lat2 = Convert.ToDouble(lnglatlist[1]);
                    }
                }
                var entity = _dbContext.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUuid == guid);
                //判断数据库的经纬度是否为空                    
                if (!string.IsNullOrEmpty(entity.Lon) && !string.IsNullOrEmpty(entity.Lat))
                {
                    double lng1 = Convert.ToDouble(entity.Lon.ToString());//经度
                    double lat1 = Convert.ToDouble(entity.Lat.ToString());//纬度
                    //判断数据库里面的经纬度和传过来的经纬度之间的距离是否超过20米
                    double juli = GetDistance(lng1, lat1, lng2, lat2);//距离单位米
                    if (juli <= 20)//判断是否是<=20米 如果是的就符合条件，否则不符合
                    {
                        response.SetData(0);
                        return Ok(response);
                    }
                }
                response.SetData(1);
                return Ok(response);
            }
        }
        /// <summary>
        /// 垃圾厢房列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{lnglat}")]
        public IActionResult TrashRoomList(string lnglat)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                double lng2 = 0;
                double lat2 = 0;
                if (lnglat != "")
                {
                    string[] lnglatlist = lnglat.Split(',');
                    if (lnglatlist.Length > 0)
                    {
                        lng2 = Convert.ToDouble(lnglatlist[0]);
                        lat2 = Convert.ToDouble(lnglatlist[1]);
                    }
                }
                string idlist = "0";
                //垃圾箱房
                var entity = _dbContext.GrabageRoom.Where(x => x.IsDelete == "0").ToArray();
                for (var i = 0; i < entity.Length; i++)
                {
                    //判断数据库的经纬度是否为空                    
                    if (!string.IsNullOrEmpty(entity[i].Lon) && !string.IsNullOrEmpty(entity[i].Lat))
                    {
                        double lng1 = Convert.ToDouble(entity[i].Lon.ToString());//经度
                        double lat1 = Convert.ToDouble(entity[i].Lat.ToString());//纬度
                        //判断数据库里面的经纬度和传过来的经纬度之间的距离是否超过20米
                        double juli = GetDistance(lng1, lat1, lng2, lat2);//距离单位米
                        if (juli <= 20)//判断是否是<=20米 如果是的就符合条件，否则不符合
                        {
                            idlist += "," + entity[i].Id;
                        }
                    }
                }
                var shuzu = idlist.Split(',').Select(s => int.Parse(s)).ToList();
                var entitys = _dbContext.GrabageRoom.Where(x => shuzu.Contains(x.Id)).ToArray();
                response.SetData(entitys);
                return Ok(response);
            }
        }
        /*          
        * @param lng1第一个经度
        * @param lat1 第一个纬度
        * @param lng2第二个经度
        * @param lat2第二个纬度         
        * @return 两个经纬度的距离(单位千米)
        */
        public static double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double earth_radius = 6378.137;//地球半径,单位千米
            double radLat1 = lat1 * Math.PI / 180.0;
            double radLat2 = lat2 * Math.PI / 180.0;
            double radLng1 = lng1 * Math.PI / 180.0;
            double radLng2 = lng2 * Math.PI / 180.0;
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
                    Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * earth_radius;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        /// <summary>
        /// 是否绑定家庭码
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult JiatinmaList(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {

                var query = from a in _dbContext.SystemUser
                            where a.IsDeleted != 1
                            && a.SystemUserUuid == guid
                            select new
                            {
                                a
                            };
                var entity = query.ToList();
                response.SetData(entity);
                return Ok(response);
            }
        }

        [HttpGet]
        public IActionResult UntieMa(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == guid);
                query.HomeAddressUuid = null;
                query.RealName = "";
                query.UserIdCard = null;
                query.OldCard = null;
                query.IdcardMd5 = null;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }
        /// <summary>
        /// 添加家庭码信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddJiatinma(Guid guid, Guid vuid)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var hentity = _dbContext.HomeAddress.FirstOrDefault(x => x.HomeAddressUuid == vuid);
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == guid);
                if (hentity!=null)
                {
                    if (entity != null)
                    {
                        if (entity.HomeAddressUuid == null)
                        {
                            entity.HomeAddressUuid = vuid;
                            _dbContext.SaveChanges();
                            response.SetSuccess("绑定成功");
                        }
                        else
                        {
                            response.SetSuccess("您已绑定过家庭码");
                        }

                    }
                    else
                    {
                        response.SetSuccess("绑定失败");
                    }
                }
                else
                {
                    response.SetSuccess("该家庭码信息不存在");
                }
                
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取商家的兑换记录信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        //[HttpGet("{guid}")]
        [HttpGet]
        public IActionResult ShopaddRecordList(string guid, string suid, string time)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            DateTime time1 =Convert.ToDateTime(time);
            var date1 = time1.ToString("yyyy-MM-dd HH:mm:ss");
            //DateTime time2 = Convert.ToDateTime(DateTime.Now.ToShortDateString().ToString() + " 23:59:59");
            using (_dbContext)
            {
                SystemPublic.CreateSystemLog("开始查询商品兑换信息", "1");
                var shop = _dbContext.GoodsExchange.FirstOrDefault(x => x.ShopId == Guid.Parse(suid));
                if (shop != null)
                {
                    SystemPublic.CreateSystemLog("开始执行数据操作", "2");
                    var query = from c in _dbContext.GoodsExchange
                                join h in _dbContext.HomeAddress
                                on c.SystemUserUuid equals h.HomeAddressUuid
                                where c.ShopId == shop.ShopId
                                && c.ExchageTime.CompareTo(date1) >= 0
                                select new
                                {
                                    c.Shop.ShopName,
                                    c.Goods.Gname,
                                    c.SystemUserUuid,
                                    h.Address,
                                    c.ExchageTime,
                                    c.DeduceScore,
                                    c.Id
                                };
                    var query1 = from c in _dbContext.GoodsExchange
                                 where c.ShopId == shop.ShopId
                                 select new
                                 {
                                     c.DeduceScore
                                 };
                    var query2 = from c in _dbContext.GoodsExchange
                                 where c.ShopId == shop.ShopId
                                 && c.IsDelete!="1"
                                 && c.DeduceScore.Contains("-")
                                && c.ExchageTime.CompareTo(date1) >= 0
                                 select new
                                 {
                                     c.ExchageTime,
                                     c.DeduceScore,
                                     c.Id
                                 };
                    SystemPublic.CreateSystemLog("结束执行数据操作", "3");


                    var list = query.OrderByDescending(x=>x.Id).ToList();
                    var list1 = query1.ToList();
                    var list2 = query2.OrderByDescending(x => x.Id).ToList();
                    var total = 0.0;
                    for (int i = 0; i < list1.Count; i++)
                    {
                        total += double.Parse(list1[i].DeduceScore);
                    }
                    //new { list, total };
                    response.SetData(new { list, total,list2 });
                    return Ok(response);

                    //var list = _dbContext.GoodsExchange.Where(x => x.ShopId == shop.ShopUuid).ToArray();

                    //response.SetData(list);
                    //return Ok(response);
                }
                response.SetFailed();
                return Ok(response);
            }
        }


        /// <summary>
        /// 督导员赋分设备获取信息
        /// </summary>
        /// <param name="uuid">设备UUID</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("{uuid}")]
        public IActionResult GetInfo(string uuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.GrabageRoom.FirstOrDefault(x => x.Facilityuuid == uuid);
                if (entity == null)
                {
                    response.SetFailed("该设备请先绑定对应的垃圾厢房");
                    return Ok(response);
                }
                else
                {
                    response.SetSuccess();
                    response.SetData(entity);
                    return Ok(response);
                }
            }

        }

        /// <summary>
        /// 获取默认赋分分数 好
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet]
        public IActionResult GetScore()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.ScoreSetting.FirstOrDefault(x => x.ScoreName == "好");
                response.SetData(entity);
            }
            return Ok(response);
        }

        /// <summary>
        /// 添加赋分
        /// </summary>
        /// <param name="DeviceID">设备id</param>
        /// <param name="Strcontent">家庭码</param>
        /// <param name="Sign"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet]
        public IActionResult AddScore(string DeviceID, string Strcontent)
        {
            DeviceID = DeviceID.ToLower();
            var response = ResponseModelFactory.CreateInstance;
            if (string.IsNullOrEmpty(DeviceID) || string.IsNullOrEmpty(Strcontent))
            {
                response.SetFailed("请求参数存在空值"+"deviceid" + DeviceID + "strcontent" + Strcontent );
                //response.SetData(DeviceID+";"+ Strcontent+";"+ Sign);
                return Ok(response);

            }
            else
            {
                if (Strcontent.Split('_')[0]=="d")
                {
                    string ddyuuid = Strcontent.Split('_')[1];
                    //根据设备获取厢房
                    var room = _dbContext.GrabageRoom.FirstOrDefault(x => x.Facilityuuid == DeviceID);
                    if (room == null)
                    {
                        response.SetFailed("请先将设备绑定厢房");
                        return Ok(response);
                    }
                    var ddyentity = _dbContext.GrabageRoom.FirstOrDefault(x=>x.GarbageRoomUuid==Guid.Parse(ddyuuid)&&x.IsDelete!="1");
                    if (ddyentity==null)
                    {
                        response.SetFailed("暂无该厢房信息");
                        return Ok(response);
                    }
                    if (Guid.Parse(ddyuuid) != room.GarbageRoomUuid)
                    {
                        response.SetFailed("您暂未拥有该厢房的赋分权限");
                        return Ok(response);
                    }
                    using (_dbContext)
                    {
                        var query = _dbContext.GrabageDisposal.Where(x => x.IsDelete != "1").OrderByDescending(x => x.Id).FirstOrDefault();
                        if (query.IsScore!="0")
                        {
                            response.SetFailed("暂无赋分对象");
                            return Ok(response);
                        }
                        var systementity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == query.SystemUserUuid);
                        //赋分前分数
                        var hd = _dbContext.HomeAddress.FirstOrDefault(x => x.HomeAddressUuid == systementity.HomeAddressUuid);
                        var scoreSetting = _dbContext.ScoreSetting.First();
                        //加分
                        hd.Score = hd.Score + scoreSetting.Integral;
                        query.ScoreSettingUuid = scoreSetting.ScoreUuid;
                        query.ScoreAddtime= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        query.IsScore = "1";
                        _dbContext.SaveChanges();
                    }
                    response.SetSuccess("赋分成功");
                }
                else
                {
                    //二维码内容校验
                    var arr = Strcontent.Split('_'); 
                    if (arr == null || arr.Length < 2 || arr[0].Length != 1)
                    {
                        response.SetFailed("此二维码内容有误");
                        return Ok(response);
                    }
                    if (arr[0]=="b")
                    {
                        var result = arr[1];
                        var idcard = Common(result);
                        var entity = _dbContext.SystemUser.FirstOrDefault(x => x.UserIdCard == idcard);
                        if (entity==null)
                        {
                            response.SetFailed(idcard);
                            return Ok(response);
                        }
                        using (_dbContext)
                        {
                            //根据设备获取厢房
                            var room = _dbContext.GrabageRoom.FirstOrDefault(x => x.Facilityuuid == DeviceID);
                            if (room == null)
                            {
                                response.SetFailed("请先将设备绑定厢房");
                                return Ok(response);
                            }
                            Guid puuid = entity.SystemUserUuid;
                            Guid huuid = entity.HomeAddressUuid.Value;
                            if (huuid == null || huuid == Guid.Empty)
                            {
                                response.SetFailed("此用户未绑定家庭码或家庭码不存在");
                                return Ok(response);
                            }
                            //当天赋分次数
                            var disposal = _dbContext.GrabageDisposal.Count(x => x.HomeAddressUuid == huuid && x.ScoreAddtime.Contains(DateTime.Now.ToString("yyyy-MM-dd")));
                            //获取设置的评分次数
                            var num = _dbContext.Overallsituation.First();
                            if (disposal >= num.SetNumber)
                            {
                                response.SetFailed("今日该家庭赋分超过" + num.SetNumber + "次");
                                return Ok(response);
                            }
                            else
                            {
                                var scoreSetting = _dbContext.ScoreSetting.First();
                                //添加赋分记录
                                GrabageDisposal gd = new GrabageDisposal();
                                gd.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                gd.IsDelete = "0";
                                gd.GarbageDisposalUuid = Guid.NewGuid();
                                gd.GrabageRoomId = room.GarbageRoomUuid;
                                gd.HomeAddressUuid = huuid;
                                gd.MarkType = "百姓码";
                                gd.IsScore = "0";
                                if (puuid != Guid.Empty)
                                {
                                    gd.SystemUserUuid = puuid;
                                }
                                _dbContext.GrabageDisposal.Add(gd);
                                _dbContext.SaveChanges();
                            }
                        }
                        response.SetSuccess("投放成功");
                    }
                    else
                    {
                        //string md5 = YunSendMsg.GenerateMD5(DeviceID+Strcontent);
                        //string md5 = YunSendMsg.GenerateMD5("deviceid" + DeviceID + "strcontent" + Strcontent + "8a261bcf2070c42865ec7619");
                        //md5 = YunSendMsg.GenerateMD5("deviceid" + DeviceID + "strcontent" + Strcontent+md5);
                        //if (md5 != Sign)
                        //{
                        //    response.SetFailed("校验失败" + md5 + ":" + Sign + "(" + "deviceid" + DeviceID + "strcontent" + Strcontent + "8a261bcf2070c42865ec7619");
                        //    return Ok(response);
                        //}
                        using (_dbContext)
                        {
                            //根据设备获取厢房
                            var room = _dbContext.GrabageRoom.FirstOrDefault(x => x.Facilityuuid == DeviceID);
                            if (room == null)
                            {
                                response.SetFailed("请先将设备绑定厢房");
                                return Ok(response);
                            }
                            Guid huuid = Guid.Empty;
                            Guid puuid = Guid.Empty;
                            //家庭码
                            if (arr[0] == "h")
                            {
                                huuid = Guid.Parse(arr[1]);
                            }
                            if (arr[0] == "u")
                            {
                                puuid = Guid.Parse(arr[1]);
                                huuid = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == Guid.Parse(arr[1])).HomeAddressUuid.Value;
                            }
                            if (huuid == null || huuid == Guid.Empty)
                            {
                                response.SetFailed("此用户未绑定家庭码或家庭码不存在");
                                return Ok(response);
                            }
                            //当天赋分次数
                            var disposal = _dbContext.GrabageDisposal.Count(x => x.HomeAddressUuid == huuid && x.ScoreAddtime.Contains(DateTime.Now.ToString("yyyy-MM-dd")));
                            //获取设置的评分次数
                            var num = _dbContext.Overallsituation.First();
                            if (disposal >= num.SetNumber)
                            {
                                response.SetFailed("今日该家庭赋分超过" + num.SetNumber + "次");
                                return Ok(response);
                            }
                            else
                            {

                                //之前赋分记录
                                //var all = from g in _dbContext.GrabageDisposal
                                //          join sc in _dbContext.ScoreSetting
                                //          on g.ScoreSettingUuid equals sc.ScoreUuid
                                //          where g.HomeAddressUuid == huuid
                                //          select new
                                //          {
                                //              sc.Integral
                                //          };
                                //之前赋分分数之和
                                //var sum = all.Sum(x => x.Integral.Value);
                                //获取设置的评分分数
                                //var scoreSetting = _dbContext.ScoreSetting.First(x => x.ScoreName == "好");
                                var scoreSetting = _dbContext.ScoreSetting.First();
                                //添加赋分记录
                                GrabageDisposal gd = new GrabageDisposal();
                                gd.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                gd.IsDelete = "0";
                                gd.GarbageDisposalUuid = Guid.NewGuid();
                                gd.GrabageRoomId = room.GarbageRoomUuid;
                                gd.ScoreSettingUuid = scoreSetting.ScoreUuid;
                                gd.HomeAddressUuid = huuid;
                                gd.IsScore = "0";
                                if (puuid != Guid.Empty)
                                {
                                    gd.SystemUserUuid = puuid;
                                }
                                if (arr[0] == "h")
                                {
                                    gd.MarkType = "家庭码";
                                }
                                _dbContext.GrabageDisposal.Add(gd);
                                _dbContext.SaveChanges();
                            }
                        }
                        response.SetSuccess("投放成功");
                    }
                }

                //response.SetData(DeviceID + ";" + Strcontent + ";" + Sign);
                return Ok(response);
            }
        }

        /// <summary>
        /// 市民卡赋分
        /// </summary>
        /// <param name="DeviceID">设备id</param>
        /// <param name="card">芯片卡号</param>
        /// <param name="Sign">秘钥</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AddScore2Async(string DeviceID, string card)
        {
            DeviceID = DeviceID.ToLower();
            card = card.ToLower();
            var response = ResponseModelFactory.CreateInstance;
            if (string.IsNullOrEmpty(card) ||string.IsNullOrEmpty(DeviceID))
            {
                response.SetFailed("请求参数存在空值" + "card" + card +"deviceid" + DeviceID );
                //response.SetData(DeviceID+";"+ Strcontent+";"+ Sign);
                return Ok(response);

            }
            else
            {
                    card = card.Split('_')[1];
                    var reqSeq = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
                    //参数信息
                    Dictionary<string, string> messageDic = new Dictionary<string, string>();
                    messageDic.Add("reqSeq", reqSeq.ToString());
                    messageDic.Add("reqTime", DateTime.Now.ToString("yyyymmddhhmmss"));
                    messageDic.Add("fjcsn", card.ToUpper());
                    messageDic.Add("cardNo", "");
                    string messageDataJson = JsonConvert.SerializeObject(messageDic);
                    //公共请求体参数
                    Dictionary<string, string> openDic = new Dictionary<string, string>();
                    openDic.Add("appId", "PRECARD_807850581711240");
                    openDic.Add("method", "getIDCardTerminal");
                    openDic.Add("reqSeq", reqSeq.ToString());

                    openDic.Add("bizContent", messageDataJson);
                    openDic.Add("sign_param", "appId,method,bizContent");
                    //排序
                    var content = SHA256WithRSA.getSignContent(openDic);
                    //加签
                    openDic.Add("sign", SHA256WithRSA.Signature(content, this.private_key));
                    string messageJson = JsonConvert.SerializeObject(openDic);
                    //请求响应
                    var data=await ToHttp<IDCardTerminal>.ToPost("https://ypay.96225.com/openapi", messageJson);
                    if (!data.success)
                    {
                        response.SetFailed(data.respCode+":"+data.respDesc);
                        return Ok(response);
                    }
                    var idcard = JsonConvert.DeserializeObject<IDCardValue>(data.value);
                    
                    //返回参数体
                    Dictionary<string, string> check = new Dictionary<string, string>();
                    check.Add("reqSeq", reqSeq.ToString());
                    check.Add("sign_param", "success,value");
                    //bool类型转为小写
                    check.Add("success", data.success.ToString().ToLower());
                    check.Add("value", data.value);
                    check.Add("sign", data.sign);
                    //加签字符串
                    var content2 = SHA256WithRSA.getSignContent(check);
                    //var sign = SHA256WithRSA.Signature(content2, this.public_key);
                    //验签
                    var result = SHA256WithRSA.rsaCheck(content2, public_key, data.sign);
                    if (!result)
                    {
                        response.SetFailed("验签失败");
                    }
                    else
                    {
                        using (_dbContext)
                        {
                            //根据设备获取厢房
                            var room = _dbContext.GrabageRoom.FirstOrDefault(x => x.Facilityuuid == DeviceID);
                            if (room == null)
                            {
                                response.SetFailed("请先将设备绑定厢房");
                                return Ok(response);
                            }
                            var user = _dbContext.SystemUser.FirstOrDefault(x => x.IdcardMd5 == idcard.idcard);
                            if (user == null)
                            {
                                response.SetFailed("此用户未绑定市民卡号");
                                return Ok(response);
                            }
                            if (user.HomeAddressUuid == null)
                            {
                                response.SetFailed("此用户未绑定家庭码");
                                return Ok(response);
                            }
                            //当天赋分次数
                            var disposal = _dbContext.GrabageDisposal.Count(x => x.HomeAddressUuid == user.HomeAddressUuid && x.ScoreAddtime.Contains(DateTime.Now.ToString("yyyy-MM-dd")));
                            //获取设置的评分次数
                            var num = _dbContext.Overallsituation.First();
                            //TimeSpan nowDt = DateTime.Now.TimeOfDay;
                            //TimeSpan AmstartDt = DateTime.Parse("7:00").TimeOfDay;
                            //TimeSpan AmendDt = DateTime.Parse("9:00").TimeOfDay;
                            //TimeSpan PmstartDt = DateTime.Parse("18:00").TimeOfDay;
                            //TimeSpan PmendDt = DateTime.Parse("20:00").TimeOfDay;
                            if (disposal >= num.SetNumber)
                            {
                                response.SetFailed("今日该家庭赋分超过" + num.SetNumber + "次");
                                return Ok(response);
                            }
                            //else if ((nowDt > AmstartDt && nowDt < AmendDt) || (nowDt > PmstartDt && nowDt < PmendDt))
                            //{

                            //}
                            else
                            {

                                //之前赋分记录
                                //var all = from g in _dbContext.GrabageDisposal
                                //          join sc in _dbContext.ScoreSetting
                                //          on g.ScoreSettingUuid equals sc.ScoreUuid
                                //          where g.HomeAddressUuid == huuid
                                //          select new
                                //          {
                                //              sc.Integral
                                //          };
                                //之前赋分分数之和
                                //var sum = all.Sum(x => x.Integral.Value);
                                //获取设置的评分分数
                                //var scoreSetting = _dbContext.ScoreSetting.First(x => x.ScoreName == "好");
                                var scoreSetting = _dbContext.ScoreSetting.First();
                                //添加赋分记录
                                GrabageDisposal gd = new GrabageDisposal();
                                gd.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                gd.ScoreAddtime = gd.AddTime;
                                gd.IsDelete = "0";
                                gd.GarbageDisposalUuid = Guid.NewGuid();
                                gd.GrabageRoomId = room.GarbageRoomUuid;
                                gd.ScoreSettingUuid = scoreSetting.ScoreUuid;
                                gd.HomeAddressUuid = user.HomeAddressUuid;
                                gd.SystemUserUuid = user.SystemUserUuid;
                                gd.IsScore = "0";
                                gd.MarkType = "市民卡";
                                _dbContext.GrabageDisposal.Add(gd);
                                
                                _dbContext.SaveChanges();
                            }
                        }
                        response.SetSuccess("投放成功");
                        response.SetData("ok");
                        return Ok(response);

                    }
            }


            response.SetData(card);
            return Ok(response);
        }


        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> TestAsync()
        {
            
            var reqSeq = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
            var response = ResponseModelFactory.CreateInstance;
            //参数信息
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("reqSeq", reqSeq.ToString());
            messageDic.Add("reqTime", DateTime.Now.ToString("yyyymmddhhmmss"));
            messageDic.Add("fjcsn", "8DD044E8");
            messageDic.Add("cardNo", "");
            string messageDataJson = JsonConvert.SerializeObject(messageDic);
            //公共请求体参数
            Dictionary<string, string> openDic = new Dictionary<string, string>();
            openDic.Add("appId", "PRECARD_807850581711240");
            openDic.Add("method", "getIDCardTerminal");
            openDic.Add("reqSeq", reqSeq.ToString());
            openDic.Add("bizContent", messageDataJson);
            openDic.Add("sign_param", "appId,method,bizContent");
            //排序
            var content = SHA256WithRSA.getSignContent(openDic);
            //加签
            openDic.Add("sign", SHA256WithRSA.Signature(content, this.private_key));
            string messageJson = JsonConvert.SerializeObject(openDic);
            var data=await ToHttp<IDCardTerminal>.ToPost("https://ypay.96225.com/openapi",messageJson);
           // string me = "{\"success\":true,\"respDesc\":null,\"respCode\":null,\"value\":\"{\\\"respSeq\\\":\\\"1590473159846\\\",\\\"idcard\\\":\\\"b74f6765195b786cc3aeed27058c868c\\\"}\",\"sign\":\"CIKP5xznzxMFqC3i4VZhBDFNNdQEYoWEpmzzA+DH5CuQkIO/2dIzpjtpxTANjeGzm3evC3Iea3I7S0FrgSO4K5Gr+aeXS6ysYa38InlaPNpaEQ/v/Lt/mxrCaE5mdwsv5xxRQ+BkgaKzz1rl8ghf5z43fI1UK29mUjiKnNGSEzFT6xmowdNC1GjBrvXwM17pbduB5dhBBh5cEmbEH1bzzyK4zv2LPOCZo1bCcsFE3/voGA+FeFzuKUw805BiBXvy//pKvbF+Clfw+cf7GmGhzTb20pan7gIcxijIsb5O3Zuzk3rfHKppLnq3926zqjIvcFecwDpGix+EKPz4/6KKxg==\"}";
            //var data = JsonConvert.DeserializeObject<IDCardTerminal>(me);
            var idcard = JsonConvert.DeserializeObject<IDCardValue>(data.value);
            //返回参数体
            Dictionary<string, string> check = new Dictionary<string, string>();
            check.Add("reqSeq", reqSeq.ToString());
            check.Add("sign_param", "success,value");
            //bool类型转为小写
            check.Add("success", data.success.ToString().ToLower());
            check.Add("value", data.value);
            check.Add("sign", data.sign);
            //加签字符串
            var content2 = SHA256WithRSA.getSignContent(check);
            //var sign = SHA256WithRSA.Signature(content2, this.public_key);
            //验签
            var result = SHA256WithRSA.rsaCheck(content2, public_key, data.sign);
            //解密
            if (result)
            {
                var user=_dbContext.SystemUser.FirstOrDefault(x => x.IdcardMd5 == idcard.idcard);
                
                

            }
            response.SetData(result);
            return Ok(response);
        }

        //百姓码
        public string Common(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                return "请求参数存在空值";

            }
            else
            {
                //设置请求接口
                var request = (HttpWebRequest)WebRequest.Create("https://api.bechangedt.com/api/healthcode/verify");
                //请求参数
                var postData = "{" +'"'+"healthQrCode"+'"'+":"+'"'+ result + '"'+"}";
                var data = Encoding.ASCII.GetBytes(postData);
                //请求方式
                request.Method = "POST";
                //请求头参数设置
                request.Headers.Add("organizationId", "002504421");
                request.Headers.Add("appId", "6737FDA2D");
                request.Headers.Add("dataSources", "3");
                request.Headers.Add("tradeCode", "60002");
                var requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                request.Headers.Add("requestTime", requestTime);
                string md5 = YunSendMsg.GenerateMD5("002504421" + "6737FDA2D" + "840CA391C69950D13514A2A3CD572506" + "3" + "60002" + requestTime).ToUpper();
                request.Headers.Add("sign", md5);
                request.ContentType = "application/json;charset=UTF-8";
                request.ContentLength = data.Length;
                request.UseDefaultCredentials = true;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //结果返回
                var responses = (HttpWebResponse)request.GetResponse();
                //转字符串
                var responseString = new StreamReader(responses.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                //转换为json对象
                CommonCard commodel = JsonConvert.DeserializeObject<CommonCard>(responseString);
                if (commodel.code!=1 || commodel.data == null)
                {
                    return commodel.msg;
                }
                else
                {
                    var card = AESHelper.Decrypt(commodel.data.verifyResp, "HEALTHCODEVERIFY");
                    CommonCardMan commodels = JsonConvert.DeserializeObject<CommonCardMan>(card);
                    return commodels.idCardValue;
                }
            }
        }

        //申领百信码
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetCommon(GetCommonMan model)
        {

            var response = ResponseModelFactory.CreateInstance;
            //转字符串
            var responseString = Commonset(model);
            //转换为json对象
            GetCommon commodel = JsonConvert.DeserializeObject<GetCommon>(responseString);
            response.SetData(commodel);
            return Ok(response);
        }

        public string Commonset(GetCommonMan model)
        {
            //设置请求接口
            var request = (HttpWebRequest)WebRequest.Create("https://api.bechangedt.com/api/healthcode/apply");
            string sex = model.idCardValue.Substring(14, 3);
            //性别代码为偶数是女性奇数为男性
            if (int.Parse(sex) % 2 == 0)
            {
                model.sex = "2";
            }
            else
            {
                model.sex = "1";
            }
            //请求参数
            var postDatas = "{" + '"' + "name" + '"' + ":" + '"' + model.name + '"' + "," +
                '"' + "idCardType" + '"' + ":" + '"' + model.idCardType + '"' + "," +
                '"' + "idCardValue" + '"' + ":" + '"' + model.idCardValue + '"' + "," +
                '"' + "sex" + '"' + ":" + '"' + model.sex + '"' + "," +
                '"' + "tel" + '"' + ":" + '"' + model.tel + '"' + "}";

            var card = AESHelper.Encrypt(postDatas, "HEALTHCODEVERIFY");
            var postData = "{" + '"' + "applyReq" + '"' + ":" + '"' + card + '"' + "}";
            var data = Encoding.ASCII.GetBytes(postData);
            //请求方式
            request.Method = "POST";
            //请求头参数设置
            request.Headers.Add("organizationId", "002504421");
            request.Headers.Add("appId", "6737FDA2D");
            request.Headers.Add("dataSources", "3");
            request.Headers.Add("tradeCode", "60001");
            var requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            request.Headers.Add("requestTime", requestTime);

            string md5 = YunSendMsg.GenerateMD5("002504421" + "6737FDA2D" + "840CA391C69950D13514A2A3CD572506" + "3" + "60001" + requestTime).ToUpper();
            //string md5 = YunSendMsg.GenerateMD5("organizationId" + "appId" + "134A997D10EDAB8EA4A6CF48CC3FD0BB" + "dataSources" + "tradeCode" + "requestTime");
            request.Headers.Add("sign", md5);
            request.ContentType = "application/json;charset=UTF-8";
            request.ContentLength = data.Length;
            request.UseDefaultCredentials = true;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            //结果返回
            var responses = (HttpWebResponse)request.GetResponse();
            //转字符串
            var responseString = new StreamReader(responses.GetResponseStream()).ReadToEnd();
            //转换为json对象
            //GetCommon commodel = JsonConvert.DeserializeObject<GetCommon>(responseString);
            return responseString;
        }

        /// <summary>
        /// 获取用户百姓码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetCommonEWM(Guid id)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateInstance;
                var query = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == id);
                GetCommonMan model = new GetCommonMan();
                model.name = query.RealName;
                model.idCardType = query.OldCard;
                model.idCardValue = query.UserIdCard;
                model.sex = query.Sex;
                model.tel = query.Phone;
                var result = Commonset(model);
                GetCommon commodel = JsonConvert.DeserializeObject<GetCommon>(result);
                response.SetData("b_" + commodel.data.healthQrCode + "," + commodel.data.qrCodeVaildDate);
                return Ok(response);
            }
        }
    }
}
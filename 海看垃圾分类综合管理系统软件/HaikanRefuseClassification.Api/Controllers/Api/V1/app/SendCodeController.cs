using AutoMapper;
using HaikanRefuseClassification.Api.Configurations;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.CustomException;
using HaikanRefuseClassification.Api.ViewModels.App;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.app
{
    [Route("api/v1/app/[controller]/[action]")]
    [ApiController]
    public class SendCodeController : ControllerBase
    {
        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly MdDesEncrypt MdDesEncrypt;
        public SendCodeController(RefuseClassificationContext dbContext, IMapper mapper, IHostingEnvironment hostingEnvironment, IOptions<MdDesEncrypt> mdDesEncryp)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            MdDesEncrypt = mdDesEncryp.Value;
        }


        /// <summary>
        /// 用户注册，获取验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet("{phone}")]
        public IActionResult GetRegCode(string phone)
        {
            //SystemPublic.CreateSystemLog("短信验证发送开始", "adds");
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                if (!String.IsNullOrEmpty(phone))
                {
                    string str = CreateValidateNumber(4);

                    var result = HaikanRefuseClassification.Api.Entities.YunSendMsg2.SendMsg(phone, str);
                   // SystemPublic.CreateSystemLog("短信验证发送", "add");
                    response.SetData(new { str });
                    response.SetSuccess();
                    return Ok(response);
                }

                response.SetFailed("请输入正确的手机号");
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
        /// 微信注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult CreateUser(SystemUser model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                if (_dbContext.SystemUser.Count(x => x.Phone == model.Phone) > 0)
                {
                    response.SetFailed("该手机号已注册");
                    return Ok(response);
                }
                var entity = new SystemUser();
                entity.SystemUserUuid = Guid.NewGuid();

                entity.RealName = model.RealName;
                entity.LoginName = model.RealName;
                entity.PassWord = Haikan3.Utils.DesEncrypt.Encrypt(model.PassWord.Trim(), MdDesEncrypt.SecretKey);
                entity.Address = model.Address;
                entity.OldCard = model.OldCard;
                entity.Phone = model.Phone;
                entity.UserType = 2;
                entity.SystemRoleUuid = "C6BDB5B3-990B-4943-B2A1-1492044E38B8";//普通用户
                entity.AddPeople = "微信自主注册";

                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");

                entity.IsDeleted = 0;
                _dbContext.SystemUser.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess("注册成功");
                return Ok(response);
            }
        }

        [HttpPost]
        public IActionResult BindUser(CheckUserModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var user = _dbContext.SystemUser.FirstOrDefault(x => x.Phone == model.Username.Trim());
                if (user == null || user.IsDeleted == 1)
                {
                    response.SetFailed("用户不存在");
                    return Ok(response);
                }
                string s = Haikan3.Utils.DesEncrypt.Encrypt(model.Password.Trim(), MdDesEncrypt.SecretKey);
                if (user.PassWord != Haikan3.Utils.DesEncrypt.Encrypt(model.Password.Trim(), MdDesEncrypt.SecretKey))
                {
                    response.SetFailed("密码不正确");
                    return Ok(response);
                }
                user.Wechat = model.Wechat;
                _dbContext.SaveChanges();
                response.SetSuccess("绑定成功");
                return Ok(response);
            }
            
        }

        [HttpPost]
        [CustomAuthorize]
        public IActionResult RelieveUser(CheckUserModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var user = _dbContext.SystemUser.FirstOrDefault(x => x.Phone == model.Username.Trim()&&x.Wechat==model.Wechat.Trim());
                if (user == null || user.IsDeleted == 1)
                {
                    response.SetFailed("用户不存在");
                    return Ok(response);
                }
                
                user.Wechat =null;
                _dbContext.SaveChanges();
                response.SetSuccess("解绑成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取openid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetWechat(WechatModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            var code = "0";
            //先根据用户请求的uri构造请求地址
            string serviceUrl = "https://api.weixin.qq.com/sns/jscode2session?appid=" + model.Appid + "&secret=" + model.Secret +
                            "&js_code=" +model.Js_code;
            //创建Web访问对象
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
            //把用户传过来的数据转成“UTF-8”的字节流
            //byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data);

            myRequest.Method = "POST";
            //myRequest.ContentLength = buf.Length;
            myRequest.ContentType = "application/x-www-form-urlencoded";
            //myRequest.MaximumAutomaticRedirections = 1;
            //myRequest.AllowAutoRedirect = true;
            //发送请求
            Stream stream = myRequest.GetRequestStream();
            //stream.Write(buf, 0, buf.Length);
            stream.Close();

            //获取接口返回值
            //通过Web访问对象获取响应内容
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
            string returnJson = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
            code = myResponse.StatusCode.ToString();
            reader.Close();
            myResponse.Close();
            var data = JsonConvert.DeserializeObject<OpenIDModel>(returnJson);
            if (code=="OK"&& !string.IsNullOrEmpty(data.Openid))
            {
                response.SetSuccess();
                response.SetData(data);
                return Ok(response);
            }
            else
            {
                response.SetFailed();
                response.SetData(null);
                return Ok(response);
            }

        }
    }
}
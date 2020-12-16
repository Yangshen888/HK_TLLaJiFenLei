using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaikanDemo.qa
{
    public partial class mcardsea1 : System.Web.UI.Page
    {
        //系统日志表
        //HaikanDemo.BLL.SystemLog SystemLogbll = new BLL.SystemLog();
        //Model.SystemLog systemLogmodel = new Model.SystemLog();
        DBtlljfl dBtlljfl = new DBtlljfl();
        SystemLog systemLogmodel = new SystemLog();

        private readonly string private_key = "-----BEGIN RSA PRIVATE KEY-----MIIEpQIBAAKCAQEAvG6zZy9Nqc0mhuo3oxYsjo3+j6ZPdzgG0ToLPcgRyBTyN5zXILTavrwRnbvhrZn/MVO968IMHKGGZdZox0NHNvMQWMpPG9XhSWZOuo2xER4welFDkMBGgrAe9B1lXpTPVtnOJxfUeTcZvRFL4BYOjfdb7R1m0aqBfJ6Z7yQGJvonzmhyGyhh1LDKNBxoPv5KCWxClqaGYRwmVAcxSEfMiMjLLqcomha74m8JfND1Zf9hqkKULCpA2AqPSKmcoZ+P2I1s0VFiaoULlxu73tYPQKqNrwaUKUs/RY+X5PMgBcbur3TgbeEu0WCxZyV5HPwT8j86MSFsuja08CO3uz3dLwIDAQABAoIBAHVZQ6TpEqbCulGfH5MTRiBpUbVIT4jhfW1jhoitRlWipc34gQk/WFMccKQY08z3cLPZgReHu19BQJ+/TXV68qjH6tBA/c9J/Ylmi3UmtLUCZhJm9Xr6I29UG5LPk8e5SP6/meFCt/HdXMwgL26YjevftoIOo2/Djex8IUWK28H9SUEPak5JUg+l741r55XGZEhnoAY/vhcghweU0DJajK8Wi3UxRVUXBuf5ls00dGYMZCZAL5+TxjQwwqizqDiZ0RM1eUFwz9aJBGjw6sCZeLc37KoNv5Q5pfa80JTy0u7uzI2AxwVfQbC2lxA9dRkSx4cxLyATsmPatSVB5bc32TECgYEA8w50C6x6/OybhC7kKls5tF3H5DLAD307ueGUQX2BJZ5VJaAhAOtsnEHMsz/R7NKY70rjGKE47n++CEEbneBgqBdqawzOToWpeqafjoiKzzIbnuNBwiAQmp68/UZ9ZV2Ls8fZpq/1OqNQIbV6O9Ecp4najMY0P1s+v/PIbDJ6f5MCgYEAxneRRpiA9WcvqPolZ+Q1BccQvWRkFcarLtBvWay6Cb7QQss5LWaBTZXEbbRWuTowlBnsX4G9FjsZ8RxqH77v8Dc3ehp0l9Sx2Vw8YGjfCzbV3IGDmlAGFGKP3tBEIMkDK8YdvMurQQqmMjFrx6YR7LpUtLxi/oglYAewxMWblXUCgYEAgl8hTdWxjpMXg9pnFnUiSaX3/2ZdcLF65OSj0lEQge4gu/LdYRHmixYcR5WW85Gu6MPhdiecUwmAFAtgVdmx3tfYhB01WBcH5jsT4K9KzYKSIDLD5e2vGlDFDJHP1xxLQB6Vl3xQbKiG5d3i98zdstwVt2blRYqa6PlJawfUfzMCgYEAiQgRdI7jq409aQyeOydkPML/meTw/eAYXdBosaADK6tmHFg4+FHoQWuBHsX/gxDcbcWgYSkxJ2JTPRkDZTvuawuU8GfHzPV8frmirmZ6akHIU+HQvgE20WhkMdHW2FQyLk0yRyLQ8a2qpslcw5K0maDlz4yrRVc3hyCIOrS+AekCgYEAqVbuQeWqW2q9EGbsPwa9/woI7aHEIdAx+Wfn50PM7a1KK5+oHpv2pVFcQND5gqsN5kcJ7WRO3i8KxR6y/xT1CDNsjA97CJ7/LGo4aLzvOs7zlBBaQpzET6f7r6+oed1iVE1PQi7KQSGJBcCsbPY9YFJH4KZCGIXYiWgUP6pe27Q=-----END RSA PRIVATE KEY-----";
        private readonly string public_key = "-----BEGIN PUBLIC KEY-----MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAjGpnmKByseFngibUJiNIc4" +
                "/6wfIQHMOHAsUNe1EACJUrFDFat78IqCJZIsYZ91G8He+88rBQL9+zK7DoubJsHPb6JTHE8krdFN1u/oaEDQz" +
                "/k5YNff89byrAdjsa3g5GcRU4d1/1D1TpkSVk0BTojtAgCgyNfRPqxB95gF" +
                "+Bsu2LQB1qMojSVoUx7rbSMofexahjqCWBqIryDYskegjNZhcfh0fiIZd/hK7bZDILN" +
                "+gHhi60H5vvAEe9M84GYo4yszW51qt8Blf2u5SZ0WkiAsh4+AMFdoRpBDP7MGsmWIYLkylSYy2QoSM4nyaEFZXp3qk" +
                "/zI9k2HSB/XuW8FwFXQIDAQAB-----END PUBLIC KEY-----";

        protected void Page_Load(object sender, EventArgs e)
        {
            bind();
            //http://192.168.0.221:4401/
            //Geturl("http://192.168.0.221:4401/api/v1/app/");
        }

        public void bind()
        {
            if (Request.QueryString["cardid"] != null && Request.QueryString["cjihao"] != null && Request.QueryString["mjihao"] != null && Request.QueryString["status"] != null && Request.QueryString["time"] != null)
            {
                string cardid = Request.QueryString["cardid"].ToString();
                string mjihao = Request.QueryString["mjihao"].ToString();
                string cjihao = Request.QueryString["cjihao"].ToString();
                string status = Request.QueryString["status"].ToString();
                string time = Request.QueryString["time"].ToString();
                string result = "";
                if (cardid.Split('_')[0] == "d" || cardid.Split('_')[0] == "b" || cardid.Split('_')[0] == "h" || cardid.Split('_')[0] == "u")
                {
                    result = Get1(cjihao, cardid);
                    //result = Geturl("https://ljfl.hztlcgj.com/api/v1/app/supervisormanage/addscore?DeviceID=" + cjihao + "&Strcontent=" + cardid);
                }
                else if (cardid.Length==8)
                {
                    result = Get2(cjihao, cardid);
                    //result = Geturl("https://ljfl.hztlcgj.com/api/v1/app/supervisormanage/AddScore2Async?DeviceID=" + cjihao + "&card=" + cardid);
                }
                else
                {
                    result = "暂无数据";
                }
                string strlist = "{\"data\":[{\"cardid\":\"" + cardid + "\",\"cjihao\":\"" + cjihao + "\",\"mjihao\":" + mjihao + ",\"status\":1,\"time\":\"" + time + "\",\"output\":2}],\"code\":0,\"message\":\"成功\"}";
                systemLogmodel.SystemLogUUID = Guid.NewGuid();
                systemLogmodel.OperationTime = DateTime.Now;
                systemLogmodel.Type = "2";
                systemLogmodel.IsDelete = 0;
                systemLogmodel.AddTime = DateTime.Now;
                systemLogmodel.OperationContent = strlist + "&&&&&" + result;
                dBtlljfl.SystemLog.Add(systemLogmodel);
                dBtlljfl.SaveChanges();
                Response.Write(strlist);
            }
            else
            {
                string strlist = "{\"data\":[{\"cardid\":\"123456\",\"cjihao\":272546,\"mjihao\":0,\"status\":0,\"time\":\"1503708909\",\"output\":2}],\"code\":0,\"message\":\"\"}";
                Response.Write(strlist);
            }
        }

        public string Geturl(string url, string contentType = "application/json;charset=UTF-8")
        {
            HttpClient client = new HttpClient();

            var response = client.GetAsync(url);
            string result = "";
            if (response.Result.IsSuccessStatusCode)
            {
                Stream stream = response.Result.Content.ReadAsStreamAsync().Result;
                StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
                result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
                return result;
            }
            else
            {
                return result;
            }
        }


        public string Get1(string DeviceID, string Strcontent)
        {
            DeviceID = DeviceID.ToLower();

            if (string.IsNullOrEmpty(DeviceID) || string.IsNullOrEmpty(Strcontent))
            {
                return ("请求参数存在空值" + "deviceid" + DeviceID + "strcontent" + Strcontent);


            }
            else
            {
                if (Strcontent.Split('_')[0] == "d")
                {
                    string ddyuuid = Strcontent.Split('_')[1];
                    //根据设备获取厢房
                    var room = dBtlljfl.GrabageRoom.FirstOrDefault(x => x.Facilityuuid == DeviceID);
                    if (room == null)
                    {
                        return ("请先将设备绑定厢房");

                    }
                    var ddyentity = dBtlljfl.GrabageRoom.FirstOrDefault(x => x.GarbageRoomUUID == room.GarbageRoomUUID && x.IsDelete != "1");
                    if (ddyentity == null)
                    {
                        return ("暂无该厢房信息");
                    }
                    if (Guid.Parse(ddyuuid) != room.GarbageRoomUUID)
                    {
                        return ("您暂未拥有该厢房的赋分权限");
                    }

                    var query = dBtlljfl.GrabageDisposal.Where(x => x.IsDelete != "1").OrderByDescending(x => x.ID).First();
                    if (query.IsScore != "0")
                    {
                        return ("暂无赋分对象");

                    }
                    //赋分前分数
                    var hd = dBtlljfl.HomeAddress.FirstOrDefault(x => x.HomeAddressUUID == query.HomeAddressUUID);
                    var scoreSetting = dBtlljfl.ScoreSetting.First();
                    //加分
                    hd.Score = hd.Score + scoreSetting.Integral;
                    query.ScoreSettingUUid = scoreSetting.ScoreUUID;
                    query.ScoreAddtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    query.IsScore = "1";
                    dBtlljfl.SaveChanges();

                    return ("赋分成功");
                }
                else
                {
                    //二维码内容校验
                    var arr = Strcontent.Split('_');
                    if (arr == null || arr.Length < 2 || arr[0].Length != 1)
                    {
                        return ("此二维码内容有误");
                    }
                    if (arr[0] == "b")
                    {
                        var result = arr[1];
                        var idcard = Common(result);
                        var entity = dBtlljfl.SystemUser.FirstOrDefault(x => x.UserIdCard == idcard);
                        if (entity == null)
                        {
                            return (idcard);
                        }

                        //根据设备获取厢房
                        var room = dBtlljfl.GrabageRoom.FirstOrDefault(x => x.Facilityuuid == DeviceID);
                        if (room == null)
                        {
                            return ("请先将设备绑定厢房");
                        }
                        Guid puuid = entity.SystemUserUUID;
                        Guid huuid = entity.HomeAddressUUID.Value;
                        if (huuid == null || huuid == Guid.Empty)
                        {
                            return ("此用户未绑定家庭码或家庭码不存在");

                        }
                        var time = DateTime.Now.ToString("yyyy-MM-dd");
                        //当天赋分次数
                        var disposal = dBtlljfl.GrabageDisposal.Count(x => x.HomeAddressUUID == huuid && x.ScoreAddtime.Substring(0, 10)==time && x.IsScore=="1");
                        //获取设置的评分次数
                        var num = dBtlljfl.Overallsituation.First();
                        TimeSpan nowDt = DateTime.Now.TimeOfDay;
                        TimeSpan AmstartDt = DateTime.Parse("7:00").TimeOfDay;
                        TimeSpan AmendDt = DateTime.Parse("9:00").TimeOfDay;
                        TimeSpan PmstartDt = DateTime.Parse("18:00").TimeOfDay;
                        TimeSpan PmendDt = DateTime.Parse("20:00").TimeOfDay;
                        int datediff = 10;
                        var disposals = dBtlljfl.GrabageDisposal.Where(x => x.HomeAddressUUID == huuid && x.IsScore == "0");
                        if (disposals.Count() > 0)
                        {
                            var disposalss = disposals.OrderByDescending(x => x.ID).First();
                            TimeSpan a = (DateTime.Now - DateTime.Parse(disposalss.AddTime));
                            datediff = a.Seconds;
                        }
                        if (disposal >= num.SetNumber)
                        {
                            return ("今日该家庭赋分超过" + num.SetNumber + "次");

                        }
                        else if (datediff<10)
                        {
                            return ("间隔10秒后再赋分");
                        }
                        else if ((nowDt > AmstartDt && nowDt < AmendDt) || (nowDt > PmstartDt && nowDt < PmendDt))
                        {
                            var scoreSetting = dBtlljfl.ScoreSetting.First();
                            //添加赋分记录
                            GrabageDisposal gd = new GrabageDisposal();
                            gd.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            gd.IsDelete = "0";
                            gd.GarbageDisposalUUID = Guid.NewGuid();
                            gd.GrabageRoomID = room.GarbageRoomUUID;
                            gd.HomeAddressUUID = huuid;
                            gd.MarkType = "百姓码";
                            gd.IsScore = "0";
                            if (puuid != Guid.Empty)
                            {
                                gd.SystemUserUUID = puuid;
                            }
                            dBtlljfl.GrabageDisposal.Add(gd);
                            dBtlljfl.SaveChanges();
                        }
                        else
                        {
                            return ("未在投放时间投放");
                        }

                        return ("投放成功");
                    }
                    else
                    {
                        //string md5 = YunSendMsg.GenerateMD5(DeviceID+Strcontent);
                        //string md5 = YunSendMsg.GenerateMD5("deviceid" + DeviceID + "strcontent" + Strcontent + "8a261bcf2070c42865ec7619");
                        //md5 = YunSendMsg.GenerateMD5("deviceid" + DeviceID + "strcontent" + Strcontent+md5);
                        //if (md5 != Sign)
                        //{
                        //    return ("校验失败" + md5 + ":" + Sign + "(" + "deviceid" + DeviceID + "strcontent" + Strcontent + "8a261bcf2070c42865ec7619");
                        //     
                        //}

                        //根据设备获取厢房
                        var room = dBtlljfl.GrabageRoom.FirstOrDefault(x => x.Facilityuuid == DeviceID);
                        if (room == null)
                        {
                            return ("请先将设备绑定厢房");

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
                            huuid = dBtlljfl.SystemUser.FirstOrDefault(x => x.SystemUserUUID == Guid.Parse(arr[1])).HomeAddressUUID.Value;
                        }
                        var ent = dBtlljfl.HomeAddress.FirstOrDefault(x=>x.HomeAddressUUID==huuid);
                        if (huuid == null || huuid == Guid.Empty || ent==null)
                        {
                            return ("此用户未绑定家庭码或家庭码不存在");

                        }
                        var time = DateTime.Now.ToString("yyyy-MM-dd");
                        //当天赋分次数
                        var disposal = dBtlljfl.GrabageDisposal.Count(x => x.HomeAddressUUID == huuid && x.ScoreAddtime.Substring(0, 10) == time && x.IsScore == "1");
                        //获取设置的评分次数
                        var num = dBtlljfl.Overallsituation.First();
                        TimeSpan nowDt = DateTime.Now.TimeOfDay;
                        TimeSpan AmstartDt = DateTime.Parse("7:00").TimeOfDay;
                        TimeSpan AmendDt = DateTime.Parse("9:00").TimeOfDay;
                        TimeSpan PmstartDt = DateTime.Parse("18:00").TimeOfDay;
                        TimeSpan PmendDt = DateTime.Parse("20:00").TimeOfDay;
                        int datediff = 10;
                        var disposals = dBtlljfl.GrabageDisposal.Where(x => x.HomeAddressUUID == huuid  && x.IsScore == "0");
                        if (disposals.Count()>0)
                        {
                            var disposalss = disposals.OrderByDescending(x => x.ID).First();
                            TimeSpan a = (DateTime.Now - DateTime.Parse(disposalss.AddTime));
                            datediff = a.Seconds;
                        }
                        if (disposal >= num.SetNumber)
                        {
                            return ("今日该家庭赋分超过" + num.SetNumber + "次");

                        }
                        else if (datediff<10)
                        {
                            return ("间隔10秒后再赋分");
                        }
                        else if ((nowDt > AmstartDt && nowDt < AmendDt) || (nowDt > PmstartDt && nowDt < PmendDt))
                        {
                            //之前赋分记录 
                            //var all = from g in dBtlljfl.GrabageDisposal
                            //          join sc in dBtlljfl.ScoreSetting
                            //          on g.ScoreSettingUuid equals sc.ScoreUuid
                            //          where g.HomeAddressUuid == huuid
                            //          select new
                            //          {
                            //              sc.Integral
                            //          };
                            //之前赋分分数之和
                            //var sum = all.Sum(x => x.Integral.Value);
                            //获取设置的评分分数
                            //var scoreSetting = dBtlljfl.ScoreSetting.First(x => x.ScoreName == "好");
                            var scoreSetting = dBtlljfl.ScoreSetting.First();
                            //添加赋分记录
                            GrabageDisposal gd = new GrabageDisposal();
                            gd.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            gd.IsDelete = "0";
                            gd.GarbageDisposalUUID = Guid.NewGuid();
                            gd.GrabageRoomID = room.GarbageRoomUUID;
                            gd.ScoreSettingUUid = scoreSetting.ScoreUUID;
                            gd.HomeAddressUUID = huuid;
                            gd.IsScore = "0";
                            if (puuid != Guid.Empty)
                            {
                                gd.SystemUserUUID = puuid;
                            }
                            if (arr[0] == "h")
                            {
                                gd.MarkType = "家庭码";
                            }
                            dBtlljfl.GrabageDisposal.Add(gd);
                            dBtlljfl.SaveChanges();
                        }
                        else
                        {
                            return ("未在投放时间投放");

                        }

                        return ("投放成功");
                    }
                }

                return "";
            }
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
                var postData = "{" + '"' + "healthQrCode" + '"' + ":" + '"' + result + '"' + "}";
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
                if (commodel.code != 1 || commodel.data == null)
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

        public string Get2(string DeviceID, string card)
        {
            DeviceID = DeviceID.ToLower();
            card = card.ToLower();
            if (string.IsNullOrEmpty(card) || string.IsNullOrEmpty(DeviceID))
            {
                return ("请求参数存在空值" + "card" + card + "deviceid" + DeviceID);
            }
            else
            {
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
                var data = ToHttp<IDCardTerminal>.ToPost2("https://ypay.96225.com/openapi", messageJson);
                if (!data.success)
                {
                    return (data.respCode + ":" + data.respDesc);

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
                    return ("验签失败");
                }
                else
                {

                    //根据设备获取厢房
                    var room = dBtlljfl.GrabageRoom.FirstOrDefault(x => x.Facilityuuid == DeviceID);
                    if (room == null)
                    {
                        return ("请先将设备绑定厢房");

                    }
                    var user = dBtlljfl.SystemUser.FirstOrDefault(x => x.IDCardMD5 == idcard.idcard);
                    if (user == null)
                    {
                        return ("此用户未绑定市民卡号");

                    }
                    if (user.HomeAddressUUID == null)
                    {
                        return ("此用户未绑定家庭码");

                    }
                    var time = DateTime.Now.ToString("yyyy-MM-dd");
                    //当天赋分次数
                    var disposal = dBtlljfl.GrabageDisposal.Count(x => x.HomeAddressUUID == user.HomeAddressUUID && x.ScoreAddtime.Substring(0, 10) ==time && x.IsScore == "1");
                    //获取设置的评分次数
                    var num = dBtlljfl.Overallsituation.First();
                    TimeSpan nowDt = DateTime.Now.TimeOfDay;
                    TimeSpan AmstartDt = DateTime.Parse("7:00").TimeOfDay;
                    TimeSpan AmendDt = DateTime.Parse("9:00").TimeOfDay;
                    TimeSpan PmstartDt = DateTime.Parse("18:00").TimeOfDay;
                    TimeSpan PmendDt = DateTime.Parse("20:00").TimeOfDay;
                    int datediff = 10;
                    var disposals = dBtlljfl.GrabageDisposal.Where(x => x.HomeAddressUUID == user.HomeAddressUUID && x.IsScore == "0");
                    if (disposals.Count() > 0)
                    {
                        var disposalss = disposals.OrderByDescending(x => x.ID).First();
                        TimeSpan a = (DateTime.Now - DateTime.Parse(disposalss.AddTime));
                        datediff = a.Seconds;
                    }
                    if (disposal >= num.SetNumber)
                    {
                        return ("今日该家庭赋分超过" + num.SetNumber + "次");

                    }
                    else if (datediff < 10)
                    {
                        return ("间隔10秒后再赋分");
                    }
                    else if ((nowDt > AmstartDt && nowDt < AmendDt) || (nowDt > PmstartDt && nowDt < PmendDt))
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
                        var scoreSetting = dBtlljfl.ScoreSetting.First();
                        //添加赋分记录
                        GrabageDisposal gd = new GrabageDisposal();
                        gd.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        gd.IsDelete = "0";
                        gd.GarbageDisposalUUID = Guid.NewGuid();
                        gd.GrabageRoomID = room.GarbageRoomUUID;
                        gd.ScoreSettingUUid = scoreSetting.ScoreUUID;
                        gd.HomeAddressUUID = user.HomeAddressUUID;
                        gd.SystemUserUUID = user.SystemUserUUID;
                        gd.IsScore = "0";
                        gd.MarkType = "市民卡";
                        dBtlljfl.GrabageDisposal.Add(gd);

                        dBtlljfl.SaveChanges();
                    }
                    else
                    {
                        return ("未在投放时间投放");

                    }
                    return ("投放成功");
                }
            }
        }
    }
}
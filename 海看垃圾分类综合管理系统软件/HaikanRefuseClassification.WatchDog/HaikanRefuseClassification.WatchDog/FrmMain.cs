using Haikan.DebugTools;
using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Haikan.DBHelper;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

using System.Security.Cryptography;
using System.Linq;

using HaiKanTaskHousekeeperCore.Data;
using System.Configuration;
using System.Linq.Expressions;
using Senparc.Weixin.TenPay.V3;
using log4net.Appender;
using System.Web;
using HaiKanTaskHousekeeperCore;
using HaiKanTaskHousekeeperWatchDog.Model;
using SmartCampusManagementWatchDog.Model;
using System.ComponentModel;

namespace HaiKanTaskHousekeeperWatchDog
{
    public partial class FrmMain : Form
    {
        #region 实例化对象
        //实例化Timer类，设置间隔时间为1秒；  
        System.Timers.Timer t = new System.Timers.Timer(1000 * 60 * 60 * 24);
        System.Timers.Timer t2 = new System.Timers.Timer(1000 * 60 * 10);
        System.Timers.Timer t3 = new System.Timers.Timer(1000 * 60 * 60 * 24 );
        System.Timers.Timer t4 = new System.Timers.Timer(5000);
        //System.Timers.Timer t3 = new System.Timers.Timer(1000);

        Model1 context_m = new Model1();
        Model1 context_m2 = new Model1();
        Model1 context_m3 = new Model1();
        Model1 context_m4 = new Model1();
        List<Car> car = new List<Car>();
        List<GrabageWeighting> Weighting = new List<GrabageWeighting>();
        Dictionary<string, int> dic = new Dictionary<string, int>();
        int num = 0;
        int ingcount = 0;

        #endregion
        public FrmMain()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Console.ForegroundColor = ConsoleColor.Green;
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            //var st = DateTime.Now;
            car = context_m.Car.Where(x => x.AddPeople == "狗").ToList();

            //AddLog("看门狗", "" + typeof(FrmMain) + "", "看门狗程序启动");
            LogHelper.WriteLog(typeof(FrmMain), "看门狗程序启动！");
            //到达时间的时候执行事件； 
            t2.Elapsed += timerDo_Tick2;
            t.Elapsed += timerDo_Tick;
            t3.Elapsed += timerDo_Tick3;
            t4.Elapsed += timerDo_Tick4;
            //设置是执行一次（false）还是一直执行(true)； 
            t2.AutoReset = true;
            t.AutoReset = true;
            t3.AutoReset = true;
            t4.AutoReset = true;
            //t2.AutoReset = false;

            //是否执行System.Timers.Timer.Elapsed事件；  
            t2.Enabled = true;
            t.Enabled = true;
            t3.Enabled = true;
            t4.Enabled = true;
            //AddLog("看门狗", "" + typeof(FrmMain) + "", "看门狗程序启动完毕！");
            LogHelper.WriteLog(typeof(FrmMain), "看门狗程序启动完毕！");

            //var et = DateTime.Now;
            //var td = et - st;
            //Console.WriteLine(td.ToString("g"));

            //string result = Encrypt("123456", "eda53bf9af5f48749208139abb190231");
        }

        /// <summary>
        /// 获取称重信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerDo_Tick2(object sender, ElapsedEventArgs e)
        {
            try
            {
                lock (context_m2)
                {
                    var time = DateTime.Now;
                    Console.WriteLine(time);
                    Console.WriteLine(time.Second);

                    if (time.Second % 2 == 0)
                    {
                        Console.WriteLine("跳出2");
                        LogHelper.WriteLog(typeof(FrmMain), "非时间段2");
                        return;
                    }
                    LogHelper.WriteLog(typeof(FrmMain), "计时器事件触发2");
                    LogHelper.WriteLog(typeof(FrmMain), time.ToString("yyyyy-MM-dd HH:mm:ss:fff"));
                    t.Enabled = false;



                    Console.WriteLine(car);

                    #region 车辆信息
                    for (int i = 0; i < car.Count; i++)
                    {
                        #region 车辆称重参数
                        //发送的参数
                        string appSecure = "ed6824f1-18e0-11eb-a0a0-00163e0fab85";
                        string appKey = "ed6824d9-18e0-11eb-a0a0-00163e0fab85";
                        MD5 md5 = new MD5CryptoServiceProvider();
                        string apiName2 = "CarWeightData";
                        string timestamp2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        byte[] result3 = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(appKey + apiName2 + timestamp2 + appSecure));
                        string sign2 = BitConverter.ToString(result3).Replace("-", "").ToLower();
                        byte[] result4 = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(timestamp2));
                        string nonceApi2 = BitConverter.ToString(result4).Replace("-", "").ToLower();
                        string beginDate = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
                        string endDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        #endregion

                        #region 称重信息
                        string carNo = "";
                        string carType = "";
                        carNo = car[i].CarNum;
                        carType = car[i].GrabType;
                        LogHelper.WriteLog(typeof(FrmMain), carNo);
                        LogHelper.WriteLog(typeof(FrmMain), timestamp2);
                        #region 称重参数
                        var request2 = (HttpWebRequest)WebRequest.Create("http://47.96.2.154/api/CloudApi/CarExchangeController/CarWeightData?appKey=" + appKey + "&apiName=" + apiName2 + "&timestamp=" + HttpUtility.UrlEncode(timestamp2, Encoding.UTF8) + "&" + "sign=" + sign2 + "&nonceApi=" + nonceApi2 + "&carNo=" + carNo + "&beginDate=" + beginDate + "&endDate=" + endDate);
                        request2.Method = "POST";

                        //结果返回
                        var responses2 = (HttpWebResponse)request2.GetResponse();
                        //转字符串
                        var responseString2 = new StreamReader(responses2.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                        Console.WriteLine(i);

                        //转换为json对象
                        var commode2 = JsonConvert.DeserializeObject<weightinfo>(responseString2);
                        #endregion

                        if (commode2.bean == null) continue;

                        for (var j = 0; j < commode2.bean.Count; j++)
                        {
                            Console.WriteLine(j);
                            Console.WriteLine(timestamp2);
                            var id = Guid.Parse(commode2.bean[j].ID);
                            var carNumber = commode2.bean[j].CarNumber;
                            var weightTime = commode2.bean[j].WeightTime;
                            var wTimeSub = weightTime.Substring(0, 11);
                            var addTime = commode2.bean[j].AddTime;
                            #region 称重数据
                            var lightweight = context_m2.GrabageWeighting.FirstOrDefault(x => x.CarNumber==carNumber && x.IsDelete != "1" && x.AddTime == addTime || x.WeightTime==weightTime);
                            var featherweight = context_m2.GrabageWeighting.Where(x => x.CarNumber == carNumber && x.IsDelete != "1" && x.WeightTime.Contains(wTimeSub));
                            if (featherweight.Count()>0)
                            {
                                var fWeight = featherweight.OrderByDescending(x => x.ID).First();
                                if (lightweight == null && fWeight.Weight != commode2.bean[j].Weight.ToString())
                                {
                                    Console.WriteLine(commode2.bean[j].Weight.ToString());
                                    Console.WriteLine(fWeight.Weight);
                                    var weightModel = new GrabageWeighting()
                                    {
                                        GrabageWeighingRecordUUID = Guid.NewGuid(),
                                        WeightUUID = id,
                                        CarNumber = commode2.bean[j].CarNumber,
                                        AddTime = commode2.bean[j].AddTime,
                                        Weight = commode2.bean[j].Weight.ToString(),
                                        Type = carType,
                                        RecordType = commode2.bean[j].RecordType,
                                        IsDelete = "0",
                                        IsCheck = 0,
                                        CarUUID = car[i].CarUUID,
                                        WeightTime = commode2.bean[j].WeightTime
                                    };
                                    context_m2.GrabageWeighting.Add(weightModel);

                                    if (Convert.ToDouble(fWeight.Weight) > commode2.bean[j].Weight && Convert.ToDateTime(fWeight.WeightTime) > Convert.ToDateTime(commode2.bean[j].WeightTime) && commode2.bean[j].Weight != 0)
                                    {
                                        Console.WriteLine("筛选" + commode2.bean[j].CarNumber);
                                        var weightModelSon = new GrabageWeightSon()
                                        {
                                            GrabageWeighingRecordUUID = Guid.NewGuid(),
                                            WeightUUID = id,
                                            CarNumber = commode2.bean[j].CarNumber,
                                            AddTime = commode2.bean[j].AddTime,
                                            Weight = (Convert.ToDouble(fWeight.Weight) - commode2.bean[j].Weight).ToString("0.00"),
                                            Type = carType,
                                            RecordType = commode2.bean[j].RecordType,
                                            IsDelete = "0",
                                            IsCheck = 0,
                                            CarUUID = car[i].CarUUID,
                                            WeightTime = commode2.bean[j].WeightTime
                                        };
                                        context_m2.GrabageWeightSon.Add(weightModelSon);
                                    }
                                    else if(Convert.ToDouble(fWeight.Weight)==0)
                                    {
                                        Console.WriteLine("筛选" + commode2.bean[j].CarNumber);
                                        var weightModelSon = new GrabageWeightSon()
                                        {
                                            GrabageWeighingRecordUUID = Guid.NewGuid(),
                                            WeightUUID = id,
                                            CarNumber = commode2.bean[j].CarNumber,
                                            AddTime = commode2.bean[j].AddTime,
                                            Weight = (commode2.bean[j].Weight).ToString(),
                                            Type = carType,
                                            RecordType = commode2.bean[j].RecordType,
                                            IsDelete = "0",
                                            IsCheck = 0,
                                            CarUUID = car[i].CarUUID,
                                            WeightTime = commode2.bean[j].WeightTime
                                        };
                                        context_m2.GrabageWeightSon.Add(weightModelSon);
                                    }
                                   
                                    context_m2.SaveChanges();
                                }
                            }
                            else
                            {
                                var weightModel = new GrabageWeighting()
                                {
                                    GrabageWeighingRecordUUID = Guid.NewGuid(),
                                    WeightUUID = id,
                                    CarNumber = commode2.bean[j].CarNumber,
                                    AddTime = commode2.bean[j].AddTime,
                                    Weight = commode2.bean[j].Weight.ToString(),
                                    Type = carType,
                                    RecordType = commode2.bean[j].RecordType,
                                    IsDelete = "0",
                                    IsCheck = 0,
                                    CarUUID = car[i].CarUUID,
                                    WeightTime = commode2.bean[j].WeightTime
                                };
                                context_m2.GrabageWeighting.Add(weightModel);
                                context_m2.SaveChanges();
                            }
                            
                            #endregion

                            #region 称重数据筛选
                            //var weighting = context_m2.GrabageWeightSon.FirstOrDefault(x => x.WeightTime == weightTime && x.CarNumber == carNumber && x.IsDelete != "1");
                            //var getWeight = context_m2.GrabageWeightSon.Where(x => x.CarNumber == carNumber && x.IsDelete != "1");
                            //if (getWeight.Any())
                            //{
                            //     var firstWeight= getWeight.OrderByDescending(x => x.WeightTime).First();
                            //     if (weighting != null && firstWeight.Weight == commode2.bean[j].Weight.ToString())
                            //     {
                            //         if (Convert.ToDouble(firstWeight.Weight) > commode2.bean[j].Weight)
                            //         {
                            //             firstWeight.Weight = commode2.bean[j].Weight.ToString();
                            //             firstWeight.WeightTime = commode2.bean[j].WeightTime;
                            //             firstWeight.AddTime = commode2.bean[j].AddTime;
                            //             firstWeight.Type = commode2.bean[j].TYPE;
                            //             firstWeight.WeightUUID = id;
                            //             firstWeight.RecordType = commode2.bean[j].RecordType;
                            //             context_m2.SaveChanges();
                            //         }
                            //         else
                            //         {
                            //             var weightModel = new GrabageWeighting()
                            //             {
                            //                 GrabageWeighingRecordUUID = Guid.NewGuid(),
                            //                 WeightUUID = id,
                            //                 CarNumber = commode2.bean[j].CarNumber,
                            //                 AddTime = commode2.bean[j].AddTime,
                            //                 Weight = commode2.bean[j].Weight.ToString(),
                            //                 Type = commode2.bean[j].TYPE,
                            //                 RecordType = commode2.bean[j].RecordType,
                            //                 IsDelete = "0",
                            //                 IsCheck = 0,
                            //                 CarUUID = car[i].CarUUID,
                            //                 WeightTime = commode2.bean[j].WeightTime
                            //             };
                            //             context_m2.GrabageWeighting.Add(weightModel);
                            //             context_m2.SaveChanges();
                            //         }
                            //     }
                            //}
                            //else
                            //{
                            //    var weightModel = new GrabageWeightSon()
                            //    {
                            //        GrabageWeighingRecordUUID = Guid.NewGuid(),
                            //        WeightUUID = id,
                            //        CarNumber = commode2.bean[j].CarNumber,
                            //        AddTime = commode2.bean[j].AddTime,
                            //        Weight = commode2.bean[j].Weight.ToString(),
                            //        Type = commode2.bean[j].TYPE,
                            //        RecordType = commode2.bean[j].RecordType,
                            //        IsDelete = "0",
                            //        IsCheck = 0,
                            //        CarUUID = car[i].CarUUID,
                            //        WeightTime = commode2.bean[j].WeightTime
                            //    };
                            //    context_m2.GrabageWeightSon.Add(weightModel);
                            //    context_m2.SaveChanges();
                            //}
                            #endregion

                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FrmMain), "错误2:" + ex.Message);
                Console.WriteLine(ex.Message);
                //t.Enabled = true;
            }
            t.Enabled = true;
        }


        /// <summary>
        /// 查询相应的表，执行相应的硬件控制方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerDo_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                lock (context_m)
                {
                    var time = DateTime.Now;
                    //Console.WriteLine(time);
                    //Console.WriteLine(time.Second);


                    if (time.Second % 2 == 1)
                    {
                        Console.WriteLine("跳出1");
                        LogHelper.WriteLog(typeof(FrmMain), "非时间段1");
                        return;
                    }
                    LogHelper.WriteLog(typeof(FrmMain), "计时器事件触发");
                    LogHelper.WriteLog(typeof(FrmMain), time.ToString("yyyyy-MM-dd HH:mm:ss:fff"));
                    t.Enabled = false;

                    #region 车辆称重参数
                    //发送的参数
                    string appSecure = "ed6824f1-18e0-11eb-a0a0-00163e0fab85";
                    string appKey = "ed6824d9-18e0-11eb-a0a0-00163e0fab85";
                    string apiName = "BaseCarData";
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(appKey + apiName + timestamp + appSecure));
                    string sign = BitConverter.ToString(result).Replace("-", "").ToLower();
                    byte[] result2 = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(timestamp));
                    string nonceApi = BitConverter.ToString(result2).Replace("-", "").ToLower();
                    string carNo = "";

                    #endregion

                    #region 车辆信息
                    var request = (HttpWebRequest)WebRequest.Create("http://47.96.2.154/api/CloudApi/CarExchangeController/BaseCarData?appKey=" + appKey + "&apiName=" + apiName + "&timestamp=" + HttpUtility.UrlEncode(timestamp, Encoding.UTF8) + "&" + "sign=" + sign + "&nonceApi=" + nonceApi + "&carNo=");
                    request.Method = "POST";
                    //结果返回
                    var responses = (HttpWebResponse)request.GetResponse();
                    //转字符串
                    var responseString = new StreamReader(responses.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    //转换为json对象
                    carinfo commodel = JsonConvert.DeserializeObject<carinfo>(responseString);
                    if (commodel.bean.Count > 0)
                    {
                        for (int i = 0; i < commodel.bean.Count; i++)
                        {
                            carNo = commodel.bean[i].CarNum;
                            var GrabType = commodel.bean[i].GrabType;
                            var getcar = context_m.Car.FirstOrDefault(x => x.CarNum == carNo);
                            if (getcar == null)
                            {
                                Guid cuuid = Guid.NewGuid();
                                Car carmodel = new Car()
                                {
                                    CarUUID = cuuid,
                                    CarNum = carNo,
                                    BalanceID = commodel.bean[i].ID,
                                    CarType = commodel.bean[i].CarType,
                                    AddPeople = "狗",
                                    AddTime = DateTime.Now.ToString("yyyy-MM-dd"),
                                    IsDelete = 0,
                                    RegisterTime = commodel.bean[i].ADDTIME,
                                    company = commodel.bean[i].Company,
                                    Photo = commodel.bean[i].Photo,
                                    GrabType = GrabType == "可回收垃圾"?"3": GrabType=="有害垃圾" ?"2": GrabType=="易腐垃圾" ?"1": GrabType == "易腐垃圾"?"0":""
                                };
                                Console.WriteLine(carNo);
                                context_m.Car.Add(carmodel);
                                var getcarM = context_m.CarMonitoring.FirstOrDefault(x => x.MonitoringNum == carNo);
                                if (getcarM == null)
                                {
                                    CarMonitoring carMmodel = new CarMonitoring()
                                    {
                                        CarUUID = cuuid,
                                        MonitoringNum = carNo,
                                        CarMonitoringUUID = Guid.NewGuid(),
                                        AddTime = DateTime.Now.ToString("yyyy-MM-dd"),
                                        IsDelete = 0
                                    };
                                    context_m.CarMonitoring.Add(carMmodel);
                                }
                                context_m.SaveChanges();
                            }
                            else
                            {
                                LogHelper.WriteLog(typeof(FrmMain), carNo);
                                var getcarM = context_m.CarMonitoring.FirstOrDefault(x => x.MonitoringNum == carNo);
                                if (getcarM == null)
                                {
                                    CarMonitoring carMmodel = new CarMonitoring()
                                    {
                                        CarUUID = getcar.CarUUID,
                                        MonitoringNum = carNo,
                                        CarMonitoringUUID = Guid.NewGuid(),
                                        AddTime = DateTime.Now.ToString("yyyy-MM-dd"),
                                        IsDelete = 0
                                    };
                                    Console.WriteLine(carNo);
                                    context_m.CarMonitoring.Add(carMmodel);
                                    context_m.SaveChanges();
                                }
                            }
                        }
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FrmMain), "错误:" + ex.Message);
                //t.Enabled = true;
            }
            t.Enabled = true;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        /// <summary>
        /// 使用 MD5  对字符串  进行加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// 
        public static string GetMD5Hash(string str)
        {
            StringBuilder result = new StringBuilder();
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {

                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                int length = data.Length;
                for (int i = 0; i < length; i++)
                    result.Append(data[i].ToString("x2"));

            }
            return result.ToString();
        }

        /// <summary>
        /// 每天获取地址库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerDo_Tick3(object sender, ElapsedEventArgs e)
        {
            try
            {
                lock (context_m)
                {
                    var code = "0";
                    var time = DateTime.Now;

                    LogHelper.WriteLog(typeof(FrmMain), "计时器事件触发3");
                    LogHelper.WriteLog(typeof(FrmMain), time.ToString("yyyyy-MM-dd HH:mm:ss:fff"));
                    t.Enabled = false;

                    #region 
                    //发送的参数
                    var timeStamp = GetTimeStamp();
                    var timepass = timeStamp.ToString() + "123";
                    string valPassword = GetMD5Hash(timepass);
                    string serviceUrl = "http://10.33.79.173:8090/addrMatch/auth/getToken?" + "user=ljfl&time=" + timeStamp + "&secret=" + valPassword;
                    //创建Web访问对象
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
                    myRequest.ContentType = "application/json";
                    //通过Web访问对象获取响应内容
                    HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                    //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
                    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    string returnJson = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
                    code = myResponse.StatusCode.ToString();
                    reader.Close();
                    myResponse.Close();
                    #endregion
                    var data = JsonConvert.DeserializeObject<Token>(returnJson);
                    string token = "";
                    if (data != null)
                    {
                        token = data.data.token;
                        using (context_m3)
                        {
                            var list1 = context_m3.Village.Select(x => new { x.VName }).ToList();
                            for (int i = 0; i < list1.Count; i++)
                            {
                                #region 参数信息
                                List<HomeAddress> addresses = new List<HomeAddress>();
                                string infoUrl = "http://10.33.79.173:8090/addrMatch/addrApi/searchAddr?token=" + token + "&addr=" + list1[i].VName + "&page=1&limit=9999&fuzzy=false";
                                HttpWebRequest myRequest1 = (HttpWebRequest)WebRequest.Create(infoUrl);
                                myRequest1.ContentType = "application/json";
                                HttpWebResponse myResponse1 = (HttpWebResponse)myRequest1.GetResponse();
                                StreamReader reader1 = new StreamReader(myResponse1.GetResponseStream(), Encoding.UTF8);
                                string returnJson1 = reader1.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
                                code = myResponse1.StatusCode.ToString();
                                reader1.Close();
                                myResponse1.Close();
                                #endregion
                                var data1 = JsonConvert.DeserializeObject<AddressInfo>(returnJson1);
                                for (int j = 0; j < data1.data.addrList.Count; j++)
                                {
                                    var addr = data1.data.addrList[j].addr;
                                    var homeentity = context_m3.HomeAddress.Any(x => x.Address == addr);
                                    if (!homeentity)
                                    {
                                        HomeAddress homeAddress = new HomeAddress();
                                        homeAddress.Address = data1.data.addrList[j].addr;
                                        homeAddress.Addresscode = data1.data.addrList[j].code;
                                        homeAddress.town = data1.data.addrList[j].town;
                                        homeAddress.ccmmunity = data1.data.addrList[j].community;
                                        homeAddress.squad = data1.data.addrList[j].squad;
                                        homeAddress.village = data1.data.addrList[j].village;
                                        homeAddress.szone = data1.data.addrList[j].szone;
                                        homeAddress.street = data1.data.addrList[j].street;
                                        homeAddress.door = data1.data.addrList[j].door;
                                        homeAddress.resregion = data1.data.addrList[j].resregion;
                                        homeAddress.building = data1.data.addrList[j].building;
                                        homeAddress.building_num = data1.data.addrList[j].building_num;
                                        homeAddress.unit = data1.data.addrList[j].unit;
                                        homeAddress.floor = data1.data.addrList[j].floor;
                                        homeAddress.room = data1.data.addrList[j].room;
                                        homeAddress.Score = 0;
                                        homeAddress.room_floor = data1.data.addrList[j].room_floor;

                                        homeAddress.HomeAddressUUID = Guid.NewGuid();
                                        //_dbContext.HomeAddress.Add(homeAddress);
                                        //_dbContext.SaveChanges();
                                        addresses.Add(homeAddress);
                                        LogHelper.WriteLog(typeof(FrmMain), data1.data.addrList[j].addr);
                                    }
                                }
                                context_m3.HomeAddress.AddRange(addresses);
                                context_m3.SaveChanges();
                                //await _dbContext.AddRangeAsync(addresses);
                                LogHelper.WriteLog(typeof(FrmMain), list1[i].VName);
                            }
                        }
                    }
                    LogHelper.WriteLog(typeof(FrmMain), "计时器事件结束3");
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FrmMain), "错误3:" + ex.Message);
                Console.WriteLine(ex.Message);
                //t.Enabled = true;
            }
            t.Enabled = true;
        }

        /// <summary>
        /// 每5秒获取一次轨迹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerDo_Tick4(object sender, ElapsedEventArgs e)
        {
            try
            {
                lock (context_m4)
                {
                    var time = DateTime.Now;
                    LogHelper.WriteLog(typeof(FrmMain), "计时器事件触发4");
                    LogHelper.WriteLog(typeof(FrmMain), time.ToString("yyyyy-MM-dd HH:mm:ss:fff"));
                    t.Enabled = false;

                    #region 定位认证
                    //发送的参数
                    string url = "http://47.96.2.154/clbs/oauth/token";
                    model1 user = new model1();
                    user.client_id = "mobile_1";
                    user.client_secret = "secret_1";
                    user.grant_type = "password";
                    user.scope = "read";
                    user.username = "zhengfu_api";
                    user.password = "lanhai123!@#";
                    var strPostData = ToPaeameter(user);
                    var strResponse = PostUrl(url, strPostData);
                    TrackToken usermodel = JsonConvert.DeserializeObject<TrackToken>(strResponse);
                    #endregion

                    #region 获取定位
                    //发送参数
                    var name = string.Empty;
                    var access_token = usermodel.value;
                    for (int i = 0; i < car.Count; i++)
                    {
                        name = car[i].CarNum;
                        var request = (HttpWebRequest)WebRequest.Create("http://47.96.2.154/clbs/swagger/m/monitoring/monitor/location/latest?name=" + name + "&access_token=" + access_token);
                        request.Method = "GET";
                        //结果返回
                        var responses = (HttpWebResponse)request.GetResponse();
                        //转字符串
                        var responseString = new StreamReader(responses.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                        //转换为json对象
                        Track commodel = JsonConvert.DeserializeObject<Track>(responseString);
                        if (!string.IsNullOrEmpty(commodel.data[0].longitude.ToString()) && !string.IsNullOrEmpty(commodel.data[0].latitude.ToString()))
                        {
                            Guid caruuid = car[i].CarUUID;
                            var log = ((double)commodel.data[0].longitude / (double)1000000).ToString();
                            var lat = ((double)commodel.data[0].latitude / (double)1000000).ToString();

                            #region 经纬度转换
                            var jwdrequest = (HttpWebRequest)WebRequest.Create("http://api.map.baidu.com/geoconv/v1/?coords="+log+","+lat+"&from=1&to=5&ak=nSxiPohfziUaCuONe4ViUP2N");
                            request.Method = "GET";
                            //结果返回
                            var jwdresponses = (HttpWebResponse)jwdrequest.GetResponse();
                            //转字符串
                            var jwdresponseString = new StreamReader(jwdresponses.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                            //转换为json对象
                            JWDBaidu jwdcommodel = JsonConvert.DeserializeObject<JWDBaidu>(jwdresponseString);
                            #endregion

                            if (jwdcommodel.status==0) {
                                var addtime = DateTime.Now.ToString("yyyy-MM-dd");
                                var carweight = context_m4.VehicleInfo.Where(x => x.CarUUID == caruuid && x.Addtime.Contains(addtime));
                                if (carweight.Count() > 0)
                                {
                                    var query = carweight.OrderByDescending(x => x.Addtime).First();
                                    if (query.Lon != jwdcommodel.result[0].x.ToString() || query.Lat != jwdcommodel.result[0].y.ToString())
                                    {
                                        VehicleInfo model = new VehicleInfo();
                                        model.VehicleInfo1 = Guid.NewGuid();
                                        model.CarUUID = caruuid;
                                        model.Lat = jwdcommodel.result[0].y.ToString();
                                        model.Lon = jwdcommodel.result[0].x.ToString();
                                        model.Addtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        context_m4.VehicleInfo.Add(model);
                                        context_m4.SaveChanges();
                                    }
                                }
                                else
                                {
                                    VehicleInfo model = new VehicleInfo();
                                    model.VehicleInfo1 = Guid.NewGuid();
                                    model.CarUUID = caruuid;
                                    model.Lat = jwdcommodel.result[0].y.ToString();
                                    model.Lon = jwdcommodel.result[0].x.ToString();
                                    model.Addtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    context_m4.VehicleInfo.Add(model);
                                    context_m4.SaveChanges();
                                }
                            }
                        }
                    }
                    #endregion
                }


            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FrmMain), "错误:" + ex.Message);
                //t.Enabled = true;
            }
            t.Enabled = true;
        }

        public string PostUrl(string url, string postData)
        {
            string result = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                req.Method = "POST";

                req.ContentType = "application/x-www-form-urlencoded";

                req.Timeout = 800;//请求超时时间

                byte[] data = Encoding.UTF8.GetBytes(postData);

                req.ContentLength = data.Length;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);

                    reqStream.Close();
                }

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                Stream stream = resp.GetResponseStream();

                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception e) {
                LogHelper.WriteLog(typeof(FrmMain), "错误:" + e.Message);
            }

            return result;
        }

        /// <summary>
        /// 参数拼接Url
        /// </summary>
        /// <param name="source">要拼接的实体</param>
        /// <paramref name="IsStrUpper">是否开启首字母大写,默认小写</paramref>
        /// <returns>Url,</returns>
        public string ToPaeameter(object source)
        {
            var buff = new StringBuilder(string.Empty);
            if (source == null)
                throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                object value = property.GetValue(source);
                if (value != null)
                {
                    buff.Append(WebUtility.UrlEncode(property.Name) + "=" + WebUtility.UrlEncode(value + "") + "&");
                }
            }
            return buff.ToString().Trim('&');
        }
    }
}

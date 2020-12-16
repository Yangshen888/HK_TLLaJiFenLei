using System;
using System.Collections.Generic;
using System.Data;

using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Haikan3.Utils;
using HaikanRefuseClassification.Api.Configurations;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.Extensions;
using HaikanRefuseClassification.Api.Extensions.AuthContext;
using HaikanRefuseClassification.Api.RequestPayload.Person;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
//using Microsoft.Office.Interop.Word;
//using MsWord = Microsoft.Office.Interop.Word;
using System.Reflection;
using Newtonsoft.Json;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System.Drawing;
using Microsoft.Data.SqlClient;
using Section = Spire.Doc.Section;
using Document = Spire.Doc.Document;
using NPOI.XWPF.UserModel;

namespace HaikanRefuseClassification.Api.Controllers.Api.V1.Person
{
    [Route("api/v1/Person/[controller]/[action]")]
    [ApiController]
    public class AddressController : ControllerBase
    {

        private readonly RefuseClassificationContext _dbContext;
        private readonly IMapper _mapper;
        //用来获取路径相关
        private IHostingEnvironment _hostingEnvironment;

        public AddressController(RefuseClassificationContext dbContext, IMapper mapper, IOptions<MdDesEncrypt> mdDesEncrypt, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 地址显示
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(AddressRequestPayload payload)
       {
            using (_dbContext)
            {
                var user=AuthContextService.CurrentUser;
                string s1 = payload.Kw2;
                string[] s2 = s1.Split(" ");
                StringBuilder where = new StringBuilder();
                StringBuilder sql = new StringBuilder("SELECT * FROM HomeAddress HA ");
                for (int i = 0; i<s2.Length; i++) {
                    if (s2.Length > 0 && i == 0)
                    {
                        where.Append("WHERE ");
                    }
                    where.Append("HA.Address LIKE '%" + s2[i] + "%' ");
                    if (i >= 0 && i < s2.Length-1)
                    {
                        where.Append(" AND ");
                    }
                }
                sql.Append(where);
                //var query = _dbContext.HomeAdd.FromSql(sql.ToString());
                var query =  _dbContext.HomeAddress.FromSqlRaw(sql.ToString());

                if (AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community == "" && AuthContextService.CurrentUser.Biotope != "")
                {
                    var Biotope = AuthContextService.CurrentUser.Biotope.Split(',');
                    query = query.Where(x => Biotope.Contains(x.Resregion));
                }
                else if (AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "" && AuthContextService.CurrentUser.Biotope == "")
                {
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    query = query.Where(x => Community.Contains(x.Ccmmunity));
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "" && AuthContextService.CurrentUser.Biotope == "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    query = query.Where(x => Streets.Contains(x.Town));
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community != "" && AuthContextService.CurrentUser.Biotope == "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    query = query.Where(x => Streets.Contains(x.Town) || Community.Contains(x.Ccmmunity));
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community == "" && AuthContextService.CurrentUser.Biotope != "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Biotope = AuthContextService.CurrentUser.Biotope.Split(',');
                    query = query.Where(x => Streets.Contains(x.Town) || Biotope.Contains(x.Resregion));
                }
                else if (AuthContextService.CurrentUser.Streets == "" && AuthContextService.CurrentUser.Community != "" && AuthContextService.CurrentUser.Biotope != "")
                {
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    var Biotope = AuthContextService.CurrentUser.Biotope.Split(',');
                    query = query.Where(x => Community.Contains(x.Ccmmunity) || Biotope.Contains(x.Resregion));
                }
                else if (AuthContextService.CurrentUser.Streets != "" && AuthContextService.CurrentUser.Community != "" && AuthContextService.CurrentUser.Biotope != "")
                {
                    var Streets = AuthContextService.CurrentUser.Streets.Split(',');
                    var Community = AuthContextService.CurrentUser.Community.Split(',');
                    var Biotope = AuthContextService.CurrentUser.Biotope.Split(',');
                    query = query.Where(x => Streets.Contains(x.Town) || Community.Contains(x.Ccmmunity) || Biotope.Contains(x.Resregion));
                }
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Address.Contains(payload.Kw));
                }
                if (!string.IsNullOrEmpty(payload.Kw1))
                {
                    query = query.Where(x => x.Address.Contains(payload.Kw1));
                }
                if (!string.IsNullOrEmpty(payload.Kw3))
                {
                    query = query.Where(x => x.Address.Contains(payload.Kw3));
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.Address);
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }


        /// 添加地址
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            string address = model.address;
            string addresscode = model.addresscode;
            using (_dbContext)
            {
                var entity = new HaikanRefuseClassification.Api.Entities.HomeAddress();
                entity.HomeAddressUuid = Guid.NewGuid();
                entity.Address = address;
                entity.Addresscode = "T" + addresscode;
                entity.Resregion = model.resregion;
                entity.Town = model.town;
                entity.Ccmmunity = model.ccmmunity;
                _dbContext.HomeAddress.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult GetEdit(string guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.HomeAddress.FirstOrDefault(x => x.HomeAddressUuid.ToString() == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }
        /// <summary>
        /// 保存编辑(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(dynamic model)
        {
            string uuid = model.homeAddressUuid;
            string address = model.address;
            string addresscode = model.addresscode;
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.HomeAddress.FirstOrDefault(x => x.HomeAddressUuid == Guid.Parse(uuid));
                entity.Address = address;
                if (entity.Addresscode!= addresscode)
                {
                    if (addresscode != "")
                    {
                        entity.Addresscode = "T" + addresscode;
                    }
                    else
                    {
                        entity.Addresscode = "";
                    }
                }
                entity.Town = model.town;
                entity.Ccmmunity = model.ccmmunity;
                entity.Resregion = model.resregion;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }


        //根据乡镇(街道)获取社区
        [HttpPost]
        public IActionResult Huoqushequ()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = from v in _dbContext.Village
                            select new
                            {
                                v.Vname,
                                v.Towns,
                                v.VillageUuid,
                                v.Address
                            };
                var entiy = query.ToList();
                response.SetData(entiy);
                return Ok(response);
            }
        }


        [HttpGet]
        public IActionResult GetFamily(string vill)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = from h in _dbContext.HomeAddress
                            where h.Ccmmunity== vill
                            select new
                            {
                                h.Street,
                                h.Door,
                                h.Resregion,
                                h.BuildingNum,
                                h.Unit,
                                h.Floor,
                                h.Room,
                                h.RoomFloor,
                                h.Ccmmunity
                            };
                var entiy = query.ToList();
                response.SetData(entiy);
                return Ok(response);
            }
        }

        [HttpGet]
        public IActionResult GetFamily1(string vill)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                if(vill!= "undefined")
                {
                    var entity = _dbContext.Village.FirstOrDefault(x => x.VillageUuid == Guid.Parse(vill));
                    var query = from h in _dbContext.HomeAddress
                                where h.Ccmmunity == entity.Vname
                                select new
                                {
                                    h.Street,
                                    h.Door,
                                    h.Resregion,
                                    h.BuildingNum,
                                    h.Unit,
                                    h.Floor,
                                    h.Room,
                                    h.RoomFloor,
                                    h.Ccmmunity
                                };
                    var entiy = query.ToList();
                    response.SetData(entiy);
                    return Ok(response);
                }
                else
                {
                    response.SetData("1");
                    return Ok(response);
                }
                
            }
        }




        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult shopInfoImport(IFormFile excelfile)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                DateTime beginTime = DateTime.Now;

                string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoExcel";


                //var schoolinfo = _dbContext.SchoolInforManagement.AsQueryable();
                string uploadtitle = "地址信息导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string sFileName = $"{uploadtitle}.xlsx";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                //string conStr = ConnectionStrings.DefaultConnection;
                string responsemsgsuccess = "";
                string responsemsgrepeat = "";
                string responsemsgdefault = "";
                int successcount = 0;
                int repeatcount = 0;
                int defaultcount = 0;
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                try
                {
                    //把excelfile中的数据复制到file中
                    using (FileStream fs = new FileStream(file.ToString(), FileMode.Create)) //初始化一个指定路径和创建模式的FileStream
                    {
                        excelfile.CopyTo(fs);
                        fs.Flush();  //清空stream的缓存，并且把缓存中的数据输出到file
                    }
                    System.Data.DataTable dt = Haikan3.Utils.ExcelTools.ExcelToDataTable(file.ToString(), "Sheet1", true);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        response.SetFailed("表格无数据");
                        return Ok(response);
                    }
                    else
                    {
                        if (!dt.Columns.Contains("地址(请详细到门牌号)"))
                        {
                            response.SetFailed("无‘地址(请详细到门牌号)’列");
                            return Ok(response);
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            var entity = new HomeAddress();
                            entity.HomeAddressUuid = Guid.NewGuid();
                            if (!string.IsNullOrEmpty(dt.Rows[i]["地址(请详细到门牌号)"].ToString()))
                            {
                                var a = dt.Rows[i]["地址(请详细到门牌号)"].ToString();
                                var user = _dbContext.HomeAddress.FirstOrDefault(x => x.Address == dt.Rows[i]["地址(请详细到门牌号)"].ToString());

                                if (user == null)
                                {
                                    entity.Address = dt.Rows[i]["地址(请详细到门牌号)"].ToString();
                                }
                                else
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行地址已存在" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行地址为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["地址编码"].ToString()))
                            {
                                entity.Addresscode = "T" + dt.Rows[i]["地址编码"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["街道"].ToString()))
                            {
                                entity.Town = dt.Rows[i]["街道"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行街道为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["社区"].ToString()))
                            {
                                entity.Ccmmunity = dt.Rows[i]["社区"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行社区为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["小区"].ToString()))
                            {
                                entity.Resregion = dt.Rows[i]["小区"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行小区为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            _dbContext.HomeAddress.Add(entity);
                            _dbContext.SaveChanges();
                            successcount++;
                        }
                    }
                    responsemsgsuccess = "<p style='color:green'>导入成功:" + successcount + "条</p></br>" + responsemsgsuccess;
                    responsemsgrepeat = "<p style='color:orange'>重复需手动修改数据:" + repeatcount + "条</p></br>" + responsemsgrepeat;
                    responsemsgdefault = "<p style='color:red'>导入失败:" + defaultcount + "条</p></br>" + responsemsgdefault;


                    DateTime endTime = DateTime.Now;
                    TimeSpan useTime = endTime - beginTime;
                    string taketime = "导入时间" + useTime.TotalSeconds.ToString() + "秒  ";
                    response.SetData(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new
                    {
                        time = taketime,
                        successmsg = responsemsgsuccess
                        ,
                        repeatmsg = responsemsgrepeat,
                        defaultmsg = responsemsgdefault
                    })));
                    return Ok(response);
                }
                catch (Exception ex)
                {

                    response.SetFailed(ex.Message);
                    return Ok(response);
                }
            }
        }

        
        /// <summary>
        /// 根据查询结果一键导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult yjExportInfo(AddressRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateInstance;

            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoWord\\";
            var timeInfo = DateTime.Now.ToString("yyyyMMddHHmmss");
            var wordName = timeInfo + "address.docx";
            string wordUrl = sWebRootFolder + wordName;
            MemoryStream ms = new MemoryStream();
            XWPFDocument m_Docx = new XWPFDocument();
            m_Docx.Write(ms);
            ms.Flush();
            SaveToFile(ms, wordUrl);

            List<string> list = new List<string>();
            using (_dbContext)
            {
                Document document111 = new Document();
                document111.LoadFromFile(wordUrl);
                Section section = document111.Sections[0];
                section.Paragraphs[0].AppendBookmarkStart("picture");
                section.Paragraphs[0].AppendBookmarkEnd("picture");

                var user = AuthContextService.CurrentUser;
                string s1 = payload.Kw2;
                string[] s2 = s1.Split(" ");
                StringBuilder where = new StringBuilder();
                StringBuilder sql = new StringBuilder("SELECT * FROM HomeAddress HA ");
                for (int i = 0; i < s2.Length; i++)
                {
                    if (s2.Length > 0 && i == 0)
                    {
                        where.Append("WHERE ");
                    }
                    where.Append("HA.Address LIKE '%" + s2[i] + "%' ");
                    if (i >= 0 && i < s2.Length - 1)
                    {
                        where.Append(" AND ");
                    }
                }
                sql.Append(where);
                //var query = _dbContext.HomeAdd.FromSql(sql.ToString());
                var query = _dbContext.HomeAddress.FromSqlRaw(sql.ToString());
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Address.ToString().Contains(payload.Kw));
                }
                if (!string.IsNullOrEmpty(payload.Kw1))
                {
                    query = query.Where(x => x.Address.ToString().Contains(payload.Kw1));
                }
                var entities = query.ToList();
                //var entities = _dbContext.HomeAddress.Where(x => ids.IndexOf(x.HomeAddressUuid.ToString()) >= 0).ToList();
                for (int i = 0; i < entities.Count(); i++)
                {
                    var pata = _hostingEnvironment.WebRootPath + EWM.GetEWM2("h_" + entities[i].HomeAddressUuid.ToString(), _hostingEnvironment, entities[i].Address);
                    //实例化BookmarksNavigator类,指定需要添加图片的书签“”
                    BookmarksNavigator bn = new BookmarksNavigator(document111);
                    bn.MoveToBookmark("picture", true, true);
                    //添加段落，加载图片并插入到段落
                    Section section0 = document111.AddSection();
                    Spire.Doc.Documents.Paragraph paragraph = section0.AddParagraph();
                    Image image = Image.FromFile(pata);
                    DocPicture picture = document111.Sections[0].Paragraphs[0].AppendPicture(image);
                    picture.Width = 160;
                    picture.Height = 180;
                    //picture.HorizontalPosition = 100.0f;
                    //picture.VerticalPosition = 50.0f;
                    bn.InsertParagraph(paragraph);
                    document111.Sections.Remove(section0);
                    //string output = sWebRootFolder + "test.docx";
                    document111.SaveToFile(wordUrl, FileFormat.Docx);
                    //Arraypata = pata.;
                    //list.Add(pata);

                }
                list.Add(wordUrl);
                //关闭进程
                document111.Dispose();
                var time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var check = ZIP.CompressMulti(list, _hostingEnvironment.WebRootPath + "\\UploadFiles\\EWM\\" + time, false);
                if (check)
                {
                    response.SetSuccess("导出成功");
                    response.SetData("\\UploadFiles\\EWM\\" + time + ".zip");
                }
                else
                {
                    response.SetFailed("导出失败");
                }
            }
            return Ok(response);
        }



        //public IActionResult Batch(string command, string ids)
        //{
        //    var response = ResponseModelFactory.CreateInstance;
        //    List<string> list = new List<string>();
        //    using (_dbContext)
        //    {
        //        var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
        //        var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
        //        var entities = _dbContext.HomeAddress.Where(x => ids.IndexOf(x.HomeAddressUuid.ToString()) >= 0).ToList();
        //        for (int i = 0; i < entities.Count; i++)
        //        {
        //            var pata = _hostingEnvironment.WebRootPath + EWM.GetEWM2("h_" + entities[i].HomeAddressUuid.ToString(), _hostingEnvironment, entities[i].Address);
        //            //Arraypata = pata.;
        //            list.Add(pata);
        //        }
        //        var time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //        var check = ZIP.CompressMulti(list, _hostingEnvironment.WebRootPath + "\\UploadFiles\\EWM\\" + time, false);
        //        if (check)
        //        {
        //            response.SetSuccess("导出成功");
        //            response.SetData("\\UploadFiles\\EWM\\" + time + ".zip");
        //        }
        //        else
        //        {
        //            response.SetFailed("导出失败");
        //        }
        //    }
        //    return Ok(response);
        //}

        /// <summary>
        /// 多张或单张生成并导出二维码
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]

        public IActionResult Batch(string command, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoWord\\";
            var timeInfo = DateTime.Now.ToString("yyyyMMddHHmmss");
            var wordName = timeInfo + "address.docx";
            string wordUrl = sWebRootFolder + wordName;
            MemoryStream ms = new MemoryStream();
            XWPFDocument m_Docx = new XWPFDocument();
            m_Docx.Write(ms);
            ms.Flush();
            SaveToFile(ms, wordUrl);
            List<string> list = new List<string>();


            using (_dbContext)
            {
                //document111.LoadFromFile(sWebRootFolder + "test.docx");
                Document document111 = new Document();
                document111.LoadFromFile(wordUrl);
                Section section = document111.Sections[0];
                document111.Watermark = null;
                section.Paragraphs[0].AppendBookmarkStart("picture");
                section.Paragraphs[0].AppendBookmarkEnd("picture");
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var entities = _dbContext.HomeAddress.Where(x => ids.IndexOf(x.HomeAddressUuid.ToString()) >= 0).ToList();
                for (int i = 0; i < entities.Count; i++)
                {
                    var pata = _hostingEnvironment.WebRootPath + EWM.GetEWM2("h_" + entities[i].HomeAddressUuid.ToString(), _hostingEnvironment, entities[i].Address);              
                    //实例化BookmarksNavigator类,指定需要添加图片的书签“”
                    BookmarksNavigator bn = new BookmarksNavigator(document111);
                    bn.MoveToBookmark("picture", true, true);
                    //添加段落，加载图片并插入到段落
                    Section section0 = document111.AddSection();
                    Spire.Doc.Documents.Paragraph paragraph = section0.AddParagraph();
                    Image image = Image.FromFile(pata);

                    //DocPicture picture = paragraph.AppendPicture(image);
                    DocPicture picture = document111.Sections[0].Paragraphs[0].AppendPicture(image);
                    picture.Width = 160;
                    picture.Height = 180;
                    //picture.HorizontalPosition = 100.0f;
                    //picture.VerticalPosition = 100.0f;
                    bn.InsertParagraph(paragraph);
                    document111.Sections.Remove(section0);
                    //string output = sWebRootFolder + "test.docx";
                    document111.SaveToFile(wordUrl, FileFormat.Docx);
                    
                }
                list.Add(wordUrl);
                //关闭进程
                document111.Dispose();
                var time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var check = ZIP.CompressMulti(list, _hostingEnvironment.WebRootPath + "\\UploadFiles\\EWM\\" + time, false);
                if (check)
                {
                    response.SetSuccess("导出成功");
                    response.SetData("\\UploadFiles\\EWM\\" + time + ".zip");
                }
                else
                {
                    response.SetFailed("导出失败");
                }
                return Ok(response);
            }
        }

        public static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();
                data = null;
            }
        }
        public void GengerateWord()
        {

            //string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoWord\\";
            //Object Nothing = System.Reflection.Missing.Value;
            //Directory.CreateDirectory(sWebRootFolder);  //创建文件所在目录
            //string name = "家庭码" + DateTime.Now.ToShortDateString() + ".docx";
            //object filename = sWebRootFolder + name;  //文件保存路径
            ////创建Word文档
            //MsWord.Application WordApp = new MsWord.Application();
            //MsWord.Document WordDoc = WordApp.Documents.Add();
            //WordDoc.SaveAs();
            //WordDoc.Close();
            //WordApp.Quit();


            //string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoWord\\";
            //var timeInfo = DateTime.Now.ToString("yyyyMMddHHmmss");
            //string wordName = timeInfo + "address.docx";
            //string wordUrl = sWebRootFolder + wordName;
            //Document document2 = new Document(@wordUrl);
            //document2.SaveToFile(wordName, FileFormat.Docx);

            //object path1;//文件路径
            //string strContent;//文件内容
            //MsWord.Application wordApp;//Word应用程序变量
            //MsWord.Document wordDoc;//Word文档变量
            //path1 = sWebRootFolder + "address.docx";//保存为Word文档
            //wordApp = new MsWord.ApplicationClass();//初始化
            ////由于使用的是COM 库，因此有许多变量需要用Missing.Value 代替
            //object Nothing = Missing.Value;
            ////新建一个word对象
            //wordDoc = wordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            ////WdSaveDocument为Word2003文档的保存格式(文档后缀.doc)\wdFormatDocumentDefault为Word2007的保存格式(文档后缀.docx)
            //object format = MsWord.WdSaveFormat.wdFormatDocument;
            ////将wordDoc 文档对象的内容保存为DOC 文档,并保存到path指定的路径
            //wordDoc.SaveAs(ref path1, ref format, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            ////关闭wordDoc文档
            //wordDoc.Close(ref Nothing, ref Nothing, ref Nothing);
            ////关闭wordApp组件对象
            //wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);

            //string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoWord\\";
            //Document document = new Document();
            //document.LoadFromFile(sWebRootFolder + "test.docx");
            ////实例化BookmarksNavigator类,指定需要添加图片的书签“”
            //BookmarksNavigator bn = new BookmarksNavigator(document);
            //bn.MoveToBookmark("picture", true, true);
            ////添加段落，加载图片并插入到段落
            //Section section0 = document.AddSection();
            //Spire.Doc.Documents.Paragraph paragraph = section0.AddParagraph();
            //Image image = Image.FromFile("D:\\timg.jpg");
            //DocPicture picture = paragraph.AppendPicture(image);
            //bn.InsertParagraph(paragraph);
            //document.Sections.Remove(section0);
            ////保存文档
            //string output = sWebRootFolder + "test.docx";
            //document.SaveToFile(output, FileFormat.Docx);
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public IActionResult BtnTokenAsync()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var code = "0";
            try
            {

                var timeStamp = GetTimeStamp();
                var timepass = timeStamp.ToString() + "123";
                string valPassword = GetMD5Hash(timepass);
                string serviceUrl = "http://10.33.79.173:8090/addrMatch/auth/getToken?" + "user=ljfl&time=" + timeStamp + "&secret=" + valPassword;
                //创建Web访问对象
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
                //把用户传过来的数据转成“UTF-8”的字节流
                //byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data);

                //myRequest.Method = "GET";
                //myRequest.ContentLength = buf.Length;
                myRequest.ContentType = "application/json";
                //myRequest.MaximumAutomaticRedirections = 1;
                //myRequest.AllowAutoRedirect = true;
                //发送请求
                //Stream stream = myRequest.GetRequestStream();
                //stream.Write(buf, 0, buf.Length);
                //stream.Close();

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
                var data = JsonConvert.DeserializeObject<ViewModels.TkenInfo.Token>(returnJson);
                string token = "";
                if (data != null)
                {
                    token = data.data.token;
                    using (_dbContext)
                    {
                        var list1 = _dbContext.Village.Select(x => new { x.Vname }).ToList();
                        for (int i = 0; i < list1.Count; i++)
                        {

                            List<HomeAddress> addresses = new List<HomeAddress>();
                            string infoUrl = "http://10.33.79.173:8090/addrMatch/addrApi/searchAddr?token=" + token +"&addr=" + list1[i].Vname + "&page=1&limit=9999&fuzzy=false";
                            HttpWebRequest myRequest1 = (HttpWebRequest)WebRequest.Create(infoUrl);
                            myRequest1.ContentType = "application/json";
                            HttpWebResponse myResponse1 = (HttpWebResponse)myRequest1.GetResponse();
                            StreamReader reader1 = new StreamReader(myResponse1.GetResponseStream(), Encoding.UTF8);
                            string returnJson1 = reader1.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
                            code = myResponse1.StatusCode.ToString();
                            reader1.Close();
                            myResponse1.Close();
                            var data1 = JsonConvert.DeserializeObject<ViewModels.AddresInfo.AddressInfo>(returnJson1);
                            //response.SetData(data1);
                            //return Ok(response);
                            for (int j = 0; j < data1.data.addrList.Count; j++)
                            {
                                HomeAddress homeAddress = new HomeAddress();
                                homeAddress.Address = data1.data.addrList[j].addr;
                                homeAddress.Addresscode = data1.data.addrList[j].code;
                                homeAddress.Town = data1.data.addrList[j].town;
                                homeAddress.Ccmmunity = data1.data.addrList[j].community;
                                homeAddress.Squad = data1.data.addrList[j].squad;
                                homeAddress.Village = data1.data.addrList[j].village;
                                homeAddress.Szone = data1.data.addrList[j].szone;
                                homeAddress.Street = data1.data.addrList[j].street;
                                homeAddress.Door = data1.data.addrList[j].door;
                                homeAddress.Resregion = data1.data.addrList[j].resregion;
                                homeAddress.Building = data1.data.addrList[j].building;
                                homeAddress.BuildingNum = data1.data.addrList[j].building_num;
                                homeAddress.Unit = data1.data.addrList[j].unit;
                                homeAddress.Floor = data1.data.addrList[j].floor;
                                homeAddress.Room = data1.data.addrList[j].room;
                                homeAddress.Score = 0.0;
                                homeAddress.RoomFloor = data1.data.addrList[j].room_floor;

                                homeAddress.HomeAddressUuid = Guid.NewGuid();
                                //_dbContext.HomeAddress.Add(homeAddress);
                                //_dbContext.SaveChanges();
                                addresses.Add(homeAddress);


                            }
                            _dbContext.AddRange(addresses);
                            _dbContext.SaveChanges();
                            //await _dbContext.AddRangeAsync(addresses);
                        }

                    }

                }
                return Ok(response);


            }
            catch (Exception ex)
            {
                response.SetFailed("xxxxxxxx");
                response.SetData(ex.Message);
                return Ok(response);
            }

            

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

        //验证
        public static bool VerifyMD5Hash(string str, string hash)
        {
            string hashOfInput = GetMD5Hash(str);

            if (hashOfInput.CompareTo(hash) == 0)
                return true;
            else
                return false;
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


    }
}
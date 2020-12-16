using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaikanDemo.qa
{
    /// <summary>
    /// mcardsea 的摘要说明
    /// </summary>
    public class mcardsea : IHttpHandler
    {
        //系统日志表
        HaikanDemo.BLL.SystemLog SystemLogbll = new BLL.SystemLog();
        Model.SystemLog systemLogmodel = new Model.SystemLog();

        public void ProcessRequest(HttpContext context)
        {            
            context.Response.ContentType = "text/plain";
            string cardid = context.Request.QueryString["cardid"].ToString();
            systemLogmodel.SystemLogUUID = Guid.NewGuid();
            systemLogmodel.OperationTime = DateTime.Now;
            systemLogmodel.Type = "3";
            systemLogmodel.IsDelete = 0;
            systemLogmodel.AddTime = DateTime.Now;
            systemLogmodel.OperationContent = cardid;
            SystemLogbll.Add(systemLogmodel);
            string strlist = "{\"data\":[{\"cardid\":\"123456\",\"cjihao\":\"272546\",\"mjihao\":1,\"status\":1,\"tine\":\"1503708909\",\"output\":2}],\"code\":0,\"message\":\"\"}";
            context.Response.Write(strlist);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
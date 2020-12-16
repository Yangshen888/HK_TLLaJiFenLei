using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Haikan.DBHelper;
namespace HaikanRefuseClassificationCore.DAL
{
	/// <summary>
	/// 数据访问类:GrabageMonitoring
	/// </summary>
	public partial class GrabageMonitoring
	{
		public GrabageMonitoring()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(Guid GrabageMonitoringUUID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from GrabageMonitoring");
			strSql.Append(" where GrabageMonitoringUUID=@GrabageMonitoringUUID ");
			SqlParameter[] parameters = {
					new SqlParameter("@GrabageMonitoringUUID", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = GrabageMonitoringUUID;

			return DbHelperSql.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(HaikanRefuseClassificationCore.Model.GrabageMonitoring model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into GrabageMonitoring(");
			strSql.Append("GrabageMonitoringUUID,MonitoringNum,GarbageRoomUUID,PalyType,SvrIp,SvrPort,appkey,appSecret,time,timeSecret,httpsflag,CamList,remark,AddTime,IsDelete,RegionId,jiankongName,longitude,latitude,VideoUrl)");
			strSql.Append(" values (");
			strSql.Append("@GrabageMonitoringUUID,@MonitoringNum,@GarbageRoomUUID,@PalyType,@SvrIp,@SvrPort,@appkey,@appSecret,@time,@timeSecret,@httpsflag,@CamList,@remark,@AddTime,@IsDelete,@RegionId,@jiankongName,@longitude,@latitude,@VideoUrl)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@GrabageMonitoringUUID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@MonitoringNum", SqlDbType.NVarChar,255),
					new SqlParameter("@GarbageRoomUUID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@PalyType", SqlDbType.NVarChar,255),
					new SqlParameter("@SvrIp", SqlDbType.NVarChar,255),
					new SqlParameter("@SvrPort", SqlDbType.NVarChar,255),
					new SqlParameter("@appkey", SqlDbType.NVarChar,255),
					new SqlParameter("@appSecret", SqlDbType.NVarChar,255),
					new SqlParameter("@time", SqlDbType.NVarChar,255),
					new SqlParameter("@timeSecret", SqlDbType.NVarChar,255),
					new SqlParameter("@httpsflag", SqlDbType.NVarChar,255),
					new SqlParameter("@CamList", SqlDbType.NVarChar,255),
					new SqlParameter("@remark", SqlDbType.NVarChar,255),
					new SqlParameter("@AddTime", SqlDbType.NVarChar,255),
					new SqlParameter("@IsDelete", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@jiankongName", SqlDbType.NVarChar,255),
					new SqlParameter("@longitude", SqlDbType.NVarChar,255),
					new SqlParameter("@latitude", SqlDbType.NVarChar,255),
					new SqlParameter("@VideoUrl", SqlDbType.NVarChar,-1)};
			parameters[0].Value = Guid.NewGuid();
			parameters[1].Value = model.MonitoringNum;
			parameters[2].Value = Guid.NewGuid();
			parameters[3].Value = model.PalyType;
			parameters[4].Value = model.SvrIp;
			parameters[5].Value = model.SvrPort;
			parameters[6].Value = model.appkey;
			parameters[7].Value = model.appSecret;
			parameters[8].Value = model.time;
			parameters[9].Value = model.timeSecret;
			parameters[10].Value = model.httpsflag;
			parameters[11].Value = model.CamList;
			parameters[12].Value = model.remark;
			parameters[13].Value = model.AddTime;
			parameters[14].Value = model.IsDelete;
			parameters[15].Value = model.RegionId;
			parameters[16].Value = model.jiankongName;
			parameters[17].Value = model.longitude;
			parameters[18].Value = model.latitude;
			parameters[19].Value = model.VideoUrl;

			object obj = DbHelperSql.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(HaikanRefuseClassificationCore.Model.GrabageMonitoring model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update GrabageMonitoring set ");
			strSql.Append("MonitoringNum=@MonitoringNum,");
			
			strSql.Append("PalyType=@PalyType,");
			strSql.Append("SvrIp=@SvrIp,");
			strSql.Append("SvrPort=@SvrPort,");
			strSql.Append("appkey=@appkey,");
			strSql.Append("appSecret=@appSecret,");
			strSql.Append("time=@time,");
			strSql.Append("timeSecret=@timeSecret,");
			strSql.Append("httpsflag=@httpsflag,");
			strSql.Append("CamList=@CamList,");
			strSql.Append("remark=@remark,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("IsDelete=@IsDelete,");
			strSql.Append("RegionId=@RegionId,");
			strSql.Append("jiankongName=@jiankongName,");
			strSql.Append("longitude=@longitude,");
			strSql.Append("latitude=@latitude,");
			strSql.Append("VideoUrl=@VideoUrl");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@MonitoringNum", SqlDbType.NVarChar,255),
					new SqlParameter("@GarbageRoomUUID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@PalyType", SqlDbType.NVarChar,255),
					new SqlParameter("@SvrIp", SqlDbType.NVarChar,255),
					new SqlParameter("@SvrPort", SqlDbType.NVarChar,255),
					new SqlParameter("@appkey", SqlDbType.NVarChar,255),
					new SqlParameter("@appSecret", SqlDbType.NVarChar,255),
					new SqlParameter("@time", SqlDbType.NVarChar,255),
					new SqlParameter("@timeSecret", SqlDbType.NVarChar,255),
					new SqlParameter("@httpsflag", SqlDbType.NVarChar,255),
					new SqlParameter("@CamList", SqlDbType.NVarChar,255),
					new SqlParameter("@remark", SqlDbType.NVarChar,255),
					new SqlParameter("@AddTime", SqlDbType.NVarChar,255),
					new SqlParameter("@IsDelete", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@jiankongName", SqlDbType.NVarChar,255),
					new SqlParameter("@longitude", SqlDbType.NVarChar,255),
					new SqlParameter("@latitude", SqlDbType.NVarChar,255),
					new SqlParameter("@VideoUrl", SqlDbType.NVarChar,-1),
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@GrabageMonitoringUUID", SqlDbType.UniqueIdentifier,16)};
			parameters[0].Value = model.MonitoringNum;
			parameters[1].Value = model.GarbageRoomUUID;
			parameters[2].Value = model.PalyType;
			parameters[3].Value = model.SvrIp;
			parameters[4].Value = model.SvrPort;
			parameters[5].Value = model.appkey;
			parameters[6].Value = model.appSecret;
			parameters[7].Value = model.time;
			parameters[8].Value = model.timeSecret;
			parameters[9].Value = model.httpsflag;
			parameters[10].Value = model.CamList;
			parameters[11].Value = model.remark;
			parameters[12].Value = model.AddTime;
			parameters[13].Value = model.IsDelete;
			parameters[14].Value = model.RegionId;
			parameters[15].Value = model.jiankongName;
			parameters[16].Value = model.longitude;
			parameters[17].Value = model.latitude;
			parameters[18].Value = model.VideoUrl;
			parameters[19].Value = model.ID;
			parameters[20].Value = model.GrabageMonitoringUUID;

			int rows=DbHelperSql.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from GrabageMonitoring ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			int rows=DbHelperSql.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(Guid GrabageMonitoringUUID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from GrabageMonitoring ");
			strSql.Append(" where GrabageMonitoringUUID=@GrabageMonitoringUUID ");
			SqlParameter[] parameters = {
					new SqlParameter("@GrabageMonitoringUUID", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = GrabageMonitoringUUID;

			int rows=DbHelperSql.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string IDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from GrabageMonitoring ");
			strSql.Append(" where ID in ("+IDlist + ")  ");
			int rows=DbHelperSql.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public HaikanRefuseClassificationCore.Model.GrabageMonitoring GetModel(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,GrabageMonitoringUUID,MonitoringNum,GarbageRoomUUID,PalyType,SvrIp,SvrPort,appkey,appSecret,time,timeSecret,httpsflag,CamList,remark,AddTime,IsDelete,RegionId,jiankongName,longitude,latitude,VideoUrl from GrabageMonitoring ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			HaikanRefuseClassificationCore.Model.GrabageMonitoring model=new HaikanRefuseClassificationCore.Model.GrabageMonitoring();
			DataSet ds=DbHelperSql.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public HaikanRefuseClassificationCore.Model.GrabageMonitoring DataRowToModel(DataRow row)
		{
			HaikanRefuseClassificationCore.Model.GrabageMonitoring model=new HaikanRefuseClassificationCore.Model.GrabageMonitoring();
			if (row != null)
			{
				if(row["ID"]!=null && row["ID"].ToString()!="")
				{
					model.ID=int.Parse(row["ID"].ToString());
				}
				if(row["GrabageMonitoringUUID"]!=null && row["GrabageMonitoringUUID"].ToString()!="")
				{
					model.GrabageMonitoringUUID= new Guid(row["GrabageMonitoringUUID"].ToString());
				}
				if(row["MonitoringNum"]!=null)
				{
					model.MonitoringNum=row["MonitoringNum"].ToString();
				}
				if(row["GarbageRoomUUID"]!=null && row["GarbageRoomUUID"].ToString()!="")
				{
					model.GarbageRoomUUID= new Guid(row["GarbageRoomUUID"].ToString());
				}
				if(row["PalyType"]!=null)
				{
					model.PalyType=row["PalyType"].ToString();
				}
				if(row["SvrIp"]!=null)
				{
					model.SvrIp=row["SvrIp"].ToString();
				}
				if(row["SvrPort"]!=null)
				{
					model.SvrPort=row["SvrPort"].ToString();
				}
				if(row["appkey"]!=null)
				{
					model.appkey=row["appkey"].ToString();
				}
				if(row["appSecret"]!=null)
				{
					model.appSecret=row["appSecret"].ToString();
				}
				if(row["time"]!=null)
				{
					model.time=row["time"].ToString();
				}
				if(row["timeSecret"]!=null)
				{
					model.timeSecret=row["timeSecret"].ToString();
				}
				if(row["httpsflag"]!=null)
				{
					model.httpsflag=row["httpsflag"].ToString();
				}
				if(row["CamList"]!=null)
				{
					model.CamList=row["CamList"].ToString();
				}
				if(row["remark"]!=null)
				{
					model.remark=row["remark"].ToString();
				}
				if(row["AddTime"]!=null)
				{
					model.AddTime=row["AddTime"].ToString();
				}
				if(row["IsDelete"]!=null && row["IsDelete"].ToString()!="")
				{
					model.IsDelete=int.Parse(row["IsDelete"].ToString());
				}
				if(row["RegionId"]!=null && row["RegionId"].ToString()!="")
				{
					model.RegionId=int.Parse(row["RegionId"].ToString());
				}
				if(row["jiankongName"]!=null)
				{
					model.jiankongName=row["jiankongName"].ToString();
				}
				if(row["longitude"]!=null)
				{
					model.longitude=row["longitude"].ToString();
				}
				if(row["latitude"]!=null)
				{
					model.latitude=row["latitude"].ToString();
				}
				if(row["VideoUrl"]!=null)
				{
					model.VideoUrl=row["VideoUrl"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ID,GrabageMonitoringUUID,MonitoringNum,GarbageRoomUUID,PalyType,SvrIp,SvrPort,appkey,appSecret,time,timeSecret,httpsflag,CamList,remark,AddTime,IsDelete,RegionId,jiankongName,longitude,latitude,VideoUrl ");
			strSql.Append(" FROM GrabageMonitoring ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSql.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" ID,GrabageMonitoringUUID,MonitoringNum,GarbageRoomUUID,PalyType,SvrIp,SvrPort,appkey,appSecret,time,timeSecret,httpsflag,CamList,remark,AddTime,IsDelete,RegionId,jiankongName,longitude,latitude,VideoUrl ");
			strSql.Append(" FROM GrabageMonitoring ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSql.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM GrabageMonitoring ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSql.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.ID desc");
			}
			strSql.Append(")AS Row, T.*  from GrabageMonitoring T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSql.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "GrabageMonitoring";
			parameters[1].Value = "ID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSql.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}


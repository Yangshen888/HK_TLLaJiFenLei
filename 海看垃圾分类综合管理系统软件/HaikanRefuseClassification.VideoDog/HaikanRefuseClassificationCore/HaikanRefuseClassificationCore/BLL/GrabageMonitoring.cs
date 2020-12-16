using Haikan.DataTools;
using System;
using System.Data;
using System.Collections.Generic;
using HaikanRefuseClassificationCore.Model;
namespace HaikanRefuseClassificationCore.BLL
{
	/// <summary>
	/// �����᷿���
	/// </summary>
	public partial class GrabageMonitoring
	{
		private readonly HaikanRefuseClassificationCore.DAL.GrabageMonitoring dal=new HaikanRefuseClassificationCore.DAL.GrabageMonitoring();
		public GrabageMonitoring()
		{}
		#region  BasicMethod
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(Guid GrabageMonitoringUUID)
		{
			return dal.Exists(GrabageMonitoringUUID);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(HaikanRefuseClassificationCore.Model.GrabageMonitoring model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(HaikanRefuseClassificationCore.Model.GrabageMonitoring model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int ID)
		{
			
			return dal.Delete(ID);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(Guid GrabageMonitoringUUID)
		{
			
			return dal.Delete(GrabageMonitoringUUID);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string IDlist )
		{
			return dal.DeleteList(PageValidate.SafeLongFilter(IDlist,0) );
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public HaikanRefuseClassificationCore.Model.GrabageMonitoring GetModel(int ID)
		{
			
			return dal.GetModel(ID);
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
		/// </summary>
		public HaikanRefuseClassificationCore.Model.GrabageMonitoring GetModelByCache(int ID)
		{
			
			string CacheKey = "GrabageMonitoringModel-" + ID;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(ID);
					if (objModel != null)
					{
						int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (HaikanRefuseClassificationCore.Model.GrabageMonitoring)objModel;
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// ���ǰ��������
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<HaikanRefuseClassificationCore.Model.GrabageMonitoring> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<HaikanRefuseClassificationCore.Model.GrabageMonitoring> DataTableToList(DataTable dt)
		{
			List<HaikanRefuseClassificationCore.Model.GrabageMonitoring> modelList = new List<HaikanRefuseClassificationCore.Model.GrabageMonitoring>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				HaikanRefuseClassificationCore.Model.GrabageMonitoring model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// ��ҳ��ȡ�����б�
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// ��ҳ��ȡ�����б�
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
		/// <summary>
		/// ��ҳ��ȡ�����б�
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}


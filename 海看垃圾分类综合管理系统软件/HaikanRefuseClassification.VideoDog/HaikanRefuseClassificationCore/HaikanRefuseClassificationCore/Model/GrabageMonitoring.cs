using System;
namespace HaikanRefuseClassificationCore.Model
{
	/// <summary>
	/// À¬»øÏá·¿¼à¿Ø
	/// </summary>
	[Serializable]
	public partial class GrabageMonitoring
	{
		public GrabageMonitoring()
		{}
		#region Model
		private int _id;
		private Guid _grabagemonitoringuuid;
		private string _monitoringnum;
		private Guid _garbageroomuuid;
		private string _palytype;
		private string _svrip;
		private string _svrport;
		private string _appkey;
		private string _appsecret;
		private string _time;
		private string _timesecret;
		private string _httpsflag;
		private string _camlist;
		private string _remark;
		private string _addtime;
		private int? _isdelete;
		private int? _regionid;
		private string _jiankongname;
		private string _longitude;
		private string _latitude;
		private string _videourl;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// Ïá·¿¼à¿ØUUID
		/// </summary>
		public Guid GrabageMonitoringUUID
		{
			set{ _grabagemonitoringuuid=value;}
			get{return _grabagemonitoringuuid;}
		}
		/// <summary>
		/// ¼à¿Ø±àºÅ
		/// </summary>
		public string MonitoringNum
		{
			set{ _monitoringnum=value;}
			get{return _monitoringnum;}
		}
		/// <summary>
		/// À¬»øÏá·¿UUID
		/// </summary>
		public Guid GarbageRoomUUID
		{
			set{ _garbageroomuuid=value;}
			get{return _garbageroomuuid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PalyType
		{
			set{ _palytype=value;}
			get{return _palytype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SvrIp
		{
			set{ _svrip=value;}
			get{return _svrip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SvrPort
		{
			set{ _svrport=value;}
			get{return _svrport;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string appkey
		{
			set{ _appkey=value;}
			get{return _appkey;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string appSecret
		{
			set{ _appsecret=value;}
			get{return _appsecret;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string time
		{
			set{ _time=value;}
			get{return _time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string timeSecret
		{
			set{ _timesecret=value;}
			get{return _timesecret;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string httpsflag
		{
			set{ _httpsflag=value;}
			get{return _httpsflag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CamList
		{
			set{ _camlist=value;}
			get{return _camlist;}
		}
		/// <summary>
		/// ±¸×¢
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// Ìí¼ÓÊ±¼ä
		/// </summary>
		public string AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// ÊÇ·ñÉ¾³ý£¨0É¾£©
		/// </summary>
		public int? IsDelete
		{
			set{ _isdelete=value;}
			get{return _isdelete;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RegionId
		{
			set{ _regionid=value;}
			get{return _regionid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string jiankongName
		{
			set{ _jiankongname=value;}
			get{return _jiankongname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string longitude
		{
			set{ _longitude=value;}
			get{return _longitude;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string latitude
		{
			set{ _latitude=value;}
			get{return _latitude;}
		}
		/// <summary>
		/// ÊÓÆµµØÖ·
		/// </summary>
		public string VideoUrl
		{
			set{ _videourl=value;}
			get{return _videourl;}
		}
		#endregion Model

	}
}


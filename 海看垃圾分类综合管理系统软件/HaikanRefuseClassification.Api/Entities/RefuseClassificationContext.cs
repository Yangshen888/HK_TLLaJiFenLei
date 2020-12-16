using System;
using HaikanRefuseClassification.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class RefuseClassificationContext : DbContext
    {
        public RefuseClassificationContext()
        {
        }

        public RefuseClassificationContext(DbContextOptions<RefuseClassificationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<CarMonitoring> CarMonitoring { get; set; }
        public virtual DbSet<CarWeightView> CarWeightView { get; set; }
        public virtual DbSet<DateScore> DateScore { get; set; }
        public virtual DbSet<DayOfWeight> DayOfWeight { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<DeviceToCar> DeviceToCar { get; set; }
        public virtual DbSet<Driver> Driver { get; set; }
        public virtual DbSet<Goods> Goods { get; set; }
        public virtual DbSet<GoodsExchange> GoodsExchange { get; set; }
        public virtual DbSet<GrabType> GrabType { get; set; }
        public virtual DbSet<GrabageDisposal> GrabageDisposal { get; set; }
        public virtual DbSet<GrabageDisposalCopy1> GrabageDisposalCopy1 { get; set; }
        public virtual DbSet<GrabageMonitoring> GrabageMonitoring { get; set; }
        public virtual DbSet<GrabageRoom> GrabageRoom { get; set; }
        public virtual DbSet<GrabageWeightSon> GrabageWeightSon { get; set; }
        public virtual DbSet<GrabageWeighting> GrabageWeighting { get; set; }
        public virtual DbSet<HomeAddress> HomeAddress { get; set; }
        public virtual DbSet<HomeAddressView> HomeAddressView { get; set; }
        public virtual DbSet<HomeDateScore> HomeDateScore { get; set; }
        public virtual DbSet<Leaderboard> Leaderboard { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Overallsituation> Overallsituation { get; set; }
        public virtual DbSet<Ownsource> Ownsource { get; set; }
        public virtual DbSet<PerishRubbishView> PerishRubbishView { get; set; }
        public virtual DbSet<PerishRubbishViewTwo> PerishRubbishViewTwo { get; set; }
        public virtual DbSet<PerishableRubbish> PerishableRubbish { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionPerson> QuestionPerson { get; set; }
        public virtual DbSet<RecordTime> RecordTime { get; set; }
        public virtual DbSet<RecycleInfo> RecycleInfo { get; set; }
        public virtual DbSet<RegionInfo> RegionInfo { get; set; }
        public virtual DbSet<Response> Response { get; set; }
        public virtual DbSet<RubbishAll> RubbishAll { get; set; }
        public virtual DbSet<RubbishSyrecord> RubbishSyrecord { get; set; }
        public virtual DbSet<ScoreSetting> ScoreSetting { get; set; }
        public virtual DbSet<Shop> Shop { get; set; }
        public virtual DbSet<ShouScore> ShouScore { get; set; }
        public virtual DbSet<StreeStatistical> StreeStatistical { get; set; }
        public virtual DbSet<SubdistrictCommunity> SubdistrictCommunity { get; set; }
        public virtual DbSet<SupervisorInspection> SupervisorInspection { get; set; }
        public virtual DbSet<SupervisorVolunteer> SupervisorVolunteer { get; set; }
        public virtual DbSet<SupervisorsWork> SupervisorsWork { get; set; }
        public virtual DbSet<SystemIcon> SystemIcon { get; set; }
        public virtual DbSet<SystemLog> SystemLog { get; set; }
        public virtual DbSet<SystemMenu> SystemMenu { get; set; }
        public virtual DbSet<SystemPermission> SystemPermission { get; set; }
        public virtual DbSet<SystemRole> SystemRole { get; set; }
        public virtual DbSet<SystemRolePermissionMapping> SystemRolePermissionMapping { get; set; }
        public virtual DbSet<SystemSetting> SystemSetting { get; set; }
        public virtual DbSet<SystemUser> SystemUser { get; set; }
        public virtual DbSet<SystemUserRoleMapping> SystemUserRoleMapping { get; set; }
        public virtual DbSet<VehicleInfo> VehicleInfo { get; set; }
        public virtual DbSet<ViewMenu> ViewMenu { get; set; }
        public virtual DbSet<ViewSystemPermissionWithMenu> ViewSystemPermissionWithMenu { get; set; }
        public virtual DbSet<ViewSystemPermissionWithMenu2> ViewSystemPermissionWithMenu2 { get; set; }
        public virtual DbSet<Village> Village { get; set; }
        public virtual DbSet<VolunteerService> VolunteerService { get; set; }
        public virtual DbSet<VolunteerYy> VolunteerYy { get; set; }
        public virtual DbSet<WasteRatio> WasteRatio { get; set; }
        public virtual DbSet<WingNoRecord> WingNoRecord { get; set; }
        public virtual DbSet<WorkTime> WorkTime { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("Data Source=192.168.0.214;Initial Catalog=桐庐垃圾分类;Persist Security Info=True;User ID=桐庐垃圾分类;Password=haikan051030");
                var conncectstr = ConfigurationManager.ConnectionStrings.DefaultConnection;
                optionsBuilder.UseSqlServer(conncectstr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => e.AttendanceUuid)
                    .HasName("PK__Attendan__2D64B903000A2655");

                entity.HasComment("考勤记录表");

                entity.Property(e => e.AttendanceUuid)
                    .HasColumnName("AttendanceUUID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.AmendPlace)
                    .HasColumnName("AMEndPlace")
                    .HasMaxLength(255)
                    .HasComment("(上午)下班打卡地点");

                entity.Property(e => e.AmendTime)
                    .HasColumnName("AMEndTime")
                    .HasMaxLength(255)
                    .HasComment("(上午)下班打卡时间");

                entity.Property(e => e.AmstartPlace)
                    .HasColumnName("AMStartPlace")
                    .HasMaxLength(255)
                    .HasComment("(上午)上班打卡地点");

                entity.Property(e => e.AmstartTime)
                    .HasColumnName("AMStartTime")
                    .HasMaxLength(255)
                    .HasComment("(上午)上班打卡时间");

                entity.Property(e => e.ColckDate)
                    .HasMaxLength(255)
                    .HasComment("打卡日期");

                entity.Property(e => e.EndState).HasComment("下班状态（0.未打卡1.正常2.早退）");

                entity.Property(e => e.GarbageRoomUuid).HasColumnName("GarbageRoomUUID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("自增序列")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0未删除 1删除");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("打卡人名字");

                entity.Property(e => e.PmendPlace)
                    .HasColumnName("PMEndPlace")
                    .HasMaxLength(255)
                    .HasComment("(下午)下班打卡地点");

                entity.Property(e => e.PmendTime)
                    .HasColumnName("PMEndTime")
                    .HasMaxLength(255)
                    .HasComment("(下午)下班打卡时间");

                entity.Property(e => e.PmstartPlace)
                    .HasColumnName("PMStartPlace")
                    .HasMaxLength(255)
                    .HasComment("(下午)上班打卡地点");

                entity.Property(e => e.PmstartTime)
                    .HasColumnName("PMStartTime")
                    .HasMaxLength(255)
                    .HasComment("(下午)上班打卡时间");

                entity.Property(e => e.StartState).HasComment("上班状态（0.未打卡1.正常2.迟到）");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("打卡人UUID");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0 督导员 1志愿者");

                entity.Property(e => e.Volunteerevaluation)
                    .HasColumnName(" Volunteerevaluation ")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("志愿者评价");

                entity.HasOne(d => d.GarbageRoomUu)
                    .WithMany(p => p.Attendance)
                    .HasForeignKey(d => d.GarbageRoomUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("grab_attend");

                entity.HasOne(d => d.SystemUserUu)
                    .WithMany(p => p.Attendance)
                    .HasForeignKey(d => d.SystemUserUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Attendance_FK");
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(e => e.CarUuid)
                    .HasName("PK__Car__0F7A0BB6042758CD");

                entity.HasComment("车辆管理");

                entity.Property(e => e.CarUuid)
                    .HasColumnName("CarUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.AwardTime)
                    .HasMaxLength(255)
                    .HasComment("发证时间");

                entity.Property(e => e.BalanceId)
                    .HasColumnName("BalanceID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("称重设备id");

                entity.Property(e => e.Brand)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("品牌型号");

                entity.Property(e => e.CameraId)
                    .HasColumnName("CameraID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("摄像机编号");

                entity.Property(e => e.CarNum)
                    .HasMaxLength(255)
                    .HasComment("车牌号");

                entity.Property(e => e.CarType)
                    .HasMaxLength(255)
                    .HasComment("车辆类型");

                entity.Property(e => e.Company)
                    .HasColumnName("company")
                    .HasMaxLength(255)
                    .HasComment("所属公司");

                entity.Property(e => e.GrabType)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("垃圾类型");

                entity.Property(e => e.GrabageRoomId)
                    .HasColumnName("GrabageRoomID")
                    .HasComment("厢房UUID");

                entity.Property(e => e.HolderId)
                    .HasColumnName("HolderID")
                    .HasMaxLength(255)
                    .HasComment("持有人姓名");

                entity.Property(e => e.HolderPhone)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("持有人电话");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.OnlyNum)
                    .HasMaxLength(255)
                    .HasComment("车载唯一编码");

                entity.Property(e => e.Photo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("车辆图片");

                entity.Property(e => e.RegisterTime)
                    .HasMaxLength(255)
                    .HasComment("注册时间");

                entity.Property(e => e.Street)
                    .HasMaxLength(255)
                    .HasComment("所属街道");

                entity.Property(e => e.Weight)
                    .HasMaxLength(255)
                    .HasComment("车辆重量");
            });

            modelBuilder.Entity<CarMonitoring>(entity =>
            {
                entity.HasKey(e => e.CarMonitoringUuid);

                entity.HasComment("车辆监控");

                entity.Property(e => e.CarMonitoringUuid)
                    .HasColumnName("CarMonitoringUUID")
                    .HasComment("车辆监控UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.AppSecret)
                    .HasColumnName("appSecret")
                    .HasMaxLength(255);

                entity.Property(e => e.Appkey)
                    .HasColumnName("appkey")
                    .HasMaxLength(255);

                entity.Property(e => e.CamList).HasMaxLength(255);

                entity.Property(e => e.CarUuid)
                    .HasColumnName("CarUUID")
                    .HasComment("车辆UUID");

                entity.Property(e => e.Httpsflag)
                    .HasColumnName("httpsflag")
                    .HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除（0删）");

                entity.Property(e => e.MonitoringNum)
                    .HasMaxLength(255)
                    .HasComment("监控编号");

                entity.Property(e => e.PalyType).HasMaxLength(255);

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasMaxLength(255)
                    .HasComment("备注");

                entity.Property(e => e.SvrIp).HasMaxLength(255);

                entity.Property(e => e.SvrPort).HasMaxLength(255);

                entity.Property(e => e.Svrlp).HasMaxLength(255);

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasMaxLength(255);

                entity.Property(e => e.TimeSecret)
                    .HasColumnName("timeSecret")
                    .HasMaxLength(255);

                entity.HasOne(d => d.CarUu)
                    .WithMany(p => p.CarMonitoring)
                    .HasForeignKey(d => d.CarUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("CarMonitoring_FK");
            });

            modelBuilder.Entity<CarWeightView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CarWeightView");

                entity.Property(e => e.CarNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarUuid).HasColumnName("CarUUID");

                entity.Property(e => e.GrabType)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Street).HasMaxLength(255);

                entity.Property(e => e.Weight).HasColumnType("numeric(20, 2)");

                entity.Property(e => e.Wtime)
                    .HasColumnName("WTime")
                    .HasMaxLength(22)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DateScore>(entity =>
            {
                entity.HasKey(e => e.DateScoreUuid)
                    .HasName("IK_DateScore");

                entity.Property(e => e.DateScoreUuid)
                    .HasColumnName("DateScoreUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime).HasMaxLength(255);

                entity.Property(e => e.HomeAddressUuid).HasColumnName("HomeAddressUUID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.HasOne(d => d.HomeAddressUu)
                    .WithMany(p => p.DateScore)
                    .HasForeignKey(d => d.HomeAddressUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_DateScore_HomeAddress");
            });

            modelBuilder.Entity<DayOfWeight>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("DayOfWeight");

                entity.Property(e => e.GrabageRoomId).HasColumnName("GrabageRoomID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Proportion).HasColumnType("decimal(38, 6)");

                entity.Property(e => e.Qtlj)
                    .HasColumnName("qtlj")
                    .HasColumnType("decimal(38, 4)");

                entity.Property(e => e.Wtime)
                    .HasColumnName("WTime")
                    .HasMaxLength(22)
                    .IsUnicode(false);

                entity.Property(e => e.Yflj)
                    .HasColumnName("yflj")
                    .HasColumnType("decimal(38, 4)");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DepartmentUuid)
                    .HasName("PK__Departme__1AA1F4C0A1AE1F0B");

                entity.HasComment("科室表");

                entity.HasIndex(e => e.DepartmentUuid)
                    .HasName("DepartmentUUID");

                entity.HasIndex(e => e.Id)
                    .HasName("ID");

                entity.Property(e => e.DepartmentUuid)
                    .HasColumnName("DepartmentUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(255)
                    .HasComment("科室名");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("自增")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDeleted).HasComment("是否删除0.未删1.已删");
            });

            modelBuilder.Entity<DeviceToCar>(entity =>
            {
                entity.HasKey(e => e.DeviceToCarUuid)
                    .HasName("DeviceToCar_PK")
                    .IsClustered(false);

                entity.HasComment("平板汽车关系表");

                entity.Property(e => e.DeviceToCarUuid)
                    .HasColumnName("DeviceToCarUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPerson)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AddTime)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CarUuid)
                    .HasColumnName("CarUUID")
                    .HasComment("汽车UUID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Imei)
                    .HasColumnName("IMEI")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("车载平板IMEI");

                entity.HasOne(d => d.CarUu)
                    .WithMany(p => p.DeviceToCar)
                    .HasForeignKey(d => d.CarUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("DeviceToCar_FK");
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasKey(e => e.DriverUuid)
                    .HasName("PK__Driver__1C295CE0997E4850");

                entity.HasComment("驾驶员");

                entity.Property(e => e.DriverUuid)
                    .HasColumnName("DriverUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasComment("地址");

                entity.Property(e => e.DriverName)
                    .HasMaxLength(255)
                    .HasComment("驾驶员名称");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .HasComment("电话");
            });

            modelBuilder.Entity<Goods>(entity =>
            {
                entity.Property(e => e.GoodsId)
                    .HasColumnName("GoodsID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AddTime)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Gname)
                    .HasColumnName("GName")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("商品名");

                entity.Property(e => e.Price)
                    .HasMaxLength(225)
                    .IsUnicode(false)
                    .HasComment("价格");

                entity.Property(e => e.ShopId)
                    .HasColumnName("ShopID")
                    .HasComment("商店id");

                entity.Property(e => e.State)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))")
                    .HasComment("0未兑换 1以兑换");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("商店_商品");
            });

            modelBuilder.Entity<GoodsExchange>(entity =>
            {
                entity.HasKey(e => e.StoreExchangeUuid)
                    .HasName("PK__GoodsExc__E371A88EF01E0563");

                entity.HasComment("积分兑换");

                entity.Property(e => e.StoreExchangeUuid)
                    .HasColumnName("StoreExchangeUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DeduceScore)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("扣除积分");

                entity.Property(e => e.ExchageTime)
                    .HasMaxLength(50)
                    .HasComment("兑换时间");

                entity.Property(e => e.GoodsId)
                    .HasColumnName("GoodsID")
                    .HasComment("商品id");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0未删除 1删除");

                entity.Property(e => e.ShopId)
                    .HasColumnName("ShopID")
                    .HasComment("商店id");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("用户id");

                entity.HasOne(d => d.Goods)
                    .WithMany(p => p.GoodsExchange)
                    .HasForeignKey(d => d.GoodsId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("商品_兑换");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.GoodsExchange)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("商店_兑换");
            });

            modelBuilder.Entity<GrabType>(entity =>
            {
                entity.HasKey(e => e.GrabTypeUuid)
                    .HasName("PK__GrabType__3DFFCA10844D0CD0");

                entity.HasComment("垃圾类型表");

                entity.Property(e => e.GrabTypeUuid)
                    .HasColumnName("GrabTypeUUid")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeopel)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AddTime)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GrabName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("垃圾名");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0未删除 1已删除");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("垃圾所属类型");
            });

            modelBuilder.Entity<GrabageDisposal>(entity =>
            {
                entity.HasKey(e => e.GarbageDisposalUuid)
                    .HasName("PK__GrabageD__398F9B2CFA754341");

                entity.HasComment("垃圾投放");

                entity.Property(e => e.GarbageDisposalUuid)
                    .HasColumnName("GarbageDisposalUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(20)
                    .HasComment("投放时间");

                entity.Property(e => e.GrabType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("垃圾类型");

                entity.Property(e => e.GrabageRoomId)
                    .HasColumnName("GrabageRoomID")
                    .HasComment("垃圾厢房id");

                entity.Property(e => e.HomeAddressUuid).HasColumnName("HomeAddressUUID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0未删除 1删除");

                entity.Property(e => e.IsScore).HasMaxLength(20);

                entity.Property(e => e.MarkType).HasMaxLength(255);

                entity.Property(e => e.ScoreAddtime)
                    .HasMaxLength(255)
                    .HasComment("赋分时间");

                entity.Property(e => e.ScoreSettingUuid)
                    .HasColumnName("ScoreSettingUUid")
                    .HasComment("赋分类型");

                entity.Property(e => e.SupervisorUuid)
                    .HasColumnName("SupervisorUUID")
                    .HasComment("督导员id");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("投放人id");

                entity.HasOne(d => d.GrabageRoom)
                    .WithMany(p => p.GrabageDisposal)
                    .HasForeignKey(d => d.GrabageRoomId)
                    .HasConstraintName("垃圾房_投放");

                entity.HasOne(d => d.HomeAddressUu)
                    .WithMany(p => p.GrabageDisposal)
                    .HasForeignKey(d => d.HomeAddressUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("地址");

                entity.HasOne(d => d.ScoreSettingUu)
                    .WithMany(p => p.GrabageDisposal)
                    .HasForeignKey(d => d.ScoreSettingUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("赋分_投放");

                entity.HasOne(d => d.SupervisorUu)
                    .WithMany(p => p.GrabageDisposalSupervisorUu)
                    .HasForeignKey(d => d.SupervisorUuid)
                    .HasConstraintName("督导员");

                entity.HasOne(d => d.SystemUserUu)
                    .WithMany(p => p.GrabageDisposalSystemUserUu)
                    .HasForeignKey(d => d.SystemUserUuid)
                    .HasConstraintName("业主");
            });

            modelBuilder.Entity<GrabageDisposalCopy1>(entity =>
            {
                entity.HasKey(e => e.GarbageDisposalUuid)
                    .HasName("PK__GrabageD__398F9B2CFA754341_copy1");

                entity.ToTable("GrabageDisposal_copy1");

                entity.HasComment("垃圾投放");

                entity.Property(e => e.GarbageDisposalUuid)
                    .HasColumnName("GarbageDisposalUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(20)
                    .HasComment("投放时间");

                entity.Property(e => e.GrabType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("垃圾类型");

                entity.Property(e => e.GrabageRoomId)
                    .HasColumnName("GrabageRoomID")
                    .HasComment("垃圾厢房id");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0未删除 1删除");

                entity.Property(e => e.ScoreAddtime)
                    .HasMaxLength(255)
                    .HasComment("赋分时间");

                entity.Property(e => e.ScoreSettingUuid)
                    .HasColumnName("ScoreSettingUUid")
                    .HasComment("赋分类型");

                entity.Property(e => e.SupervisorUuid)
                    .HasColumnName("SupervisorUUID")
                    .HasComment("督导员id");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("投放人id");

                entity.HasOne(d => d.GrabageRoom)
                    .WithMany(p => p.GrabageDisposalCopy1)
                    .HasForeignKey(d => d.GrabageRoomId)
                    .HasConstraintName("FK__GrabageDi__Graba__7D439ABD");

                entity.HasOne(d => d.ScoreSettingUu)
                    .WithMany(p => p.GrabageDisposalCopy1)
                    .HasForeignKey(d => d.ScoreSettingUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__GrabageDi__Score__7B5B524B");

                entity.HasOne(d => d.SupervisorUu)
                    .WithMany(p => p.GrabageDisposalCopy1SupervisorUu)
                    .HasForeignKey(d => d.SupervisorUuid)
                    .HasConstraintName("FK__GrabageDi__Super__7C4F7684");

                entity.HasOne(d => d.SystemUserUu)
                    .WithMany(p => p.GrabageDisposalCopy1SystemUserUu)
                    .HasForeignKey(d => d.SystemUserUuid)
                    .HasConstraintName("FK__GrabageDi__Syste__7E37BEF6");
            });

            modelBuilder.Entity<GrabageMonitoring>(entity =>
            {
                entity.HasKey(e => e.GrabageMonitoringUuid);

                entity.HasComment("垃圾厢房监控");

                entity.Property(e => e.GrabageMonitoringUuid)
                    .HasColumnName("GrabageMonitoringUUID")
                    .HasComment("厢房监控UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.AppSecret)
                    .HasColumnName("appSecret")
                    .HasMaxLength(255);

                entity.Property(e => e.Appkey)
                    .HasColumnName("appkey")
                    .HasMaxLength(255);

                entity.Property(e => e.CamList).HasMaxLength(255);

                entity.Property(e => e.GarbageRoomUuid)
                    .HasColumnName("GarbageRoomUUID")
                    .HasComment("垃圾厢房UUID");

                entity.Property(e => e.Httpsflag)
                    .HasColumnName("httpsflag")
                    .HasMaxLength(255)
                    .HasComment("街道社区名");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除（0删）");

                entity.Property(e => e.JiankongName)
                    .HasColumnName("jiankongName")
                    .HasMaxLength(255);

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasMaxLength(255);

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasMaxLength(255);

                entity.Property(e => e.MonitoringNum)
                    .HasMaxLength(255)
                    .HasComment("监控编号");

                entity.Property(e => e.PalyType).HasMaxLength(255);

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasMaxLength(255)
                    .HasComment("备注");

                entity.Property(e => e.SvrIp).HasMaxLength(255);

                entity.Property(e => e.SvrPort).HasMaxLength(255);

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasMaxLength(255);

                entity.Property(e => e.TimeSecret)
                    .HasColumnName("timeSecret")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<GrabageRoom>(entity =>
            {
                entity.HasKey(e => e.GarbageRoomUuid)
                    .HasName("PK__1__25244D74F7DF3761");

                entity.HasComment("垃圾厢房");

                entity.Property(e => e.GarbageRoomUuid)
                    .HasColumnName("GarbageRoomUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加时间");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("街道");

                entity.Property(e => e.CarId)
                    .HasColumnName("CarID")
                    .HasComment("收运车辆id");

                entity.Property(e => e.EndTime)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("开放时间结束");

                entity.Property(e => e.Ewm)
                    .HasColumnName("EWM")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("二维码");

                entity.Property(e => e.Facilityuuid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("设备id");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0未删除 1删除");

                entity.Property(e => e.Lat)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("纬度");

                entity.Property(e => e.Ljname)
                    .HasColumnName("LJName")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("名称");

                entity.Property(e => e.Lon)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("经度");

                entity.Property(e => e.MonitoringNum)
                    .HasMaxLength(255)
                    .HasComment("监控ID");

                entity.Property(e => e.Posititon)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("垃圾厢房位置");

                entity.Property(e => e.RottenRubbishPercent)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("易腐烂垃圾比例");

                entity.Property(e => e.StartTime)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("开放时间开始");

                entity.Property(e => e.State)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("状态（0使用中，1暂停使用）");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("督导员id");

                entity.Property(e => e.Towns)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("乡镇");

                entity.Property(e => e.VillageId)
                    .HasColumnName("VillageID")
                    .HasComment("小区id");

                entity.Property(e => e.WingId)
                    .HasColumnName("WingID")
                    .HasMaxLength(255)
                    .HasComment("厢房编号");

                entity.HasOne(d => d.Village)
                    .WithMany(p => p.GrabageRoom)
                    .HasForeignKey(d => d.VillageId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("垃圾箱-社区");
            });

            modelBuilder.Entity<GrabageWeightSon>(entity =>
            {
                entity.HasKey(e => e.GrabageWeighingRecordUuid)
                    .HasName("PK__GrabageW__8DFC4F08E33A87E5");

                entity.HasComment("垃圾称重筛选");

                entity.Property(e => e.GrabageWeighingRecordUuid)
                    .HasColumnName("GrabageWeighingRecordUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarUuid).HasColumnName("CarUUID");

                entity.Property(e => e.CheckedUser)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GrabageRoomId).HasColumnName("GrabageRoomID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RecordType)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0其他垃圾  1易腐垃圾");

                entity.Property(e => e.Weight)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.WeightTime)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.WeightUuid).HasColumnName("WeightUUID");

                entity.HasOne(d => d.CarUu)
                    .WithMany(p => p.GrabageWeightSon)
                    .HasForeignKey(d => d.CarUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("GrabageWeightSon_FK");

                entity.HasOne(d => d.GrabageRoom)
                    .WithMany(p => p.GrabageWeightSon)
                    .HasForeignKey(d => d.GrabageRoomId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("垃圾房_垃圾称重筛选");
            });

            modelBuilder.Entity<GrabageWeighting>(entity =>
            {
                entity.HasKey(e => e.GrabageWeighingRecordUuid)
                    .HasName("PK__GrabageW__8DFC4F08E33A99E7");

                entity.HasComment("垃圾称重");

                entity.Property(e => e.GrabageWeighingRecordUuid)
                    .HasColumnName("GrabageWeighingRecordUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("车牌");

                entity.Property(e => e.CarUuid).HasColumnName("CarUUID");

                entity.Property(e => e.CheckedUser)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GrabageRoomId).HasColumnName("GrabageRoomID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0未删除 1删除");

                entity.Property(e => e.RecordType)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("称重类型(车辆称重，地磅称重)");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("垃圾类型");

                entity.Property(e => e.Weight)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("重量");

                entity.Property(e => e.WeightTime)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.WeightUuid).HasColumnName("WeightUUID");

                entity.HasOne(d => d.CarUu)
                    .WithMany(p => p.GrabageWeighting)
                    .HasForeignKey(d => d.CarUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("GrabageWeighting_FK");

                entity.HasOne(d => d.GrabageRoom)
                    .WithMany(p => p.GrabageWeighting)
                    .HasForeignKey(d => d.GrabageRoomId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("垃圾房_垃圾称重");
            });

            modelBuilder.Entity<HomeAddress>(entity =>
            {
                entity.HasKey(e => e.HomeAddressUuid)
                    .IsClustered(false);

                entity.HasComment("家庭地址");

                entity.Property(e => e.HomeAddressUuid)
                    .HasColumnName("HomeAddressUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Addresscode)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Building)
                    .HasColumnName("building")
                    .HasMaxLength(255);

                entity.Property(e => e.BuildingNum)
                    .HasColumnName("building_num")
                    .HasMaxLength(255);

                entity.Property(e => e.Ccmmunity)
                    .HasColumnName("ccmmunity")
                    .HasMaxLength(255);

                entity.Property(e => e.Door)
                    .HasColumnName("door")
                    .HasMaxLength(255);

                entity.Property(e => e.Floor)
                    .HasColumnName("floor")
                    .HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Resregion)
                    .HasColumnName("resregion")
                    .HasMaxLength(255);

                entity.Property(e => e.Room)
                    .HasColumnName("room")
                    .HasMaxLength(255);

                entity.Property(e => e.RoomFloor).HasColumnName("room_floor");

                entity.Property(e => e.Score).HasDefaultValueSql("((0))");

                entity.Property(e => e.Squad)
                    .HasColumnName("squad")
                    .HasMaxLength(255);

                entity.Property(e => e.Street)
                    .HasColumnName("street")
                    .HasMaxLength(255);

                entity.Property(e => e.Szone)
                    .HasColumnName("szone")
                    .HasMaxLength(255);

                entity.Property(e => e.Town)
                    .HasColumnName("town")
                    .HasMaxLength(255);

                entity.Property(e => e.Unit)
                    .HasColumnName("unit")
                    .HasMaxLength(255);

                entity.Property(e => e.Village)
                    .HasColumnName("village")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<HomeAddressView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("HomeAddress_view");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Ccmmunity)
                    .HasColumnName("ccmmunity")
                    .HasMaxLength(255);

                entity.Property(e => e.HomeAddressUuid).HasColumnName("HomeAddressUUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LoginName).HasMaxLength(255);

                entity.Property(e => e.Per).HasColumnName("per");

                entity.Property(e => e.SystemUserUuid).HasColumnName("SystemUserUUID");

                entity.Property(e => e.Town)
                    .HasColumnName("town")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<HomeDateScore>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("HomeDateScore");

                entity.Property(e => e.HomeAddressUuid).HasColumnName("HomeAddressUUID");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Leaderboard>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Leaderboard");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Ccmmunity)
                    .HasColumnName("ccmmunity")
                    .HasMaxLength(255);

                entity.Property(e => e.HomeAddressUuid).HasColumnName("HomeAddressUUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LoginName).HasMaxLength(255);

                entity.Property(e => e.SystemUserUuid).HasColumnName("SystemUserUUID");

                entity.Property(e => e.Town)
                    .HasColumnName("town")
                    .HasMaxLength(255);

                entity.Property(e => e.Zyzsc).HasColumnName("zyzsc");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessageUuid)
                    .HasName("Message_PK");

                entity.HasComment("站内信");

                entity.HasIndex(e => e.Id)
                    .HasName("Message_UN")
                    .IsUnique();

                entity.Property(e => e.MessageUuid)
                    .HasColumnName("MessageUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("添加时间");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Message1)
                    .HasColumnName("Message")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("消息内容");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("督导员uuid");

                entity.HasOne(d => d.SystemUserUu)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.SystemUserUuid)
                    .HasConstraintName("站内信_用户");
            });

            modelBuilder.Entity<Overallsituation>(entity =>
            {
                entity.HasKey(e => e.SetUuid)
                    .HasName("PK__ Overall__96BE7712E4CF3558");

                entity.Property(e => e.SetUuid)
                    .HasColumnName("SetUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.HourScore).HasComment("每小时对应的积分");

                entity.Property(e => e.SetName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("设置参数名字");

                entity.Property(e => e.SetNumber).HasComment("每天可赋分次数");
            });

            modelBuilder.Entity<Ownsource>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("OWNSource");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Alls).HasColumnName("alls");

                entity.Property(e => e.Can).HasColumnName("can");

                entity.Property(e => e.Ccmmunity).HasMaxLength(255);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Resregion).HasMaxLength(255);

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.Town).HasMaxLength(255);

                entity.Property(e => e.Used).HasColumnName("used");
            });

            modelBuilder.Entity<PerishRubbishView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PerishRubbishView");

                entity.Property(e => e.GrabageRoomId).HasColumnName("GrabageRoomID");

                entity.Property(e => e.GrabageWeighingRecordUuid).HasColumnName("GrabageWeighingRecordUUID");

                entity.Property(e => e.Ljname)
                    .HasColumnName("LJName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Towns)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Vname)
                    .HasColumnName("VName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Weight)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PerishRubbishViewTwo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PerishRubbishViewTwo");

                entity.Property(e => e.Addtimes)
                    .HasColumnName("addtimes")
                    .HasMaxLength(22)
                    .IsUnicode(false);

                entity.Property(e => e.Dataratio).HasColumnName("dataratio");

                entity.Property(e => e.DayData).HasColumnName("dayData");

                entity.Property(e => e.GrabageRoomId).HasColumnName("GrabageRoomID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ljname)
                    .HasColumnName("LJName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Towns)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Vname)
                    .HasColumnName("VName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Weekdata).HasColumnName("weekdata");

                entity.Property(e => e.Weekratio).HasColumnName("weekratio");

                entity.Property(e => e.Yeardata).HasColumnName("yeardata");

                entity.Property(e => e.Yearratio).HasColumnName("yearratio");
            });

            modelBuilder.Entity<PerishableRubbish>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PerishableRubbish");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.VillageUuid).HasColumnName("VillageUUID");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.QuestionUuid)
                    .HasName("PK__Question__B1C6F55E78DEAC8C");

                entity.HasComment("问题反馈");

                entity.Property(e => e.QuestionUuid)
                    .HasColumnName("QuestionUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarUuid).HasColumnName("CarUUID");

                entity.Property(e => e.Estimate)
                    .HasColumnName("estimate")
                    .HasMaxLength(255);

                entity.Property(e => e.Estpeople)
                    .HasColumnName("estpeople")
                    .HasMaxLength(100);

                entity.Property(e => e.Esttime)
                    .HasColumnName("esttime")
                    .HasMaxLength(100);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0未删除 1删除");

                entity.Property(e => e.QueRoomId)
                    .HasColumnName("QueRoomID")
                    .HasComment("问题厢房");

                entity.Property(e => e.QueType)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("问题类型");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("备注");

                entity.Property(e => e.VillageUuid).HasColumnName("VillageUUID");

                entity.HasOne(d => d.CarUu)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.CarUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Question_FK");

                entity.HasOne(d => d.QueRoom)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.QueRoomId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("垃圾房_问题");

                entity.HasOne(d => d.VillageUu)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.VillageUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("QuestionV_FK");
            });

            modelBuilder.Entity<QuestionPerson>(entity =>
            {
                entity.HasKey(e => e.QuestionPersonUuid)
                    .HasName("PK__Question__C4C4CC4F2A2CDB6B");

                entity.Property(e => e.QuestionPersonUuid)
                    .HasColumnName("QuestionPersonUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estimate)
                    .HasColumnName("estimate")
                    .HasMaxLength(255);

                entity.Property(e => e.Estpeople)
                    .HasColumnName("estpeople")
                    .HasMaxLength(100);

                entity.Property(e => e.Esttime)
                    .HasColumnName("esttime")
                    .HasMaxLength(100);

                entity.Property(e => e.HomeUserUuid).HasColumnName("HomeUserUUID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProblemContent)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProblemType)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.HomeUserUu)
                    .WithMany(p => p.QuestionPerson)
                    .HasForeignKey(d => d.HomeUserUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("问题反馈_用户");
            });

            modelBuilder.Entity<RecordTime>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecordTime");

                entity.Property(e => e.GarbageRoomUuid).HasColumnName("GarbageRoomUUID");

                entity.Property(e => e.Times)
                    .HasColumnName("times")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RecycleInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RecycleInfo");

                entity.Property(e => e.GarbageRoomUuid).HasColumnName("GarbageRoomUUID");

                entity.Property(e => e.GrabageRoomId).HasColumnName("GrabageRoomID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Lat)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Ljname)
                    .HasColumnName("LJName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Lon)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Towns)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Vname)
                    .HasColumnName("VName")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RegionInfo>(entity =>
            {
                entity.HasKey(e => e.RegionId);

                entity.ToTable("Region_info");

                entity.Property(e => e.RegionId).ValueGeneratedNever();

                entity.Property(e => e.DanweiName).HasColumnName("danweiName");

                entity.Property(e => e.IndexCode).HasColumnName("index_code");
            });

            modelBuilder.Entity<Response>(entity =>
            {
                entity.HasKey(e => e.ProblemId)
                    .HasName("PK__Response__5CED516A22153E65");

                entity.Property(e => e.ProblemId)
                    .HasColumnName("ProblemID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SystemUserUuid).HasColumnName("SystemUserUUID");
            });

            modelBuilder.Entity<RubbishAll>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RubbishAll");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.VillageUuid).HasColumnName("VillageUUID");
            });

            modelBuilder.Entity<RubbishSyrecord>(entity =>
            {
                entity.HasKey(e => e.GarbageSyuuid)
                    .HasName("PK__RubbishS__6C4E18BE1F560AB4");

                entity.ToTable("RubbishSYRecord");

                entity.HasComment("垃圾收运记录表");

                entity.Property(e => e.GarbageSyuuid)
                    .HasColumnName("GarbageSYUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CarUuid)
                    .HasColumnName("CarUUID")
                    .HasComment("车辆");

                entity.Property(e => e.CorruptRubbishPercent)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("易腐烂垃圾比例");

                entity.Property(e => e.GrabageRoomUuid)
                    .HasColumnName("GrabageRoomUUID")
                    .HasComment("垃圾箱房id");

                entity.Property(e => e.GrabageWeighingUuid)
                    .HasColumnName("GrabageWeighingUUID")
                    .HasComment("垃圾称重");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("是否删除（0未删除1已删除）");

                entity.Property(e => e.RubbishType)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("垃圾类型");

                entity.Property(e => e.State)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("当前状态0使用中1暂停使用");

                entity.Property(e => e.Sytime)
                    .HasColumnName("SYTime")
                    .HasMaxLength(255)
                    .HasComment("收运时间");

                entity.Property(e => e.VillageUuid)
                    .HasColumnName("VillageUUID")
                    .HasComment("社区id");

                entity.HasOne(d => d.GrabageRoomUu)
                    .WithMany(p => p.RubbishSyrecord)
                    .HasForeignKey(d => d.GrabageRoomUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("厢房——记录");

                entity.HasOne(d => d.GrabageWeighingUu)
                    .WithMany(p => p.RubbishSyrecord)
                    .HasForeignKey(d => d.GrabageWeighingUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("重量_记录");

                entity.HasOne(d => d.VillageUu)
                    .WithMany(p => p.RubbishSyrecord)
                    .HasForeignKey(d => d.VillageUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("社区——记录");
            });

            modelBuilder.Entity<ScoreSetting>(entity =>
            {
                entity.HasKey(e => e.ScoreUuid)
                    .HasName("PK__ScoreSet__EC730B10D52C1C16");

                entity.HasComment("赋分设置表");

                entity.Property(e => e.ScoreUuid)
                    .HasColumnName("ScoreUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加时间");

                entity.Property(e => e.Addpeople)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加人");

                entity.Property(e => e.Integral).HasComment("对应积分");

                entity.Property(e => e.ScoreName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("评价名称");
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(e => e.ShopUuid)
                    .HasName("PK__Shop__E28968903F52213A");

                entity.HasComment("商店表");

                entity.Property(e => e.ShopUuid)
                    .HasColumnName("ShopUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("地址");

                entity.Property(e => e.Addtime)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0未删除 1删除");

                entity.Property(e => e.Lat)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("维度");

                entity.Property(e => e.Lon)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("经度");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("电话");

                entity.Property(e => e.ShopName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("商店名称");

                entity.Property(e => e.Shopowner)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("商店负责人");

                entity.Property(e => e.State)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("1营业中 2休息");
            });

            modelBuilder.Entity<ShouScore>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ShouScore");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Addtime)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Lat)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Lon)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ShopName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ShopUuid).HasColumnName("ShopUUID");

                entity.Property(e => e.Shopowner)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StreeStatistical>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("StreeStatistical");

                entity.Property(e => e.Dayprop).HasColumnName("dayprop");

                entity.Property(e => e.Daysum).HasColumnName("daysum");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Times)
                    .HasColumnName("times")
                    .HasMaxLength(22)
                    .IsUnicode(false);

                entity.Property(e => e.Towns)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Vname)
                    .HasColumnName("VName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Weekend)
                    .HasColumnName("weekend")
                    .HasColumnType("datetime");

                entity.Property(e => e.Weekprop).HasColumnName("weekprop");

                entity.Property(e => e.Weekstr)
                    .HasColumnName("weekstr")
                    .HasColumnType("datetime");

                entity.Property(e => e.Weeksum).HasColumnName("weeksum");

                entity.Property(e => e.Weekyf).HasColumnName("weekyf");

                entity.Property(e => e.Yearprop).HasColumnName("yearprop");

                entity.Property(e => e.Yearsum).HasColumnName("yearsum");
            });

            modelBuilder.Entity<SubdistrictCommunity>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("SubdistrictCommunity");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Resregion)
                    .HasColumnName("resregion")
                    .HasMaxLength(255);

                entity.Property(e => e.Towns)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Vname)
                    .HasColumnName("VName")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SupervisorInspection>(entity =>
            {
                entity.HasKey(e => e.AuditUuid)
                    .HasName("PK__Supervis__4BD5DF201D794171");

                entity.Property(e => e.AuditUuid).ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GarbageSoring).IsUnicode(false);

                entity.Property(e => e.Grade)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddressUuid).HasColumnName("HomeAddressUUID");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Picture).IsUnicode(false);

                entity.HasOne(d => d.HomeAddressUu)
                    .WithMany(p => p.SupervisorInspection)
                    .HasForeignKey(d => d.HomeAddressUuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Superviso__HomeA__01D345B0");
            });

            modelBuilder.Entity<SupervisorVolunteer>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Supervisor_Volunteer");

                entity.Property(e => e.Ap)
                    .HasColumnName("AP")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AttendanceUuid).HasColumnName("AttendanceUUID");

                entity.Property(e => e.ColckDate).HasMaxLength(255);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RealName).HasMaxLength(255);

                entity.Property(e => e.SystemUserUuid).HasColumnName("SystemUserUUID");
            });

            modelBuilder.Entity<SupervisorsWork>(entity =>
            {
                entity.HasKey(e => e.SupervisorWordUuid)
                    .HasName("PK__Supervis__E3849ED0D98776D5");

                entity.HasComment("督导员工作记录");

                entity.Property(e => e.SupervisorWordUuid)
                    .HasColumnName("SupervisorWordUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.GrabageType)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("垃圾类型");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0未删除 1删除");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OwnerID")
                    .HasComment("用户id");

                entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.SupervisorsWorkOwner)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("用户");

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.SupervisorsWorkSupervisor)
                    .HasForeignKey(d => d.SupervisorId)
                    .HasConstraintName("督导");
            });

            modelBuilder.Entity<SystemIcon>(entity =>
            {
                entity.HasKey(e => e.SystemIconUuid)
                    .HasName("PK__SystemIc__540CED9D42DF8109");

                entity.HasIndex(e => e.Id)
                    .HasName("ID");

                entity.HasIndex(e => e.SystemIconUuid)
                    .HasName("SystemIconUUID");

                entity.Property(e => e.SystemIconUuid)
                    .HasColumnName("SystemIconUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Custom).HasMaxLength(60);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Size).HasMaxLength(20);
            });

            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.HasKey(e => e.SystemLogUuid)
                    .HasName("PK__SystemLo__65A475E79A921FFD");

                entity.HasComment("系统日志表");

                entity.HasIndex(e => e.Id)
                    .HasName("ID");

                entity.HasIndex(e => e.SystemLogUuid)
                    .HasName("SystemLogUUID");

                entity.Property(e => e.SystemLogUuid)
                    .HasColumnName("SystemLogUUID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime")
                    .HasComment("添加时间");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("IPAddress")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("IP地址");

                entity.Property(e => e.IsDelete).HasComment("标记删除1，未删除2，已删除");

                entity.Property(e => e.OperationContent)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasComment("操作内容");

                entity.Property(e => e.OperationTime)
                    .HasColumnType("datetime")
                    .HasComment("操作时间");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("操作类型(增删改查)");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("操作用户ID");

                entity.Property(e => e.UserIdtype)
                    .HasColumnName("UserIDType")
                    .HasComment("用户名和类型");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("操作用户用户名");
            });

            modelBuilder.Entity<SystemMenu>(entity =>
            {
                entity.HasKey(e => e.SystemMenuUuid)
                    .HasName("PK__DncMenu__A2B5777CA1511602");

                entity.Property(e => e.SystemMenuUuid)
                    .HasColumnName("SystemMenuUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.BeforeCloseFun).HasMaxLength(255);

                entity.Property(e => e.Component).HasMaxLength(255);

                entity.Property(e => e.CreatedByUserName).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.ModifiedByUserName).HasMaxLength(255);

                entity.Property(e => e.ModifiedOn).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ParentName).HasMaxLength(255);

                entity.Property(e => e.Url).HasMaxLength(255);
            });

            modelBuilder.Entity<SystemPermission>(entity =>
            {
                entity.HasKey(e => e.SystemPermissionUuid)
                    .HasName("PK__DncPermi__18DD8CCDCC3FD718");

                entity.Property(e => e.SystemPermissionUuid)
                    .HasColumnName("SystemPermissionUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActionCode).HasMaxLength(255);

                entity.Property(e => e.CaPower).HasMaxLength(255);

                entity.Property(e => e.CreatedByUserName).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.ModifiedByUserName).HasMaxLength(255);

                entity.Property(e => e.ModifiedOn).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SystemMenuUuid).HasColumnName("SystemMenuUUID");

                entity.HasOne(d => d.SystemMenuUu)
                    .WithMany(p => p.SystemPermission)
                    .HasForeignKey(d => d.SystemMenuUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_SystemPermission_SystemMenu");
            });

            modelBuilder.Entity<SystemRole>(entity =>
            {
                entity.HasKey(e => e.SystemRoleUuid)
                    .HasName("PK__DncRole__DF75FB283AD1E2C6");

                entity.Property(e => e.SystemRoleUuid)
                    .HasColumnName("SystemRoleUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople).HasMaxLength(255);

                entity.Property(e => e.AddTime).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsFixation).HasDefaultValueSql("((0))");

                entity.Property(e => e.RoleName).HasMaxLength(255);
            });

            modelBuilder.Entity<SystemRolePermissionMapping>(entity =>
            {
                entity.HasKey(e => new { e.SystemRoleUuid, e.SystemPermissionUuid })
                    .HasName("PK__DncRoleP__1EF823E41817FDB5");

                entity.Property(e => e.SystemRoleUuid).HasColumnName("SystemRoleUUID");

                entity.Property(e => e.SystemPermissionUuid).HasColumnName("SystemPermissionUUID");

                entity.Property(e => e.AddPeople).HasMaxLength(255);

                entity.Property(e => e.AddTime).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.HasKey(e => e.ClobalUuid)
                    .HasName("PK__SystemSe__BD195C823DA4BBE6");

                entity.HasComment(@"全局设置
");

                entity.Property(e => e.ClobalUuid).ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime).HasComment("添加时间");

                entity.Property(e => e.ClobalName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("全局名称");

                entity.Property(e => e.ClobalSuo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("全局缩写");

                entity.Property(e => e.GlobalLogo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasComment("是否删除 0 正常 1删除");
            });

            modelBuilder.Entity<SystemUser>(entity =>
            {
                entity.HasKey(e => e.SystemUserUuid)
                    .HasName("PK__DncUser__A2B5777C0780DFF0")
                    .IsClustered(false);

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople).HasMaxLength(255);

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("注册时间");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("地址");

                entity.Property(e => e.DepartmentUuid).HasColumnName("DepartmentUUID");

                entity.Property(e => e.Ewm)
                    .HasColumnName("EWM")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GrabageRoomId).HasColumnName("GrabageRoomID");

                entity.Property(e => e.HomeAddressUuid).HasColumnName("HomeAddressUUID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdcardMd5)
                    .HasColumnName("IDCardMD5")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.InTime)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("入职时间");

                entity.Property(e => e.LoginName).HasMaxLength(255);

                entity.Property(e => e.ManageDepartment).HasColumnType("text");

                entity.Property(e => e.OldCard)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PassWord).HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProblemContent)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("反馈内容");

                entity.Property(e => e.ProblemType).HasMaxLength(255);

                entity.Property(e => e.RealName).HasMaxLength(255);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Sex)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ShopUuid).HasColumnName("ShopUUID");

                entity.Property(e => e.SystemRoleUuid)
                    .HasColumnName("SystemRoleUUid")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("角色id,用逗号分隔");

                entity.Property(e => e.Types)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("类型");

                entity.Property(e => e.UserIdCard).HasMaxLength(255);

                entity.Property(e => e.VillageId).HasColumnName("VillageID");

                entity.Property(e => e.Wechat)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ZaiGang)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.GrabageRoom)
                    .WithMany(p => p.SystemUser)
                    .HasForeignKey(d => d.GrabageRoomId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("垃圾箱房");

                entity.HasOne(d => d.HomeAddressUu)
                    .WithMany(p => p.SystemUser)
                    .HasForeignKey(d => d.HomeAddressUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("家庭码");

                entity.HasOne(d => d.ShopUu)
                    .WithMany(p => p.SystemUser)
                    .HasForeignKey(d => d.ShopUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("商店");

                entity.HasOne(d => d.Village)
                    .WithMany(p => p.SystemUser)
                    .HasForeignKey(d => d.VillageId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("社区");
            });

            modelBuilder.Entity<SystemUserRoleMapping>(entity =>
            {
                entity.HasKey(e => new { e.SystemUserUuid, e.SystemRoleUuid })
                    .HasName("PK__DncUserR__328A96E56EE320C2");

                entity.Property(e => e.SystemUserUuid).HasColumnName("SystemUserUUID");

                entity.Property(e => e.SystemRoleUuid).HasColumnName("SystemRoleUUID");

                entity.Property(e => e.AddPeople).HasMaxLength(255);

                entity.Property(e => e.AddTime)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<VehicleInfo>(entity =>
            {
                entity.HasKey(e => e.VehicleInfo1);

                entity.HasComment("车载信息位置");

                entity.Property(e => e.VehicleInfo1)
                    .HasColumnName("VehicleInfo")
                    .HasComment("UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Addtime)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("记录时间");

                entity.Property(e => e.CarUuid)
                    .HasColumnName("CarUUID")
                    .HasComment("车辆UUID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Lat)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("纬度");

                entity.Property(e => e.Lon)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("经度");
            });

            modelBuilder.Entity<ViewMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_Menu");

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.BeforeCloseFun).HasMaxLength(255);

                entity.Property(e => e.Component).HasMaxLength(255);

                entity.Property(e => e.CreatedByUserName).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.ModifiedByUserName).HasMaxLength(255);

                entity.Property(e => e.ModifiedOn).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ParentName).HasMaxLength(255);

                entity.Property(e => e.Pd).HasColumnName("pd");

                entity.Property(e => e.Ps).HasColumnName("ps");

                entity.Property(e => e.Pt).HasColumnName("pt");

                entity.Property(e => e.SystemMenuUuid).HasColumnName("SystemMenuUUID");

                entity.Property(e => e.SystemRoleUuid).HasColumnName("SystemRoleUUID");

                entity.Property(e => e.Url).HasMaxLength(255);
            });

            modelBuilder.Entity<ViewSystemPermissionWithMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_SystemPermissionWithMenu");

                entity.Property(e => e.MenuAlias).HasMaxLength(255);

                entity.Property(e => e.MenuName).HasMaxLength(255);

                entity.Property(e => e.Pd).HasColumnName("pd");

                entity.Property(e => e.PermissionActionCode).HasMaxLength(255);

                entity.Property(e => e.PermissionName).HasMaxLength(255);

                entity.Property(e => e.Ps).HasColumnName("ps");

                entity.Property(e => e.SystemRoleUuid).HasColumnName("SystemRoleUUID");
            });

            modelBuilder.Entity<ViewSystemPermissionWithMenu2>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_SystemPermissionWithMenu2");

                entity.Property(e => e.MenuAlias).HasMaxLength(255);

                entity.Property(e => e.MenuName).HasMaxLength(255);

                entity.Property(e => e.Pd).HasColumnName("pd");

                entity.Property(e => e.PermissionActionCode).HasMaxLength(255);

                entity.Property(e => e.PermissionName).HasMaxLength(255);

                entity.Property(e => e.Ps).HasColumnName("ps");
            });

            modelBuilder.Entity<Village>(entity =>
            {
                entity.HasKey(e => e.VillageUuid)
                    .HasName("PK__Village__95B60B5FB50B5F5D");

                entity.HasComment("小区");

                entity.Property(e => e.VillageUuid)
                    .HasColumnName("VillageUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("添加时间");

                entity.Property(e => e.Addpeople)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加人");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("街道（不要）");

                entity.Property(e => e.Exchange).HasDefaultValueSql("((1))");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("1删除0未删除");

                entity.Property(e => e.Towns)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("乡镇(街道)");

                entity.Property(e => e.Vname)
                    .HasColumnName("VName")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("社区名");
            });

            modelBuilder.Entity<VolunteerService>(entity =>
            {
                entity.HasKey(e => e.VolunteerServiceUuid)
                    .HasName("PK__Voluntee__0C6F7A105DC3B72D");

                entity.HasComment("志愿者服务");

                entity.Property(e => e.VolunteerServiceUuid)
                    .HasColumnName("VolunteerServiceUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GrabageRoomId)
                    .HasColumnName("GrabageRoomID")
                    .HasComment("垃圾房id");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Intime)
                    .HasColumnName("INTime")
                    .HasMaxLength(50)
                    .HasComment("签到时间");

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("0未删除 1删除");

                entity.Property(e => e.Outtime)
                    .HasColumnName("OUTTime")
                    .HasMaxLength(50)
                    .HasComment("签退时间");

                entity.Property(e => e.Score)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("分数");

                entity.Property(e => e.ScoreAddtime)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("赋分时间");

                entity.Property(e => e.SupervisorUuid)
                    .HasColumnName("SupervisorUUid")
                    .HasComment("督导员uuid");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("志愿者id");

                entity.Property(e => e.VillageId)
                    .HasColumnName("VillageID")
                    .HasComment("小区id");

                entity.HasOne(d => d.GrabageRoom)
                    .WithMany(p => p.VolunteerService)
                    .HasForeignKey(d => d.GrabageRoomId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("垃圾房_志愿者服务");

                entity.HasOne(d => d.SupervisorUu)
                    .WithMany(p => p.VolunteerServiceSupervisorUu)
                    .HasForeignKey(d => d.SupervisorUuid)
                    .HasConstraintName("督导员_志愿者");

                entity.HasOne(d => d.SystemUserUu)
                    .WithMany(p => p.VolunteerServiceSystemUserUu)
                    .HasForeignKey(d => d.SystemUserUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("志愿者");

                entity.HasOne(d => d.Village)
                    .WithMany(p => p.VolunteerService)
                    .HasForeignKey(d => d.VillageId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("小区_志愿者服务");
            });

            modelBuilder.Entity<VolunteerYy>(entity =>
            {
                entity.HasKey(e => e.VolunteerYyuuid)
                    .HasName("PK__Voluntee__14FAF34172856863");

                entity.ToTable("VolunteerYY");

                entity.HasComment("志愿者预约");

                entity.Property(e => e.VolunteerYyuuid)
                    .HasColumnName("VolunteerYYUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AddTime).HasMaxLength(20);

                entity.Property(e => e.Ap)
                    .HasColumnName("AP")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("上午或下午");

                entity.Property(e => e.GrabRoomUuid)
                    .HasColumnName("GrabRoomUUid")
                    .HasComment("垃圾厢房id");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.StartTime)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("开始志愿时间");

                entity.Property(e => e.VolunteerUuid)
                    .HasColumnName("VolunteerUUid")
                    .HasComment("志愿者id");

                entity.HasOne(d => d.GrabRoomUu)
                    .WithMany(p => p.VolunteerYy)
                    .HasForeignKey(d => d.GrabRoomUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("厢房_预约");

                entity.HasOne(d => d.VolunteerUu)
                    .WithMany(p => p.VolunteerYy)
                    .HasForeignKey(d => d.VolunteerUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("志愿者_预约");
            });

            modelBuilder.Entity<WasteRatio>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("WasteRatio");

                entity.Property(e => e.Eflj)
                    .HasColumnName("eflj")
                    .HasColumnType("numeric(20, 2)");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Per)
                    .HasColumnName("per")
                    .HasColumnType("numeric(20, 2)");

                entity.Property(e => e.Qtlj)
                    .HasColumnName("qtlj")
                    .HasColumnType("numeric(20, 2)");

                entity.Property(e => e.Vname)
                    .HasColumnName("VName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Waste)
                    .HasColumnName("waste")
                    .HasColumnType("numeric(20, 2)");
            });

            modelBuilder.Entity<WingNoRecord>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("WingNoRecord");

                entity.Property(e => e.ColckDate).HasMaxLength(255);

                entity.Property(e => e.GarbageRoomUuid).HasColumnName("GarbageRoomUUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ljname)
                    .HasColumnName("LJName")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SystemUserUuid).HasColumnName("SystemUserUUID");

                entity.Property(e => e.Times)
                    .HasColumnName("times")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Vname)
                    .HasColumnName("VName")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WorkTime>(entity =>
            {
                entity.HasKey(e => e.WorkTimeUuid)
                    .HasName("PK__WorkTime__EAF4DCDC67F1A5B9");

                entity.HasComment("考勤时间设置");

                entity.Property(e => e.WorkTimeUuid)
                    .HasColumnName("WorkTimeUUID")
                    .HasComment("主键")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.EndTime)
                    .HasMaxLength(255)
                    .HasComment("下班时间");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("自增序列")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDeleted).HasComment("是否删除(0,未删,1,删除)");

                entity.Property(e => e.StartTime)
                    .HasMaxLength(255)
                    .HasComment("上班时间");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

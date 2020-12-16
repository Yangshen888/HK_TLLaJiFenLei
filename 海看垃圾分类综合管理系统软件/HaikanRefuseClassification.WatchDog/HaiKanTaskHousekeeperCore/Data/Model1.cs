namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<CarMonitoring> CarMonitoring { get; set; }
        public virtual DbSet<DateScore> DateScore { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<DeviceToCar> DeviceToCar { get; set; }
        public virtual DbSet<Driver> Driver { get; set; }
        public virtual DbSet<Goods> Goods { get; set; }
        public virtual DbSet<GoodsExchange> GoodsExchange { get; set; }
        public virtual DbSet<GrabageDisposal> GrabageDisposal { get; set; }
        public virtual DbSet<GrabageDisposal_copy1> GrabageDisposal_copy1 { get; set; }
        public virtual DbSet<GrabageMonitoring> GrabageMonitoring { get; set; }
        public virtual DbSet<GrabageRoom> GrabageRoom { get; set; }
        public virtual DbSet<GrabageWeighting> GrabageWeighting { get; set; }
        public virtual DbSet<GrabageWeightSon> GrabageWeightSon { get; set; }
        public virtual DbSet<GrabType> GrabType { get; set; }
        public virtual DbSet<HomeAddress> HomeAddress { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Overallsituation> Overallsituation { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionPerson> QuestionPerson { get; set; }
        public virtual DbSet<Region_info> Region_info { get; set; }
        public virtual DbSet<Response> Response { get; set; }
        public virtual DbSet<RubbishSYRecord> RubbishSYRecord { get; set; }
        public virtual DbSet<ScoreSetting> ScoreSetting { get; set; }
        public virtual DbSet<Shop> Shop { get; set; }
        public virtual DbSet<SupervisorInspection> SupervisorInspection { get; set; }
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
        public virtual DbSet<Village> Village { get; set; }
        public virtual DbSet<VolunteerService> VolunteerService { get; set; }
        public virtual DbSet<VolunteerYY> VolunteerYY { get; set; }
        public virtual DbSet<WorkTime> WorkTime { get; set; }
        public virtual DbSet<CarWeightView> CarWeightView { get; set; }
        public virtual DbSet<HomeAddress_view> HomeAddress_view { get; set; }
        public virtual DbSet<HomeDateScore> HomeDateScore { get; set; }
        public virtual DbSet<Leaderboard> Leaderboard { get; set; }
        public virtual DbSet<OWNSource> OWNSource { get; set; }
        public virtual DbSet<PerishableRubbish> PerishableRubbish { get; set; }
        public virtual DbSet<RecordTime> RecordTime { get; set; }
        public virtual DbSet<RecycleInfo> RecycleInfo { get; set; }
        public virtual DbSet<RubbishAll> RubbishAll { get; set; }
        public virtual DbSet<ShouScore> ShouScore { get; set; }
        public virtual DbSet<Supervisor_Volunteer> Supervisor_Volunteer { get; set; }
        public virtual DbSet<WingNoRecord> WingNoRecord { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<Attendance>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<Attendance>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Attendance>()
                .Property(e => e.C_Volunteerevaluation_)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.BalanceID)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.Photo)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.CameraID)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.Brand)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.HolderPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.GrabType)
                .IsUnicode(false);

            modelBuilder.Entity<DeviceToCar>()
                .Property(e => e.IMEI)
                .IsUnicode(false);

            modelBuilder.Entity<DeviceToCar>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<DeviceToCar>()
                .Property(e => e.AddPerson)
                .IsUnicode(false);

            modelBuilder.Entity<Goods>()
                .Property(e => e.GName)
                .IsUnicode(false);

            modelBuilder.Entity<Goods>()
                .Property(e => e.Price)
                .IsUnicode(false);

            modelBuilder.Entity<Goods>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<Goods>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<Goods>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<GoodsExchange>()
                .Property(e => e.DeduceScore)
                .IsUnicode(false);

            modelBuilder.Entity<GoodsExchange>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageDisposal>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageDisposal>()
                .Property(e => e.GrabType)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageDisposal_copy1>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageDisposal_copy1>()
                .Property(e => e.GrabType)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.Posititon)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.LJName)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.EWM)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.RottenRubbishPercent)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.StartTime)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.EndTime)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.Lon)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.Lat)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.Towns)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .Property(e => e.Facilityuuid)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageRoom>()
                .HasMany(e => e.GrabageDisposal)
                .WithOptional(e => e.GrabageRoom)
                .HasForeignKey(e => e.GrabageRoomID);

            modelBuilder.Entity<GrabageRoom>()
                .HasMany(e => e.GrabageDisposal_copy1)
                .WithOptional(e => e.GrabageRoom)
                .HasForeignKey(e => e.GrabageRoomID);

            modelBuilder.Entity<GrabageRoom>()
                .HasMany(e => e.GrabageWeighting)
                .WithOptional(e => e.GrabageRoom)
                .HasForeignKey(e => e.GrabageRoomID);

            modelBuilder.Entity<GrabageRoom>()
                .HasMany(e => e.GrabageWeightSon)
                .WithOptional(e => e.GrabageRoom)
                .HasForeignKey(e => e.GrabageRoomID);

            modelBuilder.Entity<GrabageRoom>()
                .HasMany(e => e.Question)
                .WithOptional(e => e.GrabageRoom)
                .HasForeignKey(e => e.QueRoomID);

            modelBuilder.Entity<GrabageRoom>()
                .HasMany(e => e.VolunteerService)
                .WithOptional(e => e.GrabageRoom)
                .HasForeignKey(e => e.GrabageRoomID);

            modelBuilder.Entity<GrabageRoom>()
                .HasMany(e => e.SystemUser)
                .WithOptional(e => e.GrabageRoom)
                .HasForeignKey(e => e.GrabageRoomID);

            modelBuilder.Entity<GrabageRoom>()
                .HasMany(e => e.VolunteerYY)
                .WithOptional(e => e.GrabageRoom)
                .HasForeignKey(e => e.GrabRoomUUid);

            modelBuilder.Entity<GrabageRoom>()
                .HasMany(e => e.RubbishSYRecord)
                .WithOptional(e => e.GrabageRoom)
                .HasForeignKey(e => e.GrabageRoomUUID);

            modelBuilder.Entity<GrabageWeighting>()
                .Property(e => e.CarNumber)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeighting>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeighting>()
                .Property(e => e.Weight)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeighting>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeighting>()
                .Property(e => e.RecordType)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeighting>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeighting>()
                .Property(e => e.CheckedUser)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeighting>()
                .Property(e => e.WeightTime)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeighting>()
                .HasMany(e => e.RubbishSYRecord)
                .WithOptional(e => e.GrabageWeighting)
                .HasForeignKey(e => e.GrabageWeighingUUID);

            modelBuilder.Entity<GrabageWeightSon>()
                .Property(e => e.CarNumber)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeightSon>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeightSon>()
                .Property(e => e.Weight)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeightSon>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeightSon>()
                .Property(e => e.RecordType)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeightSon>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeightSon>()
                .Property(e => e.CheckedUser)
                .IsUnicode(false);

            modelBuilder.Entity<GrabageWeightSon>()
                .Property(e => e.WeightTime)
                .IsUnicode(false);

            modelBuilder.Entity<GrabType>()
                .Property(e => e.GrabName)
                .IsUnicode(false);

            modelBuilder.Entity<GrabType>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<GrabType>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<GrabType>()
                .Property(e => e.AddPeopel)
                .IsUnicode(false);

            modelBuilder.Entity<GrabType>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<HomeAddress>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<HomeAddress>()
                .Property(e => e.Addresscode)
                .IsUnicode(false);

            modelBuilder.Entity<HomeAddress>()
                .HasMany(e => e.SupervisorInspection)
                .WithRequired(e => e.HomeAddress)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                .Property(e => e.Message1)
                .IsUnicode(false);

            modelBuilder.Entity<Message>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<Overallsituation>()
                .Property(e => e.SetName)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.QueType)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionPerson>()
                .Property(e => e.ProblemType)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionPerson>()
                .Property(e => e.ProblemContent)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionPerson>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionPerson>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionPerson>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Response>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<Response>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<Response>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<Response>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<RubbishSYRecord>()
                .Property(e => e.RubbishType)
                .IsUnicode(false);

            modelBuilder.Entity<RubbishSYRecord>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<RubbishSYRecord>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<RubbishSYRecord>()
                .Property(e => e.CorruptRubbishPercent)
                .IsUnicode(false);

            modelBuilder.Entity<ScoreSetting>()
                .Property(e => e.ScoreName)
                .IsUnicode(false);

            modelBuilder.Entity<ScoreSetting>()
                .Property(e => e.Addpeople)
                .IsUnicode(false);

            modelBuilder.Entity<ScoreSetting>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<ScoreSetting>()
                .HasMany(e => e.GrabageDisposal)
                .WithOptional(e => e.ScoreSetting)
                .HasForeignKey(e => e.ScoreSettingUUid);

            modelBuilder.Entity<ScoreSetting>()
                .HasMany(e => e.GrabageDisposal_copy1)
                .WithOptional(e => e.ScoreSetting)
                .HasForeignKey(e => e.ScoreSettingUUid);

            modelBuilder.Entity<Shop>()
                .Property(e => e.Shopowner)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .Property(e => e.Addtime)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .Property(e => e.ShopName)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .Property(e => e.Lat)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .Property(e => e.Lon)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .HasMany(e => e.Goods)
                .WithOptional(e => e.Shop)
                .HasForeignKey(e => e.ShopID);

            modelBuilder.Entity<Shop>()
                .HasMany(e => e.GoodsExchange)
                .WithOptional(e => e.Shop)
                .HasForeignKey(e => e.ShopID);

            modelBuilder.Entity<SupervisorInspection>()
                .Property(e => e.GarbageSoring)
                .IsUnicode(false);

            modelBuilder.Entity<SupervisorInspection>()
                .Property(e => e.Picture)
                .IsUnicode(false);

            modelBuilder.Entity<SupervisorInspection>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<SupervisorInspection>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<SupervisorInspection>()
                .Property(e => e.Grade)
                .IsUnicode(false);

            modelBuilder.Entity<SupervisorsWork>()
                .Property(e => e.GrabageType)
                .IsUnicode(false);

            modelBuilder.Entity<SupervisorsWork>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.OperationContent)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.IPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<SystemSetting>()
                .Property(e => e.ClobalName)
                .IsUnicode(false);

            modelBuilder.Entity<SystemSetting>()
                .Property(e => e.ClobalSuo)
                .IsUnicode(false);

            modelBuilder.Entity<SystemSetting>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.ManageDepartment)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.ZaiGang)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Wechat)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.OldCard)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.InTime)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Types)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.SystemRoleUUid)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.ProblemContent)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.EWM)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.IDCardMD5)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .HasMany(e => e.GrabageDisposal)
                .WithOptional(e => e.SystemUser)
                .HasForeignKey(e => e.SupervisorUUID);

            modelBuilder.Entity<SystemUser>()
                .HasMany(e => e.GrabageDisposal1)
                .WithOptional(e => e.SystemUser1)
                .HasForeignKey(e => e.SystemUserUUID);

            modelBuilder.Entity<SystemUser>()
                .HasMany(e => e.GrabageDisposal_copy1)
                .WithOptional(e => e.SystemUser)
                .HasForeignKey(e => e.SupervisorUUID);

            modelBuilder.Entity<SystemUser>()
                .HasMany(e => e.GrabageDisposal_copy11)
                .WithOptional(e => e.SystemUser1)
                .HasForeignKey(e => e.SystemUserUUID);

            modelBuilder.Entity<SystemUser>()
                .HasMany(e => e.QuestionPerson)
                .WithOptional(e => e.SystemUser)
                .HasForeignKey(e => e.HomeUserUUID);

            modelBuilder.Entity<SystemUser>()
                .HasMany(e => e.SupervisorsWork)
                .WithOptional(e => e.SystemUser)
                .HasForeignKey(e => e.SupervisorID);

            modelBuilder.Entity<SystemUser>()
                .HasMany(e => e.SupervisorsWork1)
                .WithOptional(e => e.SystemUser1)
                .HasForeignKey(e => e.OwnerID);

            modelBuilder.Entity<SystemUser>()
                .HasMany(e => e.VolunteerService)
                .WithOptional(e => e.SystemUser)
                .HasForeignKey(e => e.SupervisorUUid);

            modelBuilder.Entity<SystemUser>()
                .HasMany(e => e.VolunteerService1)
                .WithOptional(e => e.SystemUser1)
                .HasForeignKey(e => e.SystemUserUUID);

            modelBuilder.Entity<SystemUser>()
                .HasMany(e => e.VolunteerYY)
                .WithOptional(e => e.SystemUser)
                .HasForeignKey(e => e.VolunteerUUid);

            modelBuilder.Entity<VehicleInfo>()
                .Property(e => e.Addtime)
                .IsUnicode(false);

            modelBuilder.Entity<VehicleInfo>()
                .Property(e => e.Lon)
                .IsUnicode(false);

            modelBuilder.Entity<VehicleInfo>()
                .Property(e => e.Lat)
                .IsUnicode(false);

            modelBuilder.Entity<Village>()
                .Property(e => e.VName)
                .IsUnicode(false);

            modelBuilder.Entity<Village>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Village>()
                .Property(e => e.Addpeople)
                .IsUnicode(false);

            modelBuilder.Entity<Village>()
                .Property(e => e.AddTime)
                .IsUnicode(false);

            modelBuilder.Entity<Village>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<Village>()
                .Property(e => e.Towns)
                .IsUnicode(false);

            modelBuilder.Entity<Village>()
                .HasMany(e => e.GrabageRoom)
                .WithOptional(e => e.Village)
                .HasForeignKey(e => e.VillageID);

            modelBuilder.Entity<Village>()
                .HasMany(e => e.SystemUser)
                .WithOptional(e => e.Village)
                .HasForeignKey(e => e.VillageID);

            modelBuilder.Entity<Village>()
                .HasMany(e => e.VolunteerService)
                .WithOptional(e => e.Village)
                .HasForeignKey(e => e.VillageID);

            modelBuilder.Entity<VolunteerService>()
                .Property(e => e.Score)
                .IsUnicode(false);

            modelBuilder.Entity<VolunteerService>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<VolunteerService>()
                .Property(e => e.IsDelete)
                .IsUnicode(false);

            modelBuilder.Entity<VolunteerService>()
                .Property(e => e.ScoreAddtime)
                .IsUnicode(false);

            modelBuilder.Entity<VolunteerYY>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<VolunteerYY>()
                .Property(e => e.StartTime)
                .IsUnicode(false);

            modelBuilder.Entity<VolunteerYY>()
                .Property(e => e.AP)
                .IsUnicode(false);

            modelBuilder.Entity<CarWeightView>()
                .Property(e => e.CarNumber)
                .IsUnicode(false);

            modelBuilder.Entity<CarWeightView>()
                .Property(e => e.GrabType)
                .IsUnicode(false);

            modelBuilder.Entity<CarWeightView>()
                .Property(e => e.WTime)
                .IsUnicode(false);

            modelBuilder.Entity<CarWeightView>()
                .Property(e => e.Weight)
                .HasPrecision(20, 2);

            modelBuilder.Entity<HomeAddress_view>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Leaderboard>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<OWNSource>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<RecordTime>()
                .Property(e => e.times)
                .IsUnicode(false);

            modelBuilder.Entity<RecycleInfo>()
                .Property(e => e.LJName)
                .IsUnicode(false);

            modelBuilder.Entity<RecycleInfo>()
                .Property(e => e.VName)
                .IsUnicode(false);

            modelBuilder.Entity<RecycleInfo>()
                .Property(e => e.Towns)
                .IsUnicode(false);

            modelBuilder.Entity<RecycleInfo>()
                .Property(e => e.Lon)
                .IsUnicode(false);

            modelBuilder.Entity<RecycleInfo>()
                .Property(e => e.Lat)
                .IsUnicode(false);

            modelBuilder.Entity<RecycleInfo>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<ShouScore>()
                .Property(e => e.ShopName)
                .IsUnicode(false);

            modelBuilder.Entity<ShouScore>()
                .Property(e => e.Shopowner)
                .IsUnicode(false);

            modelBuilder.Entity<ShouScore>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<ShouScore>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<ShouScore>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<ShouScore>()
                .Property(e => e.Lat)
                .IsUnicode(false);

            modelBuilder.Entity<ShouScore>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<ShouScore>()
                .Property(e => e.Lon)
                .IsUnicode(false);

            modelBuilder.Entity<ShouScore>()
                .Property(e => e.Addtime)
                .IsUnicode(false);

            modelBuilder.Entity<Supervisor_Volunteer>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Supervisor_Volunteer>()
                .Property(e => e.AP)
                .IsUnicode(false);

            modelBuilder.Entity<WingNoRecord>()
                .Property(e => e.times)
                .IsUnicode(false);

            modelBuilder.Entity<WingNoRecord>()
                .Property(e => e.LJName)
                .IsUnicode(false);

            modelBuilder.Entity<WingNoRecord>()
                .Property(e => e.VName)
                .IsUnicode(false);
        }
    }
}

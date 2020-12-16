namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrabageRoom")]
    public partial class GrabageRoom
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GrabageRoom()
        {
            Attendance = new HashSet<Attendance>();
            GrabageDisposal = new HashSet<GrabageDisposal>();
            GrabageDisposal_copy1 = new HashSet<GrabageDisposal_copy1>();
            GrabageWeighting = new HashSet<GrabageWeighting>();
            GrabageWeightSon = new HashSet<GrabageWeightSon>();
            Question = new HashSet<Question>();
            VolunteerService = new HashSet<VolunteerService>();
            SystemUser = new HashSet<SystemUser>();
            VolunteerYY = new HashSet<VolunteerYY>();
            RubbishSYRecord = new HashSet<RubbishSYRecord>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid GarbageRoomUUID { get; set; }

        [StringLength(255)]
        public string Posititon { get; set; }

        [StringLength(255)]
        public string LJName { get; set; }

        public Guid? VillageID { get; set; }

        [StringLength(255)]
        public string EWM { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public Guid? SystemUserUUID { get; set; }

        [StringLength(255)]
        public string State { get; set; }

        public Guid? CarID { get; set; }

        [StringLength(255)]
        public string RottenRubbishPercent { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        [StringLength(255)]
        public string StartTime { get; set; }

        [StringLength(255)]
        public string EndTime { get; set; }

        [StringLength(255)]
        public string Lon { get; set; }

        [StringLength(255)]
        public string Lat { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string Towns { get; set; }

        [StringLength(100)]
        public string Facilityuuid { get; set; }

        [StringLength(255)]
        public string MonitoringNum { get; set; }

        [StringLength(255)]
        public string WingID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendance { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrabageDisposal> GrabageDisposal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrabageDisposal_copy1> GrabageDisposal_copy1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrabageWeighting> GrabageWeighting { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrabageWeightSon> GrabageWeightSon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Question { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolunteerService> VolunteerService { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SystemUser> SystemUser { get; set; }

        public virtual Village Village { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolunteerYY> VolunteerYY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RubbishSYRecord> RubbishSYRecord { get; set; }
    }
}

namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemUser")]
    public partial class SystemUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SystemUser()
        {
            Attendance = new HashSet<Attendance>();
            GrabageDisposal = new HashSet<GrabageDisposal>();
            GrabageDisposal1 = new HashSet<GrabageDisposal>();
            GrabageDisposal_copy1 = new HashSet<GrabageDisposal_copy1>();
            GrabageDisposal_copy11 = new HashSet<GrabageDisposal_copy1>();
            Message = new HashSet<Message>();
            QuestionPerson = new HashSet<QuestionPerson>();
            SupervisorsWork = new HashSet<SupervisorsWork>();
            SupervisorsWork1 = new HashSet<SupervisorsWork>();
            VolunteerService = new HashSet<VolunteerService>();
            VolunteerService1 = new HashSet<VolunteerService>();
            VolunteerYY = new HashSet<VolunteerYY>();
        }

        [Key]
        public Guid SystemUserUUID { get; set; }

        [StringLength(255)]
        public string LoginName { get; set; }

        [StringLength(255)]
        public string RealName { get; set; }

        [StringLength(255)]
        public string UserIdCard { get; set; }

        [StringLength(255)]
        public string PassWord { get; set; }

        public int? UserType { get; set; }

        public Guid? DepartmentUUID { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? IsDeleted { get; set; }

        [Column(TypeName = "text")]
        public string ManageDepartment { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid? GrabageRoomID { get; set; }

        public Guid? VillageID { get; set; }

        [StringLength(255)]
        public string ZaiGang { get; set; }

        [StringLength(255)]
        public string Wechat { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string OldCard { get; set; }

        [StringLength(255)]
        public string Sex { get; set; }

        [StringLength(20)]
        public string InTime { get; set; }

        [StringLength(255)]
        public string Types { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string SystemRoleUUid { get; set; }

        [StringLength(255)]
        public string ProblemContent { get; set; }

        [StringLength(255)]
        public string ProblemType { get; set; }

        [StringLength(255)]
        public string EWM { get; set; }

        [StringLength(255)]
        public string Remarks { get; set; }

        public Guid? ShopUUID { get; set; }

        public Guid? HomeAddressUUID { get; set; }

        [StringLength(255)]
        public string IDCardMD5 { get; set; }

        public string Streets { get; set; }

        public string Community { get; set; }

        public string Biotope { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendance { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrabageDisposal> GrabageDisposal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrabageDisposal> GrabageDisposal1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrabageDisposal_copy1> GrabageDisposal_copy1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrabageDisposal_copy1> GrabageDisposal_copy11 { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }

        public virtual HomeAddress HomeAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Message { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionPerson> QuestionPerson { get; set; }

        public virtual Shop Shop { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupervisorsWork> SupervisorsWork { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupervisorsWork> SupervisorsWork1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolunteerService> VolunteerService { get; set; }

        public virtual Village Village { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolunteerService> VolunteerService1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VolunteerYY> VolunteerYY { get; set; }
    }
}

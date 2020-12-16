namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Car")]
    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            CarMonitoring = new HashSet<CarMonitoring>();
            DeviceToCar = new HashSet<DeviceToCar>();
            GrabageWeighting = new HashSet<GrabageWeighting>();
            GrabageWeightSon = new HashSet<GrabageWeightSon>();
            Question = new HashSet<Question>();
        }

        [Key]
        public Guid CarUUID { get; set; }

        [StringLength(255)]
        public string CarNum { get; set; }

        [StringLength(255)]
        public string CarType { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        public int? IsDelete { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(255)]
        public string HolderID { get; set; }

        [StringLength(255)]
        public string RegisterTime { get; set; }

        [StringLength(255)]
        public string AwardTime { get; set; }

        [StringLength(255)]
        public string Weight { get; set; }

        [StringLength(255)]
        public string BalanceID { get; set; }

        [StringLength(255)]
        public string Photo { get; set; }

        [StringLength(255)]
        public string CameraID { get; set; }

        [StringLength(255)]
        public string Brand { get; set; }

        [StringLength(255)]
        public string HolderPhone { get; set; }

        [StringLength(255)]
        public string GrabType { get; set; }

        [StringLength(255)]
        public string company { get; set; }

        [StringLength(255)]
        public string Street { get; set; }

        [StringLength(255)]
        public string OnlyNum { get; set; }

        public string GrabageRoomID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarMonitoring> CarMonitoring { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeviceToCar> DeviceToCar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrabageWeighting> GrabageWeighting { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrabageWeightSon> GrabageWeightSon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Question { get; set; }
    }
}

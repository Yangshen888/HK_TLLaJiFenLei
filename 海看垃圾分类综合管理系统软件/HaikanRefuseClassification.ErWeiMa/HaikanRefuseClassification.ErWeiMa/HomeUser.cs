namespace HaikanDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HomeUser")]
    public partial class HomeUser
    {
        [Key]
        public Guid HomeUserUUID { get; set; }

        [StringLength(255)]
        public string LoginName { get; set; }

        [StringLength(255)]
        public string RealName { get; set; }

        [StringLength(255)]
        public string UserIdCard { get; set; }

        [StringLength(255)]
        public string PassWord { get; set; }

        public int? UserType { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? IsDeleted { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(255)]
        public string ZaiGang { get; set; }

        [StringLength(255)]
        public string Wechat { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Sex { get; set; }

        [StringLength(255)]
        public string SystemRoleUUid { get; set; }

        public Guid? ShopUUID { get; set; }

        public Guid? HomeAddressUUID { get; set; }
    }
}

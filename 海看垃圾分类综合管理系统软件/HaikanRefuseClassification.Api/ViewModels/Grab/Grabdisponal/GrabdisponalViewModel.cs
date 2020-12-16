
using HaikanRefuseClassification.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.ViewModels
{
    public class GrabdisponalViewModel
    {
        public Guid GarbageDisposalUuid { get; set; }
        public int Id { get; set; }
        public Guid? GrabageRoomId { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? SupervisorId { get; set; }
        public string AddTime { get; set; }
        public string Score { get; set; }
        public string IsDelete { get; set; }

        public virtual GrabageRoom GrabageRoom { get; set; }
        public virtual Ownsource Owner { get; set; }
        public virtual SystemUser Supervisor { get; set; }
    }
}

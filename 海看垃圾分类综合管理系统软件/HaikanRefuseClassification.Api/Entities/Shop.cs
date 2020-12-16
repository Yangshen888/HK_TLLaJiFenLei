using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Shop
    {
        public Shop()
        {
            Goods = new HashSet<Goods>();
            GoodsExchange = new HashSet<GoodsExchange>();
            SystemUser = new HashSet<SystemUser>();
        }

        public Guid ShopUuid { get; set; }
        public string Shopowner { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string AddPeople { get; set; }
        public string Addtime { get; set; }
        public int Id { get; set; }
        public string IsDelete { get; set; }
        public string ShopName { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }

        public virtual ICollection<Goods> Goods { get; set; }
        public virtual ICollection<GoodsExchange> GoodsExchange { get; set; }
        public virtual ICollection<SystemUser> SystemUser { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class Goods
    {
        public Goods()
        {
            GoodsExchange = new HashSet<GoodsExchange>();
        }

        public Guid GoodsId { get; set; }
        public string Gname { get; set; }
        public string Price { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public Guid? ShopId { get; set; }
        public string State { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual ICollection<GoodsExchange> GoodsExchange { get; set; }
    }
}

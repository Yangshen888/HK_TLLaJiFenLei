using System;
using System.Collections.Generic;

namespace HaikanRefuseClassification.Api.Entities
{
    public partial class GoodsExchange
    {
        public Guid StoreExchangeUuid { get; set; }
        public int Id { get; set; }
        public Guid? ShopId { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string ExchageTime { get; set; }
        public string DeduceScore { get; set; }
        public string IsDelete { get; set; }
        public Guid? GoodsId { get; set; }

        public virtual Goods Goods { get; set; }
        public virtual Shop Shop { get; set; }
    }
}

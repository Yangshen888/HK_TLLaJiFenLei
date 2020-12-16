using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCampusManagementWatchDog.Model
{
    public class Track
    {
        /// <summary>
        /// 
        /// </summary>
        public List<DataItem> data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string success { get; set; }
        public class CardInfosItem
        {
            /// <summary>
            /// 
            /// </summary>
            public int business1 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int business2 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int business3 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int business4 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string number { get; set; }
        }

        public class RfidListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public List<CardInfosItem> cardInfos { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string content { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string count { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int important { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int importent { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int len { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int unusual { get; set; }
        }

        public class DataItem
        {
            /// <summary>
            /// 
            /// </summary>
            public int alarm { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int altitude { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int angle { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<long> fuelStatus { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<long> fuelTankAdd { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<double> fuelTankCapacity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<long> fuelTankLeak { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<double> fuelTankOilTemp { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<double> fuelTankTemp { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int latitude { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<long> loadStatus { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<long> loadWeight { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int longitude { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int mileage { get; set; }
            /// <summary>
            /// 浙A6S826
            /// </summary>
            public string monitorName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<RfidListItem> rfidList { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int satellitesNumber { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int speed { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<double> tyrePressure { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<long> tyreStatus { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<long> tyreTemp { get; set; }
        }

    }
}

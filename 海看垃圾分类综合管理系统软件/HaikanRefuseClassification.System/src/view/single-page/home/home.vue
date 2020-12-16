<template>
  <div ref="mainBox">
    <Row :gutter="10" style="margin-top: 10px;">
       <Row :gutter="10">
        <i-col
          :xs="12"
          :md="8"
          :lg="4"
          v-for="(infor, i) in inforCardData2"
          :key="`infor-${i}`"
          style="height: 120px;padding-bottom: 10px;"
        >
          <infor-card shadow :color="infor.color" :icon="infor.icon" :icon-size="36">
            <count-to id="countBox" ref="count" :end="infor.count" :style="screenWidth>1600? 'font-size:50px;' : 'font-size:30px;'"  />
            <p>{{ infor.title }}</p>
          </infor-card>
        </i-col>
      </Row>
      <Row :gutter="10">
        <i-col
          :xs="12"
          :md="8"
          :lg="4"
          v-for="(infor, i) in inforCardData"
          :key="`infor-${i}`"
          style="height: 120px;padding-bottom: 10px;"
        >
          <infor-card shadow :color="infor.color" :icon="infor.icon" :icon-size="36">
            <count-to :end="infor.count" :style="screenWidth>1600? 'font-size:50px;' : 'font-size:30px;'" />
            <p>{{ infor.title }}</p>
          </infor-card>
        </i-col>
      </Row>
    </Row>
    <Row :gutter="10" style="margin-top: 10px;">
      <i-col :md="24" :lg="8" style="margin-bottom: 20px;">
        <Card shadow v-if="isShow">
          <chart-pie style="height: 300px;" :value="pieData" text="各类垃圾占比"></chart-pie>
        </Card>
      </i-col>
      <i-col
        :md="24"
        :lg="8"
        style="margin-bottom: 20px;"
        v-if="tabData!=null&&tabData!=undefined&&tabData.length>0"
      >
        <Card shadow>
          <p slot="title">本月各社区易腐垃圾比例排行榜</p>
          <Table height="250" width="auto" :columns="columns" :data="tabData"></Table>
        </Card>
      </i-col>
      <i-col
        :md="24"
        :lg="8"
        style="margin-bottom: 20px;"
        v-if="tabData1!=null&&tabData1!=undefined&&tabData1.length>0"
      >
        <Card shadow>
          <p slot="title">本月志愿者积分排行榜</p>
          <Table height="250" width="auto" :columns="columns1" :data="tabData1"></Table>
        </Card>
      </i-col>
    </Row>
    <Row :gutter="10" style="margin-top: 10px;">
      <i-col :md="24" :lg="12" style="margin-bottom: 20px;">
        <Card shadow>
          <div id="c3" style="height: 300px;width:100%;"></div>
        </Card>
      </i-col>
      <i-col :md="24" :lg="12" style="margin-bottom: 20px;">
        <Card shadow>
          <div id="c4" style="height: 300px;"></div>
        </Card>
      </i-col>
    </Row>
    <Row></Row>
  </div>
</template>

<script>
// 引入基本模板
let echarts = require("echarts/lib/echarts");
// 引入柱状图组件
require("echarts/lib/chart/bar");
require("echarts/lib/chart/line");
require("echarts/lib/chart/pie");

// 引入提示框和title组件
require("echarts/lib/component/tooltip");
require("echarts/lib/component/title");
require("echarts/theme/dark");
import InforCard from "_c/info-card";
import CountTo from "_c/count-to";
import { ChartPie, ChartBar } from "_c/charts";
import tdTheme from "_c/charts/theme.json";
echarts.registerTheme("tdTheme", tdTheme);
import Example from "./example.vue";
import {
  getHPageData1,
  getData1,
  getData4,
  getData2,
  getData3,
  getData6,
  getData7,
  getData8,
  GetVisitorNum
} from "@/api/GiMap/DataBord";
export default {
  name: "home",
  components: {
    InforCard,
    CountTo,
    ChartPie,
    ChartBar,
    Example
  },
  data() {
    return {
      isShow: false,
      cc:0,
      inforCardData: [
        {
          title: "新增用户",
          icon: "md-person-add",
          count: 0,
          color: "#2d8cf0"
        },
        { title: "累计点击", icon: "md-locate", count: 0, color: "#ff9900" },
        {
          title: "新增垃圾箱房",
          icon: "md-home",
          count: 0,
          color: "#19be6b"
        },
        {
          title: "新增志愿者",
          icon: "ios-person-add",
          count: 0,
          color: "#ed3f14"
        },
      ],
      inforCardData2: [
        { title: "参与小区", icon: "ios-home", count: 0, color: "#2d8cf0" },
        { title: "垃圾箱房", icon: "md-home", count: 0, color: "#19be6b" },
        { title: "积分用户", icon: "md-people", count: 0, color: "#2d8cf0" },
        
        { title: "累计积分", icon: "md-ribbon", count: 0, color: "#FF1493" },
        { title: "合作商店", icon: "md-cart", count: 0, color: "#2d8cf0" },
        {
          title: "累计兑换积分",
          icon: "ios-ribbon-outline",
          count: 0,
          color: "#ed3f14"
        },
      ],

      pieData: [
        { value: 0, name: "其他垃圾" },
        { value: 0, name: "易腐垃圾" },

        { value: 0, name: "可回收垃圾" },
        { value: 0, name: "有害" }
      ],
      barData: {
        Mon: 13253,
        Tue: 34235,
        Wed: 26321,
        Thu: 12340,
        Fri: 24643,
        Sat: 1322,
        Sun: 1324
      },
      columns: [
        {
          title: "序号",
          type: "index",
          width: 50,
          align: "center"
        },
        {
          title: "社区",
          key: "vname"
        },
        {
          title: "易腐垃圾",
          key: "c1"
        },
        {
          title: "其他垃圾",
          key: "c2"
        },
        {
          title: "易腐垃圾比例",
          key: "percent"
        }
      ],
      columns1: [
        {
          title: "序号",
          type: "index",
          width: 50,
          align: "center"
        },
        {
          title: "微信名",
          key: "loginName"
        },
        {
          title: "积分",
          key: "zyzsc"
        }
      ],
      tabData: "",
      tabData1: "",
      tabData2: "",
      screenWidth: this.$refs.mainBox,//屏幕宽度
      size:'50px'
    };
  },
  // watch:{
  //     screenWidth(val){ //监听屏幕宽度变化
  //         var screenWidth=document.body.clientWidth;
  //         console.log(screenWidth)
  //         if(screenWidth > 1600){
  //           this.fontSize = "50px";
  //         }else if(screenWidth > 1200){
  //           this.fontSize = "35px";
  //         }else{
  //           this.fontSize = "30px";
  //         }
  //         // console.log(this.screenWidth);
  //         // let length=document.getElementById('countBox');
  //         //   console.log(length.style.fontSize);
  //         // if(this.screenWidth<1200){
  //         //   length.style.fontSize='46px';
  //         //   console.log(length.style.fontSize);
  //         // }
  //         // console.log(length.offsetWidth);
  //     }
  // },
  methods: {
    DoGetData3() {
      getData3().then(res => {
        let max = res.data.data.num[0];
        res.data.data.num.forEach(item => (max = item > max ? item : max));
        let fenge = parseFloat(max / 5).toFixed(2);
        //console.log(fenge);
        this.ljfenge = parseFloat(fenge);
        this.ljmax = this.ljfenge * 5;
        // 基于准备好的dom，初始化echarts实例
      let myChart = echarts.init(document.getElementById("c3"), "tdTheme");
      // 绘制图表
      let option = {
        title: {
          text: "易腐垃圾统计",
          x: "center"
        },
        backgroundColor: "rgba(225, 225, 225, 0)", //rgba设置透明度0.1,
        tooltip: {
          trigger: "axis",
          axisPointer: {
            type: "cross",
            crossStyle: {
              color: "#000000"
            }
          }
        },
        toolbox: {
          feature: {
            magicType: { show: true, type: ["line", "bar"] },
            restore: { show: true }
          }
        },
        legend: {
          data: ["垃圾总量", "易腐垃圾比例"],
          orient: "horizontal",
          bottom: "5"
        },
        tooltip: {
          trigger: "axis",
          formatter: "{b0}<br/>{a0}: {c0}吨<br />{a1}：{c1}%"
        },

        grid: {
          left: "3%",
          right: "6%",
          bottom: "12%",
          containLabel: true
        },
        xAxis: [
          {
            type: "category",
            data: [
              "1月",
              "2月",
              "3月",
              "4月",
              "5月",
              "6月",
              "7月",
              "8月",
              "9月",
              "10月",
              "11月",
              "12月"
            ],
            axisPointer: {
              type: "shadow"
            }
          }
        ],
        yAxis: [
          {
            type: "value",
            name: "垃圾总量（吨）",
            min: 0,
            max: this.ljmax,
            interval: this.ljfenge,
            axisLabel: {
              formatter: "{value}"
            }
          },
          {
            type: "value",
            name: "易腐垃圾比例",
            min: 0,
            max: 100,
            interval: 20,
            axisLabel: {
              formatter: "{value} %"
            }
          }
        ],
        series: [
          {
            name: "垃圾总量",
            type: "bar",
            data: res.data.data.num
          },
          {
            name: "易腐垃圾比例",
            type: "line",
            yAxisIndex: 1,
            data: res.data.data.percent
          }
        ],
      };
        myChart.setOption(option);
      });
      
      
    },
    // DoGetData3() {
    //   // 基于准备好的dom，初始化echarts实例
    //   let myChart = echarts.init(document.getElementById("c3"), "tdTheme");
    //   // 绘制图表
    //   let option = {
    //     title: {
    //       text: "易腐垃圾统计",
    //       x: "center"
    //     },
    //     backgroundColor: "rgba(225, 225, 225, 0)", //rgba设置透明度0.1,
    //     tooltip: {
    //       trigger: "axis",
    //       axisPointer: {
    //         type: "cross",
    //         crossStyle: {
    //           color: "#000000"
    //         }
    //       }
    //     },
    //     toolbox: {
    //       feature: {
    //         magicType: { show: true, type: ["line", "bar"] },
    //         restore: { show: true }
    //       }
    //     },
    //     legend: {
    //       data: ["垃圾总量", "易腐垃圾比例"],
    //       orient: "horizontal",
    //       bottom: "5"
    //     },
    //     tooltip: {
    //       trigger: "axis",
    //       formatter: "{b0}<br/>{a0}: {c0}KG<br />{a1}：{c1}%"
    //     },

    //     grid: {
    //       left: "3%",
    //       right: "6%",
    //       bottom: "12%",
    //       containLabel: true
    //     },
    //     xAxis: [
    //       {
    //         type: "category",
    //         data: [
    //           "1月",
    //           "2月",
    //           "3月",
    //           "4月",
    //           "5月",
    //           "6月",
    //           "7月",
    //           "8月",
    //           "9月",
    //           "10月",
    //           "11月",
    //           "12月"
    //         ],
    //         axisPointer: {
    //           type: "shadow"
    //         }
    //       }
    //     ],
    //     yAxis: [
    //       {
    //         type: "value",
    //         name: "垃圾总量（KG）",
    //         min: 0,
    //         max: 1000,
    //         interval: 200,
    //         axisLabel: {
    //           formatter: "{value}"
    //         }
    //       },
    //       {
    //         type: "value",
    //         name: "易腐垃圾比例",
    //         min: 0,
    //         max: 100,
    //         interval: 20,
    //         axisLabel: {
    //           formatter: "{value} %"
    //         }
    //       }
    //     ],
    //     series: [
    //       {
    //         name: "垃圾总量",
    //         type: "bar",
    //         data: []
    //       },
    //       {
    //         name: "易腐垃圾比例",
    //         type: "line",
    //         yAxisIndex: 1,
    //         data: []
    //       }
    //     ]
    //   };
    //   getData3().then(res => {
    //     option.series[0].data = res.data.data.num;
    //     option.series[1].data = res.data.data.percent;

    //     myChart.setOption(option);
    //   });
    // },
    DogetData1() {
      getData1().then(res => {
        // console.log(res.data.data);
        this.cc=res.data.data[6];
        for (let i = 0; i < res.data.data.length-1; i++) {
          this.inforCardData2[i].count = res.data.data[i];
        }
        if(this.cc==1){
          this.inforCardData2.splice(4,1);
          this.inforCardData.splice(1,1);
        }
      });
    },
    DogetData6() {
      getData6().then(res => {
        //console.log(res);
        if (res.data.code == 200) {
          this.tabData = res.data.data;
        }
      });
    },
    DogetData4() {
      getData4().then(res => {
        ////console.log(res);
        if (res.data.code == 200) {
          let pmax = 1000;
          let arr = res.data.data.pnum.slice(0, 12);
          arr.sort();
          pmax=Math.max(...arr)
          //console.log("xxxxxxxxx");
          //console.log(res.data.data.pnum);
          // if (arr[11] < 10) {
          //   pmax = 50;
          // } else if (arr[11] < 100) {
          //   pmax = 150;
          // } else {
          //   pmax = pmax * 1.5;
          // }

          let myChart = echarts.init(document.getElementById("c4"), "tdTheme");
          let option = {
            title: {
              text: "居民垃圾分类参与率",
              x: "center"
            },
            backgroundColor: "rgba(225, 225, 225, 0)", //rgba设置透明度0.1,
            tooltip: {
              trigger: "axis",
              axisPointer: {
                type: "cross",
                crossStyle: {
                  color: "#000000"
                }
              }
            },
            toolbox: {
              feature: {
                magicType: { show: true, type: ["line", "bar"] },
                restore: { show: true }
              }
            },
            legend: {
              data: ["参与人数", "参与比例"],
              orient: "horizontal",
              bottom: "5"
            },
            tooltip: {
              trigger: "axis",
              formatter: "{b0}<br/>{a0}: {c0}<br />{a1}：{c1}%"
            },

            grid: {
              left: "3%",
              right: "6%",
              bottom: "12%",
              containLabel: true
            },
            xAxis: [
              {
                type: "category",
                data: [
                  "1月",
                  "2月",
                  "3月",
                  "4月",
                  "5月",
                  "6月",
                  "7月",
                  "8月",
                  "9月",
                  "10月",
                  "11月",
                  "12月"
                ],
                axisPointer: {
                  type: "shadow"
                }
              }
            ],
            yAxis: [
              {
                type: "value",
                name: "参与人数",
                min: 0,
                max: pmax,
                interval: pmax/5,
                axisLabel: {
                  formatter: "{value}"
                }
              },
              {
                type: "value",
                name: "参与比例",
                min: 0,
                max: 100,
                interval: 20,
                axisLabel: {
                  formatter: "{value} %"
                }
              }
            ],
            series: [
              {
                name: "参与人数",
                type: "bar",
                data: res.data.data.pnum,
              },
              {
                name: "参与比例",
                type: "line",
                yAxisIndex: 1,
                data: res.data.data.num,
              }
            ]
          };
          myChart.setOption(option);
        }
      });
    },
    DogetData7() {
      getData7().then(res => {
        //console.log(res);
        if (res.data.code == 200) {
          this.tabData1 = res.data.data;
        }
      });
    },
    DogetData8() {
      getData8().then(res => {
        //console.log(res);
        // for(i==0;i<res.data.data.address.length;i++){
        //   this.tabData2[i]=[res.data.data.loginName[i],res.data.data.address[i],res.data.data.percent[i]];
        // }
        // //console.log(this.tabData2);
        // if (res.data.code == 200) {
        //   this.tabData2 = res.data.data;
        // }
      });
    },
    DoGetData2() {
      getData2().then(res => {
        //console.log(res);
        if (res.data.code == 200) {
          //console.log(res.data.data);
          this.pieData[0].value = res.data.data[0].dd;
          this.pieData[1].value = res.data.data[0].aa;
          this.pieData[2].value = res.data.data[0].bb;
          this.pieData[3].value = res.data.data[0].cc;
          this.isShow = true;
          //console.log(this.pieData);
        }
      });
    },
    DoVisitorNum() {
      GetVisitorNum().then(res => {
        //console.log(res);
        if (res.data.code == 200) {
          this.inforCardData[1].count = res.data.data;
        }
      });
    }
  },
  mounted() {
    this.DoVisitorNum();
    this.DogetData1();
    this.DoGetData2();
    this.DoGetData3();
    this.DogetData4();

    this.DogetData6();
    this.DogetData7();
    this.DogetData8();
    getHPageData1().then(res => {
      //console.log(res);
      this.inforCardData[2].count = res.data.data.room;
      this.inforCardData[3].count = res.data.data.user;
      this.inforCardData[0].count = res.data.data.user2;
    });
    var _this = this;
            window.onresize = function(){ // 定义窗口大小变更通知事件
                _this.screenWidth = document.body.clientWidth; //窗口宽度
            };
  }
}
</script>

<style lang="less">
.count-style {
  font-size: 50px;
}

#tbsub {
  font-weight: 600;
  font-size: 13px;
  text-align: center;
  overflow-y: scroll;
  display: inline-block;
  height: 28rem;
  border: none;
  cursor: default;
}

#tbsub1 {
  font-weight: 600;
  font-size: 13px;
  text-align: center;
  overflow-y: scroll;
  display: inline-block;
  height: 12rem;
  width: 400px;
  border: none;
  cursor: default;
}

#tbsub tr,
#tbsub1 tr {
  border: none;
}

#tbsub td,
#tbsub1 td {
  height: 3rem;
  border: none;
}

#tbsub .subTr:hover,
#tbsub1 .subTr:hover {
  background-color: rgb(10, 255, 31);
}
</style>

<template>
  <div style="height: 100%">
    <div
      id="main"
      :style="btnStatue == false ? '' : btnStyle"
      style="height: 100%"
    >
      <div class="head" style="height: 8%">
        <h1>桐庐县垃圾分类</h1>
        <div class="fullScreen" @click="fullScreen">
          <div class="enter" v-if="screenStatue==false" style="text-align:right;">
            <Icon
              type="md-expand"
              size="20"
              style="
                position: absolute;
                top: 50%;
                left: 40px;
                transform: translateY(-50%);
              "
            />
            <span style="font-size: 16px; margin-left: 10px; font-weight: bold"
              >全屏</span
            >
          </div>
          <div class="exit" v-if="screenStatue==true" style="text-align:right;">
            <Icon
              type="md-contract"
              size="20"
              style="
                position: absolute;
                top: 50%;
                left: 0;
                transform: translateY(-50%);
              "
            />
            <span style="font-size: 16px; margin-left: 10px; font-weight: bold"
              >退出全屏</span
            >
          </div>
        </div>
      </div>
      <div class="mainbox" style="overflow: hidden; height: 92%">
        <!-- 左侧 -->
        <div style="float: left; width: 25%; height: 100%">
          <div style="height: 60%">
            <!-- 桐庐垃圾分类概况 -->
            <div style="height: 40%; padding: 10px 0">
              <div class="boxall" style="padding: 10px 0; height: 100%">
                <!--这里放内容-->
                <div
                  id="data1"
                  style="height: 100%; display: flex; flex-direction: column"
                >
                  <h3>桐庐县垃圾分类概况</h3>
                  <ul style="overflow: hidden">
                    <li>
                      参与社区：
                      <span class="gk_num" style="color: yellow">{{
                        data1.village
                      }}</span>
                    </li>
                    <li>
                      垃圾箱房：
                      <span class="gk_num" style="color: pink">{{
                        data1.GrabRoom
                      }}</span>
                    </li>
                    <li>
                      积分用户：
                      <span class="gk_num" style="color: deepskyblue">{{
                        data1.User
                      }}</span>
                    </li>
                    <li>
                      累计积分：
                      <span class="gk_num" style="color: yellow">{{
                        data1.AllScore
                      }}</span>
                    </li>
                    <li>
                      合作商店：
                      <span class="gk_num" style="color: pink">{{
                        data1.Shop
                      }}</span>
                    </li>
                    <li>
                      累计兑换积分：
                      <span class="gk_num" style="color: deepskyblue">{{
                        data1.UsedScore
                      }}</span>
                    </li>
                  </ul>
                </div>
              </div>
            </div>
            <!-- 各类垃圾占比 -->
            <div style="height: 60%; padding: 10px 0">
              <div class="boxall" style="padding: 10px; height: 100%">
                <!--这里放内容-->
                <div id="data2" style="width: 100%; height: 100%">
                  <div id="c2" style="width: 100%; height: 100%"></div>
                </div>
              </div>
            </div>
          </div>
          <!-- 易腐垃圾比例 -->
          <div style="height: 40%; padding: 10px 0">
            <div class="boxall" style="padding: 10px; height: 100%">
              <!--这里放内容-->
              <div id="c3" style="height: 100%"></div>
            </div>
          </div>
        </div>
        <!-- 中间 -->
        <div style="float: left; width: 50%; height: 100%">
          <!-- 地图 -->
          <div style="height: 60%; padding: 10px 0">
            <div class="boxall" style="height: 100%">
              <!--这里放内容-->
              <baidu-map
                id="mmm"
                class="map"
                center="桐庐"
                :zoom="15"
                :scroll-wheel-zoom="true"
                @ready="mapReady"
              >
                          <bm-scale anchor="BMAP_ANCHOR_TOP_RIGHT"></bm-scale>
                          
        <bml-marker-clusterer :averageCenter="true">
      <bm-marker
        v-for="(marker,index) of mm"
        :key="marker.code"
        :position="{lng: marker.lng, lat: marker.lat}"
        :dragging="false"
         @click="lookDetail(marker)"
        :icon='{url:require("../../assets/icons/mapicon"+marker.type+".png"),size:{width:40,height:40},opts:{imageSize:{width:40,height:40}}}'
      ></bm-marker>
      <bm-marker
      v-for="(marker,index) of roominfo"
      :key="marker.id"
      :position="{lng: marker.lon, lat: marker.lat}"
      :dragging="false"
       @click="lookDetail2(marker)"
      :icon='{url:require("../../assets/icons/"+marker.iconPath),size:{width:40,height:35},opts:{imageSize:{width:40,height:35}}}'
    >
      </bm-marker>
        </bml-marker-clusterer>
       <!--信息窗体-->
      <bm-info-window
        :position="{lng: infoWindow.info.lng, lat: infoWindow.info.lat}"
        :title="infoWindow.info.name"
        :show="infoWindow.show"
        @close="infoWindowClose"
        @open="infoWindowOpen"
        :offset="{width:0, height:-20}"
      >
      </bm-info-window>
      <bm-info-window
        :position="{lng: infoWindow2.info.lon, lat: infoWindow2.info.lat}"
        :title="infoWindow2.info.ljname"
        :show="infoWindow2.show"
        @close="infoWindowClose2"
        @open="infoWindowOpen2"
        :offset="{width:0, height:-20}"
      >
      </bm-info-window>
              </baidu-map>
            </div>
          </div>
          <div style="height: 40%; padding: 10px 0">
            <!-- 居民垃圾分类参与率 -->
            <div style="width: 50%; float: left; height: 100%">
              <div class="boxall" style="padding: 10px; height: 100%">
                <!--这里放内容-->
                <div id="c4" style="height: 100%"></div>
              </div>
            </div>
            <!-- 本月志愿者积分排行榜 -->
            <div style="width: 50%; float: left; height: 100%">
              <div class="boxall" style="height: 100%; padding: 10px 0">
                <!--这里放内容-->
                <div
                  id="data5"
                  style="
                    height: 100%;
                    padding: 10px 0;
                    overflow: hidden;
                    display: flex;
                    flex-direction: column;
                  "
                >
                  <h3>本月志愿者积分排行榜</h3>
                  <table
                    id="tbsub1"
                    border
                    rules="none"
                    cellspacing="0"
                    align="center"
                  >
                    <tr>
                      <td>序号</td>
                      <td>微信名</td>
                      <td>积分</td>
                    </tr>
                    <tr
                      class="subTr"
                      v-if="tabData1 === '' || tabData1 === undefined"
                    >
                      <td colspan="5">暂无数据</td>
                    </tr>
                    <tr class="subTr" v-else v-for="(it, i) in tabData1" :key="i">
                      <td>{{ i + 1 }}</td>
                      <td>{{ it.loginName }}</td>
                      <td>
                        <span v-if="it.zyzsc !== 'NaN'">{{ it.zyzsc }}</span>
                        <span v-else>0</span>
                      </td>
                    </tr>
                  </table>
                </div>
              </div>
            </div>
          </div>
        </div>
        <!-- 右侧 -->
        <div style="float: left; width: 25%; height: 100%">
          <!-- 本月各社区易腐垃圾比例排行榜 -->
          <div style="height: 60%; padding: 10px 0">
            <div class="boxall" style="height: 100%">
              <!--这里放内容-->
              <div
                id="data5"
                style="
                  height: 100%;
                  padding: 10px 0;
                  overflow: hidden;
                  display: flex;
                  flex-direction: column;
                "
              >
                <h3>本月各社区易腐垃圾比例排行榜</h3>
                <table
                  id="tbsub"
                  border
                  rules="none"
                  cellspacing="0"
                  align="center"
                >
                  <tr>
                    <td>序号</td>
                    <td>社区</td>
                    <td>易腐垃圾</td>
                    <td>其他垃圾</td>
                    <td>易腐垃圾比例</td>
                  </tr>
                  <tr
                    class="subTr"
                    v-if="tabData === '' || tabData === undefined"
                  >
                    <td colspan="5">暂无数据</td>
                  </tr>
                  <tr class="subTr" v-else v-for="(it, i) in tabData" :key="i">
                    <td>{{ i + 1 }}</td>
                    <td>{{ it.vname }}</td>
                    <td>{{ it.c1 }}</td>
                    <td>{{ it.c2 }}</td>
                    <td>
                      <span v-if="it.percent !== 'NaN'">{{ it.percent }}</span>
                      <span v-else>0%</span>
                    </td>
                  </tr>
                </table>
              </div>
            </div>
          </div>
          <!-- 本月家庭积分排行榜 -->
          <div style="height: 40%; padding: 10px 0">
            <div class="boxall" style="height: 100%">
              <!--这里放内容-->
              <div
                id="data5"
                style="
                  height: 100%;
                  padding: 10px 0;
                  overflow: hidden;
                  display: flex;
                  flex-direction: column;
                "
              >
                <h3>本月家庭积分排行榜</h3>
                <table
                  id="tbsub1"
                  border
                  rules="none"
                  cellspacing="0"
                  align="center"
                >
                  <tr>
                    <td>序号</td>
                    <!-- <td>微信名</td> -->
                    <td>家庭地址</td>
                    <td>积分</td>
                  </tr>
                  <tr
                    class="subTr"
                    v-if="tabData2 === '' || tabData2 === undefined"
                  >
                    <td colspan="5">暂无数据</td>
                  </tr>
                  <tr class="subTr" v-else v-for="(it, i) in tabData2" :key="i">
                    <td>{{ i + 1 }}</td>
                    <!-- <td>{{it[0]}}</td> -->
                    <td>{{ it[1] }}</td>
                    <td>
                      <span v-if="it[2] !== 'NaN'">{{ it[2] }}</span>
                      <span v-else>0</span>
                    </td>
                  </tr>
                </table>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
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
import { BmlMarkerClusterer } from "vue-baidu-map";

import {
  getData1,
  getData4,
  getData2,
  getData3,
  getData6,
  getData7,
  getData8,
  mapgetData1
} from "../../api/GiMap/DataBord";
export default {
  name: "dataBord",
  components: {
    BmlMarkerClusterer,
  },
  data() {
    return {
      mm: [],
      clock:{},
      roominfo: [],
      activeName: "",
      infoWindow: {
        lng: 0,
        lat: 0,
        show: false,
        info: {
          village: "",
          street: 12313,
          areaEnergy: 0.64,
          code: "440300A055",
          energy: 7922.66,
          lat: "32.043433",
          lng: "118.784107",
          name: "xxxxxx",
          water: 0
        }
      },
      infoWindow2: {
        lng: 0,
        lat: 0,
        show: false,
        info: {
          village: "",
          street: 12313,
          areaEnergy: 0.64,
          code: "440300A055",
          energy: 7922.66,
          lat: "32.043433",
          lng: "118.784107",
          name: "xxxxxx",
          water: 0
        }
      },
      //分类概况
      data1: {
        Village: 0,
        GrabRoom: 0,
        User: 0,
        AllScore: 0,
        Shop: 0,
        UsedScore: 0,
      },
      //本月政府
      tabData: "",
      tabData1: "",
      tabData2: [],
      ljmax: 0,
      ljfenge: 0,
      //本月社区
      mapData: {},
      btnStatue: false,
      btnStyle:
        "position: fixed;top: 0;left:0;width: 100%;height: 100%;z-index: 999;",
      screenStatue:false,
    };
  },
  mounted() {
    this.loadMarker();
    // this.clock=setInterval(()=>this.loadMarker(),5000);
    this.DoGetData3();
    this.DogetData1();
    this.DogetData4();
    this.DoGetData2();
    this.DogetData6();
    this.DogetData7();
    this.DogetData8();
   window.onresize = () => {
      console.log(123);
      // 基于准备好的dom，初始化echarts实例
      let myChart2 = echarts.init(document.getElementById("c2"));
      myChart2.resize();
      // 基于准备好的dom，初始化echarts实例
      let myChart3 = echarts.init(document.getElementById("c3"));
      myChart3.resize();
      // 基于准备好的dom，初始化echarts实例
      let myChart4 = echarts.init(document.getElementById("c4"));
      myChart4.resize();
    };
    //this.baiduMap();//渲染地图

  },
  beforeDestroy() {
    //console.log("页面切换");
    clearInterval(this.clock);
  },
  methods: {
    // 一键全屏
    handleFullScreen() {
      let element = document.documentElement;
      if (element.requestFullscreen) {
        element.requestFullscreen();
      } else if (element.webkitRequestFullScreen) {
        element.webkitRequestFullScreen();
      } else if (element.mozRequestFullScreen) {
        element.mozRequestFullScreen();
      } else if (element.msRequestFullscreen) {
        // IE11
        element.msRequestFullscreen();
      }
      // this.index++;
    },
    // 一键退出
    exitFullscreen() {
      if (document.exitFullscreen) {
        document.exitFullscreen();
      } else if (document.webkitCancelFullScreen) {
        document.webkitCancelFullScreen();
      } else if (document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
      } else if (document.msExitFullscreen) {
        document.msExitFullscreen();
      }
      // this.index--;
    },
    // 全屏按钮点击事件
    fullScreen() {
      if (this.btnStatue == false) {
        this.btnStatue = true;
        this.handleFullScreen();
        this.screenStatue=true
      } else {
        this.btnStatue = false;
        this.exitFullscreen();
        this.screenStatue=false
      }
      // this.$router.go(0)
    },
    infoWindowClose(e) {
      this.infoWindow.show = false;
    },
    infoWindowClose2(e) {
      this.infoWindow2.show = false;
    },
    infoWindowOpen(e) {
      this.infoWindow.show = true;
    },
    infoWindowOpen2(e) {
      this.infoWindow2.show = true;
    },
    setIcon() {
      return {
        url: require("../../assets/icons/mapicon2.png"),
        size: { width: 20, height: 20 },
        opts: { imageSize: { width: 20, height: 20 } }
      };
    },
    lookDetail(data) {
      this.infoWindow.show = true;
      this.infoWindow.info = data;
      this.activeName = data.name;
      // let This=this;
      //为弹窗口标题添加title
      // this.$nextTick(() => {
      //   var win = document.querySelector(".BMap_bubble_title");
      //   win.title = this.activeName;
      // })
    },
    lookDetail2(data) {
      this.infoWindow2.show = true;
      this.infoWindow2.info = data;
      this.activeName = data.ljname;
      // let This=this;
      //为弹窗口标题添加title
      // this.$nextTick(() => {
      //   var win = document.querySelector(".BMap_bubble_title");
      //   win.title = this.activeName;
      // })
    },
    loadMarker() {
      mapgetData1().then(res => {
        this.mm=[];
        //console.log(this.mm);
        this.roominfo=[];
        //console.log(res);
        if (res.data.code == 200) {
          this.mm = res.data.data.dat;
          for (let i = 0; i < res.data.data.rooms.length; i++) {
            //console.log(3333);
            if (
              i != 0 &&
              res.data.data.rooms[i].garbageRoomUuid ==
                res.data.data.rooms[i - 1].garbageRoomUuid
            ) {
              let index = this.roominfo.findIndex(
                x => x.garbageRoomUuid == res.data.data.rooms[i].garbageRoomUuid
              );
              //console.log(index);
              if (res.data.data.rooms[i].type == "其他垃圾") {
                this.roominfo[index].otherWaste = true;
              } else if (res.data.data.rooms[i].type == "易腐垃圾") {
                this.roominfo[index].perishable = true;
              } else {
              }
            } else {
              //console.log(4444);
              let room = {
                id:res.data.data.rooms[i].id,
                garbageRoomUuid: res.data.data.rooms[i].garbageRoomUuid,
                ljname: res.data.data.rooms[i].ljname,
                vname: res.data.data.rooms[i].vname,
                towns: res.data.data.rooms[i].towns,
                lon: res.data.data.rooms[i].lon,
                lat: res.data.data.rooms[i].lat,
                otherWaste: false,
                perishable: false,
                iconPath:'',
              };
              //console.log(room);
              if (res.data.data.rooms[i].type == "其他垃圾") {
                room.otherWaste = true;
              } else if (res.data.data.rooms[i].type == "易腐垃圾") {
                room.perishable = true;
              } else {
              }
              if (room.otherWaste && room.perishable) {
					      room.iconPath='aa_1.png';
				      }else if(!room.otherWaste && room.perishable){
					      room.iconPath='ab_1.png';
				      }else if(!room.otherWaste && !room.perishable){
					      room.iconPath='bb_1.png';
				      }else if(room.otherWaste && !room.perishable){
					      room.iconPath='ba_1.png';
				      }else{
					
				      }
              if (
                room.lon != null &&
                room.lat != null &&
                room.lon != "" &&
                room.lon != ""
              ) {
                this.roominfo.push(room);
                //console.log(this.roominfo);
              }
            }
          }
        }
      });
    },
    async DoGetData3() {
      getData3().then((res) => {
        let max = res.data.data.num[0];
        res.data.data.num.forEach((item) => (max = item > max ? item : max));
        let fenge = parseFloat(max / 5).toFixed(2);
        //console.log(fenge);
        this.ljfenge = parseFloat(fenge);
        this.ljmax = this.ljfenge * 5;
        // 基于准备好的dom，初始化echarts实例
        let myChart = echarts.init(document.getElementById("c3"), "dark");
        // 绘制图表
        let option = {
          backgroundColor: "rgba(128, 128, 128, 0.1)", //rgba设置透明度0.1,
          tooltip: {
            trigger: "axis",
            axisPointer: {
              type: "cross",
              crossStyle: {
                color: "#fff",
              },
            },
          },
          toolbox: {
            feature: {
              magicType: { show: true, type: ["line", "bar"] },
              restore: { show: true },
            },
          },
          legend: {
            data: ["垃圾总量", "易腐垃圾比例"],
          },
          tooltip: {
            trigger: "axis",
            formatter: "{b0}<br/>{a0}: {c0}吨<br />{a1}：{c1}%",
          },

          grid: {
            left: "4.5%",
            right: "4%",
            bottom: "3%",
            containLabel: true,
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
                "12月",
              ],
              axisPointer: {
                type: "shadow",
              },
            },
          ],
          yAxis: [
            {
              type: "value",
              name: "垃圾总量（吨）",
              min: 0,
              max: this.ljmax,
              interval: this.ljfenge,
              axisLabel: {
                formatter: "{value}",
              },
            },
            {
              type: "value",
              name: "易腐垃圾比例",
              min: 0,
              max: 100,
              interval: 20,
              axisLabel: {
                formatter: "{value} %",
              },
            },
          ],
          series: [
            {
              name: "垃圾总量",
              type: "bar",
              data: res.data.data.num,
            },
            {
              name: "易腐垃圾比例",
              type: "line",
              yAxisIndex: 1,
              data: res.data.data.percent,
            },
          ],
        };
        myChart.setOption(option);
      });
    },
    DogetData1() {
      getData1().then((res) => {
        this.data1.village = res.data.data[0];
        this.data1.GrabRoom = res.data.data[1];
        this.data1.User = res.data.data[2];
        this.data1.AllScore = res.data.data[3];
        this.data1.Shop = res.data.data[4];
        this.data1.UsedScore = res.data.data[5];
      });
    },
    DogetData6() {
      getData6().then((res) => {
        //console.log(res);
        if (res.data.code == 200) {
          this.tabData = res.data.data;
        }
      });
    },
    DogetData4() {
      getData4().then((res) => {
        ////console.log(res);
        if (res.data.code == 200) {
          let myChart = echarts.init(document.getElementById("c4"), "dark");
          let option = {
            backgroundColor: "rgba(128, 128, 128, 0.1)", //rgba设置透明度0.1,
            title: {
              text: "居民垃圾分类参与率",
            },
            tooltip: {
              trigger: "axis",
              formatter: "{b0}<br/>{a0}: {c0}%",
            },
            legend: {
              data: ["比例"],
            },
            grid: {
              left: "3%",
              right: "4%",
              bottom: "3%",
              containLabel: true,
            },
            xAxis: {
              type: "category",
              boundaryGap: false,
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
                "12月",
              ],
            },
            yAxis: {
              type: "value",
              min: 0,
              max: 100,
              interval: 20,
              axisLabel: {
                show: true,
                interval: "auto",
                formatter: "{value}%",
              },
              show: true,
            },
            series: [
              {
                name: "比例",
                type: "line",
                data: res.data.data.num,
              },
            ],
          };
          myChart.setOption(option);
        }
      });
    },
    DogetData7() {
      getData7().then((res) => {
        //console.log(res);
        if (res.data.code == 200) {
          this.tabData1 = res.data.data;
        }
      });
    },
    DogetData8() {
      getData8().then((res) => {
        this.tabData2=[];
        console.log(res);
        if (res.data.code == 200) {
          for (let i = 0; i < res.data.data.address.length; i++) {
            this.tabData2[i] = [
              res.data.data.loginName[i],
              res.data.data.address[i],
              res.data.data.percent[i],
            ];
          }
          //console.log(this.tabData2);
        }
      });
    },
    // baiduMap(){
    mapReady(e) {
      // var map = new e.BMap.Map("mmm");
      var map = e.map;
      var cityName = "桐庐";
      map.centerAndZoom(cityName, 12); // 初始化地图,设置中心点坐标和地图级别。    map.addControl(new BMap.ScaleControl());                    // 添加比例尺控件
      map.enableScrollWheelZoom();
      var bdary = new e.BMap.Boundary();
      var mapStyle = {
        style: "light",
      };
      map.setMapStyle(mapStyle);
      bdary.get(cityName, function (rs) {
        //获取行政区域
        // map.clearOverlays(); //清除地图覆盖物
        //网上查了下，东西经南北纬的范围
        var EN_JW = "180, 90;"; //东北角
        var NW_JW = "-180,  90;"; //西北角
        var WS_JW = "-180, -90;"; //西南角
        var SE_JW = "180, -90;"; //东南角
        //4.添加环形遮罩层
        var ply1 = new e.BMap.Polygon(
          rs.boundaries[0] + SE_JW + SE_JW + WS_JW + NW_JW + EN_JW + SE_JW,
          {
            strokeColor: "none",
            fillColor: "#2F3D54", //修改不想显示的地图颜色
            fillOpacity: 0.9,
            strokeOpacity: 0.5,
          }
        ); //建立多边形覆盖物

        map.addOverlay(ply1);
        //5. 给目标行政区划添加边框，其实就是给目标行政区划添加一个没有填充物的遮罩层
        var ply = new e.BMap.Polygon(rs.boundaries[0], {
          strokeWeight: 2,
          strokeColor: "#00f",
          fillColor: "#104da7", //显示的地图区域颜色设置
          fillOpacity: 0.2,
        });
        map.addOverlay(ply);
        //map.setViewport(ply.getPath());    //调整视野
      });
      // }
    },
    DoGetData2() {
      getData2().then((res) => {
        if (res.data.code == 200) {
          //console.log(res.data.data);
          let myChart = echarts.init(document.getElementById("c2"), "dark");
          let option = {
            backgroundColor: "rgba(128, 128, 128, 0.1)", //rgba设置透明度0.1,
            title: { text: "各类垃圾占比" },
            tooltip: {
              trigger: "item",
              formatter: "{b}: {c}吨 ({d}%)",
            },
            legend: {
              orient: "vertical",
              left: 10,
              top: 40,
              data: ["其他垃圾", "易腐垃圾", "可回收垃圾", "有害垃圾"],
            },
            series: [
              {
                type: "pie",
                radius: ["50%", "70%"],
                avoidLabelOverlap: false,
                label: {
                  show: false,
                  position: "center",
                },
                emphasis: {
                  label: {
                    show: true,
                    fontSize: "20",
                    fontWeight: "bold",
                  },
                },
                labelLine: {
                  show: false,
                },
                data: [
                  { value: res.data.data[0].dd, name: "其他垃圾" },
                  { value: res.data.data[0].aa, name: "易腐垃圾" },
                  { value: res.data.data[0].bb, name: "可回收垃圾" },
                  { value: res.data.data[0].cc, name: "有害垃圾" },
                ],
              },
            ],
          };
          myChart.setOption(option);
        }
      });
    },
  },
};
</script>

<style scoped>
@import "comon0.css";

.map2 {
  height: 100%;
}

.showVideo {
  width: 10rem;
  border: none;
  background-color: orange;
  color: white;
  height: 2rem;
  margin-left: 2rem;
}
.head {
  position: relative;
}
.fullScreen {
  position: absolute;
  top: 50%;
  right: 2%;
  transform: translateY(-50%);
  color: #ffffff;
  /* background-color: pink; */
  width: 100px;
  height: 30px;
  line-height: 30px;
  text-align: center;
  cursor: pointer;
  user-select: none;
}
.fullScreenBtn {
  font-style: normal;
  font-size: 18px;
  font-family: "icomoon";
}
.boxall {
  /* width: 27rem; */
  /* height: 15rem; */
  margin: 0 20px;
  /* margin-bottom: 20px; */
  color: white;
}

.boxall h3 {
  font-size: 1.5rem;
  padding-left: 0.9rem;
}
#data1 ul {
  flex: 1;
}
#data1 li {
  display: block;
  float: left;
  width: 50%;
  font-size: 1rem;
  height: 33.33%;
  font-weight: 500;
}

.gk_num {
  font-size: 1.5rem;
}

table {
  width: 100%;
}

td {
  padding: 0px;
  margin: 0px;
  width: 20rem;
}

.map {
  width: 100%;
  height: 100%;
}

#tbsub {
  font-weight: 600;
  font-size: 13px;
  text-align: center;
  overflow-y: scroll;
  display: inline-block;
  flex: 1;
  border: none;
  cursor: default;
}

#tbsub1 {
  font-weight: 600;
  font-size: 13px;
  text-align: center;
  overflow-y: scroll;
  display: inline-block;
  flex: 1;
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
  background-color: rgb(0, 0, 0, 0.4);
}
</style>

<template>
  <div>
    <Card>
      <div style="height: 60px;line-height:60px;">
        <Row style="width: 100%">
          <template>
            <div style="float: left">
              <Select
                v-model="formModel.carlist.query.carUuid"
                placeholder="请选择车辆"
                style="width: 150px; margin-right: 10px"
              >
                <Option
                  v-for="item in carlist"
                  :value="item.carUuid"
                  :key="item.carUuid"
                  >{{ item.carNum }}</Option
                >
              </Select>
              <DatePicker
                v-model="formModel.carlist.query.kw1"
                format="yyyy-MM-dd"
                type="date"
                placeholder="选择日期"
                style="width: 200px; margin-right: 10px"
              ></DatePicker>
              <!-- <TimePicker
                v-model="formModel.carlist.query.kw2"
                format="HH’mm"
                type="timerange"
                placement="bottom-end"
                placeholder="选择时间"
                style="width: 168px; margin-right: 10px"
              ></TimePicker> -->
              <label>调速：</label>
            </div>
            <div>
              <Slider
                :v-model="speed"
                :tip-format="sliders"
                :min="100"
                :max="1000"
                style="width: 200px; float: left; margin-right: 10px;margin-top: 10px;"
              ></Slider>
            </div>
            <Button type="primary" @click="handleDetail3()" style="margin-right: 10px;">查询</Button>
            <Button type="primary" @click="doplay(1)" style="margin-right: 10px;">播放</Button>
            <Button type="primary" @click="doplay(2)">暂停</Button>
            <!-- <Button type="primary" @click="doplay(3)">停止</Button> -->
          </template>
        </Row>
      </div>
      <baidu-map
        class="map"
        center="桐庐"
        :zoom="15"
        @ready="mapOnly"
        :scroll-wheel-zoom="true"
      >
        <!--设置经纬度，缩放等级，滚轮缩放 -->
        <!-- 在右上角加入比例尺-->
        <bm-scale anchor="BMAP_ANCHOR_TOP_RIGHT"></bm-scale>
        <!-- 在右上角加入缩放控件 -->
        <bm-navigation anchor="BMAP_ANCHOR_TOP_RIGHT"></bm-navigation>
        <!-- 在左上角加入地图类型控件 -->
        <bm-map-type
          :map-types="['BMAP_NORMAL_MAP', 'BMAP_HYBRID_MAP']"
          anchor="BMAP_ANCHOR_TOP_LEFT"
        ></bm-map-type>
        <bml-lushu
          @stop="reset"
          :path="points"
          :icon="icon"
          :play="play"
          :speed="speed"
          :rotation="true"
        >
        </bml-lushu>
        <bm-scale anchor="BMAP_ANCHOR_TOP_RIGHT"></bm-scale>
          <bm-marker
            v-for="(marker, index) of carinfo"
            :position="{ lng: marker.lon, lat: marker.lat }"
            :dragging="false"
            :icon="{
              url: require('../../assets/images/'+marker.iconPath),
              size: { width: 80, height: 60 },
              opts: { imageSize: { width: 80, height: 60 } },
            }"
          >
          </bm-marker>
        <bml-marker-clusterer :averageCenter="true">
          <!-- <bm-marker
          v-for="(marker, index) of mm"
          :key="marker.code"
          :position="{ lng: marker.lng, lat: marker.lat }"
          :dragging="false"
          @click="lookDetail(marker)"
          :icon="{
            url: require('../../assets/icons/mapicon' + marker.type + '.png'),
            size: { width: 40, height: 40 },
            opts: { imageSize: { width: 40, height: 40 } },
          }"
        ></bm-marker> -->
          <bm-marker
            v-for="(marker, index) of roominfo"
            :key="marker.id"
            :position="{ lng: marker.lon, lat: marker.lat }"
            :dragging="false"
            @click="lookDetail2(marker)"
            :icon="{
              url: require('../../assets/icons/' + marker.iconPath),
              size: { width: 40, height: 35 },
              opts: { imageSize: { width: 40, height: 35 } },
            }"
          >
          </bm-marker>
        </bml-marker-clusterer>
        <!--信息窗体-->
        <bm-info-window
          :position="{ lng: infoWindow.info.lng, lat: infoWindow.info.lat }"
          :title="infoWindow.info.name"
          :show="infoWindow.show"
          @close="infoWindowClose"
          @open="infoWindowOpen"
          :offset="{ width: 0, height: -20 }"
        >
          <button class="showVideo" @click="showVideo">实时查看</button>
        </bm-info-window>
        <bm-info-window
          :position="{ lng: infoWindow2.info.lon, lat: infoWindow2.info.lat }"
          :title="infoWindow2.info.ljname"
          :show="infoWindow2.show"
          @close="infoWindowClose2"
          @open="infoWindowOpen2"
          :offset="{ width: 0, height: -20 }"
        >
          <button class="showVideo" @click="showVideo">实时查看</button>
        </bm-info-window>
      </baidu-map>
    </Card>
  </div>
</template>

<script>
import { BmlLushu, BmlMarkerClusterer } from "vue-baidu-map";
import { getData1 } from "../../api/GiMap/map";
import { timeGetTravel, GetCarLists,GetCarSite } from "@/api/grab/travel";

export default {
  name: "Map",
  components: {
    BmlLushu,
    BmlMarkerClusterer,
  },
  data() {
    return {
      mm: [],
      roominfo: [],
      carinfo: [],
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
          water: 0,
        },
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
          water: 0,
        },
      },

      icon: {
        url: "http://api.map.baidu.com/library/LuShu/1.2/examples/car.png",
        size: { width: 52, height: 26 },
        opts: { anchor: { width: 27, height: 13 } },
      },

      map2: {},
      BMap2: {},
      speed: 100,
      points: [],
      play: false,
      carlist: [],
      formModel: {
        carlist: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isDelete: 0,
            status: -1,
            kw: "",
            kw1: "",
            kw2: [],
            carUuid: "",
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {},
          data: [],
        },
      },
      clock: {},
      clock2: {},
    };
  },
  methods: {
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
    mapOnly(e) {
      this.BMap2 = e.BMap;
      this.map2 = e.map;
    },
    setIcon() {
      return {
        url: require("../../assets/icons/mapicon2.png"),
        size: { width: 20, height: 20 },
        opts: { imageSize: { width: 20, height: 20 } },
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
    showVideo() {
      alert("等待开发");
    },
    loadMarker() {
      getData1().then((res) => {
        this.mm = [];
        //console.log(this.mm);
        this.roominfo = [];
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
                (x) =>
                  x.garbageRoomUuid == res.data.data.rooms[i].garbageRoomUuid
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
                id: res.data.data.rooms[i].id,
                garbageRoomUuid: res.data.data.rooms[i].garbageRoomUuid,
                ljname: res.data.data.rooms[i].ljname,
                vname: res.data.data.rooms[i].vname,
                towns: res.data.data.rooms[i].towns,
                lon: res.data.data.rooms[i].lon,
                lat: res.data.data.rooms[i].lat,
                otherWaste: false,
                perishable: false,
                iconPath: "",
              };
              //console.log(room);
              if (res.data.data.rooms[i].type == "其他垃圾") {
                room.otherWaste = true;
              } else if (res.data.data.rooms[i].type == "易腐垃圾") {
                room.perishable = true;
              } else {
              }
              if (room.otherWaste && room.perishable) {
                room.iconPath = "aa_1.png";
              } else if (!room.otherWaste && room.perishable) {
                room.iconPath = "ab_1.png";
              } else if (!room.otherWaste && !room.perishable) {
                room.iconPath = "bb_1.png";
              } else if (room.otherWaste && !room.perishable) {
                room.iconPath = "ba_1.png";
              } else {
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
    doGetCarList() {
      GetCarLists().then((res) => {
        this.carlist = res.data.data;
      });
    },
    doGetCarSite(){
      GetCarSite().then((res) => {
        console.log(res);
        var carsite=res.data.data;
        if(carsite.length>0){
          var iconPath="";
          for(var i=0;i<carsite.length;i++){
            if(carsite[i].grabtype!=""){
              if(carsite[i].grabtype=="0"){
                iconPath='qtlj.png';
              }else if(carsite[i].grabtype=="1"){
                 iconPath='yflj.png';
              }else if(carsite[i].grabtype=="2"){
                 iconPath='yhlj.png';
              }else if(carsite[i].grabtype=="3"){
                 iconPath='khslj.png';
              }
              let carsitemark={
                lon:carsite[i].lon,
                lat:carsite[i].lat,
                iconPath:iconPath
              };
              this.carinfo.push(carsitemark);
            }
          }
        }
      });
    },
    completeDate(value) {
        return value < 10 ? "0"+value:value;
    },
    handleDetail3() {
      if(this.formModel.carlist.query.carUuid==""){
        this.$Message.warning("请选择车辆");
        return;
      }
      if(this.points.length>0){
        this.map2.clearOverlays();
      }
      this.points=[];
      let ss = this.formModel.carlist.query;
      if (ss.kw1 != "" ) {
        timeGetTravel(this.formModel.carlist.query).then((res) => {
          //console.log(res);
          for (let i = 0; i < res.data.data.length; i++) {
            if (
              res.data.data[i].lat != null &&
              res.data.data[i].lat != "" &&
              res.data.data[i].lon != null &&
              res.data.data[i].lon != ""
            ) {
              let point = new this.BMap2.Point(
                res.data.data[i].lon,
                res.data.data[i].lat
              );
              this.points.push(point);
              // let myIcon=new this.BMap2.Icon("http://api.map.baidu.com/library/LuShu/1.2/examples/car.png",new this.BMap2.Size(50,50));
              //     let marker = new this.BMap2.Marker(
              //       new this.BMap2.Point(res.data.data[0].lon, res.data.data[0].lat),
              //       {icon:myIcon}
              //     );
              //     this.map2.addOverlay(marker);
              // let marker = new this.BMap2.Marker(
              //   new this.BMap2.Point(res.data.data[i].lon, res.data.data[i].lat)
              // );
              // console.log(marker);
              // this.map2.addOverlay(marker);
            }
          }
          var polyline = new this.BMap2.Polyline(this.points, {
            strokeColor: "blue",
            strokeWeight: 5,
            strokeOpacity: 0.5,
          }); //创建折线
          this.map2.addOverlay(polyline);
          this.map2.setViewport(this.points);
        });
      } else {
        this.$Message.warning("请选择时间段");
      }
    },
    reset() {
      this.play = false;
    },
    sliders(val) {
      this.speed = val;
      return "速度：" + val;
    },
    doplay(val) {
      switch (val) {
        case 1:
          this.play = true;
          break;
        case 2:
          this.play = false;
          break;
        case 3:
          false;
          break;
      }
    },
  },
  mounted() {
     var d = new Date();
      var date = d.getFullYear() + "-" +this.completeDate(d.getMonth() + 1) + "-" + this.completeDate(d.getDate());
      this.formModel.carlist.query.kw1=date;
    this.loadMarker();
    this.doGetCarSite();
    this.doGetCarList();
    this.clock = setInterval(() => this.loadMarker(), 30000000);
    this.clock2 = setInterval(() => this.doGetCarSite(), 1200000);
  },
  beforeDestroy() {
    //console.log("页面切换");
    clearInterval(this.clock);
    clearInterval(this.clock2);
  },
};
</script>

<style scoped>
.map {
  height: 720px;
}

.showVideo {
  width: 10rem;
  border: none;
  background-color: orange;
  color: white;
  height: 2rem;
  margin-left: 2rem;
}
</style>

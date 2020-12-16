<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.grab.query.totalCount"
        :pageSize="stores.grab.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      style="width: 150px"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.grab.query.kw"
                      placeholder="请输入车牌号码"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <!-- <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>-->

                  <Button
                    icon="md-refresh"
                    title="刷新"
                    @click="handleRefresh"
                  ></Button>
                </ButtonGroup>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.grab.data"
          :columns="stores.grab.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <template slot-scope="{ row, index }" slot="auditState">
            <span>{{ renderAuditState(row.auditState) }}</span>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <Tooltip
              placement="top"
              content="历史轨迹"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'show1'"
                type="success"
                size="small"
                shape="circle"
                icon="ios-cloud-upload"
                @click="handleDetail(row)"
                >历史轨迹</Button
              >
            </Tooltip>
            <!-- <Tooltip
              placement="top"
              content="实时查看"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'show2'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail2()"
                >实时查看</Button
              >
            </Tooltip> -->
          </template>
        </Table>
      </dz-table>
    </Card>

    <Drawer
      title="历史轨迹"
      v-model="formModel.opened"
      width="1000"
      :mask-closable="false"
      :mask="false"
    >
      <dz-table class="carlist">
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="formModel.carlist.data"
          :columns="formModel.carlist.columns"
          :row-class-name="rowClsRender"
        ></Table>
      </dz-table>

      <!-- <dz-table
        :totalCount="stores2.ljlist.query.totalCount"
        :pageSize="stores2.ljlist.query.pageSize"
        @on-page-change="handlePageChanged2"
        @on-page-size-change="handlePageSizeChanged2"
      >
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores2.ljlist.data"
          :columns="stores2.ljlist.columns"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged2"
          @on-page-size-change="handlePageSizeChanged2"
          @on-sort-change="handleSortChange2"
        ></Table>
      </dz-table> -->
      <Form
        :model="formModel.fields"
        ref="formCarDispatchDetail"
        label-position="left"
      >
        <baidu-map
          v-if="btnClick"
          class="map"
          center="桐庐"
          :zoom="15"
          :scroll-wheel-zoom="true"
          @ready="mapOnly"
          style="height: 700px"
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
        </baidu-map>
        <br />
        <Row>
          <template>
            <div style="float: left">
              <DatePicker
                v-model="formModel.carlist.query.kw1"
                format="yyyy-MM-dd"
                type="date"
                placeholder="选择日期"
                style="width: 200px"
              ></DatePicker>
              <!-- <TimePicker
                v-model="formModel.carlist.query.kw2"
                format="HH’mm"
                type="timerange"
                placement="bottom-end"
                placeholder="选择时间"
                style="width: 168px"
              ></TimePicker> -->
              <label>调速：</label>
            </div>
            <div>
              <Slider
                :v-model="speed"
                :tip-format="sliders"
                :min="100"
                :max="1000"
                style="width: 200px; float: left; margin-right: 10px"
              ></Slider>
            </div>
            <Button type="primary" @click="handleDetail3()">查询</Button>
            <Button type="primary" @click="doplay(1)">播放</Button>
            <Button type="primary" @click="doplay(2)">暂停</Button>
            <!-- <Button type="primary" @click="doplay(3)">停止</Button> -->
          </template>
        </Row>
      </Form>
    </Drawer>

    <!-- <Drawer
      title="实时查看"
      v-model="formModel2.opened"
      width="1000"
      :mask-closable="false"
      :mask="false"
    >
      <Form
        :model="formModel2.fields"
        ref="formCarDispatchDetail"
        label-position="left"
      >
        <baidu-map
          class="map"
          center="杭州"
          :zoom="15"
          :scroll-wheel-zoom="true"
          @ready="mapReady"
        > -->
          <!--设置经纬度，缩放等级，滚轮缩放 -->
          <!-- 在右上角加入比例尺-->
          <!-- <bm-scale anchor="BMAP_ANCHOR_TOP_RIGHT"></bm-scale> -->
          <!-- 在右上角加入缩放控件 -->
          <!-- <bm-navigation anchor="BMAP_ANCHOR_TOP_RIGHT"></bm-navigation> -->
          <!-- 在左上角加入地图类型控件 -->
          <!-- <bm-map-type
            :map-types="['BMAP_NORMAL_MAP', 'BMAP_HYBRID_MAP']"
            anchor="BMAP_ANCHOR_TOP_LEFT"
          ></bm-map-type>-->
          <!-- 在右下角加入定位控件 -->
          
      <!-- </Form>
    </Drawer> -->
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import { BmlLushu } from "vue-baidu-map";
import {
  timeGetTravel,
  getList,
  GetLatLon,
  getTravel, //历史轨迹
  getCarlist, //垃圾信息
  batchCommand //右边上方删除
} from "@/api/grab/travel";

export default {
  name: "travel",
  components: {
    DzTable,
    BmlLushu,
  },
  data() {
    return {
      lushu: {},
      path: [],
      icon: {
        url: "http://api.map.baidu.com/library/LuShu/1.2/examples/car.png",
        size: { width: 52, height: 26 },
        opts: { anchor: { width: 27, height: 13 } },
      },
      speed: 100,

      play: false,
      btnClick: false,

      rowid: "",

      BMap: {},
      map: {},
      map2: {},
      BMap2: {},
      markers: [],
      points: [],

      formModel: {
        opened: false,
        title: "历史轨迹",
        carlist: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isDelete: 0,
            status: -1,
            kw: "",
            kw1: "",
            kw2:[],
            carUuid: "",
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {},
          columns: [
            //{ type: "selection", width: 50, key: "carUuid" },
            { title: "车牌号码", key: "carNum", sortable: true },
            { title: "持有人", key: "holderId" },
            { title: "持有人电话", key: "holderPhone" },
            { title: "车辆重量/kg", key: "weight" }
          ],
          data: []
        }
      },
      //垃圾信息
      stores2: {
        ljlist: {
          query: {
            totalCount: 0,
            pageSize: 5,
            currentPage: 1,
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {},
          //列表显示
          columns: [
            //{ type: "selection", width: 50, key: "GrabageWeighingRecordUuid" },
            { title: "小区名称", key: "vname", sortable: true },
            { title: "垃圾箱名称", key: "ljname" },
            { title: "重量", key: "weight" },
            { title: "添加时间", key: "addTime" }
          ],
          data: []
        }
      },

      formModel2: {
        opened: false,
        title: "实时查看",
        selection: []
      },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" }
      },
      //查询搜索
      stores: {
        grab: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {},
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "carUuid" },
            { title: "车牌号", key: "carNum", sortable: true },
            { title: "所属公司", key: "company" },
            { title: "车辆类型", key: "carType" ,
              ellipsis: true,
              tooltip: true },
            { title: "垃圾类型", key: "grabType" },
            { title: "所属街道", key: "street" },
            {
              title: "操作",
              align: "center",
              width: 300,
              className: "table-command-column",
              slot: "action"
            }
          ],
          data: []
        }
      }
    };
  },
  computed: {
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.carUuid);
    } //删除
  },
  methods: {
    mapReady(e) {
      this.BMap = e.BMap;
      this.map = e.map;
      this.loadLonLatList();
    },

    mapOnly(e) {
      this.BMap2 = e.BMap;
      this.map2 = e.map;
      // //console.log(11111111);
      // //console.log(e);
      // this.map2.centerAndZoom(new BMap2.Point(116.404, 39.915), 15);
      // this.map2.enableScrollWheelZoom();
      // var polyline = new BMap2.Polyline(this.points, {
      //   strokeColor: "blue",
      //   strokeWeight: 2,
      //   strokeOpacity: 0.5
      // }); //创建折线
      // map11.addOverlay(polyline); //增加折线
    },
    //this.$Message.warning("请选择至少一条数据");
    
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
    //获取经纬度数据
    loadLonLatList() {
      GetLatLon(this.stores.grab.query).then(res => {
        // //console.log(res)
        // this.stores.grab.data = res.data.data;
        //console.log(res.data.data);
        for (let i = 0; i < res.data.data.length; i++) {
          if (
            res.data.data[i].lat != null &&
            res.data.data[i].lat != "" &&
            res.data.data[i].lon != null &&
            res.data.data[i].lon != ""
          ) {
            let marker = new this.BMap.Marker(
              new this.BMap.Point(res.data.data[i].lon, res.data.data[i].lat)
            );
            //console.log(marker);
            this.map.addOverlay(marker);
          }
        }
      });
    },
    //根据时间查看历史记录
    handleDetail3() {
      this.points=[];
      if(this.points.length>0){
        this.map2.clearOverlays();
      }
      let ss = this.formModel.carlist.query;
      this.formModel.carlist.query.carUuid = this.rowid;
      if(ss.kw1!="")
      {
      timeGetTravel(this.formModel.carlist.query).then(res => {
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
    //获取信息数据
    loadCarDispatchList() {
      console.log(this.stores.grab.query);
      getList(this.stores.grab.query).then(res => {
        ////console.log(res.data.data)
        this.stores.grab.data = res.data.data;
        ////console.log(res.data.data);
        this.stores.grab.query.totalCount = res.data.totalCount;
      });
    },

    //翻页
    handlePageChanged(page) {
      this.stores.grab.query.currentPage = page;
      this.loadCarDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.grab.query.pageSize = pageSize;
      this.loadCarDispatchList();
    },

    //获取信息数据
    loadCarDispatchList2() {
      getCarlist(this.stores2.ljlist.query).then(res => {
        //console.log(res.data.data);
        this.stores2.ljlist.data = res.data.data;
        ////console.log("111");
        this.stores2.ljlist.query.totalCount = res.data.totalCount;
      });
    },
    //翻页
    handlePageChanged2(page) {
      this.stores2.ljlist.query.currentPage = page;
      this.loadCarDispatchList2();
    },
    //显示条数改变
    handlePageSizeChanged2(pageSize) {
      this.stores2.ljlist.query.pageSize = pageSize;
      this.loadCarDispatchList2();
    },
    //打开窗口
    // handleOpenFormWindow() {
    //   this.formModel.opened = true;
    // },
    // //自动关闭窗口
    // handleCloseFormWindow() {
    //   this.formModel.opened = false;
    // },
    //行样式
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    //搜索
    handleSearchDispatch() {
      this.loadCarDispatchList();
    },
    //刷新
    handleRefresh() {
      this.loadCarDispatchList();
    },
    //右上边删除按钮
    handleBatchCommand(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        }
      });
    },
    //右上边删除
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadCarDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },

    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleSortChange(column) {
      this.stores.grab.query.sort.direction = column.order;
      this.stores.grab.query.sort.field = column.key;
      this.loadCarDispatchList();
    },
    handleSortChange2(column) {
      this.stores2.ljlist.query.sort.direction = column.order;
      this.stores2.ljlist.query.sort.field = column.key;
      this.loadCarDispatchList2();
    },
    //历史轨迹
    handleDetail(row) {
      this.btnClick = true;
      if(this.points.length>0){
        this.map2.clearOverlays();
      }
      // this.formModel.carlist.query.kw1="";
      var d = new Date();
      var date = d.getFullYear() + "-" +this.completeDate(d.getMonth() + 1) + "-" + this.completeDate(d.getDate());
      this.formModel.carlist.query.kw1=date;
      // this.formModel.carlist.query.kw2=[];
      this.points=[];
      //this.$refs["formCarDispatchDetail"].resetFields();
      this.rowid = row.carUuid;
      getTravel(row.carUuid).then(res => {
        this.formModel.carlist.data = res.data.data;
        //this.carNum = res.data.data.carNum;
        this.formModel.opened = true;
        this.handleDetail3();
        ////console.log(this.formModel.carlist.data);
      });
      this.loadCarDispatchList2();
    },
    
    //实时查看
    handleDetail2() {
      ////console.log(row);
      this.formModel2.opened = true;
      //this.$refs["formCarDispatchDetail"].resetFields();
      // loadCarDriverDetail({ guid: row.attendanceUuid }).then(res => {
      //   this.formModel.fields = res.data.data;
      ////console.log(this.formModel.fields);
      // });
    },
    completeDate(value) {
        return value < 10 ? "0"+value:value;
    }
  },
  mounted() {
    this.loadCarDispatchList();
  }
};
</script>
<style>
.map1 {
  width: 100%;
  height: 400px;
}
.map {
  width: 100%;
  height: 800px;
}
.carlist .ivu-page,
.anchorBL {
  display: none;
}
</style>
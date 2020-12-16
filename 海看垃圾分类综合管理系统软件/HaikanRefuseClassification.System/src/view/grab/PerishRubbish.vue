
<template>
  <div>
    <Card>
      <dz-table :totalCount="stores.car.query.totalCount" :pageSize="stores.car.query.pageSize"
        @on-page-change="handlePageChanged" @on-page-size-change="handlePageSizeChanged">
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
              <Form inline @submit.native.prevent>
                <FormItem>
                  <Select v-model="stores.car.query.street" style="width:150px" @on-change="xz" clearable
                    placeholder="请选择街道">
                    <Option v-for="item in this.stores.car.sources.town" :value="item" :key="item">{{ item }}</Option>
                  </Select>
                  <Select v-model="stores.car.query.ccmmunity" style="width:150px" @on-change="sq" clearable
                    placeholder="请选择社区">
                    <Option v-for="item in this.stores.car.sources.college" :value="item" :key="item">{{ item }}
                    </Option>
                  </Select>
                </FormItem>
                <FormItem>
                  <Input style="width: 150px;" type="text" search :clearable="true" v-model="stores.car.query.kw"
                    placeholder="请输入箱房名称" @on-search="handleSearchDispatch()"></Input>
                </FormItem>
                <!-- <FormItem>
                  <Input style="width: 150px;" type="text" search :clearable="true" v-model="stores.car.query.kw1"
                    placeholder="请输入社区名称" @on-search="handleSearchDispatch()"></Input>
                </FormItem>
                <FormItem>
                  <DatePicker type="daterange" v-model="stores.car.query.time" format="yyyy/MM/dd" placement="top"
                    placeholder="记录日期" style="width: 200px" :confirm="true" :editable="false"
                    @on-ok="handleSearchDispatch()"></DatePicker>
                </FormItem> -->
              </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
              <ButtonGroup class="mr3">
                <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
              </ButtonGroup>
              </Col>
            </Row>
          </section>
        </div>
        <Table slot="table" ref="tables" :border="false" size="small" :highlight-row="true" :data="stores.car.data"
          :columns="stores.car.columns" @on-select="handleSelect" @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh" :row-class-name="rowClsRender" @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged" @on-sort-change="handleSortChange">
          <template slot-scope="{ row, index}" slot="action">
            <Tooltip v-can="'show'" placement="top" content="称重记录详情" :delay="1000" :transfer="true">
              <Button v-can="'show'" type="warning" size="small" shape="circle" icon="md-search"
                @click="handleDetail(row)"></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer title="称重记录详情" v-model="formModel2.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel2.fields" label-position="left">
        <Row :gutter="16">
          <FormItem label="垃圾箱房名字">
            <Input v-model="formModel2.fields.ljname" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="所在社区">
            <Input v-model="formModel2.fields.vname" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="车牌号">
            <Input v-model="formModel2.fields.carNumber" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="垃圾种类">
            <Input v-model="formModel2.fields.type" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="称重类型">
            <Input v-model="formModel2.fields.recordType" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="重量">
            <Input v-model="formModel2.fields.weight" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="称重时间">
            <Input v-model="formModel2.fields.addTime" :readonly="true" />
          </FormItem>
        </Row>
      </Form>
    </Drawer>
  </div>
</template>
<script>
  import DzTable from "_c/tables/dz-table.vue";
  import {
    getCarDriverList, //显示数据
    getCarDriverList2, //右边详情
    deleteCarDriver, //右边删除
    batchCommand, //右边上方删除
    loadCarDriver, //加载
  } from "@/api/grab/record";
  import {
    getPerishRubbishList, //显示易腐垃圾统计数据
  } from "@/api/grab/PerishRubbish"
  import {
    getshequ
  } from "@/api/base/house";
  export default {
    name: "roomAcceptance",
    components: {
      DzTable
    },
    data() {
      return {
        formModel1: {
          opened: false,
          title: "详情",
          selection: [],
          fields: {
            state: "",
            ljname: "", //箱房名称
            garbageRoomUuid: "",
            towns: "",
            vname: "",
            corruptRubbishPercent: "",
            sytime: "",
            rubbishType: "",
            addTime: "",
            weight: "",
            garbageSyuuid: "",
          }
        },
        formModel2: {
          opened: false,
          title: "称重记录详情",
          selection: [],
          fields: {
            grabageWeighingRecordUuid: "",
            grabageRoomId: "",
            ljname: "", //箱房名称
            vname: "",
            carNumber: "",
            type: "",
            weight: "",
            addTime: "",
            recordType: "",
          }
        },
        list33: [],
        stores: {
          car: {
            query: {
              totalCount: 0,
              pageSize: 20,
              currentPage: 1,
              kw: "",
              kw1: "",
              kw2: "",
              vuuid: this.$store.state.user.villageGuid,
              street: "",
              ccmmunity: "",
              time: "",
              isDeleted: 0,
              status: -1,
              sort: [{
                direct: "DESC",
                field: "ID"
              }]
            },
            sources: {
              //社区集合
              college: ["全部"],
              //街道集合
              town: ["全部"],
            },
            //列表显示
            columns: [
              // { type: "selection", width: 50, key: "handle" },
              {
                title: "所在乡镇（街道）",
                minWidth: 100,
                key: "towns"
              },
              {
                title: "所在社区",
                minWidth: 100,
                key: "vName"
              },
              {
                title: "箱房名称",
                minWidth: 100,
                key: "ljName"
              },
              {
                title: "当天记录时间",
                minWidth: 110,
                key: "addtimes",
              },
              //   {
              //     title: "当天量",
              //     minWidth: 80,
              //     key: "dayData"
              //   },
              {
                title: "当天易腐垃圾比例",
                minWidth: 80,
                key: "dataratio"
              },
              //   {
              //     title: "当周量",
              //     minWidth: 100,
              //     key: "weekdata"
              //   },
              {
                title: "本周易腐垃圾比例",
                minWidth: 80,
                key: "weekratio"
              },
            //   {
            //     title: "当年量",
            //     minWidth: 80,
            //     key: "yeardata"
            //   },
              {
                title: "截至上月易腐垃圾比例",
                minWidth: 100,
                key: "yearratio"
              },
              //   {
              //     title: "操作",
              //     align: "center",
              //     width: 100,
              //     className: "table-command-column",
              //     slot: "action"
              //   }
            ],
            data: []
          }
        }
      };
    },
    computed: {
      selectedRows() {
        return this.formModel1.selection;
      },
      selectedRowsId() {
        return this.formModel1.selection.map(x => x.garbageSyuuid);
      } //删除
    },
    methods: {
      //获取垃圾箱房统计数据
      loadCarDispatchList() {
        getPerishRubbishList(this.stores.car.query).then(res => {
          console.log(res.data.data)
          this.stores.car.data = res.data.data;
          this.stores.car.data.addTime = new Date();
          this.stores.car.query.totalCount = res.data.totalCount;
        });
      },
      //右边称重记录详情
      handleDetail(row) {
        //console.log(row);
        this.formModel2.opened = true;
        let data = {
          guid: row.grabageWeighingRecordUuid.toString(),
          time: row.addTime
        };
        //console.log(data);
        getCarDriverList2(data).then(res => {
          //console.log(res)
          this.formModel2.fields = res.data.data;
        });
      },
      //翻页
      handlePageChanged(page) {
        this.stores.car.query.currentPage = page;
        this.loadCarDispatchList();
      },
      //显示条数改变
      handlePageSizeChanged(pageSize) {
        this.stores.car.query.pageSize = pageSize;
        this.loadCarDispatchList();
      },
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

      //右边删除（单个删除）
      handleDelete(row) {
        this.doDelete(row.garbageSyuuid);
      },
      doDelete(ids) {
        if (!ids) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        deleteCarDriver(ids).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.loadCarDispatchList();
            this.formModel1.selection = [];
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
      //右上边删除按钮
      handleBatchCommand(command) {
        if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        this.$Modal.confirm({
          title: "操作提示",
          content: "<p>确定要执行当前 [" +
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
            this.formModel1.selection = [];
          } else {
            this.$Message.warning(res.data.message);
          }
          this.$Modal.remove();
        });
      },
      //申请社区下拉框
      loadDepartmentList1() {
        let data = this.$store.state.user.villageGuid;
        getshequ(data).then(res => {
          //console.log(res.data.data);
          this.list33 = res.data.data;
          let townn = Array.from(new Set(this.list33.map(x => x.towns)));
          //console.log(townn);          
          this.stores.car.sources.town = townn;
        });

      },
      xz(e) {
        let college = this.list33.filter(x => x.towns == e);
        this.stores.car.sources.college = college.map(x => x.vname);
        this.loadCarDispatchList();
      },
      sq(e) {
        if (e == "全部") {
          this.stores.car.query.ccmmunity = "";
        }
        this.loadCarDispatchList();
      },
      handleSelect(selection, row) {},
      handleSelectionChange(selection) {
        this.formModel1.selection = selection;
      },

      handleSortChange(column) {
        this.stores.user.query.sort.direction = column.order;
        this.stores.user.query.sort.field = column.key;
        this.loadCarDispatchList();
      },
    },
    mounted() {
      this.loadDepartmentList1();
      this.loadCarDispatchList();
    }
  };

</script>
<style>
</style>

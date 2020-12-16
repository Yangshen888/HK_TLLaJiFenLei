<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.car.query.totalCount"
        :pageSize="stores.car.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Select
                      v-model="stores.car.query.street"
                      style="width: 150px"
                      @on-change="xz"
                      clearable
                      placeholder="请选择街道"
                    >
                      <Option
                        v-for="item in this.stores.car.sources.town"
                        :value="item"
                        :key="item"
                        >{{ item }}</Option
                      >
                    </Select>
                    <Select
                      v-model="stores.car.query.ccmmunity"
                      style="width: 150px"
                      @on-change="sq"
                      clearable
                      placeholder="请选择社区"
                    >
                      <Option
                        v-for="item in this.stores.car.sources.college"
                        :value="item"
                        :key="item"
                        >{{ item }}</Option
                      >
                    </Select>
                  </FormItem>
                  <FormItem>
                    <Input
                      style="width: 150px"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.car.query.kw"
                      placeholder="请输入箱房名称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                  <FormItem>
                    <Input
                      style="width: 150px"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.car.query.kw1"
                      placeholder="请输入社区名称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                  <FormItem>
                    <DatePicker
                      type="daterange"
                      v-model="stores.car.query.time"
                      format="yyyy/MM/dd"
                      placement="top"
                      placeholder="收运日期"
                      style="width: 200px"
                      :confirm="true"
                      :editable="false"
                      @on-ok="handleSearchDispatch()"
                    ></DatePicker>
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
                  ></Button> -->

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
          :data="stores.car.data"
          :columns="stores.car.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <template slot-scope="{ row, index }" slot="action">
            <!-- <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip> -->
            <Tooltip
              v-can="'show'"
              placement="top"
              content="称重记录详情"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      title="称重记录详情"
      v-model="formModel2.opened"
      width="1100"
      :mask-closable="false"
      :mask="false"
    >
      <Table
        :border="false"
        size="small"
        :highlight-row="true"
        :data="stores.car.data2"
        :columns="stores.car.columns2"
        @on-select="handleSelect"
        @on-selection-change="handleSelectionChange"
        @on-refresh="handleRefresh"
        :row-class-name="rowClsRender"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
        @on-sort-change="handleSortChange"
      >
        <template slot-scope="{ row, index }" slot="action">
          <!-- <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip> -->
          <Tooltip
            v-can="'show'"
            placement="top"
            content="编辑"
            :delay="1000"
            :transfer="true"
          >
            <Button
              v-can="'show'"
              type="info"
              size="small"
              shape="circle"
              icon="md-create"
              @click="handleRecordEdit(row)"
            ></Button>
          </Tooltip>
        </template>
      </Table>
    </Drawer>
    <Drawer
      title="编辑称重记录"
      v-model="formModel3.opened"
      width="400"
      :mask-closable="false"
      :mask="false"
    >
      <Form
        :model="formModel3.fields"
        ref="form3"
        :rules="formModel3.rules"
        label-position="left"
      >
        <FormItem label="垃圾重量" prop="weight" label-position="left">
          <InputNumber v-model="formModel3.fields.weight" :min="0" :step="0.1" :precision="2" style="width:200px;"></InputNumber>
        </FormItem>
        <!-- <FormItem label="当前状态" prop="state">
          <Select v-model="formModel3.fields.state">
            <Option value="0">{{ "使用中" }}</Option>
            <Option value="1">{{ "暂停使用" }}</Option>
          </Select>
        </FormItem> -->
        <FormItem label="垃圾类型" prop="type">
          <Select v-model="formModel3.fields.type">
            <Option value="3">可回收物</Option>
            <Option value="2">有害垃圾</Option>
            <Option value="1">易腐垃圾</Option>
            <Option value="0">其他垃圾</Option>
          </Select>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitRecord"
          >保 存</Button
        >
        <Button
          style="margin-left: 8px"
          icon="md-close"
          @click="formModel3.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getCarDriverList, //显示数据
  getCarDriverList2, //右边详情
  getDayOfWeightList,
  deleteCarDriver, //右边删除
  batchCommand, //右边上方删除
  loadCarDriver, //加载
  loadRecordInfo,
  saveRecordInfo,
} from "@/api/grab/record";

import { getshequ } from "@/api/base/house";
export default {
  name: "roomAcceptance",
  components: {
    DzTable,
  },
  data() {
    return {
      formModel1: {
        opened: false,
        title: "详情",
        selection: [],
        fields: {
          state: "",
          ljname: "",
          garbageRoomUuid: "",
          towns: "",
          vname: "",
          corruptRubbishPercent: "",
          sytime: "",
          rubbishType: "",
          addTime: "",
          weight: "",
          garbageSyuuid: "",
        },
      },

      formModel2: {
        opened: false,
        title: "称重记录详情",
        selection: [],
        fields: {
          grabageWeighingRecordUuid: "",
          grabageRoomId: "",
          ljname: "",
          vname: "",
          carNumber: "",
          type: "",
          weight: "",
          addTime: "",
          recordType: "",
        },
      },

      formModel3: {
        opened: false,
        title: "编辑",
        selection: [],
        fields: {
          grabageWeighingRecordUuid: "",

          // state: "",
          type: "",
          weight: 0,
        },
        rules: {},
      },
      checkData: {
        guid: "",
        time: "",
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
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },

          sources: {
            //社区集合
            college: ["全部"],
            //街道集合
            town: ["全部"],
            state: [
              { value: 0, text: "使用中" },
              { value: 1, text: "暂停使用" },
            ],
          },
          //列表显示
          columns: [
            // { type: "selection", width: 50, key: "handle" },
            { title: "所在乡镇（街道）", minWidth: 100, key: "towns" },
            { title: "所在社区", minWidth: 100, key: "vname" },
            { title: "箱房名称", minWidth: 150, key: "ljname" },
            { title: "箱房编号", minWidth: 80, key: "wingID" },
            { title: "当前状态", minWidth: 100, key: "state" },
            //{ title: "易腐垃圾比例",width: 150, key: "corruptRubbishPercent" },
            { title: "垃圾收运时间", minWidth: 110, key: "wTime" },
            // { title: "垃圾称重时间", key: "addTime" },
            { title: "当天易腐垃圾比例", minWidth: 100, key: "proportion" },
            {
              title: "操作",
              align: "center",
              width: 100,
              className: "table-command-column",
              slot: "action",
            },
          ],
          columns2: [
            // { type: "selection", width: 50, key: "handle" },
            {
              title: "所在乡镇（街道）",
              minWidth: 100,
              maxWidth: 120,
              key: "towns",
            },
            { title: "所在社区", minWidth: 100, maxWidth: 110, key: "vname" },
            { title: "箱房名称", minWidth: 150, key: "ljname" },
            { title: "箱房编号", minWidth: 80, key: "wingID" },
            { title: "垃圾类型", width: 90, key: "type" },
            { title: "当前状态", width: 90, key: "state" },
            //{ title: "易腐垃圾比例",width: 150, key: "corruptRubbishPercent" },
            // { title: "垃圾收运时间",minWidth: 110, key: "wTime" },
            { title: "垃圾称重时间", width: 130, key: "addTime" },
            { title: "重量", minWidth: 60, maxWidth: 65, key: "weight" },
            // { title: "当天易腐垃圾比例", minWidth: 100,key: "proportion" },
            {
              title: "操作",
              align: "center",
              width: 90,
              className: "table-command-column",
              slot: "action",
            },
          ],

          data: [],
          data2: [],
        },
      },
    };
  },
  computed: {
    selectedRows() {
      return this.formModel1.selection;
    },
    selectedRowsId() {
      return this.formModel1.selection.map((x) => x.garbageSyuuid);
    }, //删除
  },
  methods: {
    handleRecordEdit(row) {
      console.log(row);
      loadRecordInfo(row.grabageWeighingRecordUuid).then((res) => {
        console.log(res);
        this.formModel3.fields = res.data.data;
        this.formModel3.opened = true;
      });
    },
    handleSubmitRecord() {
      if(this.formModel3.fields.weight<=0){
        this.$Message.warning("请输入大于0的称重数据");
        return;
      }
      saveRecordInfo(this.formModel3.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel3.opened = false;
          this.handleDetail2();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },

    //获取垃圾箱房收运数据
    loadCarDispatchList() {
      // getCarDriverList(this.stores.car.query).then(res => {
      //   //console.log(res.data.data)
      //   this.stores.car.data = res.data.data;
      //   this.stores.car.query.totalCount = res.data.totalCount;
      // });
      getDayOfWeightList(this.stores.car.query).then((res) => {
        console.log(res.data.data);
        this.stores.car.data = res.data.data;
        this.stores.car.query.totalCount = res.data.totalCount;
      });
    },

    //右边称重记录详情
    handleDetail(row) {
      console.log(row);
      this.formModel2.opened = true;
      this.checkData = { guid: row.grabageRoomID.toString(), time: row.wTime };

      //console.log(data);
      getCarDriverList2(this.checkData).then((res) => {
        console.log(res);
        this.stores.car.data2 = res.data.data;
        // this.formModel3.opened = true;
      });
    },

    handleDetail2() {
      getCarDriverList2(this.checkData).then((res) => {
        console.log(res);
        this.stores.car.data2 = res.data.data;
        // this.formModel3.opened = true;
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
      deleteCarDriver(ids).then((res) => {
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
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        },
      });
    },
    //右上边删除
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
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
      getshequ(data).then((res) => {
        //console.log(res.data.data);
        this.list33 = res.data.data;
        let townn = Array.from(new Set(this.list33.map((x) => x.towns)));
        //console.log(townn);
        this.stores.car.sources.town = townn;
      });
    },
    xz(e) {
      let college = this.list33.filter((x) => x.towns == e);
      this.stores.car.sources.college = college.map((x) => x.vname);
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
  },
};
</script>
<style>
</style>

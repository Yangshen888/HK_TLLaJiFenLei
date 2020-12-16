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
                      style="width:150px"
                      @on-change="xz"
                      clearable
                      placeholder="请选择街道"
                    >
                      <Option
                        v-for="item in this.stores.car.sources.town"
                        :value="item"
                        :key="item"
                      >{{ item }}</Option>
                    </Select>
                    <Select
                      v-model="stores.car.query.ccmmunity"
                      style="width:150px"
                      @on-change="sq"
                      clearable
                      placeholder="请选择社区"
                    >
                      <Option
                        v-for="item in this.stores.car.sources.college"
                        :value="item"
                        :key="item"
                      >{{ item }}</Option>
                    </Select>
                  </FormItem>
                  <FormItem>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.car.query.kw"
                      placeholder="请输入用户昵称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                  <FormItem>
                    <DatePicker
                      type="daterange"
                      v-model="stores.car.query.kw2"
                      format="yyyy/MM/dd"
                      placement="top"
                      placeholder="输入时间搜索..."
                      style="width: 200px"
                      :confirm="true"
                      :editable="false"
                      @on-ok="handleSearchDispatch()"
                      @on-clear="handleSearchDispatch()"
                    ></DatePicker>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <!--                  <Button-->
                  <!--                    v-can="'delete'"-->
                  <!--                    class="txt-danger"-->
                  <!--                    icon="md-trash"-->
                  <!--                    title="删除"-->
                  <!--                    @click="handleBatchCommand('delete')"-->
                  <!--                  ></Button>-->
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
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
          <template slot-scope="{row,index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <!--            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">-->
            <!--              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">-->
            <!--                <Button type="error" size="small" shape="circle" icon="md-trash"></Button>-->
            <!--              </Tooltip>-->
            <!--            </Poptip>-->
            <Button
              v-can="'show'"
              type="warning"
              size="small"
              shape="circle"
              icon="md-search"
              @click="loadRecord(row)"
            ></Button>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer title="积分兑换记录" v-model="stores2.opened" width="80%" :mask-closable="false" :mask="true">
      <dz-table
        :totalCount="stores2.own.query.totalCount"
        :pageSize="stores2.own.query.pageSize"
        @on-page-change="handlePageChanged1"
        @on-page-size-change="handlePageSizeChanged1">
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores2.own.data"
          :columns="stores2.own.columns"
          :row-class-name="rowClsRender"
        >
        </Table>
      </dz-table>
      <div class="demo-drawer-footer">
        <Button style="margin-left: 30px" icon="md-close" @click="stores2.opened = false">关 闭</Button>
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getVoluList,
  batchCommand, //右边上方删除
  loadCarDriver, //加载
  GetRecord, //详情
} from "@/api/score/volu";

import { getChangesList, getChangesList1 } from "@/api/score/own";
import { getshequ } from "@/api/base/house";

export default {
  name: "house",
  components: {
    DzTable,
  },
  data() {
    return {
      rowid: "",
      formModel: {
        opened: false,
        mode: "create",
        selection: [],
        fields: {
          realName: "",
          phone: "",
          isDelete: 0,
          we_Chat: "",
          addTime: "",
        },
        rules: {},
      },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
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
            street: "",
            ccmmunity: "",
            isDelete: 0,
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
          },
          columns: [
            { type: "selection", width: 50, key: "storeExchangeUuid" },
            // { title: "商品名称",width:200, key: "gname", sortable: true },
            { title: "住户", key: "address", ellipsis: true, tooltip: true },
            { title: "兑换时间", key: "exchageTime" },
            // { title: "扣除积分", width: 100, key: "deduceScore" },
            // { title: "剩余积分", width: 100, key: "score" },
            // {
            //   title: "商店名称",
            //   key: "shopName",
            //   ellipsis: true,
            //   tooltip: true,
            // },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
      },
      stores2: {
        opened: false,
        own: {
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
                field: "ID",
              },
            ],
          },
          sources: {},
          columns: [
            { type: "selection", width: 50, key: "storeExchangeUuid" },
            // { title: "商品名称",width:200, key: "gname", sortable: true },
            { title: "住户", key: "address", ellipsis: true, tooltip: true },
            { title: "兑换时间", key: "exchageTime" },
            // { title: "扣除积分", width: 100, key: "deduceScore" },
            // { title: "剩余积分", width: 100, key: "score" },
            // {
            //   title: "商店名称",
            //   key: "shopName",
            //   ellipsis: true,
            //   tooltip: true,
            // },
          ],
          data: [],
        },
      },
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建调度申请";
      }
      if (this.formModel.mode === "edit") {
        return "编辑调度信息";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.systemUserUuid);
    }, //删除
  },
  methods: {
    //获取信息数据
    loadCarDispatchList() {
      getChangesList(this.stores.car.query).then((res) => {
        this.stores.car.data = res.data.data;
        // for(let i=0;i<res.data.data.length;i++){
        //   if(res.data.data[i].address.split('县')[1]!=undefined){
        //       this.stores.car.data[i].address= res.data.data[i].address.split('县')[1];
        //   }
        // }
        this.stores.car.query.totalCount = res.data.totalCount;
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
     //翻页
    handlePageChanged1(page) {
      this.stores2.own.query.currentPage = page;
      this.dogetChangesList1();
    },
    //显示条数改变
    handlePageSizeChanged1(pageSize) {
      this.stores2.own.query.pageSize = pageSize;
      this.dogetChangesList1();
    },
    //打开窗口
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    //自动关闭窗口
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    //行样式
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    //非空验证提示
    validateDispatchForm() {
      let _valid = false;
      this.$refs["formdispatch"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },

    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatch"].resetFields();
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
      this.doDelete(row.volunteerServiceUuid);
    },

    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
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
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    //获取积分
    loadRecord(row) {
      this.stores2.own.data.kw = row.systemUserUuid;
      this.dogetChangesList1();
    },
    dogetChangesList1(){
      getChangesList1(this.stores2.own.query).then((res) => {
        this.stores2.own.data = res.data.data;
        this.stores2.opened = true;
        this.stores2.own.query.totalCount = res.data.totalCount;
        //console.log(this.stores2.own.data);
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
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
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


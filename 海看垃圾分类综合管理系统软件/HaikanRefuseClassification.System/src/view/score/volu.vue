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
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.car.query.kw"
                      placeholder="输入昵称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                  <FormItem>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.car.query.kw1"
                      placeholder="输入联系方式"
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
          <template slot-scope="{ row, index }" slot="action">
<!--            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">-->
<!--              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">-->
<!--                <Button type="error" size="small" shape="circle" icon="md-trash"></Button>-->
<!--              </Tooltip>-->
<!--            </Poptip>-->
            <Button  v-can="'show'" type="warning" size="small" shape="circle" icon="md-search" @click="loadRecord(row)"></Button>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      title="志愿者服务记录"
      v-model="stores2.opened"
      width="80%"
      :mask-closable="false"
      :mask="true"
    >
      <dz-table>
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
<!--          <template slot-scope="{ row, index }" slot="action1">-->
<!--            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">-->
<!--              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">-->
<!--                <Button type="error" size="small" shape="circle" icon="md-trash"></Button>-->
<!--              </Tooltip>-->
<!--            </Poptip>-->
<!--          </template>-->
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
    deleteVoluSerRecord, //右边删除
    batchCommand, //右边上方删除
    loadCarDriver, //加载
    getScoreData,//加载分数
    GetRecord,//详情

    globalvalidatePhone
  } from "@/api/score/volu";

  export default {
    name: "house",
    components: {
      DzTable
    },
    data() {
      let globalvalidatePhone = (rule, value, callback) => {
        if (value !== '' && value !== null) {
          callback();
        } else {
          callback(new Error('联系方式号码不能为空'));
        }
        callback();
      };
      return {
        rowid:"",
        formModel: {
          opened: false,
          mode: "create",
          selection: [],
          fields: {
            loginName:"",
            phone:"",
            isDelete: 0,
            we_Chat:"",
            addTime:""
          },
          rules: {
            loginName: [
              { type: "string", required: true, message: "请输入志愿者" }
            ],
            phone: [
              { type: "string", required: true, validator:globalvalidatePhone}
            ]
          }
        },
        commands: {
          delete: { name: "delete", title: "删除" },
          recover: { name: "recover", title: "恢复" }
        },
        stores: {
          car: {
            query: {
              totalCount: 0,
              pageSize: 20,
              currentPage: 1,
              kw: "",
              kw1:"",
              kw2:"",
            vuuid: this.$store.state.user.villageGuid,
              isDelete: 0,
              status: -1,
              sort: [
                {
                  direct: "DESC",
                  field: "ID"
                }
              ]
            },
            sources: {
            },
            columns: [
              { type: "selection", width: 50, key: "systemUserUuid" },
              { title: "志愿者姓名", key: "loginName", sortable: true },
              { title: "真实姓名", key: "realName", sortable: true },
              { title: "注册时间", key: "addTime" },
              { title: "联系方式", key: "phone" },
              { title: "累积服务次数", key: "count" },
              { title: "累积服务时长", key: "alltime" },
              { title: "累计积分", key: "score" },
              {
                title: "操作",
                align: "center",
                width: 150,
                className: "table-command-column",
                slot: "action"
              }
            ],
            data: []
          }
        },
        stores2: {
          opened: false,
          own: {
            query: {
              totalCount: 0,
              pageSize: 20,
              currentPage: 1,
              kw: "",
              kw1:"",
              kw2:"",
              isDelete: 0,
              status: -1,
              sort: [
                {
                  direct: "DESC",
                  field: "ID"
                }
              ]
            },
            sources: {
            },
            columns: [
              { type: "selection", width: 50, key: "volunteerServiceUuid" },
              // { title: "志愿者姓名", key: "name", sortable: true },
              // { title: "真实姓名", key: "realName", sortable: true },
              { title: "垃圾箱房", key: "grID" },
              { title: "所在小区", key: "vilage" },
              { title: "签到时间", key: "intime" },
              { title: "签退时间", key: "outtime" },
              { title: "时段", key: "fw" },
            ],
            data: []
          }
        }
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
        return this.formModel.selection.map(x => x.systemUserUuid);
      } //删除
    },
    methods: {
      //获取信息数据
      loadCarDispatchList() {
        getVoluList(this.stores.car.query).then(res => {
          this.stores.car.data = res.data.data;
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
        this.$refs["formdispatch"].validate(valid => {
          if (!valid) {
            this.$Message.error("请完善表单信息");
          } else {
            _valid = true;
          }
        });
        return _valid;
      },
      doLoadCarDispatch(volunteerId) {
        loadCarDriver({guid: volunteerId}).then(res => {
          this.formModel.fields = res.data.data;
        });
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
      //获取积分
      loadRecord(row) {
        this.rowid=row.systemUserUuid;
        GetRecord(row.systemUserUuid).then(res => {
          this.stores2.own.data = res.data.data;
          this.stores2.opened = true;
          //console.log(this.stores2.own.data);
        });
      },
      doDelete(ids) {
        if (!ids) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        deleteVoluSerRecord(ids).then(res => {

          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.loadCarDispatchList();
            this.formModel.selection = [];
            GetRecord(this.rowid).then(res => {
              this.stores2.own.data = res.data.data;
            });
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

      handleSwitchFormModeToEdit() {
        this.formModel.mode = "edit";
      },
      handleSelect(selection, row) {
      },
      handleSelectionChange(selection) {
        this.formModel.selection = selection;
      },
      handleSortChange(column) {
        this.stores.user.query.sort.direction = column.order;
        this.stores.user.query.sort.field = column.key;
        this.loadCarDispatchList();
      },

      //添加按钮（志愿者申请）
      handleShowCreateWindow() {
        this.handleSwitchFormModeToCreate();
        this.handleOpenFormWindow(); //打开窗口
        this.handleResetFormDispatch(); //清空表单
        this.formModel.fields.realName = this.$store.state.user.userName;

      },
      handleSwitchFormModeToCreate() {
        this.formModel.mode = "create";
      },
      //保存按钮
      handleSubmitConsumable() {
        let valid = this.validateDispatchForm();
        if (valid) {
          if (this.formModel.mode === "create") {
            this.docreateCarDispatch();
          }
          if (this.formModel.mode === "edit") {
            this.doEditCarDispatch();
          }
        }
      },
    },
    mounted() {
      this.loadCarDispatchList();
    }
  };
</script>


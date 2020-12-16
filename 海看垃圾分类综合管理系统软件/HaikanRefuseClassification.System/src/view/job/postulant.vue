<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.job.query.totalCount"
        :pageSize="stores.job.query.pageSize"
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
                      v-model="stores.job.query.kw"
                      placeholder="输入志愿者姓名"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                  <FormItem>
                    <DatePicker
                      type="daterange"
                      v-model="stores.job.query.kw2"
                      format="yyyy/MM/dd"
                      placement="top"
                      placeholder="输入时间搜索..."
                      style="width: 200px"
                      :confirm="true"
                      :editable="false"
                      @on-ok="handleSearchDispatch()"
                    ></DatePicker>
                  </FormItem> 
                  <FormItem>
                    <Select
                      v-model="stores.indexId"
                      @on-change="dakatime()"
                      placeholder="打卡时间"
                      style="width:150px;"
                    >
                      <Option
                        v-for="item in stores.index"
                        :value="item.indexId"
                        :key="item.indexId"
                      >{{item.name}}</Option>
                    </Select>
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
          :data="stores.job.data"
          :columns="stores.job.columns"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <template slot-scope="{row,index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <!-- <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>-->
            <!-- <Tooltip
              placement="top"
              content="编辑"
              :delay="1000"
              :transfer="true"             
            >
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>-->
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
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

    <Drawer title="详情" v-model="formModel2.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel2.fields" ref="formCarDispatchDetail" label-position="left">
        <Row :gutter="16">
          <FormItem label="微信名称">
            <Input v-model="formModel2.fields.loginName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="上午打卡状态">
            <Input
              v-model="formModel2.fields.startState"
              :readonly="true"
              style="width:40%;margin-right: 10px;"
            />
            <Button
              v-can="'buka1'"
              type="primary"
              shape="circle"
              v-show="this.formModel2.fields.startState=='未打卡'"
            >补卡</Button>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="上午上班打卡时间">
            <Time-picker
              format="HH:mm"
              v-model="buka.amstartTime"
              placeholder="选择时间"
              style="width: 100%"
              v-if="this.formModel2.fields.startState=='未打卡' && this.formModel2.fields.amstartTime==null"
            ></Time-picker>
            <Input v-model="formModel2.fields.amstartTime" :readonly="true" v-else />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="上午上班打卡地点">
            <Input
              type="textarea"
              v-model="formModel2.fields.amstartPlace"
              v-if="this.formModel2.fields.startState=='未打卡' && this.formModel2.fields.amstartPlace==null"
            />
            <Input
              type="textarea"
              v-model="formModel2.fields.amstartPlace"
              :readonly="true"
              v-else
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="上午下班打卡时间">
            <Time-picker
              format="HH:mm"
              v-model="buka.amendTime"
              placeholder="选择时间"
              style="width: 100%"
              v-if="this.formModel2.fields.startState=='未打卡' && this.formModel2.fields.amendTime==null"
            ></Time-picker>
            <Input v-model="formModel2.fields.amendTime" :readonly="true" v-else />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="上午下班打卡地点">
            <Input
              type="textarea"
              v-model="formModel2.fields.amendPlace"
              v-if="this.formModel2.fields.startState=='未打卡' && this.formModel2.fields.amendPlace==null"
            />
            <Input type="textarea" v-model="formModel2.fields.amendPlace" :readonly="true" v-else />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="下午打卡状态" prop="endState">
            <Input
              v-model="formModel2.fields.endState"
              :readonly="true"
              style="width:40%;margin-right: 10px;"
            />
            <Button
              v-can="'buka2'"
              type="primary"
              shape="circle"
              v-show="this.formModel2.fields.endState=='未打卡'"
              @click="bukaDetail2"
            >补卡</Button>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="下午上班打卡时间">
            <Time-picker
              format="HH:mm"
              v-model="buka.pmstartTime"
              placeholder="选择时间"
              style="width: 100%"
              v-if="this.formModel2.fields.endState=='未打卡' && this.formModel2.fields.pmstartTime==null"
            ></Time-picker>
            <Input v-model="formModel2.fields.pmstartTime" :readonly="true" v-else />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="下午上班打卡地点">
            <Input
              type="textarea"
              v-model="buka.pmstartPlace"
              v-if="this.formModel2.fields.endState=='未打卡' && this.formModel2.fields.pmstartPlace==null"
            />
            <Input
              type="textarea"
              v-model="formModel2.fields.pmstartPlace"
              :readonly="true"
              v-else
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="下午下班打卡时间">
            <Time-picker
              format="HH:mm"
              editable
              v-model="buka.pmendTime"
              placeholder="选择时间"
              style="width: 100%"
              v-if="this.formModel2.fields.endState=='未打卡' && this.formModel2.fields.pmendTime==null"
            ></Time-picker>
            <Input v-model="formModel2.fields.pmendTime" :readonly="true" v-else />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="下午下班打卡地点">
            <Input
              type="textarea"
              v-model="buka.pmendPlace"
              v-if="this.formModel2.fields.endState=='未打卡' && this.formModel2.fields.pmendPlace==null"
            />
            <Input type="textarea" v-model="formModel2.fields.pmendPlace" :readonly="true" v-else />
          </FormItem>
        </Row>
      </Form>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getList,
  loadCarDriverDetail, //详情
  //deleteCarDriver, //右边删除
  //batchCommand, //右边上方删除
  //createCarDriver, //添加（保存）
  //editCarDriver, //编辑（保存）
  loadCarDriver, //加载
  EditAttend,
} from "@/api/job/postulant";

export default {
  name: "postulant",
  components: {
    DzTable
  },
  data() {
    return {
      formModel2: {
        opened: false,
        title: "详情",
        selection: [],
        fields: {
          attendanceUuid: "",
          loginName: "",
          startState: "",
          amstartTime: "",
          amstartPlace: "",
          amendTime: "",
          amendPlace: "",
          endState: "",
          pmstartTime: "",
          pmstartPlace: "",
          pmendTime: "",
          pmendPlace: "",
        },
      },
      statebuka: 0,
      endbuka: 0,
      buka: {
        attendanceUuid: "",
        startState: "",
        amstartTime: "",
        amstartPlace: "",
        amendTime: "",
        amendPlace: "",
        endState: "",
        pmstartTime: "",
        pmstartPlace: "",
        pmendTime: "",
        pmendPlace: "",
        satend: "",
      },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" }
      },
      //查询搜索
      stores: {
         indexId: "",
        index: [
          {
            indexId: 0,
            name: "全天"
          },
          {
            indexId: 1,
            name: "上午"
          },
          {
            indexId: 2,
            name: "下午"
          }
        ],
        job: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw2: "",
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
            // state: [
            //   { value: "", text: "全部" },
            //   { value: 1, text: "使用中" },
            //   { value: 2, text: "暂停使用" },
            // ]
          },
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "attendanceUUID" },
            { title: "微信名称", key: "loginName", sortable: true },
            { title: "上班状态", key: "startState" },
            { title: "上班打卡时间", key: "amstartTime" },
            { title: "下班状态", key: "endState" },
            { title: "下班打卡时间", key: "pmendTime" },
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
      }
    };
  },
  computed: {
    // formTitle() {
    //   if (this.formModel.mode === "create") {
    //     return "创建垃圾箱房申请";
    //   }
    //   if (this.formModel.mode === "edit") {
    //     return "编辑垃圾箱房信息";
    //   }
    //   return "";
    // },
    // selectedRows() {
    //   return this.formModel.selection;
    // },
    // selectedRowsId() {
    //   return this.formModel.selection.map(x => x.AttendanceUUID);
    // } //删除
  },
  methods: {
    //获取信息数据
    loadCarDispatchList() {
      getList(this.stores.job.query).then(res => {
        ////console.log(res.data.data)
        this.stores.job.data = res.data.data;
        ////console.log(res.data.data);
        this.stores.job.query.totalCount = res.data.totalCount;
      });
    },

    //翻页
    handlePageChanged(page) {
      this.stores.job.query.currentPage = page;
      this.loadCarDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.job.query.pageSize = pageSize;
      this.loadCarDispatchList();
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
    //非空验证提示
    // validateDispatchForm() {
    //   let _valid = false;
    //   this.$refs["formdispatch"].validate(valid => {
    //     if (!valid) {
    //       this.$Message.error("请完善表单信息");
    //     } else {
    //       _valid = true;
    //     }
    //   });
    //   return _valid;
    // },
    // doLoadCarDispatch(attendanceUUID) {
    //   //console.log(attendanceUUID)
    //   loadCarDriver({ guid: attendanceUUID }).then(res => {
    //     this.formModel2.fields = res.data.data;
    //     //console.log(res.data.data)
    //   });
    // },
    //清空
    // handleResetFormDispatch() {
    //   this.$refs["formdispatch"].resetFields();
    // },
    //搜索
    handleSearchDispatch() {
      this.loadCarDispatchList();
    },
    bukaDetail() {
      if (this.buka.amstartTime == null ||this.buka.amstartTime == "") {
          this.$Message.warning("上午上班时间不能为空");
          return;
      }
      if (this.buka.amstartPlace == null ||this.buka.amstartPlace == "") {
          this.$Message.warning("上午上班地点不能为空");
          return;
      }
      if (this.buka.amendTime == null ||this.buka.amendTime == "") {
          this.$Message.warning("上午下班时间不能为空");
          return;
      }
      if (this.buka.amendPlace == null ||this.buka.amendPlace == "") {
          this.$Message.warning("上午下班地点不能为空");
          return;
      }
      this.buka.satend = "0";
      EditAttend(this.buka).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadCarDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    bukaDetail2() {
      if (this.buka.pmstartTime == null ||this.buka.pmstartTime == "") {
          this.$Message.warning("下午上班时间不能为空");
          return;
      }
      if (this.buka.pmstartPlace == null ||this.buka.pmstartPlace == "") {
          this.$Message.warning("下午上班地点不能为空");
          return;
      }
      if (this.buka.pmendTime == null ||this.buka.pmendTime == "") {
          this.$Message.warning("下午下班时间不能为空");
          return;
      }
      if (this.buka.pmendPlace == null ||this.buka.pmendPlace == "") {
          this.$Message.warning("下午下班地点不能为空");
          return;
      }
      this.buka.satend = "1";
      EditAttend(this.buka).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadCarDispatchList();
          this.formModel2.opened=false;
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    dakatime() {
      if (this.stores.indexId == 0) {
        this.stores.job.columns = [
          { type: "selection", width: 50, key: "attendanceUUID" },
          { title: "微信名称", key: "loginName", sortable: true },
          { title: "上班状态", key: "startState" },
          { title: "上班打卡时间", key: "amstartTime" },
          { title: "下班状态", key: "endState" },
          { title: "下班打卡时间", key: "pmendTime" },
          {
            title: "操作",
            align: "center",
            width: 150,
            className: "table-command-column",
            slot: "action"
          }
        ];
        this.loadCarDispatchList();
      } else if (this.stores.indexId == 1) {
        this.stores.job.columns = [
          { type: "selection", width: 50, key: "attendanceUUID" },
          { title: "微信名称", key: "loginName", sortable: true },
          { title: "上班打卡时间", key: "amstartTime" },
          { title: "下班打卡时间", key: "amendTime" },
          {
            title: "操作",
            align: "center",
            width: 150,
            className: "table-command-column",
            slot: "action"
          }
        ];
        this.loadCarDispatchList();
      } else {
        this.stores.job.columns = [
          { type: "selection", width: 50, key: "attendanceUUID" },
          { title: "微信名称", key: "loginName", sortable: true },
          { title: "上班打卡时间", key: "pmstartTime" },
          { title: "下班打卡时间", key: "pmendTime" },
          {
            title: "操作",
            align: "center",
            width: 150,
            className: "table-command-column",
            slot: "action"
          }
        ];
        this.loadCarDispatchList();
      }
    },
    //刷新
    handleRefresh() {
      this.loadCarDispatchList();
    },
    //右边删除（单个删除）
    // handleDelete(row) {
    //   this.doDelete(row.AttendanceUUID);
    // },
    // doDelete(ids) {
    //   if (!ids) {
    //     this.$Message.warning("请选择至少一条数据");
    //     return;
    //   }
    // deleteCarDriver(ids).then(res => {
    //   if (res.data.code === 200) {
    //     this.$Message.success(res.data.message);
    //     this.loadCarDispatchList();
    //     this.formModel.selection = [];
    //   } else {
    //     this.$Message.warning(res.data.message);
    //   }
    // });
    // },
    //右上边删除按钮
    // handleBatchCommand(command) {
    //   if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
    //     this.$Message.warning("请选择至少一条数据");
    //     return;
    //   }
    //   this.$Modal.confirm({
    //     title: "操作提示",
    //     content:
    //       "<p>确定要执行当前 [" +
    //       this.commands[command].title +
    //       "] 操作吗?</p>",
    //     loading: true,
    //     onOk: () => {
    //       this.doBatchCommand(command);
    //     }
    //   });
    // },
    //右上边删除
    // doBatchCommand(command) {
    //   batchCommand({
    //     command: command,
    //     ids: this.selectedRowsId.join(",")
    //   }).then(res => {
    //     if (res.data.code === 200) {
    //       this.$Message.success(res.data.message);
    //       this.loadCarDispatchList();
    //       this.formModel.selection = [];
    //     } else {
    //       this.$Message.warning(res.data.message);
    //     }
    //     this.$Modal.remove();
    //   });
    // },
    //右边编辑
    // handleEdit(row) {
    //   this.handleOpenFormWindow();
    //   this.handleSwitchFormModeToEdit();
    //   this.handleResetFormDispatch();
    //   //console.log(row)
    //   this.doLoadCarDispatch(row.garbageRoomUuid);
    //   //console.log(row.garbageRoomUuid);
    // },
    // handleSwitchFormModeToEdit() {
    //   this.formModel.mode = "edit";
    // },
    // handleSelect(selection, row) {},
    // handleSelectionChange(selection) {
    //   this.formModel.selection = selection;
    // },
    // handleSortChange(column) {
    //   this.stores.user.query.sort.direction = column.order;
    //   this.stores.user.query.sort.field = column.key;
    //   this.loadCarDispatchList();
    // },
    //右边详情
    handleDetail(row) {
      ////console.log(row);
      this.formModel2.opened = true;
      this.$refs["formCarDispatchDetail"].resetFields();
      loadCarDriverDetail({ guid: row.attendanceUuid }).then((res) => {
        this.formModel2.fields = res.data.data;
        this.buka.startState = this.formModel2.fields.startState;
        this.buka.attendanceUuid = this.formModel2.fields.attendanceUuid;
        this.buka.amstartTime = this.formModel2.fields.amstartTime;
        this.buka.amstartPlace = this.formModel2.fields.amstartPlace;
        this.buka.amendTime = this.formModel2.fields.amendTime;
        this.buka.amendPlace = this.formModel2.fields.amendPlace;
        this.buka.endState = this.formModel2.fields.endState;
        this.buka.pmstartTime = this.formModel2.fields.pmstartTime;
        this.buka.pmstartPlace = this.formModel2.fields.pmstartPlace;
        this.buka.pmendTime = this.formModel2.fields.pmendTime;
        this.buka.pmendPlace = this.formModel2.fields.pmendPlace;
      });
    }
    //添加按钮（志愿者申请）
    // handleShowCreateWindow() {
    //   this.handleSwitchFormModeToCreate();
    //   this.handleOpenFormWindow(); //打开窗口
    //   this.handleResetFormDispatch(); //清空表单
    //   this.formModel.fields.loginName = this.$store.state.user.userName;
    //    //社区
    //    this.formModel.fields.vname = this.$store.state.user.vname;
    //   this.formModel.fields.villageUuid = this.stores.car.sources.college[1][
    //     "villageUuid"
    //   ];
    //    //督导员下拉框
    //    this.formModel.fields.loginName = this.$store.state.user.loginName;
    //   this.formModel.fields.systemUserUuid = this.stores.car.sources.supervisor[1][
    //     "systemUserUuid"
    //   ];
    //    //收运车辆下拉框
    //    this.formModel.fields.carNum = this.$store.state.user.carNum;
    //   this.formModel.fields.carUuid = this.stores.car.sources.vehicle[1][
    //     "carUuid"
    //   ];
    // },
    // handleSwitchFormModeToCreate() {
    //   this.formModel.mode = "create";
    // },
    //添加志愿者（保存）
    // docreateCarDispatch() {
    //   ////console.log(this.formModel.fields);
    //   createCarDriver(this.formModel.fields).then(res => {
    //     if (res.data.code === 200) {
    //       this.$Message.success(res.data.message);
    //       this.handleCloseFormWindow(); //关闭表单
    //       this.loadCarDispatchList();
    //     } else {
    //       this.$Message.warning(res.data.message);
    //     }
    //   });
    // },
    //编辑志愿者（保存）
    // doEditCarDispatch() {
    //   editCarDriver(this.formModel.fields).then(res => {
    //     //console.log(this.formModel.fields)
    //     if (res.data.code === 200) {
    //       this.$Message.success(res.data.message);
    //       this.handleCloseFormWindow();
    //       this.loadCarDispatchList();
    //     } else {
    //       this.$Message.warning(res.data.message);
    //     }
    //   });
    // },
    //保存按钮
    // handleSubmitConsumable() {
    //   let valid = this.validateDispatchForm();
    //   if (valid) {
    //     if (this.formModel.mode === "create") {
    //       this.docreateCarDispatch();
    //     }
    //     if (this.formModel.mode === "edit") {
    //       this.doEditCarDispatch();
    //     }
    //   }
    // },
    //提交按钮
    // handleSubmitToAuditDispatch() {
    //   let valid = this.validateDispatchForm();
    //   if (valid) {
    //     if (this.formModel.mode === "create") {
    //       this.doCreateToAuditCarDispatch();
    //     }
    //     if (this.formModel.mode === "edit") {
    //       this.doEditToAuditCarDispatch();
    //     }
    //   }
    // }
  },
  mounted() {
    //  this.loadDepartmentList1();//社区下拉框
    this.loadCarDispatchList();
    //  this.loadDepartmentList2();//督导员下拉框
    //  this.loadDepartmentList3();//收运车辆下拉框
  }
};
</script>
<style>
</style>
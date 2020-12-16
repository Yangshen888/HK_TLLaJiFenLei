<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.monitoring.query.totalCount"
        :pageSize="stores.monitoring.query.pageSize"
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
                      v-model="stores.monitoring.query.kw"
                      placeholder="请输入车牌号码"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                    <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加车辆监控信息"
                >添加车辆监控信息</Button>
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
          :data="stores.monitoring.data"
          :columns="stores.monitoring.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <!-- <template slot-scope="{row, index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>-->
          <template slot-scope="{ row, index }" slot="action">      
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
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
            <Tooltip placement="top" content="实时查看" :delay="1000" :transfer="true">
              <Button
                v-can="'show1'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail2(row)"
                >实时查看</Button
              >
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formModel.fields"
        ref="formdispatch"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="监控编号" prop="monitoringNum">
            <Input v-model="formModel.fields.monitoringNum" placeholder="请输入监控编号" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="车牌号" prop="carUuid">
            <Select v-model="formModel.fields.carUuid" @on-change="xz">
              <Option
                v-for="item in this.formModel.sources"
                :value="item.carUuid"
                :key="item.carUuid"
              >{{item.carNum}}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="PalyType" prop="palyType">
            <Input v-model="formModel.fields.palyType" placeholder="请输入PalyType" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="SvrIp" prop="svrIp">
            <Input v-model="formModel.fields.svrIp" placeholder="请输入SvrIp" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="SvrPort" prop="svrPort">
            <Input v-model="formModel.fields.svrPort" placeholder="请输入SvrPort" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="appkey" prop="appkey">
            <Input v-model="formModel.fields.appkey" placeholder="请输入appkey" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="appSecret" prop="appSecret">
            <Input v-model="formModel.fields.appSecret" placeholder="请输入appSecret" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="time" prop="time">
            <Input v-model="formModel.fields.time" placeholder="请输入time" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="timeSecret" prop="timeSecret">
            <Input v-model="formModel.fields.timeSecret" placeholder="请输入timeSecret" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="httpsflag" prop="httpsflag">
            <Input v-model="formModel.fields.httpsflag" placeholder="请输入httpsflag" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="CamList" prop="camList">
            <Input v-model="formModel.fields.camList" placeholder="请输入CamList" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" prop="remark">
            <Input v-model="formModel.fields.remark" placeholder="请输入备注" />
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer title="详情" v-model="formModel2.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel2.fields" ref="formCarDispatchDetail" label-position="left">
        <Row :gutter="16">
          <FormItem label="监控编号">
            <Input v-model="formModel2.fields.monitoringNum" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="车牌号">
            <Input v-model="formModel2.fields.carNum" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="PalyType">
            <Input v-model="formModel2.fields.palyType" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="SvrIp">
            <Input v-model="formModel2.fields.svrIp" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="SvrPort">
            <Input v-model="formModel2.fields.svrPort" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="appkey">
            <Input v-model="formModel2.fields.appkey" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="appSecret">
            <Input v-model="formModel2.fields.appSecret" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="time">
            <Input v-model="formModel2.fields.time" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="timeSecret">
            <Input v-model="formModel2.fields.timeSecret" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="httpsflag">
            <Input v-model="formModel2.fields.httpsflag" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="CamList">
            <Input v-model="formModel2.fields.camList" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注">
            <Input v-model="formModel2.fields.remark" :readonly="true" />
          </FormItem>
        </Row>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel2.opened = false">取 消</Button>
      </Form>
    </Drawer>
    <Drawer
      title="实时查看"
      v-model="formModel3.opened"
      width="1000"
      :mask-closable="false"
      :mask="false"
    >
      <Form
        :model="formModel3.fields"
        ref="formCarDispatchDetail"
        label-position="left"
      >
        <div>
          <iframe
            name="iframeMap"
            id="iframeMapViewComponent"
            width="100%"
            height="800px"
            v-bind:src="smgHtmlPath"
            frameborder="0"
            scrolling="no"
            ref="iframeDom"
          ></iframe>
        </div>
      </Form>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getCarList, //显示列表
  AddCarMonit, //新增车辆监控
  CarNums, //未绑车辆下拉框
  GetCarMonit, //获取选定车辆监控编辑信息
  EditCarMonit, //编辑车辆监控信息
    batchCommand//右边上方删除
} from "@/api/monitoring/monitoring";
export default {
  name: "monitoring",
  components: {
    DzTable
  },
  data() {
    let IsnullCarNumber = (rule, value, callback) => {
      if (value == undefined) {
        callback(new Error("车牌号不能为空"));
      } else if (value !== "" && value !== null) {
        callback();
      } else {
        callback(new Error("车牌号不能为空"));
      }
      callback();
    };
    return {
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" }
      },
      monitoringCarNum: "",
      smgHtmlPath:'',
      //查询搜索
      stores: {
        monitoring: {
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
            { type: "selection", width: 50, key: "carMonitoringUuid" },
            // { title: "监控编号", key: "monitoringNum", sortable: true },
            { title: "所属车辆", key: "carNum", sortable: true },
            { title: "备注", key: "remark" },
            {
              title: "操作",
              align: "center",
              width: 200,
              className: "table-command-column",
              slot: "action"
            }
          ],
          data: []
        }
      },
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        selection: [],
        fields: {
          monitoringNum: "",
          carUuid: "",
          palyType: "",
          carMonitoringUuid:"",
          svrIp: "",
          svrPort: "",
          appkey: "",
          appSecret: "",
          time: "",
          timeSecret: "",
          httpsflag: "",
          camList: "",
          remark: ""
        },
        carNum: "",
        sources: [{ carUuid: "0", carNum: "请选择" }],
        rules: {
          monitoringNum: [
            { type: "string", required: true, message: "请输入监控编号" }
          ],
          carUuid: [
            { type: "string", required: true, validator: IsnullCarNumber }
          ]
        }
      },
      formModel2: {
        opened: false,
        title: "详情",
        selection: [],
        fields: {
          monitoringNum: "",
          carUuid: "",
          palyType: "",
          svrIp: "",
          svrPort: "",
          appkey: "",
          appSecret: "",
          time: "",
          timeSecret: "",
          httpsflag: "",
          camList: "",
          remark: "",
          addTime: "",
          carNum:"",
        }
      },
      formModel3: {
        opened: false,
        title: "实时查看",
        selection: []
      }
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "新增车辆监控信息";
      }
      if (this.formModel.mode === "edit") {
        return "编辑车辆监控信息";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.carMonitoringUuid);
    } //删除
  },
  methods: {
    //获取信息数据
    loadCarDispatchList() {
      getCarList(this.stores.monitoring.query).then(res => {
        this.stores.monitoring.data = res.data.data;
        this.stores.monitoring.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    //多选
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //翻页
    handlePageChanged(page) {
      this.stores.monitoring.query.currentPage = page;
      this.loadCarDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.monitoring.query.pageSize = pageSize;
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
    //打开窗口
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    //自动关闭窗口
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatch"].resetFields();
    },
    //右边编辑
    handleEdit(row) {
      this.handleOpenFormWindow();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormDispatch();
      this.doLoadShopEditData(row.carMonitoringUuid);
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
    },
    //右边详情
    handleDetail(row) {
      this.formModel2.opened = true;
      this.doLoadShopEditData(row.carMonitoringUuid);
    },
    //查询当前行信息
    doLoadShopEditData(id) {
      //console.log(id);
      GetCarMonit(id).then(res => {
        //console.log(res.data.data);
        if (res.data.code === 200) {
          this.formModel.fields = res.data.data[0];
          this.loadDepartmentList1();
          this.formModel2.fields = res.data.data[0];
        }
      });
    },
    //添加按钮（车辆监控）
    handleShowCreateWindow() {
      this.loadDepartmentList1();
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    //获取车辆下拉框
    loadDepartmentList1() {
      CarNums().then(res => {
        this.formModel.sources = res.data.data;
      });
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
    //选择车辆
    xz(e) {
      this.formModel.fields.CarUUID = e;
    },
    //添加车辆监控（保存）
    docreateCarDispatch() {
      AddCarMonit(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow(); //关闭表单
          this.loadCarDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑车辆监控（保存）
      doEditCarDispatch() {
        EditCarMonit(this.formModel.fields).then(res => {
          //console.log(this.formModel.fields)
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.handleCloseFormWindow();
            this.loadCarDispatchList();
          } else {
            this.$Message.warning(res.data.message);
          }
        });
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
    //实时查看
    handleDetail2(row) {
      this.formModel3.opened = true;
      console.log(row);
      this.smgHtmlPath='http://gps.zwlbs.cn/clbs/v/monitoring/forward/realtime?key=EFAD60B7A0EC7016EE4AFC9B3CE5D52E&VehPlateNum='+row.monitoringNum+'&Channel=&VehPlateColorCode=';
    },
  },
  mounted() {
    this.loadCarDispatchList();
    this.loadDepartmentList1();//车辆下拉框
    this.iframeWin = this.$refs.iframeDom.contentWindow;
  }
};
</script>
<style>
</style>
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
                  <!-- <FormItem>    
                    <Select
                      v-model="stores.monitoring.query.street"
                      style="width:150px"
                      @on-change="xz1"
                      clearable
                      placeholder="请选择街道"
                    >
                      <Option
                        v-for="item in this.stores.monitoring.sources.town"
                        :value="item"
                        :key="item"
                      >{{ item }}</Option>
                    </Select>
                    <Select
                      v-model="stores.monitoring.query.ccmmunity"
                      style="width:150px"
                      @on-change="sq"
                      clearable
                      placeholder="请选择社区"
                    >
                      <Option
                        v-for="item in this.stores.monitoring.sources.college"
                        :value="item"
                        :key="item"
                      >{{ item }}</Option>
                    </Select>
                  </FormItem> -->
                  <FormItem>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.monitoring.query.kw"
                      placeholder="请输入监控位置"
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
                  <Button
                    v-can="'import'"
                    icon="ios-cloud-upload"
                    type="success"
                    @click="handleImportUserInfo"
                    title="导入地址信息"
                  >导入地址信息</Button>
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <!-- <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加箱房监控信息"
                >添加箱房监控信息</Button>-->
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
            <!-- <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail(row)"
              ></Button>
            </Tooltip>-->
            <Tooltip placement="top" content="实时查看" :delay="1000" :transfer="true">
              <Button
                v-can="'show1'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail2(row)"
              >实时查看</Button>
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
          <FormItem label="箱房" prop="garbageRoomUuid">
            <Select v-model="formModel.fields.garbageRoomUuid" @on-change="xz">
              <Option
                v-for="item in this.formModel.sources"
                :value="item.garbageRoomUuid"
                :key="item.garbageRoomUuid"
              >{{item.ljname}}</Option>
            </Select>
          </FormItem>
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="PalyType" prop="palyType">
            <Input v-model="formModel.fields.palyType" placeholder="请输入PalyType" />
          </FormItem>
        </Row> -->
        <!-- <Row :gutter="16">
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
        </Row> -->
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
      <Form :model="formModel2.fields" ref="formGrabageDispatchDetail" label-position="left">
        <Row :gutter="16">
          <FormItem label="监控编号">
            <Input v-model="formModel2.fields.monitoringNum" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="箱房">
            <Input v-model="formModel2.fields.ljname" :readonly="true" />
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
      <video style="width: 900px;height: 500px" ref="video" controls></video>
<!--      <Form :model="formModel3.fields" ref="formGrabageDispatchDetail" label-position="left"></Form>-->
    </Drawer>
    <Drawer
      title="地址信息导入"
      v-model="formimport.opened"
      width="600"
      :mask-closable="true"
      :mask="true"
    >
      <div>
        <input
          ref="inputer"
          id="upload"
          type="file"
          @change="importfxx"
          accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
        />
        <Button
          v-can="'model'"
          icon="ios-cloud-download"
          type="warning"
          @click="handleimportmodel"
          title="箱房监控信息导入模板"
        >箱房监控信息导入模板</Button>
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleimport"
          :disabled="importdisable"
        >导入</Button>
        <!-- <div>
           <Col class="demo-spin-col" span="8">
            <Spin fix>
                <Icon type="ios-loading" size=18 class="demo-spin-icon-load"></Icon>
                <div>Loading</div>
            </Spin>
        </Col>
        </div>-->
        <Tabs value="name1">
          <TabPane label="成功" name="name1" v-html="successmsg"></TabPane>
          <TabPane label="重复" name="name2" v-html="repeatmsg"></TabPane>
          <TabPane label="失败" name="name3" v-html="defaultmsg"></TabPane>
        </Tabs>
      </div>
    </Drawer>
  </div>
</template>
<script>
import JSZip from "jszip";
import FileSaver from "file-saver";
import DzTable from "_c/tables/dz-table.vue";
import {
  shopInfoImport, //导入
  getGrabageList, //显示列表
  AddGrabageMonit, //新增箱房监控
  GrabageNums, //未绑箱房下拉框
  GetGrabageMonit, //获取选定箱房监控编辑信息
  EditGrabageMonit, //编辑箱房监控信息
  batchCommand //右边上方删除
} from "@/api/monitoring/gmonitoring";
import {
  getshequ
} from "@/api/base/house";
import config from "@/config";
let Hls=require('hls.js');
export default {
  name: "monitoring",
  components: {
    DzTable
  },
  data() {
    let Isnullljnameber = (rule, value, callback) => {
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
        hls:'',
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" }
      },

      //导入
      url: config.baseUrl.dev,
      importdisable: false,
      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      formimport: {
        opened: false
      },

      list33: [],
      //查询搜索
      stores: {
        monitoring: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            vuuid: this.$store.state.user.villageGuid,
            street:"",
            ccmmunity:"",
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
            //社区集合
            college: ["全部" ],     
             //街道集合
            town: ["全部" ],  },
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "grabageMonitoringUuid" },
            { title: "监控编号", key: "monitoringNum", sortable: true },
            { title: "监控位置", key: "jiankongName", sortable: true,
              ellipsis: true,
              tooltip: true  },
            { title: "所属箱房", key: "ljname" },
            //{ title: "备注", key: "remark" },
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
          garbageRoomUuid: "",
          palyType: "",
          grabageMonitoringUuid: "",
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
        ljname: "",
        sources: [{ garbageRoomUuid: "0", ljname: "请选择" }],
        rules: {
          monitoringNum: [
            { type: "string", required: true, message: "请输入监控编号" }
          ],
          garbageRoomUuid: [
            { type: "string", required: true, validator: Isnullljnameber }
          ]
        }
      },
      formModel2: {
        opened: false,
        title: "详情",
        selection: [],
        fields: {
          monitoringNum: "",
          garbageRoomUuid: "",
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
          ljname: ""
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
        return "新增箱房监控信息";
      }
      if (this.formModel.mode === "edit") {
        return "编辑箱房监控信息";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.grabageMonitoringUuid);
    } //删除
  },
  methods: {
      //监控
      videoPause() {
          if (this.hls) {
              this.$refs.video.pause();
              this.hls.destroy();
              this.hls = null;
          }
      },
      getStream(source) {
          if (Hls.isSupported()) {
              this.hls = new Hls();
              console.log(source)
              this.hls.loadSource(source);
              console.log(this.hls)
              this.hls.attachMedia(this.$refs.video);
              this.hls.on(Hls.Events.MANIFEST_PARSED, () => {
                  console.log('加载成功');
                  this.$refs.video.play();
              });
              this.hls.on(Hls.Events.ERROR, (event, data) => {
                   console.log(event, data);
                  // 监听出错事件
                  console.log('加载失败');
              });
          }
      },
      beforeDestroy() {
          this.videoPause();
      },

    //获取信息数据
    loadGrabageDispatchList() {
      getGrabageList(this.stores.monitoring.query).then(res => {
        this.stores.monitoring.data = res.data.data;
        this.stores.monitoring.query.totalCount = res.data.totalCount;
      });
    },

    //导入相关操作
    handleImportUserInfo() {
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      this.$refs.inputer.value = "";
      this.isexitexcel = false;
      this.formimport.opened = true;
    },
    handleimportmodel() {
      ////console.log(this.url);
      window.location.href =
        this.url + "UploadFiles/ImportUserInfoModel/箱房监控信息导入模板.xlsx";
    },
    //导入
    importfxx(e) {
      let inputDOM = this.$refs.inputer;
      ////console.log(inputDOM);
      //声明一个FormDate对象
      let formData = new FormData();
      let file = inputDOM.files[0];
      let AllUpExt = ".xls|.xlsx|";
      let extName = file.name
        .substring(file.name.lastIndexOf("."))
        .toLowerCase();
      if (AllUpExt.indexOf(extName + "|") == "-1") {
        this.$refs.inputer.value = "";
        this.$Message.warning("文件格式不正确!");
      } else {
        if (file != null && file != undefined) {
          this.isexitexcel = true;
          formData.append("excelFile", file);
          // //console.log(file);
          this.exceldata = formData;
        } else {
          this.isexitexcel = false;
        }
      }
      // //console.log(this.exceldata);
    },
    async handleimport() {
      this.importdisable = true;
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      // //console.log(111);
      if (this.isexitexcel) {
        await shopInfoImport(this.exceldata).then(res => {
          // //console.log(res.data);
          if (res.data.code == 200) {
            this.time = res.data.data.time;
            this.successmsg = res.data.data.successmsg;
            this.repeatmsg = res.data.data.repeatmsg;
            this.defaultmsg = res.data.data.defaultmsg;
            this.loadGrabageDispatchList();
          } else {
            this.$Message.warning(res.data.message);
            //clearInterval(this.intervalId);
            //this.showprogress = false;
          }
          this.$refs.inputer.value = "";
          this.exceldata = new FormData();
          this.isexitexcel = false;
        });
        //clearInterval(this.intervalId);
        //this.showprogress = false;
      } else {
        this.$Message.warning("请选择文件");
      }
      this.importdisable = false;
    },
    handleSelect(selection, row) {},
    //多选
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //翻页
    handlePageChanged(page) {
      this.stores.monitoring.query.currentPage = page;
      this.loadGrabageDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.monitoring.query.pageSize = pageSize;
      this.loadGrabageDispatchList();
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
      this.loadGrabageDispatchList();
    },
    //刷新
    handleRefresh() {
      this.loadGrabageDispatchList();
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
          this.loadGrabageDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
       //申请社区下拉框
     loadDepartmentList11() {
       let data=this.$store.state.user.villageGuid;
       getshequ(data).then(res => {
         this.list33 = res.data.data;
         let townn = Array.from(new Set(this.list33.map(x => x.towns )));
        this.stores.monitoring.sources.town=townn;  
      });
      
    },
    xz1(e){     
      let college = this.list33.filter(x => x.towns == e);
      this.stores.monitoring.sources.college=college.map(x => x.vname);    
      this.loadGrabageDispatchList();                  
    },
     sq(e) {
      if (e == "全部") {
        this.stores.monitoring.query.ccmmunity = "";
      }
      this.loadGrabageDispatchList();  
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
      this.doLoadShopEditData(row.grabageMonitoringUuid);
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
    },
    //右边详情
    handleDetail(row) {
      this.formModel2.opened = true;
      this.doLoadShopEditData(row.grabageMonitoringUuid);
    },
    //查询当前行信息
    doLoadShopEditData(id) {
      GetGrabageMonit(id).then(res => {
        if (res.data.code === 200) {
          this.formModel.fields = res.data.data[0];
          this.loadDepartmentList1();
        }
      });
    },
    //添加按钮（箱房监控）
    handleShowCreateWindow() {
      this.loadDepartmentList1();
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    //获取箱房下拉框
    loadDepartmentList1() {
      GrabageNums().then(res => {
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
    //选择箱房
    xz(e) {
      this.formModel.fields.garbageRoomUuid = e;
    },
    //添加箱房监控（保存）
    docreateGrabageDispatch() {
      AddGrabageMonit(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow(); //关闭表单
          this.loadGrabageDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑箱房监控（保存）
    doEditGrabageDispatch() {
      EditGrabageMonit(this.formModel.fields).then(res => {
        //console.log(this.formModel.fields)
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadGrabageDispatchList();
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
          this.docreateGrabageDispatch();
        }
        if (this.formModel.mode === "edit") {
          this.doEditGrabageDispatch();
        }
      }
    },
    //实时查看
    handleDetail2(row) {
      this.formModel3.opened = true;

        this.getStream(row.videoUrl);
    }
  },
  mounted() {
    this.loadDepartmentList11();
    this.loadGrabageDispatchList();
    this.loadDepartmentList1(); //箱房下拉框
      //监控



  }
};
</script>
<style>
</style>

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
                      placeholder="输入车牌号码"
                      @on-search="handleSearchDispatch()"
                    ></Input> 
                  </FormItem>     
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                </ButtonGroup>
                 <Button
                  v-can="'import'"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleImportUserInfo"
                  title="导入车辆信息"
                >导入</Button>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加车辆信息"
                >添加车辆信息</Button>
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
            <Tooltip
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
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="420"
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
            <FormItem label="车牌号" prop="carNum">
              <Input v-model="formModel.fields.carNum" placeholder="请输入车牌号"/>
            </FormItem>
        </Row>  
        <Row :gutter="16">
            <FormItem label="车辆类型" prop="carType">
              <Input v-model="formModel.fields.carType" placeholder="请输入车辆类型"/>
            </FormItem>
        </Row> 
        <Row :gutter="16">
            <FormItem label="持有人" prop="holderId">
              <Input v-model="formModel.fields.holderId" placeholder="请输入持有人"/>
            </FormItem>
        </Row>    
        <Row :gutter="16">
            <FormItem label="持有人电话" prop="holderPhone">
              <Input v-model="formModel.fields.holderPhone" placeholder="请输入持有人电话"/>
            </FormItem>
        </Row> 
        <Row :gutter="16">
            <FormItem label="品牌型号" prop="brand">
              <Input v-model="formModel.fields.brand" placeholder="请输入品牌型号"/>
            </FormItem>
        </Row>  
        <Row :gutter="16">
            <FormItem label="所属公司" prop="company">
              <Input v-model="formModel.fields.company" placeholder="请输入所属公司"/>
            </FormItem>
        </Row> 
        <Row :gutter="16">
            <FormItem label="所属街道" prop="street">
            <Select
              v-model="formModel.fields.street"
              style="width:100%"
              clearable
              placeholder="请选择街道"
            >
              <Option
                v-for="item in stores.car.streetList"
                :value="item"
                :key="item"
              >{{ item }}</Option>
            </Select>
            </FormItem>
        </Row> 
        <Row :gutter="16">
          <FormItem label="注册日期" prop="registerTime">
            <DatePicker
              v-model="formModel.fields.registerTime"
              @on-change="formModel.fields.registerTime=$event"
              format="yyyy-MM-dd"
              type="datetime"
              style="width:380px"
            ></DatePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="发证日期" prop="awardTime">
            <DatePicker
              v-model="formModel.fields.awardTime"
              @on-change="formModel.fields.awardTime=$event"
              format="yyyy-MM-dd"
              type="datetime"
              style="width:380px"
            ></DatePicker>
          </FormItem>
        </Row>
        <!-- <Row :gutter="16">
            <FormItem label="收运类型" prop="grabType">
              <Input v-model="formModel.fields.grabType" placeholder="请输入收运类型"/>
            </FormItem>
        </Row> -->
        <Row :gutter="16">
          <FormItem label="收运类型">
            <Select
              v-model="formModel.fields.grabType"
              style="width:100%;"
              placeholder="收运类型"
            >
              <Option
                v-for="item in stores.car.cityList"
                :value="item.value"
                :key="item.value"
              >{{ item.label }}</Option>
            </Select>
          </FormItem>
        </Row>                  
        <Row :gutter="16">
            <FormItem label="车辆重量" prop="weight">
              <Input v-model="formModel.fields.weight" placeholder="请输入车辆重量"/>
            </FormItem>
        </Row>  
        <Row :gutter="16">
            <FormItem label="称重设备ID" prop="balanceId">
              <Input v-model="formModel.fields.balanceId" placeholder="请输入称重设备ID"/>
            </FormItem>
        </Row>     
        <Row :gutter="16">
            <FormItem label="车载监控编号" prop="cameraId">
              <Input v-model="formModel.fields.cameraId" placeholder="请输入车载监控编号"/>
            </FormItem>
        </Row> 
        <Row :gutter="16">
            <FormItem label="车载唯一编码" prop="onlyNum">
              <Input v-model="formModel.fields.onlyNum" placeholder="请输入车载唯一编码"/>
            </FormItem>
        </Row> 

      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>

     <Drawer
      title="车辆信息导入"
      v-model="formimport.opened"
      width="500"
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
                  title="车辆信息导入模板下载"
                >车辆信息导入模板下载</Button>
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleimport"
          :disabled="importdisable"
        >导入</Button>

        <Tabs value="name1">
          <TabPane label="成功" name="name1" v-html="successmsg"></TabPane>
          <TabPane label="重复" name="name2" v-html="repeatmsg"></TabPane>
          <TabPane label="失败" name="name3" v-html="defaultmsg"></TabPane>
        </Tabs>
      </div>
    </Drawer>

    <Drawer title="详情" v-model="formModel2.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel2.fields" ref="formCarDispatchDetail" label-position="left">    
        <Row :gutter="16">
            <FormItem label="车牌号">
              <Input v-model="formModel2.fields.carNum" :readonly="true" />
            </FormItem>
        </Row>  
        <Row :gutter="16">
            <FormItem label="车辆类型">
              <Input v-model="formModel2.fields.carType" :readonly="true" />
            </FormItem>
        </Row> 
        <Row :gutter="16">
            <FormItem label="持有人">
              <Input v-model="formModel2.fields.holderId" :readonly="true" />
            </FormItem>
        </Row>    
        <Row :gutter="16">
            <FormItem label="持有人电话">
              <Input v-model="formModel2.fields.holderPhone" :readonly="true" />
            </FormItem>
        </Row>  
         <Row :gutter="16">
            <FormItem label="品牌型号">
              <Input v-model="formModel2.fields.brand" :readonly="true"/>
            </FormItem>
        </Row>  
         <Row :gutter="16">
            <FormItem label="所属公司">
              <Input v-model="formModel2.fields.company" :readonly="true"/>
            </FormItem>
        </Row>  
        <Row :gutter="16">
            <FormItem label="所属街道">
              <Input v-model="formModel2.fields.street" :readonly="true" />
            </FormItem>
        </Row>  
        <Row :gutter="16">
            <FormItem label="注册日期">
              <Input v-model="formModel2.fields.registerTime" :readonly="true" />
            </FormItem>
        </Row>  
        <Row :gutter="16">
            <FormItem label="发证日期">
              <Input v-model="formModel2.fields.awardTime" :readonly="true" />
            </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="收运类型">
              <Input v-model="formModel2.fields.grabType" :readonly="true" />
            </FormItem>
        </Row>            
        <Row :gutter="16">
            <FormItem label="车辆重量">
              <Input v-model="formModel2.fields.weight" :readonly="true" />
            </FormItem>
        </Row>  
        <Row :gutter="16">
            <FormItem label="称重设备ID">
              <Input v-model="formModel2.fields.balanceId" :readonly="true" />
            </FormItem>
        </Row>     
        <Row :gutter="16">
            <FormItem label="车载监控编号">
              <Input v-model="formModel2.fields.cameraId" :readonly="true" />
            </FormItem>
        </Row>  
        <Row :gutter="16">
            <FormItem label="车载唯一编码">
              <Input v-model="formModel2.fields.onlyNum" :readonly="true" />
            </FormItem>
        </Row>  
      </Form>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getCarDriverList,
  loadCarDriverDetail, //详情
  deleteCarDriver, //右边删除
  batchCommand, //右边上方删除
  createCarDriver, //添加（保存）
  editCarDriver, //编辑（保存）
  loadCarDriver, //加载
  globalvalidatePhone,
  UserInfoImport
} from "@/api/base/vehicle";
import { getshequ } from "@/api/base/house";

import config from "@/config";

export default {
  name: "vehicle",
  components: {
    DzTable
  },
  data() {
   let globalvalidatePhone = (rule, value, callback) => {
    if (value !== '' && value !== null) {
      var reg = /^1[3456789]\d{9}$/;
      if (!reg.test(value)) {
        callback(new Error('请输入有效的电话号码'));
      }
      callback();
    } else {
      callback(new Error('电话号码不能为空'));
    }
    callback();
    };
     let globalvalidateRegisterTime = (rule, value, callback) => {
    if (value !== '' && value !== null) {
     callback();
    } else {
    callback(new Error('注册日期不能为空'));
    }
    callback();
   };
     let globalvalidateAwardTime = (rule, value, callback) => {
    if (value !== '' && value !== null) {
     callback();
    } 
    else {
    callback(new Error('发证日期不能为空'));
    }
    callback();
   };
    return {
      //导入
      url: config.baseUrl.dev,
      importdisable: false,
      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      formimport: {
        opened: false
      },
      formModel: {
        opened: false,    
        title: "创建申请",
        mode: "create",
        selection: [],
        fields: {
          carNum:"",
          carType:"",
          holderId:"",
          holderPhone:"",
          registerTime:"",
          awardTime:"",
          weight:"",
          balanceId:"",
          cameraId:"",
          typeName:"",
          brand:"",
          grabType:"",
          isDelete: 0,
          company:"",
          street:"",
          onlyNum:""
        },
        rules: {
          carNum: [
            { type: "string", required: true, message: "请输入车牌号" }
          ], 
          //  holderId: [
          //   { type: "string", required: true, message: "请输入持有人" }
          // ], 
          // brand:[
          //     { type: "string", required: true, message: "请输入品牌型号"}
          // ],
          // carType:[
          //     { type: "string", required: true, message: "请输入车辆类型"}
          // ],
          // registerTime:[
          //     { type: "string", validator: globalvalidateRegisterTime}
          // ],
          // awardTime:[
          //     { type: "string",  validator:globalvalidateAwardTime}
          // ],
          //  holderPhone: [
          //   { type: "string", required: true, validator:globalvalidatePhone}
          // ],
          //  balanceId: [
          //   { type: "string", required: true, message: "请输入称重设备ID"}
          // ] ,
          //  cameraId: [
          //   { type: "string", required: true,  message: "车载监控编号"}
          // ]                                
        }
      },
      formModel2: {
        opened: false,
        title: "详情",
        selection: [],
        fields: {
          carNum:"",
          carType:"",
          holderId:"",
          holderPhone:"",
          registerTime:"",
          awardTime:"",
          weight:"",
          balanceId:"",
          cameraId:"",
          typeName:"",
          brand:"",
          grabType:"",
          isDelete: 0,
          company:"",
          street:"",
          onlyNum:""
        }
      },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" }
      },
      //查询搜索
      stores: {
        car: {
         cityList: [
            {
              value: "0",
              label: "其他垃圾"
            },
            {
              value: "1",
              label: "易腐垃圾"
            },
            {
              value: "2",
              label: "有害垃圾"
            },
            {
              value: "3",
              label: "可回收垃圾"
            }            
          ],
          streetList:[],
          model1: "",


          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw:"",
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
            { type: "selection", width: 50, key: "handle" },
            { title: "车牌号", key: "carNum", sortable: true },
            { title: "所属公司", key: "company" },
            // { title: "称重设备ID", key: "balanceId" },
            { title: "所属街道", key: "street" },
            { title: "收运垃圾类型", key: "grabType" },
            { title: "负责垃圾箱房", key: "ljname",ellipsis: true,tooltip: true  },      
            { title: "负责箱房编号", key: "wingID",ellipsis: true,tooltip: true  },      
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
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建车辆信息";
      }
      if (this.formModel.mode === "edit") {
        return "编辑车辆信息";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.carUuid);
    } //删除
  },
  methods: {
    //获取调度数据
    loadCarDispatchList() {
      getCarDriverList(this.stores.car.query).then(res => {
        //console.log(res.data.data)
        this.stores.car.data = res.data.data;
        this.stores.car.query.totalCount = res.data.totalCount;
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
      window.location.href =
        this.url + "UploadFiles/ImportUserInfoModel/车辆信息导入模板.xlsx";
    },
     //申请垃圾箱房下拉框
     loadDepartmentList() {
       //console.log("        1111111")
       huoquxiala().then(res => {
        // Array.prototype.push.apply(
        //   this.stores.car.sources.rubbish,
        //   res.data.data
        // );
        this.stores.car.sources.rubbish=res.data.data;
        //console.log(this.stores.car.sources.rubbish)
      });
    },
    
    //申请社区下拉框
    loadDepartmentList12() {
      let data = this.$store.state.user.villageGuid;
      getshequ(data).then((res) => {
        // console.log(res.data.data);
        var streetlist = res.data.data;
        let townn = Array.from(new Set(streetlist.map((x) => x.towns)));
        // console.log(townn);
        this.stores.car.streetList = townn;
      });
    },
      //申请社区下拉框
     loadDepartmentList1() {
       //console.log("        22222222")
       huoquxialashequ().then(res => {
        // Array.prototype.push.apply(
        //   this.stores.car.sources.rubbish,
        //   res.data.data
        // );
        this.stores.car.sources.college=res.data.data;
        //console.log(this.stores.car.sources.college)
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
    doLoadCarDispatch(carUuid) {
      //console.log(carUuid)
      loadCarDriver({ guid: carUuid }).then(res => {
        this.formModel.fields = res.data.data;
        console.log(res.data.data)
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
      this.doDelete(row.carUuid);
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
          this.formModel.selection = [];
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
    //右边编辑
    handleEdit(row) {
      this.handleOpenFormWindow();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormDispatch();    
      //console.log(row)
      this.doLoadCarDispatch(row.carUuid);
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
    //右边详情
    handleDetail(row) {
      //console.log(row);
      this.formModel2.opened = true;
      this.$refs["formCarDispatchDetail"].resetFields();
      loadCarDriverDetail({ guid: row.carUuid }).then(res => {
        //console.log(111);
        console.log(res);
        this.formModel2.fields = res.data.data[0];
        //console.log(this.formModel2.fields)
      });
    },
    //添加按钮（公车调度申请）
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
      // this.formModel.fields.realName = this.$store.state.user.userName;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    //添加公车调度（保存）
    docreateCarDispatch() {
      ////console.log(this.formModel.fields);
      createCarDriver(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow(); //关闭表单
          this.loadCarDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑公车调度（保存）
    doEditCarDispatch() {
      editCarDriver(this.formModel.fields).then(res => {
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
        if(this.formModel.fields.holderPhone!='' && this.formModel.fields.holderPhone!=null){
          var reg=/^1(3[0-9]|4[5,7]|5[0,1,2,3,5,6,7,8,9]|6[2,5,6,7]|7[0,1,7,8]|8[0-9]|9[1,8,9])\d{8}$/;
          if(!(reg.test(this.formModel.fields.holderPhone))){ 
            this.$Message.error("手机号码有误，请重填");
            return false; 
          } 
        }
       
        if (this.formModel.mode === "create") {
          this.docreateCarDispatch();
        }
        if (this.formModel.mode === "edit") {
          this.doEditCarDispatch();
        }
      }
    },
    //提交按钮
    handleSubmitToAuditDispatch() {
      let valid = this.validateDispatchForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateToAuditCarDispatch();
        }
        if (this.formModel.mode === "edit") {
          this.doEditToAuditCarDispatch();
        }
      }
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
          ////console.log(file);
          this.exceldata = formData;
        } else {
          this.isexitexcel = false;
        }
      }
      //////console.log(this.exceldata);
    },
     async handleimport() {
      this.importdisable = true;
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      if (this.isexitexcel) {
        // this.intervalId = setInterval(() => {
        //     this.loadgetprogress()
        // }, 500)
        await UserInfoImport(this.exceldata).then(res => {
          //////console.log(res.data);
          if (res.data.code == 200) {
            this.time = res.data.data.time;
            this.successmsg = res.data.data.successmsg;
            this.repeatmsg = res.data.data.repeatmsg;
            this.defaultmsg = res.data.data.defaultmsg;
            this.handleRefresh();
            //clearInterval(this.intervalId);
            //this.showprogress = false;
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
    }
  },
  mounted() { 
     this.loadCarDispatchList();
     this.loadDepartmentList12();
  }
};
</script>
<style>
</style>
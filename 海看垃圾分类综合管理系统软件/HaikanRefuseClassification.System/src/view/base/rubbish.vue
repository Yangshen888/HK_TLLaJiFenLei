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
                      @on-change="xz1"
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
                      placeholder="输入垃圾箱房"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                     <Select
                      v-model="stores.car.query.kw2"
                      @on-change="handleSearchDispatch()"
                      placeholder="状态"
                      style="width:150px;"
                    >
                      <Option
                        v-for="item in stores.car.sources.state"
                        :value="item.value"
                        :key="item.value"
                      >{{item.text}}</Option>
                    </Select>    
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
                  ></Button>  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加垃圾箱房管理"
                >添加垃圾箱房管理</Button>
                <Button
                  v-can="'ewmexport'"                
                  icon="ios-cloud-upload"
                  type="warning"
                  @click="handleExportInfo('export')"
                  title="导出"
                >二维码导出</Button>
                <Button
                  v-can="'import'"                
                  icon="ios-cloud-upload"
                  type="warning"
                  @click="handleImportUserInfo"
                  title="导入垃圾箱房"
                >导入</Button>
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
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button type="error" size="small" v-can="'deletes'" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
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
            <FormItem label="垃圾箱房名字" prop="ljname">
              <Input v-model="formModel.fields.ljname" placeholder="请输入垃圾箱房名字"/>
            </FormItem>          
        </Row>
        <Row :gutter="16">
            <FormItem label="垃圾箱房编号" prop="wingId">
              <Input v-model="formModel.fields.wingId" placeholder="请输入垃圾箱房编号"/>
            </FormItem>          
        </Row>
        <Row :gutter="16">
            <FormItem label="所在乡镇(街道)" prop="towns">
              <Select v-model="formModel.fields.towns" @on-change="xz1">
                <Option
                  v-for="item in this.stores.car.sources.town"
                  :value="item"
                  :key="item"
                >{{item}}</Option>
              </Select>
            </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="所在社区" prop="vname">
              <Select v-model="formModel.fields.vname">
                <Option
                  v-for="item in this.stores.car.sources.college"
                  :value="item"
                  :key="item"
                >{{item}}</Option>
              </Select>
            </FormItem>
        </Row>

        <Row :gutter="16">
            <FormItem label="绑定设备编号" prop="facilityuuid">
              <Input v-model="formModel.fields.facilityuuid" placeholder="绑定设备编号"/>
            </FormItem>          
        </Row>        
        <Row :gutter="16">
            <FormItem label="当前状态" prop="state">
              <Select v-model="formModel.fields.state" >                
                <Option value="0" >{{ "使用中" }}</Option>
                <Option value="1" >{{ "暂停使用" }}</Option>
              </Select>
            </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="经度" prop="lon">
              <Input v-model="formModel.fields.lon" placeholder="请输入经度"/>
            </FormItem>          
        </Row>
        <Row :gutter="16">
            <FormItem label="纬度" prop="lat">
              <Input v-model="formModel.fields.lat" placeholder="请输入纬度"/>
            </FormItem>          
        </Row>
        <Row :gutter="16">
          <FormItem label="开放时间" prop="startTime">
            <TimePicker format="HH:mm" type="timerange" placement="bottom-end" placeholder="请选择开放时间" style="width:385px;" v-model="startTime"></TimePicker>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    
    <Drawer
      title="垃圾箱房信息导入"
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
         title="垃圾箱房信息导入模板下载"
        >垃圾箱房信息导入模板下载</Button>

        <Button
        v-can="'model'"
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
          <FormItem label="垃圾箱房名字">
            <Input v-model="formModel2.fields.ljname" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="垃圾箱房编号">
            <Input v-model="formModel2.fields.wingId" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="所在乡镇(街道)">
              <Input v-model="formModel2.fields.towns" :readonly="true"/>
            </FormItem>
        </Row> 
        <Row :gutter="16">
            <FormItem label="所在社区">
              <Input v-model="formModel2.fields.vname" :readonly="true"/>
            </FormItem>
        </Row> 
         
        <Row :gutter="16">
            <FormItem label="绑定设备编号">
              <Input v-model="formModel2.fields.facilityuuid" :readonly="true"/>
            </FormItem>
        </Row> 
         
        <Row :gutter="16">
             <FormItem label="当前状态">
              <Input v-if="formModel2.fields.state==1" value="暂停使用" :readonly="true"/>
              <Input v-if="formModel2.fields.state==0" value="使用中" :readonly="true"/>
            </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="经度">
              <Input v-model="formModel2.fields.lon" :readonly="true"/>
            </FormItem>
        </Row> 
        <Row :gutter="16">
            <FormItem label="纬度">
              <Input v-model="formModel2.fields.lat" :readonly="true"/>
            </FormItem>
        </Row> 
         <Row :gutter="16">
          <FormItem label="开放时间" prop="startTime">
            <Input v-model="formModel2.fields.startTime" :readonly="true"/>
          </FormItem>
        </Row>
   
      </Form>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import { getpeopleDriverList } from "@/api/person/supervisor"; //申请督导员下拉框
 import {getshequ} from "@/api/base/house";
import {
  getCarDriverList,
  huoquxialashequ,//社区下拉框
  huoquxialatowns,//乡镇下拉框
  huoquxialatownss,//乡镇下拉框
  huoquxialaaddress,//街道下拉框
  huoquxialapeople,//督导员下拉框
  huoquxialache,//收运车辆下拉框
  loadCarDriverDetail, //详情
  deleteCarDriver, //右边删除
  batchCommand, //右边上方删除
  createCarDriver, //添加（保存）
  editCarDriver, //编辑（保存）
  loadCarDriver, //加载
  globalvalidatePhone, 
  RubbishInfoImport,
  exportInfoCommand
} from "@/api/base/rubbish";

import config from "@/config";

export default {
  name: "rubbish",
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


    let globalvalidatevname = (rule, value, callback) => {
       //console.log(value);
      if (value == undefined) {
      callback(new Error('所在社区不能为空'));
      }
      
    else 
    if (value !== '' && value !== null) {
     callback();
    }
     else {
    callback(new Error('所在社区不能为空'));
    }
    callback();
   };


    let globalvalidateTowns = (rule, value, callback) => {
    if (value !== '' && value !== null) {
     callback();
    } else {
    callback(new Error('所在乡镇不能为空'));
    }
    callback();
   };


    let globalvalidateState = (rule, value, callback) => {
    if (value !== '' && value !== null) {
     callback();
    } else {
    callback(new Error('当前状态不能为空'));
    }
    callback();
   };


      let validateLon = (rule, value, callback) => {
        if (value !== '' && value !== null) {
          let reg = /^[\-\+]?(0?\d{1,2}\.\d{1,6}|1[0-7]?\d{1}\.\d{1,6}|180\.0{1,6})$/;
          if (!reg.test(value)) {
            callback(new Error('格式错误！范围在-180.9~180.9(小数点后面至少有一位最多六位)'));
          }
          callback();
        } else {
          callback(new Error('经度不能为空'));
        }
        callback();
      };
      let validateLat = (rule, value, callback) => {
        if (value !== '' && value !== null) {
          let reg = /^[\-\+]?([0-8]?\d{1}\.\d{1,6}|90\.0{1,6})$/;
          if (!reg.test(value)) {
            callback(new Error('格式错误！范围在-90.9~90.9(小数点后面至少有一位最多六位)'));
          }
          callback();
        } else {
          callback(new Error('纬度不能为空'));
        }
        callback();
      };

   
    return {

      list33:[],


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
          villageUuid:"",
          realName:"",
          ljname:"",
          vname:"",
          towns:"",
          address:"",
          state:"0",
          startTime:"",
          wingId:"",
          endTime:"",
          sname:"",
          facilityuuid:"",
          //carNum:"",
          isDelete: 0,
          villageId:"",
          lon:0,
          lat:0,          
        },
        rules: {
          ljname: [
            { type: "string", required: true, message: "请输入垃圾箱房" }
          ],
          towns: [
            { type: "string", required: true, validator:globalvalidateTowns}
          ],  
          state: [
            { type: "string", required: true, validator:globalvalidateState}
          ], 
          vname: [
            { type: "string", required: true, validator:globalvalidatevname}
          ],
          lon: [
            { type: "string", required: true, validator:validateLon}
          ],
          lat: [
            { type: "string", required: true, validator:validateLat}
          ],                      
        }
      },
      startTime:[],
      formModel2: {
        opened: false,
        title: "详情",
        selection: [],
        fields: {
          ljname:"",
          realName:"",
          vname:"",
          towns:"",
          address:"",
          state:"0",
          startTime:"",
          endTime:"",
          lon:"",
          lat:"",
          towns:"",
          facilityuuid:"",
         // carNum:"",
          sname:"",
          villageId:"",
          wingId:""
        }
      },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        import: { name: "import", title: "导入" },
        export: { name: "export", title: "导出" },
      },
      //查询搜索
      stores: {
        car: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1:"",
            kw2:"",
            kw3:"",
            vuuid:this.$store.state.user.villageGuid,
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
            college: ["全部"],     
             //乡镇集合
            town: [ "全部" ],   
             //街道集合
            street: [{ address: "0", address: "全部" }],      
              //督导员集合
            supervisor: [{ systemUserUuid: "0", realName: "全部" }], 

            state: [
              { value: "", text: "全部" },
              { value: 0, text: "使用中" },
              { value: 1, text: "暂停使用" },
            ]    
          },
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "垃圾箱房编号", key: "wingID", sortable: true },
            { title: "垃圾箱房名字", key: "ljname", sortable: true,ellipsis: true,tooltip: true },
            { title: "所在街道", key: "towns" },
            { title: "所在社区", key: "vname" },
            { title: "当前状态", key: "state" },
            { title: "督导员", key: "realName" },
            { title: "运输车辆", key: "carNum" },
            // { title: "绑定设备编号", key: "facilityuuid" },
            { title: "开放时间", key: "startTime" },
            // { title: "结束时间", key: "endTime" },
            // { title: "经度", key: "lon" },
            // { title: "纬度", key: "lat" },    
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
        return "创建垃圾箱房申请";
      }
      if (this.formModel.mode === "edit") {
        return "编辑垃圾箱房信息";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.garbageRoomUuid);
    } //删除
  },
  methods: {
    //获取信息数据
    loadCarDispatchList() {
      getCarDriverList(this.stores.car.query).then(res => {
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
        this.url + "UploadFiles/ImportUserInfoModel/垃圾箱房导入模板.xlsx";
    },

     //申请乡镇下拉框①
     loadDepartmentList5() {
       console.log(this.$store.state.user.villageGuid);
       if(this.$store.state.user.villageGuid==null){
huoquxialatowns().then(res => {
        this.stores.car.sources.town=res.data.data;
      });
       }else{
         let data=this.$store.state.user.villageGuid;
huoquxialatownss(data).then(res => {
        this.stores.car.sources.town=res.data.data;
      });
       }
       
      
    },

     //获取督导员下拉框
     loadDepartmentList2() {
       huoquxialapeople().then(res => {
        this.stores.car.sources.supervisor=res.data.data;
      });
    },



     //申请社区下拉框
     loadDepartmentList1() {
       let data=this.stores.car.query.vuuid;
       getshequ(data).then(res => {
        // this.stores.car.sources.college=res.data.data;
         this.list33 = res.data.data;
         let townn = Array.from(new Set(this.list33.map(x => x.towns )));
        this.stores.car.sources.town=townn;          
      });
    },

    //根据乡镇/街道获取社区
    xz(e){
      if(typeof(e) == "undefined")
      {
      let towns = this.formModel.fields.towns;
      let college = this.list33.filter(x => x.towns == towns);
      this.stores.car.sources.college=college.map(x => x.vname);
      }
      else
      {
      let college = this.list33.filter(x => x.towns == e);
      this.stores.car.sources.college=college.map(x => x.vname);

      }
    },
    xz1(e){
     let college = this.list33.filter(x => x.towns == e);
      this.stores.car.sources.college=college.map(x => x.vname);
      this.loadCarDispatchList();      
    },
 sq(e) {
      if (e == "全部") {
        this.stores.car.query.ccmmunity = "";
      }
      this.loadCarDispatchList();  
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
    doLoadCarDispatch(garbageRoomUuid) {
      // //console.log(garbageRoomUuid)
      loadCarDriver({ guid: garbageRoomUuid }).then(res => {
        this.formModel.fields = res.data.data;
        this.xz();
        // this.jd();
        // //console.log(res.data.data)
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
      this.doDelete(row.garbageRoomUuid);
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
      // //console.log(row)
      this.doLoadCarDispatch(row.garbageRoomUuid);
      // //console.log(row.garbageRoomUuid);
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
      this.formModel2.opened = true;
      this.$refs["formCarDispatchDetail"].resetFields();
      loadCarDriverDetail({ guid: row.garbageRoomUuid }).then(res => {
        this.formModel2.fields = res.data.data;
        //console.log(this.formModel2.fields);
      });
    },
    //添加按钮（志愿者申请）
    handleShowCreateWindow() {
      this.loadDepartmentList5();
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
      this.formModel.fields.realName = this.$store.state.user.userName;
       //社区
       this.formModel.fields.vname = this.$store.state.user.vname;

    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    //添加志愿者（保存）
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
    //编辑志愿者（保存）
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
      let villageUuid = this.list33.find(x => x.vname == this.formModel.fields.vname)
      if( this.startTime[0]!=""){
        this.formModel.fields.startTime=this.startTime[0]+"-"+this.startTime[1];
      }else{
        this.formModel.fields.startTime="";
      }
      //console.log(this.startTime);
      //console.log(this.formModel.fields);
      this.formModel.fields.villageUuid = villageUuid.villageUuid;
      if (valid) {
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
        await RubbishInfoImport(this.exceldata).then(res => {
          //////console.log(res.data);
          if (res.data.code == 200) {
            this.time = res.data.data.time;
            this.successmsg = res.data.data.successmsg;
            this.repeatmsg = res.data.data.repeatmsg;
            this.defaultmsg = res.data.data.defaultmsg;
            this.loadCarDispatchList();
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
    },
      //导出相关操作
   handleExportInfo(command) {
     //console.log(this.selectedRowsId);
     
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要生成并 [" +
          this.commands[command].title +
          "] 当前已选垃圾箱房的二维码?</p>",
        loading: true,
        onOk: () => {
          this.doExportInfoCommand(command);
        }
      });
    },
    doExportInfoCommand(command) {
      exportInfoCommand({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          // //console.log(res.data); 
          var DZurl = res.data.data;  
          window.location = this.url + DZurl ;                        
          this.$Message.success(res.data.message);
          this.loadCarDispatchList();
          // this.formModel1.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    }
  },
  mounted() {
     this.loadDepartmentList5();//乡镇下拉框

    //  this.loadDepartmentList6();//街道下拉框
     this.loadDepartmentList1();//社区下拉框

     this.loadCarDispatchList();
     this.loadDepartmentList2();//督导员下拉框
     //this.loadDepartmentList3();//收运车辆下拉框
  }
};
</script>
<style>
</style>
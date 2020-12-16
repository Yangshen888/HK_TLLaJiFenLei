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
                  <!-- <FormItem>    
                       <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.car.query.kw"
                      placeholder="输入社区名称"
                      @on-search="handleSearchDispatch()"
                    ></Input>                
                  </FormItem> -->
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

                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加社区"
                >添加社区</Button>
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
            <!-- <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip> -->
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
            <FormItem label="所在乡镇(街道)" prop="towns">
              <Input v-model="formModel.fields.towns" placeholder="请输入所在乡镇(街道)"/>
            </FormItem>
        </Row>
         <!-- <Row :gutter="16">
            <FormItem label="所在街道" prop="address">
              <Input v-model="formModel.fields.address" placeholder="请输入所在街道"/>
            </FormItem>
        </Row> -->
        <Row :gutter="16">
            <FormItem label="社区名称" prop="vname">
              <Input v-model="formModel.fields.vname" placeholder="请输入社区名称"/>
            </FormItem>
        </Row>
       
        <Row :gutter="16">
            <FormItem label="每月兑换时间" prop="exchange">
            <Input-number
              :min="1"
              :max="31"
              v-model="formModel.fields.exchange"
              style="width:100%"
            ></Input-number>
             </FormItem>
        </Row> 
        <Row :gutter="16">
            <FormItem label="每月兑换标准" prop="disNum">
            <Input-number
              :min="0"
              v-model="formModel.fields.disNum"
              style="width:100%"
            ></Input-number>
             </FormItem>
        </Row>  
         <!-- <Row :gutter="16">
            <FormItem label="负责箱房">
              <Select v-model="formModel.fields.garbageRoomUuid">
                <Option
                  v-for="item in this.stores.car.sources.rubbish"
                  :value="item.garbageRoomUuid"
                  :key="item.garbageRoomUuid"
                >{{item.ljname}}</Option>
              </Select>
            </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="督导员">
              <Select v-model="formModel.fields.supervisorId">
                <Option
                  v-for="item in this.stores.car.sources.supervisor"
                  :value="item.supervisorUuid"
                  :key="item.supervisorUuid"
                >{{item.sname}}</Option>
              </Select>
            </FormItem>
        </Row> -->
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>

    <Drawer title="详情" v-model="formModel2.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel2.fields" ref="formCarDispatchDetail" label-position="left">    
        <Row :gutter="16">
          <FormItem label="所在乡镇(街道)">
            <Input v-model="formModel2.fields.towns" :readonly="true" />
          </FormItem>
        </Row>
         <!-- <Row :gutter="16">
            <FormItem label="所在街道">
              <Input v-model="formModel2.fields.address" :readonly="true"/>
            </FormItem>
        </Row>  -->
        <Row :gutter="16">
          <FormItem label="社区名称">
            <Input v-model="formModel2.fields.vname" :readonly="true" />
          </FormItem>
        </Row>
       
        <Row :gutter="16">
            <FormItem label="社区垃圾箱房">
              <Input type="textarea" v-model="formModel2.fields.ljname" :readonly="true"/>
            </FormItem>
        </Row> 
        <Row :gutter="16">
            <FormItem label="每月兑换时间">
              <Input v-model="formModel2.fields.exchange" :readonly="true"/>
            </FormItem>
        </Row>  
        <Row :gutter="16">
            <FormItem label="每月兑换标准">
              <Input v-model="formModel2.fields.disNum" :readonly="true"/>
            </FormItem>
        </Row>          
      </Form>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import { getlgxDriverList } from "@/api/base/rubbish"; //申请垃圾箱下拉框
import { getpeopleDriverList } from "@/api/person/supervisor"; //督导员下拉框
 import {getshequ} from "@/api/base/house";
import {
  getCarDriverList,
  huoquxiala,//垃圾箱下拉框
  loadCarDriverDetail, //详情
  deleteCarDriver, //右边删除
  batchCommand, //右边上方删除
  createCarDriver, //添加（保存）
  editCarDriver, //编辑（保存）
  loadCarDriver, //加载
  globalvalidatePhone 
} from "@/api/base/college";

export default {
  name: "college", 
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
    return {
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        selection: [],
        fields: {
          towns:"",
          vname:"",
          address:"",
          exchange:1,
          disNum:0,
          grabageRoomId:"",
          ljname:"",//垃圾箱房
          supervisorId:"",
          sname:"",
          isDelete: 0
        },
        rules: {
          vname: [
            { type: "string", required: true, message: "请输入社区名称" }
          ] ,
          // address: [
          //   { type: "string", required: true, message: "请输入所在街道" }
          // ],
          towns: [
            { type: "string", required: true, message: "请输入所在乡镇(街道)" }
          ]
        }
      },
      formModel2: {
        opened: false,
        title: "详情",
        selection: [],
        fields: {
          vname:"",
          towns:"",
          exchange:1,
          disNum:0,
          address:"",
          ljname:"",//垃圾箱房
          grabageRoomId:"",
          supervisorId:"",//督导员
          sname:""
        }
      },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" }
      },
      list33: [],
      //查询搜索
      stores: {
        car: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            street:"",
            ccmmunity:"",
            IsDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {      
              //垃圾箱房集合
            rubbish: [{ garbageRoomUuid: "0", ljname: "全部" }],
               //督导员集合
            supervisor: [{ supervisorUuid: "0", sname: "全部" }],
            //社区集合
            college: ["全部" ],     
             //街道集合
            town: ["全部" ],  
    
          },
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "所在乡镇(街道)", key: "towns" },
            // { title: "所在街道", key: "address" },
            { title: "社区名称", key: "vname" },
            // { title: "社区垃圾箱房", key: "ljname" },   
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
        return "创建社区申请";
      }
      if (this.formModel.mode === "edit") {
        return "编辑社区信息";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.villageUuid);
    } //删除
  },
  methods:{
    //获取调度数据
    loadCarDispatchList() {
      getCarDriverList(this.stores.car.query).then(res => {
        ////console.log(res.data.data)
        this.stores.car.data = res.data.data;
        this.stores.car.query.totalCount = res.data.totalCount;
      });
    },
     //申请垃圾箱房下拉框
     loadlgxtList() {
       //console.log("        1111111")
       huoquxiala().then(res => {
        this.stores.car.sources.rubbish=res.data.data;
        //console.log(this.stores.car.sources.rubbish)
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
    doLoadCarDispatch(villageUuid) {
      //console.log(villageUuid)
      loadCarDriver({ guid: villageUuid }).then(res => {
        this.formModel.fields = res.data.data;
        //console.log(this.formModel.fields)
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
      this.doDelete(row.villageUuid);
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
      //console.log(row.villageUuid)
      //console.log(row)
      this.doLoadCarDispatch(row.villageUuid);
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
       //申请社区下拉框
     loadDepartmentList1() {
       getshequ().then(res => {
        //console.log(res.data.data);
         this.list33 = res.data.data;
         let townn = Array.from(new Set(this.list33.map(x => x.towns )));
         //console.log(townn);          
        this.stores.car.sources.town=townn;  
      });
      
    },
    xz(e){     
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
    //右边详情
    handleDetail(row) {
      //console.log(row);
      this.formModel2.opened = true;
      this.$refs["formCarDispatchDetail"].resetFields();
      
      let parms={ guid: row.villageUuid,trashName:row.ljname };
      //console.log(parms);
      loadCarDriverDetail(parms).then(res => {
        this.formModel2.fields = res.data.data;
        console.log(this.formModel2.fields);
      });
    },
    //添加按钮（公车调度申请）
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
      //this.formModel.fields.realName = this.$store.state.user.userName;
      //垃圾箱房下拉框
       this.formModel.fields.ljname = this.$store.state.user.ljname;
      this.formModel.fields.garbageRoomUuid = this.stores.car.sources.rubbish[1][
        "garbageRoomUuid"
      ];
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    //添加公车调度（保存）
    docreateCarDispatch() {
      //console.log(this.formModel.fields);
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
        //console.log(this.formModel2.fields)
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
    }
  },
  mounted() {
    this.loadCarDispatchList();
    this.loadlgxtList();//垃圾箱房下拉框
    this.loadDepartmentList1();
  }
};
</script>
<style>
</style>
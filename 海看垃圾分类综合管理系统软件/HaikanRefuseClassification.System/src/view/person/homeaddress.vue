<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.addressif.query.totalCount"
        :pageSize="stores.addressif.query.pageSize"
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
                      v-model="stores.addressif.query.kw"
                      style="width:200px"
                      @on-change="xz"
                      clearable
                      placeholder="请选择街道"
                    >
                      <Option
                        v-for="item in this.stores.addressif.sources.town"
                        :value="item"
                        :key="item"
                      >{{ item }}</Option>
                    </Select>
                    <Select
                      v-model="stores.addressif.query.kw1"
                      style="width:200px"
                      @on-change="sq"
                      clearable
                      placeholder="请选择社区"
                    >
                      <Option
                        v-for="item in this.stores.addressif.sources.college"
                        :value="item"
                        :key="item"
                      >{{ item }}</Option>
                    </Select>
                    <Select
                      v-model="stores.addressif.query.kw3"
                      style="width:200px"
                      @on-change="gongyu"
                      clearable
                      placeholder="请选择小区"
                    >
                      <Option
                        v-for="item in this.stores.addressif.sources.resregion"
                        :value="item=='全部'?0:item"
                        :key="item"
                      >{{ item }}</Option>
                    </Select>
                    <Input
                      style="width: 400px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.addressif.query.kw2"
                      placeholder="关键字查询,空格隔开"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <!-- <Button
                  v-can="'import'"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleToken"
                  title="地址库对接"
                >地址库对接</Button>-->
                <Button
                  v-can="'yjexport'"
                  icon="ios-cloud-upload"
                  type="warning"
                  @click="selectYjexportInfo"
                  title="一键生成所查地址二维码"
                  :loading="loading"
                >
                  <span v-if="!loading">一键导出所有</span>
                  <span v-else>一键导出所有</span>
                </Button>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加地址"
                >添加地址</Button>                   
                <Button
                  v-can="'import'"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleImportUserInfo"
                  title="导入地址信息"
                >导入地址信息</Button>                   
                <Button
                  v-can="'ewmexport'"                
                  icon="ios-cloud-upload"
                  type="warning"
                  @click="handleExportInfo('export')"
                  title="导出"
                >二维码导出</Button>                                 
                <ButtonGroup class="mr3">
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh_Address"></Button>
           
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
          :data="stores.addressif.data"
          :columns="stores.addressif.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh_Address"
          :row-class-name="rowClsRender_Address"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <template slot-scope="{row,index}" slot="state">
            <span>{{CheckState(row.state)}}</span>
          </template>
          <!-- <template slot-scope="{row,index}" slot="status">
            <Tag :color="renderStatus(row.status).color">{{renderStatus(row.status).text}}</Tag>
          </template>-->
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

            <!-- <Poptip confirm :transfer="true" title="确定要生成并导出吗?" @on-ok="addExport(row)">
            <Tooltip placement="top" content="导出" :delay="1000" :transfer="true">
                <Button
                  v-can="'export'"
                  size="small"                  
                  icon="ios-cloud-upload"
                  type="primary"
                  @click="addExport(row)"
                  title="生成二维码并导出"
                >导出</Button>              
            </Tooltip>
            </Poptip> -->

            
          </template>
        </Table>
      </dz-table>
    </Card>
    
            
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
                  title="地址信息导入模板"
                >地址信息导入模板</Button>
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
      </div> -->
        <Tabs value="name1">
          <TabPane label="成功" name="name1" v-html="successmsg"></TabPane>
          <TabPane label="重复" name="name2" v-html="repeatmsg"></TabPane>
          <TabPane label="失败" name="name3" v-html="defaultmsg"></TabPane>
        </Tabs>
      </div>
    </Drawer>  

   <Drawer
      title="添加单条地址信息"
      v-model="formMode2.opened"
      width="500"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formMode2.fields"
        ref="formdispatch"
        :rules="formMode2.rules"
        label-position="top"
      >

        <Row :gutter="16">
            <FormItem label="地址信息" prop="address">
              <Input v-model="formMode2.fields.address" placeholder="地址信息"/>
            </FormItem>
        </Row>
        
        <Row :gutter="16">
            <FormItem label="地址编号" prop="addresscode">
              <Input v-model="formMode2.fields.addresscode" placeholder="地址编号"/>
            </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="街道" prop="town">
              <Input v-model="formMode2.fields.town" placeholder="街道"/>
            </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="社区" prop="ccmmunity">
              <Input v-model="formMode2.fields.ccmmunity" placeholder="社区"/>
            </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="小区" prop="resregion">
              <Input v-model="formMode2.fields.resregion" placeholder="小区"/>
            </FormItem>
        </Row>

      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formMode2.opened = false">取 消</Button>
      </div>
    </Drawer>


    <Drawer title="修改地址信息" v-model="formModel.opened" width="500" :mask-closable="false" :mask="false">
      <Form :model="formModel.fields" ref="formCarDispatchDetail" label-position="left">
        <Row :gutter="16">
          <FormItem label="地址信息">
            <Input v-model="formModel.fields.address" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="地址编号">
            <Input v-model="formModel.fields.addresscode"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="街道" prop="town">
              <Input v-model="formModel.fields.town" placeholder="街道"/>
            </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="社区" prop="ccmmunity">
              <Input v-model="formModel.fields.ccmmunity" placeholder="社区"/>
            </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="小区" prop="resregion">
              <Input v-model="formModel.fields.resregion" placeholder="小区"/>
            </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="btnClickEdit">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>      
    </Drawer>



  </div>
</template>

<script>
import JSZip from "jszip";
import FileSaver from "file-saver";
import DzTable from "_c/tables/dz-table.vue";
import {
yjexportInfo,  
getshequ,
getshequ1,
getfamily,
shopInfoImport,
getAddressInfo,
exportInfoCommand,
btnToken,
create,
editSave,  //编辑保存
loadEditData  //加载编辑
} from "@/api/person/address";
import config from "@/config";
export default {
  name: "homeaddress",
  components: {
    DzTable
  },
  data() {

    
    return {
      commands: {
        export: { name: "export", title: "导出" },
      },
      loading: false,
      list33: [],
      family: [],
      //导入
      url: config.baseUrl.dev,
      importdisable: false,
      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      formimport: {
        opened: false
      },


      // list:[{fileUrl:"",renameFileName:""}],
      // filename:'',


      //form保存参数
      formModel: {
        selection: [],
        opened: false,
        mode: "edit",        
        fields: {
          addresscode:"",
          address:"",
          homeAddressUuid:"",
          id:"",
          resregion:"",
          town:"",
          ccmmunity:"",
        }
      },
      formMode2: {
        opened: false,
        title: "创建申请",
        mode: "create",
        selection: [],
        fields: {
          addresscode:"",
          address:"",
          resregion:"",
          town:"",
          ccmmunity:"",
        },
        rules: {
          address: [
            { type: "string", required: true, message: "请输入地址" }
          ],
          town: [
            { type: "string", required: true, message: "请输入街道" }
          ],
          ccmmunity: [
            { type: "string", required: true, message: "请输入社区" }
          ],
        }
      },
      formModel3: {
        opened: false,
        title: "创建申请",
        mode: "",
        selection: [],
        fields: {
          towns:"",
          vname:"",
          villageId:""
        
      },
      },
      //...
      //用于提交及一些保持性数据
      stores: {
        //该实例对象相关数据
        addressif: {
          //提交请求参数
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isDeleted: 0,
            status: -1,
            //自定义参数
            kw: "",
            kw1:"",
            kw2:"",
            kw3:"",
            vuuid:this.$store.state.user.villageGuid,
            vill:this.$store.state.user.streets,
            //查询排序方式
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          //请求获得的数据集合,用于填入表格
          data: [],
          //一些自定义附件处理数据,用于下拉列表之类
          sources: {
              //社区集合
            college: ["全部" ],     
             //街道集合
            town: ["全部" ],      
             //小区集合
            resregion: ["全部" ],  
          },

          // url:'',

          //table的列项名称
          columns: [
            //选择框
            { type: "selection", width: 50, key: "handle" },
            { title: "详细地址", key: "address", minWidth:400, sortable: true },
            { title: "地址编码", key: "addresscode",minWidth:200 ,sortable: true },            
            //可进行逻辑处理的条件规则显示项,以slot绑定
            // { title: "xxx", key: "xxx", slot: "state" },
            //操作按钮显示样式
            {
              title: "操作",
              align: "center",
              fixed:"right",              
              width: 80,
              className: "table-command-column",
              slot: "action"
            }
          ],
          data:[]
        },
        xxx2: {}
        //...
      },
      //自定义样式
      styles: {}
    };
  },
  //计算属性,以名称命名的方法体
  computed: {
    formTitle() {
      //条件
      return "";
    },
    //...
    //获取表格选择的行id
    // selectedRows() {
    //   return this.formModel.selection;
    // },
    selectRowsId() {
      //返回具体选择项的uuid
      return this.formModel.selection.map(x => x.homeAddressUuid);
    }
    //...
  },
  //方法集合
  methods: {


    // handleToken() {

    //   btnToken().then(res => {
    //     console.log(res)
    //   });      
    // },
    
    //加载表格数据
    loadListAddress() {
      this.loading = false;
      getAddressInfo(this.stores.addressif.query).then(res => {
        // //console.log(res.data)
        this.stores.addressif.data = res.data.data;
        this.stores.addressif.query.totalCount = res.data.totalCount;
      });
    },

    //导出相关操作
    handleExportInfo(command) {
      //console.log(this.selectRowsId);

      if (!this.selectRowsId || this.selectRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要生成并 [" +
          this.commands[command].title +
          "] 当前已选地址的家庭码?</p>",
        loading: true,
        onOk: () => {
          this.doExportInfoCommand(command);
        }
      });
    },
    doExportInfoCommand(command) {
      exportInfoCommand({
        command: command,
        ids: this.selectRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          // //console.log(res.data);
          var DZurl = res.data.data;
          window.location = this.url + DZurl;
          this.$Message.success(res.data.message);
          this.loadListAddress();
          // this.formModel1.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },

    //一键导出
    selectYjexportInfo() {
      
        this.loading = true;
      yjexportInfo(this.stores.addressif.query).then(res => {
        //this.stores.addressif.data = res.data.data;
        var DZurl = res.data.data;
        window.location = this.url + DZurl;
        this.loadListAddress();
        this.$Message.success("一键导出成功");
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
        this.url + "UploadFiles/ImportUserInfoModel/地址信息导入模板.xlsx";
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
            this.loadListAddress();

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



    //添加按钮（志愿者申请）
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
      this.formMode2.fields.realName = this.$store.state.user.userName;

    },
    handleSwitchFormModeToCreate() {
      this.formMode2.mode = "create";
    },
    //添加志愿者（保存）
    docreateCarDispatch() {
      
        if(this.formMode2.fields.town==""){
          this.$Message.warning("请输入所属街道");
          return;
        }
        if(this.formMode2.fields.ccmmunity==""){
          this.$Message.warning("请输入所属社区");
          return;
        }
      console.log(this.formModel.fields);
      create(this.formMode2.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow(); //关闭表单
          this.loadListAddress();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
        handleSubmitConsumable() {
      let valid = this.validateDispatchForm();
      if (valid) {
        if (this.formMode2.mode === "create") {
          this.docreateCarDispatch();
        }
      }          
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

    //打开窗口
    handleOpenFormWindow() {
      this.formMode2.opened = true;
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatch"].resetFields();
    },
    //自动关闭窗口
    handleCloseFormWindow() {
      this.formMode2.opened = false;
    },


      //右边编辑
      handleEdit(row) {
        this.handleOpenFormWindow1();
        this.handleSwitchFormModeToEdit();
        this.handleResetFormDispatch1();
        this.doLoadEditData(row.homeAddressUuid);
      },
      handleSwitchFormModeToEdit() {
        this.formModel.mode = "edit";
      },
      doLoadEditData(homeAddressUuid){       
        loadEditData({guid:homeAddressUuid}).then(res=>{
          // //console.log(res.data)
            this.formModel.fields = res.data.data;
        })
      },


      //编辑商店（保存）
      doEditShopDispatch() {
        // //console.log(this.formModel.fields);
        if(this.formModel.fields.town==""){
          this.$Message.warning("请输入所属街道");
          return;
        }
        if(this.formModel.fields.ccmmunity==""){
          this.$Message.warning("请输入所属社区");
          return;
        }
        editSave(this.formModel.fields).then(res => {
          // //console.log(this.formModel.fields)
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.handleCloseFormWindow1();
            this.loadListAddress();
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
      btnClickEdit(){
          this.doEditShopDispatch();
     
      },
      

      
    //清空
    handleResetFormDispatch1() {
      this.$refs["formCarDispatchDetail"].resetFields();
    },

    //打开窗口
    handleOpenFormWindow1() {
      this.formModel.opened = true;
    },
          //自动关闭窗口
    handleCloseFormWindow1() {
      this.formModel.opened = false;
    },


    //表格行样式
    rowClsRender_Address(row, index) {
      // if (row.isDeleted) {
      //   return "table-row-disabled";
      // }
      // return "";
    },

    //刷新
    handleRefresh_Address() {
      this.loadListAddress();
    },
    //行选框的改变
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //排序改变(忽略?)
    handleSortChange(column) {
      this.stores.addressif.query.sort.direction = column.order;
      this.stores.addressif.query.sort.field = column.key;
      this.loadVoteinitiateList();
    },
    //搜索
    handleSearchDispatch() {
      this.loadListAddress();
    },
    //表格翻页
    handlePageChanged(page) {
      this.stores.addressif.query.currentPage = page;
      this.loadListAddress();
    },
    //表格页大小改变
    handlePageSizeChanged(pageSize) {
      this.stores.addressif.query.pageSize = pageSize;
      this.loadListAddress();
    },
    
     //申请社区下拉框
     loadDepartmentList1() {
       getshequ1().then(res => {
        //console.log(res.data.data);
         this.list33 = res.data.data;
         let townn = Array.from(new Set(this.list33.map(x => x.towns )));
         //console.log(townn);          
        this.stores.addressif.sources.town=townn;  
      });
      
    },
    xz(e){     
      let college = this.list33.filter(x => x.towns == e);
      this.stores.addressif.sources.college=college.map(x => x.vname);
      this.loadListAddress();                  
    },
    sq(e) {
      if (e == "全部") {
        this.stores.addressif.query.kw1 = "";
      }
      getfamily({ vill: e }).then(res => {
        this.family = res.data.data;
        for (let i = 0; i < this.family.length; i++) {
          if (
            this.family[i].resregion == null &&
            this.family[i].street != null
          ) {
            this.family[i].resregion = this.family[i].street;
          }
        }
        let resregion = Array.from(new Set(this.family.map(x => x.resregion)));
        resregion = resregion.filter(x => x != null);
        this.stores.addressif.sources.resregion = resregion;
        if (this.stores.addressif.sources.resregion.length == 0) {
          this.stores.addressif.sources.resregion = ["全部"];
        }
        console.log(this.stores.addressif.sources.resregion);
      });

      this.loadListAddress();  
    },
    gongyu(e) {
      if (e == "全部") {
        this.stores.addressif.query.kw3 = "";
      }
      this.loadListAddress();  
    },
    //---------------第二个模型-------------------

    //----------------slot-----------------
    Is_xxx(type){
      let rType="未知";
      // switch(type){
      //   case 0:
      //     rType = "否";
      //     break;
      //   case 1:
      //     rType = "是";
      //     break;
      // }
      return rType;
    },
    CheckState(state) {
      if (state == 0) {
        return "否";
      }
      if (state == 1) {
        return "是";
      }
    },
    //------------------杂项---------------------
    //将日期转为YY-MM-DD hh:mm:ss字符串格式
    dateToString(date) {
      let year = date.getFullYear();
      let month = date.getMonth() + 1;
      let day = date.getDate();
      let hour=date.getHours();
      let minute=date.getMinutes();
      let second=date.getSeconds();
      return year + "-" + month + "-" + day+" "+hour+":"+minute+":"+second;
    }
  },
  //页面加载时执行
  mounted(){
    this.loadDepartmentList1();
    this.loadListAddress();
  }
};
</script>
<style>
.demo-spin-col{
    height: 100px;
    position: absolute;
    left: 35%;
    top: 50%;
    }

.demo-spin-icon-load{
        animation: ani-demo-spin 1s linear infinite;
    }    
</style>

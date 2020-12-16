<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.shop.query.totalCount"
        :pageSize="stores.shop.query.pageSize"
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
                      v-model="stores.shop.query.kw"
                      placeholder="输入商店名称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
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
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'import'"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleImportUserInfo"
                  title="导入商店信息"
                >导入商店信息</Button>                
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加商店"
                >添加商店</Button>
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
          :data="stores.shop.data"
          :columns="stores.shop.columns"
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
              placement="top"
              content="扣除积分"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'edit1'"
                type="error"
                size="small"
                shape="circle"
                icon="md-trash"
                @click="EditScore(row)"
              ></Button>
            </Tooltip>            
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
      title="扣除商店积分"
      v-model="formModel33.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formModel33.fields"
        ref="fromeditscore"
        :rules="formModel33.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="扣除积分" prop="score">
            <Input v-model="formModel33.fields.score" :maxlength='10' placeholder="扣除积分" />
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="doshopEditScore">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel33.opened = false">取 消</Button>
      </div>
    </Drawer>

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
          <FormItem label="商店名称" prop="shopName">
            <Input v-model="formModel.fields.shopName" placeholder="请输入商店名称"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="商店负责人" prop="shopowner">
            <Input v-model="formModel.fields.shopowner" placeholder="请输入商店负责人"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系方式" prop="phone">
            <Input v-model="formModel.fields.phone" placeholder="请输入联系方式"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="商店地址" prop="address">
            <Input v-model="formModel.fields.address" placeholder="请输入商店地址"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="状态" prop="state">
            <Select v-model="formModel.fields.state" >
              <Option value="1" >{{ "正在营业" }}</Option>
              <Option value="2" >{{ "暂停营业" }}</Option>
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
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>

    <Drawer title="详情" v-model="formModel2.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel2.fields" ref="formCarDispatchDetail" label-position="left">
        <Row :gutter="16">
          <FormItem label="商店名称">
            <Input v-model="formModel2.fields.shopName" :readonly="true"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="商店负责人">
            <Input v-model="formModel2.fields.shopowner"  :readonly="true"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系方式">
            <Input v-model="formModel2.fields.phone" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="商店地址">
            <Input v-model="formModel2.fields.address" :readonly="true" />
          </FormItem>
        </Row>
        <!-- <Row :gutter="16">
          <FormItem label="可兑换物">
            <Input v-model="formModel2.fields.goods" :readonly="true" />
          </FormItem>
        </Row> -->
        <Row :gutter="16">
          <FormItem label="状态">
            <Input v-model="formModel2.fields.state"  :readonly="true"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
        <FormItem label="经度" prop="lon">
          <Input v-model="formModel2.fields.lon" :readonly="true"/>
        </FormItem>
      </Row>
        <Row :gutter="16">
          <FormItem label="纬度" prop="lat">
            <Input v-model="formModel2.fields.lat" :readonly="true"/>
          </FormItem>
        </Row>
      </Form>
    </Drawer>

 

    <Drawer
      title="商店信息导入"
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
                  title="商店信息导入模板"
                >商店信息导入模板</Button>
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleimport"
          :disabled="importdisable"
        >导入</Button> 
      <div>
           <!-- <Col class="demo-spin-col" span="8">
            <Spin fix>
                <Icon type="ios-loading" size=18 class="demo-spin-icon-load"></Icon>
                <div>Loading</div>
            </Spin>
        </Col>       -->
      </div>
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
  import DzTable from "_c/tables/dz-table.vue";
  import config from "@/config";
  import {
    dbScore,//判断积分
    loadShopEditScore,//扣除积分
    shopImport,//导入
    getShopList,
    loadShopDetail, //详情
    deleteShop, //右边删除
    batchCommand, //右边上方删除
    create, //添加（保存）
    editSave, //编辑（保存）
    loadShop, //加载
    LoadGoods,
    loadShopEditData,
    globalvalidatePhone
  } from "@/api/shop/shop";

  export default {
    name: "shop",
    components: {
      DzTable
    },
    data() {
      
      let globalvalidatePhone = (rule, value, callback) => {
        if (value !== '' && value !== null) {
          let reg = /^1[3456789]\d{9}$/;
          if (!reg.test(value)) {
            callback(new Error('请输入有效的电话号码'));
          }
          callback();
        } else {
          callback(new Error('电话号码不能为空'));
        }
        callback();
      };
      let scoreInt = (rule, value, callback) => {
        if (value !== '' && value !== null) {
          let reg = /^[1-9]\d*$/;
          if (!reg.test(value)) {
            callback(new Error('请输入正整数字'));
          }else if(value>2147483647){
            callback(new Error('请输入范围1~2147483647'));
          }

          callback();
        } else {
          callback(new Error('分数不能为空'));
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

      shopUuid:"",

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
          title: "添加商店",
          mode: "create",
          selection: [],
          fields: {
            shopName:"",
            shopuuid:"",
            shopowner:"",
            address:"",
            state:"",
            phone:"",
            isDelete: 0,
            lat: 0,
            lon: 0,
          },
          rules: {
            shopName: [
              { type: "string", required: true, message: "请输入商店名称" }
            ],
            shopowner: [
              { type: "string", required: true, message: "请输入商店负责人" }
            ],
            lon: [
              { type: "string", required: true, validator:validateLon}
            ],
            lat: [
              { type: "string", required: true, validator:validateLat}
            ],
            phone: [
              { type: "string", required: true, validator:globalvalidatePhone}
            ],
            address: [
              { type: "string", required: true, message: "请输入商店地址" }
            ],
          }
        },
        formModel2: {
          opened: false,
          title: "详情",
          selection: [],
          fields: {
            shopName:"",
            shopuuid:"",
            shopowner:"",
            address:"",
            state:"",
            phone:"",
            goods:"",
            addPeople:"",
            addTime:"",
            lon:"",
            lat:"",
          }
        },

        formModel33: {
          opened: false,
          title: "详情",
          selection: [],
          fields: {
            score:"",
            shopUuid:""
          },
          rules: {
            score: [
              { type: "number", required: true, validator:scoreInt}
            ],
          }

        },

        commands: {
          delete: { name: "delete", title: "删除" },
          recover: { name: "recover", title: "恢复" }
        },
        //查询搜索
        stores: {
          shop: {
            query: {
              totalCount: 0,
              pageSize: 20,
              currentPage: 1,
              Kw: "",
              isDeleted: 0,
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
            //列表显示
            columns: [
              { type: "selection", width: 50, key: "shopUuid" },
              { title: "商店名称", key: "shopName", sortable: true },
              { title: "商店地址", key: "address",
              ellipsis: true,
              tooltip: true },
              { title: "商店负责人", key: "shopowner"},
              { title: "联系方式", key: "phone" },
              // { title: "可兑换物品", key: "goods" },
              { title: "状态", key: "state" },
              { title: "可兑换积分",width:100, key: "score" },
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
        //商品界面数据
        stores2:{
          open:false,
          Goods: {
            query: {
              totalCount: 0,
              pageSize: 20,
              currentPage: 1,
              Kw: "",
              isDeleted: 0,
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
            //列表显示
            columns: [
              { type: "selection", width: 50, key: "shopUuid" },
              { title: "商店名称", key: "shopName", sortable: true },
              { title: "商店地址", key: "address"},
              { title: "商店负责人", key: "shopowner"},
              { title: "联系方式", key: "phone" },
              // { title: "可兑换物品", key: "goods" },
              { title: "状态", key: "state" },
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
          return "添加商店信息";
        }
        if (this.formModel.mode === "edit") {
          return "编辑商店信息";
        }
     
        return "";
      },
      selectedRows() {
        return this.formModel.selection;
      },
      selectedRowsId() {
        return this.formModel.selection.map(x => x.shopUuid);
      } //删除
    },
    methods: {

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
        this.url + "UploadFiles/ImportUserInfoModel/商店信息导入模板.xlsx";
    },
        //导入
     importfxx(e) {
      let inputDOM = this.$refs.inputer;
      // //console.log(inputDOM);
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
        await shopImport(this.exceldata).then(res => {
          //console.log(res.data);
          if (res.data.code == 200) {
            this.time = res.data.data.time;
            this.successmsg = res.data.data.successmsg;
            this.repeatmsg = res.data.data.repeatmsg;
            this.defaultmsg = res.data.data.defaultmsg;
            this.loadShopDispatchList();

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





      //获取商店信息数据
      loadShopDispatchList() {
        getShopList(this.stores.shop.query).then(res => {
          //console.log(res.data.data)
          this.stores.shop.data = res.data.data;
          this.stores.shop.query.totalCount = res.data.totalCount;
        });
      },
      //翻页
      handlePageChanged(page) {
        this.stores.shop.query.currentPage = page;
        this.loadShopDispatchList();
      },
      //显示条数改变
      handlePageSizeChanged(pageSize) {
        this.stores.shop.query.pageSize = pageSize;
        this.loadShopDispatchList();
      },
      //打开窗口
      handleOpenFormWindow() {
        this.formModel.opened = true;
      },
      //自动关闭窗口
      handleCloseFormWindow() {
        this.formModel.opened = false;
      },
      ShowGoods(row){
        this.stores2.open=true;
        this.doLoadGoods(row.shopUuid);
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

      //清空
      handleResetFormDispatch() {
        this.$refs["formdispatch"].resetFields();
      },
      //搜索
      handleSearchDispatch() {
        this.loadShopDispatchList();
      },
      //刷新
      handleRefresh() {
        this.loadShopDispatchList();
      },
      //右边删除（单个删除）
      handleDelete(row) {
        this.doDelete(row.shopUuid);
      },
      doDelete(ids) {
        if (!ids) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        deleteShop(ids).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.loadShopDispatchList();
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
            this.loadShopDispatchList();
            this.formModel.selection = [];
          } else {
            this.$Message.warning(res.data.message);
          }
          this.$Modal.remove();
        });
      },
      //加载商品列表
      doLoadGoods(id){
        LoadGoods({id:id,data:this.stores2.shop.query}).then(res => {
          ////console.log(res.data.data)
          this.stores2.Goods.data = res.data.data;
          this.stores2.Goods.query.totalCount = res.data.totalCount;
        });
      },

      //扣除积分
      EditScore(row){
        this.handleOpenFormWindow33();
        this.handleSwitchFormModeToEdit33();
        this.handleResetFormDispatch33();
        this.shopUuid = row.shopUuid;        
      },
      //扣除积分（保存）
      doshopEditScore() {
        this.formModel33.fields.shopUuid = this.shopUuid;
        let valid = this.validateDispatchForm333();        
        ////console.log(this.formModel33.fields.shopUuid);
        dbScore(this.formModel33.fields).then(res => {
          ////console.log(res.data.data.score);
        if(valid){
          if(this.formModel33.fields.score <= res.data.data.score){
            loadShopEditScore(this.formModel33.fields).then(res => {
            if (res.data.code === 200) {
              this.$Message.success(res.data.message);
              this.handleCloseFormWindow33(); //关闭表单
              this.loadShopDispatchList();
            } 
            else {
              this.$Message.warning(res.data.message);
            }
          });
          } 
          else{
              this.$Message.warning("商户所剩余积分不足");
          }

            }
        })
      },

      //非空验证提示
      validateDispatchForm333() {
        let _valid = false;
        this.$refs["fromeditscore"].validate(valid => {
          if (!valid) {
            this.$Message.error("请不要提交空的数据");
          } else {
            _valid = true;
          }
        });
        return _valid;
      },

      handleCloseFormWindow33() {
        this.formModel33.opened = false;
      },      
      //打开窗口
      handleOpenFormWindow33() {
        this.formModel33.opened = true;
      },
      handleSwitchFormModeToEdit33() {
        this.formModel33.mode = "edit";
      },
      //清空
      handleResetFormDispatch33() {
        this.$refs["fromeditscore"].resetFields();
      },      

      //右边编辑
      handleEdit(row) {
        this.handleOpenFormWindow();
        this.handleSwitchFormModeToEdit();
        this.handleResetFormDispatch();
        this.doLoadShopEditData(row.shopUuid);
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
        this.loadShopDispatchList();
      },
      //右边详情
      handleDetail(row) {
        ////console.log(row);
        this.formModel2.opened = true;
        loadShopDetail({ guid: row.shopUuid }).then(res => {

          this.formModel2.fields = res.data.data[0];
        });
      },
      doLoadShopEditData(id){
        loadShopEditData(id).then(res=>{
          if (res.data.code === 200) {
            this.formModel.fields = res.data.data;
          }
        })
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
      //添加商店（保存）
      docreateShopDispatch() {
        ////console.log(this.formModel.fields);
        create(this.formModel.fields).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.handleCloseFormWindow(); //关闭表单
            this.loadShopDispatchList();
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
      //编辑商店（保存）
      doEditShopDispatch() {
        //console.log(this.formModel.fields);
        editSave(this.formModel.fields).then(res => {
          //console.log(this.formModel.fields)
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.handleCloseFormWindow();
            this.loadShopDispatchList();
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
            this.docreateShopDispatch();
          }
          if (this.formModel.mode === "edit") {
            this.doEditShopDispatch();
          }
        }
      },
    },
    mounted() {

      this.loadShopDispatchList();
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


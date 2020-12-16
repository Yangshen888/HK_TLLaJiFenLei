<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.car.query.totalCount"
        :pageSize="stores.car.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div style="display: none;" slot="searcher">
          <section class="dnc-toolbar-wrap">           
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                   <FormItem>
                    <DatePicker
                      type="year"
                      v-model="time"
                      format="yyyy"
                      placement="top"
                      placeholder="输入注册时间搜索..."
                      style="width: 200px"
                      :confirm="true"
                      :editable="false"
                      @on-ok="handleSearchDispatch()"
                    
                    ></DatePicker>
                  </FormItem>              
                </Form>
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

            </Tooltip> 
          </template>
        </Table>
      </dz-table>
    </Card>

  </div>
</template>
<script>


import DzTable from "_c/tables/dz-table.vue";
import {
  getData3,
  getData4,
  loadCarDriverDetail, //详情
  deleteCarDriver, //右边删除
  batchCommand, //右边上方删除
  createCarDriver, //添加（保存）
  editCarDriver, //编辑（保存）
  loadCarDriver, //加载
  getScoreData,//加载分数
  globalvalidatePhone 
} from "@/api/grab/statistics";

export default {
  name: "house",
  components: {
    DzTable
  },
  data() {
    return {

      listMonth:[],


      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        selection: [],
        fields: {
            realName:"",
            phone:"",
            oldCard:"",
            isDeleted: 0,
            wechat:"",
            addTime:"",
            address:""
        },
        rules: {
          realName: [
            { type: "string", required: true, message: "请输入姓名" }
          ],
          phone: [
            { type: "string", required: true, validator:globalvalidatePhone}
          ]        
        }
      },
      // formModel2: {
      //   opened: false,
      //   title: "详情",
      //   selection: [],
      //   fields: {
      //     realName:"",
      //     phone:"",
      //     addTime:""
      //   }
      // },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" }
      },
      time:(new Date).getFullYear().toString(),
      //查询搜索
      stores: {
        car: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw:'',
            vuuid:this.$store.state.user.villageGuid,
            time:(new Date).getFullYear(),
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
            { type: "selection", minWidth: 50,key: "handle" },
            { title: "社区名称",minWidth: 100, key: "vname" },
            { title: "所在街道",minWidth: 100, key: "towns" },
            { title: "日期",minWidth: 70,  key: "times"},
            { title: "当天易腐垃圾比例",minWidth: 65,  key: "dayprop"},
            // { title: "周日期",minWidth: 65,  key: "weektime" },
            { title: "本周易腐垃圾比例",minWidth: 65,  key: "weekprop" },
            { title: "截至上月易腐垃圾比例",minWidth: 65,  key: "yearprop" },              
          ],
          data: [],
          data1: [],
          vname:"",
          towns:"",
        }
      }
    };
  },

  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建志愿者申请";
      }
      if (this.formModel.mode === "edit") {
        return "编辑志愿者信息";
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
    //获取志愿者信息数据
    loadCarDispatchList() {
      //console.log(this.time.getFullYear());
      this.stores.car.query.time=this.time.getFullYear();
      //console.log(this.stores.car.query);
      getData4(this.stores.car.query).then(res => {
        console.log(res.data);       
        this.stores.car.data = res.data.data;
        this.stores.car.query.totalCount=res.data.totalCount;
        // this.stores.car.query.totalCount = res.data.totalCount;
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
    doLoadCarDispatch(systemUserUuid) {
      //console.log(systemUserUuid)
      loadCarDriver({ guid: systemUserUuid }).then(res => {
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
  
    //右边编辑
    handleEdit(row) {
      this.handleOpenFormWindow();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormDispatch();
      this.doLoadCarDispatch(row.systemUserUuid);
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
      ////console.log(row);
      this.formModel2.opened = true;
      this.$refs["formCarDispatchDetail"].resetFields();
      loadCarDriverDetail({ guid: row.systemUserUuid }).then(res => {
        this.formModel2.fields = res.data.data;
      });
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
  }
};
</script>
<style>

</style>
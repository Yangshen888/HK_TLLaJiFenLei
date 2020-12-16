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
                  <FormItem>
                       <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.car.query.kw"
                      placeholder="输入用户名字"
                      @on-search="handleSearchDispatch()"
                    ></Input>
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
                <!-- <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加问题反馈"
                >添加问题反馈</Button> -->
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
<!--            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">-->
<!--              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">-->
<!--                <Button type="error" size="small" shape="circle" icon="md-trash"></Button>-->
<!--              </Tooltip>-->
<!--            </Poptip>-->

            <Tooltip placement="top" content="处理" :delay="1000" :transfer="true">
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
    <Drawer title="处理" v-model="formModel.opened" width="400" :mask-closable="false" :mask="true">
      <Form
        :model="formModel.fields"
        ref="formdispatch"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="处理结果" prop="estimate">
            <Input type="textarea" v-model="formModel.fields.estimate" placeholder="请输入处理结果" />
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>

    <Drawer title="问题反馈详情" v-model="formModel1.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel1.fields" ref="formCarDispatchDetail" label-position="left">
        <Row :gutter="16">
          <FormItem label="反馈人" prop="loginName">
            <Input readonly v-model="formModel1.fields.loginName" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="真实姓名" prop="realName">
            <Input readonly v-model="formModel1.fields.realName" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="反馈类型" prop="problemType">
            <Input readonly v-model="formModel1.fields.problemType" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="反馈内容" prop="problemContent">
            <Input readonly v-model="formModel1.fields.problemContent" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="反馈时间" prop="addTime">
            <Input readonly v-model="formModel1.fields.addTime" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="家庭地址" prop="address">
            <Input readonly v-model="formModel1.fields.address" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" prop="remarks">
            <Input readonly v-model="formModel1.fields.remarks" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="处理结果" prop="estimate">
            <Input readonly type="textarea" v-model="formModel1.fields.estimate" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="处理时间" prop="esttime">
            <Input readonly v-model="formModel1.fields.esttime" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="处理人" prop="estpeople">
            <Input readonly v-model="formModel1.fields.estpeople" placeholder="无" />
          </FormItem>
        </Row>
        
      <div class="demo-drawer-footer">
        <Button style="margin-left: 30px" icon="md-close" @click="formModel1.opened = false">取 消</Button>
      </div>
      </Form>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getCarDriverList,
  showDetail, //详情
  deleteCarDriver, //右边删除
  batchCommand, //右边上方删除
  createCarDriver, //添加（保存）
  editCarDriver, //编辑（保存）
  loadCarDriver, //加载
  globalvalidatePhone,
} from "@/api/person/response";
import { getshequ } from "@/api/base/house";
export default {
  name: "response",
  components: {
    DzTable,
  },
  data() {
    return {
      formModel: {
        opened: false,
        title: "处理",
        selection: [],
        fields: {
          guid: "",
          estimate: "",
        },
      },
      formModel1: {
        opened: false,
        title: "问题反馈详情",
        selection: [],
        fields: {
          guid: "",
          loginName:"",
          problemType:"",
          problemContent:"",
          address:"",
          realName:"",
          addTime:"",
          estimate:"",
          esttime:"",
          estpeople:"",
          remarks:""
        },
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
            vuuid:this.$store.state.user.villageGuid,
            street:"",
            ccmmunity:"",
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
            //社区集合
            college: ["全部" ],     
             //街道集合
            town: ["全部" ],  
          },
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "handle" },
            {
              title: "微信名称",
              key: "loginName",
              sortable: true,
              ellipsis: true,
              tooltip: true,
            },
            {
              title: "家庭住址",
              key: "address",
              ellipsis: true,
              tooltip: true,
            },
            {
              title: "反馈内容",
              key: "problemContent",
              ellipsis: true,
              tooltip: true,
            },
            { title: "添加时间", key: "addTime" },
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
      getCarDriverList(this.stores.car.query).then(res => {
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
    doLoadCarDispatch(row) {
      let data = row;
      showDetail(data).then((res) => {
        this.formModel.fields = res.data.data;
        this.formModel.fields.guid = data;
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
      this.doDelete(row.systemUserUuid);
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
       //申请社区下拉框
     loadDepartmentList1() {
       let data=this.$store.state.user.villageGuid;
       getshequ(data).then(res => {
        //console.log(res.data.data);
        this.list33 = res.data.data;
        let townn = Array.from(new Set(this.list33.map((x) => x.towns)));
        //console.log(townn);
        this.stores.car.sources.town = townn;
      });
      
    },
    xz(e) {
      let college = this.list33.filter((x) => x.towns == e);
      this.stores.car.sources.college = college.map((x) => x.vname);
      this.loadCarDispatchList();
    },
     sq(e) {
      if (e == "全部") {
        this.stores.car.query.ccmmunity = "";
      }
      this.loadCarDispatchList();  
    },
    //右边编辑
    handleEdit(row) {
      this.handleOpenFormWindow();
      this.handleResetFormDispatch();
      console.log(row);
      this.doLoadCarDispatch(row.questionPersonUuid);
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
      console.log(row);
      this.formModel1.opened = true;
      this.$refs["formCarDispatchDetail"].resetFields();
      let data=row.questionPersonUuid;
      showDetail(data).then((res) => {
        console.log(res);
        if (res.data.code == 200) {
          this.formModel1.fields = res.data.data;
          this.formModel1.fields.guid = data;
          this.formModel1.fields.loginName=row.loginName;
          this.formModel1.fields.realName=row.realName;
          this.formModel1.fields.address=row.address;
        }
      });
    },
    //添加按钮（志愿者申请）
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
      this.formModel.fields.loginName = this.$store.state.user.userName;

    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    //添加志愿者（保存）
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
    //编辑志愿者（保存）
    doEditCarDispatch() {
      editCarDriver(this.formModel.fields).then((res) => {
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
      if (this.formModel.fields.estimate==null ||this.formModel.fields.estimate=="") {
        this.$Message.warning("请输入处理结果");
        return;
      }
          this.doEditCarDispatch();
    },
    
  },
  mounted() {
    this.loadDepartmentList1();
    this.loadCarDispatchList();
  },
};
</script>
<style>
</style>

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
                      placeholder="输入调度人搜索..."
                      @on-search="handleSearchDispatch()"
                    ></Input>

                    <Select
                      v-model="stores.car.query.kw1"
                      @on-change="handleSearchDispatch()"
                      placeholder="科室"
                      style="width:80px;"
                    >
                      <Option
                        v-for="item in stores.car.sources.department"
                        :value="item.departmentUuid"
                        :key="item.departmentUuid"
                      >{{item.departmentName}}</Option>
                    </Select>
                    <Select
                      v-model="stores.car.query.kw2"
                      @on-change="handleSearchDispatch()"
                      placeholder="状态"
                      style="width:80px;"
                    >
                      <Option
                        v-for="item in stores.car.sources.auditState"
                        :value="item.value"
                        :key="item.value"
                      >{{item.text}}</Option>
                    </Select>
                    </FormItem>
                    <FormItem>
                    <DatePicker
                      type="daterange"
                      v-model="stores.car.query.kw3"
                      format="yyyy/MM/dd"
                      placement="top"
                      placeholder="输入时间搜索..."
                      style="width: 200px"
                      :confirm="true"
                      :editable="false"
                      @on-ok="handleSearchDispatch()"
                      >
                    </DatePicker>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <!-- <Button
                    class="txt-success"
                    icon="md-redo"
                    title="恢复"
                    @click="handleBatchCommand('recover')"
                  ></Button>-->

                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="公车调度申请"
                >公车调度申请</Button>
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
          <!-- <template slot-scope="{row,index}" slot="status">
            <Tag :color="renderStatus(row.status).color">{{renderStatus(row.status).text}}</Tag>
          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <!-- <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
        <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
          <Button type="error" size="small" shape="circle" icon="md-trash"></Button>
        </Tooltip>
            </Poptip>-->
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
            <Tooltip placement="top" content="审核" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="error"
                size="small"
                shape="circle"
                icon="md-share-alt"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="success"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetails(row)"
              ></Button>
            </Tooltip>
            <!-- <Tooltip placement="top" content="分配角色" :delay="1000" :transfer="true">-->
            <!-- <Button type="success" size="small" shape="circle" icon="md-contacts" @click="handleAssignRole(row)"></Button>-->
            <!-- </Tooltip>-->
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
        :model="formModel2.fields"
        ref="dispatch"
        :rules="formModel2.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="16">
            <FormItem label="调度人" prop="realName">
              <Input v-model="formModel2.fields.realName" :readonly="true" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="16">
            <FormItem label="调度时间" prop="applyTime">
              <DatePicker
                :value="formModel2.fields.applyTime"
                type="date"
                placeholder="Select date"
                style="width:240px"
              ></DatePicker>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="16">
            <FormItem label="科室名" prop="departmentName">
              <Input v-model="formModel2.fields.departmentName" placeholder="科室名" :readonly="true" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="16">
            <FormItem label="调度理由" prop="applyReason">
              <Input v-model="formModel.fields.num" placeholder="请输入调度理由" type="textarea" :rows="5" />
            </FormItem>
          </Col>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>

        <Button
          style="margin-left: 50px"
          icon="ios-share-alt"
          type="success"
          @click="handleSubmitToAuditConsumable"
        >提交</Button>

        <!-- <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button> -->
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import { getCarDispatchList } from "../../api/car/carDispatch";
import { loaddepartmentListDetail } from "../../api/rbac/department"

export default {
  name: "carDispatch",
  components: {
    DzTable
  },
  data() {
    return {
      formModel: {
        opened: false,
        mode: "",
        fields: {}
      },
      formModel2: {
        fields: {
          realName: "",
          applyTime: "",
          departmentName: "",
          applyReason: ""
        }
      },
      stores: {
        car: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            kw2:"",
            kw3:"",
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
            //科室集合
            department: [{departmentUuid:"",departmentName:"全部"}],
            //转态集合
            auditState: [
              { value: "", text: "全部" },
              { value: 0, text: "保存中" },
              { value: 1, text: "待审核" },
              { value: 2, text: "科室领导审核通过" },
              { value: 3, text: "科室领导审核未通过" },
              { value: 4, text: "分管领导审核通过" },
              { value: 5, text: "分管领导审核未通过" },
              { value: 6, text: "车队队长审核通过" },
              { value: 7, text: "车队队长审核不通过" },
              ]
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "调度人", key: "realName", sortable: true },
            { title: "调度时间", key: "applyTime" },
            { title: "科室名", key: "departmentName" },
            { title: "调度理由", key: "applyReason" },

            { title: "审核状态", key: "auditState", slot: "auditState" },
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
        return "创建调度申请";
      }
      if (this.formModel.mode === "edit") {
        return "编辑调度信息";
      }
      return "";
    }
  },
  methods: {
    //获取调度数据
    loadCarDispatchList() {
      getCarDispatchList(this.stores.car.query).then(res => {
        //console.log(res.data.data);
        this.stores.car.data = res.data.data;
        this.stores.car.query.totalCount = res.data.totalCount;
      });
    },
    //获取科室下拉列表
    loadDepartmentList(){
      loaddepartmentListDetail().then(res=>{
        //console.log("科室");
        //console.log(res.data.data);

        Array.prototype.push.apply(this.stores.car.sources.department,res.data.data);
      });
    },
    //翻页
    handlePageChanged(page) {
      this.stores.user.query.currentPage = page;
      this.loadCarDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.user.query.pageSize = pageSize;
      this.loadCarDispatchList();
    },
    //行样式
    rowClsRender(row, index) {
      // if (row.isDeleted) {
      //   return "table-row-disabled";
      // }
      // return "";
    },
    //状态显示
    renderAuditState(state) {
      var stateText = "未知";
      switch (state) {
        case 0:
          stateText = "保存中";
          break;
        case 1:
          stateText = "待审核";
          break;
        case 2:
          stateText = "科室领导审核通过";
          break;
        case 3:
          stateText = "科室领导审核未通过";
          break;
        case 4:
          stateText = "分管领导审核通过";
          break;
        case 5:
          stateText = "分管领导审核未通过";
          break;
        case 6:
          stateText = "车队队长审核通过";
          break;
        case 7:
          stateText = "车队队长审核不通过";
          break;
      }
      return stateText;
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
    },
    //右边编辑
    handleEdit(row) {
      this.handleSwitchFormModeToEdit();
      // this.handleResetFormUser();
      // this.doLoadUser(row.systemUserUuid);
    },
    handleSwitchFormModeToEdit(){
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
    //详情
    handleDetails(row){},
    //打开窗口
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    //添加按钮（公车调度申请）
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      // this.handleResetFormUser();
      //console.log("xxxxxxxxxx");
      //console.log(this.$store.state.user);
      this.formModel2.fields.realName = this.$store.state.user.userName;
      this.formModel2.fields.departmentName=this.$store.state.user.departmentName;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    }
  },
  mounted() {
    this.loadDepartmentList();
    this.loadCarDispatchList();
    
  }
};
</script>
<style>
</style>
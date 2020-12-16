<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.vote.query.totalCount"
        :pageSize="stores.vote.query.pageSize"
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
                      v-model="stores.vote.query.kw"
                      placeholder="输入发起人搜索..."
                      @on-search="handleSearchDispatch()"
                    ></Input>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.vote.query.kw1"
                      placeholder="输入活动标题搜索..."
                      @on-search="handleSearchDispatch()"
                    ></Input>
                    <DatePicker
                      v-model="stores.vote.query.kw2"
                      type="date"
                      placeholder="选择活动开始日期搜索"
                      style="width: 200px"
                      @on-change="handleSearchDispatch()"
                    ></DatePicker>

                    <!-- <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.vote.query.kw1"
                      placeholder="输入调度科室搜索..."
                      @on-search="handleSearchDispatch()"
                    >-->
                    <!-- <Select
                        slot="prepend"
                        v-model="stores.vote.query.isDeleted"
                        @on-change="handleSearchUser"
                        placeholder="删除状态"
                        style="width:60px;"
                      >
                        <Option
                          v-for="item in stores.vote.sources.isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                    </Select>-->
                    <!--                      <Select-->
                    <!--                        slot="prepend"-->
                    <!--                        v-model="stores.user.query.status"-->
                    <!--                        @on-change="handleSearchUser"-->
                    <!--                        placeholder="用户状态"-->
                    <!--                        style="width:60px;"-->
                    <!--                      >-->
                    <!--                        <Option-->
                    <!--                          v-for="item in stores.user.sources.statusSources"-->
                    <!--                          :value="item.value"-->
                    <!--                          :key="item.value"-->
                    <!--                        >{{item.text}}</Option>-->
                    <!--                      </Select>-->
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
                  title="添加投票活动"
                >添加投票活动</Button>
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
          :data="stores.vote.data"
          :columns="stores.vote.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <!-- <template slot-scope="{row,index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="status">
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
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="success"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="发布" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="error"
                size="small"
                shape="circle"
                icon="md-share-alt"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <!--            <Tooltip placement="top" content="分配角色" :delay="1000" :transfer="true">-->
            <!--              <Button type="success" size="small" shape="circle" icon="md-contacts" @click="handleAssignRole(row)"></Button>-->
            <!--            </Tooltip>-->
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form
        :model="formModel2.fields"
        ref="dispatch"
        :rules="formModel2.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="发起人" prop="realName">
              <Input v-model="formModel2.fields.realName" :readonly="true" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="标题" prop="realName">
              <Input v-model="formModel2.fields.realName" :readonly="true" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="16">
            <FormItem label="发起时间" prop="applyTime">
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
              <Input
                v-model="formModel.fields.num"
                placeholder="请输入调度理由"
                type="textarea"
                :rows="5"
              />
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
import { getVoteinitiateList } from "../../api/vote/voteinitiate";

export default {
  name: "voteinitiate",
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
          title:"",
          applyTime: "",
          departmentName: "",
          applyReason: ""
        }
      },
      stores: {
        vote: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            kw2: "",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {},
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "发起人", key: "realName", sortable: true },
            { title: "发起时间", key: "startTime" },
            { title: "截止时间", key: "endTime" },
            { title: "标题", key: "title" },
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
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static"
      }
    };
  },
  computed: {
    formTitle() {
      //console.log(this.formModel.mode);
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
    loadVoteinitiateList() {
      getVoteinitiateList(this.stores.vote.query).then(res => {
        //console.log(res.data.data);
        this.stores.vote.data = res.data.data;
        this.stores.vote.query.totalCount = res.data.totalCount;
      });
    },
    //获取科室下拉列表
    loadDepartmentList() {
      //   loaddepartmentListDetail().then(res=>{
      //     //console.log("科室");
      //     //console.log(res.data.data);
      //     Array.prototype.push.apply(this.stores.car.sources.department,res.data.data);
      //   });
    },
    //翻页
    handlePageChanged(page) {
      this.stores.user.query.currentPage = page;
      this.loadVoteinitiateList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.user.query.pageSize = pageSize;
      this.loadVoteinitiateList();
    },
    //行样式
    rowClsRender(row, index) {
      // if (row.isDeleted) {
      //   return "table-row-disabled";
      // }
      // return "";
    },
    
    //搜索
    handleSearchDispatch() {
      this.loadVoteinitiateList();
    },
    //刷新
    handleRefresh() {
      this.loadVoteinitiateList();
    },
    //删除
    handleBatchCommand() {},
    // handleDelete(row) {
    //   this.doDelete(row.systemUserUuid);
    // },
    //编辑
    handleEdit(row) {
      this.handleSwitchFormModeToEdit();
      // this.handleResetFormUser();
      // this.doLoadUser(row.systemUserUuid);
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
      this.loadVoteinitiateList();
    },
    //打开窗口
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    //添加按钮
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      // this.handleResetFormUser();
      //console.log("xxxxxxxxxx");
      //console.log(this.$store.state.user);
      this.formModel2.fields.realName = this.$store.state.user.userName;
      this.formModel2.fields.departmentName = this.$store.state.user.departmentName;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    }
  },
  mounted() {
    //this.loadDepartmentList();
    this.loadVoteinitiateList();
  }
};
</script>
<style>
</style>
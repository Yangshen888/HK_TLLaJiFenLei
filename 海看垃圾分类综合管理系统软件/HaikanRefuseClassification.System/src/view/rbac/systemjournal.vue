<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.user.query.totalCount"
        :pageSize="stores.user.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="5">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.user.query.kw"
                      placeholder="输入操作用户名搜索..."
                      @on-search="handleSearchUser()">
                    </Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="5">
                <DatePicker type="date" format="yyyy-MM-dd" placeholder="请选择日期筛选" style="width: 200px" @on-change="handleSearchUser"></DatePicker>
              </Col>
              <Col span="14" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    icon="md-refresh"
                    title="刷新"
                    type="primary"
                    @click="handleRefresh"
                  ></Button>
                </ButtonGroup>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          :highlight-row="true"
          :data="stores.user.data"
          :columns="stores.user.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <template slot-scope="{ row, index }" slot="status">
            <Tag :color="renderStatus(row.status).color">{{
              renderStatus(row.status).text
            }}</Tag>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <Tooltip
              placement="top"
              content="详情"
              :delay="1000"
              :transfer="true"
            >
              <Button
                type="primary"
                icon="ios-arrow-forward"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      title="详情"
      v-model="formModel.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form
        :model="formModel.fields"
        ref="formUser"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="操作用户名">
              <Input v-model="formModel.fields.userName" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="操作时间">
              <Input v-model="formModel.fields.operationTime" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="IP地址">
              <Input v-model="formModel.fields.iPAddress" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="操作类型">
              <Input v-model="formModel.fields.type" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="24">
            <FormItem label="操作内容">
              <Input type="textarea" v-model="formModel.fields.operationContent" :rows="6" />
            </FormItem>
          </Col>
        </Row>
      </Form>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import { getLogList, getLogByID } from "@/api/rbac/systemlog";
import { doCustomTimes } from "../../libs/util";

export default {
  name: "rbac_user_page",
  components: {
    DzTable,
  },
  data() {
    return {
      commands: {
        delete: { name: "delete", title: "禁用" },
        recover: { name: "recover", title: "恢复" },
        // forbidden: { name: "forbidden", title: "禁用" },
        // normal: { name: "normal", title: "启用" }
      },
      xqshow: false,
      family: [],
      isshow: false,
      isshow2: false,
      sVillage: "",
      treeData: [],
      treeshow: false,
      streets: [],
      community: [],
      biotope: [],
      roleLevel: 0,
      sysrolelist: [],
      formModel: {
        opened: false,
        selection: [],
        fields: {
          userName: "",
          operationTime: "",
          iPAddress: "",
          type: "",
          operationContent: "",
          isDeleted: 0,
          systemLogUUID: "",
        },
      },

      formModel22: {
        opened: false,
        title: "创建用户",
        mode: "create",
        selection: [],
        fields: {
          shopName: "",
          shopUuid: "",
          id: "",
        },
      },

      formAssignRole: {
        userGuid: "",
        opened: false,
        ownedRoles: [],
        inited: false,
        roles: [],
      },
      stores: {
        user: {
          model1: "",

          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw:'',
            operationTime: "",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          // sources: {
          //   userTypes: [
          //     { value: 0, text: "超级管理员" },
          //     { value: 1, text: "管理员" },
          //     { value: 2, text: "普通用户" },
          //   ],
          //   isDeletedSources: [
          //     { value: -1, text: "全部" },
          //     { value: 0, text: "正常" },
          //     { value: 1, text: "已禁" },
          //   ],
          //   statusSources: [
          //     { value: -1, text: "全部" },
          //     { value: 0, text: "禁用" },
          //     { value: 1, text: "正常" },
          //   ],
          //   statusFormSources: [
          //     { value: 0, text: "禁用" },
          //     { value: 1, text: "正常" },
          //   ],
          // },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            {
              title: "操作用户名",
              minWidth: 85,
              key: "userName",
              sortable: true,
            },
            { title: "操作时间", minWidth: 85, key: "operationTime" },
            { title: "操作类型", minWidth: 140, key: "type" },
            { title: "IP地址", minWidth: 140, key: "iPAddress" },
            {
              title: "操作",
              align: "center",
              width: 120,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
          data2: [],
          villages: [],
          resregion: [],
        },
      },
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static",
      },
      rolelist: [],
      checkList: [],
      departmentlist: [],
    };
  },
  computed: {
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.systemLogUuid);
    },
  },
  methods: {
    //不能删加载
    loadUserList() {
      getLogList(this.stores.user.query).then((res) => {
        console.log('传回来的数据')
        console.log(res.data.data);
        this.stores.user.data = res.data.data;
        this.stores.user.query.totalCount = res.data.totalCount;
      });
    },
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    handleSwitchFormModeToEdit() {
      // this.formModel.mode = "edit";
      this.handleOpenFormWindow();
    },

    //点击编辑按钮触发事件
    async handleEdit(row) {
      this.handleSwitchFormModeToEdit();
      this.doLoadUser(row.systemLogUuid);
    },

    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadUserList();
    },
    doLoadUser(systemLogUuid) {
      console.log(systemLogUuid)
      getLogByID({ guid: systemLogUuid}).then((res) => {
        console.log('单个数据')
        console.log(res);
        this.formModel.fields = res.data.data[0];
      });
    },
    handleSearchUser(res) {
      console.log('这里')
      console.log(res)
      this.stores.user.query.operationTime=res
      this.loadUserList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.user.query.sort.direction = column.order;
      this.stores.user.query.sort.field = column.key;
      this.loadUserList();
    },
    handlePageChanged(page) {
      this.stores.user.query.currentPage = page;
      this.loadUserList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.user.query.pageSize = pageSize;
      this.loadUserList();
    },
   
    renderStatus(status) {
      let _status = {
        color: "success",
        text: "正常",
      };
      switch (status) {
        case 0:
          _status.text = "禁用";
          _status.color = "error";
          break;
      }
      return _status;
    },
  },
  mounted() {
    this.loadUserList();
  },
};
</script>

<style>
</style>

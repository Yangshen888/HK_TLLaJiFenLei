<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.role.query.totalCount"
        :columns="stores.role.columns"
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
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.role.query.kw"
                      placeholder="输入角色名称搜索..."
                      @on-search="handleSearchRole()"
                    >
                     <!-- <Select
                       slot="prepend"
                       v-model="stores.role.query.isDeleted"
                       @on-change="handleSearchRole"
                       placeholder="禁用状态"
                       style="width:60px;"
                     >
                       <Option
                         v-for="item in stores.role.sources.isDeletedSources"
                         :value="item.value"
                         :key="item.value"
                          >{{item.text}}</Option>
                     </Select> -->

                    </Input>
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
                  ></Button>
                 <Button
                   class="txt-success"
                   icon="md-redo"
                   title="恢复"
                   @click="handleBatchCommand('recover')"
                 ></Button> -->
                  <!--                  <Button-->
                  <!--                    class="txt-danger"-->
                  <!--                    icon="md-hand"-->
                  <!--                    title="禁用"-->
                  <!--                    @click="handleBatchCommand('forbidden')"-->
                  <!--                  ></Button>-->
                  <!--                  <Button-->
                  <!--                    class="txt-success"-->
                  <!--                    icon="md-checkmark"-->
                  <!--                    title="启用"-->
                  <!--                    @click="handleBatchCommand('normal')"-->
                  <!--                  ></Button>-->
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增角色"
                >新增角色</Button>
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
          :data="stores.role.data"
          :columns="stores.role.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <!-- <template slot-scope="{row,index}" slot="status">
            <Tag :color="renderStatus(row.status).color">{{renderStatus(row.status).text}}</Tag>
          </template> -->
          <template slot-scope="{row,index}" slot="fixation">
            <Tag :color="renderfixation(row.isFixation).color">{{renderfixation(row.isFixation).text}}</Tag>
          </template>
          <template slot-scope="{ row, index }" slot="action">
           <!-- <Poptip
             confirm
             :transfer="true"
             title="确定要删除吗?"
             @on-ok="handleDelete(row)"
           >
             <Tooltip placement="top" content="禁用" :delay="1000" :transfer="true">
               <Button type="error" size="small" shape="circle" icon="md-close"></Button>
             </Tooltip>
           </Poptip> -->
            <Tooltip v-if="row.isFixation==0" placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button v-can="'edit'" type="primary" size="small" shape="circle" icon="md-create" @click="handleEdit(row)"></Button>
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
      :mask="false"
      :styles="styles"
    >
      <Form :model="formModel.fields" ref="formRole" :rules="formModel.rules" label-position="left">
        <FormItem label="角色名称" prop="roleName" label-position="left">
          <Input v-model="formModel.fields.roleName" placeholder="请输入角色名称"/>
        </FormItem>
<!--        <FormItem label="角色状态" label-position="left">-->
<!--          <i-switch size="large" v-model="formModel.fields.status" :true-value="1" :false-value="0">-->
<!--            <span slot="open">正常</span>-->
<!--            <span slot="close">禁用</span>-->
<!--          </i-switch>-->
<!--        </FormItem>-->
<!--        <FormItem label="备注" label-position="top">-->
<!--          <Input-->
<!--            type="textarea"-->
<!--            v-model="formModel.fields.description"-->
<!--            :rows="4"-->
<!--            placeholder="角色备注信息"-->
<!--          />-->
<!--        </FormItem>-->
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitRole">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
  import DzTable from "_c/tables/dz-table.vue";
import {
  getRoleList,
  createRole,
  loadRole,
  editRole,
  deleteRole,
  batchCommand
} from "@/api/rbac/role";
export default {
  name: "rbac_role_page",
  components: {
    DzTable
  },
  data() {
    return {
      commands: {
        delete: { name: "delete", title: "禁用" },
        recover: { name: "recover", title: "恢复" },
        normal: { name: "normal", title: "启用" }
      },
      formModel: {
        opened: false,
        title: "创建角色",
        mode: "create",
        selection: [],
        fields: {
          roleName: "",
        },
        rules: {
            roleName: [
            {
              type: "string",
              required: true,
              message: "请输入角色名称",
              min: 2
            }
          ]
        }
      },
      stores: {
        role: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "CreatedOn"
              }
            ]
          },
          sources: {
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已删" }
            ],
            statusSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" }
            ],
            statusFormSources: [
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" }
            ]
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "角色名称", key: "roleName", sortable: true },
            { title: "创建时间", ellipsis: true, tooltip: true,  key: "addTime" },
            { title: "创建者", key: "addPeople" },
            { title: "是否内置", key: "isFixation",slot:'fixation' },
            { title: "操作", align: "center", key: "handle", width: 150, className: "table-command-column",slot:"action"},
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
      if (this.formModel.mode === "create") {
        return "创建角色";
      }
      if (this.formModel.mode === "edit") {
        return "编辑角色";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.systemRoleUuid);
    }
  },
  methods: {
    loadRoleList() {
      getRoleList(this.stores.role.query).then(res => {
        //console.log(res.data);
        this.stores.role.data = res.data.data;
        this.stores.role.query.totalCount = res.data.totalCount;
      });
    },
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.handleOpenFormWindow();
    },
    handleEdit(params) {
      this.handleSwitchFormModeToEdit();
      this.handleResetFormRole();
      this.doLoadRole(params.systemRoleUuid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadRoleList();
    },
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormRole();
    },
    handleSubmitRole() {
      let valid = this.validateRoleForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateRole();
        }
        if (this.formModel.mode === "edit") {
          this.doEditRole();
        }
      }
    },
    handleResetFormRole() {
      this.$refs["formRole"].resetFields();
    },
    doCreateRole() {
      createRole(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadRoleList();
        } else {
          this.$Message.warning(res.data.message);
        }
        this.handleCloseFormWindow();
      });
    },
    doEditRole() {
      editRole(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadRoleList();
        } else {
          this.$Message.warning(res.data.message);
        }
        this.handleCloseFormWindow();
      });
    },
    validateRoleForm() {
      let _valid = false;
      this.$refs["formRole"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
          _valid = false;
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadRole(systemRoleUuid) {
      loadRole({ guid: systemRoleUuid}).then(res => {
        this.formModel.fields = res.data.data;
      });
    },
    handleDelete(params) {
      this.doDelete(params.systemRoleUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteRole(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadRoleList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
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
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadRoleList();
          this.formModel.selection=[];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchRole() {
      this.loadRoleList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handlePageChanged(page) {
      this.stores.role.query.currentPage = page;
      this.loadRoleList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.role.query.pageSize = pageSize;
      this.loadRoleList();
    },
    renderfixation(fixation){
      let data={
        color:'',
        text:'',
      }
      if(fixation==0){
        data.color='#5cadff';
        data.text='自定义';
      }
      if(fixation==1){
        data.color='#19be6b';
        data.text='内置';
      }
      return data;
    }
  },
  mounted() {
    this.loadRoleList();
  }
};
</script>

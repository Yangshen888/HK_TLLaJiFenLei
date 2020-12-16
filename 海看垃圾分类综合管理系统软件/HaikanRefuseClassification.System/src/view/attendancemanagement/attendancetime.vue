<template>
    <div>
      <Card>
        <dz-table
          :totalCount="stores.worktime.query.totalCount"
          :pageSize="stores.worktime.query.pageSize"
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
                        v-model="stores.worktime.query.kw"
                        placeholder="输入关键字搜索..."
                        @on-search="handleSearchWorktime()"
                      >
                        <Select
                          slot="prepend"
                          v-model="stores.worktime.query.isDeleted"
                          @on-change="handleSearchWorktime"
                          placeholder="删除状态"
                          style="width:60px;"
                        >
                          <Option
                            v-for="item in stores.worktime.sources.isDeletedSources"
                            :value="item.value"
                            :key="item.value"
                          >{{item.text}}</Option>
                        </Select>
                      </Input>
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
                    <Button
                      class="txt-success"
                      icon="md-redo"
                      title="恢复"
                      @click="handleBatchCommand('recover')"
                    ></Button>
                    <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                  </ButtonGroup>
                  <Button
                    v-can="'create'"
                    icon="md-create"
                    type="primary"
                    @click="handleShowCreateWindow"
                    title="新增时间"
                  >新增时间</Button>
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
            :data="stores.worktime.data"
            :columns="stores.worktime.columns"
            @on-select="handleSelect"
            @on-selection-change="handleSelectionChange"
            @on-refresh="handleRefresh"
            :row-class-name="rowClsRender"
            @on-page-change="handlePageChanged"
            @on-page-size-change="handlePageSizeChanged"
            @on-sort-change="handleSortChange"
          >
            <template slot-scope="{ row, index }" slot="action">
              <Poptip
                confirm
                :transfer="true"
                title="确定要删除吗?"
                @on-ok="handleDelete(row)"
              >
                <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                  <Button type="error" size="small" shape="circle" icon="md-trash"></Button>
                </Tooltip>
              </Poptip>
              <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
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
        <Form :model="formModel.fields" ref="formWorktime" :rules="formModel.rules" label-position="left">
          <FormItem label="上班时间" prop="startTime" label-position="left">
            <TimePicker type="time" v-model="formModel.fields.startTime" placeholder="请输入上班时间"/>
          </FormItem>
          <FormItem label="下班时间" prop="endTime" label-position="left">
            <TimePicker type="time" v-model="formModel.fields.endTime" placeholder="请输入下班时间"/>
          </FormItem>
        </Form>
        <div class="demo-drawer-footer">
          <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitWorktime">保 存</Button>
          <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
        </div>
      </Drawer>
    </div>
</template>

<script>
    import DzTable from "_c/tables/dz-table.vue";
    import {
        getWorktimeList,
        createWorktime,
        loadWorktime,
        editWorktime,
        deleteWorktime,
        batchCommand
    } from "@/api/attendancemanagement/worktime";
    export default {
        name: "attendancetime",
        components: {
            DzTable
        },
        data() {
            return {
                commands: {
                    delete: { name: "delete", title: "删除" },
                    recover: { name: "recover", title: "恢复" },
                },
                formModel: {
                    opened: false,
                    title: "创建时间",
                    mode: "create",
                    selection: [],
                    fields: {
                        startTime: "",
                        endTime:""
                    },
                    rules: {
                        startTime:[
                            {required:true,message:"请选择上班时间"}
                        ],
                        endTime:[
                            {required:true,message:"请选择上班时间"}
                        ]
                    }
                },
                stores: {
                    worktime: {
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
                                    field: "ID"
                                }
                            ]
                        },
                        sources: {
                            isDeletedSources: [
                                { value: -1, text: "全部" },
                                { value: 0, text: "正常" },
                                { value: 1, text: "已删" }
                            ],
                        },
                        columns: [
                            { type: "selection", width: 50, key: "handle" },
                            { title: "上班时间", key: "startTime"},
                            { title: "下班时间", key: "endTime"},
                            { title: "操作", align: "center", width: 150, className: "table-command-column",slot:"action" }
                        ],
                        data: []
                    }
                },
                styles: {
                    height: "calc(100% - 55px)",
                    overflow: "auto",
                    paddingBottom: "53px",
                    position: "static"
                },
            };
        },
        computed: {
            formTitle() {
                if (this.formModel.mode === "create") {
                    return "创建用户";
                }
                if (this.formModel.mode === "edit") {
                    return "编辑用户";
                }
                return "";
            },
            selectedRows() {
                return this.formModel.selection;
            },
            selectedRowsId() {
                return this.formModel.selection.map(x => x.workTimeUuid);
            }
        },
        methods:{
            loadworktimeList() {
                getWorktimeList(this.stores.worktime.query).then(res => {
                    this.stores.worktime.data = res.data.data;
                    this.stores.worktime.query.totalCount = res.data.totalCount;
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
            handleEdit(row) {
                this.handleSwitchFormModeToEdit();
                this.handleResetFormWorktime();
                this.doLoadWorktime(row.workTimeUuid);
            },
            handleSelect(selection, row) {},
            handleSelectionChange(selection) {
                this.formModel.selection = selection;
            },
            handleRefresh() {
                this.loadworktimeList();
            },
            handleShowCreateWindow() {
                this.handleSwitchFormModeToCreate();
                this.handleOpenFormWindow();
                this.handleResetFormWorktime();
            },
            handleSubmitWorktime() {
                let valid = this.validateWorktimeForm();
                if (valid) {
                    if (this.formModel.mode === "create") {
                        this.doCreateWorktime();
                    }
                    if (this.formModel.mode === "edit") {
                        this.doEditWorktime();
                    }
                }
            },
            handleResetFormWorktime() {
                this.$refs["formWorktime"].resetFields();
            },
            doCreateWorktime() {
                createWorktime(this.formModel.fields).then(res => {
                    if (res.data.code === 200) {
                        this.$Message.success(res.data.message);
                        this.handleCloseFormWindow();
                        this.loadworktimeList();
                    } else {
                        this.$Message.warning(res.data.message);
                    }
                });
            },
            doEditWorktime() {
                editWorktime(this.formModel.fields).then(res => {
                    if (res.data.code === 200) {
                        this.$Message.success(res.data.message);
                        this.handleCloseFormWindow();
                        this.loadworktimeList();
                    } else {
                        this.$Message.warning(res.data.message);
                    }
                });
            },
            validateWorktimeForm() {
                let _valid = false;
                this.$refs["formWorktime"].validate(valid => {
                    if (!valid) {
                        this.$Message.error("请完善表单信息");
                    } else {
                        _valid = true;
                    }
                });
                return _valid;
            },
            doLoadWorktime(workTimeUuid) {
                loadWorktime({ guid: workTimeUuid}).then(res => {
                    this.formModel.fields = res.data.data;
                });
            },
            handleDelete(row) {
                this.doDelete(row.workTimeUuid);
            },
            doDelete(ids) {
                if (!ids) {
                    this.$Message.warning("请选择至少一条数据");
                    return;
                }
                deleteWorktime(ids).then(res => {
                    if (res.data.code === 200) {
                        this.$Message.success(res.data.message);
                        this.loadworktimeList();
                        this.formModel.selection = [];
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
                        this.loadworktimeList();
                        this.formModel.selection = [];
                    } else {
                        this.$Message.warning(res.data.message);
                    }
                    this.$Modal.remove();
                });
            },
            handleSearchWorktime() {
                this.loadworktimeList();
            },
            rowClsRender(row, index) {
                if (row.isDeleted) {
                    return "table-row-disabled";
                }
                return "";
            },
            handleSortChange(column) {
                this.stores.worktime.query.sort.direction = column.order;
                this.stores.worktime.query.sort.field = column.key;
                this.loadworktimeList();
            },
            handlePageChanged(page) {
                this.stores.worktime.query.currentPage = page;
                this.loadworktimeList();
            },
            handlePageSizeChanged(pageSize) {
                this.stores.worktime.query.pageSize = pageSize;
                this.loadworktimeList();
            },
        },
        mounted() {
            this.loadworktimeList();
        }
    }
</script>

<style scoped>

</style>

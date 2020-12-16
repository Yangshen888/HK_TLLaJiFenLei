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
                        placeholder="输入督导员搜索..."
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
          <FormItem label="督导员：" prop="addPeople">
            <Input v-model="formModel.fields.addPeople" placeholder="请输入督导员姓名" :disabled="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="检查时间：" prop="addTime">
            <Input v-model="formModel.fields.addTime" placeholder="请输入检查时间" :disabled="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="地址编码" prop="addresscode">
            <Input v-model="formModel.fields.addresscode" :disabled="true" />
          </FormItem>
        </Row>
        <div style="margin:0 50px;">
            <Row :gutter="16">
            <!-- <FormItem label="分类照片"> -->
                <Col span="6"> 
                <div style="height:80px;line-height:80px; margin-bottom:20px;" v-for="(item,index) in formModel.fields.garbageSoring.split('|')" v-if="formModel.fields.garbageSoring.split('|').length-1!=index"  label="">
                    {{item}}
                </div>
                </Col>
                <Col span="18">
                <div style="font-size:0;margin-bottom:20px;"  v-for="(item,index) in formModel.fields.picture.split('|')" v-if="formModel.fields.garbageSoring.split('|').length-1!=index"  label="">
                    <img :src="'https://ljfl.hztlcgj.com/'+item" width="80px" height="80px">
                </div>
                </Col>
                <!-- </FormItem> -->
            </Row>
        </div>
        
      </Form>
    </Drawer>
    </div>
</template>

<script>
    import DzTable from "_c/tables/dz-table.vue";
    import {
        List,
        loadShopDetail,
        deleteShop,
        batchCommand
    } from "@/api/supervisorInspection/supervisorIn";
    export default {
        name: "supervisorIn",
        components: {
            DzTable
        },
        data() {
            return {
                commands: {
                    delete: { name: "delete", title: "删除" },
                    recover: { name: "recover", title: "恢复" },
                },
                imgurl:"https://ljfl.hztlcgj.com/",
                imglist:[],
                formModel: {
                    opened: false,
                    title: "创建时间",
                    mode: "create",
                    selection: [],
                    fields: {
                        addPeople:'',
                        addTime:'',
                        homeAddressUUID:'',
                        garbageSoring:'',
                        picture:'',
                        grade:'',
                        addresscode:'',
                    },
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
                            { title: "督导员", key: "addPeople"},
                            { title: "检查时间", key: "addTime"},
                            { title: "地址编码", key: "addresscode"},
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
                    return "详情";
                }
                if (this.formModel.mode === "edit") {
                    return "编辑记录";
                }
                return "";
            },
            selectedRows() {
                return this.formModel.selection;
            },
            selectedRowsId() {
                return this.formModel.selection.map(x => x.auditUuid);
            }
        },
        methods:{
            loadworktimeList() {
                List(this.stores.worktime.query).then(res => {
                    this.stores.worktime.data = res.data.data;
                    this.stores.worktime.query.totalCount = res.data.totalCount;
                    console.log(this.stores.worktime.data)
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
                this.doLoadWorktime(row.auditUuid);
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
    doLoadShopEditData(id) {
      //console.log(id);
    //   GetCarMonit(id).then(res => {
    //     //console.log(res.data.data);
    //     if (res.data.code === 200) {
    //       this.formModel.fields = res.data.data[0];
    //       this.formModel2.fields = res.data.data[0];
    //     }
    //   });
    },
         handleDetail(row) {
      this.formModel.opened = true;
      this.doLoadWorktime(row.auditUuid);
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
            doLoadWorktime(auditUuid) {
                loadShopDetail({ guid: auditUuid}).then(res => {
                    this.formModel.fields = res.data.data;
                    console.log(res.data.data)
                });
            },
            handleDelete(row) {
                this.doDelete(row.auditUuid);
            },
            doDelete(ids) {
                if (!ids) {
                    this.$Message.warning("请选择至少一条数据");
                    return;
                }
                deleteShop(ids).then(res => {
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
  .ljflfd{
   width: 50%;
  }
</style>

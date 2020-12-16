<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.consumable.query.totalCount"
        :pageSize="stores.consumable.query.pageSize"
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
                      v-model="stores.consumable.query.kw"
                      placeholder="输入申请人..."
                      @on-search="handleSearchConsumable()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.consumable.query.isDeleted"
                        @on-change="handleSearchConsumable"
                        placeholder="删除状态"
                        style="width:60px;"
                      >
                        <Option
                          v-for="item in stores.consumable.sources.isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.consumable.query.auditState"
                        @on-change="handleSearchConsumable"
                        placeholder="审核状态"
                        style="width:100px;"
                      >
                        <Option
                          v-for="item in auditStateList"
                          :value="item.auditState"
                          :key="item.auditState"
                        >{{item.auditStateName}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.consumable.query.materialType"
                        @on-change="handleSearchConsumable"
                        placeholder="材料类型"
                        style="width:100px;"
                      >
                        <Option
                          v-for="item in materialTypeList"
                          :value="item.materialType"
                          :key="item.materialType"
                        >{{item.materialTypeName}}</Option>
                      </Select>
                    </Input>
                  </FormItem>

                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.consumable.query.materialName"
                      placeholder="输入材料名称搜索..."
                      @on-search="handleSearchConsumable()"
                    ></Input>
                  </FormItem>
                  <FormItem>
                    <DatePicker
                      type="daterange"
                      v-model="stores.consumable.query.time"
                      format="yyyy/MM/dd"
                      placement="top"
                      placeholder="输入时间搜索..."
                      style="width: 200px"
                      :confirm="true"
                      :editable="false"
                      @on-ok="handleSearchConsumable()">
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
                  title="新增耗材申请"
                >新增耗材申请</Button>
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
          :data="stores.consumable.data"
          :columns="stores.consumable.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <template slot-scope="{row,index}" slot="materialType">
            <span>{{renderMaterialType(row.materialType)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>

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
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true" v-if="row.auditState==0||row.auditState==3">
              <Button v-can="'edit'" type="primary" size="small" shape="circle" icon="md-create" @click="handleEdit(row)"></Button>
            </Tooltip>
            <Tooltip placement="top" content="审核" :delay="1000" :transfer="true" v-if="row.auditState==1">
              <Button v-can="'audit'" type="success" size="small" shape="circle" icon="ios-hammer" @click="handleAudit(row)"></Button>
            </Tooltip>
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
              <Button v-can="'detail'" type="warning" size="small" shape="circle" icon="md-search" @click="handleDetail(row)"></Button>
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
      <Form :model="formModel.fields" ref="formConsumable" :rules="formModel.rules" label-position="left">
        <Row :gutter="16">
            <FormItem label="申请人" prop="realName">
              <Input v-model="formModel.fields.realName" :readonly="true"/>
            </FormItem>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="材料类型" prop="materialType">
              <Select v-model="formModel.fields.materialType">
                <Option
                  v-for="item in materialTypeList"
                  :value="item.materialType"
                  :key="item.materialType"
                >{{item.materialTypeName}}</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="材料名称" prop="materialName">
              <Input v-model="formModel.fields.materialName" placeholder="请输入材料类型"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="数量" prop="num">
              <Input v-model="formModel.fields.num" @keyup.native="formModel.fields.num=formModel.fields.num.replace(/[^\d]/g,'')" placeholder="请输入数量"  />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="型号规格" prop="specification">
              <Input v-model="formModel.fields.specification" placeholder="请输入型号规格"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
            <FormItem label="备注" >
              <Input v-model="formModel.fields.remark" placeholder="请输入备注"/>
            </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>

        <Button style="margin-left: 8px" icon="ios-share-alt" type="success" @click="handleSubmitToAuditConsumable">提交审核</Button>

        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
      title="申请审核"
      v-model="formModel1.opened"
      width="400"
      :mask-closable="false"
      :mask="false"
      :styles="styles"
    >
      <Form :model="formModel1.fields" ref="formConsumableAudit" label-position="left">
        <Row :gutter="16">
          <FormItem label="申请人" >
            <Input v-model="formModel1.fields.realName" :readonly="true"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="申请时间" >
            <Input v-model="formModel1.fields.applyTime" :readonly="true"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="材料类型" >
              <Select v-model="formModel1.fields.materialType" :disabled="true">
                <Option
                  v-for="item in materialTypeList"
                  :value="item.materialType"
                  :key="item.materialType"
                >{{item.materialTypeName}}</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="材料名称" >
              <Input v-model="formModel1.fields.materialName" :readonly="true"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="数量" >
              <Input v-model="formModel1.fields.num"  :readonly="true"/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="型号规格" >
              <Input v-model="formModel1.fields.specification" :readonly="true"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" >
            <Input v-model="formModel1.fields.remark" :readonly="true"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="审核意见" >
            <Input v-model="formModel1.fields.auditOpinion"  />
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="success" @click="handleSubmitPass">通 过</Button>
        <Button style="margin-left: 8px" icon="md-close-circle" type="primary" @click="handleSubmitNoPass">不通过</Button>
      </div>
    </Drawer>
    <Drawer
      title="详情"
      v-model="formModel2.opened"
      width="400"
      :mask-closable="false"
      :mask="false"
      :styles="styles"
    >
      <Form :model="formModel2.fields" ref="formConsumableDetail" label-position="left">
        <Row :gutter="16">
          <FormItem label="申请人" prop="realName">
            <Input v-model="formModel2.fields.realName" :readonly="true"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="申请时间" >
            <Input v-model="formModel2.fields.applyTime" :readonly="true"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="材料类型" prop="materialType">
              <Select v-model="formModel2.fields.materialType" :disabled="true">
                <Option
                  v-for="item in materialTypeList"
                  :value="item.materialType"
                  :key="item.materialType"
                >{{item.materialTypeName}}</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="材料名称" prop="materialName">
              <Input v-model="formModel2.fields.materialName" :readonly="true"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="数量" prop="num">
              <Input v-model="formModel2.fields.num"  :readonly="true"/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="型号规格" prop="specification">
              <Input v-model="formModel2.fields.specification" :readonly="true"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" >
            <Input v-model="formModel2.fields.remark" :readonly="true"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="审核意见" >
            <Input v-model="formModel2.fields.auditOpinion"  :readonly="true"/>
          </FormItem>
        </Row>
      </Form>
    </Drawer>
  </div>
</template>

<script>
    import DzTable from "_c/tables/dz-table.vue";
    import {
        getConsumableList,
        createConsumable,
        loadConsumable,
        editConsumable,
        deleteConsumable,
        batchCommand,
        createToAuditConsumable,
        editToAuditConsumable,
        auditPassConsumable,
        auditNoPassConsumable,
        loadConsumableDetail,
        loadConsumableAuditDetail
    } from "@/api/material/consumable";
    export default {
        name: "consumable",
        components: {
            DzTable
        },
        data() {
            return {
                materialTypeList:[
                    {materialType:-1,materialTypeName:'全部'},
                    {materialType:0,materialTypeName:'办公室用品'},
                    {materialType:1,materialTypeName:'电子设备'},
                ],
                auditStateList:[
                    {auditState:-1,auditStateName:'全部'},
                    {auditState:0,auditStateName:'保存中'},
                    {auditState:1,auditStateName:'待审核'},
                    {auditState:2,auditStateName:'审核通过'},
                    {auditState:3,auditStateName:'审核未通过'},
                ],
                commands: {
                    delete: { name: "delete", title: "删除" },
                    recover: { name: "recover", title: "恢复" },
                },
                formModel: {
                    opened: false,
                    title: "创建申请",
                    mode: "create",
                    selection: [],
                    fields: {
                        realName:'',
                        materialName: '',
                        materialType:0,
                        num:'',
                        specification:'',
                        remark:''
                    },
                    rules: {
                        materialName: [
                            { type: "string", required: true, message: "请输入材料名" }
                        ],
                        num: [
                            {  required: true, message: "请输入数量"}
                        ],
                        specification: [
                            { type: "string", required: true, message: "请输入规格型号" }
                        ],
                    }
                },
                formModel1: {
                    opened: false,
                    title: "申请审核",
                    selection: [],
                    fields: {
                        realName:'',
                        applyTime:'',
                        materialName: '',
                        materialType:0,
                        num:'',
                        specification:'',
                        remark:'',
                        auditOpinion:""
                    },
                },
                formModel2: {
                    opened: false,
                    title: "详情",
                    selection: [],
                    fields: {
                        realName:'',
                        applyTime:'',
                        materialName: '',
                        materialType:0,
                        num:'',
                        specification:'',
                        remark:'',
                        auditOpinion:""
                    },
                },
                stores: {
                    consumable: {
                        query: {
                            totalCount: 0,
                            pageSize: 20,
                            currentPage: 1,
                            kw: "",
                            materialType:"",
                            materialName:"",
                            auditState:"",
                            time:'',
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
                            { title: "申请人", key: "realName"},
                            { title: "申请时间",  width: 150,key: "applyTime"},
                            { title: "材料类型", key: "materialType",slot:"materialType"},
                            { title: "材料名称", key: "materialName"},
                            { title: "数量", key: "num"},
                            { title: "型号规格", key: "specification"},
                            { title: "备注", key: "remark"},
                            { title: "审核状态", key: "auditState",slot:"auditState"},
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
                    return "创建申请";
                }
                if (this.formModel.mode === "edit") {
                    return "编辑申请";
                }
                return "";
            },
            selectedRows() {
                return this.formModel.selection;
            },
            selectedRowsId() {
                return this.formModel.selection.map(x => x.consumableUuid);
            }
        },
        methods:{
            loadconsumableList() {
                getConsumableList(this.stores.consumable.query).then(res => {
                    this.stores.consumable.data = res.data.data;
                    this.stores.consumable.query.totalCount = res.data.totalCount;
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
                this.handleResetFormConsumable();
                this.doLoadConsumable(row.consumableUuid);
            },
            handleAudit(row) {
                this.formModel1.opened = true;
                this.$refs["formConsumableAudit"].resetFields();
                loadConsumableAuditDetail({ guid: row.consumableUuid}).then(res => {
                    this.formModel1.fields = res.data.data;
                });
            },
            handleDetail(row) {
                this.formModel2.opened = true;
                this.$refs["formConsumableDetail"].resetFields();
                loadConsumableDetail({ guid: row.consumableUuid}).then(res => {
                    this.formModel2.fields = res.data.data;
                });
            },
            handleSelect(selection, row) {},
            handleSelectionChange(selection) {
                this.formModel.selection = selection;
            },
            handleRefresh() {
                this.loadconsumableList();
            },
            handleShowCreateWindow() {
                this.handleSwitchFormModeToCreate();
                this.handleOpenFormWindow();
                this.handleResetFormConsumable();
                this.formModel.fields.realName = this.$store.state.user.userName;
            },
            handleSubmitConsumable() {
                let valid = this.validateConsumableForm();
                if (valid) {
                    if (this.formModel.mode === "create") {
                        this.doCreateConsumable();
                    }
                    if (this.formModel.mode === "edit") {
                        this.doEditConsumable();
                    }
                }
            },
            handleSubmitToAuditConsumable(){
                let valid = this.validateConsumableForm();
                if (valid) {
                    if (this.formModel.mode === "create") {
                        this.doCreateToAuditConsumable();
                    }
                    if (this.formModel.mode === "edit") {
                        this.doEditToAuditConsumable();
                    }
                }
            },
            handleResetFormConsumable() {
                this.$refs["formConsumable"].resetFields();
            },
            doCreateConsumable() {
                createConsumable(this.formModel.fields).then(res => {
                    if (res.data.code === 200) {
                        this.$Message.success(res.data.message);
                        this.handleCloseFormWindow();
                        this.loadconsumableList();
                    } else {
                        this.$Message.warning(res.data.message);
                    }
                });
            },
            handleSubmitPass(){
                auditPassConsumable(this.formModel1.fields).then(res => {
                    if (res.data.code === 200) {
                        this.$Message.success(res.data.message);
                        this.formModel1.opened=false;
                        this.loadconsumableList();
                    } else {
                        this.$Message.warning(res.data.message);
                    }
                });
            },
            handleSubmitNoPass(){
                auditNoPassConsumable(this.formModel1.fields).then(res => {
                    if (res.data.code === 200) {
                        this.$Message.success(res.data.message);
                        this.formModel1.opened=false;
                        this.loadconsumableList();
                    } else {
                        this.$Message.warning(res.data.message);
                    }
                });
            },
            doEditConsumable() {
                editConsumable(this.formModel.fields).then(res => {
                    if (res.data.code === 200) {
                        this.$Message.success(res.data.message);
                        this.handleCloseFormWindow();
                        this.loadconsumableList();
                    } else {
                        this.$Message.warning(res.data.message);
                    }
                });
            },
            doCreateToAuditConsumable() {
                createToAuditConsumable(this.formModel.fields).then(res => {
                    if (res.data.code === 200) {
                        this.$Message.success(res.data.message);
                        this.handleCloseFormWindow();
                        this.loadconsumableList();
                    } else {
                        this.$Message.warning(res.data.message);
                    }
                });
            },
            doEditToAuditConsumable() {
                editToAuditConsumable(this.formModel.fields).then(res => {
                    if (res.data.code === 200) {
                        this.$Message.success(res.data.message);
                        this.handleCloseFormWindow();
                        this.loadconsumableList();
                    } else {
                        this.$Message.warning(res.data.message);
                    }
                });
            },
            validateConsumableForm() {
                let _valid = false;
                this.$refs["formConsumable"].validate(valid => {
                    if (!valid) {
                        this.$Message.error("请完善表单信息");
                    } else {
                        _valid = true;
                    }
                });
                return _valid;
            },
            doLoadConsumable(consumableUuid) {
                loadConsumable({ guid: consumableUuid}).then(res => {
                    this.formModel.fields = res.data.data;
                });
            },
            handleDelete(row) {
                this.doDelete(row.consumableUuid);
            },
            doDelete(ids) {
                if (!ids) {
                    this.$Message.warning("请选择至少一条数据");
                    return;
                }
                deleteConsumable(ids).then(res => {
                    if (res.data.code === 200) {
                        this.$Message.success(res.data.message);
                        this.loadconsumableList();
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
                        this.loadconsumableList();
                        this.formModel.selection = [];
                    } else {
                        this.$Message.warning(res.data.message);
                    }
                    this.$Modal.remove();
                });
            },
            handleSearchConsumable() {
                this.loadconsumableList();
            },
            rowClsRender(row, index) {
                if (row.isDeleted) {
                    return "table-row-disabled";
                }
                return "";
            },
            handleSortChange(column) {
                this.stores.consumable.query.sort.direction = column.order;
                this.stores.consumable.query.sort.field = column.key;
                this.loadconsumableList();
            },
            handlePageChanged(page) {
                this.stores.consumable.query.currentPage = page;
                this.loadconsumableList();
            },
            handlePageSizeChanged(pageSize) {
                this.stores.consumable.query.pageSize = pageSize;
                this.loadconsumableList();
            },
            renderMaterialType(materialType){
                var materialTypeText = "未知";
                switch (materialType) {
                    case 0:
                        materialTypeText = "办公室用品";
                        break;
                    case 1:
                        materialTypeText = "电子设备";
                        break;
                }
                return materialTypeText;
            },
            renderAuditState(auditState){
                var auditText = "未知";
                switch (auditState) {
                    case 0:
                        auditText = "保存中";
                        break;
                    case 1:
                        auditText = "待审核";
                        break;
                    case 2:
                        auditText = "审核通过";
                        break;
                    case 3:
                        auditText = "审核未通过";
                        break;
                }
                return auditText;
            }
        },
        mounted() {
            this.loadconsumableList();
        }
    }
</script>

<style scoped>

</style>

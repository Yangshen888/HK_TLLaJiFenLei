<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.grabtype.query.totalCount"
        :pageSize="stores.grabtype.query.pageSize"
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
                      v-model="stores.grabtype.query.kw"
                      placeholder="输入名称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                  <FormItem>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.grabtype.query.kw1"
                      placeholder="输入类型名"
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
                  ></Button>-->
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                  <Button
                    v-can="'create'"
                    icon="md-create"
                    type="primary"
                    @click="handleShowCreateWindow"
                    title="添加分类"
                  >添加分类</Button>
                  <Button
                    v-can="'import'"
                    icon="ios-cloud-upload"
                    type="success"
                    @click="handleImportUserInfo"
                    title="导入垃圾分类信息"
                  >导入垃圾分类信息</Button>
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
          :data="stores.grabtype.data"
          :columns="stores.grabtype.columns"
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
            </Poptip>-->
            <Button
              v-can="'edit'"
              type="primary"
              size="small"
              shape="circle"
              icon="md-create"
              @click="handleShowEditWindow(row)"
            ></Button>
          </template>
        </Table>
      </dz-table>
    </Card>

    <Drawer
      title="垃圾分类信息导入"
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
          title="垃圾分类导入模板"
        >垃圾分类导入模板</Button>
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleimport"
          :disabled="importdisable"
        >导入</Button>
        <!-- <div>
           <Col class="demo-spin-col" span="8">
            <Spin fix>
                <Icon type="ios-loading" size=18 class="demo-spin-icon-load"></Icon>
                <div>Loading</div>
            </Spin>
        </Col>      
        </div>-->
        <Tabs value="name1">
          <TabPane label="成功" name="name1" v-html="successmsg"></TabPane>
          <TabPane label="重复" name="name2" v-html="repeatmsg"></TabPane>
          <TabPane label="失败" name="name3" v-html="defaultmsg"></TabPane>
        </Tabs>
      </div>
    </Drawer>
    <Drawer
      :title="formModel.title"
      v-model="formModel.opened"
      width="20%"
      :mask-closable="false"
      :mask="true"
    >
      <Form :model="formModel.fields" ref="form" :rules="formModel.rules" label-position="top">
        <Row :gutter="16">
          <FormItem label="垃圾名称" prop="name">
            <Input v-model="formModel.fields.name" placeholder="请输入垃圾名称" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="垃圾类型" prop="type">
            <Select v-model="formModel.fields.type">
              <Option value="可回收物">可回收物</Option>
              <Option value="有害垃圾">有害垃圾</Option>
              <Option value="易腐垃圾">易腐垃圾</Option>
              <Option value="其他垃圾">其他垃圾</Option>
            </Select>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          style="background-color:#3399ff;color:white"
          icon="md-checkmark"
          @click="handleSubmitConsumable()"
        >提 交</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">关 闭</Button>
      </div>
    </Drawer>
  </div>
</template>
<script>
import FileSaver from "file-saver";
import DzTable from "_c/tables/dz-table.vue";
import {
  shopInfoImport,
  getList, //加载
  create, //添加
  edit, //修改
  Detail, //详情
  deleteOne, //删除单个
  GetEditData, //获取要修改的数据
  batchCommand //多个操作
} from "@/api/base/GrabType";
import config from "@/config";
export default {
  name: "grabType",
  components: {
    DzTable
  },
  data() {
    return {
      formModel: {
        title: "",
        opened: false,
        mode: "create",
        selection: [],
        fields: {
          grabTypeUuid: "",
          name: "",
          type: ""
        },
        rules: {
          name: [{ type: "string", required: true, message: "请输入垃圾名称" }],
          type: [{ type: "string", required: true, message: "请选择垃圾类别" }]
        }
      },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        export: { name: "export", title: "导出" }
      },
            //导入
      url: config.baseUrl.dev,
      importdisable: false,
      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      formimport: {
        opened: false
      },

      stores: {
        grabtype: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            Kw: "",
            kw1: "",
            isDelete: 0,
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
            { type: "selection", width: 50, key: "grabTypeUUid" },
            { title: "垃圾名称", key: "grabName", sortable: true },
            { title: "垃圾类型", key: "type" },
            { title: "添加人", key: "addPeopel" },
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
      return this.formModel.selection.map(x => x.grabTypeUuid);
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
        this.url + "UploadFiles/ImportUserInfoModel/垃圾分类导入模板.xlsx";
    },
    //导入
    importfxx(e) {
      let inputDOM = this.$refs.inputer;
      ////console.log(inputDOM);
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
        await shopInfoImport(this.exceldata).then(res => {
          // //console.log(res.data);
          if (res.data.code == 200) {
            this.time = res.data.data.time;
            this.successmsg = res.data.data.successmsg;
            this.repeatmsg = res.data.data.repeatmsg;
            this.defaultmsg = res.data.data.defaultmsg;
            this.loadCarDispatchList();

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

    //获取信息数据
    loadCarDispatchList() {
      getList(this.stores.grabtype.query).then(res => {
        this.stores.grabtype.data = res.data.data;
        this.stores.grabtype.query.totalCount = res.data.totalCount;
      });
    },
    //翻页
    handlePageChanged(page) {
      this.stores.grabtype.query.currentPage = page;
      this.loadCarDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.grabtype.query.pageSize = pageSize;
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

      this.$refs["form"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadCarDispatch(volunteerId) {
      loadCarDriver({ guid: volunteerId }).then(res => {
        this.formModel.fields = res.data.data;
      });
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["form"].resetFields();
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
      this.doDelete(row.grabTypeUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteOne(ids).then(res => {
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
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleSortChange(column) {
      this.stores.user.query.sort.direction = column.order;
      this.stores.user.query.sort.field = column.key;
      this.loadCarDispatchList();
    },

    //添加按钮
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
    },

    //修改按钮
    handleShowEditWindow(row) {
      this.handleSwitchFormModeToEdit();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
      //读取数据
      GetEditData(row.grabTypeUuid).then(res => {
        if (res.data.code === 200) {
          this.formModel.fields = res.data.data[0];
        }
      });
    },
    //添加页面
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
      this.formModel.title = "垃圾分类添加";
    },
    //修改页面
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.formModel.title = "垃圾分类修改";
    },
    //保存按钮
    handleSubmitConsumable() {
      let valid = this.validateDispatchForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          create(this.formModel.fields).then(res => {
            if (res.data.code === 200) {
              this.handleCloseFormWindow();
              this.$Message.success(res.data.message);
              this.loadCarDispatchList();
            }
          });
        }
        if (this.formModel.mode === "edit") {
          //console.log(this.formModel.fields);
          edit(this.formModel.fields).then(res => {
            if (res.data.code === 200) {
              this.handleCloseFormWindow();
              this.$Message.success(res.data.message);
              this.loadCarDispatchList();
            }
          });
        }
      }
    }
  },
  mounted() {
    this.loadCarDispatchList();
  }
};
</script>


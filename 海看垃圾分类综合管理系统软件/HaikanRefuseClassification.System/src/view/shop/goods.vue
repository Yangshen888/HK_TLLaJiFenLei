<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.goods.query.totalCount"
        :pageSize="stores.goods.query.pageSize"
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
                      v-model="stores.goods.query.kw"
                      placeholder="输入商品名称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.goods.query.kw2"
                      placeholder="输入商店名称"
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
                  ></Button> -->
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加商品"
                >添加商品</Button>
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
          :data="stores.goods.data"
          :columns="stores.goods.columns"
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
            </Poptip> -->
            <Tooltip
              placement="top"
              content="编辑"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
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
          <FormItem label="商品名称" prop="gname">
            <Input v-model="formModel.fields.gname" placeholder="请输入商品名称"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
        <FormItem label="商店名称" prop="shopId">
          <Select v-model="formModel.fields.shopId"  >
            <Option v-for="item in formModel.ShopList" :value="item.shopUuid" >{{ item.shopName }}</Option>
          </Select>
        </FormItem>
      </Row>
        <Row :gutter="16">
          <FormItem label="价格" prop="price">
            <Input v-model="formModel.fields.price" placeholder="请输入价格"/>
          </FormItem>
        </Row>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
      </Form>
    </Drawer>
  </div>
</template>
<script>
  import DzTable from "_c/tables/dz-table.vue";
  import {
    List,
    loadShopDetail, //详情
    create, //添加（保存）
    editSave, //编辑（保存）
    loadShop, //加载
    LoadGoods,
    ShopList,//商店下拉框
    loadGoodsEditData,//编辑数据
    globalvalidatePhone
  } from "@/api/shop/goods";

  export default {
    name: "goods",
    components: {
      DzTable
    },
    data() {
      let validatePrice = (rule, value, callback) => {
        if (value !== '' && value !== null) {
          let reg = /^\d+(\.\d+)?$/;
          if (!reg.test(value)) {
            callback(new Error('请输入有效的价格'));
          }
          callback();
        } else {

          callback(new Error('价格不能为空'));
        }
        callback();
      };
      return {
        formModel: {
          opened: false,
          title: "添加商品",
          mode: "create",
          selection: [],
          ShopList:[],
          fields: {
            gname:"",
            price:0,
            addTime:"",
            addPeople:"",
            shopId:"",
          },
          rules: {
            gname: [
              { type: "string", required: true, message: "请输入商品名称" }
            ],
            shopId: [
              { type: "string", required: true, message: "请选择商店" }
            ],
            price: [
              { type: "string", required: true, message: "请输入价格",validator:validatePrice }
            ]
          }
        },
        commands: {
          delete: { name: "delete", title: "删除" },
          recover: { name: "recover", title: "恢复" }
        },
        //查询搜索
        stores: {
          goods: {
            query: {
              totalCount: 0,
              pageSize: 20,
              currentPage: 1,
              Kw: "",
              Kw2: "",
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
            },
            //列表显示
            columns: [
              { type: "selection", width: 50, key: "goodsId" },
              { title: "商品名称", key: "gname", sortable: true },
              { title: "积分", key: "price"},
              // { title: "添加时间", key: "addTime"},
              { title: "商店名称", key: "shopName" },
              // { title: "状态", key: "state" },
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

      };
    },
    computed: {
      formTitle() {
        if (this.formModel.mode === "create") {
          return "添加商店信息";
        }
        if (this.formModel.mode === "edit") {
          return "编辑商店信息";
        }
        return "";
      },
      selectedRows() {
        return this.formModel.selection;
      },
      selectedRowsId() {
        return this.formModel.selection.map(x => x.goodsId);
      } //删除
    },
    methods: {
      //获取表格数据
      loadShopDispatchList() {
        List(this.stores.goods.query).then(res => {
          ////console.log(res.data.data)
          this.stores.goods.data = res.data.data;
          this.stores.goods.query.totalCount = res.data.totalCount;
        });
      },
      //翻页
      handlePageChanged(page) {
        this.stores.goods.query.currentPage = page;
        this.loadShopDispatchList();
      },
      //显示条数改变
      handlePageSizeChanged(pageSize) {
        this.stores.goods.query.pageSize = pageSize;
        this.loadShopDispatchList();
      },
      //打开窗口
      handleOpenFormWindow() {
        this.formModel.opened = true;
        this.doLoadShopList();
      },
      //自动关闭窗口
      handleCloseFormWindow() {
        this.formModel.opened = false;
      },
      ShowGoods(row){
        this.stores2.open=true;
        this.doLoadGoods(row.goodsId);
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

      //清空
      handleResetFormDispatch() {
        this.$refs["formdispatch"].resetFields();
      },
      //搜索
      handleSearchDispatch() {
        this.loadShopDispatchList();
      },
      //刷新
      handleRefresh() {
        this.loadShopDispatchList();
      },
      //右边删除（单个删除）
      handleDelete(row) {
        this.doDelete(row.shopUuid);
      },
      doDelete(ids) {
        if (!ids) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        deleteShop(ids).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.loadShopDispatchList();
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
            this.loadShopDispatchList();
            this.formModel.selection = [];
          } else {
            this.$Message.warning(res.data.message);
          }
          this.$Modal.remove();
        });
      },
      //加载商品列表
      doLoadShopList(){
        ShopList().then(res => {
          ////console.log(res.data.data)
          this.formModel.ShopList = res.data.data;
        });
      },
      //右边编辑
      handleEdit(row) {
        this.handleOpenFormWindow();
        this.handleSwitchFormModeToEdit();
        this.handleResetFormDispatch();
        //console.log(row);
        this.doLoadGoodsEditData(row.goodsId);
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
        this.loadShopDispatchList();
      },
      //右边详情
      handleDetail(row) {
        ////console.log(row);
        this.formModel2.opened = true;
        loadShopDetail({ guid: row.goodsUuid }).then(res => {

          this.formModel2.fields = res.data.data[0];
        });
      },
      doLoadGoodsEditData(id){
        loadGoodsEditData(id).then(res=>{
          if (res.data.code === 200) {

            this.formModel.fields = res.data.data;
          }
        })
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
      //添加商店（保存）
      docreateShopDispatch() {
        ////console.log(this.formModel.fields);
        create(this.formModel.fields).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.handleCloseFormWindow(); //关闭表单
            this.loadShopDispatchList();
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
      //编辑商品（保存）
      doEditShopDispatch() {
        editSave(this.formModel.fields).then(res => {
          //console.log(this.formModel.fields)
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.handleCloseFormWindow();
            this.loadShopDispatchList();
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
            this.docreateShopDispatch();
          }
          if (this.formModel.mode === "edit") {
            this.doEditShopDispatch();
          }
        }
      },
    },
    mounted() {

      this.loadShopDispatchList();
    }
  };
</script>
<style>
</style>

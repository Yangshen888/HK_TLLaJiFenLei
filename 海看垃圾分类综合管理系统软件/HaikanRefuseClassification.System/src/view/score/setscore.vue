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
                      placeholder="输入评价名称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>


              <Col span="8" class="dnc-toolbar-btns">
              <Button
                  v-can="'model'"
                  icon="ios-cloud-download"
                  type="warning"
                  @click="handledaynum"
                  title="每天赋分次数"
                >每天赋分次数</Button>
              <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="updatePWdModel"
                  title="志愿者积分规则设置"
                >志愿者积分规则设置</Button>

                <Button
                  v-can="'import'"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleShowCreateWindow"
                  title="赋分设置"
                >赋分设置</Button>

                <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                <ButtonGroup class="mr3">
                  <!-- <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button> -->
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
          <template slot-scope="{ row, index }" slot="action">
<!--           <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">-->
<!--             <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">-->
<!--               <Button type="error" size="small" shape="circle" icon="md-trash"></Button>-->
<!--             </Tooltip>-->
<!--           </Poptip>-->
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
            <FormItem label="赋分评价" prop="name">
              <Input v-model="formModel.fields.scoreName" placeholder="请输入评价名称"/>
            </FormItem>
        </Row>
        <Row :gutter="16">
            <FormItem label="对应积分" prop="score">
              <Input v-model="formModel.fields.integral" placeholder="请输评价对应的积分"/>
            </FormItem>
        </Row>


      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>

    <!--  设置每天赋分次数-->
    <Modal
      title="志愿者积分规则设置"
      v-model="updatePwd"
      class-name="vertical-center-modal"
      footer-hide>
      <Form ref="formInline" :model="formInline" :rules="ruleInline">
        <Row>
          <Col span="24">
            <FormItem label="每次对应积分" prop="scoreinfo">
              <Input type="number" v-model="formInline.score" placeholder="请输积分" />
            </FormItem>
          </Col>
        </Row>
        <FormItem>
          <Button type="primary" @click="handleSubmit()">提交</Button>
        </FormItem>
      </Form>
    </Modal>

    <!--  设置每天赋分次数-->
    <Modal
      title="每天赋分次数设置"
      v-model="updatePwd2"
      class-name="vertical-center-modal"
      footer-hide>
      <Form ref="formInline2" :model="formInline2" :rules="ruleInline2">
        <Row>
          <Col span="24">
            <FormItem label="每天可赋分次数" prop="scoreinfo">
              <Input type="number" v-model="formInline2.score"   placeholder="请输次数" />
            </FormItem>
          </Col>
        </Row>
        <FormItem>
          <Button type="primary" @click="handleSubmit2()">提交</Button>
        </FormItem>
      </Form>
    </Modal>

  </div>
</template>
<script>
  import DzTable from "_c/tables/dz-table.vue";
  import {
    getScoreList,
    deletescore, //右边删除
    batchCommand, //右边上方删除
    loadCarDriver1, //加载
    getScoreData,//加载分数
    GetRecord,//详情
    editCarDriver1, //编辑（保存）
    globalvalidatePhone,
    addscore,
    doHourintegral,
    doDivisionTimes,
    getSetNumber
  } from "@/api/score/own";

  export default {
    name: "house",
    components: {
      DzTable
    },
    data() {
      let globalvalidatescore = (rule, value, callback) => {
        var val=this.formModel.fields.integral;
        if (val !== '' && val !== null) {
          var reg=new RegExp('^[0-9]*$');

          if(reg.test(val))
          {
            callback();
          }else
          callback(new Error('必须为正整数'));
        } else {
          callback(new Error('积分不能为空'));
        }
        callback();
      };
      let globalvalidatename = (rule, value, callback) => {
        var val=this.formModel.fields.scoreName;
        if (val !== '' && val !== null) {
          callback();
        } else {
          callback(new Error('评价名称不能为空'));
        }
        callback();
      };
      let volunteerscore = (rule, value, callback) => {
        var val=this.formInline.score;
        if (val !== '' && val !== null) {
          var reg=new RegExp('^[0-9]*$');

          if(reg.test(val))
          {
            //console.log(val);
            callback();
          }else
          callback(new Error('必须为正整数'));
        } else {
          callback(new Error('积分不能为空'));
        }
        callback();
      };
      let volunteerscore2 = (rule, value, callback) => {
        var val=this.formInline2.score;
        if (val !== '' && val !== null) {
          var reg=new RegExp('^[0-9]*$');

          if(reg.test(val))
          {
            callback();
          }else
          callback(new Error('必须为正整数'));

        } else {
          callback(new Error('次数不能为空'));
        }
        callback();
      };
      return {
        rowid:"",
        updatePwd:false,
        updatePwd2:false,
        formInline:{
            score:'0',
        },
        ruleInline: {
                scoreinfo: [{required: true,validator:volunteerscore }],
            },
            formInline2:{
            score:'0',
        },
        ruleInline2: {
                scoreinfo: [{required: true,validator:volunteerscore2 }],
            },
        formModel: {
          opened: false,
          mode: "create",
          selection: [],
          fields: {
            scoreName:"",
            integral:0,
          },
          rules: {
            name: [
              { type: "string", required: true, validator:globalvalidatename }
            ],
            score: [
              { type: "string", required: true, validator:globalvalidatescore}
            ]
          }
        },
        commands: {
          delete: { name: "delete", title: "删除" },
          recover: { name: "recover", title: "恢复" }
        },
        stores: {
          car: {
            query: {
              totalCount: 0,
              pageSize: 20,
              currentPage: 1,
              kw: "",
              kw1:"",
              kw2:"",
              isDelete: 0,
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
            columns: [
              { type: "selection", width: 50, key: "scoreUuid" },
              { title: "评价名称", key: "scoreName", sortable: true },
              { title: "对应积分", key: "integral" },
              { title: "添加人", key: "addpeople" },
              { title: "添加时间", key: "addTime" },
              {
                title: "操作",
                align: "center",
                width: 150,
                className: "table-command-column",
                slot: "action"
              }
            ],
            data: [],
            setnumbers:[]
          }
        },
      };
    },
    computed: {
      formTitle() {
      if (this.formModel.mode === "create") {
        return "创建积分评价";
      }
      if (this.formModel.mode === "edit") {
        return "编辑积分评价";
      }
      return "";
    },
      selectedRows() {
        return this.formModel.selection;
      },
      selectedRowsId() {
        return this.formModel.selection.map(x => x.scoreUuid);
      } //删除
    },
    methods: {

      //获取信息数据
      loadScoreList() {
        getScoreList(this.stores.car.query).then(res => {
          this.stores.car.data = res.data.data;
          this.stores.car.query.totalCount = res.data.totalCount;
        //   //console.log(this.stores.car.data);
        });
      },
      //翻页
      handlePageChanged(page) {
        this.stores.car.query.currentPage = page;
        this.loadScoreList();
      },
      //显示条数改变
      handlePageSizeChanged(pageSize) {
        this.stores.car.query.pageSize = pageSize;
        this.loadScoreList();
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
      //编辑
      doloadScore(scoreUuid) {
      //console.log(scoreUuid)
      loadCarDriver1({ guid: scoreUuid }).then(res => {
        this.formModel.fields = res.data.data;
      });
    },
      //清空
      handleResetFormDispatch() {
        this.$refs["formdispatch"].resetFields();
      },
      //搜索
      handleSearchDispatch() {
        this.loadScoreList();
      },
      //刷新
      handleRefresh() {
        this.loadScoreList();
      },
      //右边删除（单个删除）
      handleDelete(row) {
        this.doDelete(row.scoreUuid);
      },

      doDelete(ids) {
        if (!ids) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        deletescore(ids).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.loadScoreList();
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
            this.loadScoreList();
            this.formModel.selection = [];
          } else {
            this.$Message.warning(res.data.message);
          }
          this.$Modal.remove();
        });
      },
   //右边编辑
    handleEdit(row) {
      this.handleOpenFormWindow();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormDispatch();
      this.doloadScore(row.scoreUuid);
    },
      handleSwitchFormModeToEdit() {
        this.formModel.mode = "edit";
      },
      handleSelect(selection, row) {
      },
      handleSelectionChange(selection) {
        this.formModel.selection = selection;
      },
      handleSortChange(column) {
        this.stores.user.query.sort.direction = column.order;
        this.stores.user.query.sort.field = column.key;
        this.loadScoreList();
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
      //保存按钮
    // handleSubmitConsumable() {
    //   let valid = this.validateDispatchForm();
    //   if (valid) {
    //     this.docreatescore();
    //   }
    // },
      //保存按钮
    handleSubmitConsumable() {
      let valid = this.validateDispatchForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.docreatescore();
        }
        if (this.formModel.mode === "edit") {
          this.doeditscore();
        }
      }
    },
    //添加公车调度（保存）
    docreatescore() {
      //console.log(this.formModel.fields);
      addscore(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow(); //关闭表单
          this.loadScoreList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑（保存）
    doeditscore() {
      editCarDriver1(this.formModel.fields).then(res => {
        //console.log(this.formModel.fields)
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadScoreList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    updatePWdModel(){
          this.updatePwd=true;
      },
      handledaynum(){
          this.updatePwd2=true;
      },
      validateConsumableForm() {
          let _valid = false;
          this.$refs["formInline"].validate(valid => {
              if (!valid) {
                  this.$Message.error("请完善表单信息");
              } else {
                  _valid = true;
              }
          });
          return _valid;
      },
      validateConsumableForm2() {
          let _valid = false;
          this.$refs["formInline2"].validate(valid => {
              if (!valid) {
                  this.$Message.error("请完善表单信息");
              } else {
                  _valid = true;
              }
          });
          return _valid;
      },
      handleSubmit(){
          let valid = this.validateConsumableForm();
          if(valid){

              doHourintegral(this.formInline.score).then(res=>{
                  if(res.data.code==200)
                  {
                      this.$Message.success("设置成功");
                      this.updatePwd=false;
                  }
                  else{
                      this.$Message.error(res.data.message);
                  }
              })
          }
      },
      handleSubmit2(){
          let valid = this.validateConsumableForm2();
          if(valid){

              doDivisionTimes(this.formInline2.score).then(res=>{
                  if(res.data.code==200)
                  {
                      this.$Message.success("设置成功");
                      this.updatePwd2=false;
                  }
                  else{
                      this.$Message.error(res.data.message);
                  }
              })
          }
      },
      dogetSetNumber(){
        getSetNumber().then(res=>{
          if(res.data.code==200)
          {
            this.setnumbers=res.data.data;
            this.formInline.score=this.setnumbers[0].hourScore;
            this.formInline2.score=this.setnumbers[0].setNumber;
          }
        })
      }

    },
    mounted() {
      this.loadScoreList();
      this.dogetSetNumber();
    }
  };
</script>


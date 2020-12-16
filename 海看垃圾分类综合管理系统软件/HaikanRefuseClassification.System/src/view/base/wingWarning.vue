<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.xxx1.query.totalCount"
        :pageSize="stores.xxx1.query.pageSize"
        @on-page-change="handlePageChanged_xxx"
        @on-page-size-change="handlePageSizeChanged_xxx"
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
                      v-model="stores.xxx1.query.kw"
                      placeholder="输入箱房名称搜索..."
                      @on-search="handleSearchDispatch_xxx()"
                    ></Input>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.xxx1.query.kw1"
                      placeholder="输入社区名称搜索..."
                      @on-search="handleSearchDispatch_xxx()"
                    ></Input>
                    <DatePicker
                      type="daterange"
                      v-model="stores.xxx1.query.kw2"
                      format="yyyy/MM/dd"
                      placement="top"
                      placeholder="输入时间搜索..."
                      style="width: 200px"
                      :confirm="true"
                      :editable="false"
                      @on-ok="handleSearchDispatch_xxx()"
                    ></DatePicker>
                  </FormItem>
                  <span style='color:#ff9900;'>提示：颜色标注为当天未打卡的记录</span>
                </Form>
              </Col>
              
              <!--<Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand_xxx('delete')"
                  ></Button>
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh_xxx"></Button>
                </ButtonGroup>
                <Button
                  type="success"
                  icon="md-cloud-upload"
                  @click="importVoteinitiateQuestions"
                >导入活动及问题</Button> 
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加投票活动"
                >添加投票活动</Button>
              </Col>-->
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.xxx1.data"
          :columns="stores.xxx1.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh_xxx"
          :row-class-name="rowClsRender_xxx"
          @on-page-change="handlePageChanged_xxx"
          @on-page-size-change="handlePageSizeChanged_xxx"
          @on-sort-change="handleSortChange"
        >
          <template slot-scope="{row,index}" slot="state">
            <span>{{CheckState(row.state)}}</span>
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
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true" v-if="row.state==0" >
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit_xxx(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="success"
                size="small"
                shape="circle"
                icon="md-search"
                @click="showInfo_xxx(row)"
              ></Button>
            </Tooltip>
            <!-- <Tooltip
              placement="top"
              content="发布"
              :delay="1000"
              :transfer="true"
              v-if="row.state==0"
            >
              <Button
                v-can="'edit'"
                type="error"
                size="small"
                shape="circle"
                icon="md-share-alt"
                @click="publish(row)"
              ></Button>
            </Tooltip> -->
            <!--            <Tooltip placement="top" content="分配角色" :delay="1000" :transfer="true">-->
            <!--              <Button type="success" size="small" shape="circle" icon="md-contacts" @click="handleAssignRole(row)"></Button>-->
            <!--            </Tooltip>-->
          </template>
        </Table>
      </dz-table>
    </Card>
    
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getList
} from "@/api/base/wingWarn";
export default {
  name: "wingWarning",
  components: {
    DzTable
  },
  data() {
    return {
      //form保存参数
      formModel: {
        selection: [],
        opened: false,
        mode: "",
        fields: {
          // 具体实例保存参数
        }
      },
      formModel2: {},
      formModel3: {},
      //...
      //用于提交及一些保持性数据
      stores: {
        //该实例对象相关数据
        xxx1: {
          //提交请求参数
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isDeleted: 0,
            status: -1,
            //自定义参数
            kw: "",
            kw1: "",
            kw2: "",
            //查询排序方式
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          //请求获得的数据集合,用于填入表格
          data: [],
          //一些自定义附件处理数据,用于下拉列表之类
          sources: {},
          //table的列项名称
          columns: [
            //选择框
            { type: "selection", width: 50, key: "handle" },
            { title: "时间", key: "times", sortable: true },
            { title: "箱房名称", key: "ljname" },
            { title: "社区名称", key: "vname" },

            //可进行逻辑处理的条件规则显示项,以slot绑定
            // { title: "箱房名称", key: "xxx", slot: "state" },
            //操作按钮显示样式
            // {
            //   title: "操作",
            //   align: "center",
            //   width: 150,
            //   className: "table-command-column",
            //   slot: "action"
            // }
          ]
        },
        xxx2: {}
        //...
      },
      //自定义样式
      styles: {}
    };
  },
  //计算属性,以名称命名的方法体
  computed: {
    formTitle() {
      //条件
      return "";
    },
    //...
    //获取表格选择的行id
    selectRowsId() {
      //返回具体选择项的uuid
      return this.formModel.selection.map(x => x.xxxUuid);
    }
    //...
  },
  //方法集合
  methods: {
    //表格行样式
    rowClsRender_xxx(row, index) {
      let date=new Date();
      let m="";
      let d="";
      if(date.getMonth()<9){
        m="0"+(date.getMonth()+1);
      }else{
        m=""+(date.getMonth()+1);
      }
      if(date.getDate()<10){
        d="0"+date.getDate();
      }else{
        d=""+date.getDate();
      }
      let s=date.getFullYear()+"-"+m+"-"+d;
      ////console.log(s);
      //console.log(row.times);
      //console.log(s);
      //console.log(row.times.toString()==s);
       if (row.times==s) {
         
         return "demo-table-info-row";
       }
       return "";
    },
    //加载表格数据
    loadList_xxx() {
      //console.log(this.stores.xxx1.query);
      getList(this.stores.xxx1.query).then(res=>{
        //console.log(res.data.data);
        this.stores.xxx1.data=res.data.data;
        this.stores.xxx1.query.totalCount=res.data.totalCount;
      })
    },
    //刷新
    handleRefresh_xxx() {
      this.loadList_xxx();
    },
    //行选框的改变
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //排序改变(忽略?)
    handleSortChange(column) {
      this.stores.user.query.sort.direction = column.order;
      this.stores.user.query.sort.field = column.key;
      this.loadVoteinitiateList();
    },
    //搜索
    handleSearchDispatch_xxx() {
      this.loadList_xxx();
    },
    //表格翻页
    handlePageChanged_xxx(page) {
      this.stores.xxx1.query.currentPage = page;
      this.loadList_xxx();
    },
    //表格页大小改变
    handlePageSizeChanged_xxx(pageSize) {
      this.stores.xxx1.query.pageSize = pageSize;
      this.loadList_xxx();
    },
    //导入
    handleImport_xxx() {},
    //提示批量(操作)删除,command为提示操作类型
    handleBatchCommand_xxx(command) {
      if (!this.selectRowsId || this.selectRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content: "<p>确定要执行当前 [删除] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand_xxx(command);
        }
      });
    },
    //批量(操作)删除,command为提示操作类型
    doBatchCommand_xxx(command) {
      batchCommand_xxx({
        command: command,
        ids: this.selectRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadList_xxx();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    //详情
    showInfo_xxx(row){

    },
    //编辑时打开窗口
    handleOpenFormWindow_xxx() {
      this.formModel.opened = true;
    },
    //启用编辑的状态
    handleSwitchFormModeToEdit_xxx() {
      this.formModel.mode = "edit";
    },
    //启用添加的状态
    handleSwitchFormModeToCreate_xxx(){
      this.formModel.mode = "create";
    },
    //编辑按钮
    handleEdit_xxx(row) {
      this.handleSwitchFormModeToEdit_xxx();
      this.handleOpenFormWindow_xxx();
      //查询该条数据内容
    },
    //添加按钮
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate_xxx();
      this.handleOpenFormWindow_xxx();
    },
    //表单验证
    validateForm_xxx() {
      let _valid= false;
      this.$refs["xxxxxx"].validate(valid=>{
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    //保存按钮
    handleSubmit_xxx(){
      let valid=this.validateForm_xxx();
      if(valid){
        
        if (this.formModel.mode === "create") {
          this.doCreate_xxx();
        }
        if (this.formModel.mode === "edit") {
          this.doEdit_xxx();
        }
      }
    },
    //保存添加数据
    doCreate_xxx() {
      save_xxx(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false;
          this.loadList_xxx();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //保存编辑数据
    doEdit_xxx() {
      save_xxx(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false;
          this.loadList_xxx();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //取消保存
    doCancel_xxx() {
      if(this.formModel.mode=="create"){
        // cancelVoteinitiate({ guid: this.formModel.fields.voteinitiateUuid });
        //取消的操作
      }
      this.formModel.opened = false;
    },
    //---------------第二个模型-------------------

    //----------------slot-----------------
    Is_xxx(type){
      let rType="未知";
      // switch(type){
      //   case 0:
      //     rType = "否";
      //     break;
      //   case 1:
      //     rType = "是";
      //     break;
      // }
      return rType;
    },
    CheckState(state) {
      if (state == 0) {
        return "否";
      }
      if (state == 1) {
        return "是";
      }
    },
    //------------------杂项---------------------
    //将日期转为YY-MM-DD hh:mm:ss字符串格式
    dateToString(date) {
      let year = date.getFullYear();
      let month = date.getMonth() + 1;
      let day = date.getDate();
      let hour=date.getHours();
      let minute=date.getMinutes();
      let second=date.getSeconds();
      return year + "-" + month + "-" + day+" "+hour+":"+minute+":"+second;
    }
  },
  //页面加载时执行
  mounted(){
    this.loadList_xxx();
  }
};
</script>
<style>
.ivu-table .demo-table-info-row td{
        background-color: #ff9900;
        color: #fff;
    }
</style>

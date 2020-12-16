<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores1.grab.query.totalCount"
        :pageSize="stores1.grab.query.pageSize"
        @on-page-change="handlePageChanged1"
        @on-page-size-change="handlePageSizeChanged1"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="24" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh1"></Button>
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
            :data="stores1.grab.data"
            :columns="stores1.grab.columns"
            @on-select="handleSelect"
            @on-refresh="handleRefresh1"
            :row-class-name="rowClsRender"
            @on-page-change="handlePageChanged1"
            @on-page-size-change="handlePageSizeChanged1"
            @on-sort-change="handleSortChange1"
          >
          <template slot-scope="{ row, index }" slot="action">
            <Tooltip
              placement="top"
              content="详情"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'show1'"
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
      title="赋分记录"
      v-model="formModelopened"
      width="90%"
      :mask-closable="false"
      :mask="true"
    >
    <Card>
      <dz-table
        :totalCount="stores.grab.query.totalCount"
        :pageSize="stores.grab.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>    
                    <Select
                      v-model="stores.grab.query.street"
                      style="width:150px"
                      @on-change="xz"
                      clearable
                      placeholder="请选择街道"
                    >
                      <Option
                        v-for="item in this.stores.grab.sources.town"
                        :value="item"
                        :key="item"
                      >{{ item }}</Option>
                    </Select>
                    <Select
                      v-model="stores.grab.query.ccmmunity"
                      style="width:150px"
                      @on-change="sq"
                      clearable
                      placeholder="请选择社区"
                    >
                      <Option
                        v-for="item in this.stores.grab.sources.college"
                        :value="item"
                        :key="item"
                      >{{ item }}</Option>
                    </Select>
                  </FormItem>
                  <FormItem>
                       <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.grab.query.kw1"
                      placeholder="请输入地址"
                      @on-search="handleSearchData()"
                    >
                    </Input>
                  </FormItem>
                  <FormItem>
                      <Select
                        v-model="stores.grab.query.isScore"
                        @on-change="handleSearchData()"
                        placeholder="赋分状态"
                        style="width:100px;"
                      >
                        <Option
                          v-for="item in stores.grab.sources.isSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                  </FormItem>
                  <FormItem>
                    <Select
                      v-model="stores.grab.query.markType"
                      @on-change="handleSearchData()"
                      placeholder="赋分渠道"
                      style="width:100px;"
                    >
                      <Option
                        v-for="item in stores.grab.mark"
                        :value="item.value"
                        :key="item.value"
                      >{{item.value}}</Option>
                    </Select>
                  </FormItem>
                  <FormItem>
                    <DatePicker
                      type="daterange"
                      v-model="stores.grab.query.time"
                      format="yyyy/MM/dd"
                      placement="top"
                      placeholder="输入时间搜索..."
                      style="width: 200px"
                      :confirm="true"
                      :editable="false"
                      @on-ok="handleSearchData()"
                    ></DatePicker>
                  </FormItem>
                </Form>
              </Col>

              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
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
            :data="stores.grab.data"
            :columns="stores.grab.columns"
            @on-select="handleSelect"
            @on-selection-change="handleSelectionChange"
            @on-refresh="handleRefresh"
            :row-class-name="rowClsRender"
            @on-page-change="handlePageChanged"
            @on-page-size-change="handlePageSizeChanged"
            @on-sort-change="handleSortChange"
          >
          <template slot-scope="{row,index}" slot="Score">
            <Tag :color="renderScore(row.isScore).color">{{renderScore(row.isScore).text}}</Tag>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <Poptip
              confirm
              :transfer="true"
              title="确定要为其赋分吗?"
              @on-ok="handleEdit(row)"
              >
                <Tooltip
                  placement="top"
                  content="赋分"
                  :delay="1000"
                  :transfer="true"
                >
                  <Button
                    v-can="'edit'"
                    type="primary"
                    size="small"
                    shape="circle"
                    v-show="row.isScore == '已投放'"
                    icon="md-create"
                  ></Button>
                </Tooltip>
              </Poptip>
              <Poptip
                confirm
                :transfer="true"
                title="确定要取消其赋分吗?"
                @on-ok="handleEditNo(row)"
              >
                <Tooltip
                  placement="top"
                  content="投放"
                  :delay="1000"
                  :transfer="true"
                >
                  <Button
                    v-can="'edit'"
                    type="primary"
                    size="small"
                    shape="circle"
                    v-show="row.isScore == '已赋分'"
                    icon="md-create"
                  ></Button>
                </Tooltip>
              </Poptip>
            </template>
          </Table>
        </dz-table>
      </Card>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getGrabageDisposal,
  DelGrabageDisposal,
  BatchGrabageDisposal,
  disUpdate,
  disUpdateNo,
  getVillageList,
} from "@/api/grab/grabagedisposal";
import { loadRoleListByUserGuid,loadSimpleList } from "@/api/rbac/role";
import { loaddepartmentListDetail } from "@/api/rbac/department";
import { getshequ } from "@/api/base/house";

export default {
  name: "rbac_user_page",
  components: {
    DzTable
  },
  data() {
    return {
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        // forbidden: { name: "forbidden", title: "禁用" },
        // normal: { name: "normal", title: "启用" }
      },
      formModelopened:false,
      formModel: {
        opened: false,
        title: "创建用户",
        mode: "create",
        selection: [],
        fields: {
          loginName: "",
          realName: "",
          passWord: "",
          //avatar: "",
          userType: 0,
          //isLocked: 0,
          //status: 1,
          isDeleted: 0,
            systemRoleUuid:'',
            userIdCard:'',
            departmentUuid:''
        },
        rules: {
          loginName: [
            { type: "string", required: true, message: "请输入登录名", min: 3 }
          ],
          realName: [],
          password: []
        }
      },
      formAssignRole: {
        userGuid: "",
        opened: false,
        ownedRoles: [],
        inited: false,
        roles: []
      },
      list33: [],
      stores1: {
        grab: {
          query: {
            totalCount: 0,
            pageSize: 30,
            currentPage: 1,
            kw1: "",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          columns: [
            { title: "社区", key: "ccmmunity"},
            { title: "赋分总数",  key: "score"},
            { title: "操作", align: "center", width: 150, className: "table-command-column",slot:"action" }
          ],
          data: []
        }
      },
      stores: {
        grab: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            time: "",
            isScore: "已赋分",
            vuuid: this.$store.state.user.villageGuid,
            street:"",
            ccmmunity:"",
            markType: "全部",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          mark:[
            {value:'全部'},
            {value:'家庭码'},
            {value:'百姓码'},
            {value:'市民卡'},
          ],
          sources: {
            //社区集合
            college: ["全部" ],     
             //街道集合
            town: ["全部" ],  
            isSources:[
              { value: "全部", text: "全部" },
              { value: "已投放", text: "已投放" },
              { value: "已赋分", text: "已赋分" }
            ],
            userTypes: [
              { value: 0, text: "超级管理员" },
              { value: 1, text: "管理员" },
              { value: 2, text: "普通用户" }
            ],
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
            { title: "住户", key: "address",ellipsis: true,tooltip: true,sortable: true },
            { title: "箱房名", key: "roomID",ellipsis: true,tooltip: true },
            { title: "赋分渠道",  key: "markType", width: 100 },
            { title: "赋分状态",  key: "isScore", width: 100,slot:'Score' },
            { title: "投放时间", key: "addTime" },
            { title: "赋分时间", key: "scoreAddtime" },
            {
              title: "本次积分",
              width: 80,
              ellipsis: true,
              tooltip: true,
              key: "score"
            },
            { title: "操作", align: "center", width: 80, className: "table-command-column",slot:"action" }
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
        rolelist:[],
        departmentlist:[]
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
      return this.formModel.selection.map(x => x.garbageDisposalUuid);
    }
  },
  methods: {
    loadGrabDisList1() {
      getVillageList(this.stores1.grab.query).then(res => {
        console.log(res.data.data);
        this.stores1.grab.data = res.data.data;
        this.stores1.grab.query.totalCount = res.data.totalCount;
      });
    },
    loadGrabDisList() {
      getGrabageDisposal(this.stores.grab.query).then(res => {
        console.log(res.data.data);
        this.stores.grab.data = res.data.data;
        this.stores.grab.query.totalCount = res.data.totalCount;
      });
    },
    renderScore(Score){
      let data={
        color:'',
        text:'',
      }
      if(Score=='已投放'){
        data.color='#5cadff';
        data.text='已投放';
      }
      if(Score=='已赋分'){
        data.color='#19be6b';
        data.text='已赋分';
      }
      return data;
    },
       //申请社区下拉框
     loadDepartmentList1() {
       let data=this.$store.state.user.villageGuid;
       getshequ(data).then(res => {
        //console.log(res.data.data);
         this.list33 = res.data.data;
         let townn = Array.from(new Set(this.list33.map(x => x.towns )));
         //console.log(townn);          
        this.stores.grab.sources.town=townn;  
      });
      
    },
    xz(e){     
      let college = this.list33.filter(x => x.towns == e);
      this.stores.grab.sources.college=college.map(x => x.vname);
      this.loadGrabDisList();                  
    },
     sq(e) {
      if (e == "全部") {
        this.stores.grab.query.ccmmunity = "";
      }
      this.loadGrabDisList();  
    },
      doloadRoleList(){
          loadSimpleList().then(res=>{
              this.rolelist = res.data.data;
          })
      },
      doloadDepartmentListdetail(){
          loaddepartmentListDetail().then(res=>{
              ////console.log(res.data)
              this.departmentlist = res.data.data;
          })
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
      // this.handleSwitchFormModeToEdit();
      // this.handleResetFormUser();
      this.doLoadUser(row.garbageDisposalUuid);
    },
    handleEditNo(row) {
      this.doLoadUserNo(row.garbageDisposalUuid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadGrabDisList();
    },
    handleRefresh1() {
      this.loadGrabDisList1();
    },
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormUser();
    },
    handleSubmitUser() {
      let valid = this.validateUserForm();
        ////console.log(valid);
        ////console.log(this.formModel.fields);
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateUser();
        }
        if (this.formModel.mode === "edit") {
          this.doEditUser();
        }
      }
    },
    doCreateUser() {
      createUser(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadUserList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditUser() {
      editUser(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadUserList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    validateUserForm() {
      let _valid = false;
      this.$refs["formUser"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadUser(uuid) {
      let data=uuid;
      disUpdate(data).then(res => {
        if (res.data.code === 200) {
          this.$Message.success("赋分成功");
          this.loadGrabDisList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doLoadUserNo(uuid) {
      let data = uuid;
      disUpdateNo(data).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success("取消赋分成功");
          this.loadGrabDisList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    handleDelete(row) {
      this.doDelete(row.garbageDisposalUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      DelGrabageDisposal(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadGrabDisList();
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
      BatchGrabageDisposal({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadGrabDisList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchData() {
      this.loadGrabDisList();
    },
    rowClsRender(row, index) {
      if (row.isDelete) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.grab.query.sort.direction = column.order;
      this.stores.grab.query.sort.field = column.key;
      this.loadGrabDisList();
    },
    handleSortChange1(column) {
      this.stores1.grab.query.sort.direction = column.order;
      this.stores1.grab.query.sort.field = column.key;
      this.loadGrabDisList1();
    },
    handlePageChanged(page) {
      this.stores.grab.query.currentPage = page;
      this.loadGrabDisList();
    },
    handlePageChanged1(page) {
      this.stores1.grab.query.currentPage = page;
      this.loadGrabDisList1();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.grab.query.pageSize = pageSize;
      this.loadGrabDisList();
    },
    handlePageSizeChanged1(pageSize) {
      this.stores1.grab.query.pageSize = pageSize;
      this.loadGrabDisList1();
    },
    renderUserType(userType){
      var userTypeText = "未知";
      switch (userType) {
        case 0:
          userTypeText = "超级管理员";
          break;
        case 1:
          userTypeText = "管理员";
          break;
        case 2:
          userTypeText = "普通用户";
          break;
      }
      return userTypeText;
    },
    renderStatus(status){
      let _status = {
        color:"success",
        text:"正常"
      };
      switch(status){
        case 0:
        _status.text = "禁用";
        _status.color = "error";
        break;
      }
      return _status;
    },
    handleDetail(row){
      console.log(row)
      this.formModelopened=true;
      this.stores.grab.query.ccmmunity=row.ccmmunity;
    this.loadGrabDisList();
    }
  },
  mounted() {
    this.loadDepartmentList1();
    this.loadGrabDisList1();
    this.doloadRoleList();
    this.doloadDepartmentListdetail();
  }
};
</script>

<style>
</style>

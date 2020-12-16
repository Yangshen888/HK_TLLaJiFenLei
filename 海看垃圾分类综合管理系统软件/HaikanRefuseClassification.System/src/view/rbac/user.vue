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
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.user.query.kw"
                      placeholder="输入登录名搜索..."
                      @on-search="handleSearchUser()"
                    >
                      <!--                      <Select-->
                      <!--                        slot="prepend"-->
                      <!--                        v-model="stores.user.query.isDeleted"-->
                      <!--                        @on-change="handleSearchUser"-->
                      <!--                        placeholder="禁用状态"-->
                      <!--                        style="width:60px;"-->
                      <!--                      >-->
                      <!--                        <Option-->
                      <!--                          v-for="item in stores.user.sources.isDeletedSources"-->
                      <!--                          :value="item.value"-->
                      <!--                          :key="item.value"-->
                      <!--                        >{{item.text}}</Option>-->
                      <!--                      </Select>-->
                      <!--                      <Select-->
                      <!--                        slot="prepend"-->
                      <!--                        v-model="stores.user.query.status"-->
                      <!--                        @on-change="handleSearchUser"-->
                      <!--                        placeholder="用户状态"-->
                      <!--                        style="width:60px;"-->
                      <!--                      >-->
                      <!--                        <Option-->
                      <!--                          v-for="item in stores.user.sources.statusSources"-->
                      <!--                          :value="item.value"-->
                      <!--                          :key="item.value"-->
                      <!--                        >{{item.text}}</Option>-->
                      <!--                      </Select>-->
                    </Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <!--                  <Button-->
                  <!--                    class="txt-danger"-->
                  <!--                    icon="md-close"-->
                  <!--                    title="禁用"-->
                  <!--                    @click="handleBatchCommand('delete')"-->
                  <!--                  ></Button>-->
                  <!--                  <Button-->
                  <!--                    class="txt-success"-->
                  <!--                    icon="md-redo"-->
                  <!--                    title="恢复"-->
                  <!--                    @click="handleBatchCommand('recover')"-->
                  <!--                  ></Button>-->
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
                  title="新增用户"
                >新增用户</Button>
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
          <template slot-scope="{row,index}" slot="status">
            <Tag :color="renderStatus(row.status).color">{{renderStatus(row.status).text}}</Tag>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <!--            <Poptip-->
            <!--              confirm-->
            <!--              :transfer="true"-->
            <!--              title="确定要禁用吗?"-->
            <!--              @on-ok="handleDelete(row)"-->
            <!--              >-->
            <!--              <Tooltip placement="top" content="禁用" :delay="1000" :transfer="true">-->
            <!--                <Button type="error" size="small" shape="circle" icon="md-close"></Button>-->
            <!--              </Tooltip>-->
            <!--            </Poptip>-->
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <!--            <Tooltip placement="top" content="分配角色" :delay="1000" :transfer="true">-->
            <!--              <Button type="success" size="small" shape="circle" icon="md-contacts" @click="handleAssignRole(row)"></Button>-->
            <!--            </Tooltip>-->
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModel.fields" ref="formUser" :rules="formModel.rules" label-position="top">
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="登录名" prop="loginName">
              <Input v-model="formModel.fields.loginName" placeholder="请输入登录名" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="真实名" prop="realName">
              <Input v-model="formModel.fields.realName" placeholder="请输入真实名" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="密码" prop="passWord">
              <Input type="password" v-model="formModel.fields.passWord" v-show="false" />
              <Input type="password" v-model="formModel.fields.passWord" placeholder="请输入登录密码" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="手机号" prop="phone">
              <Input v-model="formModel.fields.phone" placeholder="请输入手机号" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="身份证号" prop="userIdCard">
              <Input v-model="formModel.fields.userIdCard" placeholder="请输入身份证号" :maxlength="18" />
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="角色" prop="userIdCard">
                    <Select
                      v-model="formModel.fields.systemRoleUuid"
                      style="width:100%"
                      @on-change="rolechange"
                      clearable
                      placeholder="请选择角色"
                    >
                      <Option
                        v-for="item in this.sysrolelist"
                        :value="item.systemRoleUuid"
                        :key="item.systemRoleUuid"
                      >{{ item.roleName }}</Option>
                    </Select>
            </FormItem>
          </Col>
          <!-- <Col span="12">
            <FormItem label="角色" prop="roleUuid">
              
              <Checkbox-group v-model="checkList" @on-change="test">
                <Checkbox
                  class="cbox"
                  v-for="it in rolelist"
                  :label="it.systemRoleUuid"
                  :disabled="disabCheck(it.roleName)"
                >
                  <span>{{it.roleName}}</span>
                </Checkbox>
              </Checkbox-group>
            </FormItem>
          </Col>-->
        </Row>
        <!-- 角色树 -->
        <Row :gutter="16" v-if="treeshow">
          <Col span="14">
            <FormItem label="管理区域">
              <Tree :data="treeData" show-checkbox @on-check-change="powerTreeChange"></Tree>
            </FormItem>
          </Col>
        </Row>
        <!-- <Row :gutter="16" v-if="isshow">
          <FormItem label="请选择商铺">
            <Select v-model="formModel.fields.shopUuid" style="width:150px" placeholder="请选择商铺">
              <Option
                v-for="item in stores.user.cityList2"
                :value="item.value"
                :key="item.value"
              >{{ item.label }}</Option>
            </Select>
          </FormItem>
        </Row>-->
        <!-- <Row :gutter="16" v-if="isshow2">
          <Col span="12">
            <FormItem label="请选择所管理的社区">
              <Select
                v-model="sVillage"
                style="width:150px"
                placeholder="请选择所管理的社区"
                @on-change="sq"
              >
                <Option
                  v-for="item in stores.user.villages"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
            </FormItem>
            <FormItem label="请选择所管理的小区" v-if="xqshow">
              <Select
                v-model="formModel.fields.streets"
                style="width:200px"
                @on-change="gongyu"
                clearable
                placeholder="请选择所管理的小区"
              >
                <Option
                  v-for="item in this.stores.user.resregion"
                  :value="item=='全部'?0:item"
                  :key="item"
                >{{ item }}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>-->
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitUser">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getUserList,
  createUser,
  getShopList,
  loadUser,
  editUser,
  deleteUser,
  batchCommand,
  showRoleTree,
  GetRoleList
} from "@/api/rbac/user";
import { getfamily1 } from "@/api/person/address";
import { loadRoleListByUserGuid, loadSimpleList } from "@/api/rbac/role";
import { loaddepartmentListDetail } from "@/api/rbac/department";
import { getshequ } from "@/api/person/address";
import { doCustomTimes } from "../../libs/util";

export default {
  name: "rbac_user_page",
  components: {
    DzTable,
  },
  data() {
    // let globalvalidateIDCord = (rule, value, callback) => {
    //   if (value !== "" && value !== null) {
    //     let reg = /^[1-9]\d{5}(18|19|20|(3\d))\d{2}((0[1-9])|(1[0-2]))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$/;
    //     if (!reg.test(value)) {
    //       callback(new Error("请输入有效的身份证号"));
    //     }
    //     callback();
    //   } else {
    //     callback(new Error("身份证号不能为空"));
    //   }
    //   callback();
    // };
    // let globalvalidatePhone = (rule, value, callback) => {
    //   if (value !== "" && value !== null) {
    //     let reg = /^[1]([3-9])[0-9]{9}$/;
    //     if (!reg.test(value)) {
    //       callback(new Error("请输入有效的手机号"));
    //     }
    //     callback();
    //   } else {
    //     callback(new Error("手机号不能为空"));
    //   }
    //   callback();
    // };
    let globalvalidatePass = (rule, value, callback) => {
      if (value !== "" && value !== null) {
        let reg = /^[^\s]+$/;
        if (!reg.test(value)) {
          callback(new Error("请输入有效的密码"));
        }
        callback();
      } else {
        callback(new Error("密码不能为空"));
      }
      callback();
    };
    let globalvalidateLName = (rule, value, callback) => {
      if (value !== "" && value !== null) {
        let reg = /^[^\s]+$/;
        if (!reg.test(value)) {
          callback(new Error("请输入有效的登录名"));
        }
        callback();
      } else {
        callback(new Error("登录名不能为空"));
      }
      callback();
    };
    let globalvalidateRName = (rule, value, callback) => {
      if (value !== "" && value !== null) {
        let reg = /^[^\s]+$/;
        if (!reg.test(value)) {
          callback(new Error("请输入有效的真实名"));
        }
        callback();
      } else {
        callback(new Error("真实名不能为空"));
      }
      callback();
    };
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
      sysrolelist:[],
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
          oldCard: "",
          //status: 1,
          isDeleted: 0,
          systemRoleUuid: "",
          userIdCard: "",
          departmentUuid: "",
          shopUuid: "",
          villageId: "",
          phone: "",
          streets: "",
          community: "",
          biotope: "",
        },
        rules: {
          loginName: [
            { type: "string", required: true, validator: globalvalidateLName },
          ],
          // userIdCard: [
          //   { type: "string", required: true, validator: globalvalidateIDCord },
          // ],
          // phone: [
          //   { type: "string", required: true, validator: globalvalidatePhone },
          // ],
          realName: [
            { type: "string", required: true, validator: globalvalidateRName },
          ],
          passWord: [
            { type: "string", required: true, validator: globalvalidatePass },
          ],
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
          cityList2: [
            {
              value: "0",
              label: "暂无商铺",
            },
            {
              value: "1",
              label: "xxx商店",
            },
          ],
          model1: "",

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
                field: "ID",
              },
            ],
          },
          sources: {
            userTypes: [
              { value: 0, text: "超级管理员" },
              { value: 1, text: "管理员" },
              { value: 2, text: "普通用户" },
            ],
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已禁" },
            ],
            statusSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" },
            ],
            statusFormSources: [
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" },
            ],
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "登录名", minWidth: 85, key: "loginName", sortable: true },
            { title: "真实名", minWidth: 85, key: "realName" },
            { title: "身份证号", minWidth: 140, key: "userIdCard" },
            // { title: "老年卡号", key: "oldCard" },
            {
              title: "用户类型",
              minWidth: 135,
              key: "userType",
              ellipsis: true,
              tooltip: true,
            },
            {
              title: "创建时间",
              minWidth: 80,
              ellipsis: true,
              tooltip: true,
              key: "addTime",
            },
            // { title: "创建者", minWidth: 110, key: "addPeople" },
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
      return this.formModel.selection.map((x) => x.systemUserUuid);
    },
  },
  methods: {
    doGetRoleList(){
      GetRoleList().then(res=>{
        if(res.data.code==200){
          this.sysrolelist=res.data.data;
          console.log(this.sysrolelist);
        }
      });
    },
    rolechange(){
      console.log(this.formModel.fields.systemRoleUuid);
    },
    test(data) {
      console.log(this.rolelist);
      let data1 = this.rolelist.find((x) => x.roleName == "商户");
      let data2 = this.rolelist.find((x) => x.roleName == "社区管理员");
      let check = data.includes(data1.systemRoleUuid);
      let check2 = data.includes(data2.systemRoleUuid);
      let data3 = this.rolelist.find((x) => x.roleName == "小区管理员");
      let check3 = data.includes(data3.systemRoleUuid);
      //this.isshow = check;
      this.isshow2 = check2;
      this.xqshow = check3;
      if (this.xqshow) {
        this.isshow2 = check3;
      }
      // //console.log(check);
    },
    loadUserList() {
      getUserList(this.stores.user.query).then((res) => {
        //console.log(res.data.data);
        this.stores.user.data = res.data.data;
        this.stores.user.query.totalCount = res.data.totalCount;
      });
    },

    loadShopList() {
      getShopList(this.stores.user.query).then((res) => {
        // //console.log(res.data.data);
        ////console.log(this.checkList);
        ////console.log(this.rolelist);
        this.stores.user.cityList2 = [];
        for (let i = 0; i < res.data.data.length; i++) {
          let data = {
            value: res.data.data[i].shopUuid,
            label: res.data.data[i].shopName,
          };
          this.stores.user.cityList2.push(data);
        }
      });
    },

    doloadRoleList() {
      loadSimpleList().then((res) => {
        //console.log(res.data.data);
        this.rolelist = res.data.data;
      });
    },
    doloadDepartmentListdetail() {
      loaddepartmentListDetail().then((res) => {
        ////console.log(res.data)
        this.departmentlist = res.data.data;
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

    //点击编辑按钮触发事件
    async handleEdit(row) {
      this.treeshow = false;
      await this.loadTreeRole();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormUser();
      this.doLoadUser(row.systemUserUuid);
      this.loadShopList();
    },

    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadUserList();
      this.doloadRoleList();
    },
    handleShowCreateWindow() {
      this.treeshow = false;
      this.sVillage = "";
      this.checkList = [];
      this.loadTreeRole();
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormUser();
    },
    sq(e) {
      getfamily1({ vill: e }).then((res) => {
        this.family = res.data.data;
        for (let i = 0; i < this.family.length; i++) {
          if (
            this.family[i].resregion == null &&
            this.family[i].street != null
          ) {
            this.family[i].resregion = this.family[i].street;
          }
        }
        let resregion = Array.from(
          new Set(this.family.map((x) => x.resregion))
        );
        resregion = resregion.filter((x) => x != null);
        this.stores.user.resregion = resregion;
        if (this.stores.user.resregion.length == 0) {
          this.stores.user.resregion = ["全部"];
        }
      });
    },
    gongyu(e) {
      if (e == "全部") {
        this.$Message.warning("请选择小区");
      }
    },
    handleSubmitUser() {
      if (
        this.formModel.fields.streets == "" &&
        this.formModel.fields.community == "" &&
        this.formModel.fields.biotope == ""&&this.checkList.length==0
      ) {
        this.$Message.warning("请选择管理范围");
        return;
      }
      if(this.formModel.fields.phone!=""){
        var reg=/^1(3[0-9]|4[5,7]|5[0,1,2,3,5,6,7,8,9]|6[2,5,6,7]|7[0,1,7,8]|8[0-9]|9[1,8,9])\d{8}$/;
        if(!(reg.test(this.formModel.fields.phone))){ 
          this.$Message.error("手机号码有误，请重填");
          return false; 
        } 
      }
      if(this.formModel.fields.userIdCard!=""){
        var reg=/^[1-9]\d{5}(18|19|20|(3\d))\d{2}((0[1-9])|(1[0-2]))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$/;
        if(!(reg.test(this.formModel.fields.userIdCard))){ 
          this.$Message.error("请输入有效的身份证号");
          return false; 
        } 
      }
      
      let valid = this.validateUserForm();
      // this.formModel.fields.systemRoleUuid = this.checkList;
      this.formModel.fields.systemRoleUuid=this.formModel.fields.systemRoleUuid.split(',');
      if (this.isshow2 == true) {
        this.formModel.fields.villageId = this.sVillage;
      } else {
        this.formModel.fields.villageId = "";
      }
      // if (this.xqshow == true) {
      //   if (
      //     this.formModel.fields.streets == "" ||
      //     this.formModel.fields.streets == undefined
      //   ) {
      //     this.$Message.warning("请选择小区");
      //     return;
      //   }
      // } else {
      //   this.formModel.fields.streets = "";
      // }
      ////console.log(this.formModel.fields.systemRoleUuid.splice(0,1));
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateUser();
        }
        if (this.formModel.mode === "edit") {
          this.doEditUser();
        }
      }
    },
    handleResetFormUser() {
      this.$refs["formUser"].resetFields();
    },
    doCreateUser() {
      createUser(this.formModel.fields).then((res) => {
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
      editUser(this.formModel.fields).then((res) => {
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
      this.$refs["formUser"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadUser(systemUserUuid) {
      this.roleLevel = 0;
      this.streets = [];
      this.community = [];
      this.biotope = [];
      loadUser({ guid: systemUserUuid }).then((res) => {
        console.log(res);
        this.formModel.fields = res.data.data.query;
        this.sVillage = this.formModel.fields.villageId;
        this.checkList = res.data.data.role;
        console.log(this.rolelist);
        let suuid = this.rolelist.find((x) => x.roleName == "超级管理员")
          .systemRoleUuid;
        let tree = this.treeData;
        if (this.checkList.includes(suuid)) {
          for (let i = 0; i < tree.length; i++) {
            tree[i].checked = true;
            for (let j = 0; j < tree[i].children.length; j++) {
              tree[i].children[j].checked = true;
              for (let k = 0; k < tree[i].children[j].children.length; k++) {
                tree[i].children[j].children[k].checked = true;
                for (
                  let x = 0;
                  x < tree[i].children[j].children[k].children.length;
                  x++
                ) {
                  tree[i].children[j].children[k].children[x].checked = true;
                }
              }
            }
          }
        }
        console.log(777777);
        console.log(this.formModel.fields.streets);
        console.log(this.formModel.fields.community);
        console.log(this.formModel.fields.biotope);
        if (
          this.formModel.fields.streets != null &&
          this.formModel.fields.streets != ""
        ) {
          this.streets = this.formModel.fields.streets.split(",");
          for (let n = 0; n < this.streets.length; n++) {
            for (let i = 0; i < tree.length; i++) {
              tree[i].indeterminate=true;
              for (let j = 0; j < tree[i].children.length; j++) {
                if (this.streets[n] == tree[i].children[j].title) {
                  tree[i].children[j].checked = true;
                  for (
                    let k = 0;
                    k < tree[i].children[j].children.length;
                    k++
                  ) {
                    tree[i].children[j].children[k].checked = true;
                    for (
                      let x = 0;
                      x < tree[i].children[j].children[k].children.length;
                      x++
                    ) {
                      tree[i].children[j].children[k].children[
                        x
                      ].checked = true;
                    }
                  }
                }
              }
            }
          }
        }
        if (
          this.formModel.fields.community != null &&
          this.formModel.fields.community != ""
        ) {
          this.community = this.formModel.fields.community.split(",");
          for (let n = 0; n < this.community.length; n++) {
            for (let i = 0; i < tree.length; i++) {
              for (let j = 0; j < tree[i].children.length; j++) {
                let check=false;
                // tree[i].children[j].indeterminate=true;
                for (let k = 0; k < tree[i].children[j].children.length; k++) {
                  if (
                    this.community[n] == tree[i].children[j].children[k].title
                  ) {
                    check=true;
                    tree[i].children[j].children[k].checked = true;
                    for (
                      let x = 0;
                      x < tree[i].children[j].children[k].children.length;
                      x++
                    ) {
                      tree[i].children[j].children[k].children[
                        x
                      ].checked = true;
                    }
                  }
                }
                if(check){
                  tree[i].children[j].indeterminate=true;
                }
              }
            }
          }
        }

        if (
          this.formModel.fields.biotope != null &&
          this.formModel.fields.biotope != ""
        ) {
          this.biotope = this.formModel.fields.biotope.split(",");
          for (let n = 0; n < this.biotope.length; n++) {
            for (let i = 0; i < tree.length; i++) {
              for (let j = 0; j < tree[i].children.length; j++) {
                let check=false;
                for (let k = 0; k < tree[i].children[j].children.length; k++) {
                  let check2=false;
                  // tree[i].children[j].indeterminate=true;
                  for (
                    let x = 0;
                    x < tree[i].children[j].children[k].children.length;
                    x++
                  ) {
                    if (
                      this.biotope[n] ==
                      tree[i].children[j].children[k].children[x].title
                    ) {
                      check2=true;
                      tree[i].children[j].children[k].children[
                        x
                      ].checked = true;
                    }
                  }
                  if(check2){
                    tree[i].children[j].children[k].indeterminate=true;
                    check=true;
                  }
                }
                if(check){
                  tree[i].children[j].indeterminate=true;
                }
              }
            }
          }
        }

        console.log(tree);
        tree.checked = true;

        //console.log(this.rolelist);
        //console.log(this.checkList);
        // let data1 = this.rolelist.find((x) => x.roleName == "商户");
        // let data2 = this.rolelist.find((x) => x.roleName == "社区管理员");
        // let check = this.checkList.includes(data1.systemRoleUuid);
        //console.log(data2);
        // let check2 = this.checkList.includes(data2.systemRoleUuid);
        // let data3 = this.rolelist.find((x) => x.roleName == "小区管理员");
        // let check3 = this.checkList.includes(data3.systemRoleUuid);
        //this.isshow = check;
        // this.isshow2 = check2;
        // this.xqshow = check3;
        // if (this.xqshow) {
        //   this.isshow2 = check3;
        //   this.sq(this.sVillage);
        // }
        // this.isshow = check;
        // //console.log(this.checkList);
        // //console.log(data1);
        // //console.log(check);
      });

      //  //勾选权限框
      //  let ch=document.getElementsByClassName("cbox");
      // // ch.classList.remove("ivu-checkbox-checked");//先移除所有选中
      //  for (let i=0;i<ch.length;i++){
      //    let a=ch[i].children[0].children[1].getAttribute('name').toUpperCase();
      //    if(this.formModel.fields.systemRoleUuid.toUpperCase().indexOf(a)!=-1)
      //      ch[i].children[0].classList.add("ivu-checkbox-checked");
      //  }
    },
    handleDelete(row) {
      this.doDelete(row.systemUserUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteUser(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadUserList();
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
        },
      });
    },
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadUserList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchUser() {
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
    // renderOwnedRoles(item) {
    //   return item.label;
    // },
    // handleChangeOwnedRolesChanged(newTargetKeys, direction, moveKeys) {
    //   this.formAssignRole.ownedRoles = newTargetKeys;
    // },
    // loadUserRoleList(guid) {
    //   this.formAssignRole.roles = [];
    //   this.formAssignRole.ownedRoles = [];
    //   loadRoleListByUserGuid(guid).then(res => {
    //     var result = res.data.data;
    //     this.formAssignRole.roles = result.roles;
    //     this.formAssignRole.ownedRoles = result.assignedRoles;
    //   });
    // },
    // handleAssignRole(row) {
    //   this.formAssignRole.opened = true;
    //   this.formAssignRole.userGuid = row.guid;
    //   this.loadUserRoleList(row.guid);
    // },
    // handleSaveUserRoles() {
    //   var data = {
    //     userGuid: this.formAssignRole.userGuid,
    //     assignedRoles: this.formAssignRole.ownedRoles
    //   };
    //   saveUserRoles(data).then(res => {
    //     this.formAssignRole.opened = false;
    //     if (res.data.code === 200) {
    //       this.$Message.success(res.data.message);
    //     } else {
    //       this.$Message.warning(res.data.message);
    //     }
    //   });
    // },
    renderUserType(userType) {
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
    disabCheck(name) {
      let arr = ["家庭用户", "督导员", "商户", "志愿者"];
      if (arr.indexOf(name) != -1) {
        return true;
      } else {
        return false;
      }
    },
    async loadTreeRole() {
      await showRoleTree().then((res) => {
        console.log(res.data.data);
        this.treeData[0] = res.data.data;
        console.log(this.treeData);
        this.treeshow = true;
      });
    },
    powerTreeChange(e) {
      this.roleLevel = 0;
      this.streets = [];
      this.community = [];
      this.biotope = [];
      this.checkList = [];
      // console.log(this.rolelist);
      // console.log(e);
      // console.log(1111);
      let sls = e.map((x) => x.title);
      // console.log(sls);
      let trls = this.treeData[0];
      // console.log(trls);
      // console.log(sls.findIndex((x) => x == "全部") );
      if (sls.findIndex((x) => x == "全部") != -1) {
        // this.checkList.push(
        //   this.rolelist.find((x) => x.roleName == "超级管理员").systemRoleUuid
        // );
        // console.log(this.checkList);
      } else {
        let child = trls.children;
        //街道集合
        let children = child.map((x) => x.title);
        children.forEach((item) => {
          if (sls.includes(item)) {
            if (this.roleLevel == 0) {
              this.roleLevel = 1;
            }
            this.streets.push(item);
            let index = sls.indexOf(item);
            sls.splice(index, 1);
            let child2 = trls.children.find((x) => x.title == item).children;
            if (child2.length > 0) {
              let children2 = child2.map((x) => x.title);
              children2.forEach((item2) => {
                if (sls.includes(item2)) {
                  let index2 = sls.indexOf(item2);
                  sls.splice(index2, 1);
                  // console.log(child2);
                  // console.log(item2);
                  let child3 = child2.find((x) => x.title == item2).children;
                  // console.log(child3);
                  if (child3.length > 0) {
                    let children3 = child3.map((x) => x.title);
                    children3.forEach((item3) => {
                      if (sls.includes(item3)) {
                        let index3 = sls.indexOf(item3);
                        sls.splice(index3, 1);
                      }
                    });
                  }
                }
              });
            }
          }
        });
        // console.log(this.streets);
        //筛除已选择的街道
        let result = children.slice();
        children.forEach((item) => {
          if (this.streets.includes(item)) {
            let index = result.indexOf(item);
            result.splice(index, 1);
          }
        });
        // console.log(result);
        let comm = [];
        result.forEach((item) => {
          let index = child.findIndex((x) => x.title == item);
          if (index >= 0) {
            let data = child[index].children;
            data.forEach((item2) => {
              comm.push(item2);
            });
          }
        });
        // console.log(comm);
        let community = [];
        if (comm.length > 0 && sls.length > 0) {
          community = comm.map((x) => x.title);
          community.forEach((item) => {
            if (sls.includes(item)) {
              if (this.roleLevel == 0) {
                this.roleLevel = 2;
              }
              this.community.push(item);
              let index = sls.indexOf(item);
              sls.splice(index, 1);
              let comm2 = comm.find((x) => x.title == item).children;
              if (comm2.length > 0) {
                let community3 = comm2.map((x) => x.title);
                community3.forEach((item3) => {
                  if (sls.includes(item3)) {
                    let index3 = sls.indexOf(item3);
                    sls.splice(index3, 1);
                  }
                });
              }
            }
          });
        }
        // console.log(this.community);

        //筛除已选择的社区
        let result2 = community.slice();
        community.forEach((item) => {
          if (this.community.includes(item)) {
            let index = result2.indexOf(item);
            result2.splice(index, 1);
          }
        });
        // console.log(result2);

        let biot = [];
        result2.forEach((item) => {
          let index = comm.findIndex((x) => x.title == item);
          if (index >= 0) {
            let data = comm[index].children;
            // console.log(comm[index]);
            data.forEach((item2) => {
              biot.push(item2);
            });
          }
        });
        // console.log(biot);
        // console.log(sls);
        if (biot.length > 0 && sls.length > 0) {
          let biotope = biot.map((x) => x.title);
          biotope.forEach((item) => {
            if (sls.includes(item)) {
              if (this.roleLevel == 0) {
                this.roleLevel = 3;
              }
              this.biotope.push(item);
              let index = sls.indexOf(item);
              sls.splice(index, 1);
            }
          });
        }
        // console.log(5555555);
        // console.log(this.streets);
        // console.log(this.community);
        // console.log(this.biotope);
        // console.log(this.roleLevel);
        this.formModel.fields.streets = this.streets.join(",");
        this.formModel.fields.community = this.community.join(",");
        this.formModel.fields.biotope = this.biotope.join(",");
        // if (this.roleLevel == 1) {
        //   this.checkList.push(
        //     this.rolelist.find((x) => x.roleName == "街道管理员").systemRoleUuid
        //   );
        // }
        // if (this.roleLevel == 2) {
        //   this.checkList.push(
        //     this.rolelist.find((x) => x.roleName == "社区管理员").systemRoleUuid
        //   );
        // }
        // if (this.roleLevel == 3) {
        //   this.checkList.push(
        //     this.rolelist.find((x) => x.roleName == "小区管理员").systemRoleUuid
        //   );
        // }
        // console.log(sls);
        // console.log(this.treeData);
        // console.log(this.checkList);
      }
    },
  },
  mounted() {
    getshequ().then((res) => {
      //console.log(res);
      this.stores.user.villages = res.data.data.map((x) => {
        let data = { value: x.villageUuid, label: x.vname };
        return data;
      });
      //console.log(this.stores.user.villages);
    });
    this.doGetRoleList();
    this.loadUserList();
    this.doloadRoleList();
    this.doloadDepartmentListdetail();
  },
};
</script>

<style>
</style>

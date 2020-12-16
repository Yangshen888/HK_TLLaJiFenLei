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
                      style="width: 150px"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.car.query.kw"
                      placeholder="输入督导员姓名"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                  <FormItem>
                    <Select
                      v-model="stores.car.query.kw1"
                      style="width: 150px"
                      clearable
                      placeholder="请选择社区"
                      @on-change="sq"
                    >
                      <Option
                        v-for="item in this.stores.car.sources.college"
                        :value="item.vname"
                        :key="item.vname"
                        >{{ item.vname }}</Option
                      >
                    </Select>
                  </FormItem>
                  <FormItem>
                    <Input
                      style="width: 150px"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.car.query.kw2"
                      placeholder="输入厢房名称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>

                  <Button
                    icon="md-refresh"
                    title="刷新"
                    @click="handleRefresh"
                  ></Button>
                </ButtonGroup>

                <Button
                  v-can="'import'"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleImportUserInfo"
                  title="导入督导员"
                  >导入</Button
                >
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加督导员"
                  >添加督导员</Button
                >
                <!-- <Button
                  v-can="'ewmexport'"                
                  icon="ios-cloud-upload"
                  type="warning"
                  @click="handleExportInfo('export')"
                  title="导出"
                >二维码导出</Button> -->
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
          <template slot-scope="{ row, index }" slot="auditState">
            <span>{{ renderAuditState(row.auditState) }}</span>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <Poptip
              confirm
              :transfer="true"
              title="确定要删除吗?"
              @on-ok="handleDelete(row)"
            >
              <Tooltip
                placement="top"
                content="删除"
                :delay="1000"
                :transfer="true"
              >
                <Button
                  type="error"
                  v-can="'deletes'"
                  size="small"
                  shape="circle"
                  icon="md-trash"
                ></Button>
              </Tooltip>
            </Poptip>
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

            <Tooltip
              placement="top"
              content="详情"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'view'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail(row)"
              ></Button>
            </Tooltip>
            <Tooltip
              placement="top"
              content="赋分记录详情"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'view'"
                type="success"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleRecordInfo(row)"
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
          <FormItem label="督导员姓名" prop="realName">
            <Input
              v-model="formModel.fields.realName"
              placeholder="请输入督导员姓名"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="性别" prop="sex">
            <Select v-model="formModel.fields.sex">
              <Option value="1">{{ "男" }}</Option>
              <Option value="0">{{ "女" }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系方式" prop="phone">
            <Input
              v-model="formModel.fields.phone"
              placeholder="请输入联系方式"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="负责箱房" prop="garbageRoomUuid">
            <Select v-model="formModel.fields.garbageRoomUuid">
              <Option
                v-for="item in this.formModel.rubbish"
                :value="item.garbageRoomUuid"
                :key="item.garbageRoomUuid"
                >{{ item.ljname }}</Option
              >
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="入职时间" prop="inTime">
            <DatePicker
              v-model="formModel.fields.inTime"
              @on-change="formModel.fields.inTime = $event"
              format="yyyy-MM-dd"
              type="datetime"
              style="width: 380px"
            ></DatePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="是否在职" prop="zaiGang">
            <Select v-model="formModel.fields.zaiGang">
              <Option value="1">{{ "是" }}</Option>
              <Option value="0">{{ "否" }}</Option>
            </Select>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitConsumable"
          >保 存</Button
        >
        <Button
          style="margin-left: 30px"
          icon="md-close"
          @click="formModel.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>

    <Drawer
      title="督导员信息导入"
      v-model="formimport.opened"
      width="500"
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
          title="督导员信息导入模板下载"
          >督导员信息导入模板下载</Button
        >
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleimport"
          :disabled="importdisable"
          >导入</Button
        >

        <Tabs value="name1">
          <TabPane label="成功" name="name1" v-html="successmsg"></TabPane>
          <TabPane label="重复" name="name2" v-html="repeatmsg"></TabPane>
          <TabPane label="失败" name="name3" v-html="defaultmsg"></TabPane>
        </Tabs>
      </div>
    </Drawer>

    <Drawer
      title="详情"
      v-model="formModel2.opened"
      width="400"
      :mask-closable="false"
      :mask="false"
    >
      <Form
        :model="formModel2.fields"
        ref="formCarDispatchDetail"
        label-position="left"
      >
        <Row :gutter="16">
          <FormItem label="督导员姓名">
            <Input v-model="formModel2.fields.realName" :readonly="true" />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="性别" prop="sex">
            <Input v-model="formModel2.fields.sex" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="联系方式">
            <Input v-model="formModel2.fields.phone" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="所属箱房">
            <Input v-model="formModel2.fields.ljname" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="入职时间">
            <Input v-model="formModel2.fields.inTime" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="是否在职" prop="zaiGang">
            <Input v-model="formModel2.fields.zaiGang" :readonly="true" />
          </FormItem>
        </Row>
      </Form>
    </Drawer>

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
          @on-page-change="handlePageChanged2"
          @on-page-size-change="handlePageSizeChanged2"
        >
          <div slot="searcher">
            <section class="dnc-toolbar-wrap">
              <Row :gutter="16">
                <Col span="16">
                  <Form inline @submit.native.prevent>
                    <FormItem>
                      <!-- <Select
                        v-model="stores.grab.query.street"
                        style="width: 150px"
                        @on-change="xz"
                        clearable
                        placeholder="请选择街道"
                      >
                        <Option
                          v-for="item in this.stores.grab.sources.town"
                          :value="item"
                          :key="item"
                          >{{ item }}</Option
                        >
                      </Select>
                      <Select
                        v-model="stores.grab.query.ccmmunity"
                        style="width: 150px"
                        @on-change="sq"
                        clearable
                        placeholder="请选择社区"
                      >
                        <Option
                          v-for="item in this.stores.grab.sources.college"
                          :value="item"
                          :key="item"
                          >{{ item }}</Option
                        >
                      </Select> -->
                    </FormItem>
                    <FormItem>
                      <Input
                        style="width: 150px"
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
                        style="width: 100px"
                      >
                        <Option
                          v-for="item in stores.grab.sources.isSources"
                          :value="item.value"
                          :key="item.value"
                          >{{ item.text }}</Option
                        >
                      </Select>
                    </FormItem>
                    <FormItem>
                      <Select
                        v-model="stores.grab.query.markType"
                        @on-change="handleSearchData()"
                        placeholder="赋分渠道"
                        style="width: 100px"
                      >
                        <Option
                          v-for="item in stores.grab.mark"
                          :value="item.value"
                          :key="item.value"
                          >{{ item.value }}</Option
                        >
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
                    <Button
                      icon="md-refresh"
                      title="刷新"
                      @click="handleRefresh2"
                    ></Button>
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
            
            @on-refresh="handleRefresh2"
            
            @on-page-change="handlePageChanged2"
            @on-page-size-change="handlePageSizeChanged2"
            
          >
            <template slot-scope="{ row, index }" slot="Score">
              <Tag :color="renderScore(row.isScore).color">{{
                renderScore(row.isScore).text
              }}</Tag>
            </template>
            <template slot-scope="{ row, index }" slot="action">
              <!-- <Poptip
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
              </Poptip> -->
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
  getCarDriverList,
  loadCarDriverDetail, //详情
  deleteCarDriver, //右边删除
  batchCommand, //右边上方删除
  huoquxiala, //垃圾箱下拉框
  huoquxiala1, //垃圾箱下拉框
  createCarDriver, //添加（保存）
  editCarDriver, //编辑（保存）
  loadCarDriver, //加载
  globalvalidatePhone,
  exportInfoCommand,
  UserInfoImport,
} from "@/api/person/supervisor";
import {
  getGrabageDisposal,
  DelGrabageDisposal,
  BatchGrabageDisposal,
  disUpdate,
  disUpdateNo,
  getVillageList,
} from "@/api/grab/grabagedisposal";
import config from "@/config";
import { getshequ } from "@/api/base/house";
export default {
  name: "supervisor",
  components: {
    DzTable,
  },
  data() {
    let globalvalidatePhone = (rule, value, callback) => {
      if (value !== "" && value !== null) {
        var reg = /^1[3456789]\d{9}$/;
        if (!reg.test(value)) {
          callback(new Error("请输入有效的电话号码"));
        }
        callback();
      } else {
        callback(new Error("电话号码不能为空"));
      }
      callback();
    };
    let globalvalidateSex = (rule, value, callback) => {
      //console.log(value);
      if (value == undefined) {
        callback(new Error("性别不能为空"));
      } else if (value !== "" && value !== null) {
        callback();
      } else {
        callback(new Error("性别不能为空"));
      }
      callback();
    };
    let globalvalidateZaigang = (rule, value, callback) => {
      //console.log(value);
      if (value == undefined) {
        callback(new Error("是否在职不能为空"));
      } else if (value !== "" && value !== null) {
        callback();
      } else {
        callback(new Error("是否在职不能为空"));
      }
      callback();
    };
    return {
      //导入
      url: config.baseUrl.dev,
      importdisable: false,

      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      formimport: {
        opened: false,
      },
      formModelopened: false,
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        selection: [],
        fields: {
          realName: "",
          phone: "",
          inTime: "",
          zaiGang: "1",
          sex: "1",
          garbageRoomUuid: "",
          isDeleted: 0,
          systemUserUuid: "",
        },
        rubbish: [{ garbageRoomUuid: "0", ljname: "全部" }],
        rules: {
          realName: [
            { type: "string", required: true, message: "请输入督导员姓名" },
          ],
          //  loginName: [
          //   { type: "string", required: true, message: "请输入账号" }
          // ] ,
          passWord: [{ type: "string", required: true, message: "请输入密码" }],
          phone: [
            { type: "string", required: true, validator: globalvalidatePhone },
          ],
          garbageRoomUuid: [{ required: true, message: "请选择负责厢房" }],
          sex: [
            { type: "string", required: true, validator: globalvalidateSex },
          ],
          zaiGang: [
            {
              type: "string",
              required: true,
              validator: globalvalidateZaigang,
            },
          ],
        },
      },
      formModel2: {
        opened: false,
        title: "详情",
        selection: [],
        fields: {
          realName: "",
          passWord: "",
          phone: "",
          inTime: "",
          garbageRoomUuid: "",
          zaiGang: "",
          ljname: "",
        },
      },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        export: { name: "export", title: "导出" },
      },
      //查询搜索
      stores: {
        car: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "", //姓名
            kw1: "",
            kw2: "",
            vuuid: this.$store.state.user.villageGuid,
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
            //社区集合
            college: [{ vname: "全部" }],
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "督导员姓名", key: "realName", sortable: true },
            { title: "联系方式", key: "phone" },
            { title: "管理箱房", key: "ljname", ellipsis: true, tooltip: true },
            { title: "是否在职", key: "zaiGang" },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
        grab: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            time: "",
            gruuid:"",
            isScore: "已赋分",
            vuuid: this.$store.state.user.villageGuid,
            street: "",
            ccmmunity: "",
            markType: "全部",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          mark: [
            { value: "全部" },
            { value: "家庭码" },
            { value: "百姓码" },
            { value: "市民卡" },
          ],
          sources: {
            //社区集合
            college: ["全部"],
            //街道集合
            town: ["全部"],
            isSources: [
              { value: "全部", text: "全部" },
              { value: "已投放", text: "已投放" },
              { value: "已赋分", text: "已赋分" },
            ],
            userTypes: [
              { value: 0, text: "超级管理员" },
              { value: 1, text: "管理员" },
              { value: 2, text: "普通用户" },
            ],
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已删" },
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
            {
              title: "住户",
              key: "address",
              ellipsis: true,
              tooltip: true,
              // sortable: true,
            },
            { title: "箱房名", key: "roomID", ellipsis: true, tooltip: true },
            { title: "赋分渠道", key: "markType", width: 100 },
            { title: "赋分状态", key: "isScore", width: 100, slot: "Score" },
            { title: "投放时间", key: "addTime" },
            { title: "赋分时间", key: "scoreAddtime" },
            {
              title: "本次积分",
              width: 80,
              ellipsis: true,
              tooltip: true,
              key: "score",
            },
            // {
            //   title: "操作",
            //   align: "center",
            //   width: 80,
            //   className: "table-command-column",
            //   slot: "action",
            // },
          ],
          data: [],
        },
      },
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建督导员申请";
      }
      if (this.formModel.mode === "edit") {
        return "编辑督导员信息";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.systemUserUuid);
    }, //删除
  },
  methods: {
    handlePageChanged2(page){
      this.stores.grab.query.currentPage = page;
      this.loadGrabDisList();
    },
    handlePageSizeChanged2(pageSize) {
      this.stores.grab.query.pageSize = pageSize;
      this.loadGrabDisList();
    },
    handleRefresh2(){
      this.loadGrabDisList();
    },
    handleRecordInfo(row){
      console.log(row);
      this.stores.grab.query.gruuid=row.garbageRoomUuid;
      this.loadGrabDisList();
      this.formModelopened=true;
    },
    handleSearchData() {
      this.loadGrabDisList();
    },
    loadGrabDisList() {
      getGrabageDisposal(this.stores.grab.query).then((res) => {
        console.log(res.data.data);
        this.stores.grab.data = res.data.data;
        this.stores.grab.query.totalCount = res.data.totalCount;
      });
    },
    sq(e) {
      if (e == "全部") {
        this.stores.car.query.ccmmunity = "";
      }
      this.loadCarDispatchList();
    },
    loadVillages() {
      getshequ(null).then((res) => {
        console.log(res);
        Array.prototype.push.apply(
          this.stores.car.sources.college,
          res.data.data
        );
        //this.stores.car.sources.college=res.data.data;
        //let townn = Array.from(new Set(this.list33.map(x => x.towns )));
      });
    },
    //获取调度数据
    loadCarDispatchList() {
      getCarDriverList(this.stores.car.query).then((res) => {
        //console.log(res.data.data)
        this.stores.car.data = res.data.data;
        this.stores.car.query.totalCount = res.data.totalCount;
      });
    },
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
        this.url + "UploadFiles/ImportUserInfoModel/督导员导入模板.xlsx";
    },
    //翻页
    handlePageChanged(page) {
      this.stores.car.query.currentPage = page;
      this.loadCarDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.car.query.pageSize = pageSize;
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
      this.$refs["formdispatch"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadCarDispatch(systemUserUuid) {
      loadCarDriver({ guid: systemUserUuid }).then((res) => {
        this.formModel.fields = res.data.data[0];
      });
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatch"].resetFields();
      this.formModel.fields.garbageRoomUuid = "";
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
      this.doDelete(row.systemUserUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteCarDriver(ids).then((res) => {
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
        },
      });
    },
    //右上边删除
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
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
    //右边编辑
    handleEdit(row) {
      this.handleOpenFormWindow();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormDispatch();
      //console.log(row)
      this.doLoadCarDispatch(row.systemUserUuid);
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
      this.loadCarDispatchList();
    },
    //右边详情
    handleDetail(row) {
      ////console.log(row);
      this.formModel2.opened = true;
      this.$refs["formCarDispatchDetail"].resetFields();
      loadCarDriverDetail({ guid: row.systemUserUuid }).then((res) => {
        this.formModel2.fields = res.data.data[0];
      });
    },
    //添加按钮（公车调度申请）
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow(); //打开窗口
      this.handleResetFormDispatch(); //清空表单
      // this.formModel.fields.realName = this.$store.state.user.userName;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    //申请垃圾箱房下拉框
    loadlgxtList() {
      huoquxiala().then((res) => {
        this.formModel.rubbish = res.data.data;
      });
    },
    //添加公车调度（保存）
    docreateCarDispatch() {
      //console.log(this.formModel.fields);
      createCarDriver(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow(); //关闭表单
          this.loadCarDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑公车调度（保存）
    doEditCarDispatch() {
      editCarDriver(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadCarDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //保存按钮
    handleSubmitConsumable() {
      let regn = /^[^\s]+$/;
      if (!regn.test(this.formModel.fields.realName)) {
        this.$Message.warning("请输入有效的姓名");
        return;
      }
      var reg = /(^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$)|(^0{0,1}1[3|4|5|6|7|8|9][0-9]{9}$)/;
      if (!reg.test(this.formModel.fields.phone)) {
        this.$Message.warning("电话不合法");
        return;
      }
      let valid = this.validateDispatchForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.docreateCarDispatch();
        }
        if (this.formModel.mode === "edit") {
          this.doEditCarDispatch();
        }
      }
    },
    //提交按钮
    handleSubmitToAuditDispatch() {
      let valid = this.validateDispatchForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateToAuditCarDispatch();
        }
        if (this.formModel.mode === "edit") {
          this.doEditToAuditCarDispatch();
        }
      }
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
          ////console.log(file);
          this.exceldata = formData;
        } else {
          this.isexitexcel = false;
        }
      }
      //////console.log(this.exceldata);
    },
    async handleimport() {
      this.importdisable = true;
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      if (this.isexitexcel) {
        // this.intervalId = setInterval(() => {
        //     this.loadgetprogress()
        // }, 500)
        await UserInfoImport(this.exceldata).then((res) => {
          //////console.log(res.data);
          if (res.data.code == 200) {
            this.time = res.data.data.time;
            this.successmsg = res.data.data.successmsg;
            this.repeatmsg = res.data.data.repeatmsg;
            this.defaultmsg = res.data.data.defaultmsg;
            this.loadCarDispatchList();
            //clearInterval(this.intervalId);
            //this.showprogress = false;
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
    //导出相关操作
    handleExportInfo(command) {
      //console.log(this.selectedRowsId);

      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要生成并 [" +
          this.commands[command].title +
          "] 当前已选督导员的二维码?</p>",
        loading: true,
        onOk: () => {
          this.doExportInfoCommand(command);
        },
      });
    },
    doExportInfoCommand(command) {
      exportInfoCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          // //console.log(res.data);
          var DZurl = res.data.data;
          window.location = this.url + DZurl;
          this.$Message.success(res.data.message);
          this.loadCarDispatchList();
          // this.formModel1.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
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
  },
  mounted() {
    this.loadCarDispatchList();
    this.loadlgxtList(); //垃圾箱房下拉框
    this.loadVillages();
  },
};
</script>
<style>
</style>

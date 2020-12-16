<template>
  <div>
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
                      v-model="stores.grab.query.kw"
                      placeholder="请输入问题类型"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                  <FormItem>
                    <Select
                      v-model="stores.grab.query.kw1"
                      @on-change="handleSearchDispatch()"
                      placeholder="所在箱房"
                      clearable
                      style="width:150px;"
                    >
                      <Option
                        v-for="item in stores.grab.sources.question"
                        :value="item.garbageRoomUuid"
                        :key="item.garbageRoomUuid"
                      >{{item.ljname}}</Option>
                    </Select>
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
          <template slot-scope="{row, index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                v-if="row.estimate=='' ||row.estimate==null "
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
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
    <Drawer title="处理" v-model="formModel.opened" width="400" :mask-closable="false" :mask="true">
      <Form :model="formModel.fields" ref="formdispatch" label-position="top">
        <Row :gutter="16">
          <FormItem label="处理结果" prop="estimate">
            <Input type="textarea" v-model="formModel.fields.estimate" placeholder="请输入处理结果" />
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
      title="问题反馈详情"
      v-model="formModel1.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form :model="formModel1.fields" ref="formdispatch" label-position="top">
        <Row :gutter="16">
          <FormItem label="反馈人" prop="addPeople">
            <Input readonly v-model="formModel1.fields.addPeople" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="反馈车辆" prop="carNum">
            <Input readonly v-model="formModel1.fields.carNum" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="反馈箱房" prop="ljname">
            <Input readonly v-model="formModel1.fields.ljname" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="反馈问题" prop="queType">
            <Input readonly v-model="formModel1.fields.queType" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="反馈街道" prop="towns">
            <Input readonly v-model="formModel1.fields.towns" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="反馈社区" prop="vname">
            <Input readonly v-model="formModel1.fields.vname" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="反馈时间" prop="addTime">
            <Input readonly v-model="formModel1.fields.addTime" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" prop="remarks">
            <Input readonly v-model="formModel1.fields.remarks" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="处理结果" prop="estimate">
            <Input readonly type="textarea" v-model="formModel1.fields.estimate" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="处理时间" prop="esttime">
            <Input readonly v-model="formModel1.fields.esttime" placeholder="无" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="处理人" prop="estpeople">
            <Input readonly v-model="formModel1.fields.estpeople" placeholder="无" />
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button style="margin-left: 30px" icon="md-close" @click="formModel1.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import { getpeopleDriverList } from "@/api/grab/question"; //申请垃圾箱房下拉框
import {
  getList,
  huoquxiala, //垃圾箱房下拉框
  batchCommand, //右边上方删除
  ShowQuestion, //获取
  EditQuestion, //编辑
} from "@/api/grab/question";
import { getshequ } from "@/api/base/house";

export default {
  name: "question",
  components: {
    DzTable,
  },
  data() {
    return {
      formModel: {
        opened: false,
        title: "处理",
        selection: [],
        fields: {
          guid: "",
          estimate: "",
        },
      },
      formModel1: {
        opened: false,
        title: "问题反馈详情",
        selection: [],
        fields: {
          guid: "",
          addPeople:"",
          carNum:"",
          ljname:"",
          queType:"",
          towns:"",
          vname:"",
          addTime:"",
          estimate:"",
          esttime:"",
          estpeople:"",
          remarks:""
        },
      },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
      },

      list33: [],
      //查询搜索
      stores: {
        grab: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            vuuid: this.$store.state.user.villageGuid,
            street: "",
            ccmmunity: "",
            isDelete: 0,
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
            college: ["全部"],
            //街道集合
            town: ["全部"],
            //垃圾箱房集合
            question: [{ garbageRoomUuid: "0", ljname: "全部" }],
          },
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "questionUuid" },
            { title: "问题类型", key: "queType", sortable: true },
            { title: "问题箱房", key: "ljname" },
            { title: "反馈车辆", key: "carNum" },
            { title: "街道", key: "towns" },
            { title: "社区", key: "vname" },
            { title: "时间", key: "addTime" },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
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
      return this.formModel.selection.map(x => x.questionUuid);
    } //删除
  },
  methods: {
    //获取信息数据
    loadCarDispatchList() {
      getList(this.stores.grab.query).then(res => {
        ////console.log(res.data.data)
        this.stores.grab.data = res.data.data;
        ////console.log(res.data.data);
        this.stores.grab.query.totalCount = res.data.totalCount;
      });
    },
    //获取督导员下拉框
    loadDepartmentList2() {
       huoquxiala().then(res => {
        this.stores.grab.sources.question=res.data.data;
        ////console.log(this.stores.grab.sources.question)
      });
    },
    //翻页
    handlePageChanged(page) {
      this.stores.grab.query.currentPage = page;
      this.loadCarDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.grab.query.pageSize = pageSize;
      this.loadCarDispatchList();
    },
    //行样式
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    //搜索
    handleSearchDispatch() {
      this.loadCarDispatchList();
    },
    //刷新
    handleRefresh() {
      this.loadCarDispatchList();
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
    //申请社区下拉框
    loadDepartmentList1() {
      let data = this.$store.state.user.villageGuid;
      getshequ(data).then((res) => {
        //console.log(res.data.data);
        this.list33 = res.data.data;
        let townn = Array.from(new Set(this.list33.map((x) => x.towns)));
        //console.log(townn);
        this.stores.grab.sources.town = townn;
      });
    },
    xz(e) {
      let college = this.list33.filter((x) => x.towns == e);
      this.stores.grab.sources.college = college.map((x) => x.vname);
      this.loadCarDispatchList();
    },
    sq(e) {
      if (e == "全部") {
        this.stores.grab.query.ccmmunity = "";
      }
      this.loadCarDispatchList();
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatch"].resetFields();
    },
    //右边编辑
    handleEdit(row) {
      this.formModel.opened = true;
      // this.handleOpenFormWindow();
      // this.handleSwitchFormModeToEdit();
      this.handleResetFormDispatch();
      this.doShowQuestion(row.questionUuid);
    },
    //右边详情
    handleDetail(row) {
      console.log(row);
      this.formModel1.opened = true;
      this.handleResetFormDispatch();
      let data=row.questionUuid;
      ShowQuestion(data).then((res) => {
        if (res.data.code == 200) {
          this.formModel1.fields = res.data.data;
          this.formModel1.fields.guid = data;
          this.formModel1.fields.addPeople=row.addPeople;
          this.formModel1.fields.ljname=row.ljname;
          this.formModel1.fields.towns=row.towns;
          this.formModel1.fields.vname=row.vname;
          this.formModel1.fields.carNum=row.carNum;
        }
      });
    },
    doShowQuestion(row) {
      let data = row;
      ShowQuestion(data).then((res) => {
        if (res.data.code == 200) {
          this.formModel.fields = res.data.data;
          this.formModel.fields.guid = data;
        }
      });
    },
    //保存按钮
    handleSubmitConsumable() {
      if (this.formModel.fields.estimate==null ||this.formModel.fields.estimate=="") {
        this.$Message.warning("请输入处理结果");
        return;
      }
      EditQuestion(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadCarDispatchList();
          this.formModel.selection = [];
          this.formModel.opened = false;
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    handleSortChange(column) {
      this.stores.grab.query.sort.direction = column.order;
      this.stores.grab.query.sort.field = column.key;
      this.loadCarDispatchList();
    }
  },
  mounted() {
    this.loadDepartmentList1();
    //  this.loadDepartmentList1();//社区下拉框
    this.loadCarDispatchList();
    this.loadDepartmentList2();//垃圾箱房下拉框
    //  this.loadDepartmentList3();//收运车辆下拉框
  }
};
</script>
<style>
</style>
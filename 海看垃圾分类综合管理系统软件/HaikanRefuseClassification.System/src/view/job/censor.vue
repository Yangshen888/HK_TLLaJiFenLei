<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.job.query.totalCount"
        :pageSize="stores.job.query.pageSize"
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
                      v-model="stores.job.query.kw"
                      placeholder="请输入督导员姓名"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                  <FormItem>
                    <DatePicker
                      type="daterange"
                      v-model="stores.job.query.kw2"
                      format="yyyy/MM/dd"
                      placement="top"
                      placeholder="输入时间搜索..."
                      style="width: 200px"
                      :confirm="true"
                      :editable="false"
                      @on-ok="handleSearchDispatch()"
                    ></DatePicker>
                  </FormItem>
                  <FormItem>
                    <Select
                      v-model="stores.indexId"
                      @on-change="dakatime()"
                      placeholder="打卡时间"
                      style="width:150px;"
                    >
                      <Option
                        v-for="item in stores.index"
                        :value="item.indexId"
                        :key="item.indexId"
                      >{{item.name}}</Option>
                    </Select>
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
          :data="stores.job.data"
          :columns="stores.job.columns"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <template slot-scope="{row, index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>
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

    <Drawer title="详情" v-model="formModel2.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel2.fields" ref="formCarDispatchDetail" label-position="left">
        <Row :gutter="16">
          <FormItem label="督导员姓名">
            <Input v-model="formModel2.fields.realName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="上班打卡状态">
            <Input v-model="formModel2.fields.startState" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="上午上班打卡时间">
            <Input v-model="formModel2.fields.amstartTime" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="上午上班打卡地点">
            <Input v-model="formModel2.fields.amstartPlace" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="上午下班打卡时间">
            <Input v-model="formModel2.fields.amendTime" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="上午下班打卡地点">
            <Input v-model="formModel2.fields.amendPlace" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="下午上班打卡时间">
            <Input v-model="formModel2.fields.pmstartTime" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="下午上班打卡地点">
            <Input v-model="formModel2.fields.pmstartPlace" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="下午下班打卡时间">
            <Input v-model="formModel2.fields.pmendTime" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="下午下班打卡地点">
            <Input v-model="formModel2.fields.pmendPlace" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="下班打卡状态" prop="state">
            <Input v-model="formModel2.fields.endState" :readonly="true" />
          </FormItem>
        </Row>
      </Form>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getList,
  loadCarDriverDetail, //详情
  loadCarDriver //加载
} from "@/api/job/censor";

export default {
  name: "censor",
  components: {
    DzTable
  },

  data() {
    return {
      formModel2: {
        opened: false,
        title: "详情",
        selection: [],
        fields: {
          //attendanceUUID:"",
          realName: "",
          startState: "",
          amstartTime: "",
          amstartPlace: "",
          amendTime: "",
          amendPlace: "",
          endState: "",
          pmstartTime: "",
          pmstartPlace: "",
          pmendTime: "",
          pmendPlace: ""
        }
      },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" }
      },
      //查询搜索
      stores: {
        indexId: "",
        index: [
          {
            indexId: 0,
            name: "全天"
          },
          {
            indexId: 1,
            name: "上午"
          },
          {
            indexId: 2,
            name: "下午"
          }
        ],
        job: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw2: "",
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
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "attendanceUUID" },
            { title: "督导员姓名", key: "realName", sortable: true },
            { title: "上班状态", key: "startState" },
            { title: "上班打卡时间", key: "amstartTime" },
            { title: "下班状态", key: "endState" },
            { title: "下班打卡时间", key: "pmendTime" },
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
  computed: {},

  methods: {
    //获取全天信息数据
    loadCarDispatchList() {
      getList(this.stores.job.query).then(res => {
        ////console.log(res.data.data)
        this.stores.job.data = res.data.data;
        ////console.log(res.data.data);
        this.stores.job.query.totalCount = res.data.totalCount;
      });
    },
    //翻页
    handlePageChanged(page) {
      this.stores.job.query.currentPage = page;
      this.loadCarDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.job.query.pageSize = pageSize;
      this.loadCarDispatchList();
    },
    //打开窗口
    // handleOpenFormWindow() {
    //   this.formModel.opened = true;
    // },
    // //自动关闭窗口
    // handleCloseFormWindow() {
    //   this.formModel.opened = false;
    // },
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
    dakatime() {
      if (this.stores.indexId == 0) {
        this.stores.job.columns = [
          { type: "selection", width: 50, key: "attendanceUUID" },
          { title: "督导员姓名", key: "realName", sortable: true },
          { title: "上班状态", key: "startState" },
          { title: "上班打卡时间", key: "amstartTime" },
          { title: "下班状态", key: "endState" },
          { title: "下班打卡时间", key: "pmendTime" },
          {
            title: "操作",
            align: "center",
            width: 150,
            className: "table-command-column",
            slot: "action"
          }
        ];
        this.loadCarDispatchList();
      } else if (this.stores.indexId == 1) {
        this.stores.job.columns = [
          { type: "selection", width: 50, key: "attendanceUUID" },
          { title: "督导员姓名", key: "realName", sortable: true },
          { title: "上班打卡时间", key: "amstartTime" },
          { title: "下班打卡时间", key: "amendTime" },
          {
            title: "操作",
            align: "center",
            width: 150,
            className: "table-command-column",
            slot: "action"
          }
        ];
        this.loadCarDispatchList();
      } else {
        this.stores.job.columns = [
          { type: "selection", width: 50, key: "attendanceUUID" },
          { title: "督导员姓名", key: "realName", sortable: true },
          { title: "上班打卡时间", key: "pmstartTime" },
          { title: "下班打卡时间", key: "pmendTime" },
          {
            title: "操作",
            align: "center",
            width: 150,
            className: "table-command-column",
            slot: "action"
          }
        ];
        this.loadCarDispatchList();
      }
    },
    //刷新
    handleRefresh() {
      this.loadCarDispatchList();
    },
    //右边详情
    handleDetail(row) {
      ////console.log(row);
      this.formModel2.opened = true;
      this.$refs["formCarDispatchDetail"].resetFields();
      loadCarDriverDetail({ guid: row.attendanceUuid }).then(res => {
        this.formModel2.fields = res.data.data;
        ////console.log(this.formModel2.fields);
      });
    }
  },
  mounted() {
    //  this.loadDepartmentList1();//社区下拉框
    this.loadCarDispatchList();
    //  this.loadDepartmentList2();//督导员下拉框
    //  this.loadDepartmentList3();//收运车辆下拉框
  }
};
</script>
<style>
</style>
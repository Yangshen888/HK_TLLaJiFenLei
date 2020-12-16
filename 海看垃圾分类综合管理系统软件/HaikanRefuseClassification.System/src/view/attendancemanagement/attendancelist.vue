<template>
    <div>
      <Card>
        <dz-table
          :totalCount="stores.attendancelist.query.totalCount"
          :pageSize="stores.attendancelist.query.pageSize"
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
                        v-model="stores.attendancelist.query.kw"
                        placeholder="输入姓名搜索..."
                        @on-search="handleSearchAttendancelist()"
                      >
                          <Select
                            slot="prepend"
                            v-model="stores.attendancelist.query.departmentUuid"
                            @on-change="handleSearchAttendancelist"
                            placeholder="科室"
                            style="width:100px;"
                          >
                            <Option
                              v-for="item in departmentlist"
                              :value="item.departmentUuid"
                              :key="item.departmentUuid"
                            >{{item.departmentName}}</Option>
                        </Select>
                      </Input>
                    </FormItem>
                    <FormItem>
                      <DatePicker
                        type="daterange"
                        v-model="stores.attendancelist.query.time"
                        format="yyyy/MM/dd"
                        placement="top"
                        placeholder="输入时间搜索..."
                        style="width: 200px"
                        :confirm="true"
                        :editable="false"
                        @on-ok="handleSearchAttendancelist()">
                      </DatePicker>
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
            :data="stores.attendancelist.data"
            :columns="stores.attendancelist.columns"
            @on-select="handleSelect"
            @on-selection-change="handleSelectionChange"
            @on-refresh="handleRefresh"
            @on-page-change="handlePageChanged"
            @on-page-size-change="handlePageSizeChanged"
            @on-sort-change="handleSortChange"
          >
            <template slot-scope="{ row, index }" slot="action">
              <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
                <Button v-can="'detail'" type="warning" size="small" shape="circle" icon="md-search" @click="handleDetail(row)"></Button>
              </Tooltip>
            </template>
          </Table>
        </dz-table>
      </Card>
      <Drawer
        title="详情"
        v-model="formModel.opened"
        width="400"
        :mask-closable="false"
        :mask="false"
        :styles="styles"
      >
        <Form :model="formModel.fields" ref="formAttendance"  label-position="left">
          <Row :gutter="16">
              <FormItem label="日期" >
                <Input v-model="formModel.fields.colckDate" :readonly="true"/>
              </FormItem>
          </Row>
          <Row :gutter="16">
            <Col span="12">
            <FormItem label="姓名" >
              <Input v-model="formModel.fields.userName" :readonly="true"/>
            </FormItem>
            </Col>
            <Col span="12">
              <FormItem label="科室" >
                <Input v-model="formModel.fields.departmentName" :readonly="true"/>
              </FormItem>
            </Col>
          </Row>
          <Row :gutter="16">
            <Col span="12">
              <FormItem label="上班打卡时间" >
                <Input v-model="formModel.fields.startTime" :readonly="true"/>
              </FormItem>
            </Col>
            <Col span="12">
              <FormItem label="上班打卡地点" >
                <Input v-model="formModel.fields.startPlace" :readonly="true"/>
              </FormItem>
            </Col>
          </Row>
          <Row :gutter="16">
            <Col span="12">
              <FormItem label="下班打卡时间" >
                <Input v-model="formModel.fields.endTime" :readonly="true"/>
              </FormItem>
            </Col>
            <Col span="12">
              <FormItem label="下班打卡地点" >
                <Input v-model="formModel.fields.endPlace" :readonly="true"/>
              </FormItem>
            </Col>
          </Row>
          <Row :gutter="16">
            <Col span="12">
              <FormItem label="上班打卡状态" >
                <Input v-model="formModel.fields.startState" :readonly="true"/>
              </FormItem>
            </Col>
            <Col span="12">
              <FormItem label="下班打卡状态" >
                <Input v-model="formModel.fields.endState" :readonly="true"/>
              </FormItem>
            </Col>
          </Row>
        </Form>
      </Drawer>
    </div>
</template>

<script>
    import DzTable from "_c/tables/dz-table.vue";
    import {
        getAttendancelistList,
        loadAttendanceDetail
    } from "@/api/attendancemanagement/attendancelist";
    import { loaddepartmentListDetail } from "@/api/rbac/department";
    export default {
        name: "attendancelist",
        components: {
            DzTable
        },
        data() {
            return {
                formModel: {
                    opened: false,
                    title: "详情",
                    selection: [],
                    fields: {
                        colckDate:"",
                        userName:"",
                        departmentName:"",
                        startTime: "",
                        startPlace:"",
                        startState:"",
                        endTime:"",
                        endPlace:"",
                        endState:"",
                    },
                },
                stores: {
                    attendancelist: {
                        query: {
                            totalCount: 0,
                            pageSize: 20,
                            currentPage: 1,
                            kw: "",
                            time:"",
                            departmentUuid:"",
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
                            { type: "selection", width: 50, key: "handle" },
                            { title: "姓名", key: "userName"},
                            { title: "科室", key: "departmentName"},
                            { title: "日期", key: "colckDate"},
                            { title: "上班打卡时间", key: "startTime"},
                            { title: "下班打卡时间", key: "endTime"},
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
                departmentlist:[
                    {departmentName:"全部",departmentUuid:""}
                ]
            };
        },
        methods:{
            loadattendancelistList() {
                getAttendancelistList(this.stores.attendancelist.query).then(res => {
                    this.stores.attendancelist.data = res.data.data;
                    this.stores.attendancelist.query.totalCount = res.data.totalCount;
                });
            },
            doloadDepartmentListdetail(){
                loaddepartmentListDetail().then(res=>{
                    this.departmentlist = this.departmentlist.concat(res.data.data);
                })
            },
            handleSelect(selection, row) {},
            handleSelectionChange(selection) {
                this.formModel.selection = selection;
            },
            handleRefresh() {
                this.loadattendancelistList();
            },
            handleSearchAttendancelist() {
                this.loadattendancelistList();
            },
            handleDetail(row){
                this.formModel.opened = true;
                this.$refs["formAttendance"].resetFields();
                loadAttendanceDetail({ guid: row.attendanceUuid}).then(res => {
                    this.formModel.fields = res.data.data;
                    this.formModel.fields.startState=this.renderstartstate(this.formModel.fields.startState);
                    this.formModel.fields.endState=this.renderendstate(this.formModel.fields.endState)
                });
            },
            handleSortChange(column) {
                this.stores.attendancelist.query.sort.direction = column.order;
                this.stores.attendancelist.query.sort.field = column.key;
                this.loadattendancelistList();
            },
            handlePageChanged(page) {
                this.stores.attendancelist.query.currentPage = page;
                this.loadattendancelistList();
            },
            handlePageSizeChanged(pageSize) {
                this.stores.attendancelist.query.pageSize = pageSize;
                this.loadattendancelistList();
            },
            renderstartstate(state){
                var statetext = "未知";
                switch (state) {
                    case 0:
                        statetext="未打卡";
                        break;
                    case 1:
                        statetext="正常";
                        break;
                    case 2:
                        statetext="迟到";
                        break;
                }
                return statetext;
            },
            renderendstate(state){
                var statetext = "未知";
                switch (state) {
                    case 0:
                        statetext="未打卡";
                        break;
                    case 1:
                        statetext="正常";
                        break;
                    case 2:
                        statetext="早退";
                        break;
                }
                return statetext;
            }
        },
        mounted() {
            this.loadattendancelistList();
            this.doloadDepartmentListdetail();
        }
    }
</script>

<style scoped>

</style>

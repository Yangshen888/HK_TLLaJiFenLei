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
                    <Select
                      v-model="stores.car.query.street"
                      style="width:150px"
                      @on-change="xz"
                      clearable
                      placeholder="请选择街道"
                    >
                      <Option
                        v-for="item in this.stores.car.sources.town"
                        :value="item"
                        :key="item"
                      >{{ item }}</Option>
                    </Select>
                    <Select
                      v-model="stores.car.query.ccmmunity"
                      style="width:150px"
                      @on-change="sq"
                      clearable
                      placeholder="请选择社区"
                    >
                      <Option
                        v-for="item in this.stores.car.sources.college"
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
                      v-model="stores.car.query.kw"
                      placeholder="输入地址"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                  <!-- <FormItem>
                    <DatePicker
                      type="daterange"
                      v-model="stores.car.query.kw2"
                      format="yyyy/MM/dd"
                      placement="top"
                      placeholder="输入时间搜索..."
                      style="width: 200px"
                      :confirm="true"
                      :editable="false"
                      @on-ok="handleSearchDispatch()"
                      @on-clear="handleSearchDispatch()"
                    ></DatePicker>
                  </FormItem> -->
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
<!--                  <Button-->
<!--                    v-can="'delete'"-->
<!--                    class="txt-danger"-->
<!--                    icon="md-trash"-->
<!--                    title="删除"-->
<!--                    @click="handleBatchCommand('delete')"-->
<!--                  ></Button>-->
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
        <template slot-scope="{row,index}" slot="State">
            <Tag :color="renderScore(row.state).color">{{renderScore(row.state).text}}</Tag>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <!--            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">-->
            <!--              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">-->
            <!--                <Button type="error" size="small" shape="circle" icon="md-trash"></Button>-->
            <!--              </Tooltip>-->
            <!--            </Poptip>-->
            <Button
              v-can="'show'"
              type="warning"
              size="small"
              shape="circle"
              icon="md-search"
              @click="loadRecord(row)"
            ></Button>
            <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
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
      title="未兑换积分"
      v-model="homeScose.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="homeScose.fields"
        ref="formdispatch"
        :rules="homeScose.rules"
        label-position="left"
      >
        <Row :gutter="16" v-if="getMonth-1==0||getMonth==0">
          <FormItem label="去年十二月兑换积分" prop="lastDec">
            <i-input
              v-model="homeScose.fields.lastDec"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.lastDecScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('lastDecScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.lastDecScore==0&&getMonth==0&&getDay>=1&&getDay<=exchange">兑换</i-button>
                <i-button type="info" v-if="homeScore1.lastDecScore==0&&getMonth-1==0" :disabled="true">未兑换</i-button>
              </Tooltip>
            </Poptip>
            <i-button type="info" v-if="homeScore1.lastDecScore==1&&getMonth==0">已兑换</i-button>
            <i-button type="info" v-if="homeScore1.lastDecScore==1&&getMonth-1==0" :disabled="true">已兑换</i-button>
          </FormItem>
        </Row>
        <Row :gutter="16" v-if="getMonth-1==1||getMonth+1==1||getMonth==1">
          <FormItem label="一月兑换积分" prop="jan">
            <i-input
              v-model="homeScose.fields.jan"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.janScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('janScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.janScore==0&&getMonth==1&&getDay>=1&&getDay<=exchange">兑换</i-button>
               <i-button type="info" v-if="homeScore1.janScore==0&&(getMonth-1==1||getMonth+1==1)" :disabled="true">未兑换</i-button>
              </Tooltip>
            </Poptip>
            <i-button type="info" v-if="homeScore1.janScore==1&&getMonth==1">已兑换</i-button>
            <i-button type="info" v-if="homeScore1.janScore==1&&(getMonth-1==1||getMonth+1==1)" :disabled="true">已兑换</i-button>
          </FormItem>
        </Row>
        <Row :gutter="16"  v-if="getMonth-1==2||getMonth+1==2||getMonth==2">
          <FormItem label="二月兑换积分" prop="feb">
            <i-input
              v-model="homeScose.fields.feb"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.febScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('febScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.febScore==0&&getMonth==2&&getDay>=1&&getDay<=exchange">兑换</i-button>
                <i-button type="info" v-if="homeScore1.febScore==0&&(getMonth-1==2||getMonth+1==2)" :disabled="true">未兑换</i-button>
              </Tooltip>
            </Poptip>
            <i-button type="info" v-if="homeScore1.febScore==1&&getMonth==2">已兑换</i-button>
            <i-button type="info" v-if="homeScore1.febScore==1&&(getMonth-1==2||getMonth+1==2)" :disabled="true">已兑换</i-button>
          </FormItem>
        </Row>
        <Row :gutter="16" v-if="getMonth-1==3||getMonth+1==3||getMonth==3">
          <FormItem label="三月兑换积分" prop="mar">
            <i-input
              v-model="homeScose.fields.mar"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.marScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('marScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.marScore==0&&getMonth==3&&getDay>=1&&getDay<=exchange">兑换</i-button>
                 <i-button type="info" v-if="homeScore1.marScore==0&&(getMonth-1==3||getMonth+1==3)" :disabled="true">未兑换</i-button>
              </Tooltip>
            </Poptip>
            <i-button type="info" v-if="homeScore1.marScore==1&&getMonth==3">已兑换</i-button>
             <i-button type="info" v-if="homeScore1.marScore==1&&(getMonth-1==3||getMonth+1==3)" :disabled="true">已兑换</i-button>
          </FormItem>
        </Row>
        <Row :gutter="16" v-if="getMonth-1==4||getMonth+1==4||getMonth==4">
          <FormItem label="四月兑换积分" prop="apr">
            <i-input
              v-model="homeScose.fields.apr"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.aprScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('aprScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.aprScore==0&&getMonth==4&&getDay>=1&&getDay<=exchange">兑换</i-button>
                <i-button type="info" v-if="homeScore1.aprScore==0&&(getMonth-1==4||getMonth+1==4)" :disabled="true">未兑换</i-button>
              </Tooltip>
            </Poptip>
            <i-button type="info" v-if="homeScore1.aprScore==1&&getMonth==4">已兑换</i-button>
            <i-button type="info" v-if="homeScore1.aprScore==1&&(getMonth-1==4||getMonth+1==4)" :disabled="true">已兑换</i-button>
          </FormItem>
        </Row>
        <Row :gutter="16" v-if="getMonth-1==5||getMonth+1==5||getMonth==5">
          <FormItem label="五月兑换积分" prop="may">
            <i-input
              v-model="homeScose.fields.may"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.mayScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('mayScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.mayScore==0&&getMonth==5&&getDay>=1&&getDay<=exchange">兑换</i-button>
                <i-button type="info" v-if="homeScore1.mayScore==0&&(getMonth-1==5||getMonth+1==5)" :disabled="true">未兑换</i-button>
              </Tooltip>
            </Poptip>
            <i-button type="info" v-if="homeScore1.mayScore==1&&getMonth==5">已兑换</i-button>
             <i-button type="info" v-if="homeScore1.mayScore==1&&(getMonth-1==5||getMonth+1==5)":disabled="true">已兑换</i-button>
          </FormItem>
        </Row>
        <Row :gutter="16" v-if="getMonth-1==6||getMonth+1==6||getMonth==6">
          <FormItem label="六月兑换积分" prop="jun">
            <i-input
              v-model="homeScose.fields.jun"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.junScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('junScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.junScore==0&&getMonth==6&&getDay>=1&&getDay<=exchange">兑换</i-button>
                <i-button type="info" v-if="homeScore1.junScore==0&&(getMonth-1==6||getMonth+1==6)":disabled="true">未兑换</i-button>
              </Tooltip>
            </Poptip>
            <i-button type="info" v-if="homeScore1.junScore==1&&getMonth==6">已兑换</i-button>
             <i-button type="info" v-if="homeScore1.junScore==1&&(getMonth-1==6||getMonth+1==6)":disabled="true">已兑换</i-button>
          </FormItem>
        </Row>
        <Row :gutter="16" v-if="getMonth-1==7||getMonth+1==7||getMonth==7">
          <FormItem label="七月兑换积分" prop="jul">
            <i-input
              v-model="homeScose.fields.jul"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.julScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('julScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.julScore==0&&getMonth==7&&getDay>=1&&getDay<=exchange">兑换</i-button>
                <i-button type="info" v-if="homeScore1.julScore==0&&(getMonth-1==7||getMonth+1==7)" :disabled="true">未兑换</i-button> 
              </Tooltip>
              </Tooltip>
            </Poptip>
             <i-button type="info" v-if="homeScore1.julScore==1&&(getMonth-1==7||getMonth+1==7)":disabled="true">已兑换</i-button>
            <i-button type="info" v-if="homeScore1.julScore==1&&getMonth==7">已兑换</i-button>
          </FormItem>
        </Row>
        <Row :gutter="16"  v-if="getMonth-1==8||getMonth+1==8||getMonth==8">
          <FormItem label="八月兑换积分" prop="aug">
            <i-input
              v-model="homeScose.fields.aug"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.augScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('augScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.augScore==0&&getMonth==8&&getDay>=1&&getDay<=exchange">兑换</i-button>
                <i-button type="info" v-if="homeScore1.augScore==0&&(getMonth-1==8||getMonth+1==8)" :disabled="true">未兑换</i-button> 
              </Tooltip>
            </Poptip>
            <i-button type="info" v-if="homeScore1.augScore==1&&(getMonth-1==8||getMonth+1==8)":disabled="true">已兑换</i-button>
            <i-button type="info" v-if="homeScore1.augScore==1&&getMonth==8">已兑换</i-button>
          </FormItem>
        </Row>
        <Row :gutter="16"  v-if="getMonth-1==9||getMonth+1==9||getMonth==9">
          <FormItem label="九月兑换积分" prop="sep">
            <i-input
              v-model="homeScose.fields.sep"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.sepScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('sepScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.sepScore==0&&getMonth==9&&getDay>=1&&getDay<=exchange">兑换</i-button>
                <i-button type="info" v-if="homeScore1.sepScore==0&&(getMonth-1==9||getMonth+1==9)":disabled="true">未兑换</i-button> 
              </Tooltip>
            </Poptip>
            <i-button type="info" v-if="homeScore1.sepScore==1&&getMonth==9">已兑换</i-button>
            <i-button type="info" v-if="homeScore1.sepScore==1&&(getMonth-1==9||getMonth+1==9)":disabled="true">已兑换</i-button>
          </FormItem>
        </Row>
        <Row :gutter="16"  v-if="getMonth-1==10||getMonth+1==10||getMonth==10">
          <FormItem label="十月兑换积分" prop="oct">
            <i-input
              v-model="homeScose.fields.oct"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.octScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('octScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.octScore==0&&getMonth==10&&getDay>=1&&getDay<=exchange">兑换</i-button>
                <i-button type="info" v-if="homeScore1.octScore==0&&(getMonth-1==10||getMonth+1==10)":disabled="true">未兑换</i-button> 
              </Tooltip>
            </Poptip>
            <i-button type="info" v-if="homeScore1.octScore==1&&getMonth==10">已兑换</i-button>
            <i-button type="info" v-if="homeScore1.octScore==1&&(getMonth-1==10||getMonth+1==10)":disabled="true">已兑换</i-button>
          </FormItem>
        </Row>
        <Row :gutter="16"  v-if="getMonth-1==11||getMonth+1==11||getMonth==11">
          <FormItem label="十一月兑换积分" prop="nov">
            <i-input
              v-model="homeScose.fields.nov"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.novScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('novScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.novScore==0&&getMonth==11&&getDay>=1&&getDay<=exchange">兑换</i-button>
                 <i-button type="info" v-if="homeScore1.novScore==0&&(getMonth-1==11||getMonth+1==11)":disabled="true">未兑换</i-button> 
              </Tooltip>
            </Poptip>
            <i-button type="info" v-if="homeScore1.novScore==1&&getMonth==11">已兑换</i-button>
            <i-button type="info" v-if="homeScore1.novScore==1&&(getMonth-1==11||getMonth+1==11)":disabled="true">已兑换</i-button>
          </FormItem>
        </Row>
        <Row :gutter="16"  v-if="getMonth==12||getMonth+1==12">
          <FormItem label="十二月兑换积分" prop="dec">
            <i-input
              v-model="homeScose.fields.dec"
              :readonly="true"
              style="width:200px;"
              v-show="homeScore1.decScore==0"
            ></i-input>
            <Poptip confirm :transfer="true" title="确定要兑换吗?" @on-ok="doexchange('decScore')">
              <Tooltip placement="top" content="兑换" :delay="1000" :transfer="true">
                <i-button type="info" v-if="homeScore1.decScore==0&&getMonth==12&&getDay>=1&&getDay<=exchange">兑换</i-button>
                <i-button type="info" v-if="homeScore1.decScore==0&&getMonth+1==12":disabled="true">未兑换</i-button> 
              </Tooltip>
            </Poptip>
            <i-button type="info" v-if="homeScore1.decScore==1&&getMonth==12">已兑换</i-button>
            <i-button type="info" v-if="homeScore1.decScore==1&&getMonth+1==12":disabled="true">已兑换</i-button>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="homeScose.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer title="垃圾投放详情" v-model="stores2.opened" width="80%" :mask-closable="false" :mask="true">
      <Card>
      <dz-table
        :totalCount="stores2.own.query.totalCount"
        :pageSize="stores2.own.query.pageSize"
        @on-page-change="handlePageChanged1"
        @on-page-size-change="handlePageSizeChanged1"
      >
      <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Date-picker type="year" placeholder="选择年" style="width: 200px" v-model="stores2.own.query.kw1" format="yyyy" @on-change="getyear()"></Date-picker>
                  </FormItem>
                  <FormItem>
                    <Date-picker type="month" placeholder="选择月" style="width: 200px" v-model="stores2.own.query.kw2" format="yyyy-MM" @on-change="getmouth()"></Date-picker>
                  </FormItem>
                  <FormItem>
                    {{tfscore}}：{{gettfscore}}
                  </FormItem>
                </Form>
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
          :data="stores2.own.data"
          :columns="stores2.own.columns"
          :row-class-name="rowClsRender"
        >
          <!--          <template slot-scope="{ row, index }" slot="action1">-->
          <!--            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">-->
          <!--              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">-->
          <!--                <Button type="error" size="small" shape="circle" icon="md-trash"></Button>-->
          <!--              </Tooltip>-->
          <!--            </Poptip>-->
          <!--          </template>-->
        </Table>
      </dz-table>
      </Card>
      <!-- <div class="demo-drawer-footer">
        <Button style="margin-left: 30px" icon="md-close" @click="stores2.opened = false">关 闭</Button>
      </div> -->
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getOwnList,
  deleteGrabDisRecord, //右边删除
  batchCommand, //右边上方删除
  loadCarDriver, //加载
  getScoreData, //加载分数
  GetRecord, //详情
  globalvalidatePhone,
  GetHomeScore,
  GetEditHomeScore,
} from "@/api/score/own";
import { getshequ } from "@/api/base/house";
export default {
  name: "house",
  components: {
    DzTable,
  },
  data() {
    let globalvalidatePhone = (rule, value, callback) => {
      if (value !== "" && value !== null) {
        callback();
      } else {
        callback(new Error("联系方式号码不能为空"));
      }
      callback();
    };
    return {
      rowid: "",
      month:'',
      formModel: {
        opened: false,
        mode: "create",
        selection: [],
        fields: {
          loginName: "",
          phone: "",
          isDelete: 0,
          we_Chat: "",
          addTime: "",
        },
        rules: {
          loginName: [
            { type: "string", required: true, message: "请输入昵称" },
          ],
          phone: [
            { type: "string", required: true, validator: globalvalidatePhone },
          ],
        },
      },
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
      },
      list33: [],
      stores: {
        car: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            kw2: "",
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
          },
          columns: [
            { type: "selection", width: 50, key: "systemUserUuid" },
            // { title: "昵称", key: "loginName", sortable: true },
            // { title: "手机号", key: "phone" },
            {
              title: "家庭住址",
              key: "address",
              minWidth: 400,
              sortable: true,
            },
            { title: "本月积分", key: "newScore" },
            { title: "上月积分", key: "lastScore" },
            { title: "上月兑换状态", key: "state",slot:'State' },
            { title: "累计积分", key: "all" },
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
      },
      stores2: {
        opened: false,
        own: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            kw2: "",
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {},
          columns: [
            { type: "selection", width: 50, key: "garbageDisposalUuid" },
            { title: "昵称", key: "loginName", sortable: true },
            { title: "手机号", key: "phone" },
            { title: "垃圾投放时间", key: "addTime", sortable: true },
            { title: "垃圾箱房", key: "grID" },
            // { title: "投放垃圾类型", key: "grtype" },
            { title: "赋分类型", key: "markType" },
            { title: "本次积分", key: "integral" },
          ],
          data: [],
        },
      },
      homeScose: {
        opened: false,
        mode: "create",
        selection: [],
        fields: {
          homeAddressUuid: "",
          jifen:0,
        },
        rules: {},
      },
      homeScore1: {
        janScore: 0,
        febScore: 0,
        marScore: 0,
        aprScore: 0,
        mayScore: 0,
        junScore: 0,
        julScore: 0,
        augScore: 0,
        sepScore: 0,
        octScore: 0,
        novScore: 0,
        decScore: 0,
        lastDecScore:0,
      },
      disNum: 0,
      exchange:1,
      getMonth:0,
      getDay:0,
      tfscore:'今年投放积分',
      gettfscore:0
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建调度申请";
      }
      if (this.formModel.mode === "edit") {
        return "编辑调度信息";
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
    //获取信息数据
    loadCarDispatchList() {
      getOwnList(this.stores.car.query).then((res) => {
        this.stores.car.data = res.data.data;
        this.stores.car.query.totalCount = res.data.totalCount;
        //console.log(this.stores.car.data);
      });
    },
    //翻页
    handlePageChanged(page) {
      this.stores.car.query.currentPage = page;
      this.loadCarDispatchList();
    },
    handlePageChanged1(page) {
      this.stores2.own.query.currentPage = page;
      this.doGetRecord();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.car.query.pageSize = pageSize;
      this.loadCarDispatchList();
    },
    handlePageSizeChanged1(pageSize) {
      this.stores2.own.query.pageSize = pageSize;
      this.doGetRecord();
    },
    
    renderScore(State){
      let data={
        color:'',
        text:'',
      }
      if(State=='未兑换'){
        data.color='#5cadff';
        data.text='未兑换';
      }
      if(State=='已兑换'){
        data.color='#19be6b';
        data.text='已兑换';
      }
      return data;
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
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatch"].resetFields();
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
      this.doDelete(row.garbageDisposalUuid);
    },
    //获取积分
    loadRecord(row) {
      this.tfscore='今年投放积分';
      this.gettfscore=0;
      this.stores2.own.query.kw1="";
      this.stores2.own.query.kw2="";
      this.rowid = row.homeAddressUuid;
      this.stores2.own.query.kw=row.homeAddressUuid;
      this.doGetRecord();
    },
    doGetRecord(){
      GetRecord(this.stores2.own.query).then((res) => {
        this.stores2.own.query.totalCount = res.data.totalCount;
        this.stores2.own.data = res.data.data;
        this.stores2.opened = true;
        this.gettfscore=res.data.totalCount;
      });
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteGrabDisRecord(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadCarDispatchList();
          this.formModel.selection = [];
          GetRecord(this.rowid).then((res) => {
            this.stores2.own.data = res.data.data;
          });
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
    //申请社区下拉框
    loadDepartmentList1() {
      let data = this.$store.state.user.villageGuid;
      getshequ(data).then((res) => {
        //console.log(res.data.data);
        this.list33 = res.data.data;
        let townn = Array.from(new Set(this.list33.map((x) => x.towns)));
        //console.log(townn);
        this.stores.car.sources.town = townn;
      });
    },
    xz(e) {
      let college = this.list33.filter((x) => x.towns == e);
      this.stores.car.sources.college = college.map((x) => x.vname);
      this.loadCarDispatchList();
    },
    sq(e) {
      if (e == "全部") {
        this.stores.car.query.ccmmunity = "";
      }
      this.loadCarDispatchList();
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
    handleSubmitConsumable() {
      console.log(this.homeScose.fields);
      GetEditHomeScore(this.homeScose.fields).then((res) => {
        if(res.data.code==200){
          this.$Message.success("兑换成功");
          this.loadCarDispatchList();
        }else{
          this.$Message.warning(res.data.message);
          return;
        }
      });
      this.homeScose.opened = false;
    },
    getyear(){
      this.tfscore=this.stores2.own.query.kw1.getFullYear()+'年积分';
      this.stores2.own.query.kw2='';
      this.doGetRecord();
    },
    getmouth(){
      var y=this.stores2.own.query.kw2.getFullYear();
      var m=this.stores2.own.query.kw2.getMonth() + 1;
      m = m < 10 ? '0' + m : m; 
      this.tfscore=y+'-'+m+'月积分';
      this.stores2.own.query.kw1='';
      this.doGetRecord();
    },
    doexchange(e) {
      if(e=='lastDecScore'){
        if(this.homeScose.fields.lastDec<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
        this.homeScose.fields.jifen=this.homeScose.fields.lastDec;
        this.homeScose.fields.lastDecScore=1;
        this.homeScore1.lastDecScore=1;
      }
      if(e=='janScore'){
        if(this.homeScose.fields.jan<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
        this.homeScose.fields.jifen=this.homeScose.fields.jan;
        this.homeScose.fields.janScore=1;
        this.homeScore1.janScore=1;
      }
      if(e=='febScore'){
        if(this.homeScose.fields.feb<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
        this.homeScose.fields.jifen=this.homeScose.fields.feb;
        this.homeScose.fields.febScore=1;
        this.homeScore1.febScore=1;
      }
      if(e=='marScore'){
        if(this.homeScose.fields.mar<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
        this.homeScose.fields.jifen=this.homeScose.fields.mar;
        this.homeScose.fields.marScore=1;
        this.homeScore1.marScore=1;
      }
      if(e=='aprScore'){
        if(this.homeScose.fields.apr<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
        this.homeScose.fields.jifen=this.homeScose.fields.apr;
        this.homeScose.fields.aprScore=1;
        this.homeScore1.aprScore=1;
      }
      if(e=='mayScore'){
        if(this.homeScose.fields.may<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
        this.homeScose.fields.jifen=this.homeScose.fields.may;
        this.homeScose.fields.mayScore=1;
        this.homeScore1.mayScore=1;
      }
      if(e=='junScore'){
        if(this.homeScose.fields.jun<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
        this.homeScose.fields.jifen=this.homeScose.fields.jun;
        this.homeScose.fields.junScore=1;
        this.homeScore1.junScore=1;
      }
      if(e=='julScore'){
        if(this.homeScose.fields.jul<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
        
        this.homeScose.fields.jifen=this.homeScose.fields.jul;
        this.homeScose.fields.julScore=1;
        this.homeScore1.julScore=1;
      }
      if(e=='augScore'){
        if(this.homeScose.fields.aug<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
        this.homeScose.fields.jifen=this.homeScose.fields.aug;
        this.homeScose.fields.augScore=1;
        this.homeScore1.augScore=1;
      }
      if(e=='sepScore'){
        if(this.homeScose.fields.sep<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
         this.month="sep";
         console.log(this.homeScose.fields.sep);
        this.homeScose.fields.jifen=this.homeScose.fields.sep;
        this.homeScose.fields.sepScore=1;
        this.homeScore1.sepScore=1;
      }
      if(e=='octScore'){
        if(this.homeScose.fields.oct<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
        this.homeScose.fields.jifen=this.homeScose.fields.oct;
        this.homeScose.fields.octScore=1;
        this.homeScore1.octScore=1;
      }
      if(e=='novScore'){
        if(this.homeScose.fields.nov<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
        this.homeScose.fields.jifen=this.homeScose.fields.nov;
        this.homeScose.fields.novScore=1;
        this.homeScore1.novScore=1;
      }
      if(e=='decScore'){
        if(this.homeScose.fields.dec<this.disNum){
          this.$Message.warning("暂未满足兑换标准");
          return;
        }
        this.homeScose.fields.jifen=this.homeScose.fields.dec;
        this.homeScose.fields.decScore=1;
        this.homeScore1.decScore=1;
      }
    },
    handleEdit(e) {
      this.homeScose.opened = true;
      GetHomeScore(e.homeAddressUuid).then((res) => {
        console.log(res);
        if (res.data.code == 200) {
          this.homeScose.fields = res.data.data.query;
          this.homeScore1.janScore = res.data.data.query.janScore;
          this.homeScore1.febScore = res.data.data.query.febScore;
          this.homeScore1.marScore = res.data.data.query.marScore;
          this.homeScore1.aprScore = res.data.data.query.aprScore;
          this.homeScore1.mayScore = res.data.data.query.mayScore;
          this.homeScore1.junScore = res.data.data.query.junScore;
          this.homeScore1.julScore = res.data.data.query.julScore;
          this.homeScore1.augScore = res.data.data.query.augScore;
          this.homeScore1.sepScore = res.data.data.query.sepScore;
          this.homeScore1.octScore = res.data.data.query.octScore;
          this.homeScore1.novScore = res.data.data.query.novScore;
          this.homeScore1.decScore = res.data.data.query.decScore;
          this.homeScose.fields.homeAddressUuid = e.homeAddressUuid;
          this.homeScose.fields.jifen=e.jifen;
          this.exchange=res.data.data.exchange;
          this.disNum=res.data.data.disNum;
          var date=new Date();
          this.getMonth=date.getMonth();
          this.getDay=date.getDate();
        }
      });
    },
  },
  mounted() {
    this.loadDepartmentList1();
    this.loadCarDispatchList();
  },
};
</script>


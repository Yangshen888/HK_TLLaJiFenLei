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
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.grab.query.kw"
                      placeholder="请输入车牌号码"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                  <FormItem>
                    <DatePicker
                      type="daterange"
                      v-model="stores.grab.query.kw2"
                      format="yyyy/MM/dd"
                      placement="top"
                      placeholder="输入时间搜索..."
                      style="width: 200px"
                      :confirm="true"
                      :editable="false"
                      @on-ok="handleSearchDispatch()"
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
          
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          
        >
          <template slot-scope="{row, index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>
          <template slot-scope="{ row, index }" slot="action">
              
          </template>
        </Table>
      </dz-table>
    </Card>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getList,//显示列表
} from "@/api/grab/weigh";
export default {
  name: "weigh",
  components: {
    DzTable
  },
  data() {
    return {
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" }
      },
      //查询搜索
      stores: {
        grab: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw2: "",
            vuuid:this.$store.state.user.villageGuid,
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
            { type: "selection", width: 50, key: "grabageWeighingRecordUUID" },
            { title: "车牌号码", key: "carNumber", sortable: true },
            { title: "收运类型", key: "grabType" },
            { title: "厢房编号", key: "wingID" },
            { title: "所属公司", key: "company" },
            // { title: "车辆类型", key: "carType" },
            { title: "垃圾重量/kg", key: "weight" },
            { title: "称重时间", key: "weightTime" },
            
          ],
          data: []
        }
      }
    };
  },
  computed: {},
  methods: {
    //获取信息数据
    loadCarDispatchList() {
      getList(this.stores.grab.query).then(res => {
        ////console.log(res.data.data)
        this.stores.grab.data = res.data.data;
        //console.log(res.data);
        this.stores.grab.query.totalCount = res.data.totalCount;
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
    }
  },
  mounted() {
    this.loadCarDispatchList();
  }
};
</script>
<style>
</style>
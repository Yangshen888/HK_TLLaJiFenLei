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
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.grab.query.kw1"
                      placeholder="请输入垃圾类型"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem> 
                  <FormItem>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.grab.query.kw3"
                      placeholder="请输入街道"
                      @on-search="handleSearchDispatch()"
                    ></Input>
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
} from "@/api/grab/carweight";
export default {
  name: "carweight",
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
            kw1: "",
            kw3: "",
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
            { type: "selection", width: 50, key: "carUuid" },
            { title: "车牌号码", key: "carNumber", sortable: true },
            { title: "所属街道", key: "street" },
            { title: "垃圾类型", key: "grabType" },
            { title: "垃圾重量/kg", key: "weight" },
            { title: "时间", key: "wtime" },
            
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
<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">赋分记录</block>
		</cu-custom>

		<form>
			<view class="cu-form-group pa">
				<view class="title">请选择年月日</view>
				<picker mode="date" :value="query.startdate" @change="StartDateChange">
					<view class="picker">{{ query.startdate }}</view>
				</picker>
			</view>

			<view class="pa">
				<t-table @change="change" style="background:#fff;">
					<t-tr >
						<t-th style="color:rgb(46, 100, 233);">序号</t-th>
						<t-th style="color:rgb(46, 100, 233);">用户</t-th>
						<t-th style="color:rgb(46, 100, 233);">获得积分</t-th>
						<t-th style="color:rgb(46, 100, 233);">赋分时间</t-th>
						<t-th style="color:rgb(46, 100, 233);">垃圾厢房编号</t-th>
						<t-th style="color:rgb(46, 100, 233);">操作人</t-th>
					</t-tr>
					<t-tr v-for="(item,index) in tableList" :key="index">
						<t-td>{{ index + 1 }}</t-td>
						<t-td>{{ item.realName }}</t-td>
						<t-td>{{ item.score }}</t-td>
						<t-td>{{ item.addTime }}</t-td>
						<t-td>{{ item.grabageRoomNum }}</t-td>
						<t-td>{{ item.addPeople }}</t-td>
					</t-tr>
				</t-table>
			</view>
		</form>
	</view>
</template>

<script>
import { formateDate } from '@/global/utils.js';

import tTable from '@/components/t-table/t-table.vue';
import tTh from '@/components/t-table/t-th.vue';
import tTr from '@/components/t-table/t-tr.vue';
import tTd from '@/components/t-table/t-td.vue';
import {dogetscorelist} from '@/api/supervisor/manager.js'

export default {
	components: {
		tTable,
		tTh,
		tTr,
		tTd
	},
	data: () => {
		let today = formateDate(new Date(), 'Y-M-D');
		return {
			tableList: [],
			query: {
				totalCount: 0,
				pageSize: 10,
				currentPage: 1,
				username: '',
				startdate: today,
				materialTypeUuid: '',
				auditstate: -1,
				sort: [
					{
						direct: 'DESC',
						field: 'ID'
					}
				]
			},
			allCheck: false
		};
	},
	methods: {
		change(e) {
			console.log(e.detail);
		},
		edit(item) {
			uni.showToast({
				title: item.name,
				icon: 'none'
			});
		},

		StartDateChange(e) {
			this.query.startdate = e.detail.value;
			this.getdatalist();
			
		},
		getdatalist(){
			var parms={uuid:this.$store.state.userId,addtime:this.query.startdate};
			console.log(parms);
			dogetscorelist(parms).then(res=>{
				if(res.data.code==200){
					this.tableList=res.data.data;
					console.log(this.tableList);
				}
			})
		}
	},
	created() {
		let today = formateDate(new Date(), 'Y-M-D');
		this.query.startdate=today;
		this.getdatalist();
	},
	mounted() {},
	computed: {}
};
</script>

<style>
.w_table {
	width: 100%;
	position: relative;
	display: inline-block;
	height: 100%;
	min-height: 130upx;
	min-width: 100%;
	background: #fff;
	border: 2upx solid #ccc;
	font-size: 14upx;
	box-sizing: border-box;
	overflow: scroll;
}

.w_table_swper {
	height: 100%;
	box-sizing: border-box;
}

.w_table_content {
	height: 100%;
	width: 100%;
}

.w-table_title {
	width: fit-content;
	height: 60upx;
	display: flex;
	justify-content: space-between;
	line-height: 60upx;
	position: sticky;
	top: 0;
	left: 0;
	z-index: 1;
}

.w-table_title_item {
	display: inline-block;
}

.w-table_title_item_border {
	border-right: 2upx solid #fff;
}

.w_table_title_content {
	padding: 0 10upx;
}

.w_table_body {
	display: flex;
	justify-content: space-between;
}

.w_table_body_col {
	height: 60upx;
	line-height: 60upx;
	box-sizing: border-box;
	width: 100%;
	border-bottom: 2upx solid #f8f8f8;
	margin: 0 auto;
}

.w_table_body_col > .col_cell {
	display: inline-block;
	width: 100%;
	overflow: hidden;
	text-overflow: ellipsis;
	white-space: nowrap;
}

.col_cell_border {
	border-right: 2upx solid #f8f8f8;
}

.col_cell_last {
	border: none;
}

.col_text {
	display: inline-block;
	/* padding: 0 10upx; */
	margin: 0 auto;
}

.w_table_null {
	width: 100%;
	height: 60upx;
	position: absolute;
	top: 60upx;
	line-height: 60upx;
	text-align: center;
}
.pa {
	margin: 20px 20px 0px 20px;
	border-radius: 5px;
}
</style>

<template>
	<view style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">兑换记录</block>
		</cu-custom>
		<view>
			<view>
				<image src='../../static/SH.png' style="width: 100%;height:160px;"></image>
			</view>
			<view class="jf_title">
				<view>积分
				<span style="color:rgb(0, 190, 110);font-size: 24px;padding-left:5px;">
					{{sumscore}}
				</span>
				</view>
				
			</view>
			<view class="cu-form-group" style="border-radius: 5px;margin:-30px 20px 0px;">
				<view>请选择开始时间</view>
				<picker mode="date" :value="time1" @change="StartDateChange1">
					<view class="picker">{{ time1 }}</view>
				</picker>
			</view>
			
			<!-- 导航栏 agents导航栏标题 -->
			<navTab ref="navTab" :tabTitle="tabTitle" @changeTab='changeTab'></navTab>
			<!-- swiper切换 swiper-item表示一页 scroll-view表示滚动视窗 -->
			<swiper style="min-height: 45vh;" :current="currentTab" @change="swiperTab">
				<swiper-item>
					<scroll-view style="height: 85%;" scroll-y="true"  scroll-with-animation :scroll-into-view="toView">
						<view class="pa">
							<t-table @change="change" style="background:#fff;">
								<t-tr >
									<t-th style="color:rgb(0, 190, 110);width: 53%;">家庭地址</t-th>
									<t-th style="color:rgb(0, 190, 110);width: 16%;">积分</t-th>
									<t-th style="color:rgb(0, 190, 110);width: 31%;">时间</t-th>
								</t-tr>
								<t-tr v-for="(item,index) in tableList" :key="index">
									<t-td style="width:73%;">{{ item.address }}</t-td>
									<t-td style="width:22%;">{{ item.deduceScore }}</t-td>
									<t-td style="width:43%;">{{ item.exchageTime }}</t-td>
								</t-tr>
							</t-table>
						</view>
						<!-- <view style="width: 100%;height: 100upx;opacity:0;">底部占位盒子</view> -->
					</scroll-view>
				</swiper-item>
				<swiper-item>
					<scroll-view style="height: 85%;" scroll-y="true"  scroll-with-animation :scroll-into-view="toView">
						<view class="pa">
							<t-table @change="change" style="background:#fff;">
								<t-tr >
									<t-th style="color:rgb(0, 190, 110);width: 50%;">扣除积分</t-th>
									<t-th style="color:rgb(0, 190, 110);width: 50%;">扣除时间</t-th>
								</t-tr>
								<t-tr v-for="(item,index) in tableList1" :key="index">
									<t-td style="width: 50%;">{{ item.deduceScore }}</t-td>
									<t-td style="width: 50%;">{{ item.exchageTime }}</t-td>
								</t-tr>
							</t-table>
						</view>
					</scroll-view>
				</swiper-item>
			</swiper>
		</view>
	</view>
</template>

<script>
const util = require('../../global/navtab.js');
import navTab from '@/components/nav-tab/navTab.vue';
	import { formateDate } from '@/global/utils.js';
	
	import tTable from '@/components/t-table/t-table.vue';
	import tTh from '@/components/t-table/t-th.vue';
	import tTr from '@/components/t-table/t-tr.vue';
	import tTd from '@/components/t-table/t-td.vue';
	import { ShopaddRecordList } from '@/api/supervisor/manager.js';
	export default {
		components: {
			tTable,
			tTh,
			tTr,
			tTd,
			navTab
		},
		data: () => {
			let today = formateDate(new Date(), 'Y-M-D');
			return {
				tabTitle:['收入记录','支出记录'], //导航栏格式 --导航栏组件
				tableList: [],
				tableList1: [],
				currentTab: 0, //sweiper所在页
				query: {
					totalCount: [],
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
				allCheck: false,
				sumscore:0,
				time1:formateDate(new Date(),'Y-M-D'),
				time2:formateDate(new Date(),'Y-M-D')
			};
		},
		methods: {
			edit(item) {
				uni.showToast({
					title: item.name,
					icon: 'none'
				});
			},
		changeTab(index){
			this.currentTab = index;
		},
		// swiper 滑动
		swiperTab: function(e) {
			var index = e.detail.current //获取索引
			this.$refs.navTab.longClick(index);
		},
			StartDateChange2(e) {
				if(this.time2<this.time1)
				{
					uni.showModal({
						title: '提示',
						content: '结束时间不能小于开始时间',
						showCancel: false
					})
					return;
				}
				this.time2 = e.detail.value;
				this.getdatalist();
			},
			StartDateChange1(e) {
				this.time1 = e.detail.value;
				this.getdatalist();
			},
			getdatalist(){
				let data={
					guid:this.$store.state.HomeAddressUUID,
					suid:this.$store.state.shopid,
					time:this.time1
				};
				console.log(data);
				
				ShopaddRecordList(data).then(res=>{
					console.log(res);
					if(res.data.code==200){
						this.sumscore=res.data.data.total;
						this.tableList=res.data.data.list;
						this.tableList1=res.data.data.list2;
						for(let i=0;i<res.data.data.list.length;i++){
							let add=this.tableList[i].address.split('区')[1];
							if(add!=undefined){
								this.tableList[i].address=add;
							}
						};
					}
				})
			}
			
		},
		created() {
			let today = formateDate(new Date(), 'Y-M-D');
			this.query.startdate=today;
			this.getdatalist();
			 //console.log(this.$store.state.userId);
		},
	}
</script>

<style>
#canuse{
	height:50px;
	line-height:50px;
	background-color: white;
	border-radius: 3px;
	width:70%;
	margin:0px auto;
}
.navTabBox .longTab .longItem{
	color: #000000;
}
.underline{
	background: linear-gradient(-90deg, rgba(63, 205, 235, 1), rgba(188, 226, 158, 1));
}
.can1{
	font-size:30upx;
	font-weight: 400;
	text-align:left;
	float:left;
	margin-left:20upx ;
	width:5rem;
}
.can2{
	font-size:35px;
	font-weight: 500;
	float:left;
	margin-left:2rem;
	color:rgb(0, 190, 110);
	text-align: right;
}
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
	margin: 5px 20px 0px 20px;
	border-radius: 5px;
}
.title{
	font-size:35upx;
	font-family: 微软雅黑;
	color:rgb(42, 42, 42);
	margin:20px 0px 5px 20px;
	display: inline-block;
}
.jf_title{
	background:#fff;
	border-radius: 5px;
	width:70%;
	margin:0 auto;
	height:70px;
	font-size: 16px;
	    text-align: center;
	    font-weight: bold;
		line-height: 70px;
		position: relative;
		    top: -40px;
}
</style>

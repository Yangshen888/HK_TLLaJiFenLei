<template>
	<view style="padding-top:2rem">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">积分查询</block>
		</cu-custom>
		<view>
			<view id="canuse" style="margin-top: 15px;">
				<view class="can1">用户积分</view>
				<view class="can2">{{ tableList[3] }}</view>
			</view>
			<view id="canuse">
				<view class="can1">志愿者积分</view>
				<view class="can2">{{ tableList[4] }}</view>
			</view>
			<view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 20px;">
				<view>请选择开始时间</view>
				<picker mode="date" :value="time1" @change="StartDateChange1">
					<view class="picker">{{ time1 }}</view>
				</picker>
			</view>
			<view class="cu-form-group " style="border-radius: 5px;margin:20px 20px 20px;">
				<view>查询记录</view>
				<picker @change="PickerChange" :value="index" :range-key="'name'" :range="jilu">
					<view class="picker">{{ index > -1 ? jilu[index].name : jilu[0].name }}</view>
				</picker>
			</view>
			<!-- <text class='title'>用户积分记录</text> -->
			<view class="pa" v-show="index == 0">
				<t-table @change="change" style="background:#fff;">
					<t-tr>
						<t-th style="color:rgb(0, 190, 110);width: 30%;">分类情况</t-th>
						<t-th style="color:rgb(0, 190, 110);width: 21%;">积分</t-th>
						<t-th style="color:rgb(0, 190, 110);width: 26%;">渠道</t-th>
						<t-th style="color:rgb(0, 190, 110);width: 35%;">时间</t-th>
					</t-tr>
					<t-tr v-for="(item, index) in tableList[0]" :key="index">
						<t-td style="width: 30%;">{{ item.scoreName }}</t-td>
						<t-td style="width: 21%;">{{ item.integral }}</t-td>
						<t-td style="width: 26%;">{{ item.markType }}</t-td>
						<t-td style="width: 35%;">{{ item.addTime }}</t-td>
					</t-tr>
					
				</t-table>
			</view>
			<!-- <text class='title'>志愿者积分记录</text> -->
			<view class="pa" v-show="index == 1">
				<t-table @change="change" style="background:#fff;">
					<t-tr>
						<t-th style="color:rgb(0, 190, 110);width: 50%;">积分</t-th>
						<t-th style="color:rgb(0, 190, 110);width: 50%;">时间</t-th>
					</t-tr>
					<t-tr v-for="(item, index) in tableList[1]" :key="index">
						<t-td style="width: 50%;">{{ item.score }}</t-td>
						<t-td style="width: 50%;">{{ item.time }}</t-td>
					</t-tr>
				</t-table>
			</view>
			<!-- <text class='title'>兑换记录</text> -->
			<view class="pa" v-show="index == 2">
				<t-table @change="change" style="background:#fff;">
					<t-tr>
						<!-- <t-th style="color:rgb(0, 190, 110);width: 35%;">兑换商店</t-th> -->
						<!-- <t-th style="color:rgb(0, 190, 110);">兑换物品</t-th> -->
						<!-- <t-th style="color:rgb(0, 190, 110);width: 30%;">扣除积分</t-th> -->
						<!-- <t-th style="color:rgb(0, 190, 110);width: 35%;">时间</t-th> -->
						<t-th style="color:rgb(0, 190, 110);width: 40%;">月份</t-th>
						<t-th style="color:rgb(0, 190, 110);width: 60%;">兑换状态</t-th>
					</t-tr>
					<t-tr v-for="(item, index) in dateScore" :key="index">
						<!-- <t-td style="width: 35%;">{{ item.shop }}</t-td> -->
						<!-- <t-td>{{ item.goods }}</t-td> -->
						<!-- <t-td style="width: 30%;">{{ item.deduceScore }}</t-td> -->
						<!-- <t-td style="width: 35%;">{{ item.exchageTime }}</t-td> -->
						<t-td style="width: 40%;">{{ item.mouth }}</t-td>
						<t-td style="width: 60%;">{{ item.score }}</t-td>
					</t-tr>
				</t-table>
			</view>
		</view>
	</view>
</template>

<script>
import { formateDate } from '@/global/utils.js';

import tTable from '@/components/t-table/t-table.vue';
import tTh from '@/components/t-table/t-th.vue';
import tTr from '@/components/t-table/t-tr.vue';
import tTd from '@/components/t-table/t-td.vue';
import { getScoreSigle } from '@/api/user/userinfo.js';
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
			tableList: [[], []],
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
			index: 0,
			jilu: [
				{
					name: '用户积分记录',
					index: 0
				},
				{
					name: '志愿者积分记录',
					index: 1
				},
				{
					name: '兑换记录',
					index: 2
				}
			],
			allCheck: false,
			time1: formateDate(new Date(), 'Y-M-D'),
			time2: formateDate(new Date(), 'Y-M-D'),
			dateScore: [
				{
					mouth: '一月',
					score: '未兑换'
				},
				{
					mouth: '二月',
					score: '未兑换'
				},
				{
					mouth: '三月',
					score: '未兑换'
				},
				{
					mouth: '四月',
					score: '未兑换'
				},
				{
					mouth: '五月',
					score: '未兑换'
				},
				{
					mouth: '六月',
					score: '未兑换'
				},
				{
					mouth: '七月',
					score: '未兑换'
				},
				{
					mouth: '八月',
					score: '未兑换'
				},
				{
					mouth: '九月',
					score: '未兑换'
				},
				{
					mouth: '十月',
					score: '未兑换'
				},
				{
					mouth: '十一月',
					score: '未兑换'
				},
				{
					mouth: '十二月',
					score: '未兑换'
				}
			]
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

		StartDateChange2(e) {
			if (this.time2 < this.time1) {
				uni.showModal({
					title: '提示',
					content: '结束时间不能小于开始时间',
					showCancel: false
				});
				return;
			}
			this.time2 = e.detail.value;
			this.getdatalist();
		},
		StartDateChange1(e) {
			this.time1 = e.detail.value;
			this.getdatalist();
		},
		PickerChange(e) {
			this.index = e.detail.value;
		},
		getdatalist() {
			getScoreSigle({ uid: this.$store.state.userId, hid: this.$store.state.HomeAddressUUID, time1: this.time1, time2: this.time2 }).then(res => {
				if (res.data.code == 200) {
					this.tableList = res.data.data;
					this.dateScore = [
						{ mouth: '一月', score: '未兑换' },
						{ mouth: '二月', score: '未兑换' },
						{ mouth: '三月', score: '未兑换' },
						{ mouth: '四月', score: '未兑换' },
						{
							mouth: '五月',
							score: '未兑换'
						},
						{
							mouth: '六月',
							score: '未兑换'
						},
						{
							mouth: '七月',
							score: '未兑换'
						},
						{
							mouth: '八月',
							score: '未兑换'
						},
						{
							mouth: '九月',
							score: '未兑换'
						},
						{
							mouth: '十月',
							score: '未兑换'
						},
						{
							mouth: '十一月',
							score: '未兑换'
						},
						{
							mouth: '十二月',
							score: '未兑换'
						}
					];
					if (this.tableList[2].length > 0) {
						this.dateScore[0].score = this.tableList[2][0].jan == 0 ? '未兑换' : '已兑换';
						this.dateScore[1].score = this.tableList[2][0].feb == 0 ? '未兑换' : '已兑换';
						this.dateScore[2].score = this.tableList[2][0].mar == 0 ? '未兑换' : '已兑换';
						this.dateScore[3].score = this.tableList[2][0].apr == 0 ? '未兑换' : '已兑换';
						this.dateScore[4].score = this.tableList[2][0].may == 0 ? '未兑换' : '已兑换';
						this.dateScore[5].score = this.tableList[2][0].jun == 0 ? '未兑换' : '已兑换';
						this.dateScore[6].score = this.tableList[2][0].jul == 0 ? '未兑换' : '已兑换';
						this.dateScore[7].score = this.tableList[2][0].aug == 0 ? '未兑换' : '已兑换';
						this.dateScore[8].score = this.tableList[2][0].sep == 0 ? '未兑换' : '已兑换';
						this.dateScore[9].score = this.tableList[2][0].oct == 0 ? '未兑换' : '已兑换';
						this.dateScore[10].score = this.tableList[2][0].nov == 0 ? '未兑换' : '已兑换';
						this.dateScore[11].score = this.tableList[2][0].dec == 0 ? '未兑换' : '已兑换';
					}
					this.dateScore.splice(0, this.time1.split('-')[1] - 1);
				}
			});
		}
	},
	created() {
		if (this.$store.state.userId == '') {
			uni.showModal({
				title: '提示',
				content: '请在微信授权后查看！',
				showCancel: true,
				success: function(rest) {
					if (rest.confirm) {
						uni.navigateTo({
							url: '/pages/login/login'
						});
					} else {
						uni.navigateBack({
							delta: 1,
							animationDuration: 200
						});
					}
				}
			});
		} else {
			if (this.$store.state.HomeAddressUUID == null) {
				uni.showModal({
					title: '提示',
					content: '请先绑定家庭码！',
					showCancel: false,
					success: function(rest) {
						if (rest.confirm) {
							uni.navigateBack({
								delta: 1,
								animationDuration: 200
							});
						}
					}
				});
			} else {
				let today = formateDate(new Date(), 'Y-M-D');
				this.query.startdate = today;
				this.getdatalist();
				console.log(this.$store.state.userId);
			}
		}
	}
};
</script>

<style>
#canuse {
	height: 50px;
	line-height: 50px;
	background-color: white;
	border-radius: 3px;
	width: 70%;
	margin: 0px auto;
}
.can1 {
	font-size: 30upx;
	font-weight: 400;
	text-align: left;
	float: left;
	margin-left: 20upx;
	width: 5rem;
}
.can2 {
	font-size: 35px;
	font-weight: 500;
	float: left;
	margin-left: 2rem;
	color: rgb(0, 190, 110);
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
	margin: 0px 20px 0px 20px;
	border-radius: 5px;
}
.title {
	font-size: 35upx;
	font-family: 微软雅黑;
	color: rgb(42, 42, 42);
	margin: 20px 0px 5px 20px;
	display: inline-block;
}
</style>

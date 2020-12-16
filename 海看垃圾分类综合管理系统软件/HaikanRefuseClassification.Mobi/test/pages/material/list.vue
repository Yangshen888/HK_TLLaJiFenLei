<template>
	<view style="padding-top: 1.4rem;">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/home">
			<block slot="backText">返回</block>
			<block slot="content">耗材申请列表</block>
		</cu-custom>
		<form>
			<view class="cu-form-group margin-top">
				<view class="title">申请人</view>
				<input placeholder="请输入申请人查询" name="input" focus="true" v-model="query.username"></input>
			</view>
			<view class="cu-form-group">
				<view class="title">申请开始时间</view>
				<picker mode="date" :value="query.startdate" @change="StartDateChange">
					<view class="picker">
						{{query.startdate}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title">申请结束时间</view>
				<picker mode="date" :value="query.enddate" @change="EndDateChange">
					<view class="picker">
						{{query.enddate}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title">材料类型</view>
				<picker @change="MaterialPickerChange" :value="materialtype.index" :range="materialtype.picker" range-key="materialTypeName">
					<view class="picker">
						{{materialtype.picker[materialtype.index].materialTypeName}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title">审核状态</view>
				<picker @change="AuditPickerChange" :value="auditstate.index" :range="auditstate.picker" range-key="name">
					<view class="picker">
						{{auditstate.picker[auditstate.index].name}}
					</view>
				</picker>
			</view>
		</form>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitsearch(1)">搜索</button>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="create">新增耗材申请</button>
		<view class="bj-1" v-for="(item,index) in data" :key="index">
			<view class="ul">
				<view class="li">
					<view class="li-1">申请人:</view>
					<view class="li-2">{{item.realName}}</view>
				</view>
				<view class="li">
					<view class="li-1">申请时间:</view>
					<view class="li-2">{{item.applyTime}}</view>
				</view>
				<view class="li">
					<view class="li-1">材料类型:</view>
					<view class="li-2">{{item.materialTypeName}}</view>
				</view>
				<!-- <view class="li">
					<view class="li-1">材料名称:</view>
					<view class="li-2">{{item.materialName}}</view>
				</view>
				<view class="li">
					<view class="li-1">数量:</view>
					<view class="li-2">{{item.num}}</view>
				</view>
				<view class="li">
					<view class="li-1">规格型号:</view>
					<view class="li-2">{{item.specification}}</view>
				</view> -->
				<view class="li">
					<view class="li-1">审核状态:</view>
					<view class="li-2">{{renderAuditState(item.auditState)}}</view>
				</view>
			</view>
			<view class="ul">
				<view class="li-3"><button class="li-4" @click="edit(item.consumableUuid)" v-if="editshow(item.auditState,item.systemUserUuid)">修改</button></view>
				<view class="li-3"><button class="li-4" @click="audit(item.consumableUuid)" v-if="auditshow(item.auditState,item.materialTypeName)">审核</button></view>
				<view class="li-3"><button class="li-4" @click="deletemsg(item.consumableUuid)" v-if="item.auditState==0||item.auditState==3">删除</button></view>
				<view class="li-3"><button class="li-4" @click="detail(item.consumableUuid)">详情</button></view>
			</view>
		</view>
		<text class="loading-text">
			{{loadingType === 0 ? contentText.contentdown : (loadingType === 1 ? contentText.contentrefresh : contentText.contentnomore)}}</text>

	</view>
</template>

<script>
	import {
		formatDate
	} from '../../global/utils.js'
	import {
		getConsumableList,
		deleteConsumable,
		createEmptyConsumable,
		loadMaterialTypeList
	} from '../../api/material/consumable.js'
	export default {
		data() {
			let today = formatDate(new Date(), 'yyyy-MM-dd');
			let nextMonth = this.addDate(today,0);
			return {
				query: {
					totalCount: 0,
					pageSize: 10,
					currentPage: 1,
					username: '',
					startdate: today,
					enddate: nextMonth,
					materialTypeUuid: '',
					auditstate: -1,
					sort: [{
						direct: "DESC",
						field: "ID"
					}]
				},
				data: [],
				materialtype: {
					picker: [{
						materialTypeUuid: '',
						materialTypeName: '全部'
					}],
					index: 0,
				},
				auditstate: {
					picker: [{
							value: -1,
							name: '全部'
						},
						{
							value: 0,
							name: '保存中'
						},
						{
							value: 1,
							name: '科室耗材联络员待审核'
						},
						{
							value: 2,
							name: '终审待审核'
						},
						{
							value: 3,
							name: '科室耗材联络员审核未通过'
						},
						{
							value: 4,
							name: '终审通过'
						},
						{
							value: 5,
							name: '终审未通过'
						}
					],
					index: 0,
				},
				loadingType: 0,
				contentText: {
					contentdown: "上拉显示更多",
					contentrefresh: "正在加载...",
					contentnomore: "没有更多数据了"
				}

			}
		},
		onLoad() {
			this.$checkusertoken();
			this.doloadMaterialTypeList();
			this.submitsearch(0);
		},
		//上拉加载
		onReachBottom: function() {
			if (this.loadingType != 0) {
				return false;
			}
			this.loadingType = 1;
			uni.showNavigationBarLoading();
			this.query.currentPage++;
			getConsumableList(this.query).then(res => {
				if (res.data.code == 200) {
					if (res.data.totalCount == this.data.length) {
						this.loadingType = 2;
						uni.hideNavigationBarLoading();
						return false;
					}
					this.data = this.data.concat(res.data.data);
				} else {
					uni.showModal({
						title: '提示',
						content: res.data.message,
						showCancel: false
					})
				}
				this.loadingType = 0;
				uni.hideNavigationBarLoading();
			})
		},
		methods: {
			doloadMaterialTypeList() {
				loadMaterialTypeList().then(res => {
					
					this.materialtype.picker = this.materialtype.picker.concat(res.data.data);
					this.query.materialTypeUuid = this.materialtype.picker[this.materialtype.index].materialTypeUuid;
				})
			},
			editshow(state, guid) {
				if ((state == 0 || state == 3 || state == 5) && (this.$store.state.userId == guid)) {
					return true;
				} else
					return false;
			},
			auditshow(auditState, materialTypeName) {
				if (auditState == 1 && (this.$store.state.roleName == "科室耗材联络员" || this.$store.state.roleName == "科室负责人")) {
						return true;
				} else if (auditState == 2 && this.$store.state.roleName == "办公类耗材终审" && materialTypeName == "办公类耗材") {
					return true;
				} else if (auditState == 2 && this.$store.state.roleName == "计算机类耗材终审" && materialTypeName == "计算机类耗材") {
					return true;
				} else
					return false;
			},
			addDate(date, days) {
				var d = new Date(date);
				d.setDate(d.getDate() + days);
				var month = d.getMonth() + 2;
				var day = d.getDate();
				if (month < 10) {
					month = "0" + month;
				}
				if (day < 10) {
					day = "0" + day;
				}
				var val = d.getFullYear() + "-" + month + "-" + day;
				return val;
			},
			StartDateChange(e) {
				if (new Date(e.detail.value) < new Date(this.query.enddate)) {
					this.query.startdate = e.detail.value;
				} else {
					uni.showModal({
						title: '提示',
						content: '开始时间应小于结束时间',
						showCancel: false
					})
				}
			},
			EndDateChange(e) {
				if (new Date(e.detail.value) > new Date(this.query.startdate)) {
					this.query.enddate = e.detail.value
				} else {
					uni.showModal({
						title: '提示',
						content: '结束时间应大于开始时间',
						showCancel: false
					})
				}

			},
			MaterialPickerChange(e) {
				this.materialtype.index = e.detail.value;
				this.query.materialTypeUuid = this.materialtype.picker[this.materialtype.index].materialTypeUuid;
			},
			AuditPickerChange(e) {
				this.auditstate.index = e.detail.value
				this.query.auditstate = this.auditstate.picker[this.auditstate.index].value;
			},
			submitsearch(id) {
				if(id==0)
				this.query.startdate='';
				this.query.currentPage = 1;
				this.loadingType = 0,
					getConsumableList(this.query).then(res => {
						if (res.data.code == 200) {
							console.log(res.data.data)
							this.data = res.data.data;
							if(this.query.startdate=='')
								this.query.startdate=formatDate(new Date(), 'yyyy-MM-dd');
						} else {
							uni.showModal({
								title: '提示',
								content: res.data.message,
								showCancel: false
							})
						}
					})
			},
			// renderMaterialType(materialType) {
			// 	var materialTypeText = "未知";
			// 	switch (materialType) {
			// 		case 0:
			// 			materialTypeText = "办公室用品";
			// 			break;
			// 		case 1:
			// 			materialTypeText = "电子设备";
			// 			break;
			// 	}
			// 	return materialTypeText;
			// },
			renderAuditState(auditState) {
				var auditText = "未知";
				switch (auditState) {
					case 0:
						auditText = "保存中";
						break;
					case 1:
						auditText = "科室耗材联络员待审核";
						break;
					case 2:
						auditText = "终审待审核";
						break;
					case 3:
						auditText = "科室耗材联络员审核未通过";
						break;
					case 4:
						auditText = "终审通过";
						break;
					case 5:
						auditText = "终审未通过";
						break;
				}
				return auditText;
			},
			create() {
				createEmptyConsumable().then(res => {
					console.log(res)
					if (res.data.code == 200) {
						uni.navigateTo({
							url: '/pages/material/create?uuid=' + res.data.data
						})
					} else {
						uni.showModal({
							title: '提示',
							content: '创建失败请重新添加申请',
							showCancel: false
						})
					}
				})
			},
			edit(consumableUuid) {
				uni.navigateTo({
					url: '/pages/material/create?uuid=' + consumableUuid
				})
			},
			deletemsg(consumableUuid) {
				uni.showModal({
					title: '提示',
					content: '是否确认删除',
					success: (rd) => {
						if (rd.confirm) {
							deleteConsumable(consumableUuid).then(res => {
								uni.showModal({
									title: '提示',
									content: res.data.message,
									showCancel: false,
									success: (r) => {
										if (r.confirm) {
											this.submitsearch(1);
										}
									}
								})
							})
						}
					}
				})

			},
			detail(consumableUuid) {
				uni.navigateTo({
					url: '/pages/material/detail?uuid=' + consumableUuid
				})
			},
			audit(consumableUuid) {
				uni.navigateTo({
					url: '/pages/material/audit?uuid=' + consumableUuid
				})
			}
		}
	}
</script>

<style>
	.loading-text {
		height: 80upx;
		line-height: 80upx;
		font-size: 30upx;
		display: flex;
		flex-direction: row;
		justify-content: space-around;
	}

	.bj-1 {
		background-color: #fff;
		margin: 10px;
		width: 95%;
		overflow: hidden;
	}

	.bj-1 .ul {
		float: left;
		width: 100%;
	}

	.bj-1 .ul .li {
		float: left;
		width: 50%;
		padding: 15px 0 0 20px;
	}

	.bj-1 .ul .li .li-1 {
		float: left;
		width: 50%;
	}

	.bj-1 .ul .li .li-2 {
		float: left;
		width: 50%;
	}

	.bj-1 .ul .li-3 {
		float: left;
		width: 25%;
		margin: 20px 0;
	}

	.bj-1 .ul .li-4 {
		line-height: 1.5rem;
		font-size: 0.5rem;
		width: 3.5rem;
	}
</style>

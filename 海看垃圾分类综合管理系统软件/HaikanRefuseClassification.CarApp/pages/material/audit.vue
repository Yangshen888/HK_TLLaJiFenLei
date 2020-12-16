<template>
	<view style="padding-top: 1.4rem;">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/material/list">
			<block slot="backText">返回</block>
			<block slot="content">耗材申请审核</block>
		</cu-custom>
		<form>
			<view class="cu-form-group margin-top">
				<view class="title">申请人</view>
				<input placeholder="请输入申请人" v-model="formModel.fields.realName" :disabled="true"></input>
			</view>
			<view class="cu-form-group">
				<view class="title">材料类型</view>
				<picker @change="MaterialPickerChange" :value="materialtype.index" :range="materialtype.picker" range-key="materialTypeName"
				 :disabled="true">
					<view class="picker">
						{{materialtype.picker[materialtype.index].materialTypeName}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title">备注</view>
				<input placeholder="请输入备注" name="input" v-model="formModel.fields.remark"  :disabled="true"></input>
			</view>
		</form>
		<view class="bj-1" v-for="(item,index) in specificationmaterial.data" :key="index">
			<view class="ul">
				<view class="li">
					<view class="li-1">材料名称:</view>
					<view class="li-2">{{item.MaterialName}}</view>
				</view>
				<view class="li">
					<view class="li-1">规格型号:</view>
					<view class="li-2">{{item.SpecificationName}}</view>
				</view>
				<view class="li">
					<view class="li-1">数量:</view>
					<view class="li-2">{{item.Num}}</view>
				</view>
			</view>
		</view>
		<view class="cu-form-group" v-if="showlable()">
			<view class="title">科室审核意见</view>
			<input name="input" v-model="auditOpinion" :disabled="true"></input>
		</view>
		<view class="cu-form-group">
			<view class="title">审核意见</view>
			<input placeholder="请输入审核意见" name="input" v-model="formModel.fields.auditOpinion" ></input>
		</view>

		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="audittopass">
			通过
		</button>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="audittonopass">
			不通过
		</button>
	</view>
</template>

<script>
	import {
		loadMaterialTypeList,
		loadSpecificationMaterialList,
		auditPassConsumable,
		auditNoPassConsumable
	} from '../../api/material/consumable.js'
	export default {
		data() {
			return {
				uuid: '',
				materialtype: {
					picker: [{
						materialTypeUuid: '',
						materialTypeName: ''
					}],
					index: 0,
				},
				formModel: {
					fields: {
						consumableUuid: '',
						materialTypeUuid: '',
						realName: '',
						remark: '',
						auditOpinion: '同意'
					},
				},
				specificationmaterial: {
					data: []
				},
				auditOpinion:""
			}
		},
		onLoad(options) {
			this.$checkusertoken();
			this.doloadMaterialTypeList();
			this.uuid = options.uuid;
			this.doloadSpecificationMaterialList();
		},
		methods: {
			showlable() {
				if (this.$store.state.roleName == "办公类耗材终审" || this.$store.state.roleName == "计算机类耗材终审")
					return true;
				else
					return false;
			},
			doloadMaterialTypeList() {
				loadMaterialTypeList().then(res => {
					this.materialtype.picker = res.data.data;
					this.formModel.fields.materialTypeUuid = this.materialtype.picker[this.materialtype.index].materialTypeUuid;
				})
			},
			MaterialPickerChange(e) {
				this.materialtype.index = e.detail.value;
				this.formModel.fields.materialTypeUuid = this.materialtype.picker[this.materialtype.index].materialTypeUuid;
			},
			doloadSpecificationMaterialList() {
				loadSpecificationMaterialList({
					guid: this.uuid
				}).then(res => {
					if (res.data.code == 200) {
						console.log(res.data)
						this.formModel.fields.consumableUuid = res.data.data.consumable.ConsumableUuid;
						this.formModel.fields.materialTypeUuid = res.data.data.consumable.MaterialTypeUuid;
						if (res.data.data.consumable.Remark == null)
							this.formModel.fields.remark = "";
						else
							this.formModel.fields.remark = res.data.data.consumable.Remark;
						this.auditOpinion = res.data.data.consumable.AuditOpinion;
						this.formModel.fields.realName = res.data.data.realname;
						this.specificationmaterial.data = res.data.data.material;

						this.materialtype.index = this.getindex(this.formModel.fields.materialTypeUuid, this.materialtype.picker);
						this.isedit = true;
					}
				})
			},
			getindex(value, arr) {
				for (let i in arr) {
					if (value == arr[i].materialTypeUuid) {
						return i;
					}
				}
			},
			audittopass() {
				uni.showModal({
					title: '提示',
					content: '是否确定申请通过',
					success: (r) => {
						if (r.confirm) {
							auditPassConsumable(this.formModel.fields).then(res => {
								if (res.data.code == 200) {
									uni.showModal({
										title: '提示',
										content: res.data.message,
										showCancel: false,
										success() {
											uni.redirectTo({
												url: '/pages/material/list'
											});
										}
									})
								}
								else{
									uni.showModal({
										title: '提示',
										content: res.data.message,
										showCancel: false,
										success() {
											uni.redirectTo({
												url: '/pages/material/list'
											});
										}
									})
								}
							})
						}
					}
				})

			},
			audittonopass() {
				uni.showModal({
					title: '提示',
					content: '是否确定申请不通过',
					success: (r) => {
						if (r.confirm) {
							auditNoPassConsumable(this.formModel.fields).then(res => {
								if (res.data.code == 200) {
									uni.showModal({
										title: '提示',
										content: res.data.message,
										showCancel: false,
										success() {
											uni.redirectTo({
												url: '/pages/material/list'
											});
										}
									})
								}
								else{
									uni.showModal({
										title: '提示',
										content: res.data.message,
										showCancel: false,
										success() {
											uni.redirectTo({
												url: '/pages/material/list'
											});
										}
									})
								}
								
							})
						}
					}
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

<template>
	<view style="padding-top: 1.4rem;">

		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/material/list">
			<block slot="backText">返回</block>
			<block slot="content">新增耗材申请</block>
		</cu-custom>
		<form>
			<view class="cu-form-group margin-top">
				<view class="title">申请人</view>
				<input placeholder="请输入申请人" v-model="formModel.fields.realName" :disabled="true"></input>
			</view>
			<view class="cu-form-group">
				<view class="title">材料类型</view>
				<picker @change="MaterialPickerChange" :value="materialtype.index" :range="materialtype.picker" range-key="materialTypeName"
				 :disabled="isedit">
					<view class="picker">
						{{materialtype.picker[materialtype.index].materialTypeName}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title">备注</view>
				<input placeholder="请输入备注" name="input" focus="true" v-model="formModel.fields.remark"></input>
			</view>
			<view class="cu-bar bg-white margin-top">
				<!-- <view class="action">
					<text class="cuIcon-title text-orange "></text> 侧边抽屉
				</view> -->
				<view class="action">
					<button class="cu-btn bg-green shadow" @tap="showModal">添加材料</button>
				</view>
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
			<view class="ul">
				<view class="li-3"><button class="li-4" @click="edit(item.SpecificMaterialUuid)" v-if="">修改</button></view>
				<view class="li-3"><button class="li-4" @click="deleted(item.SpecificMaterialUuid)" v-if="">删除</button></view>
			</view>
		</view>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitfrom">
			保存
		</button>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submittoaduit">
			提交审核
		</button>
	</view>
</template>

<script>
	import {
		// createConsumable,
		// createToAuditConsumable
		loadMaterialTypeList,
		loadMaterialList,
		loadMaterialListguid,
		loadSpecificationList,
		loadSpecificationListguid,
		loadSpecificationMaterialList,
		editConsumabletoAdd,
		editConsumabletoAudit
	} from '../../api/material/consumable.js'
	import {
		deleteSpecificMaterial
	} from '../../api/material/specificmaterial.js'
	export default {
		data() {
			return {
				isedit: false,
				modalName: null,
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
					},
				},
				specificationmaterial: {
					data: []
				}
			}
		},
		onLoad(options) {
			this.$checkusertoken();
			this.doloadMaterialTypeList();
			//console.log(options.uuid);
			this.uuid = options.uuid;
			//this.doloadConsumable(options.uuid);
			this.formModel.fields.consumableUuid = this.uuid;
			this.formModel.fields.realName = this.$store.state.userName;
			this.doloadSpecificationMaterialList();
		},
		methods: {
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
			showModal() {
				editConsumabletoAdd(this.formModel.fields).then(res => {
					if (res.data.code == 200) {
						uni.navigateTo({
							url: '/pages/material/createadd?uuid=' + this.uuid + '&materialTypeUuid=' + this.formModel.fields.materialTypeUuid
						})
					}
				})
			},
			doloadSpecificationMaterialList() {
				loadSpecificationMaterialList({
					guid: this.uuid
				}).then(res => {
					if (res.data.code == 200) {
						this.formModel.fields.consumableUuid = res.data.data.consumable.ConsumableUuid;
						this.formModel.fields.materialTypeUuid = res.data.data.consumable.MaterialTypeUuid;
						if (res.data.data.consumable.Remark == null)
							this.formModel.fields.remark = "";
						else
							this.formModel.fields.remark = res.data.data.consumable.Remark;
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
			edit(uuid) {
				uni.navigateTo({
					url: '/pages/material/createedit?uuid=' + uuid + '&consumableuuid=' + this.uuid
				})
			},
			deleted(uuid) {
				uni.showModal({
					title: '提示',
					content: '是否确认删除',
					success: (r) => {
						if (r.confirm) {
							deleteSpecificMaterial({
								guid: uuid
							}).then(res => {
								this.doloadSpecificationMaterialList();
							})
						}
					}
				})

			},
			submitfrom() {
				editConsumabletoAdd(this.formModel.fields).then(res => {
					if (res.data.code == 200) {
						uni.showModal({
							title: '提示',
							content: res.data.message,
							showCancel: false,
							success: (r) => {
								if (r.confirm) {
									uni.redirectTo({
										url: '/pages/material/list'
									})
								}
							}
						})
					} else {
						uni.showModal({
							title: '提示',
							content: res.data.message,
							showCancel: false
						})
					}
				})
			},
			submittoaduit() {
				let valid = this.validateConsumableForm();
				if (valid != "true") {
					uni.showModal({
						title: '提示',
						content: valid,
						showCancel: false
					})
				} else {
					editConsumabletoAudit(this.formModel.fields).then(res => {
						if (res.data.code == 200) {
							uni.showModal({
								title: '提示',
								content: res.data.message,
								success: (r) => {
									if (r.confirm) {
										uni.redirectTo({
											url: '/pages/material/list'
										})
									}
								}
							})
						} else {
							uni.showModal({
								title: '提示',
								content: res.data.message,
								showCancel: false
							})
						}
					})
				}
			},
			validateConsumableForm() {
				let _valid = "true";
				if (this.specificationmaterial.data == null||this.specificationmaterial.data.length==0) {
					_valid = "申请材料条数不能为0";
					return _valid;
				}
				return _valid;
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
	.title_pd{
		padding-top:2.5rem;
	}
</style>

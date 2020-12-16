<template>
	<view style="padding-top: 1.4rem;">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" :tourl=url>
			<block slot="backText">返回</block>
			<block slot="content">新增材料</block>
		</cu-custom>
		<form>
			<view class="cu-form-group">
				<view class="title">材料名称</view>
				<picker @change="AddMaterialPickerChange" :value="addmaterial.material.index" :range="addmaterial.material.picker"
				 range-key="materialName">
					<view class="picker">
						{{addmaterial.material.picker[addmaterial.material.index].materialName}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group margin-top">
				<view class="title">规格型号</view>
				<picker @change="AddSpecificationPickerChange" :value="addmaterial.specification.index" :range="addmaterial.specification.picker"
				 range-key="specificationName">
					<view class="picker">
						{{addmaterial.specification.picker[addmaterial.specification.index].specificationName}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group margin-top">
				<view class="title"><span style="color:red;">*</span>数量</view>
				<input placeholder="请输入数量" v-model="formModel.fields.num" focus="true" ></input>
			</view>
		</form>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitfrom">
			保存
		</button>
	</view>
</template>

<script>
	import {
		loadMaterialListguid,
		loadSpecificationListguid
	} from '../../api/material/consumable.js'
	import {
		createSpecificMaterial
	} from '../../api/material/specificmaterial.js'

	export default {
		data() {
			return {
				url: '',
				uuid: '',
				addmaterial: {
					material: {
						picker: [{
							materialUuid: '',
							materialName: ''
						}],
						index: 0,
					},
					specification: {
						picker: [{
							specificationUuid: '',
							specificationName: ''
						}],
						index: 0,
					},
				},
				formModel: {
					fields: {
						consumableUuid: '',
						materialTypeUuid: '',
						materialUuid: '',
						specificationUuid: '',
						num: ''
					},
				},
			}
		},
		onLoad(options) {
			this.uuid = options.uuid;
			this.formModel.fields.consumableUuid = this.uuid;
			this.formModel.fields.materialTypeUuid = options.materialTypeUuid;
			this.url = "/pages/material/create?uuid=" + this.uuid;
			this.doloadMaterialList();
		},
		methods: {
			doloadMaterialList() {
				loadMaterialListguid({
					guid: this.formModel.fields.materialTypeUuid
				}).then(res => {
					this.addmaterial.material.picker = res.data.data;
					console.log(this.addmaterial.material.picker)
					this.formModel.fields.materialUuid = this.addmaterial.material.picker[this.addmaterial.material.index].materialUuid;
					this.doloadSpecificationListguid();
				})
			},
			AddMaterialPickerChange(e) {
				this.addmaterial.specification.index =0;
				this.addmaterial.material.index = e.detail.value;
				this.formModel.fields.materialUuid = this.addmaterial.material.picker[this.addmaterial.material.index].materialUuid;
				this.doloadSpecificationListguid();
			},
			doloadSpecificationListguid() {
				loadSpecificationListguid({
					guid: this.formModel.fields.materialUuid
				}).then(res => {
					this.addmaterial.specification.picker = res.data.data;
					this.formModel.fields.specificationUuid = this.addmaterial.specification.picker[this.addmaterial.specification.index]
					.specificationUuid;
				})
			},
			AddSpecificationPickerChange(e) {
				this.addmaterial.specification.index = e.detail.value;
				this.formModel.fields.specificationUuid = this.addmaterial.specification.picker[this.addmaterial.specification.index]
					.specificationUuid;
			},
			submitfrom() {
				let valid = this.validateConsumableForm();
				if (valid != "true") {
					uni.showModal({
						title: '提示',
						content: valid,
						showCancel: false
					})
				}
				else{
					createSpecificMaterial(this.formModel.fields).then(res => {
						if (res.data.code == 200) {
							uni.showModal({
								title: '提示',
								content: res.data.message,
								showCancel: false,
								success: (r) => {
									if (r.confirm) {
										uni.redirectTo({
											url: this.url
										})
									}
								}
							})
						}
					})
				}
			},
			validateConsumableForm() {
				let _valid = "true";	
				if (this.formModel.fields.num == '' || this.formModel.fields.num == null) {
					_valid = "数量不能为空";
					return _valid;
				}
				if (isNaN(this.formModel.fields.num)) {
					_valid = "数量应为数字";
					return _valid;
				}
				return _valid;
			}
		}
	}
</script>

<style>
</style>

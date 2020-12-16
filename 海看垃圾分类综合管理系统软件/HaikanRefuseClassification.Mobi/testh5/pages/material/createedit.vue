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
				<view class="title">数量</view>
				<input placeholder="请输入数量" v-model="formModel.fields.num"></input>
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
		editsaveSpecificMaterial,
		editSpecificMaterial
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
						specificMaterialUuid:'',
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
			console.log(options.consumableuuid);
			//this.formModel.fields.consumableUuid = this.uuid;
			//this.formModel.fields.materialTypeUuid = options.materialTypeUuid;
			this.url = "/pages/material/create?uuid=" + options.consumableuuid;
			this.doonloadSpecificationMaterial();
			//this.doloadMaterialList();
		},
		methods: {
		    async doonloadSpecificationMaterial(){
				await editSpecificMaterial({guid:this.uuid}).then(res=>{
					if(res.data.code==200)
					{
						this.formModel.fields.specificMaterialUuid = res.data.data.specificMaterialUuid;
						this.formModel.fields.consumableUuid = res.data.data.consumableUuid;
						this.formModel.fields.materialTypeUuid= res.data.data.materialTypeUuid;
						this.formModel.fields.materialUuid= res.data.data.materialUuid;
						this.formModel.fields.specificationUuid= res.data.data.specificationUuid;
						this.formModel.fields.num= res.data.data.num;
						this.doloadMaterialList();
					}
				})
			},
			async doloadMaterialList() {
				await loadMaterialListguid({
					guid: this.formModel.fields.materialTypeUuid
				}).then(res => {
					this.addmaterial.material.picker = res.data.data;
					//this.formModel.fields.materialUuid = this.addmaterial.material.picker[this.addmaterial.material.index].materialUuid;
					this.addmaterial.material.index = this.getindex(this.formModel.fields.materialUuid,this.addmaterial.material.picker);
					this.doloadSpecificationListguid();
				})
			},
			AddMaterialPickerChange(e) {
				this.addmaterial.material.index = e.detail.value;
				this.formModel.fields.materialUuid = this.addmaterial.material.picker[this.addmaterial.material.index].materialUuid;
				this.doloadSpecificationListguid1();
			},
			doloadSpecificationListguid() {
				loadSpecificationListguid({
					guid: this.formModel.fields.materialUuid
				}).then(res => {
					this.addmaterial.specification.picker = res.data.data;
					this.addmaterial.specification.index = this.getindex1(this.formModel.fields.specificationUuid,this.addmaterial.specification.picker);
				})
			},
			doloadSpecificationListguid1() {
				loadSpecificationListguid({
					guid: this.formModel.fields.materialUuid
				}).then(res => {
					this.addmaterial.specification.index=0;
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
			getindex(value,arr){
				for(let i in arr)
				{
					if(value==arr[i].materialUuid)
					{
						return  Number(i);
					}
				}
			},
			getindex1(value,arr){
				for(let i in arr)
				{
					if(value==arr[i].specificationUuid)
					{
						return Number(i);
					}
				}
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
					editsaveSpecificMaterial(this.formModel.fields).then(res => {
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

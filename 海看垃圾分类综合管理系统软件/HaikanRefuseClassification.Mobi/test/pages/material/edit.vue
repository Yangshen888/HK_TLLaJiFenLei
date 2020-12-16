<template>
	<!-- <view>
			<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/material/list">
				<block slot="backText">返回</block>
				<block slot="content">编辑耗材申请</block>
			</cu-custom>
			<form>
				<view class="cu-form-group margin-top">
					<view class="title">申请人</view>
					<input placeholder="请输入申请人"  v-model="formModel.fields.realName" :disabled="true"></input>
				</view>
				<view class="cu-form-group">
					<view class="title">材料类型</view>
					<picker @change="MaterialPickerChange" :value="materialtype.index" :range="materialtype.picker" :range-key="'name'">
						<view class="picker">
							{{materialtype.picker[materialtype.index].name}}
						</view>
					</picker>
				</view>
				<view class="cu-form-group">
					<view class="title">材料名称</view>
					<input placeholder="请输入材料名称" name="input" v-model="formModel.fields.materialName"></input>
				</view>
				<view class="cu-form-group">
					<view class="title">数量</view>
					<input placeholder="请输入数量" name="input" v-model="formModel.fields.num"></input>
				</view>
				<view class="cu-form-group">
					<view class="title">型号规格</view>
					<input placeholder="请输入型号规格" name="input" v-model="formModel.fields.specification"></input>
				</view>
				<view class="cu-form-group">
					<view class="title">备注</view>
					<input placeholder="请输入备注" name="input" v-model="formModel.fields.remark"></input>
				</view>
			</form>
			<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitfrom">
					 保存
			</button>
			<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submittoaduit">
					 提交审核
			</button>
		</view> -->
</template>

<script>
	// import {loadConsumable,
	// editConsumable,
	// editToAuditConsumable} from '../../api/material/consumable.js'
	// export default{
	// 	data(){
	// 		return{
	// 			materialtype:{
	// 				picker: [{value:0,name:'办公用品'},
	// 			             {value:1,name:'电子设备'}],
	// 			    index:0,
	// 			},
	// 			formModel:{
	// 				fields:{
	// 					consumableUuid:'',
	// 				    realName:'',
	// 	                materialName: '',
	// 	                materialType:0,
	// 	                num:'',
	// 	                specification:'',
	// 	                remark:''
	// 				    },
	// 				}
	// 		}
	// 	},
	// 	onLoad(options) {
	// 		console.log(options.uuid);
	// 		this.doloadConsumable(options.uuid);
	// 	},
	// 	methods:{
	// 		doloadConsumable(uuid){
	// 			loadConsumable({guid:uuid}).then(res=>{
	// 				if(res.data.code==200)
	// 				{
	// 					this.formModel.fields = res.data.data;
	// 					this.materialtype.index = this.getindex(this.formModel.fields.materialType,this.materialtype.picker);
	// 				}
	// 				else
	// 				{
	// 					uni.showModal({
	// 						title:"提示",
	// 						content:res.data.message,
	// 						showCancel:false,
	// 						success: (r) => {
	// 							if(r.confirm)
	// 							{
	// 								uni.redirectTo({
	// 									url:'/pages/material/list'
	// 								})
	// 							}
	// 						}
	// 					})
	// 				}
	// 			})
	// 		},
	// 	    getindex(value,arr){
	// 			for(let i in arr)
	// 			{
	// 				if(value==arr[i].value)
	// 				{
	// 					return i;
	// 				}
	// 			}
	// 	    },
	// 		MaterialPickerChange(e) {
	// 			this.materialtype.index = e.detail.value;
	// 			this.formModel.fields.materialType = this.materialtype.picker[this.materialtype.index].value;
	// 		},
	// 		submitfrom(){
	// 			let valid = this.validateConsumableForm();
	// 			if(valid!="true")
	// 			{
	// 				uni.showModal({		
	// 						title:'提示',
	// 						content:valid,
	// 						showCancel:false
	// 				})
	// 			}
	// 			else
	// 			{
	// 				editConsumable(this.formModel.fields).then(res=>{
	// 					if(res.data.code==200)
	// 					{
	// 						uni.showModal({
	// 							title:'提示',
	// 							content:res.data.message,
	// 							showCancel:false,
	// 							success: (r) => {
	// 								if(r.confirm)
	// 								{
	// 									uni.redirectTo({
	// 										url:'/pages/material/list'
	// 									})
	// 								}
	// 							}
	// 						})
	// 					}
	// 					else{
	// 						uni.showModal({
	// 							title:'提示',
	// 							content:res.data.message,
	// 							showCancel:false
	// 						})
	// 					}
	// 				})
	// 			}
	// 			
	// 		},
	// 		submittoaduit(){
	// 			let valid = this.validateConsumableForm();
	// 			if(valid!="true")
	// 			{
	// 				uni.showModal({		
	// 						title:'提示',
	// 						content:valid,
	// 						showCancel:false
	// 				})
	// 			}
	// 			else
	// 			{
	// 				editToAuditConsumable(this.formModel.fields).then(res=>{
	// 					if(res.data.code==200)
	// 					{
	// 						uni.showModal({
	// 							title:'提示',
	// 							content:res.data.message,
	// 							showCancel:false,
	// 							success: (r) => {
	// 								if(r.confirm)
	// 								{
	// 									uni.redirectTo({
	// 										url:'/pages/material/list'
	// 									})
	// 								}
	// 							}
	// 						})
	// 					}
	// 					else{
	// 						uni.showModal({
	// 							title:'提示',
	// 							content:res.data.message,
	// 							showCancel:false
	// 						})
	// 					}
	// 				})
	// 			}
	// 		},
	// 		validateConsumableForm(){
	// 			let _valid = "true";
	// 			if(this.formModel.fields.realName==''||this.formModel.fields.realName==null)
	// 			{
	// 				_valid="申请人不能为空";
	// 				return _valid;
	// 			}
	// 			if(this.formModel.fields.materialName==''||this.formModel.fields.materialName==null)
	// 			{
	// 				_valid="材料名称不能为空";
	// 				return _valid;
	// 			}
	// 			if(this.formModel.fields.num==''||this.formModel.fields.num==null)
	// 			{
	// 				_valid="数量不能为空";
	// 				return _valid;
	// 			}
	// 			if(isNaN(this.formModel.fields.num))
	// 			{
	// 				_valid="数量应为数字";
	// 				return _valid;
	// 			}
	// 			if(this.formModel.fields.specification==''||this.formModel.fields.specification==null)
	// 			{
	// 				_valid="型号规格不能为空";
	// 				return _valid;
	// 			}
	// 			return _valid;
	// 		}
	// 	}
	// }
</script>

<style>
</style>

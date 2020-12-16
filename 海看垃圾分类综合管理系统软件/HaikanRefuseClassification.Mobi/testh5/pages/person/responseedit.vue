<template>
	<view style="padding-top: 1.4rem;">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/person/responselist">
			<block slot="backText">返回</block>
			<block slot="content">编辑用户反馈</block>
		</cu-custom>
		<form>
			<view class="cu-form-group margin-top">
				<view class="title">用户姓名</view>
				<input placeholder="请输入用户姓名查询" name="input" v-model="formModel.fields.realName"></input>
			</view>
			<view class="cu-form-group margin-top">
				<view class="title">反馈内容</view>
				<input placeholder="请输入反馈内容查询" name="input" v-model="formModel.fields.problemContent"></input>
			</view>	
		</form>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitfrom">
			保存
		</button>
	</view>
</template>

<script>
	import {
		loadCarDriver,  //加载
		editCarDriver,  //保存
	} from '../../api/person/response.js'
	export default {
		data() {
			return {
				formModel: {
					fields: {
						realName:"",
						problemContent:"",
					},
				}
			}
		},
		onLoad(options) {
			this.$checkusertoken();
			this.doloadCarDispatch(options.uuid);
		},
		methods: {		
			doloadCarDispatch(uuid){
				loadCarDriver({guid:uuid}).then(res=>{
					if(res.data.code==200)
					{
						this.formModel.fields = res.data.data;						
					}
					else
					{
						uni.showModal({
							title:"提示",
							content:res.data.message,
							showCancel:false,
							success: (r) => {
								if(r.confirm)
								{
									uni.redirectTo({
										url:'/pages/person/responselist'
									})
								}
							}
						})
					}
				})
			},
			getindex(value,arr){
				for(let i in arr)
				{
					if(value==arr[i].departmentUuid)
					{
						return i;
					}
				}
			},			
			//保存
			submitfrom() {
				let valid = this.validateCarDispatchForm();
				if (valid != "true") {
					uni.showModal({
						title: '提示',
						content: valid,
						showCancel: false
					})
				} else {
					editCarDriver(this.formModel.fields).then(res => {
						if (res.data.code == 200) {
							uni.showModal({
								title: '提示',
								content: res.data.message,
								showCancel: false,
								success: (r) => {
									if (r.confirm) {
										uni.redirectTo({
											url: '/pages/person/responselist'
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
			//非空验证
			validateCarDispatchForm() {
				let _valid = "true";
				if (this.formModel.fields.realName == '' || this.formModel.fields.realName == null) {
					_valid = "用户姓名不能为空";
					return _valid;
				}
				if (this.formModel.fields.problemContent == '' || this.formModel.fields.problemContent == null) {
					_valid = "反馈内容不能为空";
					return _valid;
				}
				return _valid;
			}
		}
	}
</script>

<style>
</style>



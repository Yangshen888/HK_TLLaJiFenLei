<template>
	<view style="padding-top: 1.4rem;">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">用户反馈</block>
		</cu-custom>
		<form>
			<!-- <view class="cu-form-group margin-top">
				<view class="title">用户姓名</view>
				<input placeholder="请输入用户姓名" name="input" v-model="formModel.fields.realName"></input>
			</view> -->
			<view class="cu-form-group margin-top">
				<view class="title" style="width: 150rpx;">问题类型</view>
				<input placeholder="请输入问题类型" name="input" v-model="formModel.fields.problemType"></input>
			</view>	
			<view class="cu-form-group margin-top uni-textarea">
				<view class="title" style="width: 150rpx;">反馈内容</view>
				<textarea  style="height: 30rpx;" auto-height placeholder="请输入反馈内容,请不要输入特殊表情" v-model="formModel.fields.problemContent"></textarea>
			</view>	
			<view class="cu-form-group margin-top uni-textarea">
				<view class="title" style="width: 150rpx;">备&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;注</view>
				<textarea style="height: 30rpx;" auto-height placeholder="请输入备注"  v-model="formModel.fields.remarks"></textarea>
			</view>	
		</form>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitfrom">
			保存
		</button>
	</view>
</template>

<script>
	import {
		createCarDriver  //保存
	} from '../../api/person/response.js'
	export default {
		data() {		
			return {				
				formModel: {
					fields: {
						SystemUserUuid:"",
						problemContent:"",
						problemType:"",
						remarks:"",
					},
				}
			}
		},
		onLoad() {
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
			this.$checkusertoken();
			}
		},
		methods: {						
			//保存
			submitfrom() {
				let valid = this.validateCarDispatchForm();
				this.formModel.fields.SystemUserUuid=this.$store.state.userId;
				console.log(this.formModel.fields.SystemUserUuid);
				console.log(this.$store.state.userId);
				if (valid != "true") {
					uni.showModal({
						title: '提示',
						content: valid,
						showCancel: false
					})
				} else {
					createCarDriver(this.formModel.fields).then(res => {
						if (res.data.code == 200) {
							uni.showModal({
								title: '提示',
								content:'反馈'+ res.data.message,
								showCancel: false,
								success: (r) => {
									if (r.confirm) {
										uni.redirectTo({
											url: '/pages/home/index'
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
				// if (this.formModel.fields.realName == '' || this.formModel.fields.realName == null) {
				// 	_valid = "用户姓名不能为空";
				// 	return _valid;
				// }
				if (this.formModel.fields.problemType == '' || this.formModel.fields.problemType == null) {
					_valid = "问题类型不能为空";
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


<style>
</style>





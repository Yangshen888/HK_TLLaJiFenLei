<template>
	<view style="padding-top: 1.4rem;">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/home">
						<block slot="backText">返回</block>
						<block slot="content">修改密码</block>
		</cu-custom>
		<form>
			<view class="cu-form-group margin-top">
				<view class="title"><span style="color:red;">*</span>原密码</view>
				<input type="password" placeholder="请输入原密码" name="input" focus="true" v-model="formModel.fields.oldpwd"></input>
			</view>
			<view class="cu-form-group">
				<view class="title"><span style="color:red;">*</span>新密码</view>
				<input type="password" placeholder="请输入新密码" name="input" v-model="formModel.fields.newpwd"></input>
			</view>
			<view class="cu-form-group">
				<view class="title"><span style="color:red;">*</span>确认密码</view>
				<input type="password" placeholder="请第二次确认密码" name="input" v-model="formModel.fields.newpwd2"></input>
			</view>
		</form>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="confirmfrom">
				 确认修改
		</button>
	</view>
</template>

<script>
	import {edituserPwd} from '../../api/user/userinfo.js'
	export default{
		data(){
			return{
				formModel:{
					fields:{
						oldpwd:'',
						newpwd:'',
						newpwd2:''
					}
				}
			}
		},
		methods:{
			confirmfrom(){
				let valid = this.validpwd();
				if(valid!="true")
				{
					uni.showModal({		
							title:'提示',
							content:valid,
							showCancel:false
					})
				}
				else{
					uni.showModal({
						title:'提示',
						content:'是否确认修改密码',
						success: (r) => {
							if(r.confirm)
							{
								this.submitform();
							}					
						}
					})
				}
			},
			submitform(){
				edituserPwd(this.formModel.fields).then(res=>{
					console.log(res.data)
					if(res.data.code==200)
					{
						uni.showModal({
							title:'提示',
							content:'修改密码成功',
							success: (r) => {
								if(r.confirm)
								{
									this.$store.commit('logout');
									uni.clearStorage();
									uni.reLaunch({
										url:'../login/login'
									})
								}					
							}
						})
					}
					else
					{
						uni.showModal({		
								title:'提示',
								content:res.data.message,
								showCancel:false
						})
					}
				})
			},
			validpwd(){
				let _valid = "true";
				if(this.formModel.fields.oldpwd==''||this.formModel.fields.oldpwd==null)
				{
					_valid="原密码不能为空";
					return _valid;
				}
				if(this.formModel.fields.newpwd==''||this.formModel.fields.newpwd==null)
				{
					_valid="新密码不能为空";
					return _valid;
				}
				if(this.formModel.fields.newpwd2==''||this.formModel.fields.newpwd2==null)
				{
					_valid="确认密码不能为空";
					return _valid;
				}	
                if(this.formModel.fields.newpwd!=this.formModel.fields.newpwd2)
				{
					_valid="新密码与确认密码不一致";
					return _valid;
				}
				return _valid;
			}
		}
	}
</script>

<style>
</style>

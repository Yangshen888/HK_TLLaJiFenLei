<template>
	<view class="content" style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/login/login">
			<block slot="backText">返回</block>
			<block slot="content">忘记密码</block>
		</cu-custom>
		<view class="list">
			<view class="tishi">若您忘记了密码，可在此重新设置新密码。</view>
			<view class="list-call">
				<image class="img" src="/static/shilu-login/1.png"></image>
				<input class="biaoti" type="number" v-model="phoneno" maxlength="11" placeholder="请输入手机号" />
			</view>
			<view class="list-call">
				<image class="img" src="/static/shilu-login/2.png"></image>
				<input class="biaoti" type="text" v-model="password" maxlength="32" placeholder="请输入新密码" :password="!showPassword" />
				<image class="img" :src="showPassword?'/static/shilu-login/op.png':'/static/shilu-login/cl.png'" @tap="display"></image>
			</view>
			<view class="list-call">
				<image class="img" src="/static/shilu-login/3.png"></image>
				<input class="biaoti"  v-model="code" maxlength="6" placeholder="验证码" />
				<view class="yzm" :class="{ yzms: second>0 }" @tap="getcode">{{yanzhengma}}</view>
			</view>
		</view>
		<view class="dlbutton" hover-class="dlbutton-hover" @tap="bindLogin()">
			<text>修改密码</text>
		</view>

	</view>
</template>

<script>
	import httpbase from '@/components/utils/http.js';
	import {doCreateUser,getRegCode} from '@/api/supervisor/manager.js'
	
	var tha,js;
	export default {
		data() {
			return {
				phoneno:'',
				second:0,
				code:"",
				showPassword:false,
				password:'',
				str:'',
				havecode:false
			}
		},
		onLoad(){
			tha = this;
		},
		computed:{
			yanzhengma(){
				if(this.second==0){
					return '获取验证码';
				}else{
					if(this.second<10&&this.second>=0){
						return '重新获取0'+this.second;
					}else if(this.second>=10){
						return '重新获取'+this.second;
					}else{
						return '重新获取';
					}
				}
			}
		},
		methods: {
			display() {
			    this.showPassword = !this.showPassword
			},
			getcode(){
				var that=this;
				if(this.phoneno==''){
					uni.showToast({
					    icon: 'none',
					    title: '请输入正确的手机号'
					});
					return;
				}
			
				if(this.havecode)
				{
					uni.showToast({title:"请等待！"});
					return;
				}
				this.havecode=true;
				// this.havecode=false;
				//到计时
				
				this.second = 60;
				js = setInterval(function(){
					tha.second--;
					if(tha.second<=0){
						that.havecode=false;
						clearInterval(js)
					}
				},1000)
				
				getRegCode(this.phoneno).then(res=>{
					if(res.data.code==200)
					{
						this.str=res.data.data.str;
						uni.showToast({title:res.data.message});
						
					}else{
						uni.showToast({title:res.data.message,icon:'none'});
					}
				}).catch(error=>{
			    console.log(error)
			  });
			
			},

			bindLogin() {
				if (this.phoneno.length != 11) {
				     uni.showToast({
				        icon: 'none',
				        title: '手机号不正确'
				    });
				    return;
				}
			    if (this.password.length < 6) {
			        uni.showToast({
			            icon: 'none',
			            title: '密码不正确'
			        });
			        return;
			    }
				if (this.code.length != 4) {
				    uni.showToast({
				        icon: 'none',
				        title: '验证码不正确'
				    });
				    return;
				}
				if (this.code.toLowerCase() != this.str.toLowerCase()) {
				    uni.showToast({
				        icon: 'none',
				        title: '验证码不正确'
				    });
				    return;
				}
				let data1= {
						phone:this.phoneno,
						password:this.password,
						code:this.code
					};
					console.log(data1);
				uni.request({
				    url: httpbase.baseUrl+'api/v1/app/SupervisorManage/SetNewPass',
				    data: {
						phone:this.phoneno,
						password:this.password,
						code:this.code
					},
					method: 'POST',
					dataType:'json',
				    success: (res) => {
						if(res.data.code!=200){
							uni.showToast({title:res.data.msg,icon:'none'});
						}else{
							uni.showToast({title:res.data.msg});
							setTimeout(function(){
								uni.navigateBack();
							},1500) 
						}
				    }
				});
				
			}
		}
	}
</script>

<style>
	.content {
		display: flex;
		flex-direction: column;
		justify-content: center;
	}
	.tishi {
		color: #999999;
		font-size: 28upx;
		line-height: 50upx;
		margin-bottom: 50upx;
	}
    .list {
    	display: flex;
    	flex-direction: column;
    	padding-top: 50upx;
    	padding-left: 70upx;
    	padding-right: 70upx;
    }
    .list-call{
    	display: flex;
    	flex-direction: row;
    	justify-content: space-between;
    	align-items: center;
    	height: 100upx;
    	color: #333333;
    	border-bottom: 1upx solid rgba(230,230,230,1);
    }
    .list-call .img{
    	width: 40upx;
    	height: 40upx;
    }
    .list-call .biaoti{
    	flex: 1;
    	text-align: left;
    	font-size: 32upx;
    	line-height: 100upx;
    	margin-left: 16upx;
    }
	.dlbutton {
		color: #FFFFFF;
		font-size: 34upx;
		width:470upx;
		height:100upx;
		background:linear-gradient(-90deg,rgba(63,205,235,1),rgba(188,226,158,1));
		box-shadow:0upx 0upx 13upx 0upx rgba(164,217,228,0.2);
		border-radius:50upx;
		line-height: 100upx;
		text-align: center;
		margin-left: auto;
		margin-right: auto;
		margin-top: 100upx;
	}
	.dlbutton-hover {
		background:linear-gradient(-90deg,rgba(63,205,235,0.9),rgba(188,226,158,0.9));
	}
	.yzm {
		color: #FF7D13;
		font-size: 24upx;
		line-height: 64upx;
		padding-left: 10upx;
		padding-right: 10upx;
		width:auto;
		height:64upx;
		border:1upx solid rgba(255,131,30,1);
		border-radius: 50upx;
	}
	.yzms {
		color: #999999 !important;
		border:1upx solid #999999;
	}
</style>

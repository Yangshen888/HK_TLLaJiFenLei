<template>
	<view class="content" style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="false" tourl="/pages/home/index">
			<!-- <block slot="backText">返回</block> -->
			<block slot="content">绑定微信</block>
		</cu-custom>
		<view class="list">
			<view class="list-call">
				<image class="img" src="/static/shilu-login/1.png"></image>
				<input class="biaoti" v-model="username" maxlength="11" placeholder="输入手机号" :disabled="qs==2"/>
			</view>
			<view v-show="qs==1" class="list-call">
				<image class="img" src="/static/shilu-login/2.png"></image>
				<input class="biaoti" v-model="password" type="text" maxlength="32" placeholder="输入密码" password="true" />
			</view>
			<view class="list-call">
				<image class="img" src="/static/shilu-login/4.png"></image>
				<input class="biaoti" v-model="wechat" type="text"  placeholder="微信号" disabled="false" />
			</view>
			<!-- <view class="list-call">
			<image class="img" src="/static/shilu-login/4.png"><view class="uni-form-item uni-column">
				<picker @change="typeChange" :range="array">
					<label>{{array[jq]}}</label>
				</picker>
			</view></image>				
		</view> -->
		</view>
		<view v-show="qs==1" class="dlbutton" hover-class="dlbutton-hover" @tap="binding()"><text>绑定</text></view>
		<view v-show="qs==2" class="dlbutton" hover-class="dlbutton-hover" @tap="relieve()"><text>解绑</text></view>
		
	</view>
</template>

<script>
	import {bindWechat,relieveWechat} from '@/api/user/bind.js';
	import {
		getWXOpenAuth
	} from '@/api/appvue.js';
export default {
	data() {
		return {
			qs:0,
			username:'',
			password:'',
			wechat:'',
			appid: 'wxda982699a0604145',
			secret: 'a070591fca4a02e9502125d8b0a4d87a'
		};
	},
	methods: {
		binding(){
			let data={
				username:this.username,
				password:this.password,
				wechat:this.wechat,
			}
			var that = this;
			bindWechat(data).then(res=>{
				console.log(res);
				if(res.data.code==200){
					uni.showToast({ title: res.data.message+' 正在登陆....', icon: 'none', mask: true, duration: 3000 });
					
					uni.login({
						success: function(res) {
							let js_code = res.code;
							uni.request({
								url: 'https://api.weixin.qq.com/sns/jscode2session?appid=' + that.appid + '&secret=' + that.secret +
									'&js_code=' + js_code,
								header: {
									"Content-Type": "application/x-www-form-urlencoded",
								},
								method: "post",
								success: res => {
									uni.setStorageSync("openid", res.data.openid);
									console.log("openid:" + res.data.openid);
									if (res.data.openid != "" && res.data.openid != null && res.data.openid != undefined) {
										getWXOpenAuth(res.data.openid).then(res => {
											console.log("最开始:");
											console.log(res);
											if (res.data.code == 200) {
												let token = res.data.data.tokens;
												uni.setStorageSync('token', token);
												let user = {
													userName: res.data.data.user_name,
													userId: res.data.data.user_guid,
													userType: res.data.data.user_type,
													//DepartmentName: res.data.data.user_departmentName,
													//DepartmentGuid: res.data.data.user_departmentGuid,
													RoleName: res.data.data.roleName,
													token: token,
													phone:res.data.data.phone,
													openid:res.data.data.openid,
												};									    
												that.$store.commit('login', user);
												//that.$store.state.phone = user.userName;
												that.$store.state.seltab = 1;
												console.log("code=" + res.data.code);
												console.log(res);
												if (res.data.data.address == "" || res.data.data.address == null) {
													uni.reLaunch({
														url: '../YongHu/addinfo'
													});
												} else {
													console.log(22);
													uni.reLaunch({
														url: '../home/index'
													});
												}
											} else {
												uni.showModal({
													title: '提示',
													content: res.data.message,
													// success: function(res) {
													// 	if (res.confirm) {
													// 		console.log('用户点击确定');
													// 		uni.reLaunch({
													// 			url: '../login/bind'
													// 		});
													// 	} else if (res.cancel) {
													// 		console.log('用户点击取消');
													// 	}
													// }
												});
											}
										});
									} else {
										uni.showModal({
											title: '提示',
											content: 'Openid获取失败,请重新获取!',
											showCancel: false
										});
									}
								}
							})
						}
					})
				}else{
                                        uni.showModal({
                                                title: '提示',
                                                content: res.data.message,
                                                showCancel: false
                                        });
                                }
			});
		},
		relieve(){
			console.log(2);
			let data={
				username:this.username,
				password:'',
				wechat:this.wechat,
			}
			relieveWechat(data).then(res=>{
				console.log(res);
				this.$store.commit('logout');
				if(res.data.code==200){
					uni.showModal({
						title: '提示',
						content: res.data.message,
						showCancel:false,
						success: function(res) {
							uni.reLaunch({
								url: '/pages/login/bind?id=1'
							})
						}
					});
					
					
				}
				
			});
		}
	},
	onLoad(opt) {
		console.log(opt);
		
		this.wechat=uni.getStorageSync('openid');
		if(opt.id==2){
			this.qs=2;
			console.log(this.$store.state);
			this.username=this.$store.state.phone;
		}
		if(opt.id==1){
			this.qs=1;
		}
	}
};
</script>

<style>
.mui-checkbox input[type='checkbox']:checked:before {
	color: #fff;
}

.RememberCheck {
	margin-left: 70%;
	margin-top: 10px;
	text-indent: 10px;
	color: #000;
}
.content {
	display: flex;
	flex-direction: column;
	justify-content: center;
}
.header {
	/* width:161upx;
	height:161upx;
	background:rgba(63,205,235,1);
	box-shadow:0upx 12upx 13upx 0upx rgba(63,205,235,0.47);
	border-radius:50%;
	margin-top: 30upx;
	margin-left: auto;
	margin-right: auto; */
}
.header image {
	/* width:161upx; */
	/* height:161upx; */
	/* border-radius:50%; */
	width: -webkit-fill-available;
	height: 160px;
}

.list {
	display: flex;
	flex-direction: column;
	padding-top: 50upx;
	padding-left: 70upx;
	padding-right: 70upx;
}
.list-call {
	display: flex;
	flex-direction: row;
	justify-content: space-between;
	align-items: center;
	height: 100upx;
	color: #333333;
	border-bottom: 1upx solid rgba(230, 230, 230, 1);
}
.list-call .img {
	width: 40upx;
	height: 40upx;
}
.list-call .biaoti {
	flex: 1;
	text-align: left;
	font-size: 32upx;
	line-height: 100upx;
	margin-left: 16upx;
}

.dlbutton {
	color: #ffffff;
	font-size: 34upx;
	width: 470upx;
	height: 100upx;
	background: linear-gradient(-90deg, rgba(63, 205, 235, 1), rgba(188, 226, 158, 1));
	box-shadow: 0upx 0upx 13upx 0upx rgba(164, 217, 228, 0.2);
	border-radius: 50upx;
	line-height: 100upx;
	text-align: center;
	margin-left: auto;
	margin-right: auto;
	margin-top: 40upx;
}
.dlbutton-hover {
	background: linear-gradient(-90deg, rgba(63, 205, 235, 0.9), rgba(188, 226, 158, 0.9));
}
.xieyi {
	display: flex;
	flex-direction: row;
	justify-content: center;
	align-items: center;
	font-size: 30upx;
	margin-top: 50upx;
	color: #ffa800;
	text-align: center;
	height: 40upx;
	line-height: 40upx;
}
.xieyi text {
	font-size: 24upx;
	margin-left: 15upx;
	margin-right: 15upx;
}
</style>

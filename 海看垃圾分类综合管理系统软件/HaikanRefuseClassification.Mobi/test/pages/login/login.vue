<template>
	<view class="content" style='padding-top:2rem'>
		<view class="header"><image src="https://ljfl.hztlcgj.com/image/head4.png" style="width:100%;"></image></view>

		<view class="list">
			<view class="list-call">
				<image class="img" src="/static/shilu-login/1.png"></image>
				<input class="biaoti" v-model="phone" maxlength="11" placeholder="请先获取手机号" disabled="true"/>
			</view>
			<!-- <view class="list-call">
				<image class="img" src="/static/shilu-login/2.png"></image>
				<input class="biaoti" v-model="password" type="text" maxlength="32" placeholder="输入密码" password="true" />
			</view> -->
			<!-- <view class="list-call">
				<image class="img" src="/static/shilu-login/4.png"><view class="uni-form-item uni-column">
					<picker @change="typeChange" :range="array">
						<label>{{array[jq]}}</label>
					</picker>
				</view></image>				
			</view> -->
		</view>

		<!-- 记住密码 -->
		<!-- <view class=" mui-input-row mui-checkbox ">
			<checkbox-group @change="checkboxChange">
				<checkbox id="chkRem" type="checkbox" :checked="rememberPsw" @tap="rememberPsw = !rememberPsw" color="#FFCC33" class="RememberCheck" style="transform:scale(0.7)" />
				记住密码
			</checkbox-group>
		</view> -->

		<!-- <view class="dlbutton" hover-class="dlbutton-hover" @tap="submitlogin()"><text>登录</text></view> -->

		<!-- <view class="xieyi">
			<navigator url="forget" open-type="navigate">忘记密码</navigator>
			<text>|</text>
			<navigator url="reg" open-type="navigate">注册账户</navigator>
		</view> -->
		<view>
			<!-- #ifdef MP-WEIXIN -->
			<button v-if="phone.length==11" class="dlbutton" hover-class="dlbutton-hover" type="default"  open-type="getUserInfo"  @getuserinfo="getUserInfo"  withCredentials="true">微信号授权登录</button>
			<button v-if="phone<11" class="dlbutton" hover-class="dlbutton-hover" open-type="getPhoneNumber" @getphonenumber="getPhoneNumber" >获取手机号</button>
			<!-- #endif -->
		</view>
	</view>
</template>

<script>
var self;

var webapiaddress = null; //链接地址

import { mapState, mapMutation } from 'vuex';
import http from '@/components/utils/http.js';
import { GetUserInfo } from '@/api/user/userinfo.js';
import { WXAuth, WXPhone, getWXOpenAuth } from '@/api/appvue.js';
import { getWechat } from 'api/user/bind.js';

export default {
	computed: mapState(['hasLogin']),
	data() {
		return {
			username: '',
			password: '',
			rememberPsw: true,
			array: ['督导员', '用户'],
			jq: 0,
			phone:'',
			appid: 'wxda982699a0604145',
			secret: 'a070591fca4a02e9502125d8b0a4d87a'
		};
	},
	methods: {
		typeChange(e) {
			this.index=e.target.value;
			this.jq=this.index;
		},
		
		submitlogin() {
			var that=this;
			if (this.username != '' && this.username != null && (this.password != '' && this.password != null)) {
				let opts = {
					url: 'api/oauth/WXAuth',
					method: 'post'
				};
				http.httpRequest(opts, {
					username: this.username,
					password: this.password
				})
					.then(res => {
						if (res.data.code == 200) {
							let token = res.data.data;
							try {
								uni.setStorageSync('token', token);
							} catch (e) {
								//TODO handle the exception
							}

							http.httpTokenRequest({
								url: 'api/v1/account/profile',
								method: 'get'
							}).then(res => {
								if (res.data.code == 200) {
									console.log(res.data);
									console.log(res.data.data);
									let user = {
										userName: res.data.data.user_name,
										userId: res.data.data.user_guid,
										userType: res.data.data.user_type,
										state: res.data.data.shop_state,
										DepartmentName: res.data.data.user_departmentName,
										DepartmentGuid: res.data.data.user_departmentGuid,
										RoleName: res.data.data.roleName,
										token: token,
										idCard:res.data.data.idCard,
									};
									this.$store.commit('login', user);
									console.log(this.$store.state);
									this.$store.state.phone=this.username;
									this.$store.state.userDepartmentName=user.state;
									//登录成功将用户名密码存储到用户本地
									if (this.rememberPsw) {
										//用户勾选“记住密码”
										uni.setStorageSync('username', this.username);
										uni.setStorageSync('password', this.password);
									} else {
										//用户没有勾选“记住密码”
										uni.removeStorageSync('username');
										uni.removeStorageSync('password');
										this.username = '';
										this.password = '';
									}
									// if(res.data.data.roleName=="普通用户")
									// 	this.$store.state.seltab=2;
									// else
									console.log(33);
									GetUserInfo(this.$store.state.userId).then(res=>{
										console.log(res);
										console.log(res.data.data);
										if(res.data.code==200){
												if(res.data.data.a.address=="" || res.data.data.a.address==null)
												{
													uni.reLaunch({
														url: '../YongHu/addinfo'
													});
												}
												else{
													console.log(22);
													this.$store.state.seltab=1;
													this.$store.state.shopid = res.data.data.a.shopUuid;
													this.$store.state.HomeAddressUUID = res.data.data.a.homeAddressUuid;
													uni.reLaunch({
														url: '../home/index'
													});
												}
											}
											else{
												console.log(22);
												this.$store.state.seltab=1;
												uni.reLaunch({
													url: '../home/index'
												});
											}
									})
									
									
								} else {
									uni.showModal({
										title: '提示',
										content: res.data.message
									});
								}
							});
						} else {
							uni.showModal({
								title: '提示',
								content: res.data.message
							});
						}
					})
					.catch(res => {
						uni.showModal({
							title: '提示',
							content: '网络服务报错，请检查您的网络'
						});
					});
			} else {
				uni.showModal({
					title: '提示',
					content: '用户名和密码不能为空',
					showCancel: false
				});
			}
		},
		checkboxChange: function(e) {
			console.log(e.detail.value.length);
			if (e.detail.value.length == 1) {
				//获取缓存的账号
				uni.getStorageSync('username', this.username);
				uni.getStorageSync('password', this.password);
			} else {
				uni.removeStorageSync('username');
				uni.removeStorageSync('password');
			}
		},
		getUserInfo(res) {
			console.log(res);
			let that=this;
			uni.login({
				provider: 'weixin',
				success: function(loginRes) {
					console.log(loginRes);
					// 获取用户信息
					uni.getUserInfo({
						provider: 'weixin',
						success: function(infoRes) {
							console.log(infoRes);
							console.log('用户昵称为：' + infoRes.userInfo.nickName);
							console.log(that.phone);
							let data={
								encryptedData:infoRes.encryptedData,
								iv:infoRes.iv,
								nickName:infoRes.userInfo.nickName,
								session_key:uni.getStorageSync('session_key'),
								openid:uni.getStorageSync('openid'),
								phone:that.phone,
								sex:infoRes.userInfo.gender,
							};
							WXAuth(data).then(res=>{
								console.log(res);
								if(res.data.code==200){
									uni.showToast({ title: res.data.message+' 正在登陆....', icon: 'none', mask: true, duration: 3000 });
									uni.login({
											success: function(res) {
												let js_code = res.code;
												let data = {
													appid: that.appid,
													secret: that.secret,
													js_code: js_code
												};
												console.log(data);
												getWechat(data).then(res => {
													console.log(res);
													if (res.data.code == 200) {
														uni.setStorageSync('openid', res.data.data.openid);
														uni.setStorageSync('session_key',res.data.data.session_key);
														console.log('openid:' + res.data.data.openid);
														console.log('session_key:' + res.data.data.session_key);
														if (res.data.data.openid != '' && res.data.data.openid != null && res.data.data.openid != undefined) {
															getWXOpenAuth(res.data.data.openid).then(res => {
																console.log('最开始:');
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
																		phone: res.data.data.phone,
																		openid: res.data.data.openid,
																		HomeAddressUUID: res.data.data.homeAddressUUID,
																		shopid:res.data.data.shop_guid
																	};
																	that.$store.commit('login', user);
																	//that.$store.state.phone = user.userName;
																	that.$store.state.seltab = 1;
																	console.log('code=' + res.data.code);
																	console.log(res);
																	// if (res.data.data.address == '' || res.data.data.address == null) {
																	// 	uni.reLaunch({
																	// 		url: '../YongHu/addinfo'
																	// 	});
																	// } else {
																		console.log(22);
																		uni.reLaunch({
																			url: '../home/index'
																		});
																	// }
																} else {
																	uni.showModal({
																		title: '提示',
																		content: '登录失败',
																		
																	});
																}
															});
														}
													} else {
														uni.showModal({
															title: '提示',
															content: 'Openid获取失败,请重新获取!',
															showCancel: false
														});
													}
												});
												
											},
									});
								}
							}
							);
						},
						fail:function(err){
							uni.showModal({
								title: '提示',
								content: '微信授权失败',
								showCancel: false
							});
						},
					});
				},
				
			});
		},
		getPhoneNumber(e) {
			//console.log(e);
			console.log(e.detail.errMsg);
			console.log(e.detail.iv);
			console.log(e.detail.encryptedData);

			if (e.detail.errMsg == 'getPhoneNumber:ok') {
				var that = this;
				uni.login({
					success: function(res) {
						let js_code = res.code;
						let data = {
							appid: that.appid,
							secret: that.secret,
							js_code: js_code
						};
						getWechat(data).then(res => {
							console.log(res);
							if (res.data.code == 200) {
								uni.setStorageSync('openid', res.data.data.openid);
								uni.setStorageSync('session_key', res.data.data.session_key);
								console.log('openid:' + res.data.data.openid);
								console.log('session_key:' + res.data.data.session_key);
								let data = {
									encryptedData: e.detail.encryptedData,
									iv: e.detail.iv,
									nickName: '',
									session_key: res.data.data.session_key,
									openid: res.data.data.openid,
									phone: '',
									sex: 0
								};
								WXPhone(data).then(res => {
									console.log(res);
									that.phone = res.data.data;
								});
							} else {
								uni.showModal({
									title: '提示',
									content: '解密用户信息失败'
								});
							}
						});
					}
				});
			}else{
				uni.showModal({
					title: '提示',
					content: '获取手机号失败,登录需要手机号',
					showCancel: false
				});
			}
			
		  }
	},
	//页面初始加载
	mounted() {
		//缓存的账号
		const username = uni.getStorageSync('username');
		//缓存的密码
		const password = uni.getStorageSync('password');
		//有缓存就赋值给文本没有就清空
		if (username && password) {
			this.username = username;
			this.password = password;
		} else {
			this.username = '';
			this.password = '';
		}
	},
	onLaunch: function() {
		uni.getSystemInfo({
			success: function(e) {
				Vue.prototype.statusBar = e.statusBarHeight;
				// #ifndef MP
				if (e.platform == 'android') {
					Vue.prototype.customBar = e.statusBarHeight + 150;
				} else {
					Vue.prototype.customBar = e.statusBarHeight + 145;
				}
				// #endif

				// #ifdef MP-WEIXIN
				let custom = wx.getMenuButtonBoundingClientRect();
				Vue.prototype.customBar = custom.bottom + custom.top - e.statusBarHeight;
				// #endif

				// #ifdef MP-ALIPAY
				Vue.prototype.customBar = e.statusBarHeight + e.titleBarHeight;
				// #endif
			}
		});
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

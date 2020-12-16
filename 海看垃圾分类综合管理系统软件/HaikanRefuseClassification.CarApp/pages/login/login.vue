<template>
	<view class="content">
		<view class="header"><image src="../../static/image/垃圾分类系统头部.png" style="width:100%;"></image></view>

		<view class="list">
			<view class="list-call">
				<image class="img" src="/static/shilu-login/1.png"></image>
				<input class="biaoti" v-model="username" maxlength="11" placeholder="输入手机号或账号" />
			</view>
			<view class="list-call">
				<image class="img" src="/static/shilu-login/2.png"></image>
				<input class="biaoti" v-model="password" type="text" maxlength="32" placeholder="输入密码" password="true" />
			</view>
			<view class="list-call">
				<image class="img" src="/static/shilu-login/4.png"><view class="uni-form-item uni-column">
					<picker @change="typeChange" :range="array">
						<label>{{array[jq]}}</label>
					</picker>
				</view></image>
				<!-- <input class="biaoti" v-model="invitation" type="text" maxlength="12" placeholder="邀请码" /> -->
				
			</view>
		</view>

		<!-- 记住密码 -->
		<view class=" mui-input-row mui-checkbox ">
			<checkbox-group @change="checkboxChange">
				<checkbox id="chkRem" type="checkbox" :checked="rememberPsw" @tap="rememberPsw = !rememberPsw" color="#FFCC33" class="RememberCheck" style="transform:scale(0.7)" />
				记住密码
				<!-- <checkbox id="chkRem" type="checkbox" :checked="rememberPsw" @tap="rememberPsw = !rememberPsw" class="RememberCheck">
					记住密码
				</checkbox> -->
			</checkbox-group>
		</view>

		<view class="dlbutton" hover-class="dlbutton-hover" @tap="submitlogin()"><text>登录</text></view>

		<view class="xieyi">
			<navigator url="forget" open-type="navigate">忘记密码</navigator>
			<text>|</text>
			<navigator url="reg" open-type="navigate">注册账户</navigator>
		</view>
	</view>
</template>

<script>
var self;

var webapiaddress = null; //链接地址

import { mapState, mapMutation } from 'vuex';
import http from '@/components/utils/http.js';
export default {
	computed: mapState(['hasLogin']),
	data() {
		return {
			username: '',
			password: '',
			rememberPsw: true,
			array: [  '督导员','用户'],
			jq:0
		};
	},
	methods: {
		typeChange(e) {
			this.index=e.target.value;
			this.jq=this.index;
		},
		submitlogin() {
			if (this.username != '' && this.username != null && (this.password != '' && this.password != null)) {
				let opts = {
					url: 'api/oauth/auth',
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
							// uni.setStorage({
							// 	key:'token',
							// 	data:res.data.data,
							// 	success: function (){},
							// 	fail: function () {
							// 		console.log('缓存token失败')
							// 	}
							// })

							http.httpTokenRequest({
								url: 'api/v1/account/profile',
								method: 'get'
							}).then(res => {
								if (res.data.code == 200) {
									console.log(res.data);
									let user = {
										userName: res.data.data.user_name,
										userId: res.data.data.user_guid,
										userType: res.data.data.user_type,
										DepartmentName: res.data.data.user_departmentName,
										DepartmentGuid: res.data.data.user_departmentGuid,
										RoleName: res.data.data.user_roleName,
										token: token
									};
									this.$store.commit('login', user);
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
									uni.reLaunch({
										url: '../home/index'
									});
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
	/* 		background-image: url("/image/无头部.png"); */
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

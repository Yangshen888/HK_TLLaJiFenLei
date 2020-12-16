<template>
	<view class="content">
		<view class="header"><image src="https://ljfl.hztlcgj.com/image/head4.png" style="width:100%;"></image></view>

		<view class="list" style="padding-top:2rem">
			<view class="list-call">
				<image class="img" src="/static/shilu-login/1.png"></image>
				<input class="biaoti" v-model="phone" maxlength="11" placeholder="请先获取手机号" />
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
		<view><button class="dlbutton" hover-class="dlbutton-hover" type="default" @click="loginbtn">登录</button></view>
	</view>
</template>

<script>
var self;

var webapiaddress = null; //链接地址

import { mapState, mapMutation } from 'vuex';
import http from '@/components/utils/http.js';
import { GetUserInfo } from '@/api/user/userinfo.js';
import { WXAuth, WXPhone, geth5OpenAuth } from '@/api/appvue.js';
import { getWechat } from 'api/user/bind.js';

export default {
	// computed: mapState(['hasLogin']),
	data() {
		return {
			username: '',
			password: '',
			rememberPsw: true,
			array: ['督导员', '用户'],
			jq: 0,
			phone: ''
		};
	},
	methods: {
		typeChange(e) {
			this.index = e.target.value;
			this.jq = this.index;
		},
		loginbtn(){
			console.log(111)
			console.log(this.phone)
			if(this.phone==''){
				uni.showToast({ title: '请输入手机号！', icon: 'none' });
				return;
			}
			geth5OpenAuth(this.phone).then(res => {
				console.log('最开始:');
				console.log(res);
				if (res.data.code == 200) {
					let token = res.data.data.tokens;
					uni.setStorageSync('token', token);
					let user = {
						userName: res.data.data.user_name,
						userId: res.data.data.user_guid,
						userType: res.data.data.user_type,
						RoleName: res.data.data.roleName,
						token: token,
						phone: res.data.data.phone,
						openid: res.data.data.openid,
						HomeAddressUUID: res.data.data.homeAddressUUID,
						shopid:res.data.data.shop_guid,
						idCard:res.data.data.idCard,
					};
					this.$store.commit('login', user);
					//this.$store.state.phone = user.userName;
					this.$store.state.seltab = 1;
					document.cookie=("phone"+"="+this.phone+";path=/;");
					console.log('code=' + res.data.code);
					console.log(res);
						uni.reLaunch({
							url: '../home/index'
						});
				} else {
				}
			});
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
		document.cookie = "name=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
		document.cookie = "phone=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
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
	height: 200px;
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

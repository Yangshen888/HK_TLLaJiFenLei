<script>
import { getWXOpenAuth } from '@/api/appvue.js';
import { getWechat } from 'api/user/bind.js';

export default {
	data() {
		return {
			appid: 'wxda982699a0604145',
			secret: 'a070591fca4a02e9502125d8b0a4d87a'
		};
	},

	onLaunch: function() {
		console.log('App Launch');
	},
	onShow: function() {
		console.log('App Show');
	},
	onHide: function() {
		console.log('App Hide');
	},
	//页面初始加载
	mounted() {
		var that = this;
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
										shopid:res.data.data.shop_guid,
										idCard:res.data.data.idCard,
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
									// 	console.log(22);
										uni.reLaunch({
											url: '../home/index'
										});
									// }
								} else {
									// uni.showModal({
									// 	title: '提示',
									// 	content: res.data.message,
									// 	showCancel: false,
									// 	success: function(res) {
											
												// uni.reLaunch({
												// 	url: '../login/login'
												// });
											uni.navigateTo({
												url: '/pages/home/index'
											});
									// 	}
									// });
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
				// uni.request({
				// 	// url: 'https://api.weixin.qq.com/sns/jscode2session?appid=' + that.appid + '&secret=' + that.secret +
				// 	// 	'&js_code=' + js_code,
				// 	url:'',
				// 	header: {
				// 		"Content-Type": "application/x-www-form-urlencoded",
				// 	},
				// 	method: "post",
				// 	success: res => {
				// 		uni.setStorageSync("openid", res.data.openid);
				// 		console.log("openid:" + res.data.openid);
				// 		if (res.data.openid != "" && res.data.openid != null && res.data.openid != undefined) {
				// 			getWXOpenAuth(res.data.openid).then(res => {
				// 				console.log("最开始:");
				// 				console.log(res);
				// 				if (res.data.code == 200) {
				// 					let token = res.data.data.tokens;
				// 					uni.setStorageSync('token', token);
				// 					let user = {
				// 						userName: res.data.data.user_name,
				// 						userId: res.data.data.user_guid,
				// 						userType: res.data.data.user_type,
				// 						//DepartmentName: res.data.data.user_departmentName,
				// 						//DepartmentGuid: res.data.data.user_departmentGuid,
				// 						RoleName: res.data.data.roleName,
				// 						token: token,
				// 						phone:res.data.data.phone,
				// 						openid:res.data.data.openid,
				// 					};
				// 				    that.$store.commit('login', user);
				// 					//that.$store.state.phone = user.userName;
				// 					that.$store.state.seltab = 1;
				// 					console.log("code=" + res.data.code);
				// 					console.log(res);
				// 					if (res.data.data.address == "" || res.data.data.address == null) {
				// 						uni.reLaunch({
				// 							url: '../YongHu/addinfo'
				// 						});
				// 					} else {
				// 						console.log(22);
				// 						uni.reLaunch({
				// 							url: '../home/index'
				// 						});
				// 					}
				// 				} else {
				// 					uni.showModal({
				// 						title: '提示',
				// 						content: res.data.message,
				// 						success: function(res) {
				// 							if (res.confirm) {
				// 								console.log('用户点击确定');
				// 								uni.reLaunch({
				// 									url: '../login/bind?id=1'
				// 								});
				// 							} else if (res.cancel) {
				// 								console.log('用户点击取消');
				// 							}
				// 						}
				// 					});
				// 				}
				// 			});
				// 		} else {
				// 			uni.showModal({
				// 				title: '提示',
				// 				content: 'Openid获取失败,请重新获取!',
				// 				showCancel: false
				// 			});
				// 		}
				// 	}
				// })
			}
		});
	}
};
</script>

<style>
	@import "colorui/main.css";
	@import "colorui/icon.css";

	.nav-list {
		display: flex;
		flex-wrap: wrap;
		padding: 20px 40upx 0px;
		justify-content: space-between;
	}

	.nav-li {
		padding: 30upx;
		border-radius: 12upx;
		width: 45%;
		margin: 0 2.5% 40upx;
		/* background-image: url(https://cdn.nlark.com/yuque/0/2019/png/280374/1552996358352-assets/web-upload/cc3b1807-c684-4b83-8f80-80e5b8a6b975.png); */
		background-size: cover;
		background-position: center;
		position: relative;
		z-index: 1;
	}

	.nav-li::after {
		content: "";
		position: absolute;
		z-index: -1;
		background-color: inherit;
		width: 100%;
		height: 100%;
		left: 0;
		bottom: -10%;
		border-radius: 10upx;
		opacity: 0.2;
		transform: scale(0.9, 0.9);
	}

	.nav-li.cur {
		color: #fff;
		background: rgb(94, 185, 94);
		box-shadow: 4upx 4upx 6upx rgba(94, 185, 94, 0.4);
	}

	.nav-title {
		font-size: 32upx;
		font-weight: 300;
	}

	/* 	.nav-title::first-letter {
		font-size: 40upx;
		margin-right: 4upx;
	} */

	.nav-name {
		font-size: 28upx;
		text-transform: Capitalize;
		margin-top: 20upx;
		position: relative;
	}

	.nav-name::before {
		content: "";
		position: absolute;
		display: block;
		width: 40upx;
		height: 6upx;
		background: #fff;
		bottom: 0;
		right: 0;
		opacity: 0.5;
	}

	.nav-name::after {
		content: "";
		position: absolute;
		display: block;
		width: 100upx;
		height: 1px;
		background: #fff;
		bottom: 0;
		right: 40upx;
		opacity: 0.3;
	}

	.nav-name::first-letter {
		font-weight: bold;
		font-size: 36upx;
		margin-right: 1px;
	}

	.nav-li text {
		position: absolute;
		right: 30upx;
		top: 30upx;
		font-size: 52upx;
		width: 60upx;
		height: 60upx;
		text-align: center;
		line-height: 60upx;
	}

	.text-light {
		font-weight: 300;
	}

	@keyframes show {
		0% {
			transform: translateY(-50px);
		}

		60% {
			transform: translateY(40upx);
		}

		100% {
			transform: translateY(0px);
		}
	}

	@-webkit-keyframes show {
		0% {
			transform: translateY(-50px);
		}

		60% {
			transform: translateY(40upx);
		}

		100% {
			transform: translateY(0px);
		}
	}

	/*每个页面公共css */
</style>

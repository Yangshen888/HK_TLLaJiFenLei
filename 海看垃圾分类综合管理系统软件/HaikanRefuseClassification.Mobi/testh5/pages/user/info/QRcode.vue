<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">我的家庭码</block>
		</cu-custom>

		<view class="top" style="padding-top:2rem"></view>
		<view class="banner">
			<view class="family" v-show="cindex == 0">
				<view class="unbind" @click="untie">解绑</view>
				<view class="code">家庭积分码</view>
				<view class="img">
					<tki-qrcode
						cid="qrcode1"
						ref="qrcode"
						:val="val"
						:size="size"
						:unit="unit"
						:icon="icon"
						:iconSize="iconsize"
						:lv="lv"
						:onval="onval"
						:loadMake="loadMake"
						:usingComponents="true"
						@result="qrR"
						@longtap="save"
					/>
				</view>
				<view class="address">{{ address }}</view>
				<br />
			</view>
			<view class="family" v-show="cindex == 1">
				<view class="code1">家庭积分码</view>
				<view class="img" @click="saoma"><image src="https://ljfl.hztlcgj.com/image/scan1.png" mode=""></image></view>
				<view class="address">点击“扫一扫” 绑定家庭码</view>
				<br />
			</view>
			<view class="cu-bar bg-white margin-top" v-show="demo == 0" style="margin-bottom: 15px;">
				<view class="action">
					<text class="cuIcon-title text-orange "></text>
					市民卡（百姓码）
				</view>
				<view class="action"><button class="cu-btn bg-gradual-green shadow-blur round sqbtn" @tap="showModal" data-target="DialogModal1">申领</button></view>
			</view>
			<view class="cu-modal" :class="modalName == 'DialogModal1' ? 'show' : ''">
				<view class="cu-dialog" style="margin-top: 75px;margin-bottom: 20px;">
					<view class="cu-bar bg-white justify-end">
						<view class="content">电子百姓码开通协议</view>
						<view class="action" @tap="claseModal"><text class="cuIcon-close text-red"></text></view>
					</view>
					<view class="padding-xl" style="text-align: left;text-indent:20px">
						<p>您在申请桐庐县电子百姓码注册流程中，点击确认开通按钮之前，请务必认真阅读以下内容：</p>
						<br />
						<p>
							本协议是桐庐县数据资源管理局（以下简称“数管局”）与用户（以下或称为“您”）就使用桐庐县居民电子百姓码授权事宜的有效合约，本协议所称"桐庐县居民电子百姓码"（以下简称“本产品”）系指由桐庐数管局向您提供的电子产品。
						</p>
						<br />
						<p>
							您通过网络页面点击确认或以其他方式选择接受本协议，即表示您与数管局已达成协议并同意接受本协议的全部约定内容。如您不同意接受本协议的任意内容，或者无法准确理解相关条款含义的，请不要进行后续操作。
						</p>
						<br />
						<p>
							1.您具备完全民事行为能力。无民事行为能力或限制行为能力的用户，请务必在监护人的指导下阅读本协议和使用本产品。若您是中国大陆以外的用户，您还需同时遵守您所属国家或地区的法律，且您确认，订立并履行本协议不受您所属、所居住或开展经营活动或其他业务的国家或地区法律法规的限制。
						</p>
						<br />
						<p>
							2.开通本产品时，您需要向数管局提供您本人的信息，包括但不限于姓名、身份证号、手机号码等并授权数管局基于向您提供本产品相关服务使用您的个人信息。您授权数管局保留您在使用本产品期间所形成的相关信息数据，该授权在您终止使用本产品后法定期限内继续有效。您同意将您的信息传输到数管局客户端并进行展示。
						</p>
						<br />
						<p>3.数管局依据审核情况决定是否为您开通本产品，并有权决定增加、减少或变更本产品的功能。</p>
						<br />
						<p>
							4.您知晓并了解本产品的所有使用规则，并承诺遵照规则使用本产品，您不得通过不正当的手段或其他不公平的手段使用本产品和服务。您使用本产品时实施的所有行为均应当遵守国家法律、法规并符合社会公共利益或公共道德，且遵守包括但不限于本协议相关的规则、业务条款。因您违反本条或其他不合理使用所造成的一切责任，由您自行承担。
						</p>
						<br />
						<p>
							5.您应当保管好您的支付宝账户、密码及本产品信息，且您只能在您本人的支付帐户绑定本人的本产品，否则由此造成的责任，由您自行承担。如您发生手机遗失等情况应及时修改本产品的登陆密码，修改密码前发生的损失需由本人自行承担。
						</p>
						<br />
						<p>
							本协议的生效、解释、变更、执行与争议均适用中华人民共和国法律。因本协议产生之争议，均应依照中华人民共和国法律予以处理，并由数管局所在地有管辖权的人民法院管辖。
						</p>
					</view>
					<view class="cu-bar bg-white justify-end" style="margin-bottom: 20px;">
						<view class="action">
							<button class="cu-btn line-green text-green" @tap="claseModal">取消</button>
							<button class="cu-btn bg-green margin-left" @tap="hideModal">同意</button>
						</view>
					</view>
				</view>
			</view>
			<view class="citizen" v-show="demo == 1">
				<view class="ma">市民卡（百姓码）</view>
				<view class="search-form round commform"><input type="text" class="common" v-model="query.name" placeholder="证件姓名" style="height: 40px;" /></view>
				<view class="cu-form-group commgroup" style="height: 42px;">
					<view>证件类型</view>
					<picker @change="PickerChange" :value="index" :range-key="'name'" :range="cardtype">
						<view class="picker">{{ cardtype[index].name }}</view>
					</picker>
				</view>
				<view class="search-form round commform"><input type="text" class="common" v-model="this.idCard" placeholder="输入身份证号码" /></view>

				<view class="search-form commform" style="border-bottom: 1px solid #cbcbcb;border-top: 1px solid #cbcbcb;">
					<input type="text" class="common" v-model="query.tel" placeholder="手机号码" />
				</view>
				<view class="action getcom"><button class="cu-btn bg-gradual-green shadow-blur round sqbtn" @click="apply">申领</button></view>
			</view>
			<view class="citizen" v-show="demo == 2">
				<!-- <view class="img1" @click="citizencode"><image src="https://ljfl.hztlcgj.com/image/citizen.png" mode=""></image></view> -->
				<!-- <view class="img1" style="margin-bottom: 50px;padding: 5px;">
					<image src="../../../static/QRcode/baixinka.png" mode=""></image>
					<button class="smbtn" @click="smbtn">去刷码</button>
				</view> -->
				<view>
					<view style="overflow: hidden;margin-top: 5px;margin-left: 15px;">
						<image src="../../../static/QRcode/shimin.png" mode="" style="width: 22px;height: 22px;float: left;"></image>
						<text style="font-size: 15px;font-weight: 550;margin-top: 1px;float: left;margin-left: 5px;">市民卡</text>
						<view class="img1" style="margin-bottom: 35px;" @click="citizencode">
							<image src="https://ljfl.hztlcgj.com/image/citizen.png" mode="" style="width: 100%;margin-top: 10px;"></image>
						</view>
					</view>
				</view>
				<view>
					<view style="overflow: hidden;margin-top: 15px;margin-left: 10px;">
						<image src="../../../static/QRcode/baixin.png" mode="" style="width: 35px;height: 35px;float: left;"></image>
						<text style="font-size: 15px;font-weight: 550;margin-top: 8px;float: left;">百姓码</text>
						<view class="img1" style="margin-bottom: 50px;padding: 5px;padding-left: 10px;" @click="smbtn">
							<image src="../../../static/QRcode/baixinka.png" mode=""></image>
						</view>
					</view>
				</view>
				
			</view>
		</view>
	</view>
</template>

<script>
import qr from '@/components/tki-qrcode/tki-qrcode.vue';
import { UntieMa, commonman, GetCommon } from '@/api/supervisor/manager.js';
import { GetJiatinEWM } from '@/api/XFs.js';
import { setIDCard } from '@/api/user/userinfo.js';
import {geth5OpenAuth } from '@/api/appvue.js';
import { AddJiatinma } from '@/api/supervisor/manager.js';
export default {
	components: {
		qr
	},
	data() {
		return {
			result:'',//获取到的二维码内容，根据自己需求处理结果
			providerList: [],
			sourceLink: 'http://yunzhujiao.cn/bind_pub/index.html',
			type: 0,
			val: '1111', // 要生成的二维码值
			size: 400, // 二维码大小
			unit: 'upx', // 单位
			background: '#b4e9e2', // 背景色
			foreground: '#309286', // 前景色
			pdground: '#32dbc6', // 角标色
			icon: '', // 二维码图标
			iconsize: 40, // 二维码图标大小
			lv: 3, // 二维码容错级别 ， 一般不用设置，默认就行
			onval: false, // val值变化时自动重新生成二维码
			loadMake: false, // 组件加载完成后自动生成二维码
			src: '', // 二维码生成后的图片地址或base64
			address: '',
			cindex: 1,
			demo: 0,
			idCard: '',
			index: 0,
			index1: 0,
			modalName: null,
			count: '', //倒计时
			cardtype: [
				{
					name: '请选择',
					index: 0
				},
				{
					name: '居民身份证',
					index: '01'
				},
				{
					name: '居民户口簿',
					index: '02'
				},
				{
					name: '护照',
					index: '03'
				},
				{
					name: '军官证',
					index: '04'
				},
				{
					name: '驾驶证',
					index: '05'
				},
				{
					name: '港澳居民来往内地通行证',
					index: '06'
				},
				{
					name: '台湾居民来往内地通行证',
					index: '07'
				},
				{
					name: '出生医学证明',
					index: '08'
				},
				{
					name: '其他法定有效证件',
					index: '99'
				}
			],
			query: {
				name: '',
				idCardType: '',
				idCardValue: '',
				sex: '',
				tel: ''
			},
			msg: '',
			code: 0
		};
	},
	onLoad() {
		
		let namecookie=this.getCookie("name");
		let phonecookie=this.getCookie("phone");
		if(phonecookie!='' && this.$store.state.userId==''){
			this.loginbtn(phonecookie);
		}else{
			// console.log(this.$store)
			if (this.$store.state.userId == '') {
				uni.showModal({
					title: '提示',
					content: '请在登录后查看！',
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
				if (this.$store.state.idCard != '' && this.$store.state.idCard != null) {
					this.demo = 2;
				}
				if (this.$store.state.HomeAddressUUID == null) {
					this.cindex = 1;
					this.demo=0;
				} else {
					this.cindex = 0;
				}
				if (this.cindex == 0) {
					this.onval = true;
					this.loadMake = true;
					GetJiatinEWM(this.$store.state.HomeAddressUUID).then(res => {
						console.log(res);
						console.log(777777);
						if (res.data.code == 200) {
							this.val = res.data.data.split(',')[0];
							this.address = res.data.data.split(',')[1];
						}
					});
				}
			}
		}
		if(namecookie!='' && this.$store.state.userId!='' && this.cindex==1){
			this.GetQRcode(namecookie.split('h_')[1]);
		}
	},
	methods: {
		qrR(res) {
			this.src = res;
		},
		//获取cookie
		getCookie(cookieName) {
			var strCookie = document.cookie;
			var arrCookie = strCookie.split("; ");
			for(var i = 0; i < arrCookie.length; i++){
				var arr = arrCookie[i].split("=");
				if(cookieName == arr[0]){
					return arr[1];
				}
			}
			return "";
		},
		//再次登录
		loginbtn(phone){
			geth5OpenAuth(phone).then(res => {
				console.log('再次登录:');
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
					document.cookie = "phone=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
					uni.redirectTo({
						url: '/pages/user/info/QRcode'
					});
					console.log('code=' + res.data.code);
					console.log(res);
					
				} else {
				}
			});
		},
		//绑定家庭码
		GetQRcode(name){
			let that=this;
			let data = {
				guid: that.$store.state.userId,
				vuid: name
			};
			AddJiatinma(data).then(res => {
				if (res.data.code == 200) {
					if (res.data.message == '绑定成功') {
						that.$store.state.HomeAddressUUID=name;
						uni.showModal({
							title: '提示',
							content: '您已绑定成功！',
							showCancel: false,
							success: function(rest) {
								if (rest.confirm) {
									document.cookie = "name=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
									uni.redirectTo({
										url: '/pages/user/info/QRcode'
									});
								}
							}
						});
					}  else {
						uni.showModal({
							title: '提示',
							content: '绑定失败！',
							showCancel: false,
							success: function(rest) {
								if (rest.confirm) {
									uni.navigateTo({
										url: '/pages/user/info/QRcode'
									});
								}
							}
						});
					}
				} else {
					uni.showModal({
						title: '提示',
						content: '未查询到家庭码信息！',
						showCancel: false,
						success: function(rest) {
							if (rest.confirm) {
								
							}
						}
					});
				}
			});
		},
		showModal(e) {
			if (this.$store.state.HomeAddressUUID == null) {
				uni.showToast({
					title: '请先绑定家庭码',
					icon: 'none'
				});
				return;
			}
			this.modalName = e.currentTarget.dataset.target;
		},
		claseModal(e){
			this.modalName=null;
		},
		hideModal(e) {
			uni.showToast({ title: '5秒后进入...', icon: 'success' });
			const time_count = 5;
			if (!this.timer) {
				this.count = time_count;
				this.show = false;
				this.timer = setInterval(() => {
					if (this.count > 0 && this.count <= time_count) {
						this.count--;
					} else {
						this.show = true;
						clearInterval(this.timer);
						this.timer = null;
						this.demo=1;
						this.modalName=null;
					}
				}, 1000);
			}
		},
		saoma() {
			// uni.navigateTo({
			// 	url: '/pages/YongHu/jiatinma'
			// });
			window.location.href="../../../static/saoma.html"
		},
		smbtn() {
			// uni.scanCode({
			// 	success: function(res) {
			// 		let s = encodeURIComponent(res.result);
			// 		commonman(s).then(res => {
			// 			console.log(res);
			// 		});
			// 	}
			// });
			uni.navigateTo({
				url: '/pages/user/info/common'
			});
		},
		PickerChange(e) {
			this.index = e.detail.value;
			this.query.idCardType = this.cardtype[e.detail.value].index;
		},
		PickerChange1(e) {
			this.index1 = e.detail.value;
			this.query.sex = this.sex[e.detail.value].index;
		},
		citizencode() {
			// uni.navigateTo({
			// 	url: '/pages/user/info/code'
			// });
		},
		untie() {
			let data = {
				guid: this.$store.state.userId
			};
			UntieMa(data).then(res => {
				if (res.data.code == 200) {
					this.cindex = 1;
					this.demo=0;
					document.cookie=("phone"+"="+this.$store.state.phone+";path=/;");
					this.$store.state.HomeAddressUUID = null;
					uni.showToast({ title: '解绑成功！' });
				} else {
					uni.showToast({ title: '解绑失败！', icon: 'none' });
				}
			});
		},
		//保存图片到相册
		save() {
			let that = this;
			uni.showActionSheet({
				itemList: ['保存图片到相册'],
				success: () => {
					uni.saveImageToPhotosAlbum({
						filePath: that.src,
						success: function() {
							uni.showToast({
								title: '保存成功',
								icon: 'none'
							});
						}
					});
				},
				fail() {
					uni.showToast({
						title: '保存失败，请重试！',
						icon: 'none'
					});
				}
			});
		},
		apply() {
			if (this.$store.state.HomeAddressUUID == null) {
				uni.showToast({
					title: '请先绑定家庭码',
					icon: 'none'
				});
				return;
			}
			if (this.query.name == '') {
				uni.showToast({
					title: '请输入姓名',
					icon: 'none'
				});
				return;
			}
			if (this.query.idCardType == '') {
				uni.showToast({
					title: '请选择证件类型',
					icon: 'none'
				});
				return;
			}
			if (this.query.tel == '') {
				uni.showToast({
					title: '请输入手机号码',
					icon: 'none'
				});
				return;
			}
			let reg = /^[1-9]\d{5}(18|19|20|(3\d))\d{2}((0[1-9])|(1[0-2]))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$/;
			if (!reg.test(this.idCard)) {
				uni.showToast({
					title: '请输入有效的身份证号',
					icon: 'none'
				});
				return;
			}
			let regs = /^[1](([3][0-9])|([4][5-9])|([5][0-3,5-9])|([6][5,6])|([7][0-8])|([8][0-9])|([9][1,8,9]))[0-9]{8}$/;
			if (!regs.test(this.query.tel)) {
				uni.showToast({
					title: '请输入有效的手机号码',
					icon: 'none'
				});
				return;
			}
			this.query.idCardValue = this.idCard;
			GetCommon(this.query).then(res => {
				this.code = res.data.data.code;
				this.msg = res.data.data.msg;
				console.log(this.code);
				if (this.code != 1) {
					uni.showToast({
						title: this.msg,
						icon: 'none'
					});

					return;
				} else {
					let openid = uni.getStorageSync('openid');
					let data = {
						idCard: this.idCard,
						openid: openid,
						phone: this.query.tel,
						sex: this.query.sex,
						oldCard: this.query.idCardType,
						realName: this.query.name
					};
					setIDCard(data).then(res => {
						console.log(res);
						if (res.data.code == 200) {
							uni.showToast({
								title: res.data.message,
								icon: 'none'
							});
							this.demo = 2;
							this.$store.state.idCard = this.idCard;
						} else {
							uni.showToast({
								title: res.data.message,
								icon: 'none'
							});
						}
					});
				}
			});
		}
	}
};
</script>

<style>
body {
	font-family: sans-serif;
}
.content {
	width: 100%;
	/* background-color: #ffffff; */
}
.top {
	width: 100%;
	/* height:400upx; */
	/* background: url(http://pds.jyt123.com/wxtest/banner.png) no-repeat ; */
	background-size: 100%;
	background-position: center center;
}
.banner {
	width: 92%;
	background-color: #ffffff;
	border-radius: 15px 15px 15px 15px;
	position: absolute;
	overflow: visible;
	margin: 30px 15px;
}
.address {
	font-size: 32rpx;
	text-align: center;
	margin-top: 40rpx;
	margin-bottom: 20rpx;
}
.family {
	border-bottom: 1px dashed #999999;
	margin-left: 1rem;
	margin-right: 1rem;
}
.family .code {
	font-size: 40rpx;
	font-weight: 600;
	text-align: center;
	margin-top: 26rpx;
	margin-left: 80rpx;
	color: #1aad19;
}
.family .code1 {
	font-size: 40rpx;
	font-weight: 600;
	text-align: center;
	margin-top: 26rpx;
	color: #1aad19;
}
.family .unbind {
	color: #0294fa;
	float: right;
	margin-right: 37rpx;
}
.citizen {
	/* margin: 10px 0 0 10px; */
}
.citizen .ma {
	font-size: 35rpx;
	font-weight: 550;
	margin-left: 19px;
	margin-top: 6px;
}
.img {
	width: 400upx;
	height: 400upx;
	margin: 0 auto;
	margin-top: 60upx;
	text-align: center;
}
.img image {
	width: 100%;
	height: 100%;
	margin-top: 20rpx;
}
.img1 {
	width: 97%;
	height: 85px;
	padding: 20upx 0 270upx -10px;
	text-align: center;
}
.img1 image {
	width: 100%;
	height: 100%;
	margin-top: 20rpx;
	/* margin-left: 35rpx; */
}
.img1 .shi,
.img1 .bai {
	margin-top: -65px;
	float: left;
	margin-left: 10px;
}
.img1 .shi image,
.img1 .bai {
	width: 35px;
	height: 35px;
}
.smbtn {
	margin-top: -2px;
	margin-left: 14px;
	width: 100%;
	height: 26px;
	font-size: 12px;
	color: #8a8a8a;
	border-radius: 0 0 10px 10px;
}
.btn {
	width: 260upx;
	height: 60upx;
	line-height: 60upx;
	margin: 0 auto;
	margin-top: 60upx;
	border-radius: 40upx;
	border: 0;
	font-size: 26upx;
	outline: 0;
	background: #33b17b;
	color: #ffffff;
	text-align: center;
}
.bottom {
	width: 100%;
	height: 400upx;
	/* 	background: url(../../static/img/beij.png) no-repeat; */
	background-position: left bottom; /* 设置背景图片位置 */
	background-size: 20%;
}
.bottom ul {
	padding-left: 40upx;
	box-sizing: border-box;
}
.bottom ul li {
	width: 100%;
	height: 60upx;
}
.sqbtn {
	margin-top: 0;
}

.shi .code,
.bai .code {
	float: left;
	z-index: 999;
	position: fixed;
	margin-left: 60px;
	margin-top: -30px;
	font-size: 16px;
	font-weight: bold;
}
.common {
	height: 42px;
}
.commgroup {
	color: #808080;
	width: 86%;
	border-top: 1px solid #cbcbcb;
	border-bottom: 1px solid #cbcbcb;
	margin-left: 5%;
}
.commform {
	padding-left: 4%;
	margin-left: 5%;
	width: 86%;
}
.getcom {
	width: 100%;
	text-align: center;
}
.getcom button {
	width: 70px;
	margin: 5px;
}
.content11{
		width: 100%;
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
	}
.btn11{
	width: 50%;
}
</style>

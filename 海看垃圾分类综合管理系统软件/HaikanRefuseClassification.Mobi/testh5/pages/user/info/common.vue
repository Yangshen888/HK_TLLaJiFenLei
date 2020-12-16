<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">我的百姓码</block>
		</cu-custom>

		<view class="top" style="padding-top:2rem"></view>
		<view class="banner">
			<view class="family">
				<view class="code">百姓码</view>
				<view class="img">
					<tki-qrcode
						cid="qrcode2"
						ref="qrcode"
						:icon="icon"
						:iconsize="iconsize"
						:val="val"
						:size="size"
						:onval="onval"
						:loadMake="loadMake"
						:usingComponents="true"
						:foreground="foreground"
						@result="qrR"
						@longtap="save"
					/>
				</view>
				<view class="address">当前时间为：{{ time }}</view>
				<view class="address" style="margin-bottom: 20px;">{{ valdata }}分钟后刷新</view>
				<br />
			</view>
		</view>
	</view>
</template>

<script>
import qr from '@/components/tki-qrcode/tki-qrcode.vue';
import { GetCommonEWM } from '@/api/supervisor/manager.js';
import { formateDate } from '@/global/utils.js';
export default {
	components: {
		qr
	},
	data() {
		return {
			providerList: [],
			sourceLink: 'http://yunzhujiao.cn/bind_pub/index.html',
			type: 0,
			val: '1111', // 要生成的二维码值
			size: 400, // 二维码大小
			unit: 'upx', // 单位
			background: '#b4e9e2', // 背景色
			// foreground: '#309286', // 前景色
			foreground: '#1aad19', // 前景色
			pdground: '#32dbc6', // 角标色
			icon: '../../../../../static/shilu-login/ewmlogo.jpg', // 二维码图标
			iconsize: 40, // 二维码图标大小
			lv: 3, // 二维码容错级别 ， 一般不用设置，默认就行
			onval: true, // val值变化时自动重新生成二维码
			loadMake: true, // 组件加载完成后自动生成二维码
			src: '', // 二维码生成后的图片地址或base64
			valdata:0,
			time: formateDate(new Date(), 'h:min:s'), //当前时分秒
		};
	},
	onLoad() {
		GetCommonEWM(this.$store.state.userId).then(res => {
			if (res.data.code == 200) {
				this.val = res.data.data.split(',')[0];
				this.valdata=res.data.data.split(',')[1];
				this.goonema();
			}
		});
		this.getTime();
	},
	methods: {
		qrR(res) {
			this.src = res;
		},
		goonema(){
			if(!this.timer){
				this.timer=setInterval(()=>{
						GetCommonEWM(this.$store.state.userId).then(res => {
							if (res.data.code == 200) {
								this.val = res.data.data.sqlit(',')[0];
								this.valdata=res.data.data.split(',')[1];
							}
						});
				},60000*this.valdata)
			}
		},
		// 实时时间
		getTime() {
			var that = this;
			setInterval(function() {
				that.time = formateDate(new Date(), 'h:min:s');
			}, 1000);
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
		}
	}
};
</script>

<style>
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
	/* color: #1aad19; */
}
.family {
	/* height: 370px; */
}
.family .code {
	font-size: 48rpx;
	font-weight: 500;
	text-align: center;
	margin-top: 65rpx;
	/* color: #1aad19; */
}
.img {
	height: 400upx;
	margin-top: 125upx;
	text-align: center;
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
</style>

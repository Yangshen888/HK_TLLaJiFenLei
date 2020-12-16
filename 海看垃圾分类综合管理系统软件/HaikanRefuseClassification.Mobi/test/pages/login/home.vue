<template>
	<view style="padding-top:2rem">
		<!-- <view class="top"></view> -->
		<view class="banner">
			<image style="width: 100%;height: 100%;" src="https://ljfl.hztlcgj.com/index.png" ></image>
			 <!--<video
				id="myVideo"
				src="https://ljfl.hztlcgj.com/bdjtmlc.mp4"
				style="width: 100%;height: 111%;margin-top: -10%;display: flex;"
				:autoplay="true"
				:direction="0"
				object-fit="fill"
				@error="videoErrorCallback"
				@ended="endv"
			></video> -->
		</view>
		<!--<view class="btn-wrap"><button type="primary" class="btn-transparent" size="mini" @click="endv">跳过</button></view> -->
	</view>
</template>

<script>
import qr from '@/components/tki-qrcode/tki-qrcode.vue';
import uniNavBar from '@/components/uni-nav-bar/uni-nav-bar.vue';
import { ewm } from '@/api/XFs.js';
export default {
	components: {
		qr,
		uniNavBar
	},
	data() {
		return {};
	},
	onLoad() {
		// let videoContext = uni.createVideoContext('myVideo');
		// videoContext.requestFullScreen();
	},
	methods: {
		endv() {
			uni.redirectTo({
				url: '/pages/home/index'
			});
		},
		videoErrorCallback: function(e) {
			uni.showModal({
				content: e.target.errMsg,
				showCancel: false
			});
		}
	}
};
</script>

<style>
.tugurl {
	color: #999;
}
.sharbuttn {
	display: flex;
	justify-content: center;
}
.shartitle {
	width: 80%;
	text-align: center;
	margin-left: 10%;
	border-bottom: 1px solid #dddddd;
	position: relative;
	height: 60upx;
}
.tgtit {
	text-align: center;
}
.shartitle view {
	height: 50upx;
	line-height: 50upx;
	font-size: 28upx;
	color: #999;
	width: 120upx;
	margin: 32upx auto;
	position: absolute;
	background: #fff;
	left: 50%;
	margin-left: -60upx;
}
.sharapk {
	display: flex;
	justify-content: center;
	margin: 20upx auto;
}
.sharapk view {
	margin: 40upx;
}
.sharapk view image {
	height: 80upx;
	width: 80upx;
}
.content {
	width: 100%;
	/* background-color: #ffffff; */
}
.top {
	width: 100%;
	height: 400upx;
	/* background: url(http://pds.jyt123.com/wxtest/banner.png) no-repeat ; */
	background-size: 100%;
	background-position: center center;
}
.banner {
	display: flex;
	justify-content: center;
	width: 100%;
	height: 100%;
	background-color: #ffffff;
	/* background-image: url(../../static/image/index.png); */
	/* border-radius: 30px 30px 0 0;
		    margin-top: -30px; */
	position: absolute;
	/* top: 280px; */
	bottom: 0;
}
.banner dl {
	margin-top: -80upx;
}
.banner dl dt {
	text-align: center;
}
.banner dl dt image {
	width: 160upx;
	height: 160upx;
	border-radius: 50%;
}
.banner dl dd {
	text-align: center;
}
.img {
	width: 400upx;
	height: 400upx;
	/* background-color: red; */
	margin: 0 auto;
	margin-top: 60upx;
	text-align: center;
}
.img image {
	width: 100%;
	height: 100%;
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
.btn-wrap {
	position: fixed;
	right: 30rpx;
	top: 8vh;
}
</style>

<template>
	<view>
		<!-- <swiper class="screen-swiper" :class="dotStyle?'square-dot':'round-dot'" :indicator-dots="true" :circular="true"
			 :autoplay="true" interval="5000" duration="500">
				<swiper-item v-for="(item,index) in swiperList" :key="index">
					<image :src="item.url" mode="aspectFill" v-if="item.type=='image'"></image>
					<video :src="item.url" autoplay loop muted :show-play-btn="false" :controls="false" objectFit="cover" v-if="item.type=='video'"></video>
				</swiper-item>
	</swiper> -->
		<view class="padding-xs flex align-center">
			<!-- <view class="padding">18</view> -->
			<view class="flex-sub text-center">
				<view class="solid-bottom text-xl padding">
					<text class="text-white text-bold">桐庐垃圾分类系统</text>
				</view>
				<!-- <view class="padding">页面大标题，用于结果页等单一信息页</view> -->
			</view>
		</view>

		<view class="nav-list">
			<navigator hover-class="none" :url="item.url" class="nav-li bg-color" navigateTo :style="[{animation: 'show ' + ((index+1)*0.2+1) + 's 1'}]"
			 v-for="(item,index) in elements" :key="index">
				<view class="nav-title">{{item.title}}</view>
				<!-- 			<view class="nav-name">{{item.name}}</view> -->
				<view class="select"></view>
				<!-- <text :class="'cuIcon-' + item.cuIcon" :style="'color:'+item.color"></text> -->
			</navigator>
		</view>

	</view>
</template>

<script>
	export default {

		data() {
			return {
				elements: [{
						title: '实时地图',
						name: 'Map',
						color: 'yellow',
						cuIcon: 'location',
						url: '/pages/map/liveMap/liveMap'
					},
					{
						title: '问题反馈',
						name: 'Feedback',
						color: 'pink',
						cuIcon: 'question',
						url: '/pages/feedback/feedback'
					},
					{
						title: '称重登记复核',
						name: ' Register ',
						color: 'blue',
						cuIcon: 'edit',
						url: '/pages/register/register'
					},
					// 	{
					// 		title: '收运记录',
					// 		name: 'Record',
					// 		color: 'green',
					// 		cuIcon: 'copy',
					// url:'/pages/record/record'
					// 	},
					{
						title: '系统信息',
						name: 'SystemInfo',
						color: 'green',
						cuIcon: 'copy',
						url: '/pages/system/info/info'
					}
				],
				swiperList: [{
						id: 0,
						type: 'image',
						url: '/static/image/垃圾分类系统头部.png'
					},
					{
						id: 1,
						type: 'image',
						url: '/static/image/垃圾分类系统头部.png'
					}
				],
				dotStyle: false,
			};
		},
		mounted() {

			this.TowerSwiper('swiperList');
			// 初始化towerSwiper 传已有的数组名即可
		},
		methods: {
			// 初始化towerSwiper
			TowerSwiper(name) {
				let list = this[name];
				for (let i = 0; i < list.length; i++) {
					list[i].zIndex = parseInt(list.length / 2) + 1 - Math.abs(i - parseInt(list.length / 2))
					list[i].mLeft = i - parseInt(list.length / 2)
				}
				this.swiperList = list
			},
		}
	}
</script>

<style>
	button {
		margin-top: 40upx;
	}

	.header {}

	.header image {
		width: -webkit-fill-available;
		height: 160px;
	}
	
	.nav-title {
		/* font-weight: 600; */
		font-size: 24upx;
		padding-left: 35upx;
		float: left;
		color: #fff;
	}
	
	/* 背景样式,勿动 */
	.nav-list .bg-color {
		background: #000;
		padding: 0;
	/* 	height: 18vw;
		line-height: 18vw; */
		height: 130upx;
		max-height: 280px;
		line-height: 130upx;
	}
	.nav-list .bg-color:nth-child(1) {
		background: linear-gradient(to right,#c745ff,#6a19d7);
	}
	.nav-list .bg-color:nth-child(2) {
		background: linear-gradient(to right,#5c71e4,#671bdd);
	}
	.nav-list .bg-color:nth-child(3) {
		background: linear-gradient(to right,#01cdff,#0071d9);
	}
	.nav-list .bg-color:nth-child(4) {
		background: linear-gradient(to right,#30c295,#01686f);
	}

	
	
	/* 图片样式,勿动 */
	.nav-list .bg-color .select{
		float: right;
		width: 100px;
		height: 100px;
		margin: 35upx 50px 0 0;
	}
	.nav-list .bg-color:nth-child(1) .select{
		background: url('@/static/image/map.png') no-repeat center center;
		background-position:50%;
		background-size:cover;
	}
	.nav-list .bg-color:nth-child(2) .select{
		background: url('@/static/image/feedback.png') no-repeat center center;
		background-position:50%;
		background-size:cover;
	}
	.nav-list .bg-color:nth-child(3) .select{
		background: url('@/static/image/weigh.png') no-repeat center center;
		background-position:50%;
		background-size:cover;
		width: 110px;
	}
	.nav-list .bg-color:nth-child(4) .select{
		background: url('@/static/image/message.png') no-repeat center center;
		background-position:50%;
		background-size:cover;
		height: 120px;
		margin: 30upx 50px 0 0;
	}
</style>

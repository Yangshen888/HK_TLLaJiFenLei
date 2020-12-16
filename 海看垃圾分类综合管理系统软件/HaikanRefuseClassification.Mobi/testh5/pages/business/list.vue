<template>
	<view style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">商店信息</block>
		</cu-custom>
				<view style="font-size: 18px;text-align: center;line-height: 200px;">敬请期待...</view>
			<!-- <div class="main-body bg-white mh100" style="width: 100%;"> -->
				<!-- <view class="nav">
					<view class='searchInput999'>
						<view class='searchBox999'>
							<image src='/static/icon-search.png' class='search999'></image>
						</view>
						<input class='input999' v-model="kw" placeholder="输入关键词" @change='LoadData'></input>
					</view>
				</view> -->
				<!-- <div class="list-side" :style="{'top':sideTop+'px'}">
		
					<div @click="setCat(item.shopUuid)" v-for="(item,index) in pageData"  :key="index" class="list-side-item" v-bind:class="{'list-side-item-active':catid==item.shopUuid}">{{item.shopName}}</div>
		
				</div>
				<div class="list-main" v-bind:style="{'min-height':height+'upx'}">
		
					<div v-for="(item,index) in pageData" :key="index" class="list-cat-item">
						<div v-if="item.shopUuid==catid"> -->
							<!-- <div @click="goList(item.shopUuid)">
								<image class="list-cat-img" mode="widthFix" :src="item.address+'.middle.jpg'" style="margin-top: 10px;"></image>
							</div> -->
							<!-- <image class="list-cat-img" mode="widthFix" src="../../static/QRcode/logo.png"></image>
							<div class="list-cat-hd">{{item.shopName}}</div>
							<div class="list-child-title">商店名称：{{item.shopName}}</div>
							<div class="list-child-title">商店位置：{{item.address}}</div> -->
							<!-- <div class="list-child">
		
								<div v-for="(cc,ccIndex) in item.child" :key="ccIndex" @click="goList(cc.shopUuid)" class="list-child-item">
									<image class="list-child-img" mode="widthFix" :src="cc.imgurl+'.middle.jpg'"></image>
									<div class="list-child-title">{{cc.shopName}}</div>
								</div>
							</div> -->
					<!-- 	</div>
					</div>
		
		
				</div> -->
			<!-- </div> -->
	</view>
</template>

<script>
	import http from '@/components/utils/http.js'
	import {getShoplist} from '@/api/supervisor/manager.js'
	export default {

		data: function() {
			return {
			 
				pageLoad: false,
				pageData: [],
				catid:0,
				height:440,
				sideTop:0
			}
		},
		onLoad() {
			if (this.$store.state.userId == '') {
				uni.showModal({
					title: '提示',
					content: '请在微信授权后查看！',
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
			var win=uni.getSystemInfoSync();
			this.height=win.windowHeight-50;
			// #ifndef H5
			this.sideTop=0;
			// #endif
			this.getPage();
			}
		},
		methods: {
			setCat:function(catid){
				this.catid=catid;
			},
			goList:function(catid){
				// uni.navigateTo({
				// 	url:"?catid="+catid
				// })
			},
			getPage() {
				var that = this;
				getShoplist().then(res=>{
					if(res.data.code==200)
					{
						that.pageData = res.data.data;
						console.log(that.pageData);
						for(var i in res.data.data){
								that.catid=res.data.data[i].shopUuid;
								break;
							}
							that.pageLoad = true;
					}else{
						uni.showModal({
							title: '提示',
							content: res.data.message,
							showCancel: false
						})
					}
				})
				
			}
		}
	}
</script>

<style scoped>
 
.bg-white{
	    position: absolute;
	    background-color: #ffffff;
	    color: #666666;
	    height: 100%;
}
.list-side {
	position: absolute;
	left: 0upx;
	top: 0px;
	/* bottom: 130upx; */
	width: 170upx;
	border-right: 1px solid #eee;
	text-align: center;
	background-color: #fff;
	/* height:100%; */
}

.list-side-item {
	color: #707070;
	padding: 22upx 11upx;
	font-size: 14px;
	display: block;
	cursor: pointer;
}

.list-side-item-active {
	color: #ff842b;
	border-left: 7upx solid #ff842b;
}

.list-main {
	margin-left: 180upx;
	/* padding-top: 70px; */
}

.list-cat-img {
	max-width: 100%;
	padding-right: 11upx;
}

.list-cat-hd {
	text-align: center;
	padding: 22upx 0;
	color: #666;
	font-size: 14px;
	position: relative;
}

.list-cat-hd:before {
	display: block;
	width: 44upx;
	height: 2upx;
	background-color: #e0e0e0;
	position: absolute;
	right: 60%;
	top: 50%;
	content: "-";
	overflow: hidden;
	color: #666;
}

.list-cat-hd:after {
	display: block;
	width: 44upx;
	height: 2upx;
	background-color: #e0e0e0;
	position: absolute;
	left: 60%;
	top: 50%;
	content: "-";
	overflow: hidden;
	color: #666;
}

.list-child {
	margin-bottom: 22upx;
	flex-direction: row;
	flex-wrap: wrap;
}

.list-child-item {
	float: left;
	display: block;
	width: 33.333%;
	margin-bottom: 22upx;
	padding: 0 22upx;
	box-sizing: border-box;
	font-size: 14px;
	color: #666;
	text-align: center;
}

.list-child-img {
	width: 100%;
	height: 60upx;
	display: block;
	margin-bottom: 11upx;
}

.list-child-title {
	height: 66upx;
	line-height: 66upx;
	overflow: hidden;
}
.nav {
		position: absolute;
		left: 0;
		top: 0px;
		color: white;
		width: 100%;
		display: flex;
		flex-direction: column;
		align-items: flex-start;
		justify-content: flex-start;
		font-size: 24upx;
		background-color: #50B7EA;
		z-index: 99999996;
		padding:12px 0px;
	}
	
	.searchInput999 {
		width: 90%;
		margin: 0 auto;
		background: white;
		border-radius: 30upx;
		display: flex;
		align-items: center;
		justify-content: center;
		height: 56upx;
	}
	
	.search999 {
		width: 32upx;
		height: 32upx;
	}
	
	.searchBox999 {
		width: 56upx;
		height: 56upx;
		display: flex;
		justify-content: center;
		align-items: center;
	}
	
	.input999 {
		color: #999;
		width: 80%;
	}
 
</style>

<template>
	<view class="">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index" style="z-index: 9999999;position: fixed;width:100%;">
			<block slot="backText">返回</block>
			<block slot="content">分类查询</block>
		</cu-custom>
		<view class="container999" @touchstart="refreshStart" @touchmove="refreshMove" @touchend="refreshEnd">
			<!-- 刷新组件需搭配scroll-view使用，并在pages.json中添加 "disableScroll":true-->
			<refresh ref="refresh" @isRefresh='isRefresh'></refresh>
			<view class='nav'>
					<view style="height: 44px;width: 100%;"></view>
				<!-- 搜索 -->
				<view class='searchInput999'>
					<view class='searchBox999'>
						<image src='/static/icon-search.png' class='search999'></image>
					</view>
					<input class='input999' v-model="kw" placeholder="输入关键词" @change='LoadData'></input>
				</view>
				<!-- 导航栏 agents导航栏标题 -->
				<navTab ref="navTab" :tabTitle="tabTitle" @changeTab='changeTab'></navTab>
			</view>
			<!-- swiper切换 swiper-item表示一页 scroll-view表示滚动视窗 -->
			<swiper style="min-height: 100vh;" :current="currentTab" @change="swiperTab">
				<swiper-item v-for="(listItem,listIndex) in list" :key="listIndex">
					<scroll-view style="height: 100%;" scroll-y="true" @scrolltolower="lower1" scroll-with-animation :scroll-into-view="toView">
					<view :id="'top'+listIndex" style="width: 100%;height: 115px;"></view>
					<view class='content'>
						<view class='card'>
							
						</view>
						<view class='cardSub' v-for="(item,index) in listItem" v-if="listItem.length > 0" :key="index">
							{{item.grabName}}
						</view>
					</view>
					<view class='noCard' v-if="listItem.length===0">
						暂无信息
					</view>
					<view style="width: 100%;height: 100upx;opacity:0;">底部占位盒子</view>
					</scroll-view>
				</swiper-item>
			</swiper>

		</view>
		
	</view>
</template>

<script>
const util = require('../../global/navtab.js');
import refresh from '@/components/nav-tab/refresh.vue';
import navTab from '@/components/nav-tab/navTab.vue';
import {getGrabTypeList} from '../../api/grabage/GrabList.js'

export default {
	components: {refresh,navTab},
	data() {
		return {
			currentPage:'index',
			toView:'',//回到顶部id
			tabTitle:['可回收物','有害垃圾','易腐垃圾','其他垃圾'], //导航栏格式 --导航栏组件
			currentTab: 0, //sweiper所在页
			pages:[1,1,1,1], //第几个swiper的第几页
			//数据
			list: [
				[1, 2, 3, 4, 5, 6],
			    ['a', 'b', 'c', 'd', 'e', 'f'],
			    [],
			    ['2233','4234','13144','324244'],
			] ,//数据格式
			listTitle:[
				{
					title:"可回收物",
				    img:"",
					content:"可回收物就是可以再生循环的垃圾。本身或材质可再利用的纸类、硬纸板、玻璃、塑料、金属、塑料包装，与这些材质有关的如：报纸、杂志、广告单及其它干净的纸类等皆可回收。",
					YQ:["可回收物应轻投轻放，清洁干燥、避免污染","废纸尽量平整","立体包装物请清空内容物，清洁后压扁投放","有尖锐边角的，应包裹后投放"],//投放要求
					BK:"废纸，塑料，玻璃，易拉罐，毛巾等",//包括
				}
			],
			kw:""
		};
	},
	onLoad(e) {
		this.LoadData();
	},
	onShow() {},
	onHide() {},
	methods: {
		LoadData(){	
			getGrabTypeList({kw:this.kw}).then(res=>{
				console.log(res);
				this.list=res.data.data;
			});
		},
		toTop(){
			this.toView = ''
			setTimeout(()=>{
				this.toView = 'top' + this.currentTab
			},10)
		},
		changeTab(index){
			this.currentTab = index;
		},
		// 其他请求事件 当然刷新和其他请求可以写一起 多一层判断。
		isRequest() {
			return new Promise((resolve, reject) => {
				this.pages[this.currentTab]++
				var that = this
				setTimeout(() => {
					uni.hideLoading()
					uni.showToast({
						icon: 'none',
						title: `请求第${that.currentTab + 1 }个导航栏的第${that.pages[that.currentTab]}页数据成功`
					})
					let newData = ['新数据1','新数据2','新数据3']
					resolve(newData)
				}, 1000)
			})
		},
		// swiper 滑动
		swiperTab: function(e) {
			var index = e.detail.current //获取索引
			this.$refs.navTab.longClick(index);
		},
		//加载更多 util.throttle为防抖函数
		lower1: util.throttle(function(e) {
		console.log(`加载${this.currentTab}`)//currentTab表示当前所在页数 根据当前所在页数发起请求并带上page页数
		uni.showLoading({
			title: '加载中',
			mask:true
		})
			this.isRequest().then((res)=>{
				let tempList = this.list
				tempList[this.currentTab] = tempList[this.currentTab].concat(res)
				console.log(tempList)
				this.list = tempList
				this.$forceUpdate() //二维数组，开启强制渲染
			})
		}, 300),
		// 刷新touch监听
		refreshStart(e) {
			this.$refs.refresh.refreshStart(e);
		},
		refreshMove(e){
			this.$refs.refresh.refreshMove(e);
		},
		refreshEnd(e) {
			this.$refs.refresh.refreshEnd(e);
		},
		isRefresh(){
				setTimeout(() => {
					uni.showToast({
						icon: 'success',
						title: '刷新成功'
					})
					this.$refs.refresh.endAfter() //刷新结束调用
				}, 1000)
		},
		
	},
	mounted(){
		
	}
};
</script>

<style lang="scss">
	
	.container999 {
	  width: 100vw;
	  font-size: 28upx;
	  min-height: 100vh;
	  overflow: hidden;
	  color: #6B8082;
	  position: relative;
	  top:50px;
	  background-color: #f6f6f6;
	}
	.content {
		width: 100%;
	}
	
	.card {
		width: 90%;
		height: 450upx;
		background-color: white;
		margin: 0 auto 42upx auto;
		background: #FFFFFF;
		box-shadow: 0 0 5px 0 rgba(0, 0, 0, 0.10);
		border-radius: 5px;
		position: relative;
	}
	.cardSub {
		width: 90%;
		height: 100upx;
		background-color: white;
		line-height:100upx;
		text-align:center;
		margin: 0 auto 42upx auto;
		background: #FFFFFF;
		box-shadow: 0 0 5px 0 rgba(0, 0, 0, 0.10);
		border-radius: 5px;
		position: relative;
	}
	.noCard {
		width: 90%;
		height: 200upx;
		margin: auto;
		background-color: white;
		display: flex;
		align-items: center;
		justify-content: center;
		color: #999999;
		box-shadow: 0 0 10upx 0 rgba(0, 0, 0, 0.10);
		border-radius: 10upx;
	}
	
	
	.nav {
		position: fixed;
		left: 0;
		top: 30px;
		color: white;
		width: 100%;
		display: flex;
		flex-direction: column;
		align-items: flex-start;
		justify-content: flex-start;
		font-size: 24upx;
		background-color: #50B7EA;
		z-index: 96;
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

<template>
	<view class="" >
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index" style="z-index: 9999999;position: fixed;width:100%;">
			<block slot="backText">返回</block>
			<block slot="content">分类查询</block>
		</cu-custom>
		<view class="container999" >
			<!-- 刷新组件需搭配scroll-view使用，并在pages.json中添加 "disableScroll":true-->
			<!-- <refresh ref="refresh" @isRefresh='isRefresh'></refresh> -->
			<view class='nav'>
					<view style="height: 15px;width: 100%;"></view>
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
					<scroll-view style="height: 100%;" scroll-y="true"  scroll-with-animation :scroll-into-view="toView">
					<view :id="'top'+listIndex" style="width: 100%;height: 115px;"></view>
					<view class='content'>
						<view class='card'>
							<view class='img'>
								<image :src="listTitle[listIndex].img"></image>
							</view>
							<view style='text-indent: 2em;line-height: 50upx;'>
								{{listTitle[listIndex].content}}
							</view>
							<!-- <view style='padding-left:50upx'>
								<text class='tt'>主要包括：</text>
								<view>
									{{listTitle[listIndex].BK}}
								</view>
							</view> -->
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
				    img:"../../static/grabagecan/3.png",
					content:"可回收物指未污染的适宜回收和资源利用的生活垃圾，如玻璃、金属、塑料（橡胶）、纸类、纺织类、小家电。主要有玻璃、金属、塑料、纸类、纺织类、小家电。",
					BK:"废纸，塑料，玻璃，易拉罐，毛巾等",//包括
				},
				{
					title:"有害垃圾",
				    img:"../../static/grabagecan/1.png",
					content:"有害垃圾指对人体健康或者自然环境造成直接或者潜在危害的生活垃圾。主要有充电电池、纽扣电池、灯管、弃置药品、杀虫剂油漆、硒鼓、水银产品等。",
					YQ:["废灯管等易破损的有害垃圾应连带包装或包裹后投放","废弃药品宜连带包装一并投放","杀虫剂等压力罐装容器，应排空内容物后投放"],//投放要求
					BK:"常见包括废电池、废荧光灯管、废灯泡、废水银温度计、废油漆桶、过期药品等",//包括
				},
				{
					title:"易腐垃圾",
				    img:"../../static/grabagecan/4.png",
					content:"易腐垃圾指餐饮经营者、单位食堂等生产过程中产生的餐厨废弃物，居民家庭生活中产生的厨余垃圾和集贸市场产生的生鲜垃圾等。主要有剩菜剩饭与西餐糕点等食物残余、菜梗菜叶、肉食内脏、茶叶渣、水果残余、果壳瓜皮、家用绿植、花卉等植物的残枝落叶、食用油等。",
					BK:"树枝花草、腐肉、肉碎骨、蛋壳、蔬菜瓜果垃圾、畜禽类动物内脏等",//包括
				},
				{
					title:"其他垃圾",
				    img:"../../static/grabagecan/2.png",
					content:"其他垃圾指可回收物，有害垃圾和易腐垃圾之外的其他生活垃圾。主要有受污染与无法再生的纸张，朔料袋与其他受污染的塑料制品、破旧陶瓷器、难以自然降解的食物残余，妇女卫生用品、烟头、少量尘土等。",
					BK:"陶瓷、渣土、卫生间废纸、瓷器碎片等难以回收的废弃物",//包括
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
			if(this.kw!=""){
				uni.navigateTo({
				  url:'/pages/garbage/querylist?kw='+this.kw,
				});
			}
			
			
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
						title: `请求数据成功`
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
		//@scrolltolower="lower1"
		//@touchstart="refreshStart" @touchmove="refreshMove" @touchend="refreshEnd"
		// lower1: util.throttle(function(e) {
		// console.log(`加载${this.currentTab}`)//currentTab表示当前所在页数 根据当前所在页数发起请求并带上page页数
		// uni.showLoading({
		// 	title: '加载中',
		// 	mask:true
		// })
		// 	this.isRequest().then((res)=>{
		// 		let tempList = this.list
		// 		tempList[this.currentTab] = tempList[this.currentTab].concat(res)
		// 		console.log(tempList)
		// 		this.list = tempList
		// 		this.$forceUpdate() //二维数组，开启强制渲染
		// 	})
		// }, 300),
		// 刷新touch监听
		// refreshStart(e) {
		// 	this.$refs.refresh.refreshStart(e);
		// },
		//@touchmove="refreshMove" 
		// refreshMove(e){
		// 	this.$refs.refresh.refreshMove(e);
		// },
		// refreshEnd(e) {
		// 	this.$refs.refresh.refreshEnd(e);
		// },
		// isRefresh(){
		// 		setTimeout(() => {
		// 			uni.showToast({
		// 				icon: 'success',
		// 				title: '刷新成功'
		// 			})
		// 			this.$refs.refresh.endAfter() //刷新结束调用
		// 		}, 1000)
		// },
		
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
		background-color: white;
		margin: 0 auto 42upx auto;
		background: #FFFFFF;
		box-shadow: 0 0 5px 0 rgba(0, 0, 0, 0.10);
		border-radius: 5px;
		position: relative;
		padding:30upx;
		line-height: 50upx;
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
		top: 50px;
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
	.img image{
		width:100upx;
		height:150upx;
	}
	.img{
		text-align: center;
	}
	.tt{
		font-size:30upx;
		font-weight: bold;
	}
</style>

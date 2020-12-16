<template>
	<view class="person-list" style="padding-top:2rem">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">附近</block>
		</cu-custom>

		<view style="position: relative;">
			<map style="width:100%;height:100vh;" :latitude="latitude" :longitude="longitude" :markers="markers" :circles="circles"
			 @controltap="Back()" @markertap="openMap">

			</map>
			<view style="background-color: rgba(0,0,0,0.2);position: absolute;top: 0;width: 100%;padding: 10px 30px;display: flex;">
				<input placeholder="请输入搜索内容" style="padding-left: 10px; border: 1px solid #ccc;border-radius: 15px;flex: 1;height: 35px;background-color: #fff;" v-model="SerchTitle" />
				<view class="searchBtn" @click="SerchMap">搜索</view>
			</view>
			<view style="position: absolute;top: 50%;left: 50%;transform: translate(-50%,-100%);display: none;width: 70%;height: 250px;overflow-y: auto; background-color: #fff;" :class="{active : popupShow}">
				<view style="margin: 10px;border-radius: 5px;position: relative;">
					<view v-for="(item,index) in searchList">
						<view class="itemBox" @click="toMarker(item)">{{item.title}}</view>
					</view>
					<view class="closeBtn" @click="popupShow = false">X</view>
				</view>
			</view>
		</view>
	</view>


</template>




<script>
	import {
		getXFsList
	} from "../../api/XFs.js";
	export default {
		data() {
			return {
				popupShow:false,
				searchList:[],
				SerchTitle:'',//搜索名称
				agentId: 0,
				title: 'map', //地图标题
				latitude: 29.79317, //纬度
				longitude: 119.6915, //经度
				//scale:5,//最小数，缩放范围最大，可见程度最大
				scale: 18, //最大数，缩放范围最小，可见程度最小,
				markers: [],
				polyline: [{ //指定一系列坐标点，从数组第一项连线至最后一项
					points: [{
							latitude: 29.79317,
							longitude: 119.6915
						},
						{
							latitude: 40.013,
							longitude: 118.685
						},
					],
					color: "#0000AA", //线的颜色
					width: 2, //线的宽度
					arrowLine: true, //带箭头的线 开发者工具暂不支持该属性
				}],
				circles: [{ //在地图上显示圆
					latitude: 29.79317,
					longitude: 119.6915,
					fillColor: "#999999AA", //填充颜色
					color: "#cacacaAA", //描边的颜色
					radius: 50, //半径
					strokeWidth: 2 //描边的宽度
				}],
				controls: [{
					id: 1, //控件id
					iconPath: '../../static/index.png', //显示的图标	
					position: { //控件在地图的位置
						left: 15,
						top: 15,
						width: 50,
						height: 50
					},
					clickable: true
				}]
			}
		},
		methods: {
			// 点击 切换地图中心点
			toMarker(e){
				// console.log('456456456')
				// console.log(e)
				this.latitude=e.latitude
				this.longitude=e.longitude
				this.popupShow = false
			},
			// 点击搜索 显示查询列表
			SerchMap(){
				console.log(this.SerchTitle)
				this.searchList=[]
				for(var i=0;i<this.markers.length;i++){
					if(this.markers[i].title.includes(this.SerchTitle)){
						// console.log(this.markers[i])
						let params=this.markers[i]
						this.searchList.push(params)
					}
				}
				console.log(this.searchList)
				this.popupShow = true
			},
			//获取点
			GetXFPosition: function() {
				getXFsList().then(res => {
					console.log('123123123')
					this.markers = res.data.data;
					console.log(this.markers);
				});
			},
			Back() {
				uni.reLaunch({
					url: '../home/index'
				});
			},
			openMap(e) {
				console.log(e);
				var a = this.markers.find(x => x.id == e.markerId);
				var lat = a.latitude;
				var lon = a.longitude;
				var name = a.title;
				//var title=a.callout.content;
				uni.openLocation({
					latitude: lat,
					longitude: lon,
					name: name,
					success: function() {
						console.log('success');
					},
					fail: function() {
						console.log('error');
					}
				});
			},
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
				//定位到当前
				uni.getLocation({
					type: "gcj02",
					altitude: true,
					success: (res) => {
						this.latitude = res.latitude;
						this.longitude = res.longitude;
						this.circles[0].latitude = res.latitude;
						this.circles[0].longitude = res.longitude;
					},
					fail: (res) => {
						uni.showModal({
							title: '提示',
							content: '定位失败',
							showCancel: false
						});
					}
				});
			}
		},
		mounted() {
			this.GetXFPosition();
		}
	}
</script>

<style>
	.searchBtn{
		height: 35px;
		line-height: 35px;
		display: inline-block;
		padding: 0 15px;
		background-color: #0081FF;
		border-radius: 5px;
		color: #fff;
		margin-left: 20px;
	}
	.active{
		display: block !important;
	}
	.itemBox{
		height: 40px;
	}
	.closeBtn{
		width: 20px;
		height: 20px;
		line-height: 20px;
		text-align: center;
		border: 1px solid #ccc;
		border-radius: 50%;
		position: fixed;
		top: 0;
		right: 0;
		size: 12px;
		color: #ccc;
	}
</style>
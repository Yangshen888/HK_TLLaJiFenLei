<template>
	<view class="person-list" >
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">附近</block>
		</cu-custom>
		<view>
			<map style="width:100%;height:100vh;" :latitude="latitude" :longitude="longitude" :markers="markers" :circles="circles" @controltap="Back()" @markertap="openMap">
				<!-- <cover-view style="width: 80%;margin-left:5%;margin-right:5%;background-color: #FAFAFA;display: flex;flex-direction: row;align-items: center;  
				            justify-content: center;padding: 2% 5%;margin-top: 20upx;border-radius: 10upx">  
				                    <img style="width: 30upx;height: 30upx" />  
				                    <input placeholder="搜索商铺" style="text-align: center;width: 100%;font-size: 32upx;" />  
				                </cover-view> -->
			</map>
		</view>
	</view>
	
	
</template>




<script>
	import { getXFsList } from "../../api/XFs.js";
	export default {
	　　data(){
	　　　　return{
	      　　 agentId:0,
	       　　title: 'map', //地图标题
	       　　latitude: 29.79317,  //纬度
	      　　 longitude: 119.6915,  //经度
	　　　　　　//scale:5,//最小数，缩放范围最大，可见程度最大
	　　　　　　scale:18,//最大数，缩放范围最小，可见程度最小,
	           markers: [],
			   polyline:[{//指定一系列坐标点，从数组第一项连线至最后一项
			   　　　　points:[
			          　　{latitude: 29.79317,longitude: 119.6915},
			         　　 {latitude: 40.013,longitude: 118.685},
			   　　　　],
			   　　　　color:"#0000AA",//线的颜色
			   　　　　width:2,//线的宽度
			   　　　　arrowLine:true,    //带箭头的线 开发者工具暂不支持该属性
			   　　}],
			   circles:[{//在地图上显示圆
			      　　latitude: 29.79317,
			      　　longitude: 119.6915,
			      　　fillColor:"#999999AA",//填充颜色
			     　　 color:"#cacacaAA",//描边的颜色
			      　　radius:50,//半径
			     　　 strokeWidth:2//描边的宽度
			      }],
				  controls:[{
					id:1,//控件id
					iconPath:'../../static/index.png',//显示的图标	
					position:{//控件在地图的位置
						left:15,
						top:15,
						width:50,
						height:50
					},
					clickable:true
                  }]
			   }
	　　},
	methods:{
		//获取点
		GetXFPosition:function(){
			getXFsList().then(res=>{
				console.log(res.data.data);
				this.markers=res.data.data;
			});
		},
		Back() {
			uni.reLaunch({
				url: '../home/index'
			});
		},
		openMap(e){  
			console.log(e);
			var a=this.markers.find(x=>x.id==e.markerId);
			var lat=a.latitude;
			var lon=a.longitude;
			var name=a.title;
			//var title=a.callout.content;
		        uni.openLocation({
		            latitude: lat,
		            longitude: lon,
					name:name,
		            success: function () {
		                console.log('success');
		            },
					fail:function(){
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
			type:"gcj02",
			altitude:true,
			success:(res)=>{
				this.latitude=res.latitude;
				this.longitude=res.longitude;
				this.circles[0].latitude=res.latitude;
				this.circles[0].longitude=res.longitude;
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





<template>
	<view class="content">
		<cu-custom ref="custon" bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">实时地图</block>
		</cu-custom>

		<view class="page-body">
			<map
				style="width:100%;height:100vh;"
				:latitude="latitude"
				:longitude="longitude"
				:markers="markers"
				:polyline="polyline"
				:controls="controls"
				@controltap="Back()"
				
			>
				<!-- <cover-view
					style="width: 80%;margin-left:5%;margin-right:5%;margin-top:25%;background-color: #FAFAFA;display: flex;flex-direction: row;align-items: center;  
				            justify-content: center;padding: 2% 5%;margin-top: 20upx;border-radius: 10upx"
				>
					<img style="width: 30upx;height: 30upx" />
					<input placeholder="搜索" style="text-align: center;width: 100%;font-size: 32upx;" @change="LoadData" />
				</cover-view> -->
			</map>
			<!-- <view class="cu-bar bg-white search fixed" style="z-index: auto;">
				<view class="search-form round">
					<text class="cuIcon-search"></text>
					<input type="text" placeholder="输入搜索的关键词" confirm-type="search" />
				</view>
				<view class="action"><button class="cu-btn bg-gradual-green shadow-blur round">搜索</button></view>
			</view> -->
		</view>
	</view>
</template>

<script>
import uniPopup from '@/components/uni-popup/uni-popup.vue';
import uniIcon from '@/components/uni-icon/uni-icon.vue';
import { getRecycleInfo } from '@/api/carApp/carapp.js';
export default {
	components: { uniIcon, uniPopup },
	data() {
		return {
			Place: '',
			title: 'map', //地图标题
			latitude: 29.794536, //纬度
			longitude: 119.692021, //经度 //scale:5,//最小数，缩放范围最大，可见程度最大
			scale: 18, //最大数，缩放范围最小，可见程度最小,
			markers: [],
			polyline: [],
			// circles: [
			// 	{
			// 		//在地图上显示圆
			// 		latitude: 29.79317,
			// 		longitude: 119.6915,
			// 		fillColor: '#999999AA', //填充颜色
			// 		color: '#cacacaAA', //描边的颜色
			// 		radius: 50, //半径
			// 		strokeWidth: 2 //描边的宽度
			// 	}
			// ],
			controls: [
				{
					id: 1, //控件id
					//iconPath: '/static/logo.png', //显示的图标
					position: {
						//控件在地图的位置
						left: 15,
						top: 15,
						width: 50,
						height: 50
					},
					clickable: true
				}
			],
			roominfo: [],
			test:-1,
			test2:-1,
			num:0,
		};
	},
	computed: {
		myStyle: function() {
			return { width: '100%', height: '100vh' };
		}
	},
	methods: {
		demo() {
			console.log('xxxxxxxxxxxxxx');
		},
		LoadData() {},
		setData() {
			console.log(5555555);
			for (let i = 0; i < this.roominfo.length; i++) {
				let marker = {
					id: (i+1),
					latitude: parseFloat(this.roominfo[i].lat),
					longitude: parseFloat(this.roominfo[i].lon),
					title: this.roominfo[i].ljname,
					iconPath: '',
					callout: {
						content:
							this.roominfo[i].towns +
							' ' +
							this.roominfo[i].vname +
							' ' +
							this.roominfo[i].ljname +
							'\n' +
							'其他垃圾:' +
							(this.roominfo[i].otherWaste ? '已收' : '未收') +
							'\n' +
							'易腐垃圾:' +
							(this.roominfo[i].perishable ? '已收' : '未收')
					}
				};
				// console.log(marker);

				if (this.roominfo[i].otherWaste && this.roominfo[i].perishable) {
					marker.iconPath='/static/aa_1.png';
				}else if(!this.roominfo[i].otherWaste && this.roominfo[i].perishable){
					marker.iconPath='/static/ab_1.png';
				}else if(!this.roominfo[i].otherWaste && !this.roominfo[i].perishable){
					marker.iconPath='/static/bb_1.png';
				}else if(this.roominfo[i].otherWaste && !this.roominfo[i].perishable){
					marker.iconPath='/static/ba_1.png';
				}else{
					
				}
				//console.log(this.markers);
				this.markers.push(marker);
				// console.log(this.markers);
			}
		},
		getMapData(){
			this.roominfo=[];
			this.markers=[];
			let that=this;
			getRecycleInfo('').then(res => {
				console.log(res);
				if (res.data.code == 200) {
					console.log(2222);
					//debugger;
					for (let i = 0; i < res.data.data.length; i++) {
						console.log(3333);
						if (i != 0 && res.data.data[i].garbageRoomUuid == res.data.data[i - 1].garbageRoomUuid) {
							let index = that.roominfo.findIndex(x => x.garbageRoomUuid == res.data.data[i].garbageRoomUuid);
							console.log(index);
							if (res.data.data[i].type == '其他垃圾') {
								that.roominfo[index].otherWaste = true;
							} else if (res.data.data[i].type == '易腐垃圾') {
								that.roominfo[index].perishable = true;
							} else {
							}
						} else {
							console.log(4444);
							let room = {
								garbageRoomUuid: res.data.data[i].garbageRoomUuid,
								ljname: res.data.data[i].ljname,
								vname: res.data.data[i].vname,
								towns: res.data.data[i].towns,
								lon: res.data.data[i].lon,
								lat: res.data.data[i].lat,
								otherWaste: false,
								perishable: false
							};
							console.log(room);
							if (res.data.data[i].type == '其他垃圾') {
								room.otherWaste = true;
							} else if (res.data.data[i].type == '易腐垃圾') {
								room.perishable = true;
							} else {
							}
							if (room.lon != null && room.lat != null && room.lon != '' && room.lon != '') {
								that.roominfo.push(room);
								console.log(that.roominfo);
							}
						}
					}
					console.log(that.roominfo);
					that.setData();
				}
			})
		}
	},
	onLoad() {
		let that = this;
		this.test2= setInterval(() =>uni.getLocation({
			type: 'gcj02 ',
			success: function(res) {
				//uni.hideLoading();
				console.log('当前位置的经度：' + res.longitude);
				console.log('当前位置的纬度：' + res.latitude);
				that.latitude= res.latitude; //纬度
				that.longitude= res.longitude;
				// uni.showModal({
				// 	title: '成功',
				// 	content: ''+JSON.stringify(res),
				// 	showCancel: false,
				// });
				let marker={
					id: 0,
					latitude: res.latitude,
					longitude: res.longitude,
					title: '当前位置',
					iconPath: '/static/ljc.png',
				}
				let index=that.markers.findIndex(x=>x.id===0)
				if(index==-1){
					that.markers.push(marker);
				}else{
					that.markers[index]=marker;
					// uni.showModal({
					// 	title: '定位mark',
					// 	content: ''+JSON.stringify(that.markers[index]),
					// 	showCancel: false,
					// });
				}
				
			},
			fail: function(res) {
				that.num++;
				console.log(res);
				console.log(that.num);
				if(that.num==1){
					uni.showModal({
						title: '失败',
						content: ''+JSON.stringify(res).split('请')[0].split('fail')[1].split(']')[1],
						showCancel: false,
					});
				}
				
				
			}
		}),2000);
		this.getMapData();
		this.test= setInterval(() =>this.getMapData(),10000);
		// getRecycleInfo('').then(res => {
		// 	console.log(res);
		// 	if (res.data.code == 200) {
		// 		console.log(2222);
		// 		//debugger;
		// 		for (let i = 0; i < res.data.data.length; i++) {
		// 			console.log(3333);
		// 			if (i != 0 && res.data.data[i].garbageRoomUuid == res.data.data[i - 1].garbageRoomUuid) {
		// 				let index = that.roominfo.findIndex(x => x.garbageRoomUuid == res.data.data[i].garbageRoomUuid);
		// 				console.log(index);
		// 				if (res.data.data[i].type == '其他垃圾') {
		// 					that.roominfo[index].otherWaste = true;
		// 				} else if (res.data.data[i].type == '易腐垃圾') {
		// 					that.roominfo[index].perishable = true;
		// 				} else {
		// 				}
		// 			} else {
		// 				console.log(4444);
		// 				let room = {
		// 					garbageRoomUuid: res.data.data[i].garbageRoomUuid,
		// 					ljname: res.data.data[i].ljname,
		// 					vname: res.data.data[i].vname,
		// 					towns: res.data.data[i].towns,
		// 					lon: res.data.data[i].lon,
		// 					lat: res.data.data[i].lat,
		// 					otherWaste: false,
		// 					perishable: false
		// 				};
		// 				console.log(room);
		// 				if (res.data.data[i].type == '其他垃圾') {
		// 					room.otherWaste = true;
		// 				} else if (res.data.data[i].type == '易腐垃圾') {
		// 					room.perishable = true;
		// 				} else {
		// 				}
		// 				if (room.lon != null && room.lat != null && room.lon != '' && room.lon != '') {
		// 					that.roominfo.push(room);
		// 					console.log(that.roominfo);
		// 				}
		// 			}
		// 		}
		// 		console.log(that.roominfo);
		// 		that.setData();
		// 	}
		// });
	},
	// onHide(){
	// 	console.log("========================");
	// 	clearInterval(test);
	// },
	onUnload(){
		console.log("***********************");
		clearInterval(this.test);
		clearInterval(this.test2);
	}
};
</script>

<style>
.title {
	z-index: 111;
	position: fixed;
	width: 100%;
	margin-top: 10px;
}
</style>

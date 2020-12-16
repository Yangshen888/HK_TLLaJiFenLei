<template >
	<view class="content" style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
						<block slot="backText">返回</block>
						<block slot="content">打卡</block>
		</cu-custom>
		<view>
			<map style="width:100%;height:30vh;"  
			:latitude="outmap.latitude" 
			:longitude="outmap.longitude" 
			:markers='outmap.markers' 
			:circles='outmap.circles' 
			:scale='outmap.scale'
			@markertap='openMap'
			>
			</map>
		</view>
		<view class="page-body">
			<!-- <text @click="reset">重置</text> -->
			<view style="text-align: center;padding: 20upx 0;">
				当前位置:
				<text @click="openLocation">{{ address }}</text>
			</view>
			<view class="" style="margin:0 20px;border-radius: 5px;">
				<view class="cu-form-group margin-top" style="border-radius: 5px;">
					<view class="title">垃圾厢房：</view>
					<picker @change="PickerChange" :value="index" :range-key="'ljname'" :range="picker_room">
						<view class="picker">
							{{index>-1?picker_room[index].ljname:picker_room[0].ljname}}
						</view>
					</picker>
				</view>
			</view>
			<view class="uni-timeline">
				<view class="uni-timeline-item uni-timeline-first-item">
					<view class="uni-timeline-item-divider" :style="{ background: !isAm ? '#1AAD19' : '#bbb' }"></view>
					<view class="uni-timeline-item-content">
						<view>
							<!-- <view class="uni-timeline-item-content-t1">上班时间{{ Timer[0].time }}</view> -->
							<view class="desc-pad" v-if="isAm">
								<view class="time uni-timeline-item-content-t">
									打卡时间 {{ amSign.time.substring(10, 16) }}
									<!-- <view class="iswq" v-if="amSign.mode == '外勤打卡'">外勤</view> -->
								</view>
								<!-- <view>
									<uni-icon type="location-filled" />
									{{ amSign.address }}
								</view> -->
							</view>
							<view v-else class="content-show">
								<view v-if="is === true">
									<view class="module CBlue" @click="clickSign">
										<view class="text">上班打卡</view>
										<view class="time">{{ time }}</view>
									</view>
								</view>
								<view v-else-if="is === false">
									<view class="module Ccc" >
										<view class="text">上班打卡</view>
										<view class="time">{{ time }}</view>
									</view>
								</view>
								<view v-else-if="is === null">
									<view class="module CAsh">
										<view class="text">请检查位置信息</view>
										<view class="time">{{ time }}</view>
									</view>
								</view>
							</view>
						</view>
					</view>
				</view>
				<view class="uni-timeline-item uni-timeline-last-item">
					<view class="uni-timeline-item-divider" :style="{ background: isAm ? '#1AAD19' : '#bbb' }"></view>
					<view class="uni-timeline-item-content">
						<view>
							<!-- <view class="uni-timeline-item-content-t1">下班时间{{ Timer[1].time }}</view> -->
							<view v-if="isAm" class="desc-pad">
								<view class="desc-pad" v-if="isPm">
									<view class="time uni-timeline-item-content-t">
										打卡时间:{{ pmSign.time.substring(10, 16) }}
										<!-- <view class="iswq" v-if="pmSign.mode == '外勤打卡'">外勤</view> -->
									</view>
									<!-- <view>
										<uni-icon type="location-filled" />
										{{ pmSign.address }}
									</view> -->
								</view>
								<view v-else class="content-show">
									<view v-if="is === true">
										<view class="module CBlue" @click="clickSign">
											<view class="text">下班打卡</view>
											<view class="time">{{ time }}</view>
										</view>
									</view>
									<view v-else-if="is === false">
										<view class="module Ccc" >
											<view class="text">下班打卡</view>
											<view class="time">{{ time }}</view>
										</view>
									</view>
									<view v-else-if="is === null">
										<view class="module CGreen">
											<view class="text">请检查位置信息</view>
											<view class="time">{{ time }}</view>
										</view>
									</view>
								</view>
							</view>
						</view>
					</view>
				</view>
			</view>

		</view>
		<view id="bottom" style='text-align:left;background-color: white;margin:20upx;padding:10upx;padding-left:30upx;border-radius: 5px;line-height: 50upx;font-size: 15px;' >
			<view>
				1.蓝色代表在范围内，可以打卡
			</view>
			<view>
				2.灰色代表不在范围内,不可以打卡
			</view>
		</view>
		<view class="cu-modal" :class="showMess">
			<view class="cu-dialog">
				<view class="cu-bar bg-white justify-end">
					<view class="content">消息</view>
					<view class="action" @tap="hideModal"><text class="cuIcon-close text-red"></text></view>
				</view>
				<view class="padding-xl">{{messageInfo}}</view>
			</view>
		</view>
	</view>
</template>

<script>

import uniIcon from '@/components/uni-icon/uni-icon.vue';
import { formateDate, pointInsideCircle, isSameDay } from '@/global/utils.js';
import {getAddress} from '@/api/map/map.js';
import { handleSignClick, setSignInfo, addSignInfo, getSignInfo, delSignInfo, getInfo, key } from '@/global/index.js';
import {
	doClockIn,
	doClockBack,
	message
	} from '@/api/attendancemanagement/attendancelist'
	import{doGetTrashLocation,getTrashRoomList} from '@/api/supervisor/manager.js'
	import { getXFsList } from ".../../api/XFs.js";
export default {
	
	components: { uniIcon },
	data() {
		return {　
		    showMess:'',
			messageInfo:'',
			outmap:{
				markers: [],
				circles:[{//在地图上显示圆
				  　　latitude: 29.79317,
				  　　longitude: 119.6915,
				  　　fillColor:"#999999AA",//填充颜色
				 　　 color:"#cacacaAA",//描边的颜色
				  　　radius:10,//半径
				 　　 strokeWidth:2//描边的宽度
				}],　
				scale:18,
				latitude: 29.79317,  //纬度
			  　longitude: 119.6915,  //经度
			},
			name: '我的名字',
			bzText: { time: '', address: '', img: '', remarks: '' },
			type: '',
			picker_room: [],
			index:0,
			trashroom:'',
			r: 80, //半径
			Timer: [{ time: '09:00' }, { time: '18:00' }], //上下班时间
			isAm: false, //上班是否打卡
			isPm: false, //下班是否打卡
			amSign: { time: '', address: '', remarks: '', img: '' }, //上午打卡信息
			pmSign: { time: '', address: '', remarks: '', img: '' }, //下午打卡信息
			signModel:{
				AmstartTime:'',AmstartPlace:'',AmendTime:'',AmendPlace:'',
				PmstartTime:'',PmstartPlace:'',PmendTime:'',PmendPlace:'',
				UserUUID:'',UserName:'',Type:'',garbageRoomUuid:'',TimeState:'am'
			},
			clickNum: 0, //点击重新获取位置信息次数
			is: true, //是否正常打卡（外勤打卡）
			isSign: false, //是否打卡
			time: formateDate(new Date(), 'h:min:s'), //当前时分秒
			date: formateDate(new Date(), 'Y-M-D'),
			latitude: '', //当前经度
			longitude: '', //当前维度
			address: '我的位置', //
			wqInfo: null,
			allSign: [], //所有打卡信息
			signInfo: { mode: '', latitude: '', longitude: '', address: '', time: '', remarks: '' }, //打卡信息 （模式，经纬度，地址，时间）
			covers: [
				// 公司点信息
				{
					id: 0,
					callout: { content: '*****科技有限公司', color: 'red', display: 'ALWAYS' },
					latitude: 37.788338,
					longitude: 112.557031,
					iconPath: '../../../static/img/location.png'
				}
			],
			circles: [
				// 公司圆信息(latitude:39.9085,longitude:116.39747 );
				{ latitude: 37.788338, longitude: 112.557031, radius: 80, strokeWidth: 1, fillColor: '#7fff0099' }
			],
			myaddress:""
		};
	},
	// 初始化
	onLoad() {
		
		var sign = getSignInfo(0);
		
		if (sign != undefined) {
			var signA = sign.main.reverse();
			this.allSign = signA;
			if (signA.length == 1) {
				if (isSameDay(signA[0].nowT)) {
					this.isSign = true;
					this.isAm = true;
					this.allSign = signA;
					this.amSign = signA[0];
				}
			} else {
				// 理想状态 认为这是 上一天 的 下班信息
				if (signA[0]) {
					if (isSameDay(signA[0].nowT)) {
						this.isSign = true;
						this.isPm = true;
						this.allSign = signA;
						this.pmSign = signA[0];
					}
				}
				// 理想状态 认为这是 上一天 的 上班信息
				if (signA[1]) {
					if (isSameDay(signA[1].nowT)) {
						this.isSign = true;
						this.isAm = true;
						this.allSign = signA;
						this.amSign = signA[1];
					}
				}
			}
		}
		// this.gettrashlocation();
		// this.getLocation();
		this.getTime();
		this.doTrashRoomList();
	},
	methods: {
		togglePopup(type) {
			this.type = type;
		},
		//获取垃圾房位置
		GetXFPosition:function(){
			getXFsList().then(res=>{
				this.outmap.markers=res.data.data;
				console.log(this.outmap.markers);
			});
		},
		doTrashRoomList(){
			let dddd=this.outmap.longitude+','+this.outmap.latitude;
			uni.getLocation({
				type:"gcj02",
				altitude:true,
				success:(res)=>{
					dddd=res.longitude+','+res.latitude;
					console.log(dddd);
					getTrashRoomList(dddd).then(res=>{
						console.log(res);
						if(res.data.code==200)
						{
							this.picker_room=res.data.data;
							this.trashroom=this.picker_room[this.index].garbageRoomUuid;
							// this.covers[0].latitude=picker_room[0].lat;
							// this.covers[0].longitude=picker_room[0].lon;
							this.gettrashlocation();
							this.getLocation();
						}
					})
					this.outmap.latitude=res.latitude;
					this.outmap.longitude=res.longitude;
					this.outmap.circles[0].latitude=res.latitude;
					this.outmap.circles[0].longitude=res.longitude;
				}
			});
		},
		//导航
		openMap(e){
			var a=this.outmap.markers.find(x=>x.id==e.markerId);
			var lat=a.latitude;
			var lon=a.longitude;
			var title=a.title;
			
			console.log(this.getDistance(lat,lon,this.outmap.latitude,this.outmap.longitude));
			
			uni.openLocation({
				latitude: lat,
				longitude: lon,
				name:title,
				success: function () {
					console.log('success');
				},
				fail:function(){
					console.log('error');
				}
			});
		},
		// 腾讯位置服务
		getAdd() {
			if (this.isAm && this.isPm) {
				return;
			}
			if (this.is === true) {
				let address = this.covers[0].callout.content;
				this.address = address;
				this.signInfo.address = address;
				
				return;
			}
			var that = this;
			var url = `https://apis.map.qq.com/ws/geocoder/v1/?location=${that.latitude},${that.longitude}&key=${key}`;
			console.log(url);
			let data={
				lat:that.latitude,
				lon:that.longitude,
				key:key,
			}
			console.log(data);
			getAddress(data).then(res=>{
				console.log(res);
				if(res.data.code==200){
					var addr = res.data.data;
					console.log(addr);
					if (addr.status != 0) {
						uni.showToast({ title: addr.message, icon: 'none' });
						return;
					}
					
					that.latitude = addr.result.ad_info.location.lat;
					that.longitude = addr.result.ad_info.location.lng;
					that.address=addr.result.address + addr.result.formatted_addresses.recommend;
					console.log(that.address);
					that.covers[0] = { id: 1, latitude: that.latitude, longitude: that.longitude, iconPath: '../../static/location.png' };
					var s = pointInsideCircle([that.latitude, that.longitude], [that.covers[0].latitude, that.covers[0].longitude], that.r / 100000);
					console.log(s);
					console.log(that.latitude);
					console.log(that.longitude);
					
					that.is = s;
					that.signInfo.latitude = that.latitude;
					that.signInfo.longitude = that.longitude;
					console.log(that.latitude);
					that.signInfo.mode = '正常打卡';
					that.signInfo.address = addr.result.address + addr.result.formatted_addresses.recommend;
					
					if (that.is === null) {
						that.address = '请检查位置信息！';
					}
					if (that.is === false) {
						let address = addr.result.address + addr.result.formatted_addresses.recommend;
						that.address = address;
						that.signInfo.address = address;
					}
				}
				else{
					uni.showModal({
						title: '提示',
						content: '请检查位置信息！',
						showCancel: false,
						success: function(rest) {
							if (rest.confirm) {
								uni.navigateTo({
									url: '/pages/home/index'
								});
							}
						}
					});
				}
			});
			// uni.request({
			// 	url,
			// 	success(res) {
			// 		console.info(res.data);
			// 		var data = res.data;
			// 		if (data.status != 0) {
			// 			uni.showToast({ title: data.message, icon: 'none' });
			// 			return;
			// 		}
			// 		that.latitude = data.result.ad_info.location.lat;
			// 		that.longitude = data.result.ad_info.location.lng;
			// 		that.covers[0] = { id: 1, latitude: that.latitude, longitude: that.longitude, iconPath: '../../static/location.png' };
			// 		var s = pointInsideCircle([that.latitude, that.longitude], [that.covers[0].latitude, that.covers[0].longitude], that.r / 100000);
			// 		console.log(s);
			// 		console.log(that.latitude);
			// 		console.log(that.longitude);
					
			// 		that.is = s;
			// 		that.signInfo.latitude = res.latitude;
			// 		that.signInfo.longitude = res.longitude;
			// 		that.signInfo.mode = '正常打卡';
			// 		that.signInfo.address = res.data.result.address + res.data.result.formatted_addresses.recommend;
					
			// 		if (that.is === null) {
			// 			that.address = '请检查位置信息！';
			// 		}
			// 		if (that.is === false) {
			// 			let address = res.data.result.address + res.data.result.formatted_addresses.recommend;
			// 			that.address = address;
			// 			that.signInfo.address = address;
			// 		}
			// 	}
			// });
		},
		
		PickerChange(e) {
			console.log(e.detail.value);
			this.index = e.detail.value
			this.trashroom=this.picker_room[this.index].garbageRoomUuid;
			this.gettrashlocation();//重新获取厢房位置
		},
		// 选择打卡日期
		bindDateChange: function(e) {
			this.date = e.target.value;
		},
		// 重置打卡
		reset(){
			var that = this;
			uni.showModal({
				title:"重置本地打卡信息",
				content:"您确定要重置打卡信息吗？点击确定本地打卡信息会被清除！",
				success: function (res) {
					if (res.confirm) {
						delSignInfo(0);
						that.isSign = false;
						that.isAm = false;
						that.isPm = false;
						that.allSign = [];
					} else if (res.cancel) {
						return;
					}
				}
			})
		},
		// 实时时间
		getTime() {
			var that = this;
			setInterval(function() {
				that.time = formateDate(new Date(), 'h:min:s');
			}, 1000);
		},
		// 获取当前位置
		getLocation() {
			var that = this;
			// if (this.clickNum !== 0) {
			// 	uni.showLoading({ title: '获取中...', mask: true });
			// }
			// if (this.clickNum >= 3) {
			// 	uni.showToast({ title: '请稍后尝试！', icon: 'none', mask: true });
			// 	return;
			// }
			// this.clickNum++;
			
			uni.getLocation({
				type: 'gcj02 ', //返回可以用于uni.openLocation的经纬度
				success(res) {
					uni.hideLoading();
					that.latitude = res.latitude;
					that.longitude = res.longitude;
					// that.covers[1] = { id: 1, latitude: res.latitude, longitude: res.longitude, iconPath: '../../static/location.png' };
					// var s = pointInsideCircle([that.latitude, that.longitude], [that.circles[0].latitude, that.circles[0].longitude], that.r / 100000);
					// console.log(s);
					// that.is = s;

					// that.signInfo.latitude = res.latitude;
					// that.signInfo.longitude = res.longitude;
					// that.signInfo.mode = '正常打卡';
					// that.signInfo.mode = s ? '正常打卡' : '外勤打卡';
					that.getAdd();
				},
				fail(err) {
					uni.hideLoading();
					that.address = '请检查位置信息！';
					uni.showToast({ title: '请检查位置信息状态！', icon: 'none', mask: true, duration: 3000 });
				}
			});
		},
		// 点击打卡
		clickSign() {
			var that = this;
			var isTrue = this.is;
			if (isTrue === null) {
				uni.showToast({ title: '请检查位置信息状态！', icon: 'none', mask: true, duration: 3000 });
				return;
			}

			uni.showLoading({ title: '打卡记录中...', mask: true });
			this.signInfo.time = formateDate(new Date(), 'Y-M-D h:min:s');
			var a = getSignInfo(0);
			if (a != undefined) {
				addSignInfo(getInfo(this.signInfo), a,0);
			} else {
				setSignInfo(getInfo(this.signInfo),0);
			}
			// setTimeout(function() {
				uni.hideLoading();
				var sign = getSignInfo(0).main;
				// console.log(sign)
				that.allSign = sign.reverse();
				that.isSign = true;
				this.trashroom=this.picker_room[this.index].garbageRoomUuid;
				if (that.isAm === false) {
					that.isAm = true;
					that.amSign = that.allSign[0];
					
					that.signModel.AmstartTime=that.amSign.time;
					that.signModel.AmstartPlace=that.amSign.address;
					that.signModel.UserUUID=that.$store.state.userId;
					that.signModel.UserName=that.$store.state.userName;
					that.signModel.Type="0";//garbageRoomUuid
					that.signModel.garbageRoomUuid=this.trashroom;
					console.log(that.signModel);
					doClockIn(that.signModel).then(res=>{
						// console.log(res);
						if(res.data.code==200){
							uni.showToast({ title: '打卡成功！' });
							this.showMessage();
						}else{
							uni.showToast({ title: res.message });
						}
					})
				} else {
					that.isPm = true;
					that.pmSign = that.allSign[0];
					
					that.signModel.AmendTime=that.pmSign.time;
					that.signModel.AmendPlace=that.pmSign.address;
					doClockBack(that.signModel).then(res=>{
						if(res.data.code==200){
							uni.showToast({ title: '打卡成功！' });
						}else{
							uni.showToast({ title: res.message });
						}
					})
				}
			// }, 2000);
			console.log(this.is);
		},
		// 选择地址
		openLocation() {
			var that = this;
			uni.chooseLocation({
				success: function(res) {
					that.address = res.address;
					that.signInfo.address = res.address;
					that.signInfo.latitude = res.latitude;
					that.signInfo.longitude = res.longitude;
					
					that.latitude = res.latitude;
					that.longitude = res.longitude;
					// 这里是有问题的 .返回的 res 中有经纬度，地址名  如果使用这个经纬度 就会存在问题，（当前位置和公司位置重合），所以不建议使用这个经纬度。
					var s = pointInsideCircle([that.latitude, that.longitude], [that.circles[0].latitude, that.circles[0].longitude], that.r / 10000);
					console.log(s);
					that.is = s;
				}
			});
		},
		// 初始化数据  （公司的经纬度 公司名称 打卡范围 ）
		gettrashlocation(){
			doGetTrashLocation(this.trashroom).then(res=>{
				if(res.data.code==200){
					let data = res.data.data;
					console.log(data);
					this.covers[0].callout.content = data.ljname;
					this.covers[0].latitude=this.circles[0].latitude=data.lat;
					this.covers[0].longitude=this.circles[0].longitude=data.lon;
					// that.r = that.circles.radius = data.r;
				}
			})
		},
		//计算距离
		getDistance(lat1, lng1, lat2, lng2){
			var radLat1 = lat1*Math.PI / 180.0;
			var radLat2 = lat2*Math.PI / 180.0;
			var a = radLat1 - radLat2;
			var b = lng1*Math.PI / 180.0 - lng2*Math.PI / 180.0;
			var s = 2 * Math.asin(Math.sqrt(Math.pow(Math.sin(a/2),2) + Math.cos(radLat1)*Math.cos(radLat2)*Math.pow(Math.sin(b/2),2)));
			s = s *6378.137 ;// EARTH_RADIUS;
			s = Math.round(s * 10000) / 10000;
			return s;
		},
		//成功打卡提示信息
		showMessage(){
			
			console.log(11111111);
			console.log(this.$store.state);
			console.log(this.$store.state.userId);
			message({guid:this.$store.state.userId}).then(res=>{
				if(res.data.code==200){
					console.log(11111111);
					console.log(res.data.data);
					let info=res.data.data;
					if(info!=null&&info.length>0){
						this.showMess='show';
						for(let i=0;i<info.length;i++){
							if(this.messageInfo.length>0){
								this.messageInfo+=',';
							}
							this.messageInfo+=info[i].realName;
						}
						this.messageInfo+='志愿者今天与您一起值班';
					}
				}
				
				
			});
		},
		hideModal(e) {
			this.showMess = null;
		},
	},
	mounted(){
		this.GetXFPosition();
		
	}
};
</script>

<style>
.map {
	width: 100%;
	height: 260px;
}
.uni-list-cell-left {
	padding: 0 30upx;
}
.text-desc {
	display: flex;
	justify-content: space-between;
	padding: 10upx 20upx;
}
.colorRed {
	color: red;
}
.colorGreen {
	color: #32cd32;
}
.colorYellow {
	color: yellow;
}
.colorBlue {
	color: #007aff;
}

.bgBlue {
	background-color: #007aff;
}
.bgGreen {
	background-color: #32cd32;
}
.bgAsh {
	background-color: #c8c7cc;
}

.uni-timeline {
	padding: 30upx 20upx;
}
.uni-timeline-item-content-t {
	font-weight: bold;
}
.desc-pad {
	padding: 0 0upx;
}
.desc-pad .bz {
	color: rgb(0, 122, 255);
}
.desc-pad .bz .icon {
	color: rgb(0, 122, 255);
}
.uni-timeline-last-item .uni-timeline-item-divider {
	background: #bbb;
}

.CBlue {
	background-color: #007aff;
	box-shadow: 0 5px 5px #007aff;
}
.CGreen {
	background-color: #32cd32;
	box-shadow: 0 5px 5px #32cd32;
}
.Ccc {
	background-color: #ccc;
}
.CAsh {
	background-color: #c8c7cc;
	box-shadow: 0 5px 5px #c8c7cc;
}

.module {
	overflow: hidden;
	margin: 20upx auto;
	width: 220upx;
	height: 220upx;
	border-radius: 50%;
	color: #fff;
	text-align: center;
}
.module .text {
	font-size: 20px;
	margin: 70upx auto 10upx;
}
.uni-timeline-item .uni-timeline-item-content {
	width: 100%;
	padding-right: 20upx;
}

.content-show {
	width: 100%;
}

.sign-title {
	display: flex;
	justify-content: space-between;
	padding: 30upx 24upx;
	border-bottom: 1upx solid #333;
}
.sign-title .portrait {
	width: 100upx;
	height: 100upx;
	line-height: 100upx;
	border-radius: 50%;
	background-color: #007aff;
	color: #fff;
	font-size: 28upx;
	text-align: center;
}
.sign-title .sign-title-l {
	display: flex;
}
.sign-title .sign-title-l .text {
	margin-left: 20upx;
}
.sign-title .sign-title-l .text .name {
	font-size: 32upx;
}
.sign-title .sign-title-l .text .gz {
	color: darkblue;
	display: inline-flex;
}
.sign-title .sign-title-l .text .gz text {
	display: inline-block;
}
.sign-title .sign-title-l .text .gz .t1 {
	overflow: hidden; /*超出部分隐藏*/
	text-overflow: ellipsis; /* 超出部分显示省略号 */
	white-space: nowrap; /*规定段落中的文本不进行换行 */
	width: 166upx; /*需要配合宽度来使用*/
}
.iswq {
	padding: 0px 12px;
	color: #99cc33;
	border: 1px solid #99cc33;
	width: 26px;
	height: 24px;
	border-radius: 4px;
	margin-left: 20upx;
	display: inline-block;
}
.desc-pad .last {
	margin-bottom: 80upx;
}
.relocation {
	color: #0000ff;
}
.uni-popup .content .text {
	color: #666666;
}

/* timeline */
.uni-timeline {
	margin: 35upx 0;
	display: flex;
	flex-direction: column;
	position: relative;
}

.uni-timeline-item {
	display: flex;
	flex-direction: row;
	position: relative;
	padding-bottom: 20upx;
	box-sizing: border-box;
	overflow: hidden;
}

.uni-timeline-item .uni-timeline-item-keynode {
	width: 160upx;
	flex-shrink: 0;
	box-sizing: border-box;
	padding-right: 20upx;
	text-align: right;
	line-height: 65upx;
}

.uni-timeline-item .uni-timeline-item-divider {
	flex-shrink: 0;
	position: relative;
	width: 30upx;
	height: 30upx;
	top: 15upx;
	border-radius: 50%;
	background-color: #bbb;
}

.uni-timeline-item-divider::before,
.uni-timeline-item-divider::after {
	position: absolute;
	left: 15upx;
	width: 1upx;
	height: 100vh;
	content: '';
	background: inherit;
}

.uni-timeline-item-divider::before {
	bottom: 100%;
}

.uni-timeline-item-divider::after {
	top: 100%;
}

.uni-timeline-last-item .uni-timeline-item-divider:after {
	display: none;
}

.uni-timeline-first-item .uni-timeline-item-divider:before {
	display: none;
}

.uni-timeline-item .uni-timeline-item-content {
	padding-left: 20upx;
}

.uni-timeline-last-item .bottom-border::after {
	display: none;
}

.uni-timeline-item-content .datetime {
	color: #cccccc;
}

/* 自定义节点颜色 */
.uni-timeline-last-item .uni-timeline-item-divider {
	background-color: #1aad19;
}
</style>

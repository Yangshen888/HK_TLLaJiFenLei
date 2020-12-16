<template>
	<view class="content"  style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
						<block slot="backText">返回</block>
						<block slot="content">志愿者打卡</block>
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
				<!-- <view class="cu-form-group" style="border-radius: 5px;">
					<view class="title">志愿者</view>
					<picker @change="volunteerChange" :value="volunteerlist.index" :range="volunteerlist.picker" range-key="realName"
					 >
						<view class="picker">
							{{volunteerlist.picker[volunteerlist.index].realName}}
						</view>
					</picker>
				</view> -->
				<view class="cu-form-group " style="border-radius: 5px;">
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
							<view class="uni-timeline-item-content-t1">上班时间{{ Timer[0].time }}</view>
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
							<view class="uni-timeline-item-content-t1">下班时间{{ Timer[1].time }}</view>
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
			<view v-if="false">
				<uni-collapse>
					<uni-collapse-item title="全部打卡信息">
						<uni-list>
							<view class="uni-list-cell-left">
								<view v-for="(item, index) in allSign" :key="index" style="border-bottom: 1px dashed #007AFF;">
									<view>{{ item.mode }}</view>
									<view>{{ item.address }}</view>
									<view>{{ item.time }}</view>
								</view>
							</view>
						</uni-list>
					</uni-collapse-item>
				</uni-collapse>
			</view>
		</view>
	</view>
</template>

<script>

import uniIcon from '@/components/uni-icon/uni-icon.vue';
import { formateDate, pointInsideCircle, isSameDay } from '@/global/utils.js';
import {getAddress} from '@/api/map/map.js';
import {checkSubscribe} from '@/api/grabage/GrabList.js'
import { handleSignClick,setSignInfo, addSignInfo, getSignInfo, delSignInfo, getInfo, key } from '@/global/index.js';
import {
	doClockIn,
	doClockBack
	} from '@/api/attendancemanagement/attendancelist'
	import {
		getalllist
		} from '@/api/volunteer/list'
	import{doGetTrashLocation,getTrashRoomList,TrashRoomYYList,Roomjuli} from '@/api/supervisor/manager.js'
		import { getXFsList } from ".../../api/XFs.js";
export default {
	components: { uniIcon },
	data() {
		return {
			outmap:{
				markers: [],
			circles:[{//在地图上显示圆
				      　　latitude: 29.79317,
				      　　longitude: 119.6915,
				      　　fillColor:"#999999AA",//填充颜色
				     　　 color:"#cacacaAA",//描边的颜色
				      　　radius:20,//半径
				     　　 strokeWidth:2//描边的宽度
				      }],　
					  scale:18,
					  latitude: 29.79317,  //纬度
			  　　 longitude: 119.6915,  //经度
			},
			name: '我的名字',
			bzText: { time: '', address: '', img: '', remarks: '' },
			type: '',
			dddd:'',
			picker_room: [],
			index:0,
			trashroom:'',
			r: 80, //半径
			juli:false,//是否够距离
			Timer: [{ time: '09:00' }, { time: '18:00' }], //上下班时间
			isAm: false, //上班是否打卡
			isPm: false, //下班是否打卡
			amSign: { time: '', address: '', remarks: '', img: '' }, //上午打卡信息
			pmSign: { time: '', address: '', remarks: '', img: '' }, //下午打卡信息
			signModel:{
				AmstartTime:'',AmstartPlace:'',AmendTime:'',AmendPlace:'',
				PmstartTime:'',PmstartPlace:'',PmendTime:'',PmendPlace:'',
				UserUUID:'',UserName:'',Type:'',garbageRoomUuid:'',TimeState:'pm'
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
			volunteerlist: {
				picker: [{
					volunteerId: '',
					name: ''
				}],
				index: 0,
			},
		};
	},
	// 初始化
	onLoad() {
		let data={
			guid:this.$store.state.userId,
			ap:'pm',
		}
		checkSubscribe(data).then(res=>{
			console.log(res);
			if(res.data.code!=200){
				uni.showModal({
					title:"提示",
					content:'志愿者'+res.data.message,
					showCancel:false,
					complete: (res) => {
						uni.navigateBack({
						    delta: 1,
						    animationDuration: 200
						});
					}
				});
			}
		});
		var sign = getSignInfo(3);
		console.log(sign);
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
		// this.getLocation();
		this.getTime();
		// this.getvolunteerlist();
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
			});
		},
		getvolunteerlist(){
			getalllist().then(res=>{
				if(res.data.code==200)
				{
					console.log(res.data.data);
					this.volunteerlist.picker=res.data.data;
				}
			});
		},
		doTrashRoomList(){
			this.dddd=this.outmap.longitude+','+this.outmap.latitude;
			uni.getLocation({
				type:"gcj02",
				altitude:true,
				success:(res)=>{
					this.dddd=res.longitude+','+res.latitude;
					console.log(this.dddd);
					this.latitude=res.latitude;
					this.longitude=res.longitude;
					let datas={
						guid:this.$store.state.userId,
						ap:'pm'
					}
					TrashRoomYYList(datas).then(res=>{
						if (res.data.data.length != 0) {
							this.picker_room=res.data.data;
							console.log(this.picker_room[this.index]);
							this.trashroom=this.picker_room[this.index].a.grabRoomUuid;
							console.log(this.trashroom);
							// this.covers[0].latitude=picker_room[0].lat;
							// this.covers[0].longitude=picker_room[0].lon;
							this.gettrashlocation();
							// var s = pointInsideCircle([this.latitude, this.longitude], [this.circles[0].latitude, this.circles[0].longitude], this.r / 100000);
							// this.is = s;
							//this.getLocation();
							this.is=false;
							this.getAdd();
							
						}
					})
					this.outmap.latitude=res.latitude;
					this.outmap.longitude=res.longitude;
					this.outmap.circles[0].latitude=res.latitude;
					this.outmap.circles[0].longitude=res.longitude;
				},
				fail: (err) => {
					this.address = '请检查位置信息！';
					uni.showModal({
						title: '提示',
						content: '请检查位置信息！',
						showCancel: false,
						success: function(rest) {
							if (rest.confirm) {
								uni.navigateBack({
								    delta: 1,
								    animationDuration: 200
								});
							}
						}
					});
				}
			});
		},
		volunteerChange(e) {
			this.volunteerlist.index = e.target.value;
			console.log(this.volunteerlist.picker[this.volunteerlist.index].realName);
			
		},
		// 腾讯位置服务
		getAdd() {
			// if (this.isAm && this.isPm) {
			// 	return;
			// }
			if (this.is === true) {
				let address = this.covers[0].callout.content;
				this.address = address;
				this.signInfo.address = address;
				return;
			}
			var that = this;
			var url = `https://apis.map.qq.com/ws/geocoder/v1/?location=${that.latitude},${that.longitude}&key=${key}`;
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
					console.log(that.is);
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
			// 		console.log(res.data);
			// 		var data = res.data;
			// 		if (data.status != 0) {
			// 			uni.showToast({ title: data.message, icon: 'none' });
			// 			return;
			// 		}
					
			// 		that.latitude = data.result.ad_info.location.lat;
			// 		that.longitude = data.result.ad_info.location.lng;
			// 		that.address=res.data.result.address + res.data.result.formatted_addresses.recommend;
			// 		console.log(that.address);
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
			// 	},
			// 	fail: (err) => {
			// 		console.log(err);
			// 		//uni.showToast({ title: '请检查位置信息状态！', icon: 'none', mask: true, duration: 3000 });
			// 		// uni.showModal({
			// 		// 	title:"定位失败",
			// 		// 	content:''+JSON.stringify(err),
			// 		// })
			// 		uni.showModal({
			// 			title: '提示',
			// 			content: '请检查位置信息！',
			// 			showCancel: false,
			// 			success: function(rest) {
			// 				if (rest.confirm) {
			// 					uni.navigateTo({
			// 						url: '/pages/home/index'
			// 					});
			// 				}
			// 			}
			// 		});
			// 	}
			// });
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
						delSignInfo(1);
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
        //导航
		openMap(e){
					console.log(e);
					var a=this.outmap.markers.find(x=>x.id==e.markerId);
					var lat=a.latitude;
					var lon=a.longitude;
					
					var title=a.title;
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
					// console.log(res);
					that.latitude = res.latitude;
					that.longitude = res.longitude;
					// console.log(res.latitude,"---",res.longitude)
					// that.covers[1] = { id: 1, latitude: res.latitude, longitude: res.longitude, iconPath: '../../static/location.png' };
					var s = pointInsideCircle([that.latitude, that.longitude], [that.circles[0].latitude, that.circles[0].longitude], that.r / 100000);
					that.is = s;

					// that.signInfo.latitude = res.latitude;
					// that.signInfo.longitude = res.longitude;
					// that.signInfo.mode = s ? '正常打卡' : '外勤打卡';

					that.getAdd();
				},
				fail: (err) =>{
					uni.hideLoading();
					that.address = '请检查位置信息！';
					// uni.showToast({ title: '请检查位置信息状态！', icon: 'none', mask: true, duration: 3000 });
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
		},
		// 点击打卡
		clickSign() {
			var that = this;
			var isTrue = this.is;
			if (isTrue === null) {
				uni.showToast({ title: '请检查位置信息状态！', icon: 'none', mask: true, duration: 3000 });
				return;
			}
			let datas = {
				lnglat: that.dddd,
				guid: that.picker_room[that.index].a.grabRoomUuid
			};
			Roomjuli(datas).then(res => {
				console.log(res.data.data);
				if (res.data.data == 0) {
					uni.showLoading({ title: '打卡记录中...', mask: true });
					this.signInfo.time = formateDate(new Date(), 'Y-M-D h:min:s');
					var a = getSignInfo(3);
					if (a != undefined) {
						addSignInfo(getInfo(this.signInfo), a, 3);
					} else {
						setSignInfo(getInfo(this.signInfo), 3);
					}
					// setTimeout(function() {
					uni.hideLoading();
					var sign = getSignInfo(3).main;
					// console.log(sign)
					that.allSign = sign.reverse();
					that.isSign = true;
					this.trashroom = this.picker_room[this.index].a.grabRoomUuid;
					if (that.isAm === false) {
						that.isAm = true;
						that.amSign = that.allSign[0];

						that.signModel.PmstartTime = that.amSign.time;
						that.signModel.PmstartPlace = that.amSign.address;
						that.signModel.UserUUID = that.$store.state.userId;
						that.signModel.UserName = that.$store.state.userName;
						that.signModel.Type = '1'; //garbageRoomUuid
						that.signModel.garbageRoomUuid = this.trashroom;
						console.log(that.signModel);

						doClockIn(that.signModel).then(res => {
							//console.log(res);
							if (res.data.code == 200) {
								uni.showToast({ title: '打卡成功！' });
							} else {
								uni.showToast({ title: res.message });
							}
						});
					} else {
						that.isPm = true;
						console.log(that.allSign[0].time);
						console.log(that.allSign[1].time);
						if (that.allSign[0].time > that.allSign[1].time) {
							that.pmSign = that.allSign[0];
						} else {
							that.pmSign = that.allSign[1];
						}
						that.signModel.PmendTime = that.pmSign.time;
						that.signModel.PmendPlace = that.signInfo.address;
						that.signModel.UserUUID = that.$store.state.userId;
						that.signModel.UserName = that.$store.state.userName;
						that.signModel.Type = '1'; 
						that.signModel.garbageRoomUuid = this.trashroom;
						console.log(that.signModel);
						doClockBack(that.signModel).then(res => {
							if (res.data.code == 200) {
								uni.showToast({ title: '打卡成功！' });
							} else {
								uni.showToast({ title: res.message });
							}
						});
					}
				} else {
					uni.showToast({ title: '未到达打卡范围！', icon: 'none', mask: true, duration: 3000 });
					return;
				}
			});
			// }, 2000);
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
					that.is = s;
				}
			});
		},
		// 初始化数据  （公司的经纬度 公司名称 打卡范围 ）
		gettrashlocation() {
			var that = this;
			doGetTrashLocation(that.trashroom).then(res => {
				if (res.data.code == 200) {
					let data = res.data.data;
					//console.log(data);
					//console.log(that.covers);
					//that.covers[0].callout.content = data.ljname;
					that.covers[0].latitude = that.circles[0].latitude = data.lat;
					that.covers[0].longitude = that.circles[0].longitude = data.lon;
					// that.r = that.circles.radius = data.r;
				}
			})
		},
		PickerChange(e) {
			this.index = e.detail.value;
			//console.log(this.picker_room[this.index]);
			this.trashroom = this.picker_room[this.index].a.grabRoomUuid;
			this.gettrashlocation(); //重新获取厢房位置
			this.doTrashRoomList();
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
.CAsh {
	background-color: #c8c7cc;
	box-shadow: 0 5px 5px #c8c7cc;
}
.Ccc {
	background-color: #ccc;
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

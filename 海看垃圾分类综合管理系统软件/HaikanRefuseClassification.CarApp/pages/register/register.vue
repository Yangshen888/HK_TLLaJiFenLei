<template>
	<view class="content">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">称重记录复核</block>
		</cu-custom>
		<view class="page-body">
			<!-- <view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 20px 20px;height: 10upx;">
				<view class="title">垃圾厢房编号</view>
				<input placeholder="统一标题的宽度" name="input" v-model="form.ljname" />
			</view> -->
			<view class="cu-form-group margin-top" style="border-radius: 5px;margin:20px 20px 0px;">
				<view class="title">街道/乡镇</view>
				<picker @change="PickerChange" :value="index" :range="picker">
					<view class="picker">{{ index != -1 ? picker[index] : '请选择街道/乡镇' }}</view>
				</picker>
			</view>
			<view class="cu-form-group margin-top" style="border-radius: 5px;margin:20px 20px 0px;">
				<view class="title">社区</view>
				<picker @change="PickerChange2" :value="index2" :range="picker2">
					<view class="picker">{{ index2 != -1 ? picker2[index2] : '请选择社区' }}</view>
				</picker>
			</view>
			<view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 0px;">
				<view class="title">垃圾箱房</view>
				<picker @change="grabChange" :value="GrabageRoomList.index" :range="GrabageRoomList.picker" range-key="ljname">
					<view class="picker">{{ GrabageRoomList.picker[GrabageRoomList.index].ljname }}</view>
				</picker>
			</view>
			<!-- <view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 20px 20px;height: 10upx;">
				<view class="title">所在小区</view>
				<input style="height: 20upx;" placeholder="统一标题的宽度" name="input" v-model="form.vname" />
			</view> -->
			<!-- <view class="cu-form-group margin-top" style="border-radius: 5px;margin:20px 20px 0px;">
				<view class="title">垃圾类型</view>
				<picker @change="PickerChange3" :value="typeindex" :range="picker3">
					<view class="picker">{{ typeindex > -1 ? picker3[typeindex] : '请选择垃圾类型' }}</view>
				</picker>
			</view> -->
			<view class="cu-form-group pingjia" style="border-radius: 5px;margin:20px 20px 20px 20px;height: 10upx;">
				<view class="title">核对人</view>
				<input placeholder="请输入核对人" name="input" v-model="checkedUser" />
			</view>
			<view v-for="(item, index) in weighData" :key="index">
				<!-- <view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 20px 20px;height: 10upx;" v-show="isShow[index]">
					<view class="title">时间</view>
					<input name="input" v-model="weighData[index][0]" />
				</view> -->
				<view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 20px 20px;height: 10upx;">
					<view class="title">日期选择</view>
					<picker mode="date" v-model="date[index]" start="2000-01-01" end="2220-01-01" v-bind:id="index" @change="DateChange">
						<view class="picker">
							{{date[index]}}
						</view>
					</picker>
				</view>
				<view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 20px 20px;height: 10upx;">
					<view class="title">时间选择</view>
					<picker mode="time" :value="time[index]" start="01:00" end="24:00" v-bind:id="index" @change="TimeChange">
						<view class="picker">
							{{time[index].length>5?time[index].substring(0,5):time[index]}}
						</view>
					</picker>
				</view>
				<view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 20px 20px;height: 10upx;" v-show="isShow[index]">
					<view class="title">称重(kg)</view>
					<input name="input" type="number" v-model="weighData[index][1]" />
				</view>
			</view>
			<view style="border-radius: 5px;margin:20px 20px 0px;">
				<view class="title1">评价</view>
				<uni-rate :value="grade" :size="60" class="xingxing"></uni-rate>
			</view>
			<view>
				<button class="btn-modify" :class="modifyMobile ? 'btn-modify-active' : ''" :disabled="!modifyMobile" hover-class="btn-modify-hover"
				 @tap="fnModify">提交</button>
			</view>
			<view class="cu-modal" :class="showMess">
				<view class="cu-dialog">
					<view class="cu-bar bg-white justify-end">
						<view class="content">消息</view>
						<view class="action" @tap="hideModal"><text class="cuIcon-close text-red"></text></view>
					</view>
					<view class="padding-xl">{{ messageInfo }}</view>
				</view>
			</view>
			<view class="cu-modal" :class="showMess2">
				<view class="cu-dialog">
					<view class="cu-bar bg-white justify-end">
						<view class="content">消息</view>
						<view class="action" @tap="hideModal2"><text class="cuIcon-close text-red"></text></view>
					</view>
					<view class="padding-xl">{{ messageInfo2 }}</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	import uniRate from '@/components/uni-rate/uni-rate.vue';
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		getWeighRecord,
		checkWeighRecord
	} from '@/api/carApp/carapp.js';
	import {
		getAllGrabRoom,
		allCommunity
	} from '@/api/grabage/GrabList.js';
	import {
		formateDate,
		pointInsideCircle,
		isSameDay
	} from '@/global/utils.js';
	export default {
		components: {
			uniIcon,
			uniPopup,
			uniRate
		},
		data() {
			return {
				showMess: '',
				showMess2: '',
				messageInfo: '',
				messageInfo2: '',
				grade: 5,
				index: -1,
				picker: [''],
				index2: -1,
				picker2: [''],
				allCommunity: [],
				GrabageRoomList: {
					picker: [{
						volunteerId: '1',
						name: '1'
					}],
					index: 0
				},
				typeindex: 3,
				picker3: ['可回收垃圾', '有害垃圾', '易腐垃圾', '其他垃圾'],
				isShow: [],
				form: {
					ljname: '',
					vname: ''

					//weighData:[],
				},
				type: '',
				checkedUser: '',
				weighData: [],
				modifyMobile: false,
				formcheck: {
					one: false,
					two: false,
					thr: true
				},
				query: {
					grabRoomUUid: '',
					UserId: this.$store.state.userId,
					startdate: formateDate(new Date(), 'Y-M-D'),
					AP: 'am'
				},
				vguid: '',
				rguid: '',
				date: [],
				time: [],
				latitude: 29.794536, //纬度
				longitude: 119.692021, //经度 //scale:5,//最小数，缩放范围最大，可见程度最大
			};
		},
		watch: {
			GrabageRoomList: {
				handler(newdata, olddata) {
					console.log(1111);
					//console.log(this.weighData);
					console.log(newdata.index);
					console.log(newdata.picker[newdata.index].ljname == '');
					if (newdata.index >= 0 && newdata.picker[newdata.index].ljname != '') {
						this.formcheck.one = true;
						console.log(this.GrabageRoomList);
					} else {
						this.formcheck.one = false;
					}
				},
				deep: true
			},
			// checkedUser: {
			// 	handler(newdata, olddata) {
			// 		if (newdata != null && newdata != '') {
			// 			this.formcheck.thr = true;
			// 		}
			// 	}
			// },
			weighData: {
				handler(newdata, olddata) {
					console.log(newdata);
					let check = true;
					for (let i = 0; i < newdata.length; i++) {
						if (newdata[i][0] == '' || newdata[i][1] == '') {
							check = false;
						}
					}
					this.formcheck.two = check;
					console.log(this.formcheck.two);
				},
				deep: true
			},
			formcheck: {
				handler(newdata, olddata) {
					console.log(999999999);
					console.log(newdata);
					if (newdata.one && newdata.two && newdata.thr) {
						this.modifyMobile = true;
					} else {
						this.modifyMobile = false;
					}
					console.log(this.modifyMobile);
				},
				deep: true,
				immediate: true
			}
		},
		methods: {
			TimeChange(e) {
				let time = e.detail.value;
				let index = parseInt(e.currentTarget.id);


				// console.log(time);
				let s = this.weighData[index][0].substring(16, 20);
				this.weighData[index][0] = this.date[index] + " " + time + s;
				this.time[index] = time + s;
				Vue.set(this.time, index, time + s);
				// console.log(this.weighData[index][0]);
				// console.log("xxxxxxxxxx1");
				//console.log(index);
				// console.log(e);

				// console.log(time);
			},
			DateChange(e) {
				let date = e.detail.value;
				let index = parseInt(e.currentTarget.id);
				// console.log(index);
				//let time=this.weighData[index][0].substring(12);
				//console.log(date);
				//console.log(time);

				this.weighData[index][0] = date + " " + this.time[index];
				this.date[index] = date;
				Vue.set(this.date, index, date);
				// console.log(this.date);
				// console.log("xxxxxxxxxx2");
				//console.log(index);
				// console.log(e);
			},
			hideModal(e) {
				this.showMess = null;
				uni.navigateTo({
					url: '/pages/home/index'
				});
			},
			hideModal2(e) {
				this.showMess = null;
				uni.navigateTo({
					url: '/pages/home/index'
				});
			},
			PickerChange(e) {
				this.index = e.detail.value;
				// console.log(this.index);
				this.picker2 = Array.from(new Set(this.allCommunity.filter(x => x.towns === this.picker[this.index]).map(x => x.vname)));
				let community = this.allCommunity.filter(x => x.vname === this.picker2[0]);
				let guid = community[0].villageUuid;
				// console.log(guid);
				this.getGrablist(guid.toString(), null);
				this.GrabageRoomList.index = 0;
			},
			PickerChange2(e) {
				this.index2 = e.detail.value;
				let community = this.allCommunity.filter(x => x.vname === this.picker2[this.index2]);
				let guid = community[0].villageUuid;
				// console.log(guid);
				this.getGrablist(guid.toString(), null);
				this.GrabageRoomList.index = 0;
			},
			PickerChange3(e) {
				console.log(e);
				this.typeindex = e.detail.value;
				this.type=this.picker3[e.detail.value];
			},
			grabChange(e) {
				// console.log(666666666);
				// console.log(e.target.value);
				this.GrabageRoomList.index = e.target.value;
				this.query.grabRoomUUid = this.GrabageRoomList.picker[this.GrabageRoomList.index].garbageRoomUuid;
				// console.log(this.GrabageRoomList.picker[this.GrabageRoomList.index].ljname);
			},
			getGrablist(guid, rguid) {
				getAllGrabRoom(guid).then(res => {
					if (res.data.code == 200) {
						// console.log(res.data.data);
						this.GrabageRoomList.picker = res.data.data;
						//this.query.grabRoomUUid = this.GrabageRoomList.picker[this.GrabageRoomList.index].garbageRoomUuid;
						if (rguid != null && rguid != '') {
							// console.log(rguid);
							// console.log(this.GrabageRoomList.picker);
							let index = this.GrabageRoomList.picker.findIndex(x => x.garbageRoomUuid == rguid);

							this.GrabageRoomList.index = index;
							// console.log(this.GrabageRoomList.index);
						}
						this.query.grabRoomUUid = this.GrabageRoomList.picker[this.GrabageRoomList.index].garbageRoomUuid;
					}
					console.log(this.GrabageRoomList);
				});
			},
			fnModify() {
				// console.log('22222');
				// console.log(JSON.stringify(this.form));
				// console.log(this.query.grabRoomUUid);
				// console.log(this.weighData);
				let data = {
					weighData: this.weighData,
					room: this.query.grabRoomUUid,
					checkedUser: this.checkedUser,
					type: this.type,
					grade: this.grade
				};
				//console.log(data);
				checkWeighRecord(data).then(res => {
					// console.log(res);
					if (res.data.code == 200) {
						this.messageInfo2 = res.data.message;
						this.showMess2 = 'show';
					}
				});
			},
			setCommunity(guid) {
				// console.log(guid);
				allCommunity().then(res => {
					if (res.data.code == 200) {
						// console.log(res.data.data);
						this.allCommunity = res.data.data.list;
						//this.multiArray[0]=res.data.data.list2;
						this.picker = res.data.data.list2;
						if (guid == null || guid == '') {
							this.index = 0;
							this.picker2 = Array.from(new Set(this.allCommunity.filter(x => x.towns === this.picker[0]).map(x => x.vname)));
							this.index2 = 0;
							let community = this.allCommunity.filter(x => x.vname === this.picker2[0]);
							let vguid = community[0].villageUuid;
							// console.log(vguid);
							this.getGrablist(vguid.toString(), null);
							this.GrabageRoomList.index = 0;
						} else {
							// console.log(this.allCommunity);
							// console.log(guid);
							let arr = this.allCommunity.filter(x => x.villageUuid == guid);
							// console.log(arr);
							let index1 = this.picker.findIndex(x => x == arr[0].towns);
							this.picker2 = Array.from(new Set(this.allCommunity.filter(x => x.towns === this.picker[index1]).map(x => x.vname)));
							let index2 = this.picker2.findIndex(x => x == arr[0].vname);
							// console.log(222222222);

							// console.log(index1);
							// console.log(index2);
							this.index = index1;
							this.index2 = index2;

							let vguid = arr[0].villageUuid;
							// console.log(vguid);
							this.getGrablist(vguid.toString(), this.rguid);
							// console.log(this.picker2);
							//this.GrabageRoomList.index=0;
						}
					}
				});
			}
		},
		onLoad() {
			let that = this;
			uni.getLocation({
			    type: 'gcj02',
			    success: function (res) {
			        console.log('当前位置的经度：' + res.longitude);
			        console.log('当前位置的纬度：' + res.latitude);
					that.latitude= res.latitude; //纬度
					that.longitude= res.longitude;
			    }
			});
			//that.setCommunity('0aa18df1-1d39-45a1-a51e-cd38974a120c');
			// getWeighRecord({imei:'520000000249566'}).then(res=>{
			// 	if(res.data.code!=200){
			// 		this.messageInfo=res.data.message;
			// 		this.showMess='show';
			// 	}
			// });
			//#ifdef APP-PLUS
			plus.device.getInfo({
				success: function(e) {
					console.log('getDeviceInfo success: ' + JSON.stringify(e));
					// console.log(e.imei);
					let arr = e.imei.toString().split(',');
					// console.log(arr[0]);
					//that.IMEI=e.imei;
					getWeighRecord({
						imei: e.uuid,
						latitude:that.latitude,
						longitude:that.longitude
					}).then(res => {
						if (res.data.code == 200) {
							// console.log(res);
							//that.form.ljname = res.data.data.ljname;
							//that.form.vname = res.data.data.vname;
							that.vguid = res.data.data.villageId;
							that.rguid = res.data.data.grabageRoomId;
							that.type = res.data.data.type;
							that.typeindex = that.picker3.findIndex(x => x == that.type);
							// console.log(that.vguid);
							// console.log(that.rguid);
							that.setCommunity(that.vguid);
							that.weighData = res.data.data.weighData;
							// console.log(that.weighData);
							that.isShow = [];
							that.date = [];
							that.time = [];
							for (let i = 0; i < that.weighData.length; i++) {
								that.isShow.push('true');
								that.date.push(that.weighData[i][0].substring(0, 10));
								that.time.push(that.weighData[i][0].substring(11, 19));
								// console.log(that.date);
								// console.log(that.time);
							}
						} else {
							that.messageInfo = res.data.message;
							that.showMess = 'show';
						}

						//console.log(that.)
					});
				},
				fail: function(e) {
					console.log('getDeviceInfo failed: ' + JSON.stringify(e));
				}
			});
			//#endif
		},
		mounted() {}
	};
</script>

<style>
	.title1 {
		text-align: justify;
		font-size: 18upx;
		position: relative;
		height: 40upx;
		line-height: 40upx;
		color: #fff;
		float: left;
		text-indent: 30upx;
		width: 110upx;
		overflow: hidden;
	}

	.pingjia {
		overflow: hidden;
	}

	

	.xingxing {
		/* float: left; */
		width: 80%;
		height: 40upx;
		overflow: hidden;
		line-height: 40upx;
	}

	.btn-modify {
		margin-top: 26upx;
		border-radius: 26upx;
		margin: 20px 20px 20px 20px;
		height: 40upx;
		line-height: 40upx;
		font-size: 20px;
		color: #fff !important;
		background: linear-gradient(to right, #30c295, #01686f);
	}

	.btn-modify-active {
		background: linear-gradient(to right, #365fff, #36bbff);
	}
	
	.cu-form-group picker .picker {
		line-height: 75upx;
		font-size: 21upx;
		text-overflow: ellipsis;
		white-space: nowrap;
		overflow: hidden;
		width: 100%;
		text-align: right;
	}
	
	.cu-form-group picker::after {
		font-family: cuIcon;
		display: block;
		content: "\e6a3";
		position: absolute;
		font-size: 34upx;
		color: #8799a3;
		line-height: 75upx;
		width: 60upx;
		text-align: center;
		top: 0;
		bottom: 0;
		right: -20upx;
		margin: auto;
	}
</style>

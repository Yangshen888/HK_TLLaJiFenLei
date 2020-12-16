<template>
	<view style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">志愿者报名</block>
		</cu-custom>
		<view>
			<image src='@/static/ZYZ.png' style="width: 100%;"></image>
		</view>
		<view>
			<!-- #ifndef MP-ALIPAY -->
			<!-- <view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 0px;">
				<view class="title">乡镇/街道/社区</view>
				<picker mode="multiSelector" @change="MultiChange" @columnchange="MultiColumnChange" :value="multiIndex" :range="multiArray" >
					<view class="picker">
						{{multiArray[0][multiIndex[0]]}}，{{multiArray[1][multiIndex[1]]}}，{{multiArray[2][multiIndex[2]]}} 
					</view>
				</picker>
			</view> -->
			<!-- #endif -->
			<view class="cu-form-group margin-top" style="border-radius: 5px;margin:20px 20px 0px;">
				<view class="title">街道/乡镇选择</view>
				<picker @change="PickerChange" :value="index" :range="picker">
					<view class="picker">
						{{index!=-1?picker[index]:"请选择街道/乡镇"}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group margin-top" style="border-radius: 5px;margin:20px 20px 0px;">
				<view class="title">社区选择</view>
				<picker @change="PickerChange2" :value="index2" :range="picker2">
					<view class="picker">
						{{index2!=0?picker2[index2]:picker2[0]}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 0px;">
				<view class="title">垃圾箱房</view>
				<picker 
				@change="grabChange" 
				:value="GrabageRoomList.index" 
				:range="GrabageRoomList.picker" 
				range-key="ljname">
					<view class="picker">
						{{GrabageRoomList.picker[GrabageRoomList.index].ljname}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 0px;">
				<view class="title">请选择年月日</view>
				<picker mode="date" :value="query.startdate" @change="StartDateChange">
					<view class="picker">{{ query.startdate }}</view>
				</picker>
			</view>
			<view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 0px;">
				<view class="title">选择上下午</view>
				<picker 
				@change="APChange" 
				:value="AP.index" 
				:range="AP.picker" 
				range-key="name">
					<view class="picker">
						{{AP.picker[AP.index].name}}
					</view>
				</picker>
			</view>
		</view>
		<button class="btn-logout" @click="YY">预  约</button>
	</view>
</template>

<script>


import uniIcon from '@/components/uni-icon/uni-icon.vue';
import { formateDate, pointInsideCircle, isSameDay } from '@/global/utils.js';
import { handleSignClick, setSignInfo, addSignInfo, getSignInfo, delSignInfo, getInfo, key } from '@/global/index.js';	
	import{getAllGrabRoom,allCommunity} from '@/api/grabage/GrabList.js'
		import {ZYZYY} from '@/api/volunteer/list.js'
export default {
	components: { uniIcon },
	data() {
		return {
			index: -1,
			picker: [''],
			index2: 0,
			picker2: [''],
			GrabageRoomList: {
				picker: [{
					volunteerId: '1',
					name: '1'
				}],
				index:0
			},
			Village: {
				picker: [{
					volunteerId: '1',
					name: '1'
				}],
				index:0
			},
			AP: {
				picker: [{
					id: 'am',
					name: '上午'
				},{
					id: 'pm',
					name: '下午'
				}],
				index:0
			},
			query:{
				grabRoomUUid:"",
				UserId:this.$store.state.userId,
				startdate:formateDate(new Date(),'Y-M-D'),
				AP:"am"
			},
			multiArray: [
				['乡镇/街道'],['社区']
			],
			multiIndex: [0, 0],
			//multiIndex: [0, 0, 0],
			allCommunity:[],
			multiSelector:"",
		};
	},
	computed:{
		
	},
	methods: {
		PickerChange(e) {
			this.index = e.detail.value;
			this.picker2=Array.from(new Set(this.allCommunity.filter(x=>x.towns===this.picker[this.index]).map(x=>x.vname)));
			this.index2=0;
			let community=this.allCommunity.filter(x=>x.vname===this.picker2[0]);
			let guid=community[0].villageUuid;
			//console.log(guid);
			this.getGrablist(guid.toString());
			this.GrabageRoomList.index=0;
		},
		PickerChange2(e) {
			this.index2 = e.detail.value;
			console.log(this.index2);
			let community=this.allCommunity.filter(x=>x.vname===this.picker2[this.index2]);
			let guid=community[0].villageUuid;
			//console.log(guid);
			this.getGrablist(guid.toString());
			this.GrabageRoomList.index=0;
		},
		MultiChange(e) {
			this.multiIndex = e.detail.value
			console.log(this.multiIndex);
			//console.log(this.multiArray[2][this.multiIndex[2]]);
			let community=this.allCommunity.filter(x=>x.vname===this.multiArray[1][this.multiIndex[1]])
			let guid=community[0].villageUuid;
			//console.log(guid);
			this.getGrablist(guid.toString());
			this.GrabageRoomList.index=0;
		},
		MultiColumnChange(e) {
			let data = {
				multiArray: this.multiArray,
				multiIndex: this.multiIndex
			};
			//console.log(data);
			data.multiIndex[e.detail.column] = e.detail.value;
			//console.log(e.detail.value);
			//console.log(e.detail.column);
			//if(e.detail.column)
			switch (e.detail.column) {
				case 0:
					//this.multiArray[1]=Array.from(new Set(this.allCommunity.filter(x=>x.towns===this.multiArray[0][data.multiIndex[0]]).map(x=>x.address)));
					this.multiArray[1]=Array.from(new Set(this.allCommunity.filter(x=>x.towns===this.multiArray[0][data.multiIndex[0]]).map(x=>x.vname)));
					//this.multiArray[2]=Array.from(new Set(this.allCommunity.filter(x=>x.towns===this.multiArray[0][data.multiIndex[0]]&&x.address===this.multiArray[1][0]).map(x=>x.vname)));
					data.multiIndex[1] = 0;
					//data.multiIndex[2] = 0;
					break;
				// case 1:
				//     console.log(data.multiIndex);
				// 	this.multiArray[2]=Array.from(new Set(this.allCommunity.filter(x=>x.towns===this.multiArray[0][data.multiIndex[0]]&&x.address===this.multiArray[1][data.multiIndex[1]]).map(x=>x.vname)));
					
				// 	data.multiIndex[2] = 0;
				// 	break;
			}
			this.multiArray = data.multiArray;
			this.multiIndex = data.multiIndex;
		},
		togglePopup(type) {
			this.type = type;
		},
		getGrablist(guid){
			getAllGrabRoom(guid).then(res=>{
				if(res.data.code==200)
				{
					//console.log(res.data.data);
					this.GrabageRoomList.picker=res.data.data;
					this.query.grabRoomUUid = this.GrabageRoomList.picker[this.GrabageRoomList.index].garbageRoomUuid;
				}
				console.log(this.GrabageRoomList);
			});
		},
		StartDateChange(e) {
			this.query.startdate = e.detail.value;
		},
		grabChange(e) {
			this.GrabageRoomList.index = e.target.value;
			this.query.grabRoomUUid = this.GrabageRoomList.picker[this.GrabageRoomList.index].garbageRoomUuid;
			//console.log(this.GrabageRoomList.picker[this.GrabageRoomList.index].ljname);
		},
		APChange(e){
			this.AP.index = e.target.value;
			this.query.AP = this.AP.picker[this.AP.index].id;
		},
		YY() {
			// if (isTrue === null) {
			// 	uni.showToast({ title: '请检查位置信息状态！', icon: 'none', mask: true, duration: 3000 });
			// 	return;
			// }
			let riqi = true;
			var date1 = new Date();
			var date2 = new Date(this.query.startdate);
			if (date1.getFullYear() > date2.getFullYear()) {
				riqi = false;
			} else if (date1.getFullYear() == date2.getFullYear()) {
				if (date1.getMonth() > date2.getMonth()) {
					riqi = false;
				} else if (date1.getMonth() == date2.getMonth()) {
					if (date1.getDate() > date2.getDate()) {
						riqi = false;
					} else if (date1.getDate() == date2.getDate()) {
						if (date1.getHours() > 12 && this.AP.index == 0) {
							uni.showToast({ title: '请从今天下午开始预约！', icon: 'none', mask: true, duration: 3000 });
							return;
						}
					}
				}
			}
			if(riqi == false){
				uni.showToast({ title: '请从今天开始预约！', icon: 'none', mask: true, duration: 3000 });
				return;
			}
			ZYZYY(this.query).then(res => {
				//被预约过了
				console.log(res);
				if(res.data=='YYNO'){
					uni.showModal({
						title: '提示',
						content: '您以预约过厢房！'
					});   
				}
				else if(res.data == 'YYED')
				{
					uni.showModal({
						title: '提示',
						content: '该箱房已被预约,请预约其他箱房'
					});   
				}
				//添加成功
				else if(res.data.code==200&&res.data.message=='success')
				{
					uni.showModal({
						title: '提示',
						content: '预约成功'
					});  
					if (this.$store.state.roleName.indexOf("志愿者") == -1) {
						this.$store.state.roleName=this.$store.state.roleName+',志愿者';
						console.log(this.$store.state.roleName);
					}
				}
				
			})
		},
		// 选择打卡日期
		bindDateChange: function(e) {
			this.date = e.target.value;
		},

	},
	onLoad(){
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
		allCommunity().then(res=>{
			if(res.data.code==200){
				console.log(res.data.data);
				this.allCommunity=res.data.data.list;
				this.multiArray[0]=res.data.data.list2;
				this.picker=res.data.data.list2;
				this.index=0;
				this.picker2=Array.from(new Set(this.allCommunity.filter(x=>x.towns===this.picker[0]).map(x=>x.vname)));
				this.index2=0;
				let community=this.allCommunity.filter(x=>x.vname===this.picker2[0]);
				let guid=community[0].villageUuid;
				console.log(guid);
				this.getGrablist(guid.toString());
				this.GrabageRoomList.index=0;
				//this.multiArray[1]=Array.from(new Set(this.allCommunity.filter(x=>x.towns===this.multiArray[0][0]).map(x=>x.address)));
				//this.multiArray[1]=Array.from(new Set(this.allCommunity.filter(x=>x.towns===this.multiArray[0][0]).map(x=>x.vname)));
				//this.multiArray[2]=Array.from(new Set(this.allCommunity.filter(x=>x.towns===this.multiArray[0][0]&&x.address===this.multiArray[1][0]).map(x=>x.vname)));
				//console.log(this.multiArray);
				//console.log(this.allCommunity.filter(x=>(x.address===this.multiArray[1][0]).map(x=>x.vname));
			}
		});
		}
	}
};
</script>

<style>
	.btn-logout {
	  margin-top: 100upx;
	  width: 80%;
	  border-radius: 50upx;
	  font-size: 16px;
	  color: #fff;
	  background: linear-gradient(to right, #365fff, #36bbff);
	}
	
	.btn-logout-hover {
	  background: linear-gradient(to right, #365fdd, #36bbfa);
	}
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
	margin: 50upx auto 10upx;
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


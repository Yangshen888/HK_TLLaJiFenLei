<template>
	<view class="content">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">问题反馈</block>
		</cu-custom>
		<view class="page-body">
			<form>
				<view class="cu-form-group" style="border-radius: 5px;margin:0 20px 0px 20px;">
					<view class="title">问题类型</view>
					<input placeholder="请输入问题类型" name="input" v-model="mobile.type"></input>
				</view>

				<!-- #ifndef MP-ALIPAY -->
				<!-- <view class="cu-form-group">
					<view class="title">问题小区</view>
					<picker mode="multiSelector"  @change="MultiChange" @columnchange="MultiColumnChange" :value="multiIndex" :range="multiArray" >
						<view class="picker">
							{{multiArray[0][multiIndex[0]]}}，{{multiArray[1][multiIndex[1]]}} ，{{multiArray[2][multiIndex[2]]}} 
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
							{{index2!=-1?picker2[index2]:"请选择社区"}}
						</view>
					</picker>
				</view>
				<view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 0px;">
					<view class="title">垃圾箱房</view>
					<picker @change="grabChange" :value="GrabageRoomList.index" :range="GrabageRoomList.picker" range-key="ljname">
						<view class="picker">{{ GrabageRoomList.picker[GrabageRoomList.index].ljname }}</view>
					</picker>
				</view>
				<view class="cu-form-group" style="border-radius: 5px;margin:20px 20px 0px;">
					<view class="title">反馈车辆</view>
					<input placeholder="统一标题的宽度" name="input" v-model="mobile.carNum" :disabled="'true'"></input>
				</view>
				<view class="cu-form-group align-start" style="border-radius: 5px;margin:20px 20px 0px;">
					<view class="title">备注</view>
					<textarea style="font-size: 21upx;" maxlength="-1" @input="textareaBInput" placeholder="多行文本输入框" v-model="mobile.remarks"></textarea>
				</view>
				<view>
					<button class="btn-modify" :class="modifyMobile ? 'btn-modify-active':''" :disabled="!modifyMobile" hover-class="btn-modify-hover"
					 @tap="fnModify">提交</button>
				</view>
				<view class="cu-modal" :class="modalName">
					<view class="cu-dialog">
						<view class="cu-bar bg-white justify-end">
							<view class="content">提示</view>
							<view class="action" @tap="hideModal">
								<text class="cuIcon-close text-red"></text>
							</view>
						</view>
						<view class="padding-xl">
							{{info}}
						</view>
					</view>
				</view>
			</form>
		</view>
	</view>
</template>

<script>
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		getAllGrabRoom,
		allCommunity
	} from '@/api/grabage/GrabList.js'
	import {
		getCarInfo,
		addQuestion
	} from '@/api/carApp/carapp.js'

	export default {
		components: {
			uniIcon,
			uniPopup
		},
		data() {
			return {
				index: -1,
				picker: [''],
				index2: -1,
				picker2: [''],
				mobile: {
					type: '',
					remarks: '',
					vguid: '',
					carNum: ''
				},
				type: "",
				remarks: "",
				multiArray: [
					['乡镇/街道'],
					['社区']
				],
				multiIndex: [0, 0],
				//multiIndex: [0, 0, 0],
				allCommunity: [],
				multiSelector: "",
				textareaBValue: "",
				vguid: "",
				carNum: "xxxxx",
				carguid: "",
				modifyMobile: false,
				modalName: '',
				info: 'xxxx',
				GrabageRoomList: {
					picker: [{
						volunteerId: '1',
						name: '1'
					}],
					index: 0
				},
				formcheck: {
					one: false,
					two: false,

				},
			};
		},
		watch: {
			/**
			 * 监听表单数值
			 */
			mobile: {
				handler(newValue) {
					console.log(33333)
					console.log(newValue);
					if (newValue.type != '' && newValue.remarks != '' && newValue.vguid != '' && newValue.carNum != '') {
						this.formcheck.one = true;
						console.log(this.formcheck.one);
					} else {
						this.formcheck.one = false;
					}
				},
				deep: true
			},
			GrabageRoomList: {
				handler(newdata, olddata) {
					console.log(1111);
					//console.log(this.weighData);
					console.log(newdata.index);
					console.log(newdata.picker[newdata.index].ljname == '');
					if (newdata.index >= 0 && newdata.picker[newdata.index].ljname != '') {
						this.formcheck.two = true;
						console.log(this.GrabageRoomList);
					} else {
						this.formcheck.two = false;
					}
				},
				deep: true
			},
			formcheck: {
				handler(newdata, olddata) {
					console.log(999999999);
					console.log(newdata);
					if (newdata.one && newdata.two) {
						this.modifyMobile = true;
					} else {
						this.modifyMobile = false;
					}
					console.log(this.modifyMobile);
				},
				deep: true,
				immediate: true
			},
		},
		methods: {
			grabChange(e) {
				console.log(666666666);
				console.log(e.target.value);
				this.GrabageRoomList.index = e.target.value;
				this.query.grabRoomUUid = this.GrabageRoomList.picker[this.GrabageRoomList.index].garbageRoomUuid;
				console.log(this.GrabageRoomList.picker[this.GrabageRoomList.index].ljname);
			},
			PickerChange(e) {
				this.index = e.detail.value;
				console.log(this.index);
				this.picker2 = Array.from(new Set(this.allCommunity.filter(x => x.towns === this.picker[this.index]).map(x => x.vname)));
				let community = this.allCommunity.filter(x => x.vname === this.picker2[0]);
				let guid = community[0].villageUuid;
				this.mobile.vguid = guid;
				// console.log(guid);
				this.getGrablist(guid.toString(), null);
				this.GrabageRoomList.index = 0;
			},
			PickerChange2(e) {
				this.index2 = e.detail.value;
				let community = this.allCommunity.filter(x => x.vname === this.picker2[this.index2]);
				let guid = community[0].villageUuid;
				this.mobile.vguid = guid;
				// console.log(guid);
				this.getGrablist(guid.toString(), null);
				this.GrabageRoomList.index = 0;
			},
			hideModal(e) {
				this.modalName = null;
				uni.redirectTo({
					url: '/pages/home/index',
				});
			},
			MultiChange(e) {
				this.multiIndex = e.detail.value
				console.log(this.multiIndex);
				//console.log(this.multiArray[2][this.multiIndex[2]]);
				let community = this.allCommunity.filter(x => x.vname === this.multiArray[1][this.multiIndex[1]])
				console.log(community);
				let guid = community[0].villageUuid;
				this.mobile.vguid = guid;
				this.vguid = community[0].villageUuid;

				console.log(this.vguid);
				this.getGrablist(guid.toString(), null);
				this.GrabageRoomList.index = 0;
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
						this.multiArray[1] = Array.from(new Set(this.allCommunity.filter(x => x.towns === this.multiArray[0][data.multiIndex[
							0]]).map(x => x.vname)));
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
			getGrablist(guid, rguid) {
				getAllGrabRoom(guid).then(res => {
					if (res.data.code == 200) {
						console.log(res.data.data);
						this.GrabageRoomList.picker = res.data.data;
						//this.query.grabRoomUUid = this.GrabageRoomList.picker[this.GrabageRoomList.index].garbageRoomUuid;
						if (rguid != null && rguid != '') {
							console.log(rguid);
							console.log(this.GrabageRoomList.picker);
							let index = this.GrabageRoomList.picker.findIndex(x => x.garbageRoomUuid == rguid);

							this.GrabageRoomList.index = index;
							console.log(this.GrabageRoomList.index);
						}
						this.query.grabRoomUUid = this.GrabageRoomList.picker[this.GrabageRoomList.index].garbageRoomUuid;
					}
					console.log(this.GrabageRoomList);
				});
			},
			textareaBInput(e) {
				this.textareaBValue = e.detail.value
			},
			//提交
			fnModify() {
				console.log("22222")
				console.log(JSON.stringify(this.mobile));
				let data = {
					queType: this.mobile.type,
					remarks: this.mobile.remarks,
					villageUuid: this.mobile.vguid,
					carUuid: this.carguid,
					queRoomId: this.GrabageRoomList.picker[this.GrabageRoomList.index].garbageRoomUuid,
				};
				console.log(4444);
				console.log(data);
				addQuestion(data).then(res => {
					console.log(res);
					this.info = res.data.message;
					this.modalName = 'show';
				});
			},
		},
		onLoad() {
			let that = this;
			//#ifdef APP-PLUS
			plus.device.getInfo({
				success: function(e) {
					console.log('getDeviceInfo success: ' + JSON.stringify(e));
					console.log(e.imei);
					let arr = e.imei.toString().split(',');
					console.log(arr[0]);
					//that.IMEI=e.imei;
					getCarInfo({
						imei: e.uuid
					}).then(res => {
						console.log(res);
						that.mobile.carNum = res.data.data.carNum;
						that.carguid = res.data.data.carUuid;
						console.log(this.carNum);
					});
				},
				fail: function(e) {
					console.log('getDeviceInfo failed: ' + JSON.stringify(e));
				}
			});
			//#endif
			allCommunity().then(res => {
				console.log(111111111);
				console.log(res);
				if (res.data.code == 200) {
					console.log(res.data.data);
					this.allCommunity = res.data.data.list;
					this.multiArray[0] = res.data.data.list2;
					this.picker = res.data.data.list2;
					this.index = 0;
					this.picker2 = Array.from(new Set(this.allCommunity.filter(x => x.towns === this.picker[0]).map(x => x.vname)));
					this.index2 = 0;
					let community = this.allCommunity.filter(x => x.vname === this.picker2[0]);
					let guid = community[0].villageUuid;
					console.log(guid);
					this.mobile.vguid = guid;
					//this.multiArray[1]=Array.from(new Set(this.allCommunity.filter(x=>x.towns===this.multiArray[0][0]).map(x=>x.address)));
					//this.multiArray[1]=Array.from(new Set(this.allCommunity.filter(x=>x.towns===this.multiArray[0][0]).map(x=>x.vname)));
					//this.multiArray[2]=Array.from(new Set(this.allCommunity.filter(x=>x.towns===this.multiArray[0][0]&&x.address===this.multiArray[1][0]).map(x=>x.vname)));
					//this.mobile.vguid=this.allCommunity[0].villageUuid;
					// console.log(this.multiArray);
					// console.log(this.multiIndex);
					//console.log(this.allCommunity.filter(x=>(x.address===this.multiArray[1][0]).map(x=>x.vname));
				}
			});
			// getCarInfo().then(res=>{
			// 	console.log(res);
			// });

		}
	};
</script>

<style>
	.page-body {
		background: linear-gradient(to right, #0071d9, #7263e4);
		padding: 20px 0;
		width: 100%;
		height: calc(100%);
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

	.cu-form-group.align-start .title {
		height: 1em;
		margin-top: 26upx;
		line-height: 1em;
	}

	.cu-form-group uni-textarea {
		margin: 26upx 0;
	}

	.btn-modify {
		margin-top: 26upx;
		border-radius: 26upx;
		margin: 20px 20px 0 20px;
		height: 40upx;
		line-height: 40upx;
		font-size: 20px;
		color: #fff !important;
		background: linear-gradient(to right, #30c295,#01686f);
	}

	.btn-modify-active {
		background: linear-gradient(to right, #365fff, #36bbff);
	}
</style>

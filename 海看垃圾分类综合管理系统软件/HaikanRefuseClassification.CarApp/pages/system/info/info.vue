<template>
	<view class="content">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">系统信息</block>
		</cu-custom>
		<view @click="toHome" style="position: absolute; top: 65px;left: 15px;color: #fff;padding: 5px 10px;border: 1px solid #ccc;border-radius: 4px;font-size: 20px;">
			实时定位
		</view>
		<view class="page-body">
			<div class="huang">
				<text class="zhit">设备唯一标识</text>
			</div>
			<div class="bai">
				<view class="neirong">{{ uuid }}</view>
			</div>
			<div class="huang">
				<text class="zhit">设备型号</text>
			</div>
			<div class="bai">
				<view class="neirong">{{ info.model }}</view>
			</div>
			<div class="huang">
				<text class="zhit">绑定车辆</text>
			</div>
			<div class="bai">
				<view class="neirong">{{ carNum }}</view>
			</div>
		</view>
	</view>
</template>

<script>
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
	import {
		getCarInfo
	} from '@/api/carApp/carapp.js'
	export default {
		components: {
			uniIcon,
			uniPopup
		},
		data() {
			return {
				ColorList: [],
				info: '',
				IMEI: '',
				uuid: '',
				carNum: '',
			};
		},
		methods: {
			toHome(){
				console.log("1111111");
				uni.navigateTo({
				    url: '/pages/map/liveMap/liveMap'
				});
				
			},
			getDeviceInfo() {
				let that = this;
				let data = "";
				//#ifdef APP-PLUS
				plus.device.getInfo({
					success: function(e) {
						console.log('getDeviceInfo success: ' + JSON.stringify(e));
						//console.log(e.imei);
						that.uuid = e.uuid;

						getCarInfo({
							imei: e.uuid
						}).then(res => {
							console.log(res);
							that.carNum = res.data.data.carNum;
							//that.carguid=res.data.data.carUuid;
							//console.log(this.carNum);
						});
						//data = e.imei;
						//console.log(data);
						// uni.showModal({
						// 	title: '成功',
						// 	content: ''+JSON.stringify(e),
						// 	showCancel: false,
						// });
					},
					fail: function(e) {
						console.log('getDeviceInfo failed: ' + JSON.stringify(e));
						uni.showModal({
							title: '失败',
							content: '' + JSON.stringify(e),
							showCancel: false,
						});
					}
				});
				//#endif

			}
		},
		onLoad() {
			let that = this;
			var arr = [];
			console.log(this.ColorList);
			let text = 'xxxxxxxxx';
			var sys = uni.getSystemInfo({
				success(res) {
					console.log(res);
					that.info = res;
					console.log(that.info);
					for (let key in res) {
						if (res[key].toString() == '[object Object]') {
							for (let i in res[key]) {
								arr.push({
									name: i + ':' + res[key][i]
								});
							}
						} else {
							arr.push({
								name: key + ':' + res[key]
							});
						}
					}
					console.log(arr);
				}
			});
			for (let i = 0; i < arr.length; i++) {
				this.ColorList.push(arr[i]);
			}
			let demo = {};
			let dev = {};
			//#ifdef APP-PLUS

			// let imei=plus.device.imei;
			// this.ColorList.push({name:"imei:"+imei});

			//#endif
			this.getDeviceInfo();
		}
	};
</script>

<style>
	.page-body {
		margin: 30upx 230upx;
		border-radius: 20upx;
		background: linear-gradient(to top, #7263e4, #0071d9);
	}

	.huang {
		line-height: 45upx;
		text-align: center;
		font-size: 18upx;
		color: #ffb400;
	}
	
	.bai{
		line-height: 45upx;
		text-align: center;
		font-size: 18upx;
		color: #ffffff;
	}

	.neirong {
		color: #eee;
	}
</style>

<template name="mycomponent">
	<view style="padding-top: 1.4rem;">
		<view style="text-align: center;">
			<view class="cu-avatar xl round margin-left" style="margin-top: 18px;background-image:url(https://ossweb-img.qq.com/images/lol/web201310/skin/big10001.jpg)"></view>
			<label>
				<view v-model="user.username" style="font-size: 28px;">{{user.username}}</view>
			</label>
		</view>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="clockin(1)">
			签到</button>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="clockin(2)">
			签退</button>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="pwd">
			修改密码</button>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="loginout">
			退出登录</button>
	</view>
</template>

<script>
	import {
		doClockIn,
		loadclock,
		doClockBack
	} from '../../api/attendancemanagement/attendancelist.js'
	export default {
		name: "mycomponent",
		data() {
			return {
				user: {
					username: ''
				},
				formModel: {
					fields: {
						starttime: '',
						startplace: '',
						endtime: '',
						endplace: '',
					}
				},
				longitude: '',
				latitude: '',
				workstate: '签到'
			}
		},
		mounted() {
			this.user.username = this.$store.state.userName;
		},
		methods: {
			loginout() {
				this.$store.commit('logout');
				// uni.clearStorage();
				uni.reLaunch({
					url: '../login/login'
				})
			},
			pwd() {
				this.$store.commit('logout');
				uni.reLaunch({
					url: '../component/password'
				})
			},
			clockin(id) {
				
				uni.getLocation({
					type: 'wgs84',
					success: (res) => {
						console.log('当前位置的经度：' + res.longitude);
						console.log('当前位置的纬度：' + res.latitude);
						this.longitude = res.longitude; //当前位置的经度
						this.latitude = res.latitude; //当前位置的纬度
						this.clockinsubmit(id);
					},
					fail: (e) => {
						uni.showModal({
							title: '提示',
							content: '获取地点失败是否继续打卡',
							success: (res) => {
								if (res.confirm) {
									this.clockinsubmit(id);
								}
							}
						})
					}
				});
				
				
			},
			clockinsubmit(id) {
				var myDate = new Date();
				let hour = myDate.getHours(); //获取当前小时数(0-23)
				let minute = myDate.getMinutes(); //获取当前分钟数(0-59)
				let second = myDate.getSeconds(); //获取当前秒数(0-59)
				this.formModel.fields.starttime = hour + ":" + minute + ":" + second;
				this.formModel.fields.startplace = this.longitude + "," + this.latitude;
				this.formModel.fields.endtime = hour + ":" + minute + ":" + second;
				this.formModel.fields.endplace = this.longitude + "," + this.latitude;
				if(id==1)
				{
					doClockIn(this.formModel.fields).then(res => {
						uni.showModal({
							title: '提示',
							content: res.data.message,
							showCancel: false
						})
					})	
				}
				if(id==2)
				{
					doClockBack(this.formModel.fields).then(res => {
						uni.showModal({
							title: '提示',
							content: res.data.message,
							showCancel: false
						})
					})	
				}
			}
		}
	}
</script>

<style>
</style>

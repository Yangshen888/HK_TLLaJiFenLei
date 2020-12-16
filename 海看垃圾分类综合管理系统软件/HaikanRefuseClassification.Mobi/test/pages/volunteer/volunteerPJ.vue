<template>
	<view style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">志愿者服务评价</block>
		</cu-custom>
		
		

		<view class="hezi">
			<view class="cu-form-group" style="border-radius: 5px;">
				<view class="title">志愿者</view>
				<picker @change="volunteerChange" :value="volunteerlist.index" :range="volunteerlist.picker" range-key="realName"
				 >
					<view class="picker">
						{{volunteerlist.picker[volunteerlist.index].realName}}
					</view>
				</picker>
			</view>
			
			<uni-section title="服务评价" type="line"></uni-section>
			<view class="example-body">
				<uni-rate :value="star" :margin="5"  />
			</view>

			<button class="cu-btn block bg-blue margin-tb-sm lg margin-top" @click="submitfrom">
				确认
			</button>
		</view>
		
	</view>
</template>

<script>
	import {dogetOneUser,doVolunteeraddScore,doscoreList,getTrashRoomList,addvolunteerPJ} from '@/api/supervisor/manager.js'
		import {
			getalllist
			} from '@/api/volunteer/list'
			import uniRate from '@/components/uni-rate/uni-rate.vue'
			import uniSection from '@/components/uni-section/uni-section.vue'
	export default {
		components: {
			uniRate,
			uniSection
		},
		data() {
			return {
				userinfo:{
					name:'',
					phone:'',
					cardnum:'',
					address:'',
					userUUid:'',
				},
				star:5,
				textareaBValue:'',
				volunteerlist: {
					picker: [{
						volunteerId: '',
						name: ''
					}],
					index: 0,
				},
			}
		},
		onLoad(){
			this.getvolunteerlist();
		},
		methods: {
			getvolunteerlist(){
				getalllist().then(res=>{
					if(res.data.code==200)
					{
						console.log(res.data.data);
						this.volunteerlist.picker=res.data.data;
					}
				});
			},
			
			volunteerChange(e) {
				this.volunteerlist.index = e.target.value;
				console.log(this.volunteerlist.picker[this.volunteerlist.index].systemUserUuid);
				
			},

			submitfrom(){
				let params={guid:this.volunteerlist.picker[this.volunteerlist.index].systemUserUuid,pj:this.star+'颗星'};
					addvolunteerPJ(params).then(res=>{
						console.log(res);
						if(res.data.code==200){
							uni.showModal({
								title: '提示',
								content: res.data.message,
								success: (r) => {
									if (r.confirm) {
										uni.redirectTo({
											url: '/pages/home/index'
										})
									}
								}
							})
						}else{
							
							uni.showModal({
								title: '提示',
								content: res.data.message,
								showCancel: false
							})
						}
						
					})
			}
		}
	}
</script>

<style>
.hezi{
	background-color: #fff;
	margin: 20px 20px 0 20px;
	border-radius:5px ;
}
.hang{
	line-height: 45px;
	border-top: 1px solid #ccc ;
	overflow: hidden;
	margin: 0 15px;
}
.hang:nth-child(1){
	line-height: 45px;
	border-top: 0 ;
}
.zuo{
	float: left;
	font-weight: bold;
}
.you{
	float: right;
	color: #999;
	font-weight: bold;
}
.cu-form-group{
	border-radius: 5px;
}

/* 头条小程序组件内不能引入字体 */
	/* #ifdef MP-TOUTIAO */
	@font-face {
		font-family: uniicons;
		font-weight: normal;
		font-style: normal;
		/* src: url('~@/static/uni.ttf') format('truetype'); */
	}

	/* #endif */

	/* #ifndef APP-NVUE */
	page {
		display: flex;
		flex-direction: column;
		box-sizing: border-box;
		background-color: #efeff4;
		min-height: 100%;
		height: auto;
	}

	view {
		font-size: 28rpx;
		line-height: inherit;
	}

	.example {
		padding: 0 30rpx 30rpx;
	}

	.example-info {
		padding: 30rpx;
		color: #3b4144;
		background: #ffffff;
	}

	.example-body {
		flex-direction: row;
		flex-wrap: wrap;
		justify-content: center;
		padding: 0;
		font-size: 14rpx;
		background-color: #ffffff;
	}

	/* #endif */
	.example {
		padding: 0 30rpx;
	}

	.example-info {
		/* #ifndef APP-NVUE */
		display: block;
		/* #endif */
		padding: 30rpx;
		color: #3b4144;
		background-color: #ffffff;
		font-size: 30rpx;
	}

	.example-info-text {
		font-size: 28rpx;
		line-height: 36rpx;
	}


	.example-body {
		flex-direction: column;
		padding: 30rpx;
		background-color: #ffffff;
	}

	.word-btn-white {
		font-size: 18px;
		color: #FFFFFF;
	}

	.word-btn {
		/* #ifndef APP-NVUE */
		display: flex;
		/* #endif */
		flex-direction: row;
		align-items: center;
		justify-content: center;
		border-radius: 6px;
		height: 48px;
		margin: 15px;
		background-color: #007AFF;
	}

	.word-btn--hover {
		background-color: #4ca2ff;
	}
</style>

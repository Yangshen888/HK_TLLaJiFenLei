<template>
	<view style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/YongHu/changehome">
			<block slot="backText">返回</block>
			<block slot="content">积分兑换</block>
		</cu-custom>

		<view class="hezi">
			<view class="hang">
				<view class="zuo">家庭地址</view>
				<view class="you">{{ userinfo.address }}</view>
			</view>
			<view class="hang">
				<view class="zuo">可用积分</view>
				<view class="you">{{ userinfo.score }}</view>
			</view>
		</view>

		<view class="hezi">
			<!-- <view class="cu-form-group margin-top">
				<view class="title">兑换物品：</view>
				<picker @change="PickerChange" :value="index" :range-key="'gname'" :range="picker">
					<view class="picker">{{ picker[index].gname }}</view>
				</picker>
			</view> -->

			<!-- <view class="cu-form-group margin-top">
				<view class="title">所需积分：</view>
				<view class="you">{{ picker[index].price }}</view>
			</view> -->
			<view class="cu-form-group margin-top">
				<view class="title">所需积分：</view>
				<input  class="you" placeholder="请输入扣除积分" name="input" v-model="deduceScore" @blur="UpNumber($event)" ></input>
			</view>	
			<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitfrom">确认</button>
		</view>
	</view>
</template>

<script>
import { getgoodlist, dogetOneUser, AddchangeGoods } from '@/api/supervisor/manager.js';
export default {
	data() {
		return {
			picker: [],
			index: 0,
			userinfo: {
				address: '',
				score: '',
				homeUUid: ''
			},
			query: {
				name: ''
			},
			deduceScore:'',
			flag:false
		};
	},
	onLoad() {
		//this.dogetgoodlist();
		this.userinfo.homeUUid = this.$store.state.EWM.split('h_')[1];
		console.log(this.userinfo.homeUUid);
		this.submitsearch();
	},
	methods: {
		// dogetgoodlist() {
		// 	let data = {
		// 		guid: this.$store.state.shopid
		// 	};
		// 	getgoodlist(data).then(res => {
		// 		console.log(res);
		// 		if (res.data.code == 200) {
		// 			console.log(this.$store.state.shopid);
		// 			console.log(res.data.data);
		// 			this.picker = res.data.data;
		// 		}
		// 	});
		// },
		UpNumber(e){
			console.log(e.target.value);
			this.flag=new RegExp("^[1-9]([0-9])*$").test(e.target.value);
		},
		PickerChange(e) {
			console.log(e.detail.value);
			this.index = e.detail.value;
		},
		submitsearch() {
			if(this.userinfo.homeUUid==undefined||this.userinfo.homeUUid==null){
				uni.showModal({
					title: '提示',
					content: '暂未找到该用户',
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
			let data={
				guid:this.userinfo.homeUUid
			};
			dogetOneUser(data).then(res => {
				console.log(res);
				if (res.data.code == 200) {
					console.log(res.data.data);
					this.userinfo.address = res.data.data.address;
					// this.userinfo.phone=res.data.data.phone;
					// //this.userinfo.cardnum=res.data.data.oldCard;
					this.userinfo.score = res.data.data.score;
					// this.userinfo.userUUid=res.data.data.userUUid;
				} else {
					if (this.query.Address != '' && this.query.Address != null){
						uni.showModal({
							title: '提示',
							content: res.data.message,
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
				}
			});
		},
		submitfrom() {
			if (this.userinfo.homeUUid != '' && this.userinfo.homeUUid != null ) {
				var scores = this.userinfo.score - this.deduceScore;
				if (scores < 0) {
					uni.showModal({
						title: '提示',
						content: '积分不够',
						showCancel: false
					});
					return;
				}else if(this.flag==true){
					let parmas = {
						// ShopId: this.picker[this.index].shopId,
						ShopId:this.$store.state.shopid,
						SystemUserUuid: this.userinfo.homeUUid,
						DeduceScore: this.deduceScore,
						//GoodsId: this.picker[this.index].goodsId
					};
					console.log(parmas);
					AddchangeGoods(parmas).then(res => {
						console.log(res);
						if (res.data.code == 200) {
							uni.showModal({
								title: '提示',
								content: res.data.message,
								success: r => {
									if (r.confirm) {
										uni.redirectTo({
											url: '/pages/YongHu/changehome'
										});
									}
								}
							});
						} else {
							uni.showModal({
								title: '提示',
								content: res.data.message,
								showCancel: false
							});
						}
					});
				}else{
					uni.showModal({
							title: '提示',
							content: '请输入正整数',
							showCancel: false,
					   });
				}
			} else {
				uni.showModal({
					title: '提示',
					content: '兑换人员不能为空',
					showCancel: false
				});
			}
		}
	}
};
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
</style>

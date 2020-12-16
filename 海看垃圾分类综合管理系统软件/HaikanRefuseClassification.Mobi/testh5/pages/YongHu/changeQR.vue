<template>
	<view >
		<!-- <cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/YongHu/changehome"> -->
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" v-on:click="back">
			<block slot="backText">返回</block>
			<block slot="content">积分兑换</block>
		</cu-custom>
	</view>
</template>

<script>
export default {
	data: () => {
		return {
			phone: '',
			count:"",//倒计时
		};
	},
	created() {
		this.sao();
		// this.goonema();
	},
	methods: {
		back() {
			this.$router.go(-1);
		},
		sao() {
			var that = this;
			uni.scanCode({
				success: function(res) {
					console.log(1);
					if (that.$store.state.HomeAddressUUID == res.result.split('h_')[1]) {
						uni.showModal({
							title: '提示',
							content: '不能为自己兑换商品！',
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
					} else {
						uni.navigateTo({
							url: '/pages/YongHu/changeNumber'
						});
					}
					console.log('条码类型：' + res.scanType);
					console.log('条码内容：' + res.result);
					that.$store.state.EWM = res.result;
				}
			});
		},
		goonema(){
			const time_count=2
			if(!this.timer){
				this.count=time_count;
				this.show=false;
				this.timer=setInterval(()=>{
					if(this.count>0&&this.count<=time_count){
						this.count--;
					}else{
						this.show=true;
						clearInterval(this.timer);
						this.timer=null;
						uni.navigateBack({
						    delta: 1,
						    animationDuration: 200
						});
					}
				},1000)
			}
		}
	}
};
</script>

<style></style>

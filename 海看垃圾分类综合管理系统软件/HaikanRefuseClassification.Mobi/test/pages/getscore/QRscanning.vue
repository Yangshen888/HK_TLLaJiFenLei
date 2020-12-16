<template>
	<view style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/getscore/menu">
			<block slot="backText">返回</block>
			<block slot="content">赋分</block>
		</cu-custom>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				
			}
		},
		created(){
			this.sao();
		},
		methods: {
			sao(){
				var that=this;
				uni.scanCode({
				    success: function (res) {
						console.log(1);
						uni.navigateTo({
							url:'/pages/getscore/addscore'
						})
				        console.log('条码类型：' + res.scanType);
				        console.log('条码内容：' + res.result);
						that.$store.state.EWM=res.result;
				    }
				});
			}
		}
	}
</script>

<style>

</style>

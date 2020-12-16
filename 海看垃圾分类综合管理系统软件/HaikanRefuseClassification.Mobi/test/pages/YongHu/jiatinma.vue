<template>
	<view style="padding-top:2rem">
		<!-- <cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/YongHu/changehome"> -->
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">家庭码绑定</block>
		</cu-custom>
	</view>
</template>

<script>
import { JiatinmaList, AddJiatinma } from '@/api/supervisor/manager.js';
export default {
	data: () => {
		return {
			phone: '',
			vuid: '',
			count:"",//倒计时
		};
	},
	created() {
		this.sao();
		this.goonema();
	},
	methods: {
		sao() {
			var that = this;
			uni.scanCode({
				success: function(res) {
					that.vuid = res.result.split('_')[1];
					console.log('条码类型：' + res.scanType);
					console.log('条码内容：' + that.vuid);
					let data = {
						guid: that.$store.state.userId,
						vuid: that.vuid
					};
					AddJiatinma(data).then(res => {
						if (res.data.code == 200) {
							if (res.data.message == '绑定成功') {
								that.$store.state.HomeAddressUUID=that.vuid;
								uni.showModal({
									title: '提示',
									content: '您已绑定成功！',
									showCancel: false,
									success: function(rest) {
										if (rest.confirm) {
											uni.navigateTo({
												url: '/pages/user/info/QRcode'
											});
										}
									}
								});
							} else if (res.data.message == '您已绑定过家庭码') {
								uni.showModal({
									title: '提示',
									content: '您已绑定过家庭码！',
									showCancel: false,
									success: function(rest) {
										if (rest.confirm) {
											uni.navigateTo({
												url: '/pages/home/index'
											});
										}
									}
								});
							} else {
								uni.showModal({
									title: '提示',
									content: '绑定失败！',
									showCancel: false,
									success: function(rest) {
										if (rest.confirm) {
											uni.navigateTo({
												url: '/pages/user/info/QRcode'
											});
										}
									}
								});
							}
						} else {
							uni.showModal({
								title: '提示',
								content: '未查询到家庭码信息！',
								showCancel: false,
								success: function(rest) {
									if (rest.confirm) {
										uni.navigateTo({
											url: '/pages/user/info/QRcode'
										});
									}
								}
							});
						}
					});
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

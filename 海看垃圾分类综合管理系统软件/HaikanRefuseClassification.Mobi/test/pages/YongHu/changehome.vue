<template>
	<view style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">积分兑换</block>
		</cu-custom>
		<view>
			<image src='https://ljfl.hztlcgj.com/image/JFHD.png' style="width: 100%;height: 150px;"></image>
		</view>
		<view class="nav-list">
			<navigator hover-class="none" :url="item.url" class="nav-li bg-color" navigateTo 
			 :style="[{animation: 'show ' + ((index+1)*0.2+1) + 's 1'}]" v-for="(item,index) in elements" :key="index">
				<view class="nav-title">{{item.title}}</view>
				<!-- <view class="nav-name">{{item.name}}</view> -->
				<text :class="'cuIcon-' + item.cuIcon"></text>
			</navigator>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				elements: [{
						title: '扫描二维码',
						name: 'QR code ',
						color: 'cyan',
						cuIcon: 'scan',
						url:'/pages/YongHu/changeQR'
					}
				],
			}
		},
		methods: {
			
		}
	}
</script>

<style>
.nav-li{
	width:100%;
}
  .bg-color{
	  background:#fff;
	  padding: 20px 10px;
  }
  .nav-title{
  	  font-weight:bold;
  }
  .cuIcon-scan:before {
  	content: "\e689";
	color:#007AFF;
  }
  .cuIcon-edit:before {
  	content: "\e649";
	color:#f37b1d;
  }
</style>

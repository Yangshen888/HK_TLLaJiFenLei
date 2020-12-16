<template>
	<view style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">打卡</block>
		</cu-custom>
		
		<view class="nav-list">
			<navigator hover-class="none" :url="item.url" class="nav-li" navigateTo :class="'bg-'+item.color"
			 :style="[{animation: 'show ' + ((index+1)*0.2+1) + 's 1'}]" v-for="(item,index) in elements" :key="index">
				<view class="nav-title">{{item.title}}</view>
				<view class="nav-name">{{item.name}}</view>
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
						title: '志愿者打卡',
						name: 'Volunteer',
						color: 'cyan',
						cuIcon: 'edit',
						url:'/pages/user/clockin/volunteer'
					},
					{
						title: '督导员打卡',
						name: 'Supervisor',
						color: 'blue',
						cuIcon: 'edit',
						url:'/pages/user/clockin/index'
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
</style>

<template>
  <view style='padding-top:2rem'>
	<swiper class="screen-swiper" :class="dotStyle?'square-dot':'round-dot'" :indicator-dots="true" :circular="true"
			 :autoplay="true" interval="5000" duration="500">
				<swiper-item v-for="(item,index) in swiperList" :key="index">
					<image :src="item.url" mode="aspectFill" v-if="item.type=='image'"></image>
					<video :src="item.url" autoplay loop muted :show-play-btn="false" :controls="false" objectFit="cover" v-if="item.type=='video'"></video>
				</swiper-item>
	</swiper>
	
	<view class="nav-list">
		<navigator hover-class="none" :url="item.url" class="nav-li bg-color" navigateTo
		 :style="[{animation: 'show ' + ((index+1)*0.2+1) + 's 1'}]" v-for="(item,index) in elements" :key="index">
			<view class="nav-title">{{item.title}}</view>
			<text :class="'cuIcon-' + item.cuIcon"></text>
		</navigator>
	</view>

  </view>
</template>

<script>
import { formateDate } from '@/global/utils.js';

  export default {

    data() {
      return {
		  elements: [
			  //{
		  // 		title: '打卡',
		  // 		name: 'Clockin',
		  // 		color: 'cyan',
		  // 		cuIcon: 'location',
				// url:'/pages/user/clockin/index',
				// type:'督导员'
		  // 	},
		  // 	{
		  // 		title: '赋分',
		  // 		name: 'Score',
		  // 		color: 'blue',
		  // 		cuIcon: 'appreciate',
				// url:'/pages/getscore/menu',
				// type:'督导员'
		  // 	},
		  // 	{
		  // 		title: '赋分记录',
		  // 		name: ' Record ',
		  // 		color: 'olive',
		  // 		cuIcon: 'copy',
				// url:'/pages/getscore/scoreRecord',
				// type:'督导员'
		  // 	},
		  	{
		  		title: '志愿者打卡',
		  		name: 'Information',
		  		color: 'green',
		  		cuIcon: 'locationfill',
				url:'/pages/user/clockin/volunteer',
				type:'志愿者'
		  	},
			// {
			// 	title: '服务评价',
			// 	name: 'Information',
			// 	color: 'green',
			// 	cuIcon: 'emoji',
			// 	url:'/pages/volunteer/volunteerPJ',
			// 	type:'督导员'
			// },
			// {
			// 	title: '站内信',
			// 	name: 'Message',
			// 	color: 'green',
			// 	cuIcon: 'sort',
			// 	url:'/pages/message/messageInfo',
			// 	type:'督导员'
			// }
		  ],
		  swiperList: [{
		  	id: 0,
		  	type: 'image',
		  	url: 'https://ljfl.hztlcgj.com/image/head4.png'
		  },
		  {
		  	id: 1,
		  	type: 'image',
		  	url: 'https://ljfl.hztlcgj.com/image/head4.png'
		  }],
		  dotStyle: false,
		  usertype:'督导员',
		  time: formateDate(new Date(), 'h:min:s'), //当前时分秒
	  };
    },
	created(){
		this.usertype=this.$store.state.roleName;
		this.loadcolockin();
		
	},
    mounted() {
		
		this.TowerSwiper('swiperList');
		// 初始化towerSwiper 传已有的数组名即可
	},
	methods:{
		// 初始化towerSwiper
		TowerSwiper(name) {
			let list = this[name];
			for (let i = 0; i < list.length; i++) {
				list[i].zIndex = parseInt(list.length / 2) + 1 - Math.abs(i - parseInt(list.length / 2))
				list[i].mLeft = i - parseInt(list.length / 2)
			}
			this.swiperList = list
		},
		//  v-show="Test(item.type)"
		// Test(v){
		// 	if(this.usertype.indexOf(v)!=-1)
		// 	  return true;
		// },
		loadcolockin(){
			
			if(this.time.substr(0,2)>=12)
			{
				for(var i=0;i<this.elements.length;i++)
				{
					if(this.elements[i].title=="打卡")
					{
						this.elements[i].url="/pages/user/clockin/PMDDY";
					}
					if(this.elements[i].title=="志愿者打卡")
					{
						this.elements[i].url="/pages/user/clockin/PMZZY";
					}
				}
			}else{
				for(var i=0;i<this.elements.length;i++)
				{
					if(this.elements[i].title=="打卡")
					{
						this.elements[i].url="/pages/user/clockin/index";
					}
					if(this.elements[i].title=="志愿者打卡")
					{
						this.elements[i].url="/pages/user/clockin/volunteer";
					}
				}
			}
		},
		
	}
	// watch:{
	// 	'time':function(val){
	// 		var hour=val.substr(0,2);
	// 		if(hour>12)
	// 		{
				
	// 		}else{
				
	// 		}
	// 	}
	// },
  }
</script>

<style>
  button {
    margin-top: 40upx;
  }
  .header {
  
  }
  .header image {
  	width: -webkit-fill-available;
  	height: 160px;
  }
  .bg-color{
	  background:#fff;
	  padding: 20px 10px;
  }
  .nav-title{
	  font-weight:bold;
  }
  .cuIcon-emoji:before {
	color:#7b07ab;
  }
</style>

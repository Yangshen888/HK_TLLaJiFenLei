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
			<navigator
				hover-class="none"
				:url="item.url"
				class="nav-li bg-color"
				navigateTo
				:style="[{ animation: 'show ' + ((index + 1) * 0.2 + 1) + 's 1' }]"
				v-for="(item, index) in elements"
				:key="index"
			>
				<view class="nav-title">{{ item.title }}</view>
				<text :class="'cuIcon-' + item.cuIcon"></text>
			</navigator>
		</view>
	<view class='backblack' @click='hide' >
		<view :hidden="ewmHidden" class="popup_content">
			<tki-qrcode  cid="qrcode2" ref="qrcode" :val="val" :size="size" :onval="onval" :loadMake="loadMake" :usingComponents="true" @result="qrR" />
			<text class="cuIcon-close" style="font-size: 20px;"></text>
	    </view>
	</view>
	
	
		


  </view>
</template>

<script>
	import cmdAvatar from "@/components/cmd-avatar/cmd-avatar.vue"
	import cmdIcon from "@/components/cmd-icon/cmd-icon.vue"
	import cmdCellItem from "@/components/cmd-cell-item/cmd-cell-item.vue"
	import qr from '@/components/tki-qrcode/tki-qrcode.vue'
	import {ewm} from '@/api/XFs.js'
  export default {
	  components: {
	    cmdAvatar,
	    cmdCellItem,
	    cmdIcon,
		qr,
	  },
    data() {
      return {
		  ewmHidden:true,
		  val: 'fdsfsdfsdfasf', // 要生成的二维码值
		  size: 200, // 二维码大小
		  unit: 'upx', // 单位
		  background: '#b4e9e2', // 背景色
		  foreground: '#309286', // 前景色
		  pdground: '#32dbc6', // 角标色
		  icon: '', // 二维码图标
		  iconsize: 40, // 二维码图标大小
		  lv: 3, // 二维码容错级别 ， 一般不用设置，默认就行
		  onval: true, // val值变化时自动重新生成二维码
		  loadMake: false, // 组件加载完成后自动生成二维码
		  src: '', // 二维码生成后的图片地址或base64
		  elements: [{
		  		title: '垃圾分类',
		  		name: 'Clockin',
		  		color: 'cyan',
		  		cuIcon: 'search',
		  				url:'/pages/garbage/typelist',
						type:"用户"
		  	},
		  	{
		  		title: '垃圾厢房',
		  		name: 'Score',
		  		color: 'blue',
		  		cuIcon: 'locationfill',
		  				url:'/pages/YongHu/around',
						type:"用户"
		  	},
		  	{
		  		title: '积分查询',
		  		name: ' Record ',
		  		color: 'olive',
		  		cuIcon: 'moneybagfill',
		  				url:'/pages/YongHu/ScoreSearch',
						type:"用户"
		  	},
		  	{
		  		title: '志愿者预约',
		  		name: 'Information',
		  		color: 'green',
		  		cuIcon: 'friendaddfill',
		  				url:'/pages/YongHu/ZYZ',
						type:"用户"
		  	},
			// {
			// 	title: '商店信息',
			// 	name: 'Information',
			// 	color: 'green',
			// 	cuIcon: 'shopfill',
			// 			url:'/pages/business/list',
			// 			type:"用户"
			// },
			{
				title: '我的家庭码',
				name: 'Information',
				color: 'green',
				cuIcon: 'myfill',
						url:'/pages/user/info/QRcode',
						type:"用户"
			},
			{
				title: '用户反馈',
				name: 'Response',
				color: 'pink',
				cuIcon: 'question',
						url:'/pages/person/responsecreate',
						type:"用户"
			},
			{
				title: '帮助中心',
				name: 'Record',
				color: 'red',
				cuIcon: 'recordfill',
						url:'/pages/person/helpcenter',
						type:"用户"
			},
			
			],
			swiperList: [
				{
					id: 0,
					type: 'image',
					url: 'https://ljfl.hztlcgj.com/image/head4.png'
				},
				{
					id: 1,
					type: 'image',
					url: 'https://ljfl.hztlcgj.com/image/head4.png'
				}
			],
			dotStyle: false,
			usertype: ''
		};
	},
	mounted() {
		this.TowerSwiper('swiperList');
		// 初始化towerSwiper 传已有的数组名即可
	},
	created() {
		this.usertype = this.$store.state.roleName;
		console.log(this.usertype)
		let count = this.$store.state.EWMShowCount;
		if (count) {
			this.showMyRqcod();
		}
		console.log(this.$store.state.roleName.indexOf("督导员"))
		if (this.$store.state.roleName.indexOf("督导员")!=-1) {
			this.elements=this.elements.concat({
				title: '督导员巡查',
				name: 'Supervisor',
				color: 'red',
				cuIcon: 'recordfill',
						url:'/pages/supervisorInspection/scanQR',
						type:"督导员"
			})
		}
		
	},
	methods: {
		// 初始化towerSwiper
		TowerSwiper(name) {
			let list = this[name];
			for (let i = 0; i < list.length; i++) {
				list[i].zIndex = parseInt(list.length / 2) + 1 - Math.abs(i - parseInt(list.length / 2));
				list[i].mLeft = i - parseInt(list.length / 2);
			}
			this.swiperList = list;
		},
		//  v-show='Test(item.type)'
		// Test(v){
		// 	if(this.usertype.indexOf(v)!=-1)
		// 	  return true;
		// },
		around() {
			uni.redirectTo({
				url:"/pages/YongHu/around",
			})
		},
		typelist(){
			uni.redirectTo({
				url:"/pages/garbage/typelist",
			})
		},
		scoreget(){
			uni.redirectTo({
				url:"/pages/YongHu/ScoreSearch",
			})
		},
		ZYZ(){
			uni.redirectTo({
				url:"/pages/YongHu/ZYZ",
			})
		},
		myQInfo(){
			uni.redirectTo({
				url:"/pages/user/info/info",
			})
		},
		//普通用户已进入就显示二维码
		showMyRqcod(){
			this.$store.state.EWMShowCount=false;
			if(this.usertype==='普通用户'){
				this.ewmHidden=false;
				 ewm(this.$store.state.userId).then(res=>{
			    	console.log(res);
			    	if(res.data.code==200)
			    	  this.val=res.data.data;
			    });
				
			}  
		},
		//点击遮罩层隐藏
		hide(){
			this.ewmHidden=true;
		}
	}
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
  .cuIcon-search:before {
  	font-weight: bold;
  	color: #1AAD19;
  	font-size: 30px;
  }
  
  .cuIcon-locationfill:before {
  	font-weight: bold;
  	color: #0081ff;
  	font-size: 30px;
  }
  
  .cuIcon-moneybagfill:before {
  	color:#fbbc02;
	font-size: 30px;
	font-weight: bold;
  }
  
  .cuIcon-recordfill:before {
  	color:#aa5500;
  	font-size: 30px;
  	font-weight: bold;
  }
  
  .cuIcon-friendaddfill:before {
  	color:#f57f77;
	font-size: 30px;
	font-weight: bold;
  }
  
  .cuIcon-shopfill:before {
  	color:#955ff3;
	font-size: 30px;
	font-weight: bold;
  }
  .cuIcon-myfill:before {
  	color:#7c7c7c;
	font-size: 30px;
	font-weight: bold;
  }
  .bg-color{
  	  background:#fff;
  	  padding: 20px 10px;
  }
  .nav-title{
  	  font-weight:bold;
  }
  .popup_overlay {
   
  		position: fixed;
  		top: 0%;
  		left: 0%;
  		width: 100%;
  		height: 100%;
  		background-color: black;
  		z-index: 1001;
  		-moz-opacity: 0.8;
	opacity: 0.8;
  		filter: alpha(opacity=88);
  	}
   
  	.popup_content {
  		position: fixed;
  		top: 50%;
  		left: 58%;
  		width: 230px;
  		height: 180px;
  		margin-left: -270upx;
  		margin-top: -270upx;
  		border: 10px solid white;
  		background-color: white;
  		z-index: 1002;
  		overflow: auto;
  		border-radius: 20upx;
		text-align:center;
		padding-top: 15px;
  	}
   
  	.popup_title {
  		padding-top: 20upx;
  		width: 480upx;
  		text-align: center;
  		font-size: 32upx;
  	}
   
  	.popup_textarea_item {
  		padding-top: 5upx;
  		height: 240upx;
  		width: 440upx;
  		background-color: #F1F1F1;
  		margin-top: 30upx;
  		margin-left: 20upx;
  	}
   
  	.popup_textarea {
  		width: 410upx;
  		font-size: 26upx;
  		margin-left: 20upx;
  	}
   
  	.popup_button {
  		color: white;
  		background-color: #4399FC;
  		border-radius: 20upx;
  	}
</style>

<template>
  <view>
    <!-- 顶部导航栏组件 -->
    <!-- <cmd-nav-bar :title="title"></cmd-nav-bar> -->
    <!-- 内容区 start -->
    <cmd-page-body type="top-bottom">
      <!-- #ifdef H5 -->
      <cmd-transition name="fade-up">
        <home v-if="current == 0"></home>
		<userhome v-if="current == 1"></userhome>
		<shophome v-if="current == 2"></shophome>
      </cmd-transition>
      <!-- #endif -->
      <!-- #ifndef H5 -->
      <cmd-transition v-if="current == 0" name="fade-up">
        <home></home>
      </cmd-transition>
      <cmd-transition v-if="current == 1" name="fade-up">
        <userhome></userhome>
      </cmd-transition>
			<cmd-transition v-if="current == 2" name="fade-up"><shophome></shophome></cmd-transition>
      <!-- #endif -->
    </cmd-page-body>
    <!-- 内容区 end -->
    <!-- 底部导航栏组件 -->
    <cmd-bottom-nav background-color="#ffffff" font-color="#3665ff" active-font-color="#3669ff" @click="getBottomNavCurrent"
      :current="current" :list="list"></cmd-bottom-nav>
  </view>
</template>

<script>
import home from './home/home.vue';
import person from './person/person.vue';
import userhome from './home/userhome.vue';
import shophome from './home/shophome.vue';
import cmdNavBar from '@/components/cmd-nav-bar/cmd-nav-bar.vue';
import cmdBottomNav from '@/components/cmd-bottom-nav/cmd-bottom-nav.vue';
import cmdPageBody from '@/components/cmd-page-body/cmd-page-body.vue';
import cmdTransition from '@/components/cmd-transition/cmd-transition.vue';

export default {
	components: {
		home,
		person,
		userhome,
		shophome,
		cmdBottomNav,
		cmdNavBar,
		cmdPageBody,
		cmdTransition
	},

	data() {
		return {
			bodyHeight: 0,
			title: '首页',
			// 底部导航栏的默认选中
			current: 0,
			// 底部导航栏的菜单项
			list: [
				{
					pagePath: '/pages/bottom-nav/home/home',
					text: '志愿者',
					icon: 'home'
				},
				// {
				//   "pagePath": "/pages/bottom-nav/person/person",
				//   "text": "商户",
				//   // 字体图标不可与图片共显
				//   "icon": "user",
				//   // 以导入方式传入的图片最佳 import srcImg from "@/static/xxx.png"
				//   // src 大小限制为40kb，建议尺寸为 81px * 81px
				//   // "src": "../../static/home.png",
				//   // "srcSelect": "../../static/homeHL.png"
				// },
				{
					pagePath: '/pages/bottom-nav/home/home',
					text: '用户',
					// 字体图标不可与图片共显
					icon: 'user'
					// 以导入方式传入的图片最佳 import srcImg from "@/static/xxx.png"
					// src 大小限制为40kb，建议尺寸为 81px * 81px
					// "src": "../../static/home.png",
					// "srcSelect": "../../static/homeHL.png"
				},
				{
					pagePath: '/pages/bottom-nav/home/home',
					text: '商户',
					icon: 'user'
				}
			]
		};
	},

    methods: {
      /**
       * 点击底部导航栏的菜单项
       * 得到的项进行跳转切换
       */
      getBottomNavCurrent(e) {
		  if(this.$store.state.roleName.indexOf("志愿者") == -1){
				this.current=1;
		  }else{
			  this.$store.state.seltab=e.select;
			  this.$store.state.seltit=e.item.text;
			  this.current = this.$store.state.seltab;
			  this.title = this.$store.state.seltit;
		  }
      },
	  
	  
    },
	created(){
		this.current = this.$store.state.seltab;
		console.log(this.$store.state);
		console.log(this.$store.state.roleName.indexOf("志愿者"));
		// if(this.$store.state.roleName.indexOf("督导员")!=-1)
		// {
		// 	this.list[0].text="督导员";
			
		// }
		if (this.$store.state.roleName.indexOf("志愿者") == -1) {
			this.current=1;
			this.list.shift();
		}
		if (this.$store.state.roleName.indexOf('商户') == -1) {
			//this.list[2].text = '商户';
			this.list.pop();
		}
		if(this.$store.state.userDepartmentName=='2'){
			this.list.pop();
		}
	}
  }
</script>

<style>
</style>

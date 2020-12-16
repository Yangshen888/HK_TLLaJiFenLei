<template>
  <view style='padding-top:2rem'>
	<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
		<block slot="backText">返回</block>
		<block slot="content">个人信息</block>
	</cu-custom>
	<view class="person-head">
	  <cmd-avatar src="https://avatar.bbs.miui.com/images/noavatar_small.gif" style="margin:0 auto;" @click="fnInfoWin" size="lg" ></cmd-avatar>
	</view>

	
      <cmd-transition name="fade-up">
        <view class="myhead">
          <cmd-cel-item title="我的二维码" slot-right arrow class="pa" @click="QRopen">
            <text class="cuIcon-qr_code"></text>
          </cmd-cel-item>
          <cmd-cel-item title="名字" :addon="userinfo.userName"  style="background:#fff;"></cmd-cel-item>
          <cmd-cel-item title="联系方式" :addon="userinfo.phone"  style="background:#fff;"></cmd-cel-item>
          <!-- <cmd-cel-item title="修改密码" @click="fnClick('modify')" arrow class="pa"></cmd-cel-item>
		  <cmd-cel-item title="解除绑定" @click="relieve" arrow class="pa"></cmd-cel-item> -->
		  <button class="cu-btn block bg-blue margin-tb-sm lg" @click="loginout">
		  	退出登录</button>
        </view>
		
      </cmd-transition>
	  
  </view>
</template>

<script>
  import cmdTransition from "@/components/cmd-transition/cmd-transition.vue"
  import cmdCelItem from "@/components/cmd-cell-item/cmd-cell-item.vue"
  import cmdAvatar from "@/components/cmd-avatar/cmd-avatar.vue"

  export default {
    components: {
      cmdTransition,
      cmdCelItem,
      cmdAvatar
    },

    data() {
      return {
		  userinfo:'',
		  score:"0",
	  };
    },
	
    mounted() {
		
		this.userinfo=this.$store.state;
		console.log(this.userinfo);
	},
    
    methods:{
      loginout() {
      	this.$store.commit('logout');
      	// uni.clearStorage();
      	uni.reLaunch({
      		url: '/pages/login/login'
      	})
      },
	  fnClick(type){
	    if(type == 'modify'){
	      uni.navigateTo({
	        url:'/pages/user/modify/modify'
	      })
	    }
	  },
	  QRopen(){
		  uni.navigateTo({
		    url:'/pages/user/info/QRcode'
		  })
	  },
	  relieve(){
		  uni.redirectTo({
		  	url: '/pages/login/bind?id=2'
		  })
	  }
	  
    }
  }
</script>

<style>
  .btn-logout {
    margin-top: 100upx;
    width: 80%;
    border-radius: 50upx;
    font-size: 16px;
    color: #fff;
    background: linear-gradient(to right, #365fff, #36bbff);
  }

  .btn-logout-hover {
    background: linear-gradient(to right, #365fdd, #36bbfa);
  }
  .person-head {
    display: flex;
    flex-direction: row;
    align-items: center;
    height: 150px;
    /* padding-left: 20px; */
    /* background: linear-gradient(to right, #365fff, #36bbff); */
    background: linear-gradient(to right, #20c09f, #36ffa2);
  }
  
  .person-head-box {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: flex-start;
    margin-left: 10px;
  }
  
  .person-head-nickname {
    font-size: 18px;
    font-weight: 500;
    color: #fff;
  }
  
  .person-head-username {
    font-size: 14px;
    font-weight: 500;
    color: #fff;
  }
  
  .person-list {
    line-height: 0;
  }
  .myhead{
	      position: fixed;
	      background: #ffffff;
	      width: 100%;
	      height: 100%;
  }
  .pa{
	  margin:8px 0px;
	  background:#fff;
  }
</style>

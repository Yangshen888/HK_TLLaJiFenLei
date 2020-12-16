<template>
  <view>
	<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/person/person">
		<block slot="backText">返回</block>
		<block slot="content">个人信息</block>
	</cu-custom>
	
      <cmd-transition name="fade-up">
        <view>
          <cmd-cel-item title="头像" slot-right arrow>
            <cmd-avatar src="../../../static/QRcode/logo.png"></cmd-avatar>
          </cmd-cel-item>
          <cmd-cel-item title="积分" :addon="score" arrow></cmd-cel-item>
          <cmd-cel-item title="姓名" :addon="userinfo.userName" arrow></cmd-cel-item>
          <cmd-cel-item title="角色" :addon="userinfo.userDepartmentName" arrow></cmd-cel-item>
          <cmd-cel-item title="修改密码" @click="fnClick('modify')" arrow></cmd-cel-item>
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
		  score:"0"
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
</style>

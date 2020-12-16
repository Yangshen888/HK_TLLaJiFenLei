<template>
  <view style='padding-top:2rem'>
	  <cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
	  	<block slot="backText">返回</block>
	  	<block slot="content">个人中心</block>
	  </cu-custom>
	  
<!--    <view class="person-head">
      <cmd-avatar src="../../static/QRcode/logo.png" @click="fnInfoWin" size="lg" :make="{'background-color': '#fff'}"></cmd-avatar>
      <view class="person-head-box">
        <view class="person-head-nickname">{{username}}</view>
        <view class="person-head-username">ID：{{usertype}}</view>
      </view>
    </view> -->
    <view class="person-list">
		<cmd-cell-item title="个人信息" slot-left arrow @click="fnInfoWin">
		  <cmd-icon type="bullet-list" size="24" color="#368dff"></cmd-icon>
		</cmd-cell-item>
      <cmd-cell-item title="本机信息" slot-left arrow @click="myequipment">
        <cmd-icon type="bullet-list" size="24" color="#368dff"></cmd-icon>
      </cmd-cell-item>
      <cmd-cell-item title="消息通知" slot-left arrow>
        <cmd-icon type="message" size="24" color="#368dff"></cmd-icon>
      </cmd-cell-item>
      <cmd-cell-item title="我的二维码" slot-left arrow @click="myQRcode">
        <cmd-icon type="settings" size="24" color="#368dff"></cmd-icon>
      </cmd-cell-item>
	  
	  <button class="btn-logout" @click="loginout">退出登录</button>
    </view>
  </view>
</template>

<script>
  import cmdAvatar from "@/components/cmd-avatar/cmd-avatar.vue"
  import cmdIcon from "@/components/cmd-icon/cmd-icon.vue"
  import cmdCellItem from "@/components/cmd-cell-item/cmd-cell-item.vue"

  export default {
    components: {
      cmdAvatar,
      cmdCellItem,
      cmdIcon
    },
    data() {
      return {
		  username:'',
		  usertype:''
	  };
    },
	created() {
		// console.log(this.$store.state.userName);
		this.username=this.$store.state.userName;
		this.usertype=this.$store.state.userDepartmentName;
	},
    methods: {
      /**
       * 打开用户信息页
       */
      fnInfoWin() {
        uni.navigateTo({
          url: '/pages/user/info/info'
        })
      },
	  myequipment(){
		  uni.navigateTo({
		    url: '/pages/user/info/equipment'
		  })
	  },
	  myQRcode(){
		  uni.navigateTo({
		    url: '/pages/user/info/QRcode'
		  })
	  },
	  loginout(){
		  uni.navigateTo({
		    url: '/pages/login/login'
		  })
	  }
    }
  }
</script>

<style>
  .person-head {
    display: flex;
    flex-direction: row;
    align-items: center;
    height: 150px;
    padding-left: 20px;
    background: linear-gradient(to right, #365fff, #36bbff);
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

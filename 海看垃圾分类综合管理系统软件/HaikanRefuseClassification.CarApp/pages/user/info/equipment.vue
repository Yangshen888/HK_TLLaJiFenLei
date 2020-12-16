<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/person/person">
						<block slot="backText">返回</block>
						<block slot="content">本机信息</block>
		</cu-custom>
		
		<cmd-transition name="fade-up">
		  <view>
		    <cmd-cel-item title="手机型号" :addon="info.model" arrow></cmd-cel-item>
		    <cmd-cel-item title="操作系统版本" :addon="info.system" arrow></cmd-cel-item>
		    <cmd-cel-item title="定位信息" :addon="info.location" arrow></cmd-cel-item>
		    <cmd-cel-item title="垃圾箱房编号" :addon="info.trashnum" arrow></cmd-cel-item>
		  </view>
		</cmd-transition>
	</view>
</template>

<script>
  import cmdPageBody from "@/components/cmd-page-body/cmd-page-body.vue"
  import cmdTransition from "@/components/cmd-transition/cmd-transition.vue"
  import cmdCelItem from "@/components/cmd-cell-item/cmd-cell-item.vue"
  import cmdAvatar from "@/components/cmd-avatar/cmd-avatar.vue"

  export default {
    components: {
      cmdPageBody,
      cmdTransition,
      cmdCelItem,
      cmdAvatar
    },

    data() {
      return {
		  info:{
			  model:'',//手机型号
			  system:'',//操作系统版本
			  location:'',
			  trashnum:''
		  }
	  };
    },

    mounted() {
		this.getphoneinfo();
	},
    
    methods:{
		getphoneinfo(){
			let that=this;
			    uni.getSystemInfo({
			        success: function (res) {
			            that.info.brand=res.brand;
			            that.info.model=res.model;
			            that.info.system=res.system;
			            
			        }
			    });
				uni.getLocation({
					type: 'wgs84',
					success: (res) => {
						console.log('当前位置的经度：' + res.longitude);
						console.log(res);
						this.longitude = res.longitude; //当前位置的经度
						this.latitude = res.latitude; //当前位置的纬度
					},
					fail: (e) => {
						
					}
				});
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
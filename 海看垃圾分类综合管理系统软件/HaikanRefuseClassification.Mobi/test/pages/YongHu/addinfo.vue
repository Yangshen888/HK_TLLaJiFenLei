<template>
	<view class="" style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/login/login">
			<block slot="backText">返回</block>
			<block slot="content">完善信息</block>
		</cu-custom>
		<view class="content reg_bg">
			
			
			<view class="list">
				<view class="list-call">
					<text class="cuIcon-myfill"></text>
					<input class="biaoti" v-model="data.realName"  maxlength="11" placeholder="请输入用户名" />
				</view>
				<view class="list-call">
					<text class="cuIcon-home"></text>
					
					<pick-regions :default-regions="defaultRegions" @getRegions="handleGetRegions" style="position: absolute;left:65px;">
					    <input  v-model="data.address" placeholder="请选择省市区" disabled />
					</pick-regions>
					
				</view>
				<view class="list-call" >
					<text class="cuIcon-calendar"></text>
					<input class="biaoti" v-model="detailAdress"   placeholder="请输入详细地址(到单元号)" />
				</view>
				<view class="list-call">
					<text class="cuIcon-vipcard"></text>
					<input class="biaoti" v-model="data.oldCard" type="number" placeholder="请输入老年卡号"  />
				</view>

			</view>
			
			<view class="dlbutton" hover-class="dlbutton-hover" @tap="bindLogin">
				<text>提交</text>
			</view>
			
		</view>
		
	</view>
</template>

<script>
	import pickRegions from '@/components/pick-regions/pick-regions.vue'
	import {doCreateUser,getRegCode} from '@/api/supervisor/manager.js'
	import {GetUserInfo,editUserInfo} from '@/api/user/userinfo.js'
	
	var tha,js;
	export default {
		components:{
		    pickRegions
		},
		onLoad(){
			tha = this;
			this.getinfo();
		},
		onUnload(){
			clearInterval(js)
			this.second = 0;
		},
		data() {
			return {
				data:[],
				userinfo:{
					OldCard:'',
					SystemUserUuid:'',
					Address:'',
					RealName:''
				},
				detailAdress:'',
				code:'',
				xieyi:true,
				showPassword:false,
				showPassword2:false,
				second:0,
				regions:[],
				defaultRegions:['浙江省','杭州市','桐庐县'],
				regpwd:false,
				regphone:false,
			};
		},
		computed:{

			regionsName(){
			    // 转为字符串
			    return this.regions.map(item=>item.name).join(' ')
			}
		},
		methods: {
			getinfo(){
				GetUserInfo(this.$store.state.userId).then(res=>{
					console.log(res.data)
					if(res.data.code==200){
						this.data=res.data.data;
					}
				})
			},




			// 获取选择的地区
			handleGetRegions(regions){
				var address='';
				for(var i=0;i<regions.length;i++)
				{
					address+=regions[i].name;
				}
			    this.data.address = address
			},


			bindLogin() {
				console.log(this.data.oldCard);
				this.userinfo.Address+="-"+this.detailAdress;
				this.userinfo.OldCard=this.data.oldCard;
				this.userinfo.RealName=this.data.realName;
				this.userinfo.SystemUserUuid=this.$store.state.userId;

				if (this.userinfo.RealName =='' ) {
				    uni.showToast({
				        icon: 'none',
				        title: '请输入用户名'
				    });
				    return;
				}
				if (this.detailAdress =='' ) {
				    uni.showToast({
				        icon: 'none',
				        title: '请输入详细地址'
				    });
				    return;
				}
				// if (this.userinfo.OldCard =="" ) {
				//     uni.showToast({
				//         icon: 'none',
				//         title: '请输入老年卡号'
				//     });
				//     return;
				// }
				
				
				
				
				console.log(this.userinfo);
				
				editUserInfo(this.userinfo).then(res=>{
					console.log(res.data)
					if(res.data.code==200)
					{
						uni.showToast({title:res.data.message});
						
						this.$store.state.seltab=1;
						uni.reLaunch({
							url: '../home/index'
						});
					}else{
						uni.showToast({title:res.data.message,icon:'none'});
					}
				});

				
		    }
		}
	}
</script>

<style>
	.reg_bg{
		    background: #fff;
		    position: absolute;
		    width: 100%;
		    height: 100%;
	}
	.content {
		display: flex;
		flex-direction: column;
		justify-content:center;
	}
	.header {
		width:161upx;
		height:161upx;
		background:rgba(63,205,235,1);
		box-shadow:0upx 12upx 13upx 0upx rgba(63,205,235,0.47);
		border-radius:50%;
		margin-top: 30upx;
		margin-left: auto;
		margin-right: auto;
	}
	.header image{
		width:161upx;
		height:161upx;
		border-radius:50%;
	}
	
	.list {
		display: flex;
		flex-direction: column;
		padding-top: 50upx;
		padding-left: 70upx;
		padding-right: 70upx;
		
		position:relative;
		top:-50px;
	}
	.list-call{
		display: flex;
		flex-direction: row;
		justify-content: space-between;
		align-items: center;
		height: 100upx;
		color: #333333;
		border-bottom: 1upx solid rgba(230,230,230,1);
	}
	.list-call .img{
		width: 40upx;
		height: 40upx;
	}
	.list-call .biaoti{
		flex: 1;
		text-align: left;
		font-size: 32upx;
		line-height: 100upx;
		margin-left: 16upx;
	}
	.yzm {
		color: #FF7D13;
		font-size: 24upx;
		line-height: 64upx;
		padding-left: 10upx;
		padding-right: 10upx;
		width:auto;
		height:64upx;
		border:1upx solid #FFA800;
		border-radius: 50upx;
	}
	.yzms {
		color: #999999 !important;
		border:1upx solid #999999;
	}
	.dlbutton {
		color: #FFFFFF;
		font-size: 34upx;
		width:470upx;
		height:100upx;
		background:linear-gradient(-90deg,rgba(63,205,235,1),rgba(188,226,158,1));
		box-shadow:0upx 0upx 13upx 0upx rgba(164,217,228,0.2);
		border-radius:50upx;
		line-height: 100upx;
		text-align: center;
		margin-left: auto;
		margin-right: auto;
		/* margin-top: 100upx; */
	}
	.dlbutton-hover {
		background:linear-gradient(-90deg,rgba(63,205,235,0.9),rgba(188,226,158,0.9));
	}
	.xieyi{
		display: flex;
		flex-direction: row;
		justify-content: center;
		align-items: center;
		font-size: 30upx;
		margin-top: 80upx;
		color: #FFA800;
		text-align: center;
		height: 40upx;
		line-height: 40upx;
	}
	.list-call text{
		font-size: 20px;
	}
	.xieyi image{
		width: 40upx;
		height: 40upx;
	}
</style>

<template>
	<view class="">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/login/login">
			<block slot="backText">返回</block>
			<block slot="content">注册</block>
		</cu-custom>
		<view class="content reg_bg">
			
			
			<view class="list">
				<view class="list-call">
					<text class="cuIcon-myfill"></text>
					<input class="biaoti" v-model="userinfo.loginName"  maxlength="11" placeholder="用户" />
				</view>
				<view class="list-call">
					<text class="cuIcon-unlock"></text>
					<input class="biaoti" v-model="userinfo.passWord" type="text" maxlength="32" placeholder="登录密码" :password="!showPassword" />
					<image class="img" :src="showPassword?'/static/shilu-login/op.png':'/static/shilu-login/cl.png'" @tap="display"></image>
				</view>
				<view class="list-call">
					<text class="cuIcon-lock"></text>
					<input class="biaoti" v-model="userinfo.passWord2" type="text" maxlength="32" placeholder="确认登录密码" :password="!showPassword2" />
					<image class="img" :src="showPassword2?'/static/shilu-login/op.png':'/static/shilu-login/cl.png'" @tap="display2"></image>
				</view>
				<view class="list-call">
					<text class="cuIcon-home"></text>
					
					<pick-regions :default-regions="defaultRegions" @getRegions="handleGetRegions" style="position: absolute;left:65px;">
					    <input  v-model="userinfo.address" placeholder="选择省市区" disabled />
					</pick-regions>
					
				</view>
				<view class="list-call">
					<text class="cuIcon-vipcard"></text>
					<input class="biaoti" v-model="userinfo.oldCard" type="number" maxlength="11" placeholder="老年卡号" />
				</view>
				<view class="list-call">
					<text class="cuIcon-mobilefill"></text>
					<input class="biaoti" v-model="userinfo.phone" type="number" maxlength="11" placeholder="手机" />
				</view>
				<!-- <view class="list-call">
					<text class="cuIcon-safe"></text>
					<input class="biaoti" v-model="code" type="number" maxlength="4" placeholder="验证码" />
					<view class="yzm" :class="{ yzms: second>0 }" @tap="getcode">{{yanzhengma}}</view>
				</view> -->
				
			</view>
			
			<view class="dlbutton" hover-class="dlbutton-hover" @tap="bindLogin">
				<text>注册</text>
			</view>
			
			<view class="xieyi">
				<image @tap="xieyitong" :src="xieyi==true?'/static/shilu-login/ty1.png':'/static/shilu-login/ty0.png'"></image>
				<text @tap="xieyitong"> 同意</text>
				<navigator url="blog?id=1" open-type="navigate">《软件用户协议》</navigator>
			</view>
		</view>
		
	</view>
</template>

<script>
	import pickRegions from '@/components/pick-regions/pick-regions.vue'
	import {doCreateUser} from '@/api/supervisor/manager.js'
	
	var tha,js;
	export default {
		components:{
		    pickRegions
		},
		onLoad(){
			tha = this;
			
		},
		onUnload(){
			clearInterval(js)
			this.second = 0;
		},
		data() {
			return {
				userinfo:{
					loginName:'',
					passWord:'',
					passWord2:'',
					code:'',
					address:'',
					oldCard:'',
					phone:''
				},
				code:'',
				xieyi:true,
				showPassword:false,
				showPassword2:false,
				second:0,
				regions:[],
				defaultRegions:['广东省','广州市','番禺区']
			};
		},
		computed:{
			yanzhengma(){
				if(this.second==0){
					return '获取验证码';
				}else{
					if(this.second<10){
						return '重新获取0'+this.second;
					}else{
						return '重新获取'+this.second;
					}
				}
			},
			regionsName(){
			    // 转为字符串
			    return this.regions.map(item=>item.name).join(' ')
			}
		},
		methods: {
			// 获取选择的地区
			handleGetRegions(regions){
				var address='';
				for(var i=0;i<regions.length;i++)
				{
					address+=regions[i].name;
				}
			    this.userinfo.address = address
			},
			display() {
			    this.showPassword = !this.showPassword
			},
			display2() {
			    this.showPassword2 = !this.showPassword2
			},
			xieyitong(){
				this.xieyi = !this.xieyi;
			},
			getcode(){
				if(this.second>0){
					return;
				}
				this.second = 60;
				// uni.request({
				//     url: 'http://***/getcode.html', //仅为示例，并非真实接口地址。
				//     data: {phoneno:this.phoneno,code_type:'reg'},
				// 	method: 'POST',
				// 	dataType:'json',
				//     success: (res) => {
				// 		if(res.data.code!=200){
				// 			uni.showToast({title:res.data.msg,icon:'none'});
				// 		}else{
				// 			uni.showToast({title:res.data.msg});
				// 			js = setInterval(function(){
				// 				tha.second--;
				// 				if(tha.second==0){
				// 					clearInterval(js)
				// 				}
				// 			},1000)
				// 		}
				//     }
				// });
			},
		    bindLogin() {
				if (this.xieyi == false) {
				    uni.showToast({
				        icon: 'none',
				        title: '请先阅读《软件用户协议》'
				    });
				    return;
				}
				
				if (this.userinfo.loginName=='' || this.userinfo.loginName==null) {
				    uni.showToast({
				        icon: 'none',
				        title: '请输入账号'
				    });
				    return;
				}
				if (this.userinfo.phone.length !=11) {
				    uni.showToast({
				        icon: 'none',
				        title: '手机号不正确'
				    });
				    return;
				}
		        if (this.userinfo.passWord.length < 6) {
		            uni.showToast({
		                icon: 'none',
		                title: '密码不正确'
		            });
		            return;
		        }
				// if (this.code.length != 4) {
				//     uni.showToast({
				//         icon: 'none',
				//         title: '验证码不正确'
				//     });
				//     return;
				// }
				console.log(this.userinfo);
				doCreateUser(this.userinfo).then(res=>{
					console.log(res.data)
					if(res.data.code==200)
					{
						uni.showToast({title:res.data.message});
						setTimeout(function(){
							uni.navigateBack();
						},2000) 
						
					}else{
						uni.showToast({title:res.data.message,icon:'none'});
					}
					console.log();
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

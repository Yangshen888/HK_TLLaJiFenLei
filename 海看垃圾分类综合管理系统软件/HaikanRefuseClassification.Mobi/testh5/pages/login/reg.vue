<template>
	<view class="" style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/login/login">
			<block slot="backText">返回</block>
			<block slot="content">注册</block>
		</cu-custom>
		<view class="content reg_bg">
			<view class="list">
				<view class="list-call">
					<view style="color:red;">*</view>
					<text class="cuIcon-myfill"></text>
					<input class="biaoti" v-model="userinfo.loginName"  maxlength="11" placeholder="请输入用户名" />
					
				</view>
				<view class="list-call">
					<view style="color:red;">*</view>
					<text class="cuIcon-unlock"></text>
					<input class="biaoti" v-model="userinfo.passWord" type="text" maxlength="32" placeholder="请输入登录密码" @blur="prop_pwd(userinfo.passWord)" :password="!showPassword" />
					<image class="img" :src="showPassword?'/static/shilu-login/op.png':'/static/shilu-login/cl.png'" @tap="display"></image>
					
				</view>
				<view class="list-call">
					<view style="color:red;">*</view>
					<text class="cuIcon-lock"></text>
					<input class="biaoti" v-model="userinfo.passWord2" type="text" maxlength="32" placeholder="确认登录密码" @blur="prop_pwd2(userinfo.passWord2)" :password="!showPassword2" />
					<image class="img" :src="showPassword2?'/static/shilu-login/op.png':'/static/shilu-login/cl.png'" @tap="display2"></image>
				</view>
				<view class="list-call">
					<view style="color:red;">*</view>
					<text class="cuIcon-home"></text>
					
					<pick-regions :default-regions="defaultRegions" @getRegions="handleGetRegions" style="position: absolute;left:65px;">
					    <input  v-model="userinfo.address" placeholder="请选择省市区" disabled />
					</pick-regions>
					
				</view>
				<view class="list-call">
					<view style="color:red;">*</view>
					<text class="cuIcon-calendar"></text>
					<input class="biaoti" v-model="detailAdress"   placeholder="请输入详细地址(到单元号)" />
				</view>
				<view class="list-call">
					<text class="cuIcon-vipcard"></text>
					<input class="biaoti" v-model="userinfo.oldCard" type="number"  placeholder="请输入老年卡号" @blur="prop_oldcard(userinfo.oldCard)" />
				</view>
				<view class="list-call">
					<view style="color:red;">*</view>
					<text class="cuIcon-mobilefill"></text>
					<input class="biaoti" v-model="userinfo.phone" type="number" maxlength="11" placeholder="请输入手机号" @blur="prop_phone(userinfo.phone)" />
				</view>
				<view class="list-call">
					<view style="color:red;">*</view>
					<text class="cuIcon-safe"></text>
					<input class="biaoti" v-model="userinfo.code"  maxlength="6" placeholder="验证码" />
					<!-- <view class="yzm"  @tap="getcode">{{yanzhengma}}</view> -->
					<view class="yzm" ref='yzm' :class="{ yzms: second>0 }" @tap="getcode" >{{yanzhengma}}</view>
				</view>
				
			</view>
			
			<view class="dlbutton" hover-class="dlbutton-hover" @tap="bindLogin">
				<text>注册</text>
			</view>
			
			<!-- <view class="xieyi">
				<image @tap="xieyitong" :src="xieyi==true?'/static/shilu-login/ty1.png':'/static/shilu-login/ty0.png'"></image>
				<text @tap="xieyitong"> 同意</text>
				<navigator url="blog?id=1" open-type="navigate">《软件用户协议》</navigator>
			</view> -->
		</view>
		
	</view>
</template>

<script>
	import pickRegions from '@/components/pick-regions/pick-regions.vue'
	import {doCreateUser,getRegCode} from '@/api/supervisor/manager.js'
	
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
				havecode:false
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
			prop_pwd(value){
				if(value!==''&&value!=null)
				{
				  var pwdRegex = new RegExp('(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9]).{8,30}');
				  if (!pwdRegex.test(value))
				  {
					uni.showToast({
					    icon: 'none',
					    title: '您的密码复杂度太低（密码中必须包含字母、数字、特殊字符），请重新输入密码！',
						duration:2000
					});
					this.regpwd=false
					return;
				  }
				  this.regpwd=true;
				}else{
					this.regpwd=false
				}
				
			},
			prop_oldcard(value){
				if(value=='' || value==null){
					uni.showToast({
					    icon: 'none',
					    title: '请填写老年卡号',
						duration:2000
					});
					return;
				}
			},
			prop_pwd2(value){
				if(value!==''&&value!=null)
				{
				  var pwdRegex = new RegExp('(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9]).{8,30}');
				  if (!pwdRegex.test(value))
				  {
					uni.showToast({
					    icon: 'none',
					    title: '您的密码复杂度太低（密码中必须包含字母、数字、特殊字符），请重新输入密码！',
						duration:2000
					});
					this.regpwd=false
					return;
				  }
				  if(value!=this.userinfo.passWord){
					  uni.showToast({
					      icon: 'none',
					      title: '两次密码不一致',
						  duration:2000
					  });
					  this.regpwd=false
					  return;
				  }
				  this.regpwd=true;
				}else{
					this.regpwd=false
				}
			},
			prop_phone(value){
				if (value !== '' && value !== null) {
				  var reg = /^1[3456789]\d{9}$/;
				  if (!reg.test(value)) {
				    uni.showToast({
				        icon: 'none',
				        title: '手机号码不合法',
						duration:2000
				    });
				    this.regphone=false
				    return;
				  }else{
					  this.regphone=true
				  }
				} else {
					uni.showToast({
					    icon: 'none',
					    title: '手机号码不能为空',
						duration:2000
					});
					this.regphone=false
				}
				
			},
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
			// xieyitong(){
			// 	this.xieyi = !this.xieyi;
			// },
			getcode(){
				var that=this;
				if(!this.regphone){
					uni.showToast({
					    icon: 'none',
					    title: '请输入正确的手机号'
					});
					return;
				}

				if(this.havecode)
				{
					uni.showToast({title:"请等待！"});
					return;
				}
				this.havecode=true;
				// this.havecode=false;
				//到计时
				
				this.second = 60;
				js = setInterval(function(){
					tha.second--;
					if(tha.second<=0){
						that.havecode=false;
						clearInterval(js)
					}
				},1000)
				
				getRegCode(this.userinfo.phone).then(res=>{
					if(res.data.code==200)
					{
						this.code=res.data.data.str;
						uni.showToast({title:res.data.message});
						
					}else{
						uni.showToast({title:res.data.message,icon:'none'});
					}
				}).catch(error=>{
                console.log(error)
              });
			
			},
		    bindLogin() {
				
				if (this.userinfo.loginName=='' || this.userinfo.loginName==null) {
				    uni.showToast({
				        icon: 'none',
				        title: '请输入账号'
				    });
				    return;
				}
				if (!this.regpwd) {
				    uni.showToast({
				        icon: 'none',
				        title: '请检查密码'
				    });
				    return;
				}
				if (!this.regphone) {
				    uni.showToast({
				        icon: 'none',
				        title: '请检查手机号'
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
				if (this.code=='' ||  this.code.toLowerCase() != this.userinfo.code.toLowerCase()) {
				    uni.showToast({
				        icon: 'none',
				        title: '验证码不正确'
				    });
				    return;
				}
				
				this.userinfo.address+="-"+this.detailAdress;
				console.log(this.userinfo);
				let params={RealName:this.userinfo.loginName,PassWord:this.userinfo.passWord,
				Address:this.userinfo.address,OldCard:this.userinfo.oldCard,Phone:this.userinfo.phone};
				doCreateUser(params).then(res=>{
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

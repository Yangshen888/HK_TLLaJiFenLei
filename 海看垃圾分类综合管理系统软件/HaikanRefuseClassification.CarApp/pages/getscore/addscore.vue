<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/getscore/menu">
			<block slot="backText">返回</block>
			<block slot="content">赋分</block>
		</cu-custom>
		
		<view class="cu-form-group margin-top" style="margin: 20px 20px 0px 20px;">
			<view class="title">申请人</view>
			<input placeholder="请输入手机号或老年卡号" name="input" focus="true" v-model="query.name"></input>
			<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitsearch">搜索</button>
		</view>
		
		<view class="hezi">
			<view class="hang">
				<view class="zuo">
					用户昵称
				</view>
				<view class="you">
					{{userinfo.name}}
				</view>
			</view>
			<view class="hang">
				<view class="zuo">
					手机号码
				</view>
				<view class="you">
					{{userinfo.phone}}
				</view>
			</view>
			<view class="hang">
				<view class="zuo">
					老年卡号
				</view>
				<view class="you">
					{{userinfo.cardnum}}
				</view>
			</view>
			<view class="hang">
				<view class="zuo">
					家庭住址
				</view>
				<view class="you">
					{{userinfo.address}}
				</view>
			</view>
		</view>
		
		
		<view class="hezi">
			<view class="cu-form-group margin-top">
				<view class="title">积分选择：</view>
				<picker @change="PickerChange" :value="index" :range="picker">
					<view class="picker">
						{{index>-1?picker[index]:picker[0]}}
					</view>
				</picker>
			</view>
			
			<view class="cu-form-group align-start">
				<view class="title">备注:</view>
				<textarea maxlength="-1" :disabled="modalName!=null" @input="textareaBInput" placeholder="备注信息"></textarea>
			</view>
			<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitfrom">
				确认
			</button>
		</view>
		
	</view>
</template>

<script>
	import {dogetOneUser,doVolunteeraddScore} from '@/api/supervisor/manager.js'
		
	export default {
		data() {
			return {
				index: -1,
				picker: [0,1,2,3,4,5],
				modalName: null,
				userinfo:{
					name:'',
					phone:'',
					cardnum:'',
					address:'',
					grabageRoom:'',
					volunteerRecodeUUid:'',
				},
				query: {
					name: '',
				},
				score:''
			}
		},
		methods: {
			PickerChange(e) {
				this.score=e.detail.value;
				this.index = e.detail.value
			},
			textareaBInput(e) {
				this.textareaBValue = e.detail.value
			},
			submitsearch(){
				dogetOneUser(this.query.name).then(res=>{
					if(res.data.code==200){
						this.userinfo.name=res.data.data.realName;
						this.userinfo.phone=res.data.data.phone;
						this.userinfo.cardnum=res.data.data.oldCard;
						this.userinfo.address=res.data.data.address;
						this.userinfo.grabageRoom=res.data.data.grabageRoomNum;
						this.userinfo.volunteerRecodeUUid=res.data.data.volunteerRecodeUUid;
					}
					else{
						uni.showModal({
							title: '提示',
							content: res.data.message,
							showCancel: false
						})
					}
				});
			},
			submitfrom(){
					if(this.userinfo.volunteerRecodeUUid!='')
					{
						let score={
							volunteerServiceUUID:this.userinfo.volunteerRecodeUUid,
							score:this.score,
							address:this.userinfo.address
						};
						doVolunteeraddScore(score).then(res=>{
								if(res.data.code==200)
								{
									uni.showModal({
										title: '提示',
										content: res.data.message,
										success: (r) => {
											if (r.confirm) {
												uni.redirectTo({
													url: '/pages/material/list'
												})
											}
										}
									})
								}else{
									uni.showModal({
										title: '提示',
										content: res.data.message,
										showCancel: false
									})
								}
						})
						
					}else{
						uni.showModal({
							title: '提示',
							content: '赋分人员不能为空',
							showCancel: false
						})
					}
			}
		}
	}
</script>

<style>
.hezi{
	background-color: #fff;
	margin: 20px 20px 0 20px;
	border-radius:5px ;
}
.hang{
	line-height: 45px;
	border-top: 1px solid #ccc ;
	overflow: hidden;
	margin: 0 15px;
}
.hang:nth-child(1){
	line-height: 45px;
	border-top: 0 ;
}
.zuo{
	float: left;
	font-weight: bold;
}
.you{
	float: right;
	color: #999;
	font-weight: bold;
}
.cu-form-group{
	border-radius: 5px;
}
</style>

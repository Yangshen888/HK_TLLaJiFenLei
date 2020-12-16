<template>
	<view style="padding-top: 1.4rem;">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
						<block slot="backText">返回</block>
						<block slot="content">考勤记录详情</block>
		</cu-custom>
		<form>
			<view class="cu-form-group margin-top">
				<view class="title">日期</view>
				<input placeholder=""  v-model="formModel.fields.colckDate" :disabled="true"></input>
			</view>
			<view class="cu-form-group margin-top">
				<view class="title">姓名</view>
				<input placeholder=""  v-model="formModel.fields.userName" :disabled="true"></input>
			</view>
			<view class="cu-form-group ">
				<view class="title">科室</view>
				<input placeholder=""  v-model="formModel.fields.departmentName" :disabled="true"></input>
			</view>
			<view class="cu-form-group margin-top">
				<view class="title">上班打卡时间</view>
				<input placeholder=""  v-model="formModel.fields.startTime" :disabled="true"></input>
			</view>
			<view class="cu-form-group ">
				<view class="title">上班打卡地点</view>
				<input placeholder=""  v-model="formModel.fields.startPlace" :disabled="true"></input>
			</view>
			<view class="cu-form-group margin-top">
				<view class="title">下班打卡时间</view>
				<input placeholder=""  v-model="formModel.fields.endTime" :disabled="true"></input>
			</view>
			<view class="cu-form-group ">
				<view class="title">下班打卡地点</view>
				<input placeholder=""  v-model="formModel.fields.endPlace" :disabled="true"></input>
			</view>
			<view class="cu-form-group margin-top">
				<view class="title">上班打卡状态</view>
				<input placeholder=""  v-model="formModel.fields.startState" :disabled="true"></input>
			</view>
			<view class="cu-form-group">
				<view class="title">下班打卡状态</view>
				<input placeholder=""  v-model="formModel.fields.endState" :disabled="true"></input>
			</view>
		</form>
		
	</view>
</template>

<script>
	import {loadAttendanceDetail} from '../../api/attendancemanagement/attendancelist'
	export default{
		data(){
			return{
				formModel:{
					fields:{
						colckDate:"",
                        userName:"",
                        departmentName:"",
                        startTime: "",
                        startPlace:"",
                        startState:"",
                        endTime:"",
                        endPlace:"",
                        endState:"",
					    },
					}
			}
		},
		onLoad(options) {
			this.doloadAttendanceDetail(options.uuid);
		},
		methods:{
			doloadAttendanceDetail(uuid)
			{
				loadAttendanceDetail({guid:uuid}).then(res=>{
					if(res.data.code==200)
					{
						this.formModel.fields = res.data.data;
						this.formModel.fields.startState=this.renderstartstate(this.formModel.fields.startState);
                        this.formModel.fields.endState=this.renderendstate(this.formModel.fields.endState);
					}
					else
					{
						uni.showModal({
							title:"提示",
							content:res.data.message,
							showCancel:false,
							success: (r) => {
								if(r.confirm)
								{
									uni.redirectTo({
										url:'/pages/attendancemanagement/attendancelist'
									})
								}
							}
						})
					}
				})
			},
			 renderstartstate(state){
                var statetext = "未知";
                switch (state) {
                    case 0:
                        statetext="未打卡";
                        break;
                    case 1:
                        statetext="正常";
                        break;
                    case 2:
                        statetext="迟到";
                        break;
                }
                return statetext;
            },
            renderendstate(state){
                var statetext = "未知";
                switch (state) {
                    case 0:
                        statetext="未打卡";
                        break;
                    case 1:
                        statetext="正常";
                        break;
                    case 2:
                        statetext="早退";
                        break;
                }
                return statetext;
            }
		}
	}
</script>

<style>
</style>

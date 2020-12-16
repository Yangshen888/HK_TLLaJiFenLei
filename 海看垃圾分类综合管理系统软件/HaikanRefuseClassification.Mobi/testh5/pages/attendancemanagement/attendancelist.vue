<template>
	<view style="padding-top: 1.4rem;">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/attendancemanagement/home">
						<block slot="backText">返回</block>
						<block slot="content">考勤记录列表</block>
		</cu-custom>
		<form>
			<view class="cu-form-group margin-top">
				<view class="title">科室</view>
				<picker @change="DepartmentlistPickerChange" :value="departmentlist.index" :range="departmentlist.picker" range-key="departmentName">
					<view class="picker">
						{{departmentlist.picker[departmentlist.index].departmentName}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title">姓名</view>
				<input placeholder="请输入姓名查询" name="input" focus="true" v-model="query.username"></input>
			</view>
			<view class="cu-form-group">
				<view class="title">开始时间</view>
				<picker mode="date" :value="query.startdate"  @change="StartDateChange">
					<view class="picker">
						{{query.startdate}}
					</view>
				</picker>
			</view>
			<view class="cu-form-group">
				<view class="title">结束时间</view>
				<picker mode="date" :value="query.enddate"  @change="EndDateChange">
					<view class="picker">
						{{query.enddate}}
					</view>
				</picker>
			</view>
		</form>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitsearch(1)">搜索</button>
		 <view class="bj-1" v-for="(item,index) in data" :key="index">
		        <view class="ul">
					<view class="li"><view class="li-1">姓名:</view><view class="li-2">{{item.userName}}</view></view>
					<view class="li"><view class="li-1">科室:</view><view class="li-2">{{item.departmentName}}</view></view>
					<view class="li"><view class="li-1">日期:</view><view class="li-2">{{item.colckDate}}</view></view>
					<view class="li"><view class="li-1">上班时间:</view><view class="li-2">{{item.startTime}}</view></view>
					<view class="li"><view class="li-1">下班时间:</view><view class="li-2">{{item.endTime}}</view></view>
				</view>
				<view class="ul">	
					<view class="li-3"><button class="li-4" @click="detail(item.attendanceUuid)" >详情</button></view>
				</view>
		  </view>
			<text class="loading-text">
		{{loadingType === 0 ? contentText.contentdown : (loadingType === 1 ? contentText.contentrefresh : contentText.contentnomore)}}</text>
	</view>
</template>

<script>
	import {formatDate} from '../../global/utils.js'
	import {getAttendancelistList,
	loaddepartmentListDetail} from '../../api/attendancemanagement/attendancelist'
	export default{ 
		data(){
			let today = formatDate(new Date(),'yyyy-MM-dd');
			//let tomorrow = this.addDate(today,1);
			let nextMonth = this.addDate(today,0);
			return{
				query:{
					totalCount: 0,
                    pageSize: 10,
                    currentPage: 1,
					username:'',
					startdate:today,
				    enddate:nextMonth,
					departmentUuid:'',
					sort: [
                                {
                                    direct: "DESC",
                                    field: "ID"
                                }
                            ]
				},
				data:[],
				departmentlist:{
					picker: [{departmentUuid:'',departmentName:'全部'}],
				    index:0,
				},

				loadingType: 0,
				contentText: {
					contentdown: "上拉显示更多",
					contentrefresh: "正在加载...",
					contentnomore: "没有更多数据了"
				}

			}
		},
		onLoad() {
			this.$checkusertoken();
			this.doloaddepartmentListDetail();
			this.submitsearch(0);
		},
		//上拉加载
		onReachBottom:function(){
			if(this.loadingType!=0)
			{
				return false;
			}
			this.loadingType=1;
			uni.showNavigationBarLoading();
			this.query.currentPage++;
			getAttendancelistList(this.query).then(res=>{
				if(res.data.code==200)
				{
					if(res.data.totalCount==this.data.length)
					{
						this.loadingType=2;
						uni.hideNavigationBarLoading();
						return false;
					}
					this.data = this.data.concat(res.data.data);
				}
				else
				{
					uni.showModal({
						title:'提示',
						content:res.data.message,
						showCancel:false
					})
				}
				this.loadingType=0;
				uni.hideNavigationBarLoading();
			})
		},
		methods:{
			auditshow(auditState){
				if(auditState==1&&(this.$store.state.roleName=='办公室主任'||this.$store.state.roleName=='信息中心负责人'))
				return true;
				else
				return false;
			},
			addDate(date,days){ 
				var d=new Date(date); 
				d.setDate(d.getDate()+days); 
				var month=d.getMonth()+2; 
				var day = d.getDate(); 
				if(month<10){ 
				month = "0"+month; 
				} 
				if(day<10){ 
				day = "0"+day; 
				} 
				var val = d.getFullYear()+"-"+month+"-"+day; 
				return val; 
			},
			StartDateChange(e) {
				if(new Date(e.detail.value)<=new Date(this.query.enddate))
				{
					this.query.startdate = e.detail.value;
				}
				else
				{
					uni.showModal({
						title:'提示',
						content:'开始时间应小于等于结束时间',
						showCancel:false
					})
				}
			},
			EndDateChange(e) {
				if(new Date(e.detail.value)>=new Date(this.query.startdate))
				{
					this.query.enddate = e.detail.value
				}
				else
				{
					uni.showModal({
						title:'提示',
						content:'结束时间应大于等于开始时间',
						showCancel:false
					})
				}
				
			},
			DepartmentlistPickerChange(e) {
				this.departmentlist.index = e.detail.value
				this.query.departmentUuid = this.departmentlist.picker[this.departmentlist.index].departmentUuid;
			},
			doloaddepartmentListDetail(){
				loaddepartmentListDetail().then(res=>{
					console.log(res.data)
					if(res.data.code==200)
					{
						this.departmentlist.picker = this.departmentlist.picker.concat(res.data.data);
					}
					else
					{
						uni.showModal({
							title:'提示',
							content:res.data.message,
							showCancel:false
						})
					}
				})
			},
			submitsearch(id){
				if(id==0)
				this.query.startdate='';
				this.query.currentPage = 1;
				this.loadingType = 0,
				getAttendancelistList(this.query).then(res=>{
					console.log(res)
					if(res.data.code==200)
					{
						this.data = res.data.data;
						this.query.totalCount = res.data.totalCount;
						if(this.query.startdate=='')
							this.query.startdate=formatDate(new Date(), 'yyyy-MM-dd');
					}
					else
					{
						uni.showModal({
							title:'提示',
							content:res.data.message,
							showCancel:false
						})
					}
				})
			},
			detail(attendanceUuid){
				uni.navigateTo({
					url:'/pages/attendancemanagement/listdetail?uuid='+attendanceUuid
				})
			},
		}
	}
</script>
<style>
		.loading-text{
			height: 80upx;
			line-height: 80upx;
			font-size: 30upx;
			display: flex;
			flex-direction: row;
			justify-content: space-around;
		}
	
	.bj-1{
		background-color: #fff;
		margin: 10px;
		width: 95%;
		overflow: hidden;
	}
	
	.bj-1 .ul{
		float: left;
		width: 100%;
	}
	
	.bj-1 .ul .li{
		float: left;
		width: 50%;
		padding: 15px 0 0 20px;
	}
	
	.bj-1 .ul .li .li-1{
		float: left;
		width: 50%;
	}
	
	.bj-1 .ul .li .li-2{
		float: left;
		width: 50%;
	}
	
	.bj-1 .ul .li-3{
		float: left;
		width: 25%;
		margin: 20px 0;
	}
	
	.bj-1 .ul .li-4{
		line-height: 1.5rem;
		font-size:0.5rem;
		width: 3.5rem;
	}
</style>

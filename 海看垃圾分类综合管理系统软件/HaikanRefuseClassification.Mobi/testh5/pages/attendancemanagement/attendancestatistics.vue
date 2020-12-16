<template>
	<view style="padding-top: 1.4rem;">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/attendancemanagement/home">
						<block slot="backText">返回</block>
						<block slot="content">考勤统计列表</block>
		</cu-custom>
		<scroll-view scroll-x class="bg-white nav margin-top">
			<view class="flex text-center">
				<view class="cu-item flex-sub" :class="index==TabCur?'text-orange cur':''" v-for="(item,index) in 4" :key="index" @tap="tabSelect" :data-id="index">
					{{TabCurContext[index].context}}
				</view>
			</view>
		</scroll-view>
		<view>
			<view v-if="TabCur==0">
				<form >
					<view class="cu-form-group margin-top">
						<view class="title">科室</view>
						<picker @change="YDepartmentlistPickerChange" :value="formModelyindao.departmentlist.index" 
						:range="formModelyindao.departmentlist.picker" range-key="departmentName">
							<view class="picker">
								{{formModelyindao.departmentlist.picker[formModelyindao.departmentlist.index].departmentName}}
							</view>
						</picker>
					</view>
					<view class="cu-form-group">
						<view class="title">姓名</view>
						<input placeholder="请输入姓名查询" name="input" focus="true" v-model="formModelyindao.query.kw"></input>
					</view>
				</form>
				<button class="cu-btn block bg-blue margin-tb-sm lg" @click="ysubmitsearch">搜索</button>
				<view class="cu-capsule round">
					<view class='cu-tag bg-blue '>
						人数
					</view>
					<view class="cu-tag line-blue">
						{{formModelyindao.query.totalCount}}
					</view>
				</view>
				 <view class="bj-1" v-for="(item,index) in formModelyindao.data" :key="index">
				        <view class="ul">
							<view class="li"><view class="li-1">姓名:</view><view class="li-2">{{item.userName}}</view></view>
							<view class="li"><view class="li-1">科室:</view><view class="li-2">{{item.departmentName}}</view></view>
						</view>
				  </view>
					<text class="loading-text">
				{{formModelyindao.loadingType === 0 ? formModelyindao.contentText.contentdown : (formModelyindao.loadingType === 1 ? 
				formModelyindao.contentText.contentrefresh : formModelyindao.contentText.contentnomore)}}</text>
			</view>
			<view v-if="TabCur==1">
				<form >
					<view class="cu-form-group margin-top">
						<view class="title">科室</view>
						<picker @change="SDepartmentlistPickerChange" :value="formModelshidao.departmentlist.index" 
						:range="formModelshidao.departmentlist.picker" range-key="departmentName">
							<view class="picker">
								{{formModelshidao.departmentlist.picker[formModelshidao.departmentlist.index].departmentName}}
							</view>
						</picker>
					</view>
					<view class="cu-form-group">
						<view class="title">姓名</view>
						<input placeholder="请输入姓名查询" name="input" focus="true" v-model="formModelshidao.query.kw"></input>
					</view>
					<view class="cu-form-group">
						<view class="title">开始时间</view>
						<picker mode="date" :value="formModelshidao.query.startdate"  @change="SStartDateChange">
							<view class="picker">
								{{formModelshidao.query.startdate}}
							</view>
						</picker>
					</view>
					<view class="cu-form-group">
						<view class="title">结束时间</view>
						<picker mode="date" :value="formModelshidao.query.enddate"  @change="SEndDateChange">
							<view class="picker">
								{{formModelshidao.query.enddate}}
							</view>
						</picker>
					</view>
				</form>
				<button class="cu-btn block bg-blue margin-tb-sm lg" @click="ssubmitsearch(1)">搜索</button>
				<view class="cu-capsule round">
					<view class='cu-tag bg-blue '>
						人数
					</view>
					<view class="cu-tag line-blue">
						{{formModelshidao.query.totalCount}}
					</view>
				</view>
				 <view class="bj-1" v-for="(item,index) in formModelshidao.data" :key="index">
				        <view class="ul">
							<view class="li"><view class="li-1">姓名:</view><view class="li-2">{{item.userName}}</view></view>
							<view class="li"><view class="li-1">科室:</view><view class="li-2">{{item.departmentName}}</view></view>
						</view>
						<view class="ul">
							<view class="li"><view class="li-1">日期:</view><view class="li-2">{{item.colckDate}}</view></view>
						</view>
				  </view>
					<text class="loading-text">
				{{formModelshidao.loadingType === 0 ? formModelshidao.contentText.contentdown : (formModelshidao.loadingType === 1 ? 
				formModelshidao.contentText.contentrefresh : formModelshidao.contentText.contentnomore)}}</text>
			</view>
			<!-- <view v-if="TabCur==2">
				<form >
					<view class="cu-form-group margin-top">
						<view class="title">科室</view>
						<picker @change="WDepartmentlistPickerChange" :value="formModelweidao.departmentlist.index" 
						:range="formModelweidao.departmentlist.picker" range-key="departmentName">
							<view class="picker">
								{{formModelweidao.departmentlist.picker[formModelweidao.departmentlist.index].departmentName}}
							</view>
						</picker>
					</view>
					<view class="cu-form-group">
						<view class="title">姓名</view>
						<input placeholder="请输入姓名查询" name="input" v-model="formModelweidao.query.kw"></input>
					</view>
					<view class="cu-form-group">
						<view class="title">时间</view>
						<picker mode="date" :value="formModelweidao.query.timeday"  @change="WStartDateChange">
							<view class="picker">
								{{formModelweidao.query.timeday}}
							</view>
						</picker>
					</view>
				</form>
				<button class="cu-btn block bg-blue margin-tb-sm lg" @click="wsubmitsearch(1)">搜索</button>
				<view class="cu-capsule round">
					<view class='cu-tag bg-blue '>
						人数
					</view>
					<view class="cu-tag line-blue">
						{{formModelweidao.query.totalCount}}
					</view>
				</view>
				 <view class="bj-1" v-for="(item,index) in formModelweidao.data" :key="index">
				        <view class="ul">
							<view class="li"><view class="li-1">姓名:</view><view class="li-2">{{item.userName}}</view></view>
							<view class="li"><view class="li-1">科室:</view><view class="li-2">{{item.departmentName}}</view></view>
						</view>
				  </view>
					<text class="loading-text">
				{{formModelweidao.loadingType === 0 ? formModelweidao.contentText.contentdown : (formModelweidao.loadingType === 1 ? 
				formModelweidao.contentText.contentrefresh : formModelweidao.contentText.contentnomore)}}</text>
			</view> -->
			<view v-if="TabCur==2">
				<form >
					<view class="cu-form-group margin-top">
						<view class="title">科室</view>
						<picker @change="CDepartmentlistPickerChange" :value="formModelchidao.departmentlist.index" 
						:range="formModelchidao.departmentlist.picker" range-key="departmentName">
							<view class="picker">
								{{formModelchidao.departmentlist.picker[formModelchidao.departmentlist.index].departmentName}}
							</view>
						</picker>
					</view>
					<view class="cu-form-group">
						<view class="title">姓名</view>
						<input placeholder="请输入姓名查询" name="input" focus="true" v-model="formModelchidao.query.kw"></input>
					</view>
					<view class="cu-form-group">
						<view class="title">开始时间</view>
						<picker mode="date" :value="formModelchidao.query.startdate"  @change="CStartDateChange">
							<view class="picker">
								{{formModelchidao.query.startdate}}
							</view>
						</picker>
					</view>
					<view class="cu-form-group">
						<view class="title">结束时间</view>
						<picker mode="date" :value="formModelchidao.query.enddate"  @change="CEndDateChange">
							<view class="picker">
								{{formModelchidao.query.enddate}}
							</view>
						</picker>
					</view>
				</form>
				<button class="cu-btn block bg-blue margin-tb-sm lg" @click="csubmitsearch(1)">搜索</button>
				<view class="cu-capsule round">
					<view class='cu-tag bg-blue '>
						人数
					</view>
					<view class="cu-tag line-blue">
						{{formModelchidao.query.totalCount}}
					</view>
				</view>
				 <view class="bj-1" v-for="(item,index) in formModelchidao.data" :key="index">
				        <view class="ul">
							<view class="li"><view class="li-1">姓名:</view><view class="li-2">{{item.userName}}</view></view>
							<view class="li"><view class="li-1">科室:</view><view class="li-2">{{item.departmentName}}</view></view>
						</view>
						<view class="ul">
							<view class="li"><view class="li-1">日期:</view><view class="li-2">{{item.colckDate}}</view></view>
						</view>
				  </view>
					<text class="loading-text">
				{{formModelchidao.loadingType === 0 ? formModelchidao.contentText.contentdown : (formModelchidao.loadingType === 1 ? 
				formModelchidao.contentText.contentrefresh : formModelchidao.contentText.contentnomore)}}</text>
			</view>
			<view v-if="TabCur==3">
				<form >
					<view class="cu-form-group margin-top">
						<view class="title">科室</view>
						<picker @change="ZDepartmentlistPickerChange" :value="formModelzaotui.departmentlist.index" 
						:range="formModelzaotui.departmentlist.picker" range-key="departmentName">
							<view class="picker">
								{{formModelzaotui.departmentlist.picker[formModelzaotui.departmentlist.index].departmentName}}
							</view>
						</picker>
					</view>
					<view class="cu-form-group">
						<view class="title">姓名</view>
						<input placeholder="请输入姓名查询" name="input" focus="true" v-model="formModelzaotui.query.kw"></input>
					</view>
					<view class="cu-form-group">
						<view class="title">开始时间</view>
						<picker mode="date" :value="formModelzaotui.query.startdate"  @change="ZStartDateChange">
							<view class="picker">
								{{formModelzaotui.query.startdate}}
							</view>
						</picker>
					</view>
					<view class="cu-form-group">
						<view class="title">结束时间</view>
						<picker mode="date" :value="formModelzaotui.query.enddate"  @change="ZEndDateChange">
							<view class="picker">
								{{formModelzaotui.query.enddate}}
							</view>
						</picker>
					</view>
				</form>
				<button class="cu-btn block bg-blue margin-tb-sm lg" @click="zsubmitsearch(1)">搜索</button>
				<view class="cu-capsule round">
					<view class='cu-tag bg-blue '>
						人数
					</view>
					<view class="cu-tag line-blue">
						{{formModelzaotui.query.totalCount}}
					</view>
				</view>
				 <view class="bj-1" v-for="(item,index) in formModelzaotui.data" :key="index">
				        <view class="ul">
							<view class="li"><view class="li-1">姓名:</view><view class="li-2">{{item.userName}}</view></view>
							<view class="li"><view class="li-1">科室:</view><view class="li-2">{{item.departmentName}}</view></view>
						</view>
						<view class="ul">
							<view class="li"><view class="li-1">日期:</view><view class="li-2">{{item.colckDate}}</view></view>
						</view>
				  </view>
					<text class="loading-text">
				{{formModelzaotui.loadingType === 0 ? formModelzaotui.contentText.contentdown : (formModelzaotui.loadingType === 1 ? 
				formModelzaotui.contentText.contentrefresh : formModelzaotui.contentText.contentnomore)}}</text>
			</view>
		</view>
	</view>
</template>

<script>
	import {formatDate} from '../../global/utils.js'
	import {loaddepartmentListDetail} from '../../api/attendancemanagement/attendancelist'
	import {getAttendancelistYingdao,
	getAttendancelistShidao,
	getAttendancelistWeidao,
	getAttendancelistChidao,
	getAttendancelistZaotui} from '../../api/attendancemanagement/attendancestatistics.js'
	export default {
		data() {
			let today = formatDate(new Date(),'yyyy-MM-dd');
			return {
				TabCur: 0,
				scrollLeft: 0,
				TabCurContext:[
					{index:0,context:'应到'},
					{index:1,context:'实到'},
					// {index:2,context:'未到'},
					{index:2,context:'迟到'},
					{index:3,context:'早退'}
				],
				formModelyindao:{
					query:{
						totalCount: 0,
					    pageSize: 10,
					    currentPage: 1,
						kw:'',
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
				},
				formModelshidao:{
					query:{
						totalCount: 0,
					    pageSize: 10,
					    currentPage: 1,
						kw:'',
						startdate:today,
						enddate:today,
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
				},
				formModelweidao:{
					query:{
						totalCount: 0,
					    pageSize: 10,
					    currentPage: 1,
						kw:'',
						timeday:today,
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
				},
				formModelchidao:{
					query:{
						totalCount: 0,
					    pageSize: 10,
					    currentPage: 1,
						kw:'',
						startdate:today,
						enddate:today,
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
				},
				formModelzaotui:{
					query:{
						totalCount: 0,
					    pageSize: 10,
					    currentPage: 1,
						kw:'',
						startdate:today,
						enddate:today,
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
				
			};
		},
		onLoad() {
			this.$checkusertoken();
			this.doloaddepartmentListDetail();
			this.ysubmitsearch();
			this.ssubmitsearch(0);
			this.wsubmitsearch(0);
			this.csubmitsearch(0);
			this.zsubmitsearch(0);
		},
		//上拉加载
		onReachBottom:function(){
			if(this.TabCur==0)
			{
				if(this.formModelyindao.loadingType!=0)
				{
					return false;
				}
				this.formModelyindao.loadingType=1;
				uni.showNavigationBarLoading();
				this.formModelyindao.query.currentPage++;
				getAttendancelistYingdao(this.formModelyindao.query).then(res=>{
					if(res.data.code==200)
					{
						if(res.data.totalCount==this.formModelyindao.data.length)
						{
							this.formModelyindao.loadingType=2;
							uni.hideNavigationBarLoading();
							return false;
						}
						this.formModelyindao.data = this.formModelyindao.data.concat(res.data.data);
					}
					else
					{
						uni.showModal({
							title:'提示',
							content:res.data.message,
							showCancel:false
						})
					}
					this.formModelyindao.loadingType=0;
					uni.hideNavigationBarLoading();
				})
			}
			else if(this.TabCur==1)
			{
				if(this.formModelshidao.loadingType!=0)
				{
					return false;
				}
				this.formModelshidao.loadingType=1;
				uni.showNavigationBarLoading();
				this.formModelshidao.query.currentPage++;
				getAttendancelistShidao(this.formModelshidao.query).then(res=>{
					if(res.data.code==200)
					{
						if(res.data.totalCount==this.formModelshidao.data.length)
						{
							this.formModelshidao.loadingType=2;
							uni.hideNavigationBarLoading();
							return false;
						}
						this.formModelshidao.data = this.formModelshidao.data.concat(res.data.data);
					}
					else
					{
						uni.showModal({
							title:'提示',
							content:res.data.message,
							showCancel:false
						})
					}
					this.formModelshidao.loadingType=0;
					uni.hideNavigationBarLoading();
				})
			}
			// else if(this.TabCur==2)
			// {
			// 	if(this.formModelweidao.loadingType!=0)
			// 	{
			// 		return false;
			// 	}
			// 	this.formModelweidao.loadingType=1;
			// 	uni.showNavigationBarLoading();
			// 	this.formModelweidao.query.currentPage++;
			// 	getAttendancelistWeidao(this.formModelweidao.query).then(res=>{
			// 		if(res.data.code==200)
			// 		{
			// 			if(res.data.totalCount==this.formModelweidao.data.length)
			// 			{
			// 				this.formModelweidao.loadingType=2;
			// 				uni.hideNavigationBarLoading();
			// 				return false;
			// 			}
			// 			this.formModelweidao.data = this.formModelweidao.data.concat(res.data.data);
			// 		}
			// 		else
			// 		{
			// 			uni.showModal({
			// 				title:'提示',
			// 				content:res.data.message,
			// 				showCancel:false
			// 			})
			// 		}
			// 		this.formModelweidao.loadingType=0;
			// 		uni.hideNavigationBarLoading();
			// 	})
			// }
			else if(this.TabCur==2)
			{
				if(this.formModelchidao.loadingType!=0)
				{
					return false;
				}
				this.formModelchidao.loadingType=1;
				uni.showNavigationBarLoading();
				this.formModelchidao.query.currentPage++;
				getAttendancelistChidao(this.formModelchidao.query).then(res=>{
					if(res.data.code==200)
					{
						if(res.data.totalCount==this.formModelchidao.data.length)
						{
							this.formModelchidao.loadingType=2;
							uni.hideNavigationBarLoading();
							return false;
						}
						this.formModelchidao.data = this.formModelchidao.data.concat(res.data.data);
					}
					else
					{
						uni.showModal({
							title:'提示',
							content:res.data.message,
							showCancel:false
						})
					}
					this.formModelchidao.loadingType=0;
					uni.hideNavigationBarLoading();
				})
			}
			else if(this.TabCur==3)
			{
				if(this.formModelzaotui.loadingType!=0)
				{
					return false;
				}
				this.formModelzaotui.loadingType=1;
				uni.showNavigationBarLoading();
				this.formModelzaotui.query.currentPage++;
				getAttendancelistZaotui(this.formModelzaotui.query).then(res=>{
					if(res.data.code==200)
					{
						if(res.data.totalCount==this.formModelzaotui.data.length)
						{
							this.formModelzaotui.loadingType=2;
							uni.hideNavigationBarLoading();
							return false;
						}
						this.formModelzaotui.data = this.formModelzaotui.data.concat(res.data.data);
					}
					else
					{
						uni.showModal({
							title:'提示',
							content:res.data.message,
							showCancel:false
						})
					}
					this.formModelzaotui.loadingType=0;
					uni.hideNavigationBarLoading();
				})
			}
			
		},
		methods: {
			tabSelect(e) {
				this.TabCur = e.currentTarget.dataset.id;
				this.scrollLeft = (e.currentTarget.dataset.id - 1) * 60
			},
			doloaddepartmentListDetail(){
				loaddepartmentListDetail().then(res=>{
					if(res.data.code==200)
					{
						this.formModelyindao.departmentlist.picker = this.formModelyindao.departmentlist.picker.concat(res.data.data);
						this.formModelshidao.departmentlist.picker = this.formModelshidao.departmentlist.picker.concat(res.data.data);
						this.formModelweidao.departmentlist.picker = this.formModelweidao.departmentlist.picker.concat(res.data.data);
						this.formModelchidao.departmentlist.picker = this.formModelchidao.departmentlist.picker.concat(res.data.data);
						this.formModelzaotui.departmentlist.picker = this.formModelzaotui.departmentlist.picker.concat(res.data.data);
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
			//应到人数搜索
			YDepartmentlistPickerChange(e) {
				this.formModelyindao.departmentlist.index = e.detail.value
				this.formModelyindao.query.departmentUuid = this.formModelyindao.departmentlist.picker[this.formModelyindao.departmentlist.index].departmentUuid;
			},
			ysubmitsearch(){
				this.formModelyindao.query.currentPage = 1;
				this.formModelyindao.loadingType = 0,
				getAttendancelistYingdao(this.formModelyindao.query).then(res=>{
					if(res.data.code==200)
					{
						this.formModelyindao.data = res.data.data;
						this.formModelyindao.query.totalCount = res.data.totalCount;
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
			//实到人数搜索
			SDepartmentlistPickerChange(e) {
				this.formModelshidao.departmentlist.index = e.detail.value
				this.formModelshidao.query.departmentUuid = this.formModelshidao.departmentlist.picker[this.formModelshidao.departmentlist.index].departmentUuid;
			},
			SStartDateChange(e) {
				if(new Date(e.detail.value)<=new Date(this.formModelshidao.query.enddate))
				{
					this.formModelshidao.query.startdate = e.detail.value;
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
			SEndDateChange(e) {
				if(new Date(e.detail.value)>=new Date(this.formModelshidao.query.startdate))
				{
					this.formModelshidao.query.enddate = e.detail.value
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
			ssubmitsearch(id){
				if(id==0)
				this.formModelshidao.query.startdate='';
				this.formModelshidao.query.currentPage = 1;
				this.formModelshidao.loadingType = 0,
				getAttendancelistShidao(this.formModelshidao.query).then(res=>{
					if(res.data.code==200)
					{
						console.log(res.data)
						this.formModelshidao.data = res.data.data;
						this.formModelshidao.query.totalCount = res.data.totalCount;
						if(this.formModelshidao.query.startdate=='')
							this.formModelshidao.query.startdate=formatDate(new Date(), 'yyyy-MM-dd');
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
			//未到人数搜索
			WDepartmentlistPickerChange(e) {
				this.formModelweidao.departmentlist.index = e.detail.value
				this.formModelweidao.query.departmentUuid = this.formModelweidao.departmentlist.picker[this.formModelweidao.departmentlist.index].departmentUuid;
			},
			WStartDateChange(e) {
				this.formModelweidao.query.timeday = e.detail.value;
			},
			wsubmitsearch(id){
				if(id==0)
				this.formModelweidao.query.timeday='';
				this.formModelweidao.query.currentPage = 1;
				this.formModelweidao.loadingType = 0,
				getAttendancelistWeidao(this.formModelweidao.query).then(res=>{
					console.log(res.data)
					if(res.data.code==200)
					{
						this.formModelweidao.data = res.data.data;
						this.formModelweidao.query.totalCount = res.data.totalCount;
						if(this.formModelweidao.query.timeday=='')
							this.formModelweidao.query.timeday=formatDate(new Date(), 'yyyy-MM-dd');
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
			//迟到人数搜索
			CDepartmentlistPickerChange(e) {
				this.formModelchidao.departmentlist.index = e.detail.value
				this.formModelchidao.query.departmentUuid = this.formModelchidao.departmentlist.picker[this.formModelchidao.departmentlist.index].departmentUuid;
			},
			CStartDateChange(e) {
				if(new Date(e.detail.value)<=new Date(this.formModelchidao.query.enddate))
				{
					this.formModelchidao.query.startdate = e.detail.value;
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
			CEndDateChange(e) {
				if(new Date(e.detail.value)>=new Date(this.formModelchidao.query.startdate))
				{
					this.formModelchidao.query.enddate = e.detail.value
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
			csubmitsearch(id){
				if(id==0)
				this.formModelchidao.query.startdate='';
				this.formModelchidao.query.currentPage = 1;
				this.formModelchidao.loadingType = 0,
				getAttendancelistChidao(this.formModelchidao.query).then(res=>{
					if(res.data.code==200)
					{
						this.formModelchidao.data = res.data.data;
						this.formModelchidao.query.totalCount = res.data.totalCount;
						if(this.formModelchidao.query.startdate=='')
							this.formModelchidao.query.startdate=formatDate(new Date(), 'yyyy-MM-dd');
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
			//早退人数搜索
			ZDepartmentlistPickerChange(e) {
				this.formModelzaotui.departmentlist.index = e.detail.value
				this.formModelzaotui.query.departmentUuid = this.formModelzaotui.departmentlist.picker[this.formModelzaotui.departmentlist.index].departmentUuid;
			},
			ZStartDateChange(e) {
				if(new Date(e.detail.value)<=new Date(this.formModelzaotui.query.enddate))
				{
					this.formModelzaotui.query.startdate = e.detail.value;
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
			ZEndDateChange(e) {
				if(new Date(e.detail.value)>=new Date(this.formModelzaotui.query.startdate))
				{
					this.formModelzaotui.query.enddate = e.detail.value
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
			zsubmitsearch(id){
				if(id==0)
				this.formModelzaotui.query.startdate='';
				this.formModelzaotui.query.currentPage = 1;
				this.formModelzaotui.loadingType = 0,
				getAttendancelistZaotui(this.formModelzaotui.query).then(res=>{
					if(res.data.code==200)
					{
						this.formModelzaotui.data = res.data.data;
						this.formModelzaotui.query.totalCount = res.data.totalCount;
						if(this.formModelzaotui.query.startdate=='')
							this.formModelzaotui.query.startdate=formatDate(new Date(), 'yyyy-MM-dd');
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
			}
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
		padding: 15px 15px 15px 15px;
	}
	
	.bj-1 .ul .li .li-1{
		float: left;
		width: 30%;
	}
	
	.bj-1 .ul .li .li-2{
		float: left;
		width: 70%;
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

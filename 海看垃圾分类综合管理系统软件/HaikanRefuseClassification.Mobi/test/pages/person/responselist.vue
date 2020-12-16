<template>
	<view style="padding-top: 1.4rem;">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">用户反馈列表</block>
		</cu-custom>
		<form>
			<view class="cu-form-group margin-top">
				<view class="title">用户姓名</view>
				<input placeholder="请输入用户姓名查询" name="input" v-model="query.kw"></input>
			</view>
		</form>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitsearch">搜索</button>
		<button class="cu-btn block bg-blue margin-tb-sm lg" @click="create">新增用户反馈</button>
		 <view class="bj-1" v-for="(item,index) in data" :key="index">
                <view class="ul">
					<view class="li"><view class="li-1">用户姓名:</view><view class="li-2">{{item.realName}}</view></view>
					<view class="li"><view class="li-1">反馈内容:</view><view class="li-2">{{item.problemContent}}</view></view>				
				</view>
				<view class="ul">
					<view class="li-3"><button class="li-4" @click="detail(item.systemUserUuid)">详情</button></view>
					<view class="li-3"><button class="li-4" @click="deletemsg(item.systemUserUuid)">删除</button></view>
					<view class="li-3"><button class="li-4" @click="edit(item.systemUserUuid)">修改</button></view>					
				</view>
          </view>
			<!-- <text class="loading-text">
		{{loadingType === 0 ? contentText.contentdown : (loadingType === 1 ? contentText.contentrefresh : contentText.contentnomore)}}</text> -->

	</view>
</template>

<script>
	import {getCarDriverList,
	        deleteCarDriver,
	        loadCarDriverDetail} from '@/api/person/response.js'
	export default {
		data(){
				return{
					query:{
						totalCount: 0,
		                pageSize: 10,
		                currentPage: 1,
						kw:"",
						sort: [
		                            {
		                                direct: "DESC",
		                                field: "ID"
		                            }
		                        ]
					},
					data:[],										
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
				this.submitsearch();
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
				getCarDriverList(this.query).then(res=>{
					
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
				//搜索
				submitsearch(){
					this.query.currentPage = 1;
					this.loadingType = 0,
					console.log(this.query)
					getCarDriverList(this.query).then(res=>{
						console.log(res)
						if(res.data.code==200)
						{
							console.log(res.data.data)
							this.data = res.data.data;
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
				//新增
				create(){
					uni.navigateTo({
						url:'/pages/person/responsecreate'
					})
				},
				//编辑
				edit(systemUserUuid){
					uni.navigateTo({
						url:'/pages/person/responseedit?uuid='+systemUserUuid
					})
				},
				//删除
				deletemsg(systemUserUuid){
					uni.showModal({
						title: '提示',
						content: '是否确认删除',
						success: (rd) => {
							if (rd.confirm) {
								deleteCarDriver(systemUserUuid).then(res => {
									uni.showModal({
										title: '提示',
										content: res.data.message,
										showCancel: false,
										success: (r) => {
											if (r.confirm) {
												this.submitsearch();
											}
										}
									})
								})
							}
						}
					})
					
				},
				//详情
				detail(systemUserUuid){
					uni.navigateTo({
						url:'/pages/person/responsedetail?uuid='+systemUserUuid
					})
				}
		},
			
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




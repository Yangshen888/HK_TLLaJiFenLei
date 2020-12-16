<template>
	<view style="padding-top: 1.4rem;">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/person/responselist">
			<block slot="backText">返回</block>
			<block slot="content">用户反馈详情</block>
		</cu-custom>
		<form>
			<view class="cu-form-group margin-top">
				<view class="title">用户姓名</view>
				<input placeholder="请输入用户姓名查询" name="input" v-model="formModel.fields.realName" :disabled="true"></input>
			</view>
			<view class="cu-form-group margin-top">
				<view class="title">反馈内容</view>
				<input placeholder="请输入反馈内容查询" name="input" v-model="formModel.fields.problemContent" :disabled="true"></input>
			</view>	
		</form>
	</view>
</template>

<script>
	import {loadCarDriverDetail} from '../../api/person/response.js'
	export default{
		data(){
			return{
				formModel:{
					fields:{
						realName:"",
						problemContent:"",
					    },
					}
			}
		},
		onLoad(options) {
			this.doLoadCarDispatch(options.uuid);
		},
		methods:{
			doLoadCarDispatch(uuid){
				loadCarDriverDetail({guid:uuid}).then(res=>{
					if(res.data.code==200)
					{
						this.formModel.fields = res.data.data;
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
										url:'/pages/person/responselist'
									})
								}
							}
						})
					}
				})
			}
		}
	}
</script>

<style>
</style>




<template>
	<view class="content" style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
						<block slot="backText">返回</block>
						<block slot="content">消息</block>
		</cu-custom>
		<view class="cu-timeline" v-if="messages!=null&&messages.length>0" v-for="(item,index) in messages" :key="index">
			<view class="cu-time" style="width: 100px;" v-if="check(item,index)">{{showDate(item.addTime)}}</view>
			<view class="cu-item text-cyan">
				<view class="content bg-cyan shadow-blur">
					<view class="cu-tag bg-mauve" style="margin-right: 15px;">{{showTime(item.addTime)}}</view><text >{{item.message1}}</text> 
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	import{showMessage} from '@/api/grabage/GrabList.js'
	export default {
		data() {
			return {
				messages:[],
				setTime:[],
			}
		},
		methods: {
			getMessages(){
				showMessage({guid:this.$store.state.userId}).then(res=>{
					if(res.data.code==200){
						console.log(res.data.data);
						this.messages=res.data.data;
					}
				});
			},
			showDate(data){
				console.log(data);
				let arr=data.split(" ");
				console.log(arr);
				return arr[0];
			},
			showTime(data){
				console.log(data);
				let arr=data.split(" ");
				console.log(arr);
				return arr[1];
			},
			check(data,index){
				console.log(index);
				if(index>0){
					let newtime=data.addTime.split(" ");
					let oldtime=this.messages[index-1].addTime.split(" ");
					if(newtime[0]==oldtime[0]){
						return false;
					}else{
						return true;
					}
				}else{
					return true;
				}
				
			}
			
		},
		onLoad() {
			this.getMessages();
		}
	}
</script>

<style>

</style>

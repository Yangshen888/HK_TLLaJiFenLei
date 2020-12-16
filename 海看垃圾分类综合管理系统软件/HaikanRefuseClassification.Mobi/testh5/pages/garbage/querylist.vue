<template>
	<view class="" style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/garbage/typelist" >
			<block slot="backText">返回</block>
			<block slot="content">垃圾分类查询结果</block>
		</cu-custom>
		<!-- <view class='content'>
			<view class='cardSub' v-for="(item,index) in arr1" v-if="arr1.length > 0" :key="index">
				{{item.grabName}}
			</view>
		</view> -->
		<view class="cu-bar bg-white solid-bottom margin-top">
			<view class="action">
				<text class="cuIcon-title text-orange"></text> 结果列表
			</view>
			
		</view>
		<view class="cu-list menu" >
		<view class="cu-item" v-for="(item,index) in arr1" v-if="arr1.length > 0" :key="index">
			<view class="content">
				<!-- <text class="cuIcon-tagfill text-red  margin-right-xs"></text> -->
				<text class="text-grey">{{item.grabName}}</text>
			</view>
			<view class="action">
				
				<view class="cu-tag round bg-blue light">可回收垃圾</view>
			</view>
		</view>
		<view class="cu-item" v-for="(item,index) in arr2" v-if="arr2.length > 0" :key="index">
			<view class="content">
				<!-- <text class="cuIcon-tagfill text-red  margin-right-xs"></text> -->
				<text class="text-grey">{{item.grabName}}</text>
			</view>
			<view class="action">
				
				<view class="cu-tag round bg-red light">有害垃圾</view>
			</view>
		</view>
		<view class="cu-item" v-for="(item,index) in arr3" v-if="arr3.length > 0" :key="index">
			<view class="content">
				<!-- <text class="cuIcon-tagfill text-red  margin-right-xs"></text> -->
				<text class="text-grey">{{item.grabName}}</text>
			</view>
			<view class="action">
				
				<view class="cu-tag round bg-green light">易腐垃圾</view>
			</view>
		</view>
		<view class="cu-item" v-for="(item,index) in arr4" v-if="arr4.length > 0" :key="index">
			<view class="content">
				<!-- <text class="cuIcon-tagfill text-red  margin-right-xs"></text> -->
				<text class="text-grey">{{item.grabName}}</text>
			</view>
			<view class="action">
				
				<view class="cu-tag round bg-black light">其他垃圾</view>
			</view>
		</view>
		</view>
	</view>
</template>

<script>
	import refresh from '@/components/nav-tab/refresh.vue';
	import navTab from '@/components/nav-tab/navTab.vue';
	import {getGrabTypeList} from '../../api/grabage/GrabList.js'
	export default {
		components: {refresh,navTab},
		data() {
			return {
				kw:"",
				arr1:[],
				arr2:[],
				arr3:[],
				arr4:[],
			}
		},
		methods: {
			
		},
		onLoad(opt) {
			console.log(opt);
			getGrabTypeList({kw:opt.kw}).then(res=>{
				console.log(res);
				this.list=res.data.data;
				this.arr1=res.data.data[0];
				this.arr2=res.data.data[1];
				this.arr3=res.data.data[2];
				this.arr4=res.data.data[3];
				console.log(this.arr1);
				console.log(this.arr2);
				console.log(this.arr3);
				console.log(this.arr4);
			});
			
		},
	}
</script>

<style>

</style>

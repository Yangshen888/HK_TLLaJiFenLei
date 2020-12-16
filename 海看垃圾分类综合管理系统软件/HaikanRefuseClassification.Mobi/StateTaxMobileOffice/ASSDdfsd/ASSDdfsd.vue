<template>
<view class="uni-list">
    <view class="uni-list-cell">
        <view class="uni-list-cell-navigate uni-navigate-right" v-for="(item,index) in list" :key="index">
            {{item.value}}
        </view>
    </view>
</view>

</template>

<script>
	export default {
		data() {
			return {
				
			};
		}
	}
	
</script>

<style lang="scss">

</style>

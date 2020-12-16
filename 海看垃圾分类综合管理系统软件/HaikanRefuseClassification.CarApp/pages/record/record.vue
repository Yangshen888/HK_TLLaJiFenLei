<template>
	<view class="content">
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">实时地图</block>
		</cu-custom>
		<view class="page-body">
			<baidu-map :center="{ lng: 120.127, lat: 30.227 }" :zoom="12.8" style="height:100%" :scroll-wheel-zoom="true">
			                        <!-- 在右下角添加定位 -->
			                        <bm-geolocation anchor="BMAP_ANCHOR_BOTTOM_RIGHT" :showAddressBar="true" :autoLocation="true"></bm-geolocation>
			                        <!-- 地图类型，两种：一种是路线一种是绿的那种-->
			                        <!-- <bm-map-type :map-types="['BMAP_NORMAL_MAP', 'BMAP_HYBRID_MAP']" anchor="BMAP_ANCHOR_TOP_LEFT"></bm-map-type> -->
			                        <!--地图搜索功能，绑定上面的input，
			                        <!--display: none样式很关键，因为下面默认会有地址提示信息很长，很烦，这样搜索会很舒服，-->
			                        <!--zoom是搜索结果的视图比例，个人觉得12.8很舒服显示-->
			                        <!-- <bm-local-search :keyword="keyword" :auto-viewport="true" :location="location" zoom="12.8"></bm-local-search> -->
			                        <!--信息窗口，show属性是控制显示隐藏，infoWindowClose和infoWindowOpen是控制信息窗口关闭隐藏的方法-->
			                        <!-- <bm-marker :position="postionMap">
			                                <bm-info-window :show="show" @close="infoWindowClose" @open="infoWindowOpen" style="font-size: 14px" :autoPan="true">
			                                        <p>附近商圈：{{ add.siteName }}</p>
			                                        <p>地址：{{ add.site }}</p>
			                                </bm-info-window>
			                        </bm-marker> -->
			                        <!-- <bm-marker :position="{ lng: item.Lat, lat: item.Long }" v-for="(item,index) in positionMap" v-if="positionMap.length>0" :key="index">
			                                <bm-info-window title="Info Window Title" :show="infoWindow.show" @close="infoWindowClose" @open="infoWindowOpen" :autoPan="true">
			                                        <p v-text="infoWindow.contents"></p>
			                                </bm-info-window>
			                        </bm-marker> -->
			                </baidu-map>
		</view>
	</view>
</template>

<script>
	import uniPopup from '@/components/uni-popup/uni-popup.vue';
	import uniIcon from '@/components/uni-icon/uni-icon.vue';
export default {
	components: { uniIcon, uniPopup },
	data() {
		return {};
	},
	methods: {}
};
</script>

<style></style>


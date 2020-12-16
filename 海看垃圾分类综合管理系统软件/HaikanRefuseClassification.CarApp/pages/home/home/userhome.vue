<template>
	<view>
		<swiper class="screen-swiper" :class="dotStyle?'square-dot':'round-dot'" :indicator-dots="true" :circular="true"
		 :autoplay="true" interval="5000" duration="500">
			<swiper-item v-for="(item,index) in swiperList" :key="index">
				<image :src="item.url" mode="aspectFill" v-if="item.type=='image'"></image>
				<video :src="item.url" autoplay loop muted :show-play-btn="false" :controls="false" objectFit="cover" v-if="item.type=='video'"></video>
			</swiper-item>
		</swiper>
		<view class="person-list" style="position: absolute;width:100%;height:100%;background:#fff;">
			<cmd-cell-item title="分类查询" slot-left arrow @click="typelist">
				<text class="cuIcon-search"></text>
			</cmd-cell-item>
			<cmd-cell-item title="附件收运点" slot-left arrow @click="around">
				<text class="cuIcon-locationfill"></text>
			</cmd-cell-item>
			<cmd-cell-item title="积分查询" slot-left arrow @click="myQRcode">
				<text class="cuIcon-moneybagfill"></text>
			</cmd-cell-item>
			<cmd-cell-item title="志愿者报名" slot-left arrow @click="myQRcode">
				<text class="cuIcon-friendaddfill"></text>
			</cmd-cell-item>
			<cmd-cell-item title="商户信息" slot-left arrow @click="myQRcode">
				<text class="cuIcon-shopfill"></text>
			</cmd-cell-item>
			<cmd-cell-item title="个人信息" slot-left arrow @click="myQRcode">
				<text class="cuIcon-myfill"></text>
			</cmd-cell-item>
		</view>

	</view>
</template>

<script>
	import cmdAvatar from "@/components/cmd-avatar/cmd-avatar.vue"
	import cmdIcon from "@/components/cmd-icon/cmd-icon.vue"
	import cmdCellItem from "@/components/cmd-cell-item/cmd-cell-item.vue"

	export default {
		components: {
			cmdAvatar,
			cmdCellItem,
			cmdIcon
		},
		data() {
			return {
				swiperList: [{
						id: 0,
						type: 'image',
						url: '/static/image/垃圾分类系统头部.png'
					},
					{
						id: 1,
						type: 'image',
						url: '/static/image/垃圾分类系统头部.png'
					}
				],
				dotStyle: false,
			};
		},
		mounted() {

			this.TowerSwiper('swiperList');
			// 初始化towerSwiper 传已有的数组名即可
		},
		methods: {
			// 初始化towerSwiper
			TowerSwiper(name) {
				let list = this[name];
				for (let i = 0; i < list.length; i++) {
					list[i].zIndex = parseInt(list.length / 2) + 1 - Math.abs(i - parseInt(list.length / 2))
					list[i].mLeft = i - parseInt(list.length / 2)
				}
				this.swiperList = list
			},
			around() {
				uni.redirectTo({
					url: "/pages/YongHu/around",
				})

			},
			typelist() {
				uni.redirectTo({
					url: "/pages/garbage/typelist",
				})
			}

		}
	}
</script>

<style>
	button {
		margin-top: 40upx;
	}

	.header {}

	.header image {
		width: -webkit-fill-available;
		height: 160px;
	}

	.cuIcon-search:before {
		font-weight: bold;
		color: #1AAD19;
		font-size: 30px;
	}

	.cuIcon-locationfill:before {
		font-weight: bold;
		color: #0081ff;
		font-size: 30px;
	}

	.cuIcon-moneybagfill:before {
		color: #fbbc02;
		font-size: 30px;
		font-weight: bold;
	}

	.cuIcon-friendaddfill:before {
		color: #f57f77;
		font-size: 30px;
		font-weight: bold;
	}

	.cuIcon-shopfill:before {
		color: #955ff3;
		font-size: 30px;
		font-weight: bold;
	}

	.cuIcon-myfill:before {
		color: #7c7c7c;
		font-size: 30px;
		font-weight: bold;
	}
</style>

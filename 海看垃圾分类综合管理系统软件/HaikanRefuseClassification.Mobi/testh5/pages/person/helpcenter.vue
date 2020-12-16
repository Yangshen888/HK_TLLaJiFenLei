<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index">
			<block slot="backText">返回</block>
			<block slot="content">帮助中心</block>
		</cu-custom>
		<view>
			<view class="pa">
				<table style="background:#fff;text-align: center; width: 100%
				;">
					<tr style="height: 30px;">
						<th style="color:rgb(0, 190, 110);width: 40%;">名称</th>
						<th style="color:rgb(0, 190, 110);width: 30%;">文字讲解</th>
						<th style="color:rgb(0, 190, 110);width: 30%;">视频讲解</th>
					</tr>
					<tr style="height: 40px;">
						<td style="width: 40%;">绑定家庭码</t-td>
						<td style="width: 30%;">
							<button
								style="margin-top: -5px;height: 30px;
"
								class="cu-btn bg-green shadow"
								@tap="showModal2"
								data-target="Modal"
							>
								查看
							</button>
						</td>
						<td style="width: 30%;">
							<button
								style="margin-top: -5px;height: 30px;
"
								class="cu-btn bg-green shadow"
								@tap="showModal"
								data-target="Modal"
							>
								查看
							</button>
						</td>
					</tr>
					<tr style="height: 40px;">
						<td style="width: 40%;">志愿者申请</td>
						<td style="width: 30%;">
							<button
								style="margin-top: -5px;height: 30px;
"
								class="cu-btn bg-green shadow"
								@tap="showModal3"
								data-target="Modal"
							>
								查看
							</button>
						</td>
						<td style="width: 30%;">
							<button
								style="margin-top: -5px;height: 30px;
"
								class="cu-btn bg-green shadow"
								@tap="showModal1"
								data-target="Modal"
							>
								查看
							</button>
						</td>
					</tr>
				</table>
				<view class="cu-modal" :class="modalName == 'Modal' ? 'show' : ''">
					<view class="cu-dialog">
						<view class="cu-bar bg-white justify-end">
							<view class="content">绑定家庭码</view>
							<view class="action" @tap="hideModal"><text class="cuIcon-close text-red"></text></view>
						</view>
						<view class="padding-xl">
							<video id="myVideo1" src="https://ljfl.hztlcgj.com/bdjtmlc.mp4" style="display: flex;height: 350px;width: 190px;margin: 0 auto;" object-fit="fill" @error="videoErrorCallback" @ended="endv"></video>
						</view>
					</view>
				</view>
				<view class="cu-modal" :class="modalName1 == 'Modal' ? 'show' : ''">
					<view class="cu-dialog">
						<view class="cu-bar bg-white justify-end">
							<view class="content">志愿者预约</view>
							<view class="action" @tap="hideModal1"><text class="cuIcon-close text-red"></text></view>
						</view>
						<view class="padding-xl">
							<video id="myVideo2" src="https://ljfl.hztlcgj.com/zyzyy.mp4" style="display: flex;height: 350px;width: 190px;margin: 0 auto;" object-fit="fill" @error="videoErrorCallback" @ended="endv"></video>
						</view>
					</view>
				</view>
				<view class="cu-modal" :class="modalName2 == 'Modal' ? 'show' : ''">
					<view class="cu-dialog">
						<view class="cu-bar bg-white justify-end">
							<view class="content">绑定家庭码</view>
							<view class="action" @tap="hideModal2"><text class="cuIcon-close text-red"></text></view>
						</view>
						<view class="padding-xl"><image style="height: 90px;" src="https://ljfl.hztlcgj.com/bdjtm.png"></image></view>
					</view>
				</view>
				<view class="cu-modal" :class="modalName3 == 'Modal' ? 'show' : ''">
					<view class="cu-dialog">
						<view class="cu-bar bg-white justify-end">
							<view class="content">志愿者预约</view>
							<view class="action" @tap="hideModal3"><text class="cuIcon-close text-red"></text></view>
						</view>
						<view class="padding-xl"><image style="height: 100px;" src="https://ljfl.hztlcgj.com/zyzyy.png"></image></view>
					</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
export default {
	components: {},
	data() {
		return {
			modalName: null,
			modalName1: null,
			modalName2: null,
			modalName3: null
		};
	},
	onLoad() {},
	methods: {
		endv() {
			this.modalName = null;
			this.modalName1 = null;
			this.modalName2 = null;
			this.modalName3 = null;
		},
		videoErrorCallback: function(e) {
			uni.showModal({
				content: e.target.errMsg,
				showCancel: false
			});
		},
		showModal(e) {
			this.modalName = e.currentTarget.dataset.target;
		},
		hideModal(e) {
			this.modalName = null;
			let videoContext = uni.createVideoContext('myVideo1');
			videoContext.pause();
		},
		showModal1(e) {
			this.modalName1 = e.currentTarget.dataset.target;
			let videoContext = uni.createVideoContext('myVideo2');
			videoContext.pause();
		},
		hideModal1(e) {
			this.modalName1 = null;
		},
		showModal2(e) {
			this.modalName2 = e.currentTarget.dataset.target;
		},
		hideModal2(e) {
			this.modalName2 = null;
		},
		showModal3(e) {
			this.modalName3 = e.currentTarget.dataset.target;
		},
		hideModal3(e) {
			this.modalName3 = null;
		}
	}
};
</script>

<style>
.pa {
	margin: 0px 20px 0px 20px;
	border-radius: 5px;
	margin-top: 50rpx;
}
</style>

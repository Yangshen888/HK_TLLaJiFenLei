<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/getscore/menu">
			<block slot="backText">返回</block>
			<block slot="content">赋分</block>
		</cu-custom>

		<view class="cu-form-group margin-top" style="margin: 20px 20px 0px 20px;">
			<view class="title">申请人</view>
			<input placeholder="输入手机,老年卡号,住址" name="input" focus="true" v-model="query.name" />
			<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitsearch">搜索</button>
		</view>

		<view class="hezi">
			<view class="hang">
				<view class="zuo">用户昵称</view>
				<view class="you">{{ userinfo.name }}</view>
			</view>
			<view class="hang">
				<view class="zuo">手机号码</view>
				<view class="you">{{ userinfo.phone }}</view>
			</view>
			<view class="hang">
				<view class="zuo">老年卡号</view>
				<view class="you">{{ userinfo.cardnum }}</view>
			</view>
			<view class="hang">
				<view class="zuo">家庭住址</view>
				<view class="you">{{ userinfo.address }}</view>
			</view>
		</view>

		<view class="hezi">
			<view class="cu-form-group margin-top">
				<view class="title">垃圾厢房：</view>
				<picker @change="PickerChange3" :value="index3" :range-key="'ljname'" :range="picker_room">
					<view class="picker">{{ index3 > -1 ? picker_room[index3].ljname : picker_room[0].ljname }}</view>
				</picker>
			</view>
			<!-- 			<view class="cu-form-group margin-top">
				<view class="title">垃圾类型：</view>
				<picker @change="PickerChange2" :value="index2"  :range="picker_trashtype">
					<view class="picker">
						{{picker_trashtype[index2]}}
					</view>
				</picker>
			</view> -->
			<view class="cu-form-group margin-top">
				<view class="title">评价方式：</view>
				<picker @change="PickerChange" :value="index" :range-key="'scoreName'" :range="picker">
					<view class="picker">{{ index > -1 ? picker[index].scoreName : picker[0].scoreName }}</view>
				</picker>
			</view>

			<!-- <view class="cu-form-group align-start">
				<view class="title">备注:</view>
				<textarea maxlength="-1" :disabled="modalName!=null" @input="textareaBInput" placeholder="备注信息"></textarea>
			</view> -->
			<button class="cu-btn block bg-blue margin-tb-sm lg" @click="submitfrom">确认</button>
		</view>
	</view>
</template>

<script>
import { dogetOneUser, doVolunteeraddScore, doscoreList, getTrashRoomList } from '@/api/supervisor/manager.js';

export default {
	data() {
		return {
			index: 0,
			index2: 0,
			index3: 0,
			picker: [],
			picker_trashtype: ['可回收物', '厨余垃圾', '有害垃圾', '其他垃圾'],
			picker_room: [],
			modalName: null,
			userinfo: {
				name: '',
				phone: '',
				cardnum: '',
				address: '',
				userUUid: '',
				old: ''
			},
			query: {
				name: ''
			},
			score: '',
			trashtype: '',
			trashroom: ''
		};
	},
	onLoad() {
		this.getscorelist();
		this.doTrashRoomList();
		let temp = this.$store.state.EWM.split(',');
		this.userinfo.phone = temp[0];
		this.userinfo.old = temp[1];
		this.userinfo.address = temp[2];
		if (temp[0] != '') this.query.name = temp[0];
		else if (temp[1] != '') this.query.name = temp[1];
		this.submitsearch();
	},
	methods: {
		getscorelist() {
			doscoreList().then(res => {
				console.log(res);
				if (res.data.code == 200) {
					this.picker = res.data.data;
				}
			});
		},
		doTrashRoomList() {
			let dddd = '';
			uni.getLocation({
				type: 'gcj02',
				altitude: true,
				success: res => {
					dddd = res.longitude + ',' + res.latitude;

					getTrashRoomList(dddd).then(res => {
						console.log(res);
						if (res.data.code == 200) {
							this.picker_room = res.data.data;
							this.trashroom = this.picker_room[this.index].garbageRoomUuid;
						} else {
							uni.showToast({
								icon: 'none',
								title: '请检查定位信息'
							});
							return;
						}
					});
				},
				fail: function(res) {
					console.log(res);

					uni.showModal({
						title: '提示',
						content: '定位失败',
						showCancel: false
					});
				}
			});
		},
		PickerChange(e) {
			console.log(e.detail.value);
			this.index = e.detail.value;
			this.score = this.picker[this.index].scoreUuid;
		},
		PickerChange2(e) {
			console.log(e.detail.value);
			this.index2 = e.detail.value;
			this.trashtype = this.picker_trashtype[this.index2];
		},
		PickerChange3(e) {
			console.log(e.detail.value);
			this.index3 = e.detail.value;
			this.trashroom = this.picker_room[this.index3].garbageRoomUuid;
		},
		textareaBInput(e) {
			this.textareaBValue = e.detail.value;
		},
		submitsearch() {
			dogetOneUser(this.query.name).then(res => {
				if (res.data.code == 200) {
					this.userinfo.name = res.data.data.realName;
					this.userinfo.phone = res.data.data.phone;
					this.userinfo.cardnum = res.data.data.oldCard;
					this.userinfo.address = res.data.data.address;
					this.userinfo.userUUid = res.data.data.userUUid;
				} else {
					if (this.query.name != '' && this.query.name != null)
						uni.showModal({
							title: '提示',
							content: res.data.message,
							showCancel: false
						});
				}
			});
		},
		submitfrom() {
			if (this.userinfo.userUUid != '' && this.userinfo.userUUid != null) {
				console.log(this.picker);
				this.score = this.picker[this.index].scoreUuid;
				// this.trashtype=this.picker_trashtype[this.index2];
				this.trashroom = this.picker_room[this.index3].garbageRoomUuid;
				let score = {
					systemUserUuid: this.userinfo.userUUid,
					scoreSettingUuid: this.score,
					grabageRoomId: this.trashroom
				};
				// console.log(score);
				doVolunteeraddScore(score).then(res => {
					console.log(res);
					if (res.data.code == 200) {
						uni.showModal({
							title: '提示',
							content: res.data.message,
							success: r => {
								if (r.confirm) {
									uni.redirectTo({
										url: '/pages/getscore/menu'
									});
								}
							}
						});
					} else {
						uni.showModal({
							title: '提示',
							content: res.data.message,
							showCancel: false
						});
					}
				});
			} else {
				uni.showModal({
					title: '提示',
					content: '赋分人员不能为空',
					showCancel: false
				});
			}
		}
	}
};
</script>

<style>
.hezi {
	background-color: #fff;
	margin: 20px 20px 0 20px;
	border-radius: 5px;
}
.hang {
	line-height: 45px;
	border-top: 1px solid #ccc;
	overflow: hidden;
	margin: 0 15px;
}
.hang:nth-child(1) {
	line-height: 45px;
	border-top: 0;
}
.zuo {
	float: left;
	font-weight: bold;
}
.you {
	float: right;
	color: #999;
	font-weight: bold;
}
.cu-form-group {
	border-radius: 5px;
}
</style>

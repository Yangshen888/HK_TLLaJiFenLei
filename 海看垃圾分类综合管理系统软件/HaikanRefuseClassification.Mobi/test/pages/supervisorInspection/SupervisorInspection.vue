<template>
	<view style='padding-top:2rem'>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true" tourl="/pages/home/index" style="z-index: 999999;position: fixed;width:100%;">
			<block slot="backText">返回</block>
			<block slot="content">督导员审核</block>
		</cu-custom>
		<view class="container999">
			<view>
				<view class="">
					<view class="">
						<picker @change="bindPickerChange" :value="index" :range="refusesortings">
							<view class="uni-input">垃圾分类项: {{refusesortings[index]}}</view>
						</picker>
					</view>
				</view>
			</view>
			<view style="display: inline-block;width:360px;height: 80px;text-align: center;padding-top:80px;">
				<!-- <se-row>
			<view style="float:left;margin: 0 20px 0 20px;" v-for="(item,index) in refuList.split('|')">
			<se-col class="ljfltxt" :span="4">{{item}}</se-col> -->
				<!-- </view>
		    </se-row> -->
				<!-- <view style="display: inline-block;" v-for="(item,index) in picurlList.split('|')">
			<image style="float: left;width: 100px;height: 100px;margin: 0 5px 0 5px;" class="info-image" :src="item"></image>
		</view> -->
				<!-- <image :src="imgtempPath"></image> -->
				<easy-upload :dataList="imageList" uploadUrl="https://ljfl.hztlcgj.com/api/v1/supervisorInspection/SupervisorInspection/UploadAsync" :header="header"
			 :types="category" deleteUrl='' :uploadCount="6" @successImage="successImage"></easy-upload>
			</view>

			<!-- 
			<camera style="width: 100%; height:60vh;position: absolute;z-index: 9999999;top: 4rem;" v-if="camera" device-position="back" flash="off" class="info-camera">
				<cover-view class="controls">
					<cover-image />
				</cover-view>
				<cover-view class="noticeTXT">请在指定范围内拍照</cover-view>
				<cover-image class="info-swtich" src="https://ljfl.hztlcgj.com/image/head4.png" width='20'  @click="switchDirection" />
			</camera>
			
				<button class="info-button btn-pz"  type="primary" @click="takePhoto">拍照</button>
				
			
		--><view style="clear: both;">
			</view> 
			 <view  class="info-button-view">
			 <button class="info-button" type="primary" @click="upload">提交</button></view>

		</view>
	</view>
</template>
<script>
	import http from '@/components/utils/http.js';
	import {
		createSupervisorIns
	} from '@/api/supervisorInspecion/supervisorInspection.js';
	export default {
		data() {
			return {
				header: { Authorization: 'Bearer ' + uni.getStorageSync('token') },
				imageList:[],
				category: 'image',
				scjkurl: http.baseUrl + 'api/v1/supervisorinspection/supervisorinspection/upload',
				picye: '0',
				camera: false,
				picurlList: '',
				refuList: '',
				imgtempPath: '',
				refusesortings: ['易腐垃圾', '可回收物', '其他垃圾', '有害垃圾'],
				index: 0,
				value: '垃圾分类',
				refmodel: {
					garbageSoring: '',
					picture: '',
					homeAddressUUID: '',//397CB00A-56B3-4544-994A-9E80A6E36293
				}
			}
		},
		onLoad: function() {
			this.refmodel.homeAddressUUID=wx.getStorageSync('homeuuid')
		},
		methods: {
			successImage(e) {
				console.log(555555);
				console.log(e);
				let data=JSON.parse(e.data);
				let strpath=data.data.strpath;
				console.log(data);
				console.log(strpath);
				this.refmodel.picture+=strpath+'|';
				this.refmodel.garbageSoring+=this.refuList + '|';
			},
			takePhoto() {
				if (this.picye == '') {
					this.camera = true
					const ctx = wx.createCameraContext()
					ctx.takePhoto({
						quality: 'high',
						success: (res) => {
							console.log(444444);
							console.log(res);
							this.imgtempPath = res.tempImagePath
							//this.picurlList+='https://ljfl.hztlcgj.com/UploadFiles/logo/'+res.tempImagePath.split('//')[1]+'|'
							//this.refmodel.picture=this.picurlList
							this.refmodel.garbageSoring += this.refuList + '|'
							this.picye = '0'
							console.log(this.imgtempPath)
							console.log(this.scjkurl)
							//wx.getFileSystemManager().readFileSync(this.$data.src,"base64")
							this.camera = false
							uni.uploadFile({
								url: this.scjkurl,
								filePath: res.tempImagePath,
								name: 'file',
								//请求参数
								// formData: {
								// 'uuid': this.refmodel.homeAddressUUID
								// },
								success: (uploadFileRes) => {
									console.log(2222);
									console.log(uploadFileRes);
									let data = JSON.parse(uploadFileRes.data);
									console.log(JSON.parse(uploadFileRes.data));
									//this.uploadImages.push(data.data.fname);
									this.picurlList += data.data.fname;
									console.log(this.picurlList);
									//this.$emit('successImage',uploadFileRes);
								},
								fail: (err) => {
									console.log("上传失败");
									console.log(err);
								}
							})
						}
					})
				} else {
					uni.showModal({
						title: '提示',
						content: "请先选择垃圾分类项！",
						showCancel: false
					})
				}
			},
			bindPickerChange: function(e) {
				console.log('picker发送选择改变，携带值为', e.target.value)
				this.index = e.target.value
				this.refuList = this.refusesortings[e.target.value],
					this.picye = ''
				console.log('当前垃圾分类:', this.refuList)
			},
			switchDirection() {
				if (this.camera == false) {
					this.camera = true

				} else {
					this.camera = false
				}
			},
			upload() {
				console.log("测试提交")
				console.log(this.refmodel)
				var that = this
				var imgfile;
				// for(var i=0;i<this.picurlList.split('|');i++){
				//  uni.uploadFile({
				//   url:'https://ljfl.hztlcgj.com/UploadFiles/logo/',   //上传地址
				//   filePath:this.picurlList.split('|')[i],   //上传图片对象
				//   name:'file',  //上传类型
				//   header:{
				//    "content-Type":"multipart/form-data",  //用from表单的格式传
				//    'session_token': uni.getStorageSync('token')   //和服务器约定的token
				//   //'accept':'application/json'
				//   },
				//  })
				// } 
				createSupervisorIns(this.refmodel).then(res => {
					console.log("测试添加接口")
					//console.log(res);
					if (res.data.code == 200) {
						uni.showModal({
							title: '提示',
							content: '提交' + res.data.message,
							showCancel: false,
							success: (r) => {
								if (r.confirm) {
									uni.redirectTo({
										url: '/pages/home/index'
									})
								}
							}
						})
					} else {
						uni.showModal({
							title: '提示',
							content: res.data.message,
							showCancel: false
						})
					}
				})
			}

		},
		watch: {
			value() {
				console.log(this.value)
			}
		}
	}
</script>
<style>
	.container999 {
		width: 100vw;
		font-size: 28upx;
		overflow: hidden;
		position: relative;
		top: 50px;
	}

	.noticeTXT {
		font-size: 10PX;
		color: rgb(12, 242, 240);
		text-align: center
	}

	.uni-input {
		text-align: center;
		width: 80vw;
		margin: 0 auto;
		margin-top: 10px;
		;
		padding: 10px;
		color: #333333;
		font-size: 1rem;
		background-color: #f6f6f6;
	}

	.info-swtich {
		width: 150rpx;
		height: 150rpx;
		border-radius: 150rpx;
		background-color: #dedcdc;
		display: flex;
		flex-direction: row;
		align-items: center;
		justify-content: center;
		position: absolute;
		bottom: 60rpx;
		left: 300rpx;
	}

	.ljfltxt {
		text-align: center;
		font-size: 1rem;
	}

	.info-button-view {
		padding-top: 35vh;
		margin: 0 auto;
	}

	.btn-pz {}
</style>

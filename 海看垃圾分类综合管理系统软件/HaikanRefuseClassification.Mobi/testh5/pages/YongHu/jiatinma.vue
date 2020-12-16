<template>
	<view>
		<view>
			<button @click="jumpHtml">跳转</button>
		</view>
			
	</view>
</template>
<script>

import { JiatinmaList, AddJiatinma } from '@/api/supervisor/manager.js';
	var barcode = null;
export default {
	data: () => {
		return {
			
		};
	},
	onLoad(d) {
		console.log(d)
		console.log(111)
		let cookie=document.cookie;
		console.log(cookie)
	},
	methods: {
		jumpHtml(){
			window.location.href="../../static/saoma.html"
		}
	}
};
</script>
<style scoped></style>

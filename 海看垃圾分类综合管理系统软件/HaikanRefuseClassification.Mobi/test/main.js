import Vue from 'vue'
import App from './App'
import store from './store'
import checkusertoken from 'api/user/user.js'
import basics from './pages/home/home/home.vue'
Vue.component('basics',basics)

import mycomponent from './pages/component/home.vue'
Vue.component('mycomponent',mycomponent)

import cuCustom from './colorui/components/cu-custom.vue'
Vue.component('cu-custom',cuCustom)


Vue.config.productionTip = false

Vue.prototype.$store = store
Vue.prototype.$checkusertoken = checkusertoken

App.mpType = 'app'

const app = new Vue({
	store,
    ...App
})
app.$mount()

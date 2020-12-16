//引入vue和vuex
import Vue from 'vue'
import Vuex from 'vuex'
Vue.use(Vuex)

const store = new Vuex.Store({//全局变量定义
    state: {
        hasLogin: false,
        userName: "",
		userType:"",
        userId:'',
		userDepartmentName:'',
		userDepartmentGuid:'',
		roleName:'',
        token:'',
		seltab:0,
		seltit:'首页',
		phone:'',
		openid:'',
		shopid:'',
		HomeAddressUUID:'',
		idCard:'',
    },
    mutations: {
        login:function(state, user,phone) {
			console.log(user);
            state.userName = user.userName;
            state.hasLogin = true;
			state.userType = user.userType;
            state.userId = user.userId;
			//state.userDepartmentName=user.DepartmentName;
			//state.userDepartmentGuid=user.DepartmentGuid;
			state.roleName = user.RoleName;
            state.token = user.token;
			state.phone=user.phone;
			state.openid=user.openid;
			state.EWM = "";
			state.EWMShowCount=true;
		   state.shopid=user.shopid;
		   state.HomeAddressUUID=user.HomeAddressUUID;
		   state.idCard=user.idCard;
        },
        logout(state) {
           state.userName = "";
           state.hasLogin = false;
           state.userId = '';
		   state.userType = '';
		   state.userDepartmentName= '';
		   state.userDepartmentGuid= '';
		   state.roleName ='';
           state.token = '';
		   state.phone='';
		   state.shopid='';
		   state.HomeAddressUUID='';
		   //state.openid='';
		   state.idCard='';
        }
    }
})
export default store
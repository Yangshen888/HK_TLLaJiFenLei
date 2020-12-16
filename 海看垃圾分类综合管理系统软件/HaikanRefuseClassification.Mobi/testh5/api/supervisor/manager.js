import http from '@/components/utils/http.js'


export const dogetOneUser = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/SupervisorManage/getOneUser',
    method: 'get'
  },data)
}

//VolunteeraddScore
export const doVolunteeraddScore = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/SupervisorManage/VolunteeraddScore',
    method: 'post'
  },data)
}

//获取志愿者赋分记录
  export const dogetscorelist=(data)=>{
  	return http.httpTokenRequest(
  	{
		url:'api/v1/app/SupervisorManage/AddScoreRecode',
		method:'post',
	},
  	data
  	)
  }
  
  //获取督导员管理的垃圾厢房的位置
  export const doGetTrashLocation = (data) => {
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/GetTrashLocation/'+data,
      method: 'get'
    })
  }


//微信注册
export const doCreateUser = (data) => {
	console.log(1111);
	console.log(data);
  return http.httpTokenRequest({
    url: 'api/v1/app/SendCode/CreateUser',
    method: 'post'
	
  },data
  )
}

//scoreList
  export const doscoreList = (data) => {
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/scoreList',
      method: 'get'
    })
  }
  
  //获取垃圾厢房列表
  export const getTrashRoomList = (data) => {
	  console.log(data);
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/TrashRoomList/'+data,
      method: 'get'
    })
  }
  
  //志愿者服务评价
  export const addvolunteerPJ = (data) => {
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/volunteerPJ/'+data,
      method: 'get'
    })
  }
  
  //用户注册，获取验证码
  export const getRegCode = (data) => {
	  console.log(data);
    return http.httpTokenRequest({
      url: 'api/v1/app/SendCode/GetRegCode/'+data,
      method: 'get'
    })
  }
  
  
  //商店列表
    export const getShoplist = () => {
      return http.httpTokenRequest({
        url: 'api/v1/app/OwnerManage/Shoplist',
        method: 'get'
      })
    }
	
	//s商品列表
	  export const getgoodlist = (data) => {
	    return http.httpTokenRequest({
	      url: 'api/v1/app/OwnerManage/goodlist/',
	      method: 'get'
	    },data)
	  }
	  
//AddchangeGoods
export const AddchangeGoods = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/OwnerManage/AddchangeGoods',
    method: 'post',
  },
  data
  )
}

//修改密码
  export const setNewPass = (data) => {
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/SetNewPass',
      method: 'post'
    },data)
  }
  
  //获取商家的兑换记录信息
  export const ShopaddRecordList = (data) => {
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/ShopaddRecordList/',
      method: 'get'
    },data)
  }
  
  //获取以预约的厢房
  export const TrashRoomYYList = (data) => {
  	  console.log(data);
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/TrashRoomYYList/',
      method: 'get'
    },data)
  }
  
  //判断距离
  export const Roomjuli = (data) => {
  	  console.log(data);
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/Roomjuli/',
      method: 'get'
    },data)
  }
  
  //判断是否绑定家庭码
  export const JiatinmaList = (data) => {
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/JiatinmaList/',
      method: 'get'
    },data)
  }
  
  //绑定家庭码
  export const AddJiatinma = (data) => {
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/AddJiatinma/',
      method: 'get'
    },data)
  }
  
  //解绑家庭码
  export const UntieMa = (data) => {
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/UntieMa/',
      method: 'get'
    },data)
  }
  
  //验证百姓码
  export const commonman = (data) => {
	  console.log(data);
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/Common?result='+data,
      method: 'get'
    })
  }
  //申领百姓码
  export const GetCommon = (data) => {
  	  console.log(data);
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/GetCommon',
      method: 'post'
    },data)
  }
  
  //百姓码
  export const GetCommonEWM = (data) => {
    return http.httpTokenRequest({
      url: 'api/v1/app/SupervisorManage/GetCommonEWM/'+data,
      method: 'get',
    })
  }
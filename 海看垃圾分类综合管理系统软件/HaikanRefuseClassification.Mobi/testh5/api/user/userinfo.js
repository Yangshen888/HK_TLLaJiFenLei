import http from '@/components/utils/http.js'

// editpwd
export const edituserPwd = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/rbac/user/edituserpwd',
    method: 'post',
  },data)
}
//获取个人积分记录
export const getScoreSigle = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/OwnerManage/getSingleScoreRecord',
    method: 'post',
  },data)
}

// 积分兑换记录
export const getChangesList = (data) => {
  return http.httpTokenRequest({
	  url: 'api/v1/score/owner/ChangesList',
	  method: 'post',
		data
  })
}

//获取个人信息
export const GetUserInfo = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/OwnerManage/GetUserInfo/'+data,
    method: 'get',
  }
  )
}

// 完善个人信息
export const editUserInfo = (data) => {
  return http.httpTokenRequest({
	  url: 'api/v1/app/OwnerManage/editUserInfo',
	  method: 'post',
		
  },data)
}


export const setIDCard = (data) => {
  return http.httpTokenRequest({
	  url: 'api/v1/rbac/User/SetIDCard',
	  method: 'post',
  },data)
}
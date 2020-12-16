import http from '@/components/utils/http.js'


export const dogetOneUser = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/SupervisorManage/getOneUser?idcard='+data,
    method: 'get'
  })
}

//VolunteeraddScore
export const doVolunteeraddScore = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/SupervisorManage/VolunteeraddScore',
    method: 'post',
	data
  })
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
  return http.httpTokenRequest({
    url: 'api/v1/app/SupervisorManage/CreateUser',
    method: 'post',
  },
  data
  )
}
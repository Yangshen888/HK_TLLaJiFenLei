import http from '@/components/utils/http.js'

// getalllist 
export const getalllist = () => {
  return http.httpTokenRequest({
    url: 'api/v1/app/SupervisorManage/AllList' ,
    method: 'get'
  })
}

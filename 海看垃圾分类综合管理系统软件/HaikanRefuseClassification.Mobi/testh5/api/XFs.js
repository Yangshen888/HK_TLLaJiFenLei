import http from '@/components/utils/http.js'

export const getXFsList = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/OwnerManage/getAllGrabroomPoint',
    method: 'get',
  })
}

export const test = () => {
  return http.httpTokenRequest({
    url: 'api/v1/message/test',
    method: 'get',
  })
}

export const ewm = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/OwnerManage/GetEWM/'+data,
    method: 'get',
  })
}

export const GetJiatinEWM = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/OwnerManage/GetJiatinEWM/'+data,
    method: 'get',
  })
}
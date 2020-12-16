import http from '@/components/utils/http.js'

export const bindWechat = (data) => {
  return http.httpRequest({
    url: 'api/v1/app/sendcode/BindUser',
    method: 'post',
  },data)
}

export const relieveWechat = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/sendcode/RelieveUser',
    method: 'post',
  },data)
}

export const getWechat = (data) => {
  return http.httpRequest({
    url: 'api/v1/app/sendcode/GetWechat',
    method: 'post',
  },data)
}
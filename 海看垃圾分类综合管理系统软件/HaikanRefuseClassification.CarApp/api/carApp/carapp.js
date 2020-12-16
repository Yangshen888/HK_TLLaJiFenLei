import http from '@/components/utils/http.js'

export const getCarInfo = (data) => {
  return http.httpRequest({
    url: 'api/v1/carapp/carapp/CarInfo/'+data.imei,
    method: 'get',
  })
}

export const addQuestion = (data) => {
  return http.httpRequest({
    url: 'api/v1/carapp/carapp/AddQuestion',
    method: 'post',
  },data)
}

export const getWeighRecord = (data) => {
  return http.httpRequest({
    url: 'api/v1/carapp/carapp/GetWeighRecord',
    method: 'post',
	},data)
}

export const checkWeighRecord = (data) => {
  return http.httpRequest({
    url: 'api/v1/carapp/carapp/CheckWeighRecord',
    method: 'post',
  },data)
}

export const getRecycleInfo = (data) => {
  return http.httpRequest({
    url: 'api/v1/carapp/carapp/RecycleInfo?qs='+data,
    method: 'get',
  })
}
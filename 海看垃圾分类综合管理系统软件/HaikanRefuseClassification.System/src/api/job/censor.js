import axios from '@/libs/api.request'

export const getList = (data) => {
  return axios.request({
    url: 'job/Censor/list',
    method: 'post',
    data
  })
}
//loadCarManagementDetail 详情
export const loadCarDriverDetail = (data) => {
  return axios.request({
    url: 'job/Censor/detail/' + data.guid,
    method: 'get'
  })
}
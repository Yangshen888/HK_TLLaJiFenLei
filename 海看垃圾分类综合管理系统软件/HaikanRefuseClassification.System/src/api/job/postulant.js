import axios from '@/libs/api.request'

export const getList = (data) => {
  return axios.request({
    url: 'job/postulant/list',
    method: 'post',
    data
  })
}
//loadCarManagementDetail 详情
export const loadCarDriverDetail = (data) => {
  return axios.request({
    url: 'job/postulant/detail/' + data.guid,
    method: 'get'
  })
}

export const EditAttend = (data) => {
  return axios.request({
    url: 'job/postulant/EditAttend',
    method: 'post',
    data
  })
}
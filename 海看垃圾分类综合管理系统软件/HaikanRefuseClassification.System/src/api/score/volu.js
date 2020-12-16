import axios from '@/libs/api.request'

export const getVoluList = (data) => {
  return axios.request({
    url: 'score/volu/list',
    method: 'post',
    data
  })
}

//loadCarManagementDetail 详情
export const loadCarDriverDetail = (data) => {
  return axios.request({
    url: 'base/house/detail/' + data.guid,
    method: 'get'
  })
}
// deleteCarManagement  删除单个
export const deleteVoluSerRecord = (ids) => {
  return axios.request({
    url: 'score/volu/Delete/' + ids,
    method: 'get'
  })
}

// batch command  删除多个
export const batchCommand = (data) => {
  return axios.request({
    url: 'score/volu/batch',
    method: 'get',
    params: data
  })
}
//查看详情
export const GetRecord = (data) => {
  return axios.request({
    url: 'score/volu/GetRecord?id='+data,
    method: 'get'
  })
}

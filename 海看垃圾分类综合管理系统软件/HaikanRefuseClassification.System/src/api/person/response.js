import axios from '@/libs/api.request'

export const getCarDriverList = (data) => {
  return axios.request({
    url: 'person/response/list',
    method: 'post',
    data
  })
}
// createCarManagement  添加（保存）
export const createCarDriver = (data) => {
  return axios.request({
    url: 'person/response/create',
    method: 'post',
    data
  })
}
//loadCarManagement  加载
export const loadCarDriver = (data) => {
  return axios.request({
    url: 'person/response/edit/' + data.guid,
    method: 'get'
  })
}
// editCarManagement  编辑（保存）
export const editCarDriver = (data) => {
  return axios.request({
    url: 'person/response/edit',
    method: 'post',
    data
  })
}
//loadCarManagementDetail 详情
export const showDetail = (data) => {
    return axios.request({
      url: 'person/response/detail?guid=' + data,
      method: 'get'
    })
  }
  // deleteCarManagement  删除单个
  export const deleteCarDriver = (ids) => {
    return axios.request({
      url: 'person/response/delete/' + ids,
      method: 'get'
    })
  }
  
  
  // batch command  删除多个
  export const batchCommand = (data) => {
    return axios.request({
      url: 'person/response/batch',
      method: 'get',
      params: data
    })
  }
import axios from '@/libs/api.request'

export const getCarDriverList = (data) => {
  return axios.request({
    url: 'base/college/list',
    method: 'post',
    data
  })
}
// createCarManagement  添加（保存）
export const createCarDriver = (data) => {
  return axios.request({
    url: 'base/college/create',
    method: 'post',
    data
  })
}
//垃圾箱下拉框
export const huoquxiala = (data) => {
  return axios.request({
    url: 'base/rubbish/huoqu',
    method: 'post',
    data
  })
}
//社区下拉框
export const huoquxialashequ = (data) => {
  return axios.request({
    url: 'base/college/huoqushequ',
    method: 'post',
    data
  })
}
//loadCarManagement  加载
export const loadCarDriver = (data) => {
  return axios.request({
    url: 'base/college/edit/' + data.guid,
    method: 'get'
  })
}
// editCarManagement  编辑（保存）
export const editCarDriver = (data) => {
  return axios.request({
    url: 'base/college/edit',
    method: 'post',
    data
  })
}
// loadCarManagementDetail 详情
export const loadCarDriverDetail = (data) => {
    return axios.request({
      url: 'base/college/detail',
      method: 'post',
      data
    })
  }
  // export const loadCarDriverDetail = (data) => {
  //   return axios.request({
  //     url: 'base/college/detail/' + data.guid,
  //     method: 'get'
  //   })
  // }
  // deleteCarManagement  删除单个
  export const deleteCarDriver = (ids) => {
    return axios.request({
      url: 'base/college/delete/' + ids,
      method: 'get'
    })
  }
  
  
  // batch command  删除多个
  export const batchCommand = (data) => {
    return axios.request({
      url: 'base/college/batch',
      method: 'get',
      params: data
    })
  }
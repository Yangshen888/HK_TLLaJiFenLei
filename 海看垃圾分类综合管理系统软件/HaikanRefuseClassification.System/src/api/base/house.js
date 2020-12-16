import axios from '@/libs/api.request'

export const getCarDriverList = (data) => {
  return axios.request({
    url: 'base/household/list',
    method: 'post',
    data
  })
}
// createCarManagement  添加（保存）
export const createCarDriver = (data) => {
  return axios.request({
    url: 'base/household/create',
    method: 'post',
    data
  })
}

//社区下拉框
export const getshequ = (data) => {
  return axios.request({
    url: 'base/household/huoqushequ?guid='+data,
    method: 'get'
  })
}

//loadCarManagement  加载
export const loadCarDriver = (data) => {
  return axios.request({
    url: 'base/household/edit/' + data.guid,
    method: 'get'
  })
}
// editCarManagement  编辑（保存）
export const editCarDriver = (data) => {
  return axios.request({
    url: 'base/household/edit',
    method: 'post',
    data
  })
}
//loadCarManagementDetail 详情
export const loadCarDriverDetail = (data) => {
    return axios.request({
      url: 'base/household/detail/' + data.guid,
      method: 'get'
    })
  }
  // deleteCarManagement  删除单个
  export const deleteCarDriver = (ids) => {
    return axios.request({
      url: 'base/household/delete/' + ids,
      method: 'get'
    })
  }
  
  
  // batch command  删除多个
  export const batchCommand = (data) => {
    return axios.request({
      url: 'base/household/batch',
      method: 'get',
      params: data
    })
  }
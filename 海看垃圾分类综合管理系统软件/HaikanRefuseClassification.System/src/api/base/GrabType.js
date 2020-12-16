import axios from '@/libs/api.request'

export const getList = (data) => {
  return axios.request({
    url: 'base/GrabType/list',
    method: 'post',
    data
  })
}

//导入shopInfoImport
export const shopInfoImport = (data) => {
  return axios.request({
    url: 'base/grabtype/shopInfoImport',
    method: 'post',
    data
  })
}
// createCarManagement  添加（保存）
export const create = (data) => {
  return axios.request({
    url: 'base/grabtype/create',
    method: 'post',
    data
  })
}

// editCarManagement  编辑（保存）
export const edit = (data) => {
  return axios.request({
    url: 'base/GrabType/Edit',
    method: 'post',
    data
  })
}
//  获取编辑数据
export const GetEditData = (guid) => {
  return axios.request({
    url: 'base/GrabType/GetEditData/'+guid,
    method: 'get',
  })
}
//loadCarManagementDetail 详情
export const Detail = (data) => {
  return axios.request({
    url: 'base/grabtype/detail',
    method: 'post',
    data

  })
}
// deleteCarManagement  删除单个
export const deleteOne = (ids) => {
  return axios.request({
    url: 'base/GrabType/delete/' + ids,
    method: 'get'
  })
}


// batch command  删除多个
export const batchCommand = (data) => {
  return axios.request({
    url: 'base/grabtype/batch',
    method: 'get',
    params: data
  })
}

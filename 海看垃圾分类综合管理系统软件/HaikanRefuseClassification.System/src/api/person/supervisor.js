import axios from '@/libs/api.request'

export const getCarDriverList = (data) => {
  return axios.request({
    url: 'person/supervisor/list',
    method: 'post',
    data
  })
}
// createCarManagement  添加（保存）
export const createCarDriver = (data) => {
  return axios.request({
    url: 'person/supervisor/create',
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

//垃圾箱下拉框
export const huoquxiala1 = (data) => {
  return axios.request({
    url: 'base/rubbish/huoqu1?guid='+data,
    method: 'get',
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

//督导员下拉框
export const huoquxialapeople = (data) => {
  return axios.request({
    url: 'person/supervisor/huoqupeople',
    method: 'post',
    data
  })
}
//loadCarManagement  加载
export const loadCarDriver = (data) => {
  return axios.request({
    url: 'person/supervisor/edit/' + data.guid,
    method: 'get'
  })
}
// editCarManagement  编辑（保存）
export const editCarDriver = (data) => {
  return axios.request({
    url: 'person/supervisor/edit',
    method: 'post',
    data
  })
}
//loadCarManagementDetail 详情
export const loadCarDriverDetail = (data) => {
    return axios.request({
      url: 'person/supervisor/detail/' + data.guid,
      method: 'get'
    })
  }
  // deleteCarManagement  删除单个
  export const deleteCarDriver = (ids) => {
    return axios.request({
      url: 'person/supervisor/delete/' + ids,
      method: 'get'
    })
  }
  
   // exportInfoCommand  导出
 export const exportInfoCommand = (data) => {
  return axios.request({
    url: 'person/supervisor/Batchs',
    method: 'get',
    params: data
  })
}

  
  // batch command  删除多个
  export const batchCommand = (data) => {
    return axios.request({
      url: 'person/supervisor/batch',
      method: 'get',
      params: data
    })
  }
//导入
// UserInfoImport
export const UserInfoImport = (data) => {
  return axios.request({
    url: 'person/supervisor/UserInfoImport',
    method: 'post',
    data
  })
}
  

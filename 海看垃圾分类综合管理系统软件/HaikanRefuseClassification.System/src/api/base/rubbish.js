import axios from '@/libs/api.request'

export const getCarDriverList = (data) => {
  return axios.request({
    url: 'base/rubbish/list',
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
// export const huoquxialashequ = (data) => {
//   return axios.request({
//     url: 'base/college/huoqushequ',
//     method: 'post',
//     data
//   })
// }
//乡镇下拉框
export const huoquxialatowns = (data) => {
  return axios.request({
    url: 'base/college/huoqutowns',
    method: 'post',
    data
  })
}

//乡镇下拉框
export const huoquxialatownss = (data) => {
  return axios.request({
    url: 'base/college/huoqutownss?vuuid='+data,
    method: 'get'
  })
}

//街道下拉框
export const huoquxialaaddress = (data) => {
  return axios.request({
    url: 'base/college/huoquaddress/'+data,
    method: 'get',
    data
  })
}
//社区下拉框
export const huoquxialashequ = (data) => {
  return axios.request({
    url: 'base/rubbish/huoqushequ',
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
//收运车辆下拉框
export const huoquxialache = (data) => {
  return axios.request({
    url: 'base/vehicle/huoquche',
    method: 'post',
    data
  })
}
// createCarManagement  添加（保存）
export const createCarDriver = (data) => {
  return axios.request({
    url: 'base/rubbish/create',
    method: 'post',
    data
  })
}
//loadCarManagement  加载
export const loadCarDriver = (data) => {
  return axios.request({
    url: 'base/rubbish/edit/' + data.guid,
    method: 'get'
  })
}
// editCarManagement  编辑（保存）
export const editCarDriver = (data) => {
  return axios.request({
    url: 'base/rubbish/edit',
    method: 'post',
    data
  })
}
//loadCarManagementDetail 详情
export const loadCarDriverDetail = (data) => {
    return axios.request({
      url: 'base/rubbish/detail/' + data.guid,
      method: 'get'
    })
  }
  // deleteCarManagement  删除单个
  export const deleteCarDriver = (ids) => {
    return axios.request({
      url: 'base/rubbish/delete/' + ids,
      method: 'get'
    })
  }
  
  
  // batch command  删除多个
  export const batchCommand = (data) => {
    return axios.request({
      url: 'base/rubbish/batch',
      method: 'get',
      params: data
    })
  }
  //导入
// RubbishInfoImport
export const RubbishInfoImport = (data) => {
  return axios.request({
    url: 'base/rubbish/RubbishInfoImport',
    method: 'post',
    data
  })
}

// exportInfoCommand  导出
export const exportInfoCommand = (data) => {
  return axios.request({
    url: 'base/rubbish/Batchs',
    method: 'get',
    params: data
  })
}
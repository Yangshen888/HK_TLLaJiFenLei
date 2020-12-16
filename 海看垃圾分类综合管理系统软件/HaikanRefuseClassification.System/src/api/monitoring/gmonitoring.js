import axios from '@/libs/api.request'

//导入shopInfoImport
export const shopInfoImport = (data) => {
  return axios.request({
    url: 'Monitoring/GMonitoring/shopInfoImport',
    method: 'post',
    data
  })
}

//箱房监控信息
export const getGrabageList = (data) => {
  return axios.request({
    url: 'Monitoring/GMonitoring/GrabageList',
    method: 'post',
    data
  })
}

//添加箱房监控信息
export const AddGrabageMonit = (data) => {
    return axios.request({
      url: 'Monitoring/GMonitoring/AddGrabageMonit',
      method: 'post',
      data
    })
}

//获取未绑定的箱房下拉框
export const GrabageNums = (data) => {
    return axios.request({
      url: 'Monitoring/GMonitoring/GrabageNums',
      method: 'post',
      data
    })
}

//获取选定箱房监控编辑信息
export const GetGrabageMonit = (data) => {
    return axios.request({
      url: 'Monitoring/GMonitoring/GetGrabageMonit/'+data,
      method: 'get'
    })
}

//编辑箱房监控信息
export const EditGrabageMonit = (data) => {
    return axios.request({
      url: 'Monitoring/GMonitoring/EditGrabageMonit',
      method: 'post',
      data
    })
}

// batch command  删除多个
export const batchCommand = (data) => {
    return axios.request({
      url: 'Monitoring/GMonitoring/batch',
      method: 'get',
      params: data
    })
}
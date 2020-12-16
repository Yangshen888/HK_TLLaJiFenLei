import axios from '@/libs/api.request'

//车辆监控信息
export const getCarList = (data) => {
  return axios.request({
    url: 'Monitoring/Monitoring/CarList',
    method: 'post',
    data
  })
}

//添加车辆监控信息
export const AddCarMonit = (data) => {
    return axios.request({
      url: 'Monitoring/Monitoring/AddCarMonit',
      method: 'post',
      data
    })
}

//获取未绑定的车辆下拉框
export const CarNums = (data) => {
    return axios.request({
      url: 'Monitoring/Monitoring/CarNums',
      method: 'post',
      data
    })
}

//获取选定车辆监控编辑信息
export const GetCarMonit = (data) => {
    return axios.request({
      url: 'Monitoring/Monitoring/GetCarMonit/'+data,
      method: 'get'
    })
}

//编辑车辆监控信息
export const EditCarMonit = (data) => {
    return axios.request({
      url: 'Monitoring/Monitoring/EditCarMonit',
      method: 'post',
      data
    })
}

// batch command  删除多个
export const batchCommand = (data) => {
    return axios.request({
      url: 'Monitoring/Monitoring/batch',
      method: 'get',
      params: data
    })
}
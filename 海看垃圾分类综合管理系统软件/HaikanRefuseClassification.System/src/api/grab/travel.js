import axios from '@/libs/api.request'

export const getList = (data) => {
  return axios.request({
    url: 'grab/Travel/list',
    method: 'post',
    data
  })
}

//GetLatLon 垃圾箱房经纬度信息
export const GetLatLon = (data) => {
  return axios.request({
    url: 'grab/Travel/listlonlat',
    method: 'post',
    data
  })
}

//timeGetTravel 根据时间查找
export const timeGetTravel = (data) => {
  return axios.request({
    url: 'grab/Travel/timeList',
    method: 'post',
    data
  })
}

//GetTravel 历史轨迹车辆信息
export const getTravel = (data) => {
  return axios.request({
    url: 'grab/Travel/CarList?guid=' + data,
    method: 'get'
  })
}

//获取车辆
export const GetCarLists = () => {
  return axios.request({
    url: 'grab/Travel/GetCarList',
    method: 'get'
  })
}

//获取车辆位置
export const GetCarSite = () => {
  return axios.request({
    url: 'grab/Travel/GetCarSite',
    method: 'get'
  })
}

//GetTravel 历史轨迹垃圾信息
export const getCarlist = (data) => {
  return axios.request({
    url: 'grab/Travel/ljList',
    method: 'post',
    data
  })
}

// deleteCarManagement  删除单个
export const deleteCarDriver = (ids) => {
    return axios.request({
      url: 'grab/Travel/delete/' + ids,
      method: 'get'
    })
  }
  
  
  // batch command  删除多个
  export const batchCommand = (data) => {
    return axios.request({
      url: 'grab/Travel/batch',
      method: 'get',
      params: data
    })
  }
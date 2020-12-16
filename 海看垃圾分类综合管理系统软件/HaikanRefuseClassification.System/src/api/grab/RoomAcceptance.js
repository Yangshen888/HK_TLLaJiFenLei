import axios from '@/libs/api.request'

export const getCarDriverList = (data) => {
  return axios.request({
    url: 'grab/roomacceptance/list',
    method: 'post',
    data
  })
}

//加载称重记录详情
export const getCarDriverList2 = (data) => {
  return axios.request({
    url: 'grab/roomacceptance/listcz?guid=' + data.guid + '&time=' + data.time ,
    method: 'get',
    data
  })
}

//loadCarManagement  加载
export const loadCarDriver = (data) => {
  return axios.request({
    url: 'grab/roomacceptance/edit/' + data.guid,
    method: 'get'
  })
}


  // deleteCarManagement  删除单个
  export const deleteCarDriver = (ids) => {
    return axios.request({
      url: 'grab/roomacceptance/delete/' + ids,
      method: 'get'
    })
  }
  
  
  // batch command  删除多个
  export const batchCommand = (data) => {
    return axios.request({
      url: 'grab/roomacceptance/batch',
      method: 'get',
      params: data
    })
  }
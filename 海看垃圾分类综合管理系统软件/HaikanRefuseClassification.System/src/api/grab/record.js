import axios from '@/libs/api.request'

export const getCarDriverList = (data) => {
  return axios.request({
    url: 'grab/grabrecord/list',
    method: 'post',
    data
  })
}

export const getDayOfWeightList = (data) => {
  return axios.request({
    url: 'grab/grabrecord/RecordList',
    method: 'post',
    data
  })
}

//加载称重记录详情
export const getCarDriverList2 = (data) => {
  return axios.request({
    url: 'grab/grabrecord/listcz?guid=' + data.guid + '&time=' + data.time ,
    method: 'get'
  })
}

export const loadRecordInfo = (data) => {
  return axios.request({
    url: 'grab/grabrecord/edit/' + data,
    method: 'get'
  })
}


//loadCarManagement  加载
export const loadCarDriver = (data) => {
  return axios.request({
    url: 'grab/grabrecord/edit/' + data.guid,
    method: 'get'
  })
}


export const saveRecordInfo = (data) => {
  return axios.request({
    url: 'grab/grabrecord/Edit',
    method: 'post',
    data
  })
}


  // deleteCarManagement  删除单个
  export const deleteCarDriver = (ids) => {
    return axios.request({
      url: 'grab/grabrecord/delete/' + ids,
      method: 'get'
    })
  }
  
  
  // batch command  删除多个
  export const batchCommand = (data) => {
    return axios.request({
      url: 'grab/grabrecord/batch',
      method: 'get',
      params: data
    })
  }
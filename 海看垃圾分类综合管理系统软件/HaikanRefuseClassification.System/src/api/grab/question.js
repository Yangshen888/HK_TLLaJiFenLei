import axios from '@/libs/api.request'

export const getList = (data) => {
  return axios.request({
    url: 'grab/Question/list',
    method: 'post',
    data
  })
}

//垃圾箱下拉框
export const huoquxiala = (data) => {
    return axios.request({
      url: 'grab/Question/huoqu',
      method: 'post',
      data
    })
  }

  //   编辑（保存）
export const EditQuestion = (data) => {
  return axios.request({
    url: 'grab/Question/EditQuestion',
    method: 'post',
    data
  })
}

//   获取
export const ShowQuestion = (data) => {
  return axios.request({
    url: 'grab/Question/ShowQuestion?guid=' + data,
    method: 'get'
  })
}

// deleteCarManagement  删除单个
export const deleteCarDriver = (ids) => {
    return axios.request({
      url: 'grab/Question/delete/' + ids,
      method: 'get'
    })
  }
  
  
  // batch command  删除多个
  export const batchCommand = (data) => {
    return axios.request({
      url: 'grab/Question/batch',
      method: 'get',
      params: data
    })
  }
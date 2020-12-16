import axios from '@/libs/api.request'

export const getCarDriverList = (data) => {
  return axios.request({
    url: 'data/datatj/list',
    method: 'post',
    data
  })
}

//loadCarManagement  加载
export const loadCarDriver = (data) => {
  return axios.request({
    url: 'data/datatj/edit/' + data.guid,
    method: 'get'
  })
}


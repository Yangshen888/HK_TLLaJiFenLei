import axios from '@/libs/api.request'

export const getList = (data) => {
  return axios.request({
    url: 'base/WingWarning/list',
    method: 'post',
    data
  })
}

export const getWarn = () => {
  return axios.request({
    url: 'base/WingWarning/Warning',
    method: 'post',
    
  })
}
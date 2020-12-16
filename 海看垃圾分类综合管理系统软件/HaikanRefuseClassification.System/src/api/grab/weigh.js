import axios from '@/libs/api.request'

export const getList = (data) => {
  return axios.request({
    url: 'grab/Weigh/list',
    method: 'post',
    data
  })
}
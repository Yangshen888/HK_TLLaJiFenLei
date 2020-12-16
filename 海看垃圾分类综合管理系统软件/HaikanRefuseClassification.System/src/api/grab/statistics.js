import axios from '@/libs/api.request'



export const getData3 = (data) => {
  return axios.request({
    url: 'grab/Statistics/List',
    method: 'post',
    data
  })
}

export const getData311 = () => {
  return axios.request({
    url: 'GiMap/DataBord/GetData3/',
    method: 'get'
  })
}

export const getData4 = (data) => {
  return axios.request({
    url: 'grab/Statistics/StreetList',
    method: 'post',
    data
  })
}

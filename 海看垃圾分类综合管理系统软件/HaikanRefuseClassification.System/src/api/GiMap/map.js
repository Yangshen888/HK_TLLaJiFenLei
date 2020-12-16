import axios from '@/libs/api.request'

export const getData1 = () => {
  return axios.request({
    url: 'GiMap/map/getAllMarker/',
    method: 'get'
  })
}

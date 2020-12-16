import http from '@/components/utils/http.js'

export const getAddress = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/GiMap/map/GetAddress',
    method: 'post',
  },data)
}
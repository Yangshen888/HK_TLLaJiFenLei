import http from '@/components/utils/http.js'

// export const getGrabTypeList = (data) => {
//   return http.httpTokenRequest({
//     url: 'api/v1/base/grabtype/GetListGroupByType',
//     method: 'post',
//   },data)
// }

export const allCommunity = () => {
  return http.httpRequest({
    url: 'api/v1/app/OwnerManage/AllCommunity',
    method: 'get',
  })
}

export const getAllGrabRoom = (data) => {
  return http.httpRequest({
    url: 'api/v1/carapp/carapp/GetAllGrabRoom?guid='+data,
    method: 'get',
  })
}


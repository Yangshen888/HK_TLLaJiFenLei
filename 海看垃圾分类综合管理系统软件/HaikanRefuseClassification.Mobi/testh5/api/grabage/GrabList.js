import http from '@/components/utils/http.js'

export const getGrabTypeList = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/base/grabtype/GetListGroupByType',
    method: 'post',
  },data)
}

export const getAllGrabRoom = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/OwnerManage/GetAllGrabRoom?guid='+data,
    method: 'get',
  })
}

export const allCommunity = () => {
  return http.httpTokenRequest({
    url: 'api/v1/app/OwnerManage/AllCommunity',
    method: 'get',
  })
}

export const showMessage = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/OwnerManage/ShowMessage/'+data.guid,
    method: 'get',
  })
}

export const checkSubscribe = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/app/OwnerManage/CheckSubscribe?guid='+data.guid+'&ap='+data.ap,
    method: 'get',
  })
}

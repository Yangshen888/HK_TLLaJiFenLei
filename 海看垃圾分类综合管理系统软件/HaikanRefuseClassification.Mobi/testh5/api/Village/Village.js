import http from '@/components/utils/http.js'

export const GetAllVillage = () => {
  return http.httpTokenRequest({
    url: 'api/v1/app/OwnerManage/GetAllVillage',
    method: 'post',
  })
}

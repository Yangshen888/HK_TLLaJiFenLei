import http from '@/components/utils/http.js'

export const getCarManagementList=(data)=>{
	return http.httpTokenRequest(
	{url:'api/v1/car/carManagement/applist',method:'post'},
	data
	)
}
//添加(保存)
export const createCarManagement=(data)=>{
	return http.httpTokenRequest(
	{url:'api/v1/car/carManagement/create',method:'post'},
	data
	)
}
//loadCarDispatch    加载
export const loadCarManagement = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/carManagement/edit/' + data.guid,
    method: 'get'
  })
}
// editCarDispatch    编辑（保存）
export const editCarManagement = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/carManagement/edit',
    method: 'post',
  },data)
}
//loadCarDispatchDetail    详情
export const loadCarManagementDetail = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/carManagement/detail/' + data.guid,
    method: 'get'
  })
}
// deleteCarDispatch     删除单个
export const deleteCarManagement = (ids) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/carManagement/delete/' + ids,
    method: 'get'
  })
}
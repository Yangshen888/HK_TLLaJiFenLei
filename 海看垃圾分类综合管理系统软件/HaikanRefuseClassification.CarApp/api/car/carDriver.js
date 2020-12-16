import http from '@/components/utils/http.js'

export const getCarDriverList=(data)=>{
	return http.httpTokenRequest(
	{url:'api/v1/car/carDriver/applist',method:'post'},
	data
	)
}
//添加(保存)
export const createCarDriver=(data)=>{
	return http.httpTokenRequest(
	{url:'api/v1/car/carDriver/create',method:'post'},
	data
	)
}
//loadCarDispatch    加载
export const loadCarDriver = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/carDriver/edit/' + data.guid,
    method: 'get'
  })
}
// editCarDispatch    编辑（保存）
export const editCarDriver = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/carDriver/edit',
    method: 'post',
  },data)
}
//loadCarDispatchDetail    详情
export const loadCarDriverDetail = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/carDriver/detail/' + data.guid,
    method: 'get'
  })
}
// deleteCarDispatch     删除单个
export const deleteCarDriver = (ids) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/carDriver/delete/' + ids,
    method: 'get'
  })
}

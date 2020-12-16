import http from '@/components/utils/http.js'

export const getCarDispatchList=(data)=>{
	return http.httpTokenRequest(
	{url:'api/v1/car/CarDispatch/applist',method:'post'},
	data
	)
}

//添加(保存)
export const createCarDispatch=(data)=>{
	return http.httpTokenRequest(
	{url:'api/v1/car/CarDispatch/create',method:'post'},
	data
	)
}
// createToAuditCarDispatch   添加（提交审核）
export const createToAuditCarDispatch = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/CarDispatch/createtoaudit',
    method: 'post',
  },data)
}
//loadCarDispatch    加载
export const loadCarDispatch = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/CarDispatch/edit/' + data.guid,
    method: 'get'
  })
}
// editCarDispatch    编辑（保存）
export const editCarDispatch = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/CarDispatch/edit',
    method: 'post',
  },data)
}

// editToAuditCarDispatch    编辑（提交审核）
export const editToAuditCarDispatch = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/CarDispatch/edittoaudit',
    method: 'post', 
  },data)
}
// deleteCarDispatch     删除单个
export const deleteCarDispatch = (ids) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/CarDispatch/delete/' + ids,
    method: 'get'
  })
}
//loadCarDispatchAuditDetail     审核
export const loadCarDispatchAuditDetail = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/CarDispatch/auditdetail/' + data.guid,
    method: 'get'
  })
}

//auditPassCarDispatch     审核通过
export const auditPassCarDispatch = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/CarDispatch/auditpass',
    method: 'post', 
  },data)
}
//auditNoPassCarDispatch    审核不通过
export const auditNoPassCarDispatch = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/CarDispatch/auditnopass',
    method: 'post',
  },data)
}
//loadCarDispatchDetail    详情
export const loadCarDispatchDetail = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/car/CarDispatch/detail/' + data.guid,
    method: 'get'
  })
}
//科室下拉框
//get departmentlist
export const loaddepartmentListDetail =() => {
  return http.httpTokenRequest({
    url: 'api/v1/rbac/department/DepartmentList',
    method: 'get'
  })
}

//选择驾驶员
export const getDriverName = (data) => {
  return  http.httpTokenRequest({
    url: 'api/v1/car/carDispatch/getDriverName',
    method: 'post',
  })
}
//选择车辆
export const getcar = (data) => {
  return  http.httpTokenRequest({
    url: 'api/v1/car/carDispatch/getcar',
    method: 'post',
  })
}
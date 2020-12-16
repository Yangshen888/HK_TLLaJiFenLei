import axios from '@/libs/api.request'

//获取表格数据
export const List = (data) => {
  return axios.request({
    url: 'supervisorInspection/SupervisorInspection/list',
    method: 'post',
    data
  })
}

//loadCarManagementDetail 详情
export const loadShopDetail = (data) => {
  return axios.request({
    url: 'supervisorInspection/SupervisorInspection/detail/' + data.guid,
    method: 'get'
  })
}
// deleteCarManagement  删除单个
export const deleteShop = (ids) => {
  return axios.request({
    url: 'supervisorInspection/SupervisorInspection/delete/' + ids,
    method: 'get'
  })
}


// batch command  删除多个
export const batchCommand = (data) => {
  return axios.request({
    url: 'supervisorInspection/SupervisorInspection/batch',
    method: 'get',
    params: data
  })
}


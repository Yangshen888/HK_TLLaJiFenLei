import axios from '@/libs/api.request'

export const getConsumableList = (data) => {
  return axios.request({
    url: 'material/consumable/list',
    method: 'post',
    data
  })
}
// createConsumable
export const createConsumable = (data) => {
  return axios.request({
    url: 'material/consumable/create',
    method: 'post',
    data
  })
}

//loadConsumable
export const loadConsumable = (data) => {
  return axios.request({
    url: 'material/consumable/edit/' + data.guid,
    method: 'get'
  })
}
//loadConsumableauditdetail
export const loadConsumableAuditDetail = (data) => {
  return axios.request({
    url: 'material/consumable/auditdetail/' + data.guid,
    method: 'get'
  })
}
//loadConsumabledetail
export const loadConsumableDetail = (data) => {
  return axios.request({
    url: 'material/consumable/detail/' + data.guid,
    method: 'get'
  })
}

// editConsumable
export const editConsumable = (data) => {
  return axios.request({
    url: 'material/consumable/edit',
    method: 'post',
    data
  })
}
// delete Consumable
export const deleteConsumable = (ids) => {
  return axios.request({
    url: 'material/consumable/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'material/consumable/batch',
    method: 'get',
    params: data
  })
}
// createToAuditConsumable
export const createToAuditConsumable = (data) => {
  return axios.request({
    url: 'material/consumable/createtoaudit',
    method: 'post',
    data
  })
}
// editConsumable
export const editToAuditConsumable = (data) => {
  return axios.request({
    url: 'material/consumable/edittoaudit',
    method: 'post',
    data
  })
}
//auditPassConsumable
export const auditPassConsumable = (data) => {
  return axios.request({
    url: 'material/consumable/auditpass',
    method: 'post',
    data
  })
}
//auditNoPassConsumable
export const auditNoPassConsumable = (data) => {
  return axios.request({
    url: 'material/consumable/auditnopass',
    method: 'post',
    data
  })
}

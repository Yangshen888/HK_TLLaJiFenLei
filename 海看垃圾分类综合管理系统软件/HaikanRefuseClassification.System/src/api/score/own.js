
import axios from '@/libs/api.request'

export const getOwnList = (data) => {
  return axios.request({
    url: 'score/owner/list',
    method: 'post',
     data
  })
}
// createCarManagement  添加（保存）
export const createCarDriver = (data) => {
  return axios.request({
    url: 'base/house/create',
    method: 'post',
    data
  })
}
//loadCarManagement  加载
export const loadCarDriver = (data) => {
  return axios.request({
    url: 'base/house/edit/' + data.guid,
    method: 'get'
  })
}
// editCarManagement  编辑（保存）
export const editCarDriver = (data) => {
  return axios.request({
    url: 'base/house/edit',
    method: 'post',
    data
  })
}
//loadCarManagementDetail 详情
export const loadCarDriverDetail = (data) => {
  return axios.request({
    url: 'base/house/detail/' + data.guid,
    method: 'get'
  })
}
// deleteCarManagement  删除单个
export const deleteGrabDisRecord = (ids) => {
  return axios.request({
    url: 'score/owner/Delete/' + ids,
    method: 'get'
  })
}

// batch command  删除多个
export const batchCommand = (data) => {
  return axios.request({
    url: 'score/owner/batch',
    method: 'get',
    params: data
  })
}

//查看详情
export const GetRecord = (data) => {
  return axios.request({
    url: 'score/owner/GetRecord',
    method: 'post',
    data
  })
}

//获取积分列表
export const getScoreList = (data) => {
  return axios.request({
    url: 'score/owner/ScoreList',
    method: 'post',
     data
  })
}

// createCarManagement  添加（保存）
export const addscore = (data) => {
  return axios.request({
    url: 'score/owner/Createscore',
    method: 'post',
    data
  })
}
// 删除积分评价
export const deletescore = (ids) => {
  return axios.request({
    url: 'score/owner/DeleteScoreinfo/' + ids,
    method: 'get'
  })
}

//每小时对应的积分
export const doHourintegral = (score) => {
  return axios.request({
    url: 'score/owner/Hourintegral/' + score,
    method: 'get'
  })
}

//每天最多赋分次数（家庭组）
export const doDivisionTimes = (score) => {
  return axios.request({
    url: 'score/owner/DivisionTimes/' + score,
    method: 'get'
  })
}

// editCarManagement  编辑（保存）
export const editCarDriver1 = (data) => {
  return axios.request({
    url: 'score/owner/edit',
    method: 'post',
    data
  })
}

//loadCarManagement  加载
export const loadCarDriver1 = (data) => {
  return axios.request({
    url: 'score/owner/edit/' + data.guid,
    method: 'get'
  })
}

//获取赋分次数与对应积分
export const getSetNumber = () => {
  return axios.request({
    url: 'score/owner/getSetNumber',
    method: 'get'
  })
}

// 积分兑换记录
export const getChangesList = (data) => {
  return axios.request({
    url: 'score/owner/ChangesList',
    method: 'post',
    data
  })
}

// 积分兑换记录
export const getChangesList1 = (data) => {
  return axios.request({
    url: 'score/owner/ChangesList1',
    method: 'post',
    data
  })
}


//未兑换 详情
export const GetHomeScore = (data) => {
  return axios.request({
    url: 'score/owner/HomeScore?guid=' + data,
    method: 'get'
  })
}

//未兑换 编辑
export const GetEditHomeScore = (data) => {
  return axios.request({
    url: 'score/owner/EditHomeScore',
    method: 'post',
    data
  })
}
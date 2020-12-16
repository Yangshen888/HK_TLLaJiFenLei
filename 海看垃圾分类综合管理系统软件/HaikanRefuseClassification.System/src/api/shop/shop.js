import axios from '@/libs/api.request'

//导入shopImport
export const shopImport = (data) => {
  return axios.request({
    url: 'person/shop/shopImport',
    method: 'post',
    data
  })
}



export const getShopList = (data) => {
  return axios.request({
    url: 'person/shop/list',
    method: 'post',
    data
  })
}
// createCarManagement  添加（保存）
export const create = (data) => {
  return axios.request({
    url: 'person/shop/Create',
    method: 'post',
    data
  })
}



//loadCarManagement  加载
export const loadShopEditData = (data) => {
  return axios.request({
    url: 'person/shop/GetEdit/' + data,
    method: 'get'
  })
}

//loadShopEditScore  扣除商店积分(保存)
export const loadShopEditScore = (data) => {
  return axios.request({
    url: 'person/shop/EditShopScore',
    method: 'post',
    data
  })
}

//dbScore  判断积分
export const dbScore = (data) => {
  return axios.request({
    url: 'person/shop/DbScore',
    method: 'post',
    data
  })
}


// editCarManagement  编辑（保存）
export const editSave = (data) => {
  return axios.request({
    url: 'person/shop/edit',
    method: 'post',
    data
  })
}
//loadCarManagementDetail 详情
export const loadShopDetail = (data) => {
  return axios.request({
    url: 'person/shop/detail/' + data.guid,
    method: 'get'
  })
}
// deleteCarManagement  删除单个
export const deleteShop = (ids) => {
  return axios.request({
    url: 'person/shop/delete/' + ids,
    method: 'get'
  })
}


// batch command  删除多个
export const batchCommand = (data) => {
  return axios.request({
    url: 'person/shop/batch',
    method: 'get',
    params: data
  })
}
//加载商品列表
export const LoadGoods = (data) => {
  return axios.request({
    url: 'person/shop/GoodsList/'+data,
    method: 'get',
  })
}


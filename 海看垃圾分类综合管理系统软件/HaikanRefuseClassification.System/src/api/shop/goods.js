import axios from '@/libs/api.request'

//获取表格数据
export const List = (data) => {
  return axios.request({
    url: 'person/goods/list',
    method: 'post',
    data
  })
}
// createCarManagement  添加（保存）
export const create = (data) => {
  return axios.request({
    url: 'person/goods/Create',
    method: 'post',
    data
  })
}

//商店下拉框
export const ShopList = () => {
  return axios.request({
    url: 'person/goods/ShopList',
    method: 'get'
  })
}

//获取编辑数据
export const loadGoodsEditData = (data) => {
  return axios.request({
    url: 'person/goods/Edit/' + data,
    method: 'get'
  })
}
// editCarManagement  编辑（保存）
export const editSave = (data) => {
  return axios.request({
    url: 'person/goods/Edit',
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


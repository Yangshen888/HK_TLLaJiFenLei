import axios from '@/libs/api.request'

//导入shopInfoImport
export const shopInfoImport = (data) => {
  return axios.request({
    url: 'person/address/shopInfoImport',
    method: 'post',
    data
  })
}

//加载表格数据getAddressInfo
export const getAddressInfo = (data) => {
  return axios.request({
    url: 'person/address/list',
    method: 'post',
    data
  })
}

//一键导出数据
export const yjexportInfo = (data) => {
  return axios.request({
    url: 'person/address/yjExportInfo',
    method: 'post',
    data
  })
}


  // exportInfoCommand  导出
 export const exportInfoCommand = (data) => {
    return axios.request({
      url: 'person/address/batch',
      method: 'get',
      params: data
    })
  }



  
//地址库对接
export const btnToken = () => {
  return axios.request({
    url: 'person/address/btnTokenAsync',
    method: 'post',
  })
}


//添加地址
export const create = (data) => {
  return axios.request({
    url: 'person/address/Create',
    method: 'post',
    data
  })
}


//  编辑加载
export const loadEditData = (data) => {
  return axios.request({
    url: 'person/address/getEdit/' + data.guid,
    method: 'get'
  })
}



//  编辑（保存）
export const editSave = (data) => {
  return axios.request({
    url: 'person/address/edit',
    method: 'post',
    data
  })
}


//社区下拉框
export const getshequ = (data) => {
  return axios.request({
    url: 'person/address/huoqushequ',
    method: 'post',
    data
  })
}

//社区下拉框
export const getshequ1 = (data) => {
  return axios.request({
    url: 'base/household/huoqushequ?guid='+data,
    method: 'get',
    data
  })
}

//家庭下拉框
export const getfamily = (data) => {
  return axios.request({
    url: 'person/address/GetFamily?vill='+data.vill,
    method: 'get'
  })
}


//家庭下拉框
export const getfamily1 = (data) => {
  return axios.request({
    url: 'person/address/GetFamily1?vill='+data.vill,
    method: 'get'
  })
}
import axios from '@/libs/api.request'

export const getUserList = (data) => {
  return axios.request({
    url: 'rbac/user/list',
    method: 'post',
    data
  })
}

// createUser
export const createUser = (data) => {
  return axios.request({
    url: 'rbac/user/create',
    method: 'post',
    data
  })
}

// createUser店铺信息
export const getShopList = (data) => {
  return axios.request({
    url: 'rbac/user/listshop',
    method: 'post',
    data
  })
}


//loadUser
export const loadUser = (data) => {
  return axios.request({
    url: 'rbac/user/edit/' + data.guid,
    method: 'get'
  })
}

// editUser
export const editUser = (data) => {
  return axios.request({
    url: 'rbac/user/edit',
    method: 'post',
    data
  })
}

// delete user
export const deleteUser = (ids) => {
  return axios.request({
    url: 'rbac/user/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'rbac/user/batch',
    method: 'get',
    params: data
  })
}

// save user roles
export const saveUserRoles = (data) => {
  return axios.request({
    url: 'rbac/user/save_roles',
    method: 'post',
    data
  })
}
// editpwd
export const editUserPwd = (data) => {
  return axios.request({
    url: 'rbac/user/edituserpwd',
    method: 'post',
    data
  })
}
// UserInfoImport
export const UserInfoImport = (data) => {
  return axios.request({
    url: 'rbac/user/UserInfoImport',
    method: 'post',
    data
  })
}


export const showRoleTree = () => {
  return axios.request({
    url: 'rbac/user/ShowRoleTree',
    method: 'get',
  })
}


export const GetRoleList = () => {
  return axios.request({
    url: 'rbac/user/RoleList',
    method: 'get',
  })
}
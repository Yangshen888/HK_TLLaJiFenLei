import axios from '@/libs/api.request'

export const getWorktimeList = (data) => {
  return axios.request({
    url: 'attendancemanagement/worktime/list',
    method: 'post',
    data
  })
}
// createDepartment
export const createWorktime = (data) => {
  return axios.request({
    url: 'attendancemanagement/worktime/create',
    method: 'post',
    data
  })
}

//loadDepartment
export const loadWorktime = (data) => {
  return axios.request({
    url: 'attendancemanagement/worktime/edit/' + data.guid,
    method: 'get'
  })
}
// editDepartment
export const editWorktime = (data) => {
  return axios.request({
    url: 'attendancemanagement/worktime/edit',
    method: 'post',
    data
  })
}
// delete department
export const deleteWorktime = (ids) => {
  return axios.request({
    url: 'attendancemanagement/worktime/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'attendancemanagement/worktime/batch',
    method: 'get',
    params: data
  })
}

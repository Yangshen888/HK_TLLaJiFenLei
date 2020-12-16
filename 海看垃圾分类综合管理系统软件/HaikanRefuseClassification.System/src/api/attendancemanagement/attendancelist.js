import axios from '@/libs/api.request'

export const getAttendancelistList = (data) => {
  return axios.request({
    url: 'attendancemanagement/attendancelist/list',
    method: 'post',
    data
  })
}
//loadAttendanceDetail
export const loadAttendanceDetail = (data) => {
  return axios.request({
    url: 'attendancemanagement/attendancelist/detail/' + data.guid,
    method: 'get'
  })
}

import http from '@/components/utils/http.js'

export const getAttendancelistYingdao = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/attendancemanagement/attendancestatistics/listyingdao',
    method: 'post' 
  },data)
}
export const getAttendancelistShidao = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/attendancemanagement/attendancestatistics/applistshidao',
    method: 'post'
  },
    data)
}
export const getAttendancelistWeidao = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/attendancemanagement/attendancestatistics/listweidao',
    method: 'post'
  },
    data)
}
export const getAttendancelistChidao = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/attendancemanagement/attendancestatistics/applistchidao',
    method: 'post'
  },
    data)
}
export const getAttendancelistZaotui = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/attendancemanagement/attendancestatistics/applistzaotui',
    method: 'post'
  },
    data)
}
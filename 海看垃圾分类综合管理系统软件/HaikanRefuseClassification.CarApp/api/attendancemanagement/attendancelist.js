import http from '@/components/utils/http.js'


//clockin
export const doClockIn = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/attendancemanagement/attendancelist/clockin',
    method: 'post', 
  },data)
}

//clocback
export const doClockBack = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/attendancemanagement/attendancelist/clockback',
    method: 'post', 
  },data)
}

//loadclock
export const loadclock = () => {
  return http.httpTokenRequest({
    url: 'api/v1/attendancemanagement/attendancelist/loadclock',
    method: 'post', 
  })
}
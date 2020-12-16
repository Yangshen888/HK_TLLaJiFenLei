import http from '@/components/utils/http.js'

export const getNoticeList=(data)=>{
	return http.httpTokenRequest(
	{url:'api/v1/notice/notices/applist',method:'post'},
	data
	)
}

// delete Voteinitiate
export const deleteNoticeinitiate = (ids) => {
  return http.httpTokenRequest({
    url: 'api/v1/notice/notices/delete/' + ids,
    method: 'get'
  })
}

//loadVotedetail
export const  loadNoticedetail = (data) => {
  return http.httpTokenRequest({
    url: 'api/v1/notice/notices/detail/' + data.guid,
    method: 'get'
  })
}









import http from '@/components/utils/http.js'


export const getWXOpenAuth = (data) => {
	return http.httpTokenRequest({
		url: 'api/oauth/WXOpenAuth?openid=' + data,
		method: 'get'
	})
}

export const WXAuth = (data) => {
	return http.httpRequest({
		url: 'api/oauth/WXAuth',
		method: 'post'
	},data)
}

export const WXPhone = (data) => {
	return http.httpRequest({
		url: 'api/oauth/WXPhone',
		method: 'post'
	},data)
}
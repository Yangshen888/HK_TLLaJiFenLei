import http from '@/components/utils/http.js'

//提交督导员审察
export const createSupervisorIns=(data)=>{
  	return http.httpTokenRequest(
  	{
		url:'api/v1/supervisorinspection/supervisorinspection/createsupervisor',
		method:'post',
	},data
  	)
  }

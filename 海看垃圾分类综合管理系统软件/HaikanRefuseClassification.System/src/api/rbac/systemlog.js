import axios from '@/libs/api.request'

//日志列表
export const getLogList=(data)=>{
    console.log(data)
    return axios.request({
        url:'rbac/Systemjournal/List',
        method:'post',
        data
    })
}

//查看日志
export const getLogByID=(data)=>{
    return axios.request({
        url:'rbac/Systemjournal/GetByid/'+data.guid,
        method:'get'
    })
}
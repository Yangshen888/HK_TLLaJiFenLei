import axios from '@/libs/api.request'
// 易腐垃圾统计信息

//加载易腐垃圾数据
export const getPerishRubbishList = (data) => {
    return axios.request({
      url: 'grab/perishrubbish/PerishRubbishAllListTwo',
      method: 'post',
      data
    })
  }
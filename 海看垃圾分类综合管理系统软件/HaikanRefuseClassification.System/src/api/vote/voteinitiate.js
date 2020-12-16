import axios from '@/libs/api.request'

export const getVoteinitiateList = (data) => {
  return axios.request({
    url: 'vote/voteinitiate/list',
    method: 'post',
    data
  })
}
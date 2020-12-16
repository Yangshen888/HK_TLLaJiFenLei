import axios from '@/libs/api.request'

export const getCarDispatchList = (data) => {
  return axios.request({
    url: 'car/CarDispatch/list',
    method: 'post',
    data
  })
}
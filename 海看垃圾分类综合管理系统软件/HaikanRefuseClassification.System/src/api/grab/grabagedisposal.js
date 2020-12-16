import axios from '@/libs/api.request'

  export const getGrabageDisposal = (data) => {
    return axios.request({
      url: 'grab/GrabageDisposal/list',
      method: 'post',
      data
    })
  }


  export const DelGrabageDisposal = (ids) => {
    return axios.request({
      url: 'grab/GrabageDisposal/Delete/'+ids,
      method: 'get',
    })
  }
  export const BatchGrabageDisposal = (data) => {
    return axios.request({
      url: 'grab/GrabageDisposal/Batch',
      method: 'post',
      data:data
    })
  }

  export const disUpdate = (data) => {
    return axios.request({
      url: 'grab/GrabageDisposal/disUpdate?guid='+data,
      method: 'get',
    })
  }

  
  export const disUpdateNo = (data) => {
    return axios.request({
      url: 'grab/GrabageDisposal/disUpdateNo?guid='+data,
      method: 'get',
    })
  }


  export const getVillageList = (data) => {
    return axios.request({
      url: 'grab/GrabageDisposal/VillageList',
      method: 'post',
      data
    })
  }

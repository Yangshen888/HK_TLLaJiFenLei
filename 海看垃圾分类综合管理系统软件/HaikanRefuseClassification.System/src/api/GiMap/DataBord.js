import axios from '@/libs/api.request'

export const getData1 = () => {
  return axios.request({
    url: 'GiMap/DataBord/GetData1/',
    method: 'get'
  })
}

export const getData4 = () => {
  return axios.request({
    url: 'GiMap/DataBord/GetData4/',
    method: 'get'
  })
}

export const getData2 = () => {
  return axios.request({
    url: 'GiMap/DataBord/GetData2/',
    method: 'get'
  })
}

export const getData3 = () => {
  return axios.request({
    url: 'GiMap/DataBord/GetData3/',
    method: 'get'
  })
}

export const getData6 = () => {
  return axios.request({
    url: 'GiMap/DataBord/GetData6/',
    method: 'get'
  })
}

export const getData7 = () => {
  return axios.request({
    url: 'GiMap/DataBord/GetData7/',
    method: 'get'
  })
}

export const getData8 = () => {
  return axios.request({
    url: 'GiMap/DataBord/GetData8/',
    method: 'get'
  })
}

export const getHPageData1 = () => {
  return axios.request({
    url: 'GiMap/DataBord/GetHPageData1',
    method: 'get'
  })
}

export const GetVisitorNum = () => {
  return axios.request({
    url: 'GiMap/DataBord/GetVisitorNum',
    method: 'get'
  })
}

export const mapgetData1 = () => {
  return axios.request({
    url: 'GiMap/map/getAllMarker/',
    method: 'get'
  })
}
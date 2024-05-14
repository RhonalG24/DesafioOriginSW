import axios from 'axios'

const  baseURL = 'https://localhost:7210/api/Account';
export const accountApi = {
    api: axios.create({
        baseURL: baseURL
    }),
    getAllAccounts () {
        return this.api.get('/')
    },
    getAccount (id) {
        return this.api.get(`/${id}`)
    },
  
};
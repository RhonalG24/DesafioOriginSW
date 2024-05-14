import axios from 'axios'

const baseURL = 'https://localhost:7210/api/BankCard';

export const bankCardApi = {
    baseURL: baseURL,
    api: axios.create({
        baseURL: baseURL,
        headers: {'Access-Control-Allow-Origin': '*'}
    }),
    getAllBankCards () {
        return this.api.get('/')
            .then(response => {return response})
            .catch(error => {return error})
    },
    getBankCard (id) {
        return this.api.get(`/${id}`)
            .then(response => {return response})
            .catch(error => {return error})
    },
    async checkBankCardNumber (bankCardNumber) {
        await this.api.get(`/Check/Number/${bankCardNumber}`)
            .then(response => {
                return response})
            .catch(error => { return error})
    },
    checkBankCardPin (body) {
        return this.api.post(`/${body.bankCardId}`, body)
            .then(response => {return response})
            .catch(error => {return error})
    },
  
};
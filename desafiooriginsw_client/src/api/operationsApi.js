import axios from 'axios'

const baseURL= 'https://localhost:7210/api/Operation';

export const OperationsApi = {
    baseURL: baseURL,
    api: axios.create({
        baseURL: baseURL,
        headers: {'Access-Control-Allow-Origin': '*'}
    }),
    createOpearionUrl: `${baseURL}/`,
    createBalanceOperationUrl: `${baseURL}/balance`,
    createWithdrawOperationUrl: `${baseURL}/withdraw`,
};
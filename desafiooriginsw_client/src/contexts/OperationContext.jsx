import { createContext, useState } from 'react';

const OperationContext = createContext();

const OperationProvider = ({ children }) => {
    const [id, setId] = useState();
    const [idBankCard, setIdBankCard] = useState();
    const [idOperationType, setidOperationType] = useState();
    const [date, setDate] = useState();
    const [amount, setAmount] = useState();

    const data = {
        id, setId, idBankCard, setIdBankCard, idOperationType, setidOperationType, date, setDate, amount, setAmount
    }
    return <OperationContext.Provider value={data}>{children}</OperationContext.Provider>
}

export { OperationProvider };
export default OperationContext;
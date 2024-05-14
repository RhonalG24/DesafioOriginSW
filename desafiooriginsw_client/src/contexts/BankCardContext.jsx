import { createContext, useState} from 'react';

const BankCardContext = createContext();

const BankCardProvider = ({ children }) => {
    const [id, setId] = useState(0);
    const [cardNumber, setCardNumber] = useState('');
    const [expiryDate, setExpiryDate] = useState(Date.now());

    const data = {
        id, setId, cardNumber, setCardNumber, expiryDate, setExpiryDate
    }
    return <BankCardContext.Provider value={data}>{children}</BankCardContext.Provider>
}

export { BankCardProvider };
export default BankCardContext;
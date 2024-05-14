import { createContext, useState } from 'react';

const ErrorContext = createContext();

const ErrorProvider = ({ children }) => {
    const [message, setMessage] = useState('');
    const [navigateTo, setNavigateTo] = useState('');



    const data = {
        message, setMessage, navigateTo, setNavigateTo
    }
    return <ErrorContext.Provider value={{ data }}>{ children }</ErrorContext.Provider>

}


export { ErrorProvider};
export default ErrorContext;
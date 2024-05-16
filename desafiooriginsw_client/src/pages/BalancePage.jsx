import {React, useState, useEffect} from 'react';
import { storage } from '../utils/Storage';
import NavigateButton from '../components/NavigateButton';
import { OperationsApi } from '../api/operationsApi';
import axios from 'axios';


function BalancePage() {
  const [balance, setBalance] = useState(null);
  const [bankCardNumber, setBankCardNumber] = useState(null);
  const [cardExpiryDate, setCardExpiryDate] = useState(null);
  const thClassName = 'px-6 mx-auto py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider';
  const tdClassName = 'px-6 py-4 whitespace-nowrap'

  useEffect(()=> {

    setBalance(data => data = storage.get('balance'));
    setBankCardNumber(data => data =  storage.get('bankCardNumber'));
    setCardExpiryDate(data => data = storage.get('cardExpiryDate'));
    // console.log(balance)
  }, [])
  return (
    <div className='max-w-xl mx-auto'>
      <h1>Balance</h1>
      <table className="mx-auto table-auto min-w-full divide-y divide-gray-200">
        <thead className='bg-gray-50'>
          <tr className='justify-items-center'> 
            <th className={thClassName}>N° Tarjeta</th>
            <th className={thClassName}>Fecha de vecimiento </th>
            <th className={thClassName}>Balance en la cuenta</th>
          </tr>
        </thead>
        <tbody className='bg-white divide-y divide-gray-200'>
          <tr>
            <td className={tdClassName}>{bankCardNumber}</td>
            <td className={tdClassName}>{cardExpiryDate}</td>
            <td className={tdClassName}>$ {balance}</td>
            
          </tr>

        </tbody>
      </table>
      <NavigateButton value={"/operations"} text={"Atras"}></NavigateButton>
          <NavigateButton value={"/home"} text={"Salir"} color={"red"}></NavigateButton>
    </div>
  )
}

export default BalancePage;
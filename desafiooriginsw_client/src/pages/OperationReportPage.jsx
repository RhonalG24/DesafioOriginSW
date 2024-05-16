import React, { useEffect, useState } from 'react';

import { storage } from '../utils/Storage';
import NavigateButton from '../components/NavigateButton';

function OperationReportPage() {
  const [operationId, setOperationId] = useState(null);
  const [balance, setBalance] = useState(null);
  const [bankCardNumber, setBankCardNumber] = useState(null);
  const [cardExpiryDate, setCardExpiryDate] = useState(null);
  const [withdrawalAmount, setWithdrawalAmount] = useState(null);
  const [operationDateTime, setOperationDateTime] = useState(null);
  const thClassName = 'px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider';
  const tdClassName = 'px-6 py-4 whitespace-nowrap'

  useEffect(()=> {
    setBalance(data => data = storage.get('balance'));
    setBankCardNumber(data => data =  storage.get('bankCardNumber'));
    setWithdrawalAmount(data => data = storage.get('withdrawalAmount'));
    setOperationDateTime(data => data = storage.get('operationDateTime'));
  }, [])
  return (
    <div className='max-w-xl mx-auto'>
    <h1>Retiro</h1>
    <table className="table-auto min-w-full divide-y divide-gray-200">
      <thead className='bg-gray-50'>
        <tr>
          <th className={thClassName}>N° Tarjeta</th>
          <th className={thClassName}>Fecha y hora</th>
          <th className={thClassName}>Cantidad retirada</th>
          <th className={thClassName}>Balance en la cuenta</th>
        </tr>
      </thead>
      <tbody className='bg-white divide-y divide-gray-200'>
        <tr>
          <td className={tdClassName}>{bankCardNumber}</td>
          <td className={tdClassName}>{operationDateTime}</td>
          <td className={tdClassName}>$ {withdrawalAmount}</td>
          <td className={tdClassName}>$ {balance}</td>
        </tr>
      </tbody>
    </table>
    <NavigateButton value={"/operations"} text={"Atras"}></NavigateButton>
          <NavigateButton value={"/home"} text={"Salir"} color={"red"}></NavigateButton>
  </div>
  );
}

export default OperationReportPage;
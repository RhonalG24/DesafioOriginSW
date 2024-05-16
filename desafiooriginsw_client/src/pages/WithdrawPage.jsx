import {React, useState} from 'react';
import { useNavigate } from 'react-router-dom';
import NumericButton from '../components/NumericButton';
import ActionButton from '../components/ActionButton';
import NavigateButton from '../components/NavigateButton';
import axios from 'axios';
import { OperationsApi } from '../api/operationsApi';
import { storage } from '../utils/Storage';

function WithdrawPage() {
  const [inputValue, setInputValue] = useState("");
  const bankCardId = storage.get('bankCardId');
  const navigate = useNavigate()

  const addValue = (e) => {
      let newValue = `${inputValue}${e}`;

      setInputValue(newValue);

  }

  const clean = () => {

      setInputValue('');
      document.getElementById("formatInputField").value = "";
  }

  const submit = async () => {
      
      const requestData = {
        "bank_card_id": bankCardId,
        "withdrawal_amount": parseFloat(inputValue)
      };
      console.log(typeof(requestData.withdrawal_amount))
      try{
        const response = await axios.post(`${OperationsApi.baseURL}/withdrawal/`, 
        {
          "bank_card_id": bankCardId,
          "withdrawal_amount": parseFloat(inputValue)
        },
          { 
            headers: {
              'Content-Type': 'application/json'
            }
          }
        );
        console.log(response)
        if(response.data.statusCode !== 200){
          storage.set("errorMessage", response.data.errorsMessage);
          storage.set("navigateTo", "/withdraw")
          navigate('/error')
        }
        if(response.status === 200){
          storage.set('balance', response.data.result.balance);
          storage.set('withdrawalAmount', response.data.result.withdrawal_amount);
          storage.set('operationDateTime', response.data.result.datetime);
          navigate('/operationReport');
          return;
        }

      }catch (error){
        console.log(error)
        storage.set("errorMessage", error.response.status === 400 ? "Fondos insuficientes" : error.response.data.errorsMessage);
        storage.set("navigateTo", "/withdraw")
        navigate('/error')
        return;
      }
      
  }
  return (
    <div className='max-w-xl mx-auto'>
        <h1 className="text-black-700 font-semibold">{'Ingrese el monto a retirar'}</h1>
        <input id="formatInputField" name="formatInputField" type={ 'text' }
            value={inputValue} placeholder={'$ 0.00'} disabled required 
            className=" h-auto border rounded-md p-2 my-5" ></input>

            <div>
                <NumericButton number={1} addValue={addValue}></NumericButton>
                <NumericButton number={2} addValue={addValue}></NumericButton>
                <NumericButton number={3} addValue={addValue}></NumericButton>
            </div>
            <div>
                <NumericButton number={4} addValue={addValue}></NumericButton>
                <NumericButton number={5} addValue={addValue}></NumericButton>
                <NumericButton number={6} addValue={addValue}></NumericButton>
            </div>
            <div>
                <NumericButton number={7} addValue={addValue}></NumericButton>
                <NumericButton number={8} addValue={addValue}></NumericButton>
                <NumericButton number={9} addValue={addValue}></NumericButton>
            </div>
            <div>
                <NumericButton number={0} addValue={addValue}></NumericButton>
          </div>
          <div className="mt-5">
              <ActionButton value="Limpiar" text={"Limpiar"} whenClick={clean} color={"yellow"}></ActionButton>

                <button type="button" onClick={ submit }
                  className='bg-white-500 hover:bg-green-500 text-green-700 font-semibold hover:text-white py-2 px-4 border border-green-500 hover:border-transparent rounded m-2'
                >Aceptar</button>
            </div>
            <div>
              <NavigateButton value={"/operations"} text={"Atras"}></NavigateButton>
              <NavigateButton value={"/home"} text={"Salir"} color={"red" }></NavigateButton>

            </div>
    </div>
  );
}

export default WithdrawPage;
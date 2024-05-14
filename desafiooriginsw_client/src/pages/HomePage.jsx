import { useContext, useEffect } from 'react';
import { React,  useState } from 'react';
import InputScreen from '../components/InputScreen';
import { useNavigate } from 'react-router-dom';
import { storage } from '../utils/Storage';
import { bankCardApi } from '../api/bankCardApi';
import axios from 'axios';

function HomePage() {
    const navigate = useNavigate()

    const submit = async (cardNumber) => {
    
        if (cardNumber.length < 16) {
            storage.set("errorMessage", "Debe ingresar los 16 numeros de la tarjeta");
            storage.set("navigateTo", "/home")
            navigate('/error')
            return;
        }
        
        try{

            var response = await axios.get(`${bankCardApi.baseURL}/Check/Number/${cardNumber}`);
            console.log(response)
            if(response.status === 401){
                storage.set("errorMessage", "La tarjeta se encuentra bloqueada. Comuniquese con su banco.");
                storage.set("navigateTo", "/home")
                navigate('/error')
                return;
            }else  if(response.status === 200){
                const bankcardId = response.data.result.id_bank_card;
                storage.set('bankCardId', bankcardId);
                storage.set('bankCardNumber', cardNumber);
                
                console.log(storage.get('bankCardId'));
            }
            navigate('/pin');
        }catch (error){
            console.log(error)
            storage.set("errorMessage", error.response.status === 404 ? "La tarjeta no existe en el sistema" : "La tarjeta se encuentra bloqueada. Comuniquese con su banco");
            storage.set("navigateTo", "/home")
            navigate('/error')
            return;
          }

    }

    return (
        <InputScreen title="Ingrese el numero de la tarjeta" inputMaxLength={16} format="0000-0000-0000-0000" handleSubmit={submit} ></InputScreen>
  );
}

export default HomePage;
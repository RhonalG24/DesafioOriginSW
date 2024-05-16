import React from 'react';
import { useNavigate } from 'react-router-dom';
import { storage } from '../utils/Storage';
import InputScreen from '../components/InputScreen';
import NavigateButton from '../components/NavigateButton';
import { bankCardApi } from '../api/bankCardApi';
import axios from 'axios';

function CardPinPage() {
    const navigate = useNavigate()

    const submit = async (pin) => {
        if (pin.length < 4) {
            storage.set("errorMessage", "Debe ingresar los 4 numeros del PIN");
            storage.set("navigateTo", "/pin")
            navigate('/error')
            return;
        }
        const body = {
            "id_bank_card": storage.get('bankCardId'),
            "pin": pin
        }
        try{

            
            var response = await axios.post(`${bankCardApi.baseURL}/Check/Pin/${pin}`, body );
            console.log(response)
            if(response.status === 401){
                storage.set("errorMessage", "La tarjeta se encuentra bloqueada. Comuniquese con su banco.");
                storage.set("navigateTo", "/pin")
                navigate('/error')
                return;
            }else  if(response.status === 200){
                storage.set('accountId', response.data.result.id_account);
                storage.set('cardExpiryDate', response.data.result.expiry_date);

                console.log(storage.get('bankCardId'));
            }
            navigate('/operations');
        }catch (error){
            console.log(error)
            storage.set("errorMessage", error.response.data.errorsMessage);
            storage.set("navigateTo", "/pin")
            navigate('/error')
            return;
        }

    }

    return (
        <div>

        <InputScreen title="Ingrese el PIN de la tarjeta" type="password" inputMaxLength={4} format="* * * *" handleSubmit={submit} ></InputScreen>
            <NavigateButton value={"/home"} text={"Salir"} color={"red"}></NavigateButton>
        </div>

    );
}

export default CardPinPage;
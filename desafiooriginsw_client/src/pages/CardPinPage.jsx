import React from 'react';
import { useNavigate } from 'react-router-dom';
import { storage } from '../utils/Storage';
import InputScreen from '../components/InputScreen';
import NavigateButton from '../components/NavigateButton';
import { bankCardApi } from '../api/bankCardApi';
import axios from 'axios';

/* Response 
{
  "statusCode": 200,
  "isSuccessful": true,
  "errorsMessage": null,
  "result": {
    "id_bank_card": 4,
    "id_account": 3,
    "number": "6245000010104040",
    "pin": "4242",
    "id_card_state": 1,
    "expiry_date": "2034-05-12",
    "failed_attempts": 0,
    "account": null,
    "card_state": null
  }
} */

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



    //useEffect(() => {
    //    document.getElementById("formatInputField").value = formatInputValue;
    //})

    //const handleInput = (e) => {
    //    console.log(e)
    //    var newFormatInputValue = bankCardFormat(e.target.value)
    //    console.log(newFormatInputValue)
    //    setFormatInputValue(newFormatInputValue);
    //};




    return (
        <div>

        <InputScreen title="Ingrese el PIN de la tarjeta" type="password" inputMaxLength={4} format="* * * *" handleSubmit={submit} ></InputScreen>
            <NavigateButton value={"/home"} text={"Salir"} color={"red"}></NavigateButton>
        </div>

    );
}

export default CardPinPage;
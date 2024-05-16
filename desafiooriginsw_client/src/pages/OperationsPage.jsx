import { useNavigate } from 'react-router-dom';

import ActionButton from '../components/ActionButton';
import NavigateButton from '../components/NavigateButton';
import { storage } from '../utils/Storage';
import { OperationsApi } from '../api/operationsApi';
import axios from 'axios';

function OperationsPage() {
  const navigate = useNavigate()

    const submit = async() => {
      const bankCardId = storage.get('bankCardId');
      try{
        console.log(bankCardId)
          var response = await axios.get(`${OperationsApi.baseURL}/balance/${bankCardId}` );

          if(response.status === 200){
              storage.set('balance', response.data.result.balance);
              storage.set('operationId', 1);
      
              console.log(storage.get('balance'));
              console.log(storage.get('bankCardId'));
              navigate('/balance');
          }
        }catch (error){
          console.log(error)
          storage.set("errorMessage", error.response.data.errorsMessage ? error.response.data.errorsMessage : "Hubo un error" );
          storage.set("navigateTo", "/operations")
          navigate('/error')
          return;
      }

    }

    return (

    <div className="justify-center w-full">
        <h1>¿Cuál operación desea realizar?</h1>
         <div>
            <ActionButton value={"/balance"} text={"Consultar saldo"} whenClick={submit}></ActionButton>
          </div>
          <div>
            <NavigateButton value={"/withdraw"} text={"Retiro"}></NavigateButton>
          </div>
          <div>
                <NavigateButton value={"/home"} text={"Salir"} color={"red"}></NavigateButton>

          </div>
   
    </div>
  );
}

export default OperationsPage;
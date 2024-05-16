import { useState } from 'react';
import ActionButton from '../components/ActionButton';
import NumericButton from '../components/NumericButton';
import { bankCardFormat } from '../utils/Utils';

function InputScreen({ title, type="text", inputMaxLength, format, handleSubmit }) {

    const [inputValue, setInputValue] = useState("");
    const [formatInputValue, setFormatInputValue] = useState("");

    const addValue = (e) => {
        if (inputValue.length >= inputMaxLength) {
            return;
        }
        let newValue = `${inputValue}${e}`;

        var newFormatInputValue = bankCardFormat(newValue);
        setFormatInputValue(newFormatInputValue);

        setInputValue(newValue);

    }

    const clean = () => {
        setInputValue('');
        document.getElementById("formatInputField").value = "";
    }

    const submit = (e) => {
        e.preventDefault()
        handleSubmit(inputValue)
    }
  return (
      <div className='max-w-xl mx-auto'>
          <h1 className="text-black-700 font-semibold">{title}</h1>
          <input id="formatInputField" name="formatInputField" type={ type }
              value={formatInputValue} placeholder={format} disabled required minLength={inputMaxLength}
              className="h-auto border rounded-md p-2 my-5"></input>

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
      </div>
  );
}

export default InputScreen;
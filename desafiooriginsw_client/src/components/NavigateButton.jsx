import React from 'react';
import { useNavigate } from 'react-router-dom';

function NavigateButton({ value, text, color }) {
    const navigate = useNavigate();

    const defaultButtonClassName = `bg-white-500 hover:bg-blue-500 text-blue-700 font-semibold hover:text-white py-2 px-4 border border-blue-500 hover:border-transparent rounded m-2`;

    var buttonClassName = null;
    if (color) {
        if (color == 'yellow') {
            buttonClassName = `bg-white-500 hover:bg-yellow-500 text-yellow-700 font-semibold hover:text-white py-2 px-4 border border-yellow-500 hover:border-transparent rounded m-2`;

        }
        if (color == 'red') {
            buttonClassName = `bg-white-500 hover:bg-red-500 text-red-700 font-semibold hover:text-white py-2 px-4 border border-red-500 hover:border-transparent rounded m-2`;

        }
        if (color == 'green') {
            buttonClassName = `bg-white-500 hover:bg-green-500 text-green-700 font-semibold hover:text-white py-2 px-4 border border-green-500 hover:border-transparent rounded m-2`;

        }
        if (color == 'blue') {
            buttonClassName = `bg-white-500 hover:bg-blue-500 text-blue-700 font-semibold hover:text-white py-2 px-4 border border-blue-500 hover:border-transparent rounded m-2`;

        } 
    }

    const handleClick = () => {
        navigate(value)
    }
    return (

        <button className={buttonClassName ? buttonClassName : defaultButtonClassName}
          value="{value}" onClick={handleClick}>
          {text}
      </button>
  );
}

export default NavigateButton;
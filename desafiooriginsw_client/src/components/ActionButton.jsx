import React from 'react';

function ActionButton({ value, text, whenClick, bgColor }) {
    const handleClick = () => {
        whenClick()
    }
  return (
      <button className="bg-{bgColor}-500 hover:bg-blue-500 text-blue-700 font-semibold hover:text-white py-2 px-4 border border-blue-500 hover:border-transparent rounded m-2"
          value="{value}" onClick={handleClick}>
          {text}
      </button>
  );
}

export default ActionButton;
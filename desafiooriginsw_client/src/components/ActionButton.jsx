import React from 'react';


function ActionButton({ value, text, whenClick, color }) {
    const defaultButtonClassName = `bg-white-500 hover:bg-blue-500 text-blue-700 font-semibold hover:text-white py-2 px-4 border border-blue-500 hover:border-transparent rounded m-2`;

    var buttonClassName = null;
    if (color) {
        if (color == 'yellow') {
            buttonClassName = `bg-white-500 hover:bg-yellow-500 text-yellow-700 font-semibold hover:text-white py-2 px-4 border border-yellow-500 hover:border-transparent rounded m-2`;
        }
    }
    const handleClick = () => {
        whenClick()
    }
  return (
      <button className={ buttonClassName ? buttonClassName : defaultButtonClassName }
          value={value} onClick={handleClick}>
          {text}
      </button>
  );
}

export default ActionButton;
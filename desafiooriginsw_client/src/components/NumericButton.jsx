import React from 'react';

const NumericButton = ({ number, addValue }) => {

    const handleClick = () => {
        addValue(number)
    }
  return (
      <button className={"bg-white-500 hover:bg-blue-500 text-blue-700 font-semibold hover:text-white py-2 px-4 border border-blue-500 hover:border-transparent rounded m-2"}
          value={"number"} onClick={handleClick}>
          {number}
      </button>
  );
}

export default NumericButton;
import React from 'react';
import NavigateButton from '../components/NavigateButton';
import { storage } from '../utils/Storage';


function ErrorPage() {
    var errorMessage = storage.get('errorMessage');
    var navigateTo = storage.get('navigateTo');
    errorMessage = Array.isArray(errorMessage) ? errorMessage.join('. ') : errorMessage;
    return (
        <div className='max-w-xl mx-auto'>
            <p className="text-red">Error: {errorMessage}</p>
            <NavigateButton value={navigateTo} text={"Atras"}></NavigateButton>

        </div>
  );
}

export default ErrorPage;
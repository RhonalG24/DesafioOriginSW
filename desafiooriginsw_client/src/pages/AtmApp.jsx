import { BrowserRouter, Routes, Route } from 'react-router-dom'
import React from 'react';
import HomePage from './HomePage';
import { MatchAllRoute } from './MatchAllRoutePage';
import  BalancePage  from './BalancePage';
import CardPinPage from './CardPinPage';
import ErrorPage from './ErrorPage';
import WithdrawPage from './WithdrawPage'
import OperationsPage from './OperationsPage';
import OperationReportPage from './OperationReportPage'


function AtmApp() {
    
    return (
        <div>
            <div className="min-h-full max-h-full h-screen flex justify-center mt-1 py-2 px-2 sm:px-6 lg:px-8">
                <div className="max-w-full w-full space-y-8 border border-blue-800 p-4">
                    <BrowserRouter>
                        <Routes>
                            <Route path='/' element={<HomePage />} />
                            <Route path='/home' element={<HomePage />} />
                            <Route path='/pin' element={<CardPinPage />} />
                            <Route path='/operations' element={<OperationsPage />} />
                            <Route path='/balance' element={<BalancePage />} />
                            <Route path='/withdraw' element={<WithdrawPage />} />
                            <Route path='/operationReport' element={<OperationReportPage />} />
                            <Route path='/error' element={<ErrorPage />} />
                            <Route path="*" element={<MatchAllRoute />} />
                        </Routes>
                    </BrowserRouter>
                </div>
            </div>
        </div>
    );
}

export default AtmApp;
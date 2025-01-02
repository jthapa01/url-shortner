import React from 'react';
import { useMsal, useIsAuthenticated } from "@azure/msal-react";
import Home from '../Home/Home';

function Login() {
    const loginRequest = {
        scopes: ["User.Read"],
    }
    const { instance } = useMsal();
    const isAuthenticated = useIsAuthenticated();

    const onLogin = async () => {
        await instance.handleRedirectPromise();
        const accounts = instance.getAllAccounts();
        if(accounts.length === 0) {
            await instance.loginRedirect(loginRequest);
        }
    };

    if(!isAuthenticated) {
        return (
            <header className='App-header'>
                <p>Welcome to the Url Shortener</p>
                <button className='login-button' onClick={onLogin}>Login</button>
            </header>
        );
    }
    return(
        <Home />
    );
}

export default Login;
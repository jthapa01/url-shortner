import "./App.css";
import { PublicClientApplication } from "@azure/msal-browser";
import { MsalProvider } from "@azure/msal-react";
import Login from "./Login/Login";

const msalConfig = {
  auth: {
    clientId: process.env.REACT_APP_CLIENT_ID,
    authority: process.env.REACT_APP_AUTHORITY,
    redirectUri:
      process.env.NODE_ENV === "production"
        ? window.location.origin
        : "http://localhost:3000",
  },
};

const msalInstance = new PublicClientApplication(msalConfig);

function App() {
  return (
    <MsalProvider instance={msalInstance}>
      <div className="App">
        <Login />
      </div>
    </MsalProvider>
  );
}

export default App;

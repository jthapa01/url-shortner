import "./LogoutButton.css";
import React from "react";

function LogoutButton({ onLogout }) {
  return (
    <button className="logout" onClick={onLogout}>
      Logout
    </button>
  );
}

export default LogoutButton;
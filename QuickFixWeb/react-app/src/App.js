import './App.css';
import React from 'react';
import ShowMyFixes from './components/showMyFixes';
import logo from './logo.svg';

function App({author}) {
  return (
    <div className="App">
       <img src={logo} className="App-logo" alt="logo" />
      <ShowMyFixes author={author}></ShowMyFixes>
    </div>
  );
}

export default App;

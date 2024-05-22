// src/index.js
import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';

const rootElement = document.getElementById('react-app');
const author = rootElement.getAttribute('data-author');
console.log("Author:", author);
ReactDOM.render(<App author={author}/>, document.getElementById('react-app'));

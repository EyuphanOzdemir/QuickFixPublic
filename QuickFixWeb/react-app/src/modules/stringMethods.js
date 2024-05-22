import React from 'react';
export var stringMethods = (function () {
  const deQuote = (str) => str.replace("'", "");
  const deDoubleQuote = (str) => str.replace('"', "");
  const makeStrong = (text) => {
    return (
      <div className="gridHeader">
        <strong>{text}</strong>
      </div>
    );
  };

  return {
    deQuote,
    deDoubleQuote,
    makeStrong,
  };
})();

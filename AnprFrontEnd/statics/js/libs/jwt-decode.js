/**
 * Minified by jsDelivr using Terser v5.19.2.
 * Original file: /npm/jwt-decode@4.0.0/build/cjs/index.js
 *
 * Do NOT use SRI with dynamically generated files! More information: https://www.jsdelivr.com/using-sri-with-dynamic-files
 */
exports = {};
"use strict";Object.defineProperty(exports,"__esModule",{value:!0}),exports.jwtDecode=exports.InvalidTokenError=void 0;class InvalidTokenError extends Error{}function b64DecodeUnicode(e){return decodeURIComponent(atob(e).replace(/(.)/g,((e,r)=>{let o=r.charCodeAt(0).toString(16).toUpperCase();return o.length<2&&(o="0"+o),"%"+o})))}function base64UrlDecode(e){let r=e.replace(/-/g,"+").replace(/_/g,"/");switch(r.length%4){case 0:break;case 2:r+="==";break;case 3:r+="=";break;default:throw new Error("base64 string is not of the correct length")}try{return b64DecodeUnicode(r)}catch(e){return atob(r)}}function jwtDecode(e,r){if("string"!=typeof e)throw new InvalidTokenError("Invalid token specified: must be a string");r||(r={});const o=!0===r.header?0:1,t=e.split(".")[o];if("string"!=typeof t)throw new InvalidTokenError(`Invalid token specified: missing part #${o+1}`);let n;try{n=base64UrlDecode(t)}catch(e){throw new InvalidTokenError(`Invalid token specified: invalid base64 for part #${o+1} (${e.message})`)}try{return JSON.parse(n)}catch(e){throw new InvalidTokenError(`Invalid token specified: invalid json for part #${o+1} (${e.message})`)}}exports.InvalidTokenError=InvalidTokenError,InvalidTokenError.prototype.name="InvalidTokenError",exports.jwtDecode=jwtDecode;
//# sourceMappingURL=/sm/6c7febb4bfafca4afa7657d8babcb2b1f8bbbcaed2aa30fda5d0b1a40d9b7cd2.map

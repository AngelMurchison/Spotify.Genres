// const credentialRequest = {
//   grant_type: "client_credentials",
//   Authorization: btoa("f28cc0ca6f1e4faea933e93c1f389749:26ac4ea4b1dd413a8321cc0d32fd7249")
// };

// var xhr = new XMLHttpRequest();
// this.token = {};

// xhr.open("GET", `https://localhost:5001/spotify/token`, true);
// xhr.setRequestHeader('Content-Type', 'application/json');
// xhr.send();

// xhr.onreadystatechange = function() { // Call a function when the state changes.
//   if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
//     debugger;
//     this.token = JSON.parse(xhr.response);
//     window.token = this.token.access_token;
//   }
// }
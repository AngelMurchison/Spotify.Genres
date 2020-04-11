var xhr = new XMLHttpRequest();
this.token = {};

xhr.open("GET", `https://localhost:5001/spotify/token`, true);
xhr.setRequestHeader('Content-Type', 'application/json');
xhr.send();

xhr.onreadystatechange = function() { // Call a function when the state changes.
  if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
    
    this.token = xhr.response;
    window.token = xhr.response;
  }
}
window.addEventListener('hashchange', (event) => {
    document.querySelector("#clientApp").innerHTML = event.newURL;
});

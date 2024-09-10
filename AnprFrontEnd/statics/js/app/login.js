import { updateToken } from './localStorage.js';
import { hide, show } from './utils.js';

export const init = (evt, globalSettings) => {
    initLogin(evt, globalSettings);
};

const initLogin = (evt, globalSettings) => {
    const form = evt.target.querySelector('form');
    const submitButton = form.querySelector('button[type="submit"]');
    const spinner = form.querySelector('.spinner-border');
    form.addEventListener('submit', (event) => {
        event.preventDefault();
        hide(submitButton);
        show(spinner);
        const formData = new FormData(form);

        const username = formData.get('username');
        const password = formData.get('password');
        const webapi_url = globalSettings.webapi_url;

        fetch(`${webapi_url}/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password }),
        }).then(response => {
            if (response.ok) {
                return response.json();
            }
            throw new Error('Network response was not ok.');
        }).then(data => {
            handleLogin(data);
            // window.location.reload();
        }).catch((error) => {
            console.error('Error:', error);
        }).finally(() => {
            hide(spinner);
            show(submitButton);
        });
    });
};

const handleLogin = (data) => {
    console.log(data)

    if (data.httpResponseCode === 401) {
        alert('Inloggen mislukt, probeer opnieuw');
    } else if (data.httpResponseCode === 200) {
        updateToken(data.accessToken.token);
        window.location.reload();
    }
};

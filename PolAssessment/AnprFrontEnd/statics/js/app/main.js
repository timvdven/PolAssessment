import { getOrPrepLocalstorage } from './localStorage.js';
import { htmx_error, htmx_beforeSwap, htmx_afterSwap } from './htmx.js';

const globalSettings = {};

((globalSettings) => {
    globalSettings.htmx_error = [];
    globalSettings.htxm_afterSwap = [];

    document.body.addEventListener("htmx:sendError", (evt) => htmx_error(evt, globalSettings));
    document.body.addEventListener("htmx:responseError", (evt) => htmx_error(evt, globalSettings));
    document.body.addEventListener("htmx:beforeSwap", (evt) => htmx_beforeSwap(evt));
    document.body.addEventListener("htmx:afterSwap", (evt) => htmx_afterSwap(evt, globalSettings, () => {
        const requestPath = evt.detail.pathInfo.finalRequestPath.split('?')[0];
        const switcher = trimSlashes(requestPath);
        switch (switcher)
        {
            case 'register_phone': xxxx.zzzz(evt); break;

            default: console.log('Dont know what to do?', evt); break;
        }
    }));

    const trimSlashes = (str) => {
        return str.replace(/^\/+|\/+$/g, '');
    }

    htmx.onLoad(function(evt) {
        // console.log('onLoad', evt);
    });

    const pol_app = getOrPrepLocalstorage();
    
    const module = pol_app.token ? 'dashboard' : 'login';
    const app = document.querySelector('#app');
    app.innerHTML += '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>';
    app.setAttribute('hx-get', `app/${module}`);
    htmx.trigger('#app', 'load');

})(globalSettings);

import { getOrPrepLocalstorage } from './localStorage.js';
import { htmx_error, htmx_beforeSwap, htmx_afterSwap } from './htmx.js';
import * as login from './login.js';
import * as dashboard from './dashboard.js';
import * as map from './map.js';

const globalSettings = { webapi_url, anpr:[], token: null, anprHash: null, anprMap: null, anprLastFetchDate: null };

((globalSettings) => {
    globalSettings.htmx_error = [];
    globalSettings.htxm_afterSwap = [];
    const pol_app = getOrPrepLocalstorage();
    globalSettings.token = pol_app.token;

    document.body.addEventListener("htmx:sendError", (evt) => htmx_error(evt, globalSettings));
    document.body.addEventListener("htmx:responseError", (evt) => htmx_error(evt, globalSettings));
    document.body.addEventListener("htmx:beforeSwap", (evt) => htmx_beforeSwap(evt));
    document.body.addEventListener("htmx:afterSwap", (evt) => htmx_afterSwap(evt, globalSettings, () => {
        const requestPath = evt.detail.pathInfo.finalRequestPath.split('?')[0];
        const switcher = trimSlashes(requestPath);
        switch (switcher)
        {
            case 'app/login': login.init(evt, globalSettings); break;
            case 'app/dashboard': dashboard.init(evt, globalSettings); break;
            case 'app/map': map.init(evt, globalSettings); break;

            default: console.warn('Dont know what to do?', evt); break;
        }
    }));

    const trimSlashes = (str) => {
        return str.replace(/^\/+|\/+$/g, '');
    }

    htmx.onLoad(function(evt) {
        // console.log('onLoad', evt);
    });
    
    const module = globalSettings.token ? 'dashboard' : 'login';
    const app = document.querySelector('#app');
    app.innerHTML += '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>';
    htmx.ajax('GET', `/app/${module}/`, '#app');

})(globalSettings);

export const htmx_error = (evt, globalSettings) => {
    globalSettings.htmx_error.push(evt);
    evt.target.innerHTML = "<p>Er is een fout opgetreden bij het ophalen van de gegevens. Probeer het later nogmaals.</p>";
};

export const htmx_beforeSwap = (evt) => {
    evt.target.classList.add('pause');
    evt.target.classList.add('fadeIn');
};

export const htmx_afterSwap = (evt, globalSettings, func) => {
    globalSettings.htxm_afterSwap.push(evt);
    func();
};

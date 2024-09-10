const getBaseLocalstorageObject = () => {
    return {
        'token': null,
        'name': null,
    };
};

export const getOrPrepLocalstorage = () => {
    const pol_app = localStorage.getItem('polAssessment_app') || JSON.stringify(getBaseLocalstorageObject());
    localStorage.setItem('polAssessment_app', pol_app);
    return JSON.parse(pol_app);
};

export const updateToken = (token) => {
    const pol_app = getOrPrepLocalstorage();
    pol_app.token = token;
    localStorage.setItem('polAssessment_app', JSON.stringify(pol_app));
};

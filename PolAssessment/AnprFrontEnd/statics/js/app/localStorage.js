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

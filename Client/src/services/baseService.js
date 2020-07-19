import axios from 'axios'
import NProgress from 'nprogress'

function getInstance(instanceConfig) {
    if (!instanceConfig || !instanceConfig.baseURL) {
        throw new Error('Please provide a valid config with at last baseURL parameter.');
    }

    const instance = axios.create({
        baseURL: instanceConfig.baseURL
    });

    instance.interceptors.request.use(function (config) {
        NProgress.start();

        const token = localStorage.getItem('token');

        if (token) {
            config.headers.Authorization =  `Bearer ${token}`;
        } else {
            delete config.headers.common['Authorization'];
        }
    
        return config;
    });

    // before a response is returned stop nprogress
    instance.interceptors.response.use(response => {
        NProgress.done();
        
        return response;
    });

    return instance;
}

export default {
    getInstance
}


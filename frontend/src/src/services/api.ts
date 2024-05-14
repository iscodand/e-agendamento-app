import axios from "axios";

const instance = axios.create({
    baseURL: "http://localhost:5008/v1"
});

export default instance;
import axios from "axios";

const instance = axios.create({
    baseURL: "https://localhost:7295/api/v1"
});

export default instance;
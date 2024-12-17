import axios, { AxiosResponse } from "axios";
import { RouteParams } from "./RouteParams";

export const apiClient = axios.create({
  headers: {
    'X-Timezone-Offset': new Date().getTimezoneOffset(),
  },
  paramsSerializer: RouteParams.generateRouteParams,
});

const { get, post, put, delete: destroy } = apiClient;
export { get, post, put, destroy };

import type { IJwtResponse } from "@/types/IJwtResponse";
import type { ISleep } from "@/types/ISleep";
import type { ISleepCreate } from "@/types/ISleepCreate";
import axios from "axios";
import type { IResultObject } from "./IResultObject";


export default class SleepService {
    private constructor() {

    }

    private static httpClient = axios.create({
        baseURL: import.meta.env.VITE_API_BASE_URL + 'Sleep/',
        validateStatus: () => true
    });


    static async getSleepTimes(jwt: IJwtResponse): Promise<IResultObject<ISleep[]>> {
        try {
            const response = await SleepService.httpClient.get<ISleep[]>("GetSleepTimes", {
                headers: {
                    "Authorization": "Bearer " + jwt.jwt
                }
            });

            if (response.status == 200) {
                return {
                    status: response.status,
                    data: response.data
                }
            }
            return {
                status: response.status,
                errors: [response.status.toString() + " " + response.statusText]
            }
        } catch (error: any) {
            return {
                status: error.response.status,
                errors: [JSON.stringify(error)]
            };

        };
    };

    static async getSleep(jwt: IJwtResponse, id: string): Promise<IResultObject<ISleep>> {
        try {
            const url = "GetSleep/" + id;
            const response = await SleepService.httpClient.get<ISleep>(url, {
                headers: {
                    "Authorization": "Bearer " + jwt.jwt
                }
            });

            if (response.status == 200) {
                return {
                    status: response.status,
                    data: response.data
                }
            }
            return {
                status: response.status,
                errors: [response.status.toString() + " " + response.statusText]
            }
        } catch (error: any) {
            return {
                status: error.response.status,
                errors: [JSON.stringify(error)]
            };

        };
    };

    static async create(jwt: IJwtResponse, sleep: ISleepCreate): Promise<IResultObject<ISleep>> {
        try {
            const response = await SleepService.httpClient.post<ISleep>("Create", sleep, {
                headers: {
                    "Authorization": "Bearer " + jwt.jwt
                }
            });

            if (response.status == 201) {
                return {
                    status: response.status,
                    data: response.data
                };
            }
            return {
                status: response.status,
                errors: [response.status.toString() + " " + response.statusText]
            }
        } catch (error: any) {
            return {
                status: error.response.status,
                errors: [JSON.stringify(error)]
            };

        };
    };

    static async edit(jwt: IJwtResponse, sleep: ISleep): Promise<IResultObject<ISleep>> {
        const url = "Edit/" + sleep.id;
        try {
            const response = await SleepService.httpClient.put<ISleep>(url, sleep, {
                headers: {
                    "Authorization": "Bearer " + jwt.jwt
                }
            });

            if (response.status == 204) {
                return { status: response.status };
            }
            return {
                status: response.status,
                errors: [response.status.toString() + " " + response.statusText]
            }
        } catch (error: any) {
            return {
                status: error.response.status,
                errors: [JSON.stringify(error)]
            };

        };
    };

    static async deleteSleep(jwt: IJwtResponse, id: string): Promise<IResultObject<ISleep>> {
        const url = "Delete/" + id;
        try {
            const response = await SleepService.httpClient.delete(url, {
                headers: {
                    "Authorization": "Bearer " + jwt.jwt
                }
            });

            if (response.status == 204) {
                return { status: response.status };
            }
            return {
                status: response.status,
                errors: [response.status.toString() + " " + response.statusText]
            }
        } catch (error: any) {
            return {
                status: error.response.status,
                errors: [JSON.stringify(error)]
            };

        };
    };
}
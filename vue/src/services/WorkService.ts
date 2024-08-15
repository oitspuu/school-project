import axios from "axios";
import type { IResultObject } from "./IResultObject";
import type { IJwtResponse } from "@/types/IJwtResponse";
import type { IWork } from "@/types/IWork";
import type { IWorkCreate } from "@/types/IWorkCreate";

export default class WorkService {
    private constructor() {

    }

    private static httpClient = axios.create({
        baseURL: import.meta.env.VITE_API_BASE_URL + 'Work/',
        validateStatus: () => true
    });


    static async getWorkplaces(jwt: IJwtResponse): Promise<IResultObject<IWork[]>> {
        try {
            const response = await WorkService.httpClient.get<IWork[]>("GetWorkplaces", {
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

    static async getWork(jwt: IJwtResponse, id: string): Promise<IResultObject<IWork>> {
        try {
            const url = "GetWork/" + id;
            const response = await WorkService.httpClient.get<IWork>(url, {
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

    static async create(jwt: IJwtResponse, work: IWorkCreate): Promise<IResultObject<IWork>> {
        try {
            const response = await WorkService.httpClient.post<IWork>("Create", work, {
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

    static async edit(jwt: IJwtResponse, work: IWork): Promise<IResultObject<IWork>> {
        const url = "Edit/" + work.id;
        try {
            const response = await WorkService.httpClient.put<IWork>(url, work, {
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

    static async deleteWork(jwt: IJwtResponse, id: string): Promise<IResultObject<IWork>> {
        const url = "Delete/" + id;
        try {
            const response = await WorkService.httpClient.delete(url, {
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
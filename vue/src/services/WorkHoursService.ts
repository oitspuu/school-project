import type { IJwtResponse } from "@/types/IJwtResponse";
import type { IWorkHours } from "@/types/IWorkHours";
import type { IWorkHoursCreate } from "@/types/IWorkHoursCreate";
import axios from "axios";
import type { IResultObject } from "./IResultObject";


export default class WorkHoursService {
    private constructor() {

    }

    private static httpClient = axios.create({
        baseURL: import.meta.env.VITE_API_BASE_URL + 'WorkHours/',
        validateStatus: () => true
    });


    static async getWorkhours(jwt: IJwtResponse, id: string): Promise<IResultObject<IWorkHours[]>> {
        try {
            const url = "GetWorkHours/" + id;
            const response = await WorkHoursService.httpClient.get<IWorkHours[]>(url, {
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

    static async getWorkhour(jwt: IJwtResponse, id: string): Promise<IResultObject<IWorkHours>> {
        try {
            const url = "GetWorkHour/" + id;
            const response = await WorkHoursService.httpClient.get<IWorkHours>(url, {
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

    static async create(jwt: IJwtResponse, work: IWorkHoursCreate): Promise<IResultObject<IWorkHours>> {
        try {
            const response = await WorkHoursService.httpClient.post<IWorkHours>("Create", work, {
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

    static async edit(jwt: IJwtResponse, work: IWorkHours): Promise<IResultObject<IWorkHours>> {
        const url = "Edit/" + work.id;
        try {
            const response = await WorkHoursService.httpClient.put<IWorkHours>(url, work, {
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

    static async deleteWork(jwt: IJwtResponse, id: string): Promise<IResultObject<IWorkHours>> {
        const url = "Delete/" + id;
        try {
            const response = await WorkHoursService.httpClient.delete(url, {
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
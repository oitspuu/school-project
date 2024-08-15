import type { IAddTime } from "@/types/IAddTime";
import type { IHobby } from "@/types/IHobby";
import type { IHobbyCreate } from "@/types/IHobbyCreate";
import type { IJwtResponse } from "@/types/IJwtResponse";
import axios from "axios";
import type { IResultObject } from "./IResultObject";


export default class HobbyService {
    private constructor() {

    }

    private static httpClient = axios.create({
        baseURL: import.meta.env.VITE_API_BASE_URL + 'Hobby/',
        validateStatus: () => true
    });


    static async getHobbies(jwt: IJwtResponse): Promise<IResultObject<IHobby[]>> {
        try {
            const response = await HobbyService.httpClient.get<IHobby[]>("GetHobbies", {
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

    static async getHobby(jwt: IJwtResponse, id: string): Promise<IResultObject<IHobby>> {
        try {
            const url = "GetHobby/" + id;
            const response = await HobbyService.httpClient.get<IHobby>(url, {
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

    static async addTime(jwt: IJwtResponse, id: string, time: IAddTime): Promise<IResultObject<IAddTime>> {
        try {
            const url = "AddTime/" + id;
            const response = await HobbyService.httpClient.patch<IAddTime>(url, time, {
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

    static async create(jwt: IJwtResponse, hobby: IHobbyCreate): Promise<IResultObject<IHobby>> {
        try {
            const response = await HobbyService.httpClient.post<IHobby>("Create", hobby, {
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

    static async edit(jwt: IJwtResponse, hobby: IHobby): Promise<IResultObject<IHobby>> {
        const url = "Edit/" + hobby.id;
        try {
            const response = await HobbyService.httpClient.put<IHobby>(url, hobby, {
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

    static async deleteHobby(jwt: IJwtResponse, id: string): Promise<IResultObject<IHobby>> {
        const url = "Delete/" + id;
        try {
            const response = await HobbyService.httpClient.delete(url, {
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
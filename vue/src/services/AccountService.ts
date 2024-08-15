import type { IDeletedCount } from "@/types/IDeletedCount";
import type { IJwtResponse } from "@/types/IJwtResponse";
import type { ILoginInfo } from "@/types/ILoginInfo";
import type { ILogout } from "@/types/ILogout";
import type { IResultObject } from "./IResultObject";
import axios from 'axios';


export default class AccountService {
    private constructor() {

    }

    private static httpClient = axios.create({
        baseURL: import.meta.env.VITE_API_BASE_URL + 'identity/Account/',
        validateStatus: () => true
    });

    static async login(email: string, pwd: string): Promise<IResultObject<IJwtResponse>> {
        const loginData: ILoginInfo = {
            email: email,
            password: pwd
        }
        try {
            const response = await AccountService.httpClient.post<IJwtResponse>("Login", loginData);
            if (response.status < 300) {
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
        }
    }

    static async register(email: string, pwd: string): Promise<IResultObject<IJwtResponse>> {
        const loginData: ILoginInfo = {
            email: email,
            password: pwd
        }
        try {
            const response = await AccountService.httpClient.post<IJwtResponse>("Register", loginData);
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
        }
    }


    static async refresh(tokenInfo: IJwtResponse): Promise<IResultObject<IJwtResponse>> {
        try {
            const response = await AccountService.httpClient.post<IJwtResponse>("RefreshTokenData", tokenInfo);
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
                errors: [JSON.stringify(error)]
            };
        }
    }

    static async logout(refreshToken: ILogout): Promise<IResultObject<IDeletedCount>> {
        try {
            const response = await AccountService.httpClient.post<IDeletedCount>("Logout", refreshToken);
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
                errors: [JSON.stringify(error)]
            };
        }
    }

}
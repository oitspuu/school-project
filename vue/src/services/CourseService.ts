import type { IAddTime } from "@/types/IAddTime";
import type { ICourse } from "@/types/ICourse";
import type { ICourseBasic } from "@/types/ICourseBasic";
import type { ICourseCreate } from "@/types/ICourseCreate";
import type { IJwtResponse } from "@/types/IJwtResponse";
import axios from "axios";
import type { IResultObject } from "./IResultObject";


export default class CourseService {
    private constructor() {

    }

    private static httpClient = axios.create({
        baseURL: import.meta.env.VITE_API_BASE_URL + 'Course/',
        validateStatus: () => true
    });


    static async getCourses(jwt: IJwtResponse): Promise<IResultObject<ICourse[]>> {
        try {
            const response = await CourseService.httpClient.get<ICourse[]>("GetCourses", {
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

    static async getCoursesBasic(jwt: IJwtResponse): Promise<IResultObject<ICourseBasic[]>> {
        try {
            const response = await CourseService.httpClient.get<ICourseBasic[]>("GetCoursesBasic", {
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

    static async getCourse(jwt: IJwtResponse, id: string): Promise<IResultObject<ICourse>> {
        try {
            const url = "GetCourse/" + id;
            const response = await CourseService.httpClient.get<ICourse>(url, {
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
            const response = await CourseService.httpClient.patch<IAddTime>(url, time, {
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

    static async create(jwt: IJwtResponse, course: ICourseCreate): Promise<IResultObject<ICourse>> {
        try {
            const response = await CourseService.httpClient.post<ICourse>("Create", course, {
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

    static async edit(jwt: IJwtResponse, course: ICourse): Promise<IResultObject<ICourse>> {
        const url = "Edit/" + course.id;
        try {
            const response = await CourseService.httpClient.put<ICourse>(url, course, {
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

    static async deleteCourse(jwt: IJwtResponse, id: string): Promise<IResultObject<ICourse>> {
        const url = "Delete/" + id;
        try {
            const response = await CourseService.httpClient.delete(url, {
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
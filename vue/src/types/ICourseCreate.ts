export interface ICourseCreate {
    "homeworkTime": string,
    "courseName": string,
    "language": string | null,
    "schoolName": string | null,
    "ects": number,
    "teacher": string | null,
    "startDate": string,
    "endDate": string
}
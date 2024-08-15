export interface ICourse {
    "id": string,
    "courseId": string,
    "homeworkTime": string,
    "courseName": string,
    "language": string | null,
    "schoolId": string | null,
    "schoolName": string | null,
    "ects": number,
    "teacher": string | null,
    "startDate": string,
    "endDate": string
}
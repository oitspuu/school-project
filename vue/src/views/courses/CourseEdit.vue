<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
import AccountService from '@/services/AccountService'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import type { ICourse } from '@/types/ICourse'
import CourseService from '@/services/CourseService'
import SpinnerComponent from '@/components/SpinnerComponent.vue'

const props = defineProps({
  id: String
})

const authStore = useAuthStore()
const router = useRouter()
let validationError = ref<string>('')
let course = ref<ICourse>()
let isLoading = ref<boolean>(true)

const loadData = async () => {
  if (authStore.jwtInfo && props.id) {
    let response = await CourseService.getCourse(authStore.jwtInfo, props.id)
    if (response.data) {
      course.value = response.data
      isLoading.value = false
      return
    }
    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) return
    authStore.jwtInfo = refresh.data
    response = await CourseService.getCourse(refresh.data, props.id)
    if (response.data) {
      course.value = response.data
      isLoading.value = false
      return
    }
  }
}

onMounted(()  => {
    loadData();
})


const validateAndSave = async () => {
  if (!course.value) {
    validationError.value = 'no course to edit'
    return
  }

  const regexDate = /^\d{4}-\d{2}-\d{2}$/
  if (!regexDate.test(course.value.endDate) || !regexDate.test(course.value.startDate)) {
    validationError.value = 'wrong date format'
    return
  }

  const regexTime = /^(\d*.)?\d{2}:\d{2}:\d{2}$/
  if (!regexTime.test(course.value.homeworkTime)) {
    validationError.value = 'wrong time format'
    return
  }

  if (course!.value.courseName.trim() == '') {
    validationError.value = 'no coursename entered'
    return
  }

  if (authStore.jwtInfo) {
    let response = await CourseService.edit(authStore.jwtInfo, course.value)
    if (response.status == 204) {
      router.push({ name: 'coursedetails', params: { id: course.value.id } }).catch(() => {});
      return;
    }

    if (response.status == 401) {
      const refresh = await AccountService.refresh(authStore.jwtInfo)
      if (!refresh.data) {
        validationError.value = refresh.errors![0]
        return
      }
      authStore.jwtInfo = refresh.data
      response = await CourseService.edit(refresh.data, course.value)
      if (response.status == 204) {
        router.push({ name: 'coursedetails', params: { id: course.value.id } }).catch(() => {});
        return;
      }
    }
    if (response.errors && response.errors.length > 0) {
      validationError.value = response.errors[0]
    } else {
      validationError.value = 'unknown error'
    }
    return
  }
  validationError.value = 'Not authenticated'
}
</script>

<template>
  <div v-if="isLoading">
    <spinner-component></spinner-component>
  </div>
  <div v-else>
    <h1>Edit</h1>

<h4>Course</h4>
<hr />
<div class="row" v-if="course">
    <div class="col-md-4">
        <div>
            <div class="text-danger">{{validationError}}</div>
            <div class="form-group">
                <label htmlFor="courseName" class="control-label">Course name</label>
                <input id="courseName"
                    value=""
                    v-model="course.courseName"
                    class="form-control" />
            </div>
            <div class="form-group">
                <label htmlFor="homeworkTime" class="control-label">Homework time ((d.)hh:mm:ss)</label>
                <input id="homeworkTime"
                    value=""
                    v-model="course.homeworkTime"
                    class="form-control" />
            </div>
            <div class="form-group">
                <label htmlFor="schoolName" class="control-label">School name (optional)</label>
                <input id="schoolName"
                    value=""
                    v-model="course.schoolName"
                    class="form-control" />
            </div>
            <div class="form-group">
                <label htmlFor="teacher" class="control-label">Teacher (optional)</label>
                <input id="teacher"
                    value=""
                    v-model="course.teacher"
                    class="form-control" />
            </div>
            <div class="form-group">
                <label htmlFor="ects" class="control-label">ECTS</label>
                <input id="ects"
                    value=""
                    v-model="course.ects"
                    class="form-control" />
            </div>
            <div class="form-group">
                <label htmlFor="startDate" class="control-label">Start date (yyyy-mm-dd)</label>
                <input id="startDate"
                    value=""
                    v-model="course.startDate"
                    class="form-control" />
            </div>
            <div class="form-group">
                <label htmlFor="endDate" class="control-label">End date (yyyy-mm-dd)</label>
                <input id="endDate"
                    value=""
                    v-model="course.endDate"
                    class="form-control" />
            </div>
            <div class="form-group">
                <input type="submit"
                    @click="validateAndSave"
                    value="Save" class="btn btn-primary" />
            </div>
        </div>
    </div>
</div>

<div>
    <router-link to="/course">Back to List</router-link>
</div>
  </div>
</template>

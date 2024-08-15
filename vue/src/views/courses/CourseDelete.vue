<script setup lang="ts">
import AccountService from '@/services/AccountService'
import CourseService from '@/services/CourseService'
import { useAuthStore } from '@/stores/auth'
import type { ICourse } from '@/types/ICourse'
import { useRouter } from 'vue-router'
import SpinnerComponent from '@/components/SpinnerComponent.vue'
import { onMounted, ref } from 'vue'


const props = defineProps({
  id: String
})

const authStore = useAuthStore()
const router = useRouter()
let isLoading = ref<boolean>(true)
let course = ref<ICourse>()
let validationError = ref<string>('')

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

const deleteCourse = async () => {
  if (!authStore.jwtInfo) {
    validationError.value = " No authentication info, can't delete"
    return
  }
  if (!course.value) {
    validationError.value = ' No course to delete'
    return
  }
  const response = await CourseService.deleteCourse(authStore.jwtInfo, course.value.id)
  if (response.status == 204) {
    router.push('/course')
    return
  }
  if (response.status == 401) {
    const refresh = await AccountService.refresh(authStore.jwtInfo!)
    if (!refresh.data) {
      validationError.value = "Not authenticated, can't delete"
      return
    }
    authStore.jwtInfo = refresh.data
    const second = await CourseService.deleteCourse(authStore.jwtInfo, course.value!.id)
    if (second.status == 204) {
      router.push('/course')
      return
    }
  }

  if (response.errors && response.errors.length > 0) {
    validationError.value = response.errors[0]
  } else {
    validationError.value = 'unknown error'
  }
}
</script>

<template>
  <div v-if="isLoading">
    <spinner-component></spinner-component>
  </div>
  <div v-else>
    <h1>Delete</h1>

    <h3>Are you sure you want to delete this?</h3>
    <div>
      <h4>Course</h4>
      <hr />
      <div class="text-danger">{{ validationError }}</div>
      <dl class="row">
        <dt class="col-sm-2">Course name</dt>
        <dd class="col-sm-10">
          {{ course?.courseName }}
        </dd>
      </dl>
      <dl class="row">
        <dt class="col-sm-2">Homework time</dt>
        <dd class="col-sm-10">
          {{ course?.homeworkTime }}
        </dd>
      </dl>

      <div class="form-group">
        <input type="submit" @click="deleteCourse" value="Delete" class="btn btn-danger" />
      </div>

      <router-link to="/course">Back to List</router-link>
    </div>
  </div>
</template>

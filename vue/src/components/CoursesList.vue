<script setup lang="ts">
import AccountService from '@/services/AccountService'
import CourseService from '@/services/CourseService'
import { useAuthStore } from '@/stores/auth'
import type { ICourse } from '@/types/ICourse'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import SpinnerComponent from './SpinnerComponent.vue'

const authStore = useAuthStore()
const router = useRouter()

let isLoading = ref<boolean>(true)
let courses = ref<ICourse[]>([])

async function loadData() {
  if (authStore.jwtInfo) {
    let response = await CourseService.getCourses(authStore.jwtInfo)
    if (response.data) {
      courses.value = response.data
      isLoading.value = false
      return
    }
    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) {
      router.replace('/login')
      return
    }
    authStore.jwtInfo = refresh.data
    response = await CourseService.getCourses(refresh.data)
    if (response.data) {
      courses.value = response.data
      isLoading.value = false
      return
    }
  }
  router.replace('/login')
  return
}

onMounted(() => {
  loadData()
})
</script>

<template>
  <div class="h4 border-bottom border-5">Courses</div>
  <router-link to="/course/create" class="btn btn-secondary" title="create">Create new</router-link>

  <table class="table table-striped table-hover">
    <thead>
      <tr>
        <th>Course name</th>
        <th>ECTS</th>
        <th>Start date</th>
        <th>End date</th>
        <th>Homework time</th>
        <th>School name</th>
        <th>Teacher</th>
        <th></th>
      </tr>
    </thead>
    <tbody v-if="isLoading">
      <spinner-component></spinner-component>
    </tbody>
    <tbody v-else>
      <tr v-for="item in courses" :key="item.id">
        <td>
          {{ item.courseName }}
        </td>
        <td>
          {{ item.ects }}
        </td>
        <td>
          {{ item.startDate }}
        </td>
        <td>
          {{ item.endDate }}
        </td>
        <td>
          {{ item.homeworkTime }}
        </td>
        <td>
          {{ item.schoolName }}
        </td>
        <td>
          {{ item.teacher }}
        </td>
        <td>
          <router-link
            :to="{ name: 'coursedetails', params: { id: item.id } }"
            class="nav-link text-dark"
            title="details"
            >Details</router-link
          >
          <router-link
            :to="{ name: 'courseedit', params: { id: item.id } }"
            class="nav-link text-dark"
            title="edit"
            >Edit</router-link
          >
          <router-link
            :to="{ name: 'coursedelete', params: { id: item.id } }"
            class="nav-link text-dark"
            title="delete"
            >Delete</router-link
          >
        </td>
      </tr>
    </tbody>
  </table>
</template>

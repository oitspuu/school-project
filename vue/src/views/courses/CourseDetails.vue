<script setup lang="ts">
import AccountService from '@/services/AccountService'
import CourseService from '@/services/CourseService'
import { useAuthStore } from '@/stores/auth'
import type { ICourse } from '@/types/ICourse'
import { onMounted, ref } from 'vue'
import SpinnerComponent from '@/components/SpinnerComponent.vue'


const props = defineProps({
  id: String
})

const authStore = useAuthStore()
let isLoading = ref<boolean>(true)
let course = ref<ICourse>()

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
</script>

<template>
  <h1>Details</h1>
  <div v-if="isLoading">
    <spinner-component></spinner-component>
  </div>

  <div>
    <dl class="row">
      <dt class="col-sm-2">CourseName</dt>
      <dd class="col-sm-10">
        {{ course?.courseName }}
      </dd>
      <dt class="col-sm-2">HomeworkTime</dt>
      <dd class="col-sm-10">
        {{ course?.homeworkTime }}
      </dd>
      <dt class="col-sm-2">ECTS</dt>
      <dd class="col-sm-10">
        {{ course?.ects }}
      </dd>
      <dt class="col-sm-2">Teacher</dt>
      <dd class="col-sm-10">
        {{ course?.teacher }}
      </dd>
      <dt class="col-sm-2">School name</dt>
      <dd class="col-sm-10">
        {{ course?.schoolName }}
      </dd>
      <dt class="col-sm-2">Start date</dt>
      <dd class="col-sm-10">
        {{ course?.startDate }}
      </dd>
      <dt class="col-sm-2">End date</dt>
      <dd class="col-sm-10">
        {{ course?.endDate }}
      </dd>
    </dl>
  </div>
  <div>
    <router-link
      :to="{ name: 'courseedit', params: { id: course?.id } }"
      class="btn btn-primary"
      >Edit</router-link
    >
    <router-link to="/course" class="btn btn-primary">Back to List</router-link>
  </div>
</template>

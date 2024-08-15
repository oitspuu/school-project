<script setup lang="ts">
import AccountService from '@/services/AccountService'
import { useAuthStore } from '@/stores/auth'
import SpinnerComponent from './SpinnerComponent.vue'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import type { IWorkHours } from '@/types/IWorkHours'
import WorkHoursService from '@/services/WorkHoursService'

const authStore = useAuthStore()
const router = useRouter()

let isLoading = ref<boolean>(true)
let workhours = ref<IWorkHours[]>([])

const loadData = async () => {
  if (authStore.jwtInfo && authStore.workId) {
    let response = await WorkHoursService.getWorkhours(authStore.jwtInfo, authStore.workId)
    if (response.data) {
      workhours.value = response.data
      isLoading.value = false
      return
    }

    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) {
      router.replace('/login')
      return
    }

    authStore.jwtInfo = refresh.data
    response = await WorkHoursService.getWorkhours(refresh.data, authStore.workId)
    if (response.data) {
      workhours.value = response.data
      isLoading.value = false
      return
    }
  }
  router.replace('/')
  return
}
onMounted(() => {
  loadData()
})
</script>

<template>
  <div class="h4 border-bottom border-5">Workplaces</div>
  <router-link
    :to="{ name: 'workadd', params: { id: authStore.workId } }"
    class="btn btn-secondary"
    title="create"
    >Add new day</router-link
  >
  <div class="col">
    <table class="table table-striped table-hover">
      <thead>
        <tr>
          <th>Date</th>
          <th>Start time</th>
          <th>End time</th>
          <th>Duration</th>
          <th>Lunch break</th>
          <th></th>
        </tr>
      </thead>
      <tbody v-if="isLoading">
        <spinner-component></spinner-component>
      </tbody>
      <tbody v-else>
        <tr v-for="item in workhours" :key="item.id">
          <td>
            {{ item.date }}
          </td>
          <td>
            {{ item.startTime }}
          </td>
          <td>
            {{ item.endTime }}
          </td>
          <td>
            {{ item.duration }}
          </td>
          <td>
            {{ item.lunchBreak }}
          </td>
          <td>
            <router-link
              :to="{ name: 'workworkhourdelete', params: { id: item.id } }"
              class="nav-link text-dark"
              title="delete"
              >Delete</router-link
            >
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

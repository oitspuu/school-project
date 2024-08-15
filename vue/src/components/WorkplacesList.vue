<script setup lang="ts">
import AccountService from '@/services/AccountService'
import { useAuthStore } from '@/stores/auth'
import SpinnerComponent from './SpinnerComponent.vue'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import type { IWork } from '@/types/IWork'
import WorkService from '@/services/WorkService'

const authStore = useAuthStore()
const router = useRouter()

let isLoading = ref<boolean>(true)
let workplaces = ref<IWork[]>([])

const loadData = async () => {
  if (authStore.jwtInfo) {
    let response = await WorkService.getWorkplaces(authStore.jwtInfo)
    if (response.data) {
      workplaces.value = response.data
      isLoading.value = false
      return
    }

    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) {
      router.replace('/login')
      return
    }

    authStore.jwtInfo = refresh.data
    response = await WorkService.getWorkplaces(refresh.data)
    if (response.data) {
      workplaces.value = response.data
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
  <div class="h4 border-bottom border-5">Workplaces</div>
  <router-link to="/work/create" class="btn btn-secondary" title="create">Create new</router-link>

  <table class="table table-striped table-hover">
    <thead>
      <tr>
        <th>Work name</th>
        <th>Lunch break duration</th>
        <th>Total work hours</th>
        <th>Start date</th>
        <th>End date</th>
        <th></th>
      </tr>
    </thead>
    <tbody v-if="isLoading">
      <spinner-component></spinner-component>
    </tbody>
    <tbody v-else>
      <tr v-for="item in workplaces" :key="item.id">
        <td>
          {{ item.workName }}
        </td>
        <td>
          {{ item.lunchBreakDuration }}
        </td>
        <td>
          {{ item.totalWorkHours }}
        </td>
        <td>
          {{ item.start }}
        </td>
        <td>
          {{ item.end }}
        </td>
        <td>
          <router-link
            :to="{ name: 'workdetails', params: { id: item.id } }"
            class="nav-link text-dark"
            title="details"
            >Details</router-link
          >
          <router-link
            :to="{ name: 'workedit', params: { id: item.id } }"
            class="nav-link text-dark"
            title="edit"
            >Edit</router-link
          >
          <router-link
            :to="{ name: 'workdelete', params: { id: item.id } }"
            class="nav-link text-dark"
            title="delete"
            >Delete</router-link
          >
          <router-link
            :to="{ name: 'workadd', params: { id: item.id } }"
            class="nav-link text-dark"
            title="add"
            >Add hours</router-link
          >
        </td>
      </tr>
    </tbody>
  </table>
</template>

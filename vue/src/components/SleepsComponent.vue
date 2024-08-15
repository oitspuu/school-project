<script setup lang="ts">
import AccountService from '@/services/AccountService'
import SleepService from '@/services/SleepService'
import { useAuthStore } from '@/stores/auth'
import type { ISleep } from '@/types/ISleep'
import SpinnerComponent from './SpinnerComponent.vue'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()

let isLoading = ref<boolean>(true)
let sleeps = ref<ISleep[]>([])

const loadData = async () => {
  if (authStore.jwtInfo) {
    let response = await SleepService.getSleepTimes(authStore.jwtInfo)
    if (response.data) {
      sleeps.value = response.data
      isLoading.value = false
      return
    }

    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) {
      router.replace('/login')
      return
    }

    authStore.jwtInfo = refresh.data
    response = await SleepService.getSleepTimes(refresh.data)
    if (response.data) {
      sleeps.value = response.data
      isLoading.value = false
      return
    }
  }
  router.replace('/login')
  return
}
onMounted(()  => {
    loadData();
})

</script>

<template>
  <div class="h4 border-bottom border-5">Sleep times</div>
  <router-link to="/sleep/create" class="btn btn-secondary" title="create">Create new</router-link>

  <table class="table table-striped table-hover">
    <thead>
      <tr>
        <th>Day</th>
        <th>Start time</th>
        <th>End time</th>
        <th>Total</th>
        <th></th>
      </tr>
    </thead>
    <tbody v-if="isLoading">
      <spinner-component></spinner-component>
    </tbody>
    <tbody v-else>
      <tr v-for="item in sleeps" :key="item.id">
        <td>
          {{ item.day }}
        </td>
        <td>
          {{ item.start }}
        </td>
        <td>
          {{ item.end }}
        </td>
        <td>
          {{ item.total }}
        </td>
        <td>
          <router-link
            :to="{ name: 'sleepedit', params: { id: item.id } }"
            class="nav-link text-dark"
            title="edit"
            >Edit</router-link
          >
          <router-link
            :to="{ name: 'sleepdelete', params: { id: item.id } }"
            class="nav-link text-dark"
            title="delete"
            >Delete</router-link
          >
        </td>
      </tr>
    </tbody>
  </table>
</template>

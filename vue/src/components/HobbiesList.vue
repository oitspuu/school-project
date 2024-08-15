<script setup lang="ts">
import AccountService from '@/services/AccountService'
import HobbyService from '@/services/HobbyService'
import { useAuthStore } from '@/stores/auth'
import type { IHobby } from '@/types/IHobby'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import SpinnerComponent from './SpinnerComponent.vue'

const authStore = useAuthStore()
const router = useRouter()

let isLoading = ref<boolean>(true)
let hobbies = ref<IHobby[]>([])

async function loadData() {
  if (authStore.jwtInfo) {
    let response = await HobbyService.getHobbies(authStore.jwtInfo)
    if (response.data) {
      hobbies.value = response.data
      isLoading.value = false
      return
    }
    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) {
      router.replace('/login')
      return
    }
    authStore.jwtInfo = refresh.data
    response = await HobbyService.getHobbies(refresh.data)
    if (response.data) {
      hobbies.value = response.data
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
  <div class="h4 border-bottom border-5">Hobbies</div>
  <router-link to="/hobby/create" class="btn btn-secondary" title="create">Create new</router-link>

  <table class="table table-striped table-hover">
    <thead>
      <tr>
        <th>Hobby name</th>
        <th>Time</th>
        <th></th>
      </tr>
    </thead>
    <tbody v-if="isLoading">
      <spinner-component></spinner-component>
    </tbody>
    <tbody v-else>
      <tr v-for="item in hobbies" :key="item.id">
        <td>
          {{ item.hobbyName }}
        </td>
        <td>
          {{ item.timeSpent }}
        </td>
        <td>
          <router-link
            :to="{ name: 'hobbyedit', params: { id: item.id } }"
            class="nav-link text-dark"
            title="edit"
            >Edit</router-link
          >
          <router-link
            :to="{ name: 'hobbydelete', params: { id: item.id } }"
            class="nav-link text-dark"
            title="delete"
            >Delete</router-link
          >
        </td>
      </tr>
    </tbody>
  </table>
</template>

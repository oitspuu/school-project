<script setup lang="ts">
import AccountService from '@/services/AccountService'
import SleepService from '@/services/SleepService'
import { useAuthStore } from '@/stores/auth'
import type { ISleep } from '@/types/ISleep'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import SpinnerComponent from '@/components/SpinnerComponent.vue'

const props = defineProps<{
  id: string
  required: true
}>()

const authStore = useAuthStore()
const router = useRouter()

let isLoading = ref<boolean>(true)
let validationError = ref<string>('')
let sleep = ref<ISleep>()

const loadData = async () => {
  if (authStore.jwtInfo) {
    let response = await SleepService.getSleep(authStore.jwtInfo, props.id)
    if (response.data) {
      sleep.value = response.data
      isLoading.value = false
      return
    }

    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) return
    authStore.jwtInfo = refresh.data
    response = response = await SleepService.getSleep(authStore.jwtInfo, props.id)
    if (response.data) {
      sleep.value = response.data
      isLoading.value = false
      return
    }
  }
}

onMounted(() => {
  loadData()
})

const deleteSleep = async () => {
  if (!authStore.jwtInfo) {
    validationError.value = "No authentication info, can't delete"
    return
  }
  let response = await SleepService.deleteSleep(authStore.jwtInfo, props.id)
  if (response.status == 204) {
    router.push('/sleep')
    return
  }
  if (response.status == 401) {
    const refresh = await AccountService.refresh(authStore.jwtInfo)
    if (!refresh.data) {
      validationError.value = "Not authenticated, can't delete"
      return
    }
    authStore.jwtInfo = refresh.data
    response = await SleepService.deleteSleep(authStore.jwtInfo, props.id)
    if (response.status == 204) {
      router.push('/sleep')
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
      <h4>Sleep</h4>
      <hr />
      <div class="text-danger">{{ validationError }}</div>
      <dl class="row">
        <dt class="col-sm-2">Sleep day</dt>
        <dd class="col-sm-10">
          {{ sleep?.day }}
        </dd>
      </dl>
      <dl class="row">
        <dt class="col-sm-2">Time</dt>
        <dd class="col-sm-10">
          {{ sleep?.total }}
        </dd>
      </dl>

      <div class="form-group">
        <input type="submit" @click="deleteSleep" value="Delete" class="btn btn-danger" />
      </div>

      <router-link to="/sleep">Back to List</router-link>
    </div>
  </div>
</template>
